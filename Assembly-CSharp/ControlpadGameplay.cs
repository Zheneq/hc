using System.Collections.Generic;
using UnityEngine;

public class ControlpadGameplay : MonoBehaviour
{
    private static ControlpadGameplay s_instance;

    private ControlpadAimingConfig m_aimingConfig;
    private int m_lastCacheFrame = -1;
    private ControllerInputSnapshot m_curFrameInput;
    private ControllerInputSnapshot m_prevFrameInput;
    private bool m_usingControllerInputForTargeting;
    private List<float> m_timeStartedHoldingDownInputs;
    private Vector3 m_lastNonzeroLeftStickWorldDir;
    private Vector3 m_controllerAimDir;
    private Vector3 m_controllerAimPos;
    private Vector3 m_aimingOriginPos;

    public ControllerInputSnapshot CurFrameInput
    {
        get
        {
            CacheInputThisFrame();
            return m_curFrameInput;
        }
        private set
        {
            if (m_curFrameInput != value)
            {
                m_curFrameInput = value;
            }
        }
    }

    public ControllerInputSnapshot PrevFrameInput
    {
        get
        {
            CacheInputThisFrame();
            return m_prevFrameInput;
        }
        private set
        {
            if (m_prevFrameInput != value)
            {
                m_prevFrameInput = value;
            }
        }
    }

    public bool UsingControllerInput
    {
        get
        {
            CacheInputThisFrame();
            return m_usingControllerInputForTargeting;
        }
        private set
        {
            if (m_usingControllerInputForTargeting != value)
            {
                m_usingControllerInputForTargeting = value;
            }
        }
    }

    public Vector3 LastNonzeroLeftStickWorldDir
    {
        get => m_lastNonzeroLeftStickWorldDir;
        private set
        {
            if (m_lastNonzeroLeftStickWorldDir != value)
            {
                m_lastNonzeroLeftStickWorldDir = value;
                if (m_aimingConfig == null)
                {
                    ControllerAimDir = m_lastNonzeroLeftStickWorldDir;
                }
            }
        }
    }

    public Vector3 LastNonzeroRightStickWorldDir { get; private set; }

    public Vector3 LastNonzeroDpadWorldDir { get; private set; }

    public Vector3 ControllerAimDir
    {
        get => m_controllerAimDir;
        private set
        {
            if (m_controllerAimDir != value)
            {
                m_controllerAimDir = value;
            }
        }
    }

    public Vector3 ControllerAimPos
    {
        get => m_controllerAimPos;
        private set
        {
            if (m_controllerAimPos != value)
            {
                m_controllerAimPos = value;
            }
        }
    }

    public Vector3 ControllerAimingOriginPos
    {
        get => m_aimingOriginPos;
        private set
        {
            if (m_aimingOriginPos != value)
            {
                m_aimingOriginPos = value;
            }
        }
    }

    public bool ShowDebugGUI { get; set; }

