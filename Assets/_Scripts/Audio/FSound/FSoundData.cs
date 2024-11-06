using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
namespace Shoelace.Audio
{
    [System.Serializable, CreateAssetMenu(fileName = "New Sound Data", menuName = "FSound")]
    public class FSoundData : ScriptableObject
    {
        public EventReference selectSound;
        //public List<PARAMETER_DESCRIPTION> parameters;
        //public bool useGlobals;
        //private void OnEnable()
        //{
        //    if(!selectSound.IsNull && 
        //        (parameters == null || parameters.Count == 0))
        //        GetAllParameters();
        //}

        //public void GetAllParameters()
        //{
        //    EventInstance instance = RuntimeManager.CreateInstance(selectSound);

        //    parameters = new List<PARAMETER_DESCRIPTION>();

        //    instance.getDescription(out EventDescription description);
        //    description.getParameterDescriptionCount(out int parameterCount);
        //    for (int i = 0; i < parameterCount; i++)
        //    {
        //        description.getParameterDescriptionByIndex(i, out PARAMETER_DESCRIPTION param);
        //        parameters.Add(param);
        //    }

        //    if (useGlobals)
        //    {
        //        RuntimeManager.StudioSystem.getParameterDescriptionList(out PARAMETER_DESCRIPTION[] globalParams);

        //        for (int i = 0; i < globalParams.Length; i++)
        //        {
        //            parameters.Add(globalParams[i]);
        //        }
        //    }

        //    instance.release();
        //}
    }
}
