using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float m_mouseWheelZoomSpeed = 10f;
    [Tooltip("Speed the camera moves in response to directional input from the keyboard")]
    public float m_keyboardMoveSpeed = 0.4f;
    [Tooltip("Speed the camera moves in response to mouse click and drag")]
    public float m_mouseMoveSpeed = 0.45f;
    [Tooltip("Speed the camera rotates in response to free rotation control with the mouse")]
    public float m_mouseRotationSpeed = 6f;
    [Tooltip("Speed the camera moves when hovering in a fringe of the screen.")]
    public float m_mouseMoveFringeSpeed = 0.2f;
    public float m_mouseMoveFringeEaseInTime = 0.4f;
    public float m_mouseMoveFringeEaseOutTime = 0.2f;
    public AnimationCurve m_mouseMoveFringeEaseIn = new AnimationCurve();
    public AnimationCurve m_mouseMoveFringeEaseOut = new AnimationCurve();
    [Tooltip("Percentage of screen where hovering the mouse should move the camera.")]
    public float m_mouseMoveFringeTop = 0.025f;
    [Tooltip("Percentage of screen where hovering the mouse should move the camera.")]
    public float m_mouseMoveFringeBottom = 0.025f;
    [Tooltip("Percentage of screen where hovering the mouse should move the camera.")]
    public float m_mouseMoveFringeLeft = 0.025f;
    [Tooltip("Percentage of screen where hovering the mouse should move the camera.")]
    public float m_mouseMoveFringeRight = 0.025f;
    public bool m_mouseMoveFringeInEditor;
    public float m_keyboardRotationIncrement = 45f;
    public float m_keyboardRotationDuration = 0.5f;
    public float m_keyboardPitchIncrement = 20f;
    public float m_keyboardRotationRepeatDelay = 0.15f;
    public float m_mousePitchSpeed = 6f;
    public float m_maxPitch = 90f;
    public float m_minPitch = 10f;
    internal Vector3 m_desiredRotationEulerAngles;
    [HideInInspector]
    public bool CameraRotateClockwiseToggled;
    [HideInInspector]
    public bool CameraRotateCounterClockwiseToggled;

    private bool m_enabled = true;
    private EasedVector3AnimationCurve m_mouseMoveFringeSpeedEased = new EasedVector3AnimationCurve(Vector3.zero);
    private float m_rotationChangedTime = -100f;
    private float m_keyboardRotationTime;
    private Quaternion m_prevRotation;

    private const float FPS_TUNED_AT = 80f;
    private static CameraControls s_instance;

    private HighlightUtils.ScrollCursorDirection m_lastScrollCursorDirection =
        HighlightUtils.ScrollCursorDirection.Undefined;
    private float m_timeLastScrollCursorStarted;
    public float m_secondsUntilScrollCursorStop = 2f;

    internal bool Enabled
    {
        get =>
            m_enabled
            && AccountPreferences.DoesApplicationHaveFocus()
            && HUD_UI.Get() != null
            && (HUD_UI.Get().m_textConsole == null || !HUD_UI.Get().m_textConsole.IsTextInputFocused())
            && (UIDialogPopupManager.Get() == null || !UIDialogPopupManager.Get().IsDialogBoxOpen());
        set => m_enabled = value;
    }

    internal static CameraControls Get()
    {
        return s_instance;
    }

    private void Awake()
    {
        s_instance = this;
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

    internal bool IsTiltUserControlled()
    {
        return DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("CameraTiltControl");
    }

    internal bool IsMouseDragRotationRequested()
    {
        return IsTiltUserControlled() && Input.GetKey(KeyCode.G);
    }

    internal bool IsMouseDragMoveRequested()
    {
        return !IsMouseDragRotationRequested() && Input.GetMouseButton(2);
    }

    internal void CalcDesiredTransform(
        Transform currentTransform,
        out Vector3 positionDelta,
        out Quaternion rotationThisFrame,
        out float zoomDelta)
    {
        Vector3 desiredEulerAngles = CalcDesiredEulerAngles(out bool toggleInput);
        zoomDelta = CalcZoomVelocity() * Mathf.Min(Time.unscaledDeltaTime, 0.06f);
        if (zoomDelta == 0f && !Mathf.Approximately(desiredEulerAngles.x, 0f))
        {
            m_rotationChangedTime = Time.time;
            m_prevRotation = currentTransform.rotation;
            m_desiredRotationEulerAngles.x += desiredEulerAngles.x;
            if (toggleInput)
            {
                float angle = Mathf.Abs(desiredEulerAngles.x);
                m_desiredRotationEulerAngles.x = (int)(m_desiredRotationEulerAngles.x / angle) * angle;
            }

            m_desiredRotationEulerAngles.x = Mathf.Clamp(m_desiredRotationEulerAngles.x, m_minPitch, m_maxPitch);
        }

        if (!Mathf.Approximately(desiredEulerAngles.y, 0f)
            && (!toggleInput || Time.time - m_keyboardRotationTime >= m_keyboardRotationRepeatDelay))
        {
            m_rotationChangedTime = Time.time;
            m_prevRotation = currentTransform.rotation;
            m_keyboardRotationTime = Time.time;
            m_desiredRotationEulerAngles.y += desiredEulerAngles.y;
            if (toggleInput)
            {
                float angle = Mathf.Abs(desiredEulerAngles.y);
                m_desiredRotationEulerAngles.y = (int)(m_desiredRotationEulerAngles.y / angle) * angle;
            }

            m_desiredRotationEulerAngles.y %= 360f;
        }

        rotationThisFrame = Quaternion.Euler(m_desiredRotationEulerAngles);
        if (!IsMouseDragRotationRequested())
        {
            float alpha = Easing.ExpoEaseOut(Time.time - m_rotationChangedTime, 0f, 1f, m_keyboardRotationDuration);
            if (alpha < 1f)
            {
                rotationThisFrame = Quaternion.Euler(
                    rotationThisFrame.eulerAngles.x,
                    Mathf.LerpAngle(m_prevRotation.eulerAngles.y, rotationThisFrame.eulerAngles.y, alpha),
                    rotationThisFrame.eulerAngles.z);
            }
        }

        positionDelta = CalcVelocity() * Mathf.Min(Time.unscaledDeltaTime, 0.03333333f);
        if (positionDelta.sqrMagnitude > float.Epsilon)
        {
            Vector3 eulerAngles3 = rotationThisFrame.eulerAngles;
            Quaternion rotation = Quaternion.Euler(0f, eulerAngles3.y, 0f);
            Vector3 a = rotation * -Vector3.forward;
            Vector3 a2 = rotation * -Vector3.right;
            positionDelta = positionDelta.x * a2 + positionDelta.z * a;
        }
    }

    private float CalcZoomVelocity()
    {
        if (ControlpadGameplay.Get() == null || !ControlpadGameplay.Get().UsingControllerInput)
        {
            if (!Input.GetMouseButton(2)
                && Enabled
                && !UIUtils.IsMouseOnGUI()
                && (KeyBinding_UI.Get() == null || !KeyBinding_UI.Get().IsVisible())
                && !EmoticonPanel.IsMouseOverEmoticonPanel())
            {
                return (0f - Input.GetAxis("Mouse ScrollWheel")) * 3f * m_mouseWheelZoomSpeed * FPS_TUNED_AT;
            }
        }
        else if (Enabled
                 && !UIUtils.IsMouseOnGUI()
                 && (KeyBinding_UI.Get() == null || !KeyBinding_UI.Get().IsVisible())
                 && !EmoticonPanel.IsMouseOverEmoticonPanel())
        {
            float axisValue = ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadY);
            return -axisValue * 3f;
        }

        return 0f;
    }

    private Vector3 CalcDesiredEulerAngles(out bool toggleInput)
    {
        toggleInput = false;
        Vector3 result = Vector3.zero;
        bool isInEndingGame = GameFlowData.Get() != null && GameFlowData.Get().gameState == GameState.EndingGame;
        if (SinglePlayerManager.Get() != null && SinglePlayerManager.Get().HasPendingCameraUpdate())
        {
            Vector3 playerPos = GameFlowData.Get().activeOwnedActorData != null
                ? GameFlowData.Get().activeOwnedActorData.transform.position
                : transform.position;
            Vector3 center = CameraManager.Get().CameraPositionBounds.center;
            center = SinglePlayerManager.Get().GetCurrentState().m_cameraRotationTarget.transform.position;

            Vector3 forward = center - playerPos;
            forward.y = 0f;
            float magnitude = forward.magnitude;

            float y = 0f;
            if (!Mathf.Approximately(magnitude, 0f))
            {
                forward /= magnitude;
                Vector3 eulerAngles = Quaternion.LookRotation(forward).eulerAngles;
                y = (int)eulerAngles.y / 45 * 45;
            }

            result.y = y - m_desiredRotationEulerAngles.y;
            return result;
        }

        if (Enabled && !isInEndingGame)
        {
            bool rotateCW = CameraRotateClockwiseToggled
                            || InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraRotateClockwise);
            bool rotateCCW = CameraRotateCounterClockwiseToggled
                             || InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraRotateCounterClockwise);
            if (Input.GetKeyDown(KeyCode.V)
                && DebugParameters.Get() != null
                && DebugParameters.Get().GetParameterAsBool("CameraTiltControl"))
            {
                result.x = m_keyboardPitchIncrement;
                toggleInput = true;
            }
            else if (Input.GetKeyDown(KeyCode.B)
                     && DebugParameters.Get() != null
                     && DebugParameters.Get().GetParameterAsBool("CameraTiltControl"))
            {
                result.x = 0f - m_keyboardPitchIncrement;
                toggleInput = true;
            }
            else if (rotateCW ^ rotateCCW)
            {
                bool invertCameraRotation = AccountPreferences.Get().GetBool(BoolPreference.InvertCameraRotationKbd);
                bool clockwise = rotateCW && !invertCameraRotation || rotateCCW && invertCameraRotation;
                result.y = m_keyboardRotationIncrement * (clockwise ? 1f : -1f);
                toggleInput = true;
                CameraManager.Get().OnPlayerMovedCamera();
            }
            else if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) != 0f)
            {
                bool clockwise = ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) < 0f
                                 && !AccountPreferences.Get().GetBool(BoolPreference.InvertCameraRotationKbd)
                                 || ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) > 0f
                                 && AccountPreferences.Get().GetBool(BoolPreference.InvertCameraRotationKbd);
                result.y = m_keyboardRotationIncrement * (clockwise ? 1f : -1f);
                toggleInput = true;
                CameraManager.Get().OnPlayerMovedCamera();
            }
            else if (IsMouseDragRotationRequested())
            {
                result.y = Input.GetAxis("Mouse X") * m_mouseRotationSpeed;
                result.x = (0f - Input.GetAxis("Mouse Y")) * m_mousePitchSpeed;
            }
        }

        return result;
    }

    private Vector3 CalcVelocity()
    {
        Vector3 result = Vector3.zero;
        bool isInEndingGame = GameFlowData.Get() != null && GameFlowData.Get().gameState == GameState.EndingGame;
        if (!Enabled || isInEndingGame)
        {
            if (isInEndingGame)
            {
                HighlightUtils.Get().ResetCursor();
            }

            return result * FPS_TUNED_AT;
        }

        float dx = 0f;
        float dy = 0f;
        bool usingKeyboardInput = ControlpadGameplay.Get() == null || !ControlpadGameplay.Get().UsingControllerInput;
        if (usingKeyboardInput)
        {
            if (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraPanUp))
            {
                dy = 1f;
            }

            if (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraPanLeft))
            {
                dx = -1f;
            }

            if (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraPanDown))
            {
                dy = -1f;
            }

            if (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraPanRight))
            {
                dx = 1f;
            }
        }
        else
        {
            dx = ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.RightStickX);
            dy = ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.RightStickY);
        }

        HighlightUtils.ScrollCursorDirection scrollCursorDirection = HighlightUtils.ScrollCursorDirection.Undefined;
        Vector2 mousePosition = new Vector2(
            Input.mousePosition.x / Screen.width,
            Input.mousePosition.y / Screen.height);

        Vector3 mouseInput = Vector3.zero;
        if (Cursor.lockState == CursorLockMode.Confined && (m_mouseMoveFringeInEditor || !Application.isEditor))
        {
            if (mousePosition.x <= m_mouseMoveFringeLeft)
            {
                mouseInput.x = m_mouseMoveFringeSpeed;
                scrollCursorDirection = HighlightUtils.ScrollCursorDirection.W;
            }
            else if (mousePosition.x >= 1f - m_mouseMoveFringeRight)
            {
                mouseInput.x = -m_mouseMoveFringeSpeed;
                scrollCursorDirection = HighlightUtils.ScrollCursorDirection.E;
            }

            if (mousePosition.y <= m_mouseMoveFringeBottom)
            {
                mouseInput.z = m_mouseMoveFringeSpeed;

                switch (scrollCursorDirection)
                {
                    case HighlightUtils.ScrollCursorDirection.W:
                        scrollCursorDirection = HighlightUtils.ScrollCursorDirection.SW;
                        break;
                    case HighlightUtils.ScrollCursorDirection.E:
                        scrollCursorDirection = HighlightUtils.ScrollCursorDirection.SE;
                        break;
                    default:
                        scrollCursorDirection = HighlightUtils.ScrollCursorDirection.S;
                        break;
                }
            }
            else if (mousePosition.y >= 1f - m_mouseMoveFringeTop)
            {
                mouseInput.z = -m_mouseMoveFringeSpeed;

                switch (scrollCursorDirection)
                {
                    case HighlightUtils.ScrollCursorDirection.W:
                        scrollCursorDirection = HighlightUtils.ScrollCursorDirection.NW;
                        break;
                    case HighlightUtils.ScrollCursorDirection.E:
                        scrollCursorDirection = HighlightUtils.ScrollCursorDirection.NE;
                        break;
                    default:
                        scrollCursorDirection = HighlightUtils.ScrollCursorDirection.N;
                        break;
                }
            }
        }

        bool hasMouseInput = mouseInput != Vector3.zero && !UIUtils.IsMouseOnGUI();
        if (hasMouseInput)
        {
            m_mouseMoveFringeSpeedEased.EaseTo(mouseInput, m_mouseMoveFringeEaseIn, m_mouseMoveFringeEaseInTime);
            HighlightUtils.Get().SetScrollCursor(scrollCursorDirection);
            if (scrollCursorDirection != m_lastScrollCursorDirection)
            {
                m_lastScrollCursorDirection = scrollCursorDirection;
                m_timeLastScrollCursorStarted = Time.unscaledTime;
            }
            else if (Time.unscaledTime > m_timeLastScrollCursorStarted + m_secondsUntilScrollCursorStop)
            {
                WinUtils.User32.GetCursorPos(out WinUtils.User32.POINT lpPoint);
                if (m_lastScrollCursorDirection == HighlightUtils.ScrollCursorDirection.NE
                    || m_lastScrollCursorDirection == HighlightUtils.ScrollCursorDirection.E
                    || m_lastScrollCursorDirection == HighlightUtils.ScrollCursorDirection.SE)
                {
                    lpPoint.X -= 2;
                }

                if (m_lastScrollCursorDirection == HighlightUtils.ScrollCursorDirection.NW
                    || m_lastScrollCursorDirection == HighlightUtils.ScrollCursorDirection.W
                    || m_lastScrollCursorDirection == HighlightUtils.ScrollCursorDirection.SW)
                {
                    lpPoint.X += 2;
                }

                if (m_lastScrollCursorDirection == HighlightUtils.ScrollCursorDirection.NE
                    || m_lastScrollCursorDirection == HighlightUtils.ScrollCursorDirection.N
                    || m_lastScrollCursorDirection == HighlightUtils.ScrollCursorDirection.NW)
                {
                    lpPoint.Y += 2;
                }

                if (m_lastScrollCursorDirection == HighlightUtils.ScrollCursorDirection.SE
                    || m_lastScrollCursorDirection == HighlightUtils.ScrollCursorDirection.S
                    || m_lastScrollCursorDirection == HighlightUtils.ScrollCursorDirection.SW)
                {
                    lpPoint.Y -= 2;
                }

                WinUtils.User32.SetCursorPos(lpPoint.X, lpPoint.Y);
            }
        }
        else
        {
            m_mouseMoveFringeSpeedEased.EaseTo(Vector3.zero, m_mouseMoveFringeEaseOut, m_mouseMoveFringeEaseOutTime);
            HighlightUtils.Get().SetScrollCursor(HighlightUtils.ScrollCursorDirection.Undefined);
            m_lastScrollCursorDirection = HighlightUtils.ScrollCursorDirection.Undefined;
            m_timeLastScrollCursorStarted = Time.unscaledTime;
        }

        if (IsMouseDragMoveRequested())
        {
            result.x = Input.GetAxis("Mouse X") * m_mouseMoveSpeed;
            result.z = Input.GetAxis("Mouse Y") * m_mouseMoveSpeed;
        }
        else
        {
            if (dx == 0f && dy == 0f)
            {
                if (hasMouseInput)
                {
                    result = m_mouseMoveFringeEaseInTime > 0f ? m_mouseMoveFringeSpeedEased : mouseInput;
                }
                else
                {
                    result = m_mouseMoveFringeEaseOutTime > 0f ? m_mouseMoveFringeSpeedEased : Vector3.zero;
                }
            }
            else
            {
                if (dy != 0f)
                {
                    result.z += 0f - m_keyboardMoveSpeed * dy;
                }

                if (dx != 0f)
                {
                    result.x += 0f - m_keyboardMoveSpeed * dx;
                }
            }
        }

        return result * FPS_TUNED_AT;
    }
}