    private void Awake()
    {
        s_instance = this;
        PrevFrameInput = new ControllerInputSnapshot();
        CurFrameInput = new ControllerInputSnapshot();
        m_timeStartedHoldingDownInputs = new List<float>((int)ControlpadInputValue.NUM);
        for (int i = 0; i < (int)ControlpadInputValue.NUM; i++)
        {
            m_timeStartedHoldingDownInputs.Add(0f);
        }

        m_aimingConfig = new ControlpadAimingConfig();
        m_aimingConfig.SetupRotation(
            ControlpadInputValue.LeftStickX,
            ControlpadInputSign.Positive,
            ControlpadInputValue.LeftStickX,
            ControlpadInputSign.Negative);
        m_aimingConfig.SetupDepthMovement(
            ControlpadInputValue.LeftStickY,
            ControlpadInputSign.Positive,
            ControlpadInputValue.LeftStickY,
            ControlpadInputSign.Negative);
        m_aimingConfig.SetupPositionMovement(
            ControlpadInputValue.LeftStickY,
            ControlpadInputSign.Positive,
            ControlpadInputValue.LeftStickY,
            ControlpadInputSign.Negative,
            ControlpadInputValue.LeftStickX,
            ControlpadInputSign.Positive,
            ControlpadInputValue.LeftStickX,
            ControlpadInputSign.Negative);
        m_aimingConfig.SetupSpeeds(
            ControlpadAimingSpeed.DefaultAnalogStickRotation(),
            ControlpadAimingSpeed.DefaultAnalogStickDepth(),
            ControlpadAimingSpeed.DefaultAnalogStickTranslation());
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

    public static ControlpadGameplay Get()
    {
        return s_instance;
    }

    private void Update()
    {
        CacheInputThisFrame();
    }

    private void CacheInputThisFrame()
    {
        if (m_lastCacheFrame >= Time.frameCount)
        {
            return;
        }

        m_lastCacheFrame = Time.frameCount;
        if (GameManager.Get() != null && !GameManager.Get().IsControlpadInputDisabled())
        {
            PrevFrameInput.CopySnapshotValuesFrom(CurFrameInput);
            CurFrameInput.CacheInputThisFrame();
            UpdateTimeStartedHoldingDownInputs();
            DetermineUserPreferredInput();
            UpdateAiming();
            UpdateLastSetDirections();
        }
        else
        {
            UsingControllerInput = false;
            CurFrameInput.ClearAllValues();
        }
    }

    public void UpdateTimeStartedHoldingDownInputs()
    {
        if (m_timeStartedHoldingDownInputs == null)
        {
            Debug.LogWarning("ControlpadGameplay-- UpdateTimeStartedHoldingDownInputs is being called, but m_timeStartedHoldingDownInputs is null.  (How did that happen...?)");
            return;
        }

        for (int i = 0; i < (int)ControlpadInputValue.NUM; i++)
        {
            ControlpadInputValue input = (ControlpadInputValue)i;
            if (Mathf.Abs(CurFrameInput.GetValueOfInput(input)) >= 0.9f)
            {
                if (m_timeStartedHoldingDownInputs[i] == 0f)
                {
                    m_timeStartedHoldingDownInputs[i] = GameTime.time;
                }
            }
            else
            {
                if (m_timeStartedHoldingDownInputs[i] != 0f)
                {
                    m_timeStartedHoldingDownInputs[i] = 0f;
                }
            }
        }
    }

    public float GetTimeSpentHoldingDownInput(ControlpadInputValue inputType)
    {
        if (inputType == ControlpadInputValue.INVALID)
        {
            return 0f;
        }

        float timeStarted = m_timeStartedHoldingDownInputs[(int)inputType];
        if (timeStarted == 0f)
        {
            return 0f;
        }

        return GameTime.time - timeStarted;
    }

    public void DetermineUserPreferredInput()
    {
        bool isControllerActive = CurFrameInput.LeftStickX != 0f
                                  || CurFrameInput.LeftStickY != 0f
                                  || CurFrameInput.RightStickX != 0f
                                  || CurFrameInput.RightStickY != 0f
                                  || CurFrameInput.DpadX != 0f
                                  || CurFrameInput.DpadY != 0f
                                  || CurFrameInput.IsUsingAnyGamepadButton();
        bool isMouseActive = CurFrameInput.MouseX != PrevFrameInput.MouseX
                             || CurFrameInput.MouseY != PrevFrameInput.MouseY
                             || CurFrameInput.IsUsingAnyMouseButton();

        if (!UsingControllerInput)
        {
            if (!isMouseActive && isControllerActive)
            {
                UsingControllerInput = true;
            }
        }
        else
        {
            if (isMouseActive && !isControllerActive)
            {
                UsingControllerInput = false;
            }
        }
    }

    public void UpdateLastSetDirections()
    {
        if (CurFrameInput.LeftStickWorldDir.sqrMagnitude > 0f)
        {
            LastNonzeroLeftStickWorldDir = CurFrameInput.LeftStickWorldDir;
        }

        if (CurFrameInput.RightStickWorldDir.sqrMagnitude > 0f)
        {
            LastNonzeroRightStickWorldDir = CurFrameInput.RightStickWorldDir;
        }

        if (CurFrameInput.DpadWorldDir.sqrMagnitude > 0f)
        {
            LastNonzeroDpadWorldDir = CurFrameInput.DpadWorldDir;
        }
    }

    public void UpdateAiming()
    {
        if (ControllerAimDir.sqrMagnitude == 0f)
        {
            Camera main = Camera.main;
            if (main != null)
            {
                Vector3 forward = main.transform.forward;
                Vector3 controllerAimDir = new Vector3(forward.x, 0f, forward.z);
                controllerAimDir.Normalize();
                ControllerAimDir = controllerAimDir;
            }
        }

        ActorData actorData = null;
        if (GameFlowData.Get() != null)
        {
            actorData = GameFlowData.Get().activeOwnedActorData;
        }

        if (actorData == null
            || Board.Get() == null
            || !UsingControllerInput)
        {
            return;
        }

        Ability selectedAbility = actorData.GetAbilityData().GetSelectedAbility();
        int targetSelectionIndex = actorData.GetActorTurnSM().GetTargetSelectionIndex();
        if (selectedAbility != null && targetSelectionIndex >= 0)
        {
            List<AbilityTarget> abilityTargets = actorData.GetActorTurnSM().GetAbilityTargets();
            switch (selectedAbility.GetControlpadTargetingParadigm(targetSelectionIndex))
            {
                case Ability.TargetingParadigm.Direction:
                    UpdateAiming_DirectionTargeter(
                        actorData,
                        selectedAbility,
                        targetSelectionIndex,
                        abilityTargets);
                    break;
                case Ability.TargetingParadigm.Position:
                    UpdateAiming_PositionTargeter();
                    break;
                case Ability.TargetingParadigm.BoardSquare:
                    UpdateAiming_PositionTargeter();
                    break;
            }
        }
        else if (actorData.GetActorTurnSM().AmDecidingMovement())
        {
            UpdateAiming_PositionTargeter();
        }
    }

    private void UpdateAiming_PositionTargeter()
    {
        ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
        if (activeOwnedActorData != null
            && activeOwnedActorData.GetActorTurnSM().IsAbilityOrPingSelectorVisible())
        {
            return;
        }

        float translationUp = CurFrameInput.GetValueOfInput(m_aimingConfig.m_translationUp);
        float timeSpentHoldingTranslationUp = GetTimeSpentHoldingDownInput(m_aimingConfig.m_translationUp);
        float translationDown = CurFrameInput.GetValueOfInput(m_aimingConfig.m_translationDown);
        float timeSpentHoldingTranslationDown = GetTimeSpentHoldingDownInput(m_aimingConfig.m_translationDown);
        float translationRight = CurFrameInput.GetValueOfInput(m_aimingConfig.m_translationRight);
        float timeSpentHoldingTranslationRight = GetTimeSpentHoldingDownInput(m_aimingConfig.m_translationRight);
        float translationLeft = CurFrameInput.GetValueOfInput(m_aimingConfig.m_translationLeft);
        float timeSpentHoldingTranslationLeft = GetTimeSpentHoldingDownInput(m_aimingConfig.m_translationLeft);

        float forwardSpeed;
        if (translationUp > 0f && m_aimingConfig.m_translationUpSign == ControlpadInputSign.Positive)
        {
            forwardSpeed = m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingTranslationUp, translationUp);
        }
        else if (translationUp < 0f && m_aimingConfig.m_translationUpSign == ControlpadInputSign.Negative)
        {
            forwardSpeed = m_aimingConfig.m_translationSpeed.GetSpeed(
                timeSpentHoldingTranslationUp,
                Mathf.Abs(translationUp));
        }
        else if (translationDown > 0f && m_aimingConfig.m_translationDownSign == ControlpadInputSign.Positive)
        {
            forwardSpeed = -m_aimingConfig.m_translationSpeed.GetSpeed(
                timeSpentHoldingTranslationDown,
                translationDown);
        }
        else if (translationDown < 0f && m_aimingConfig.m_translationDownSign == ControlpadInputSign.Negative)
        {
            forwardSpeed = -m_aimingConfig.m_translationSpeed.GetSpeed(
                timeSpentHoldingTranslationDown,
                Mathf.Abs(translationDown));
        }
        else
        {
            forwardSpeed = 0f;
        }

        float rightSpeed;
        if (translationRight > 0f && m_aimingConfig.m_translationRightSign == ControlpadInputSign.Positive)
        {
            rightSpeed = m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingTranslationRight, translationRight);
        }
        else if (translationRight < 0f && m_aimingConfig.m_translationRightSign == ControlpadInputSign.Negative)
        {
            rightSpeed = m_aimingConfig.m_translationSpeed.GetSpeed(
                timeSpentHoldingTranslationRight,
                Mathf.Abs(translationRight));
        }
        else if (translationLeft > 0f && m_aimingConfig.m_translationLeftSign == ControlpadInputSign.Positive)
        {
            rightSpeed = -m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingTranslationLeft, translationLeft);
        }
        else if (translationLeft < 0f && m_aimingConfig.m_translationLeftSign == ControlpadInputSign.Negative)
        {
            rightSpeed = -m_aimingConfig.m_translationSpeed.GetSpeed(
                timeSpentHoldingTranslationLeft,
                Mathf.Abs(translationLeft));
        }
        else
        {
            rightSpeed = 0f;
        }

        float rightDist = rightSpeed * GameTime.deltaTime;
        float forwardDist = forwardSpeed * GameTime.deltaTime;
        if (rightDist == 0f && forwardDist == 0f)
        {
            return;
        }

        Camera main = Camera.main;
        if (main == null)
        {
            return;
        }

        Vector3 cameraAimDir = main.transform.forward;
        Vector3 forward = new Vector3(cameraAimDir.x, 0f, cameraAimDir.z);
        forward.Normalize();
        Vector3 right = -Vector3.Cross(forward, Vector3.up);
        right.Normalize();
        Vector3 movement = right * rightDist + forward * forwardDist;
        Vector3 newPos = ControllerAimPos + movement;
        GameplayData gameplayData = GameplayData.Get();
        if (gameplayData != null)
        {
            newPos.x = Mathf.Clamp(newPos.x, gameplayData.m_minimumPositionX, gameplayData.m_maximumPositionX);
            newPos.z = Mathf.Clamp(newPos.z, gameplayData.m_minimumPositionZ, gameplayData.m_maximumPositionZ);
        }

        ControllerAimPos = newPos;
        if (activeOwnedActorData != null)
        {
            ControllerAimingOriginPos = activeOwnedActorData.GetFreePos();
            Vector3 controllerAimDir = ControllerAimPos - ControllerAimingOriginPos;
            controllerAimDir.y = 0f;
            controllerAimDir.Normalize();
            ControllerAimDir = controllerAimDir;
        }

        CameraManager.Get().SetTargetPosition(newPos, 0.5f);
    }

    private void UpdateAiming_DirectionTargeter(
        ActorData clientActor,
        Ability abilityBeingTargeted,
        int currentIndex,
        List<AbilityTarget> targetsSoFar)
    {
        float rotateClockwise = CurFrameInput.GetValueOfInput(m_aimingConfig.m_rotateClockwise);
        float timeSpentHoldingRotateClockwise = GetTimeSpentHoldingDownInput(m_aimingConfig.m_rotateClockwise);
        float rotateAntiClockwise = CurFrameInput.GetValueOfInput(m_aimingConfig.m_rotateAntiClockwise);
        float timeSpentHoldingRotateAntiClockwise = GetTimeSpentHoldingDownInput(m_aimingConfig.m_rotateAntiClockwise);
        float depthForward = CurFrameInput.GetValueOfInput(m_aimingConfig.m_depthForward);
        float timeSpentHoldingDepthForward = GetTimeSpentHoldingDownInput(m_aimingConfig.m_depthForward);
        float depthBackward = CurFrameInput.GetValueOfInput(m_aimingConfig.m_depthBackward);
        float timeSpentHoldingDepthBackward = GetTimeSpentHoldingDownInput(m_aimingConfig.m_depthBackward);

        float clockwiseSpeed;
        if (rotateClockwise > 0f && m_aimingConfig.m_rotateClockwiseSign == ControlpadInputSign.Positive)
        {
            clockwiseSpeed = m_aimingConfig.m_rotationSpeed.GetSpeed(timeSpentHoldingRotateClockwise, rotateClockwise);
        }
        else if (rotateClockwise < 0f && m_aimingConfig.m_rotateClockwiseSign == ControlpadInputSign.Negative)
        {
            clockwiseSpeed = m_aimingConfig.m_rotationSpeed.GetSpeed(
                timeSpentHoldingRotateClockwise,
                Mathf.Abs(rotateClockwise));
        }
        else if (rotateAntiClockwise > 0f && m_aimingConfig.m_rotateAntiClockwiseSign == ControlpadInputSign.Positive)
        {
            clockwiseSpeed = -m_aimingConfig.m_rotationSpeed.GetSpeed(
                timeSpentHoldingRotateAntiClockwise,
                rotateAntiClockwise);
        }
        else if (rotateAntiClockwise < 0f && m_aimingConfig.m_rotateAntiClockwiseSign == ControlpadInputSign.Negative)
        {
            clockwiseSpeed = -m_aimingConfig.m_rotationSpeed.GetSpeed(
                timeSpentHoldingRotateAntiClockwise,
                Mathf.Abs(rotateAntiClockwise));
        }
        else
        {
            clockwiseSpeed = 0f;
        }

        float depthSpeed;
        if (depthForward > 0f && m_aimingConfig.m_depthForwardSign == ControlpadInputSign.Positive)
        {
            depthSpeed = m_aimingConfig.m_depthSpeed.GetSpeed(timeSpentHoldingDepthForward, depthForward);
        }
        else if (depthForward < 0f && m_aimingConfig.m_depthForwardSign == ControlpadInputSign.Negative)
        {
            depthSpeed = m_aimingConfig.m_depthSpeed.GetSpeed(timeSpentHoldingDepthForward, Mathf.Abs(depthForward));
        }
        else if (depthBackward > 0f && m_aimingConfig.m_depthBackwardSign == ControlpadInputSign.Positive)
        {
            depthSpeed = -m_aimingConfig.m_depthSpeed.GetSpeed(timeSpentHoldingDepthBackward, depthBackward);
        }
        else if (depthBackward < 0f && m_aimingConfig.m_depthBackwardSign == ControlpadInputSign.Negative)
        {
            depthSpeed = -m_aimingConfig.m_depthSpeed.GetSpeed(timeSpentHoldingDepthBackward, Mathf.Abs(depthBackward));
        }
        else
        {
            depthSpeed = 0f;
        }

        float deltaRotation = -1f * clockwiseSpeed * GameTime.deltaTime;
        float deltaDepth = depthSpeed * GameTime.deltaTime;
        if (abilityBeingTargeted != null && abilityBeingTargeted.HasAimingOriginOverride(
                clientActor,
                currentIndex,
                targetsSoFar,
                out Vector3 overridePos))
        {
            ControllerAimingOriginPos = overridePos;
        }
        else
        {
            ControllerAimingOriginPos = clientActor.GetFreePos();
        }

        float currentRotation = VectorUtils.HorizontalAngle_Deg(ControllerAimDir);
        float currentDepth = (ControllerAimingOriginPos - Board.Get().PlayerFreePos).magnitude;
        float newRotation = currentRotation + deltaRotation;
        float newDepth = currentDepth + deltaDepth;
        newDepth = Mathf.Clamp(newDepth, 0.01f, 50f);

        if (abilityBeingTargeted != null
            && abilityBeingTargeted.HasRestrictedFreeAimDegrees(
                clientActor,
                currentIndex,
                targetsSoFar,
                out float min,
                out float max))
        {
            newRotation = VectorUtils.ClampAngle_Deg(newRotation, min, max);
        }

        if (abilityBeingTargeted != null
            && abilityBeingTargeted.HasRestrictedFreePosDistance(
                clientActor,
                currentIndex,
                targetsSoFar,
                out float min2,
                out float max2))
        {
            newDepth = Mathf.Clamp(newDepth, min2, max2);
        }

        Vector3 controllerAimDir = VectorUtils.AngleDegreesToVector(newRotation);
        Vector3 controllerAimPos = ControllerAimingOriginPos + controllerAimDir * newDepth;
        ControllerAimDir = controllerAimDir;
        ControllerAimPos = controllerAimPos;
    }

    public void OnCameraCenteredOnActor(ActorData cameraActor)
    {
        ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
        if (cameraActor != activeOwnedActorData)
        {
            ControllerAimDir = (cameraActor.GetFreePos() - activeOwnedActorData.GetFreePos()).normalized;
        }

        if (cameraActor != null)
        {
            ControllerAimPos = cameraActor.GetFreePos();
        }
    }

    public void OnTurnTick()
    {
        if (GameFlowData.Get() == null || GameFlowData.Get().activeOwnedActorData == null)
        {
            return;
        }

        ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
        if (!activeOwnedActorData.IsDead()
            && activeOwnedActorData.CurrentBoardSquare != null
            && activeOwnedActorData.IsActorVisibleToClient())
        {
            ControllerAimPos = activeOwnedActorData.GetFreePos();
        }
    }

    private void OnGUI()
    {
        if (!ShowDebugGUI)
        {
            return;
        }

        Rect screenRect = new Rect(60f, 5f, 800f, 500f);
        GUILayout.Window(632146, screenRect, DrawDebugGUIWindow, "Gamepad Debug Window");
    }

    private void DrawDebugGUIWindow(int windowId)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Hide Me", GetDebugGUIButtonStyle(), GUILayout.Width(80f)))
        {
            ShowDebugGUI = false;
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Label($"Aim dir: ({ControllerAimDir.x}, {ControllerAimDir.z})");
        GUILayout.Label($">>In Degrees: {VectorUtils.HorizontalAngle_Deg(ControllerAimDir)}");
        GUILayout.Label($"Aim pos: ({ControllerAimPos.x}, {ControllerAimPos.y}, {ControllerAimPos.z})");
        GUILayout.Label(
            $"Origin pos: ({ControllerAimingOriginPos.x}, {ControllerAimingOriginPos.y}, {ControllerAimingOriginPos.z})");
        GUILayout.Label($"Left stick: ({CurFrameInput.LeftStickX}, {CurFrameInput.LeftStickY})");
        GUILayout.Label($"Right stick: ({CurFrameInput.RightStickX}, {CurFrameInput.RightStickY})");
        GUILayout.Label($"D-pad: ({CurFrameInput.DpadX}, {CurFrameInput.DpadY})");
        GUILayout.Label($"A: {CurFrameInput.Button_A.GetDebugString()}");
        GUILayout.Label($"B: {CurFrameInput.Button_B.GetDebugString()}");
        GUILayout.Label($"X: {CurFrameInput.Button_X.GetDebugString()}");
        GUILayout.Label($"Y: {CurFrameInput.Button_Y.GetDebugString()}");
        GUILayout.Label($"Start: {CurFrameInput.Button_start.GetDebugString()}");
        GUILayout.Label($"Back: {CurFrameInput.Button_back.GetDebugString()}");
        GUILayout.Label($"Left shoulder: {CurFrameInput.Button_leftShoulder.GetDebugString()}");
        GUILayout.Label($"Right shoulder: {CurFrameInput.Button_rightShoulder.GetDebugString()}");
        GUILayout.Label($"Left trigger: {CurFrameInput.LeftTrigger}");
        GUILayout.Label($"Right trigger: {CurFrameInput.RightTrigger}");
        GUILayout.Label($"Left stick in: {CurFrameInput.Button_leftStickIn.GetDebugString()}");
        GUILayout.Label($"Right stick in: {CurFrameInput.Button_rightStickIn.GetDebugString()}");
        GUILayout.EndHorizontal();
    }

    private GUIStyle GetDebugGUIButtonStyle()
    {
        return new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleLeft,
            richText = true,
            fontSize = 15
        };
    }

    public bool GetButton(ControlpadInputValue controllerCode)
    {
        return CurFrameInput.GetValueOfInput(controllerCode) == 1f;
    }

    public float GetAxisValue(ControlpadInputValue controllerCode)
    {
        return CurFrameInput.GetValueOfInput(controllerCode);
    }

    public bool GetButtonDown(ControlpadInputValue controllerCode)
    {
        switch (controllerCode)
        {
            case ControlpadInputValue.Button_A:
                return CurFrameInput.Button_A.Down;
            case ControlpadInputValue.Button_B:
                return CurFrameInput.Button_B.Down;
            case ControlpadInputValue.Button_X:
                return CurFrameInput.Button_X.Down;
            case ControlpadInputValue.Button_Y:
                return CurFrameInput.Button_Y.Down;
            case ControlpadInputValue.Button_leftShoulder:
                return CurFrameInput.Button_leftShoulder.Down;
            case ControlpadInputValue.Button_rightShoulder:
                return CurFrameInput.Button_rightShoulder.Down;
            case ControlpadInputValue.Button_start:
                return CurFrameInput.Button_start.Down;
            case ControlpadInputValue.Button_back:
                return CurFrameInput.Button_back.Down;
            case ControlpadInputValue.Button_leftStickIn:
                return CurFrameInput.Button_leftStickIn.Down;
            case ControlpadInputValue.Button_rightStickIn:
                return CurFrameInput.Button_rightStickIn.Down;
            default:
                return false;
        }
    }

    public bool GetButtonUp(ControlpadInputValue controllerCode)
    {
        switch (controllerCode)
        {
            case ControlpadInputValue.Button_A:
                return CurFrameInput.Button_A.Up;
            case ControlpadInputValue.Button_B:
                return CurFrameInput.Button_B.Up;
            case ControlpadInputValue.Button_X:
                return CurFrameInput.Button_X.Up;
            case ControlpadInputValue.Button_Y:
                return CurFrameInput.Button_Y.Up;
            case ControlpadInputValue.Button_leftShoulder:
                return CurFrameInput.Button_leftShoulder.Up;
            case ControlpadInputValue.Button_rightShoulder:
                return CurFrameInput.Button_rightShoulder.Up;
            case ControlpadInputValue.Button_start:
                return CurFrameInput.Button_start.Up;
            case ControlpadInputValue.Button_back:
                return CurFrameInput.Button_back.Up;
            case ControlpadInputValue.Button_leftStickIn:
                return CurFrameInput.Button_leftStickIn.Up;
            case ControlpadInputValue.Button_rightStickIn:
                return CurFrameInput.Button_rightStickIn.Up;
            default:
                return false;
        }
    }
}