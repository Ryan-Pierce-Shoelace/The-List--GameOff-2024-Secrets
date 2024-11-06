using System;
using UnityEngine;
namespace Shoelace.Audio
{
    [Serializable]
    public class FSoundAttached : FSound
    {
        Transform transform;
        Rigidbody2D rb;
        public void InitAttached(FSoundData sound, Transform transform, Rigidbody2D rb)
        {
            this.transform = transform;
            this.rb = rb;
            data = sound;
            //data.GetAllParameters();
            instance = FMODUnity.RuntimeManager.CreateInstance(data.selectSound);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, transform, rb);
        }

        public void Start(bool isOneShot = false)
        {
            instance.setVolume(1.0f);
            if (isOneShot)
            {
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, transform, rb);
            }
            instance.start();
            
        }
        public void Stop(bool fadeOut)
        {
            instance.stop(fadeOut? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}
