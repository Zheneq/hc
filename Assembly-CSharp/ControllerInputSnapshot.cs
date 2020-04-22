using System.Collections.Generic;
using UnityEngine;

public class ControllerInputSnapshot
{
	public List<ControllerButtonState> m_allGamepadButtons;

	public float LeftStickX
	{
		get;
		private set;
	}

	public float LeftStickY
	{
		get;
		private set;
	}

	public float RightStickX
	{
		get;
		private set;
	}

	public float RightStickY
	{
		get;
		private set;
	}

	public float LeftTrigger
	{
		get;
		private set;
	}

	public float RightTrigger
	{
		get;
		private set;
	}

	public float DpadX
	{
		get;
		private set;
	}

	public float DpadY
	{
		get;
		private set;
	}

	public float MouseX
	{
		get;
		private set;
	}

	public float MouseY
	{
		get;
		private set;
	}

	public ControllerButtonState Button_A
	{
		get;
		private set;
	}

	public ControllerButtonState Button_B
	{
		get;
		private set;
	}

	public ControllerButtonState Button_X
	{
		get;
		private set;
	}

	public ControllerButtonState Button_Y
	{
		get;
		private set;
	}

	public ControllerButtonState Button_leftShoulder
	{
		get;
		private set;
	}

	public ControllerButtonState Button_rightShoulder
	{
		get;
		private set;
	}

	public ControllerButtonState Button_start
	{
		get;
		private set;
	}

	public ControllerButtonState Button_back
	{
		get;
		private set;
	}

	public ControllerButtonState Button_leftStickIn
	{
		get;
		private set;
	}

	public ControllerButtonState Button_rightStickIn
	{
		get;
		private set;
	}

	public ControllerButtonState MouseButton_0
	{
		get;
		private set;
	}

	public ControllerButtonState MouseButton_1
	{
		get;
		private set;
	}

	public ControllerButtonState MouseButton_2
	{
		get;
		private set;
	}

	public Vector3 LeftStickWorldDir
	{
		get;
		private set;
	}

	public Vector3 RightStickWorldDir
	{
		get;
		private set;
	}

	public Vector3 DpadWorldDir
	{
		get;
		private set;
	}

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
		m_allGamepadButtons = new List<ControllerButtonState>(10);
		m_allGamepadButtons.Add(Button_A);
		m_allGamepadButtons.Add(Button_B);
		m_allGamepadButtons.Add(Button_X);
		m_allGamepadButtons.Add(Button_Y);
		m_allGamepadButtons.Add(Button_leftShoulder);
		m_allGamepadButtons.Add(Button_rightShoulder);
		m_allGamepadButtons.Add(Button_start);
		m_allGamepadButtons.Add(Button_back);
		m_allGamepadButtons.Add(Button_leftStickIn);
		m_allGamepadButtons.Add(Button_rightStickIn);
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
		Camera main = Camera.main;
		if (main != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Vector3 forward = main.transform.forward;
					Vector3 vector = new Vector3(forward.x, 0f, forward.z);
					vector.Normalize();
					Vector3 a = Vector3.Cross(Vector3.up, vector);
					a.Normalize();
					Vector3 vector2 = a * LeftStickX + vector * LeftStickY;
					Vector3 vector3 = a * RightStickX + vector * RightStickY;
					Vector3 vector4 = a * DpadX + vector * DpadY;
					LeftStickWorldDir = vector2.normalized;
					RightStickWorldDir = vector3.normalized;
					DpadWorldDir = vector4.normalized;
					return;
				}
				}
			}
		}
		LeftStickWorldDir = Vector3.zero;
		RightStickWorldDir = Vector3.zero;
		DpadWorldDir = Vector3.zero;
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
		for (int i = 0; i < m_allGamepadButtons.Count; i++)
		{
			if (!m_allGamepadButtons[i].BeingUsed)
			{
				continue;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return true;
			}
		}
		return false;
	}

	public bool IsUsingAnyMouseButton()
	{
		return MouseButton_0.BeingUsed || MouseButton_1.BeingUsed || MouseButton_2.BeingUsed;
	}

	public float GetValueOfInput(ControlpadInputValue input)
	{
		if (input == ControlpadInputValue.LeftStickX)
		{
			return LeftStickX;
		}
		if (input == ControlpadInputValue.LeftStickY)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return LeftStickY;
				}
			}
		}
		if (input == ControlpadInputValue.RightStickX)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return RightStickX;
				}
			}
		}
		if (input == ControlpadInputValue.RightStickY)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return RightStickY;
				}
			}
		}
		if (input == ControlpadInputValue.LeftTrigger)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return LeftTrigger;
				}
			}
		}
		if (input == ControlpadInputValue.RightTrigger)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return RightTrigger;
				}
			}
		}
		if (input == ControlpadInputValue.DpadX)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return DpadX;
				}
			}
		}
		if (input == ControlpadInputValue.DpadY)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return DpadY;
				}
			}
		}
		if (input == ControlpadInputValue.Button_A)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					float result;
					if (Button_A.Value)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						result = 1f;
					}
					else
					{
						result = 0f;
					}
					return result;
				}
				}
			}
		}
		if (input == ControlpadInputValue.Button_B)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					float result2;
					if (Button_A.Value)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						result2 = 1f;
					}
					else
					{
						result2 = 0f;
					}
					return result2;
				}
				}
			}
		}
		if (input == ControlpadInputValue.Button_X)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					float result3;
					if (Button_X.Value)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						result3 = 1f;
					}
					else
					{
						result3 = 0f;
					}
					return result3;
				}
				}
			}
		}
		if (input == ControlpadInputValue.Button_Y)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					float result4;
					if (Button_Y.Value)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						result4 = 1f;
					}
					else
					{
						result4 = 0f;
					}
					return result4;
				}
				}
			}
		}
		if (input == ControlpadInputValue.Button_leftShoulder)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					float result5;
					if (Button_leftShoulder.Value)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						result5 = 1f;
					}
					else
					{
						result5 = 0f;
					}
					return result5;
				}
				}
			}
		}
		if (input == ControlpadInputValue.Button_rightShoulder)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					float result6;
					if (Button_rightShoulder.Value)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						result6 = 1f;
					}
					else
					{
						result6 = 0f;
					}
					return result6;
				}
				}
			}
		}
		if (input == ControlpadInputValue.Button_start)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					float result7;
					if (Button_start.Value)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						result7 = 1f;
					}
					else
					{
						result7 = 0f;
					}
					return result7;
				}
				}
			}
		}
		switch (input)
		{
		case ControlpadInputValue.Button_back:
			return (!Button_back.Value) ? 0f : 1f;
		case ControlpadInputValue.Button_leftStickIn:
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				float result9;
				if (Button_leftStickIn.Value)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					result9 = 1f;
				}
				else
				{
					result9 = 0f;
				}
				return result9;
			}
		case ControlpadInputValue.Button_rightStickIn:
		{
			float result8;
			if (Button_rightStickIn.Value)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				result8 = 1f;
			}
			else
			{
				result8 = 0f;
			}
			return result8;
		}
		default:
			return 0f;
		}
	}
}
