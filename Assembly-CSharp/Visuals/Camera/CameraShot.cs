using System;
using System.Collections.Generic;
using CameraManagerInternal;
using UnityEngine;

[Serializable]
public class CameraShot
{
    [Serializable]
    public class AnimParamSetAction
    {
        public string m_paramName;
        public int m_paramValue;
        public bool m_isTrigger;
    }

    public class CharacterToAnimParamSetActions
    {
        public ActorData m_actor;
        public List<AnimParamSetAction> m_animSetActions;

        public CharacterToAnimParamSetActions(ActorData actor, List<AnimParamSetAction> animSetActions)
        {
            m_actor = actor;
            m_animSetActions = animSetActions;
        }
    }

    public string m_name = "Camera Shot Name";
    [Tooltip("Set to 0 to wait until end of animation or associated event.")]
    public float m_duration = 1f;
    public float m_fieldOfView;
    [Tooltip("Only applicable if camera type is Animated")]
    public bool m_useAnimatedFov;
    public CameraType m_type;
    public CameraTransitionType m_transitionInType;
    [Header("-- Anim Param Setters On Beginning of shot --")]
    public List<AnimParamSetAction> m_animParamToSetOnBegin = new List<AnimParamSetAction>();
    [Header("-- (On End of Turn) Anim Setters --")]
    public List<AnimParamSetAction> m_animParamToSetOnEndOfTurn = new List<AnimParamSetAction>();

    private float m_time;
    private GameObject m_cameraPoseObject;

    internal void Begin(uint shotIndex, ActorData actor)
    {
        m_time = 0f;
        if (m_type != CameraType.Isometric)
        {
            CameraManager.Get().OnSpecialCameraShotBehaviorEnable(m_transitionInType);
        }

        FadeObjectsCameraComponent fadeComponent = Camera.main.GetComponent<FadeObjectsCameraComponent>();
        if (fadeComponent != null)
        {
            fadeComponent.ClearDesiredVisibleObjects();
            fadeComponent.AddDesiredVisibleObject(actor.gameObject);
        }

        SetAnimParamsForActor(actor, m_animParamToSetOnBegin);
        if (m_animParamToSetOnEndOfTurn.Count > 0)
        {
            CameraManager.Get().AddAnimParamSetActions(
                new CharacterToAnimParamSetActions(actor, m_animParamToSetOnEndOfTurn));
        }

        MixSnapshots mixerSnapshotManager = AudioManager.GetMixerSnapshotManager();
        switch (m_type)
        {
            case CameraType.Animated:
            {
                AnimatedCamera animatedCamera = Camera.main.GetComponent<AnimatedCamera>();
                if (animatedCamera == null)
                {
                    Camera.main.gameObject.AddComponent<AnimatedCamera>();
                    animatedCamera = Camera.main.GetComponent<AnimatedCamera>();
                    Log.Warning("Missing AnimatedCamera component on main camera. Generating dynamically for now.");
                }

                m_cameraPoseObject = actor.gameObject.FindInChildren("camera" + GetCameraIndex(shotIndex));
                animatedCamera.SetAnimator(m_cameraPoseObject);
                AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_action_cam");
                AudioManager.PostEvent("Set_state_action_cam");
                if (mixerSnapshotManager != null)
                {
                    mixerSnapshotManager.SetMix_TauntCam();
                }

                actor.m_hideNameplate = true;
                animatedCamera.enabled = true;
                if (CameraManager.Get().TauntBackgroundCamera != null)
                {
                    CameraManager.Get().TauntBackgroundCamera.gameObject.SetActive(true);
                    CameraManager.Get().TauntBackgroundCamera.SetAnimatedCameraTargetObj(m_cameraPoseObject);
                    CameraManager.Get().TauntBackgroundCamera.OnCamShotStart(m_type);
                }

                return;
            }
            case CameraType.Fixed_CasterAndTargets:
            {
                Fixed_CasterAndTargetsCamera fixedCamera = Camera.main.GetComponent<Fixed_CasterAndTargetsCamera>();
                if (fixedCamera == null)
                {
                    Camera.main.gameObject.AddComponent<Fixed_CasterAndTargetsCamera>();
                    fixedCamera = Camera.main.GetComponent<Fixed_CasterAndTargetsCamera>();
                    Log.Warning(
                        "Missing Fixed_CasterAndTargetsCamera component on main camera. Generating dynamically for now.");
                }

                m_cameraPoseObject = actor.gameObject.FindInChildren("camera" + GetCameraIndex(shotIndex));
                fixedCamera.SetAnimator(m_cameraPoseObject);
                if (fadeComponent != null)
                {
                    foreach (ActorData targetActor in SequenceManager.Get().FindSequenceTargets(actor))
                    {
                        if (targetActor != null)
                        {
                            fadeComponent.AddDesiredVisibleObject(targetActor.gameObject);
                        }
                    }
                }

                AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_action_cam");
                if (mixerSnapshotManager != null)
                {
                    mixerSnapshotManager.SetMix_TauntCam();
                }

                fixedCamera.enabled = true;
                actor.m_hideNameplate = true;
                if (CameraManager.Get().TauntBackgroundCamera != null)
                {
                    CameraManager.Get().TauntBackgroundCamera.gameObject.SetActive(true);
                    CameraManager.Get().TauntBackgroundCamera.SetFixedCasterAndTargetObj(m_cameraPoseObject);
                    CameraManager.Get().TauntBackgroundCamera.OnCamShotStart(m_type);
                }

                return;
            }
            case CameraType.Isometric:
                return;
        }
    }

