using System;
using UnityEngine;

public class CameraShotSequence : ScriptableObject
{
    [Serializable]
    public class AlternativeCamShotData
    {
        public string m_name;
        public int m_altAnimIndexTauntTrigger;
        public CameraShot[] m_altCameraShots;
    }

    public CharacterType m_characterType;
    public string m_name = "Camera Sequence Name";
    
    [Space(10f)]
    [Tooltip("To differentiate between multiple taunts for the same ability.")]
    public int m_tauntNumber = 1;
    [Tooltip("The anim index specified on ability, used to determine a match.")]
    public int m_animIndex;
    [Tooltip("Anim index passed to anim network.")]
    public int m_animIndexTauntTrigger;
    public CameraTransitionType m_transitionOutType;
    public CameraShot[] m_cameraShots;
    [HideInInspector]
    public int m_uniqueTauntID;
    
    [Space(20f, order = 0)]
    [Header("-- Alternate Camera Shots, use if ability can trigger different taunt depending on situation --", order = 1)]
    public AlternativeCamShotData[] m_alternateCameraShots;

    private float m_startDelay;
    private uint m_shotIndex;
    private float m_startTime;
    private int m_altCamShotIndex = -1;

    internal ActorData Actor { get; private set; }

    public void OnValidate()
    {
        if (m_characterType <= CharacterType.None || m_characterType >= CharacterType.Last)
        {
            Debug.LogError(
                $"Taunt {m_name} has invalid character type {m_characterType} and therefore has an invalid id {m_uniqueTauntID}.");
        }
    }

    internal void Begin(ActorData actor, int altCamShotIndex)
    {
        m_startDelay = 0f;
        m_shotIndex = 0u;
        m_altCamShotIndex = altCamShotIndex;
        if (m_altCamShotIndex >= 0
            && (m_alternateCameraShots == null || m_alternateCameraShots.Length <= m_altCamShotIndex))
        {
            m_altCamShotIndex = -1;
        }

        Actor = actor;
        m_startTime = Time.time;
        if (DebugTraceEnabled())
        {
            Debug.LogWarning(GetDebugDescription());
        }

        if (m_startDelay == 0f)
        {
            GetRuntimeCameraShotsArray()[m_shotIndex].Begin(m_shotIndex, Actor);
            if (DebugTraceEnabled())
            {
                Debug.LogWarning("[Camera Shot] BEGIN " + (Time.time - m_startTime) + " with 0 delay");
            }
        }
    }

    internal bool Update()
    {
        bool isNotPlaying = false;
        CameraShot[] runtimeCameraShotsArray = GetRuntimeCameraShotsArray();
        CameraShot cameraShot = runtimeCameraShotsArray[m_shotIndex];
        if (m_startDelay > 0f)
        {
            m_startDelay -= Time.deltaTime;
            if (m_startDelay > 0f)
            {
                return true;
            }

            cameraShot.Begin(m_shotIndex, Actor);
            cameraShot.SetElapsedTime(Time.time - m_startTime);
            if (DebugTraceEnabled())
            {
                Debug.LogWarning("[Camera Shot] BEGIN " + (Time.time - m_startTime) + " seconds after begin");
            }
        }
        else if (!cameraShot.Update())
        {
            CameraShot nextCameraShot = m_shotIndex + 1 != runtimeCameraShotsArray.Length
                ? runtimeCameraShotsArray[m_shotIndex + 1]
                : null;
            cameraShot.End(Actor);
            if (DebugTraceEnabled())
            {
                Debug.LogWarning("[Camera Shot] END " + (Time.time - m_startTime) + " seconds after begin");
            }

            if (nextCameraShot != null)
            {
                m_shotIndex++;
                nextCameraShot.Begin(m_shotIndex, Actor);
                if (DebugTraceEnabled())
                {
                    Debug.LogWarning("[Camera Shot] BEGIN " + (Time.time - m_startTime) + " seconds after begin");
                }
            }
            else
            {
                isNotPlaying = true;
                CameraManager.Get().OnSpecialCameraShotBehaviorDisable(m_transitionOutType);
            }
        }

        return !isNotPlaying;
    }

    private CameraShot[] GetRuntimeCameraShotsArray()
    {
        if (m_altCamShotIndex >= 0 && m_alternateCameraShots.Length > m_altCamShotIndex)
        {
            CameraShot[] altCameraShots = m_alternateCameraShots[m_altCamShotIndex].m_altCameraShots;
            if (altCameraShots != null)
            {
                return altCameraShots;
            }
        }

        return m_cameraShots;
    }

    private bool DebugTraceEnabled()
    {
        return Application.isEditor
               && DebugParameters.Get() != null
               && DebugParameters.Get().GetParameterAsBool("TraceCameraTransitions");
    }

    public string GetDebugDescription()
    {
        string desc = "-------------------------------------------------\n";
        desc += "[Shot Sequence Name] " + m_name + "\n";
        desc += "[Index] " + m_animIndex + "\n";
        desc += "[Transition Out Type] " + m_transitionOutType + "\n";
        float num = 0f;
        for (int i = 0; i < m_cameraShots.Length; i++)
        {
            desc += "-- Shot " + (i + 1) + " --\n";
            desc += m_cameraShots[i].GetDebugDescription("    ");
            num += m_cameraShots[i].m_duration;
            desc += "(ends at time " + num + ")\n\n";
        }

        return desc;
    }
}