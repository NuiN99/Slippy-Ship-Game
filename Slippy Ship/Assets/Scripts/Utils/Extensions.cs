using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public static class Extensions
{
    public static Vector3 AverageNormal(this Collision collision)
    {
        Vector3 avgNormal = Vector3.zero;
        foreach (var contact in collision.contacts)
        {
            avgNormal += contact.normal;
        }

        avgNormal /= collision.contactCount;
        return avgNormal.normalized;
    }
    
    public static void SetVolumeFromValue(this AudioMixer mixer, string volumeParam, float value, float minVolume = -80, float maxVolume = 20)
    {
        mixer.SetFloat(volumeParam, Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * (maxVolume - minVolume) / 4f + maxVolume);
    }

    public static void TryPlay(this ParticleSystem particleSystem)
    {
        if (!particleSystem.isPlaying) particleSystem.Play();
    }
    
    public static T FindInChildren<T>(this Transform transform, string name) where T : Object
    {
        T result = null;

        if (typeof(T) == typeof(GameObject))
        {
            foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == name)
                {
                    result = child.gameObject as T;
                    break;
                }
            }
        }
        else
        {
            foreach (var component in transform.GetComponentsInChildren<Component>(true))
            {
                if (component.name == name && component is T matched)
                {
                    result = matched;
                    break;
                }
            }
        }

#if UNITY_EDITOR
        if (result == null)
        {
            Debug.LogError($"Failed to find {name} in {transform.name}", transform);
        }
#endif

        return result;
    }
}