    internal void End(ActorData actor)
    {
        FadeObjectsCameraComponent fadeComponent = Camera.main.GetComponent<FadeObjectsCameraComponent>();
        if (fadeComponent != null)
        {
            fadeComponent.ResetDesiredVisibleObjects();
        }

        actor.m_hideNameplate = false;
        CameraType type = m_type;
        switch (type)
        {
            case CameraType.Animated:
            {
                Camera.main.GetComponent<AnimatedCamera>().enabled = false;
                if (CameraManager.Get().TauntBackgroundCamera != null)
                {
                    CameraManager.Get().TauntBackgroundCamera.OnCamShotStop();
                    CameraManager.Get().TauntBackgroundCamera.gameObject.SetActive(false);
                }

                break;
            }
            case CameraType.Fixed_CasterAndTargets:
            {
                Camera.main.GetComponent<Fixed_CasterAndTargetsCamera>().enabled = false;
                if (CameraManager.Get().TauntBackgroundCamera != null)
                {
                    CameraManager.Get().TauntBackgroundCamera.OnCamShotStop();
                    CameraManager.Get().TauntBackgroundCamera.gameObject.SetActive(false);
                }

                break;
            }
            case CameraType.Isometric:
                break;
        }

        AudioManager.PostEvent("sw_game_state", AudioManager.EventAction.SetSwitch, "game_state_resolve");
        MixSnapshots mixerSnapshotManager = AudioManager.GetMixerSnapshotManager();
        if (mixerSnapshotManager != null)
        {
            mixerSnapshotManager.SetMix_ResolveCam();
        }
    }

    private float GetFieldOfView()
    {
        if (m_type == CameraType.Animated
            && m_useAnimatedFov
            && m_cameraPoseObject != null)
        {
            Vector3 localScale = m_cameraPoseObject.transform.localScale;
            if (localScale.z > 1f)
            {
                return m_cameraPoseObject.transform.localScale.z;
            }
        }

        if (m_fieldOfView <= 0f)
        {
            return CameraManager.Get().DefaultFOV;
        }

        return m_fieldOfView;
    }

    internal static void SetAnimParamsForActor(ActorData actor, List<AnimParamSetAction> paramSetActions)
    {
        if (actor == null
            || actor.GetModelAnimator() == null
            || paramSetActions == null)
        {
            return;
        }

        foreach (AnimParamSetAction paramSetAction in paramSetActions)
        {
            if (paramSetAction.m_paramName.Length <= 0)
            {
                continue;
            }

            if (paramSetAction.m_isTrigger)
            {
                if (paramSetAction.m_paramValue != 0)
                {
                    actor.GetModelAnimator().SetTrigger(paramSetAction.m_paramName);
                }
                else
                {
                    actor.GetModelAnimator().ResetTrigger(paramSetAction.m_paramName);
                }
            }
            else
            {
                actor.GetModelAnimator().SetInteger(paramSetAction.m_paramName, paramSetAction.m_paramValue);
            }
        }
    }

    internal bool Update()
    {
        Camera.main.fieldOfView = GetFieldOfView();
        m_time += Time.deltaTime;
        return m_time < m_duration || m_duration <= 0f;
    }

    public void SetElapsedTime(float time)
    {
        m_time = time;
    }

    private uint GetCameraIndex(uint shotIndex)
    {
        return shotIndex % 2u;
    }

    public string GetDebugDescription(string linePrefix)
    {
        string desc = string.Empty;
        desc += linePrefix + "[Shot Name] " + m_name + "\n";
        desc += linePrefix + "[Duration] " + m_duration + "\n";
        desc += linePrefix + "[Type] " + m_type + "\n";
        return desc + linePrefix + "[Transition In Type] " + m_transitionInType + "\n";
    }
}