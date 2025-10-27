using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControllerInputSnapshot
{
    public List<ControllerButtonState> m_allGamepadButtons;

    public float LeftStickX { get; private set; }
    public float LeftStickY { get; private set; }
    public float RightStickX { get; private set; }
    public float RightStickY { get; private set; }
    public float LeftTrigger { get; private set; }
    public float RightTrigger { get; private set; }
    public float DpadX { get; private set; }
    public float DpadY { get; private set; }
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }
    public ControllerButtonState Button_A { get; private set; }
    public ControllerButtonState Button_B { get; private set; }
    public ControllerButtonState Button_X { get; private set; }
    public ControllerButtonState Button_Y { get; private set; }
    public ControllerButtonState Button_leftShoulder { get; private set; }
    public ControllerButtonState Button_rightShoulder { get; private set; }
    public ControllerButtonState Button_start { get; private set; }
    public ControllerButtonState Button_back { get; private set; }
    public ControllerButtonState Button_leftStickIn { get; private set; }
    public ControllerButtonState Button_rightStickIn { get; private set; }
    public ControllerButtonState MouseButton_0 { get; private set; }
    public ControllerButtonState MouseButton_1 { get; private set; }
    public ControllerButtonState MouseButton_2 { get; private set; }
    public Vector3 LeftStickWorldDir { get; private set; }
    public Vector3 RightStickWorldDir { get; private set; }
    public Vector3 DpadWorldDir { get; private set; }

    public ControllerInputSnapshot()
    {
        Button_A = new ControllerButtonState();
        Button_B = new ControllerButtonState();
        Button_X = new ControllerButtonState();
        Button_Y = new ControllerButtonState();
        Button_leftShoulder = new ControllerButtonState();
        Button_rightShoulder = new ControllerButtonState();
        Button_start = new ControllerButtonState();
        Button_back = new ControllerButtonState();
        Button_leftStickIn = new ControllerButtonState();
        Button_rightStickIn = new ControllerButtonState();
        MouseButton_0 = new ControllerButtonState();
        MouseButton_1 = new ControllerButtonState();
        MouseButton_2 = new ControllerButtonState();
        m_allGamepadButtons = new List<ControllerButtonState>(10)
        {
            Button_A,
            Button_B,
            Button_X,
            Button_Y,
            Button_leftShoulder,
            Button_rightShoulder,
            Button_start,
            Button_back,
            Button_leftStickIn,
            Button_rightStickIn
        };
    }

    public void CacheInputThisFrame()
    {
        LeftStickX = Input.GetAxis("GamepadLeftStickX");
        LeftStickY = Input.GetAxis("GamepadLeftStickY");
        RightStickX = Input.GetAxis("GamepadRightStickX");
        RightStickY = Input.GetAxis("GamepadRightStickY");
        LeftTrigger = Input.GetAxis("GamepadLeftTrigger");
        RightTrigger = Input.GetAxis("GamepadRightTrigger");
        DpadX = Input.GetAxis("GamepadDpadX");
        DpadY = Input.GetAxis("GamepadDpadY");
        Button_A.GatherState("GamepadButtonA");
        Button_B.GatherState("GamepadButtonB");
        Button_X.GatherState("GamepadButtonX");
        Button_Y.GatherState("GamepadButtonY");
        Button_leftShoulder.GatherState("GamepadButtonLeftShoulder");
        Button_rightShoulder.GatherState("GamepadButtonRightShoulder");
        Button_start.GatherState("GamepadButtonStart");
        Button_back.GatherState("GamepadButtonBack");
        Button_leftStickIn.GatherState("GamepadButtonLeftStickIn");
        Button_rightStickIn.GatherState("GamepadButtonRightStickIn");
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
        MouseButton_0.GatherStateFromMouseButton(0);
        MouseButton_1.GatherStateFromMouseButton(1);
        MouseButton_2.GatherStateFromMouseButton(2);
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 forward = mainCamera.transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = Vector3.Cross(Vector3.up, forward);
            right.Normalize();

            Vector3 leftStick = right * LeftStickX + forward * LeftStickY;
            Vector3 rightStick = right * RightStickX + forward * RightStickY;
            Vector3 dPad = right * DpadX + forward * DpadY;
            LeftStickWorldDir = leftStick.normalized;
            RightStickWorldDir = rightStick.normalized;
            DpadWorldDir = dPad.normalized;
        }
        else
        {
            LeftStickWorldDir = Vector3.zero;
            RightStickWorldDir = Vector3.zero;
            DpadWorldDir = Vector3.zero;
        }
    }

    public void ClearAllValues()
    {
        LeftStickX = 0f;
        LeftStickY = 0f;
        RightStickX = 0f;
        RightStickY = 0f;
        LeftTrigger = 0f;
        RightTrigger = 0f;
        DpadX = 0f;
        DpadY = 0f;
        Button_A.ClearAllValues();
        Button_B.ClearAllValues();
        Button_X.ClearAllValues();
        Button_Y.ClearAllValues();
        Button_leftShoulder.ClearAllValues();
        Button_rightShoulder.ClearAllValues();
        Button_start.ClearAllValues();
        Button_back.ClearAllValues();
        Button_leftStickIn.ClearAllValues();
        Button_rightStickIn.ClearAllValues();
    }

    public void CopySnapshotValuesFrom(ControllerInputSnapshot other)
    {
        LeftStickX = other.LeftStickX;
        LeftStickY = other.LeftStickY;
        RightStickX = other.RightStickX;
        RightStickY = other.RightStickY;
        LeftTrigger = other.LeftTrigger;
        RightTrigger = other.RightTrigger;
        DpadX = other.DpadX;
        DpadY = other.DpadY;
        Button_A.CopyButtonValuesFrom(other.Button_A);
        Button_B.CopyButtonValuesFrom(other.Button_B);
        Button_X.CopyButtonValuesFrom(other.Button_X);
        Button_Y.CopyButtonValuesFrom(other.Button_Y);
        Button_leftShoulder.CopyButtonValuesFrom(other.Button_leftShoulder);
        Button_rightShoulder.CopyButtonValuesFrom(other.Button_rightShoulder);
        Button_start.CopyButtonValuesFrom(other.Button_start);
        Button_back.CopyButtonValuesFrom(other.Button_back);
        Button_leftStickIn.CopyButtonValuesFrom(other.Button_leftStickIn);
        Button_rightStickIn.CopyButtonValuesFrom(other.Button_rightStickIn);
    }

    public bool IsUsingAnyGamepadButton()
    {
        return m_allGamepadButtons.Any(button => button.BeingUsed);
    }

    public bool IsUsingAnyMouseButton()
    {
        return MouseButton_0.BeingUsed
               || MouseButton_1.BeingUsed
               || MouseButton_2.BeingUsed;
    }

    public float GetValueOfInput(ControlpadInputValue input)
    {
        switch (input)
        {
            case ControlpadInputValue.LeftStickX:
                return LeftStickX;
            case ControlpadInputValue.LeftStickY:
                return LeftStickY;
            case ControlpadInputValue.RightStickX:
                return RightStickX;
            case ControlpadInputValue.RightStickY:
                return RightStickY;
            case ControlpadInputValue.LeftTrigger:
                return LeftTrigger;
            case ControlpadInputValue.RightTrigger:
                return RightTrigger;
            case ControlpadInputValue.DpadX:
                return DpadX;
            case ControlpadInputValue.DpadY:
                return DpadY;
            case ControlpadInputValue.Button_A:
                return Button_A.Value ? 1f : 0f;
            case ControlpadInputValue.Button_B:
                return Button_A.Value ? 1f : 0f;
            case ControlpadInputValue.Button_X:
                return Button_X.Value ? 1f : 0f;
            case ControlpadInputValue.Button_Y:
                return Button_Y.Value ? 1f : 0f;
            case ControlpadInputValue.Button_leftShoulder:
                return Button_leftShoulder.Value ? 1f : 0f;
            case ControlpadInputValue.Button_rightShoulder:
                return Button_rightShoulder.Value ? 1f : 0f;
            case ControlpadInputValue.Button_start:
                return Button_start.Value ? 1f : 0f;
            case ControlpadInputValue.Button_back:
                return Button_back.Value ? 1f : 0f;
            case ControlpadInputValue.Button_leftStickIn:
                return Button_leftStickIn.Value ? 1f : 0f;
            case ControlpadInputValue.Button_rightStickIn:
                return Button_rightStickIn.Value ? 1f : 0f;
            default:
                return 0f;
        }
    }
}