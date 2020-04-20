using System;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputSnapshot
{
	public List<ControllerButtonState> m_allGamepadButtons;

	public ControllerInputSnapshot()
	{
		this.Button_A = new ControllerButtonState();
		this.Button_B = new ControllerButtonState();
		this.Button_X = new ControllerButtonState();
		this.Button_Y = new ControllerButtonState();
		this.Button_leftShoulder = new ControllerButtonState();
		this.Button_rightShoulder = new ControllerButtonState();
		this.Button_start = new ControllerButtonState();
		this.Button_back = new ControllerButtonState();
		this.Button_leftStickIn = new ControllerButtonState();
		this.Button_rightStickIn = new ControllerButtonState();
		this.MouseButton_0 = new ControllerButtonState();
		this.MouseButton_1 = new ControllerButtonState();
		this.MouseButton_2 = new ControllerButtonState();
		this.m_allGamepadButtons = new List<ControllerButtonState>(0xA);
		this.m_allGamepadButtons.Add(this.Button_A);
		this.m_allGamepadButtons.Add(this.Button_B);
		this.m_allGamepadButtons.Add(this.Button_X);
		this.m_allGamepadButtons.Add(this.Button_Y);
		this.m_allGamepadButtons.Add(this.Button_leftShoulder);
		this.m_allGamepadButtons.Add(this.Button_rightShoulder);
		this.m_allGamepadButtons.Add(this.Button_start);
		this.m_allGamepadButtons.Add(this.Button_back);
		this.m_allGamepadButtons.Add(this.Button_leftStickIn);
		this.m_allGamepadButtons.Add(this.Button_rightStickIn);
	}

	public void CacheInputThisFrame()
	{
		this.LeftStickX = Input.GetAxis("GamepadLeftStickX");
		this.LeftStickY = Input.GetAxis("GamepadLeftStickY");
		this.RightStickX = Input.GetAxis("GamepadRightStickX");
		this.RightStickY = Input.GetAxis("GamepadRightStickY");
		this.LeftTrigger = Input.GetAxis("GamepadLeftTrigger");
		this.RightTrigger = Input.GetAxis("GamepadRightTrigger");
		this.DpadX = Input.GetAxis("GamepadDpadX");
		this.DpadY = Input.GetAxis("GamepadDpadY");
		this.Button_A.GatherState("GamepadButtonA");
		this.Button_B.GatherState("GamepadButtonB");
		this.Button_X.GatherState("GamepadButtonX");
		this.Button_Y.GatherState("GamepadButtonY");
		this.Button_leftShoulder.GatherState("GamepadButtonLeftShoulder");
		this.Button_rightShoulder.GatherState("GamepadButtonRightShoulder");
		this.Button_start.GatherState("GamepadButtonStart");
		this.Button_back.GatherState("GamepadButtonBack");
		this.Button_leftStickIn.GatherState("GamepadButtonLeftStickIn");
		this.Button_rightStickIn.GatherState("GamepadButtonRightStickIn");
		this.MouseX = Input.GetAxis("Mouse X");
		this.MouseY = Input.GetAxis("Mouse Y");
		this.MouseButton_0.GatherStateFromMouseButton(0);
		this.MouseButton_1.GatherStateFromMouseButton(1);
		this.MouseButton_2.GatherStateFromMouseButton(2);
		Camera main = Camera.main;
		if (main != null)
		{
			Vector3 forward = main.transform.forward;
			Vector3 vector = new Vector3(forward.x, 0f, forward.z);
			vector.Normalize();
			Vector3 a = Vector3.Cross(Vector3.up, vector);
			a.Normalize();
			Vector3 vector2 = a * this.LeftStickX + vector * this.LeftStickY;
			Vector3 vector3 = a * this.RightStickX + vector * this.RightStickY;
			Vector3 vector4 = a * this.DpadX + vector * this.DpadY;
			this.LeftStickWorldDir = vector2.normalized;
			this.RightStickWorldDir = vector3.normalized;
			this.DpadWorldDir = vector4.normalized;
		}
		else
		{
			this.LeftStickWorldDir = Vector3.zero;
			this.RightStickWorldDir = Vector3.zero;
			this.DpadWorldDir = Vector3.zero;
		}
	}

	public void ClearAllValues()
	{
		this.LeftStickX = 0f;
		this.LeftStickY = 0f;
		this.RightStickX = 0f;
		this.RightStickY = 0f;
		this.LeftTrigger = 0f;
		this.RightTrigger = 0f;
		this.DpadX = 0f;
		this.DpadY = 0f;
		this.Button_A.ClearAllValues();
		this.Button_B.ClearAllValues();
		this.Button_X.ClearAllValues();
		this.Button_Y.ClearAllValues();
		this.Button_leftShoulder.ClearAllValues();
		this.Button_rightShoulder.ClearAllValues();
		this.Button_start.ClearAllValues();
		this.Button_back.ClearAllValues();
		this.Button_leftStickIn.ClearAllValues();
		this.Button_rightStickIn.ClearAllValues();
	}

	public void CopySnapshotValuesFrom(ControllerInputSnapshot other)
	{
		this.LeftStickX = other.LeftStickX;
		this.LeftStickY = other.LeftStickY;
		this.RightStickX = other.RightStickX;
		this.RightStickY = other.RightStickY;
		this.LeftTrigger = other.LeftTrigger;
		this.RightTrigger = other.RightTrigger;
		this.DpadX = other.DpadX;
		this.DpadY = other.DpadY;
		this.Button_A.CopyButtonValuesFrom(other.Button_A);
		this.Button_B.CopyButtonValuesFrom(other.Button_B);
		this.Button_X.CopyButtonValuesFrom(other.Button_X);
		this.Button_Y.CopyButtonValuesFrom(other.Button_Y);
		this.Button_leftShoulder.CopyButtonValuesFrom(other.Button_leftShoulder);
		this.Button_rightShoulder.CopyButtonValuesFrom(other.Button_rightShoulder);
		this.Button_start.CopyButtonValuesFrom(other.Button_start);
		this.Button_back.CopyButtonValuesFrom(other.Button_back);
		this.Button_leftStickIn.CopyButtonValuesFrom(other.Button_leftStickIn);
		this.Button_rightStickIn.CopyButtonValuesFrom(other.Button_rightStickIn);
	}

	public bool IsUsingAnyGamepadButton()
	{
		for (int i = 0; i < this.m_allGamepadButtons.Count; i++)
		{
			if (this.m_allGamepadButtons[i].BeingUsed)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsUsingAnyMouseButton()
	{
		return this.MouseButton_0.BeingUsed || this.MouseButton_1.BeingUsed || this.MouseButton_2.BeingUsed;
	}

	public float GetValueOfInput(ControlpadInputValue input)
	{
		float result;
		if (input == ControlpadInputValue.LeftStickX)
		{
			result = this.LeftStickX;
		}
		else if (input == ControlpadInputValue.LeftStickY)
		{
			result = this.LeftStickY;
		}
		else if (input == ControlpadInputValue.RightStickX)
		{
			result = this.RightStickX;
		}
		else if (input == ControlpadInputValue.RightStickY)
		{
			result = this.RightStickY;
		}
		else if (input == ControlpadInputValue.LeftTrigger)
		{
			result = this.LeftTrigger;
		}
		else if (input == ControlpadInputValue.RightTrigger)
		{
			result = this.RightTrigger;
		}
		else if (input == ControlpadInputValue.DpadX)
		{
			result = this.DpadX;
		}
		else if (input == ControlpadInputValue.DpadY)
		{
			result = this.DpadY;
		}
		else if (input == ControlpadInputValue.Button_A)
		{
			float num;
			if (this.Button_A.Value)
			{
				num = 1f;
			}
			else
			{
				num = 0f;
			}
			result = num;
		}
		else if (input == ControlpadInputValue.Button_B)
		{
			float num2;
			if (this.Button_A.Value)
			{
				num2 = 1f;
			}
			else
			{
				num2 = 0f;
			}
			result = num2;
		}
		else if (input == ControlpadInputValue.Button_X)
		{
			float num3;
			if (this.Button_X.Value)
			{
				num3 = 1f;
			}
			else
			{
				num3 = 0f;
			}
			result = num3;
		}
		else if (input == ControlpadInputValue.Button_Y)
		{
			float num4;
			if (this.Button_Y.Value)
			{
				num4 = 1f;
			}
			else
			{
				num4 = 0f;
			}
			result = num4;
		}
		else if (input == ControlpadInputValue.Button_leftShoulder)
		{
			float num5;
			if (this.Button_leftShoulder.Value)
			{
				num5 = 1f;
			}
			else
			{
				num5 = 0f;
			}
			result = num5;
		}
		else if (input == ControlpadInputValue.Button_rightShoulder)
		{
			float num6;
			if (this.Button_rightShoulder.Value)
			{
				num6 = 1f;
			}
			else
			{
				num6 = 0f;
			}
			result = num6;
		}
		else if (input == ControlpadInputValue.Button_start)
		{
			float num7;
			if (this.Button_start.Value)
			{
				num7 = 1f;
			}
			else
			{
				num7 = 0f;
			}
			result = num7;
		}
		else if (input == ControlpadInputValue.Button_back)
		{
			result = ((!this.Button_back.Value) ? 0f : 1f);
		}
		else if (input == ControlpadInputValue.Button_leftStickIn)
		{
			float num8;
			if (this.Button_leftStickIn.Value)
			{
				num8 = 1f;
			}
			else
			{
				num8 = 0f;
			}
			result = num8;
		}
		else if (input == ControlpadInputValue.Button_rightStickIn)
		{
			float num9;
			if (this.Button_rightStickIn.Value)
			{
				num9 = 1f;
			}
			else
			{
				num9 = 0f;
			}
			result = num9;
		}
		else
		{
			result = 0f;
		}
		return result;
	}

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
}
