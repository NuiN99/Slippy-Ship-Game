using System;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Debug = UnityEngine.Debug;

[Serializable]
public class FMODSoundPlayer
{
    [SerializeField] EventReference eventReference;

    static readonly EVENT_CALLBACK EventStoppedCallbackDelegate = EventStoppedCallback;

    public void PlayEventNoParams()
    {
        PlayEvent();
    }
    
    public EventInstance PlayEvent(string parameterName = null, float? parameterValue = null)
    {
        try
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);

            if (parameterName != null && parameterValue != null)
            {
                eventInstance.setParameterByName(parameterName, parameterValue.Value);
            }

            eventInstance.setCallback(EventStoppedCallbackDelegate, EVENT_CALLBACK_TYPE.STOPPED);

            eventInstance.start();

            return eventInstance;
        }
        catch (Exception err)
        {
            Debug.LogError(err.Message);
        }

        return new EventInstance();
    }
    
    public EventInstance PlayEventAttached(Transform owner, string parameterName = null, float? parameterValue = null)
    {
        try
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);

            if (parameterValue != null)
            {
                eventInstance.setParameterByName(parameterName, parameterValue.Value);
            }
        
            eventInstance.setCallback(EventStoppedCallbackDelegate, EVENT_CALLBACK_TYPE.SOUND_STOPPED);
        
            RuntimeManager.AttachInstanceToGameObject(eventInstance, owner.gameObject);
        
            eventInstance.start();
            
            return eventInstance;
        }
        catch (Exception err)
        {
            Debug.LogError(err.Message);
        }

        return new EventInstance();
    }
    
    public void PlayAtPosition(Vector3 position)
    {
        try
        {
            RuntimeManager.PlayOneShot(eventReference, position);
        }
        catch (Exception err)
        {
            Debug.LogError(err.Message);
            throw;
        }
    }

    public void StopEvent(EventInstance eventInstance)
    {
        eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        eventInstance.release();
    }

    // for some reason is required for sounds to play when building with il2cpp
    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    static RESULT EventStoppedCallback(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameters)
    {
        if (type != EVENT_CALLBACK_TYPE.SOUND_STOPPED) return FMOD.RESULT.OK;

        EventInstance instance = new EventInstance(instancePtr);
        instance.release();

        return FMOD.RESULT.OK;
    }
}