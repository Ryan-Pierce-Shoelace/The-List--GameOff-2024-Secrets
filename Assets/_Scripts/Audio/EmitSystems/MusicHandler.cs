using System.Collections;
using UnityEngine;
namespace Shoelace.Audio
{
    public class MusicHandler : MonoBehaviour
    {
        private FMOD.Studio.EventInstance instance;

        [SerializeField] private bool playOnStart;
        [SerializeField, Range(0f,1f)] private float volume;
        [SerializeField] private FSoundData startingSong;
        private FSoundData currentSong;
        private FSoundData queuedSong;

        bool fadingSong;
        [SerializeField] private float fadeOutTime = 2f;

        private void Start()
        {
            if (playOnStart)
                SetNewMusic(startingSong);
        }
        private void LateUpdate()
        {
            if(currentSong != null)
                instance.setVolume(volume);
        }

        //UNITY EVENT
        public void SetNewMusic(FSoundData song)
        {
            
            if (queuedSong != null)
                return; // Dont allow queuing another song while still transitioning.

            if (currentSong == null)
            {
                print("Starting new song : " + song.ToString());
                //No song currently play. Start new song.
                instance = FMODUnity.RuntimeManager.CreateInstance(song.selectSound);
                instance.start();
                instance.setVolume(volume);

                currentSong = song;

                return;
            }

            //Song already exists

            //Check if its the same song
            if (currentSong == song)
            {
                instance.setVolume(0f);
                instance.release();
                fadingSong = false;

                currentSong = null;

                SetNewMusic(song);
                return;
            }

            instance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state);
            switch (state)
            {
                case FMOD.Studio.PLAYBACK_STATE.STOPPED:

                    instance.release();
                    instance = FMODUnity.RuntimeManager.CreateInstance(song.selectSound);
                    instance.start();

                    currentSong = song;
                    break;
                default:

                    queuedSong = song;

                    if (!fadingSong)
                        StartCoroutine(FadeOutCurrentSong());

                    StopCoroutine(WaitToPlayQueued());
                    StartCoroutine(WaitToPlayQueued());
                    break;
            }
        }
        //UNITY EVENT
        public void StopMusic()
        {
            if (fadingSong)
                return; // Already Stopping music

            if (queuedSong != null)
                Debug.Log("Song is Queued while stopping music. Make sure this isnt an issue.");

            if (instance.isValid())
                StartCoroutine(FadeOutCurrentSong());
        }

        public void ResetMusicToStartSong()
        {
            SetNewMusic(startingSong);
        }
        private IEnumerator FadeOutCurrentSong()
        {
            float t = 0f;
            fadingSong = true;
            while (t < 1)
            {

                instance.setVolume(Mathf.Lerp(1f, 0f, t));
                t += Time.deltaTime / fadeOutTime;
                yield return new WaitForEndOfFrame();
            }

            instance.setVolume(0f);
            instance.release();
            fadingSong = false;

            currentSong = null;
        }

        private IEnumerator WaitToPlayQueued()
        {
            float timeBreak = 0f;
            while (currentSong != null && timeBreak < 30)
            {
                yield return new WaitForEndOfFrame();
                timeBreak += Time.deltaTime;
            }

            instance = FMODUnity.RuntimeManager.CreateInstance(queuedSong.selectSound);
            instance.start();
            currentSong = queuedSong;
            queuedSong = null;
        }
    }
}
