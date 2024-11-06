using System;
namespace Shoelace.Audio
{
    [Serializable]
    public class FSound
    {
        public FSoundData data;
        public FMOD.Studio.EventInstance instance;
        public void Release() => instance.release();
        //public void SetParameter(int index, float value, bool ignoreSeekSpeed = false) => instance.setParameterByID(data.parameters[index].id, value, ignoreSeekSpeed);
    }
}
