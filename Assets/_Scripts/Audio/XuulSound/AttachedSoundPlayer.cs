using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;
// ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable

namespace Shoelace.Audio.XuulSound
{
    public class AttachedSoundPlayer : ISoundPlayer
    {
        private readonly EventInstance instance;
        private readonly Transform attachedTransform;
        private readonly SoundConfig config;
        private bool isValid = true;

        public AttachedSoundPlayer(SoundConfig config, Transform transform)
        {
            this.config = config;
            attachedTransform = transform;
            
            instance = RuntimeManager.CreateInstance(config.EventRef);
            RuntimeManager.AttachInstanceToGameObject(instance, transform);
            SetVolume(config.DefaultVolume);
        }

        public void Play()
        {
            if (!isValid) return;
            instance.start();
        }

        public void Stop(bool fadeOut = true)
        {
            if (!isValid) return;
            instance.stop(fadeOut ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE);
        }

        public void SetVolume(float volume)
        {
            if (!isValid) return;
            instance.setVolume(volume);
        }

        public void SetParameter(string name, float value)
        {
            if (!isValid) return;
            instance.setParameterByName(name, value);
        }

        public void Dispose()
        {
            if (!isValid) return;
            
            Stop(false);
            instance.release();
            isValid = false;
        }
    }
}
