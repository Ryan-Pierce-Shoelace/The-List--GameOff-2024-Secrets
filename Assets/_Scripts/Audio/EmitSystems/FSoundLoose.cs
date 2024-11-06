using FMODUnity;
using System;
using UnityEngine;
namespace Shoelace.Audio
{
    [Serializable]
    public class FSoundLoose : FSound
    {
        public void PlayOneShot(Vector3 pos = default)
        {
            if(data != null)
                FMODUnity.RuntimeManager.PlayOneShot(data.selectSound, pos);
        }
        public void Play(Vector3 pos = default)
        {
            if(!instance.isValid())
                instance = FMODUnity.RuntimeManager.CreateInstance(data.selectSound);
            if (pos != default)
                instance.set3DAttributes(RuntimeUtils.To3DAttributes(pos));

            instance.start();
        }

        public void Stop()
        {
            if (instance.isValid())
                instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
