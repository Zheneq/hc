using System;
using UnityEngine;

public class ControllerButtonState
{
	private bool m_value;

	private bool m_up;

	private bool m_down;

	public void GatherState(string buttonStr)
	{
		this.Value = Input.GetButton(buttonStr);
		this.Up = Input.GetButtonUp(buttonStr);
		this.Down = Input.GetButtonDown(buttonStr);
	}

	public void GatherStateFromMouseButton(int mouseButtonIndex)
	{
		this.Value = Input.GetMouseButton(mouseButtonIndex);
		this.Down = Input.GetMouseButtonDown(mouseButtonIndex);
		this.Up = Input.GetMouseButtonUp(mouseButtonIndex);
	}

	public void ClearAllValues()
	{
		this.m_value = false;
		this.m_up = false;
		this.m_down = false;
	}

	public void CopyButtonValuesFrom(ControllerButtonState other)
	{
		this.Value = other.Value;
		this.Up = other.Up;
		this.Down = other.Down;
	}

	public bool Value
	{
		get
		{
			return this.m_value;
		}
		private set
		{
			if (this.m_value != value)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControllerButtonState.set_Value(bool)).MethodHandle;
				}
				if (value)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_value = true;
				}
				else
				{
					this.m_value = false;
				}
			}
		}
	}

	public bool Up
	{
		get
		{
			return this.m_up;
		}
		private set
		{
			if (this.m_up != value)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControllerButtonState.set_Up(bool)).MethodHandle;
				}
				if (value)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_up = true;
				}
				else
				{
					this.m_up = false;
				}
			}
		}
	}

	public bool Down
	{
		get
		{
			return this.m_down;
		}
		private set
		{
			if (this.m_down != value)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControllerButtonState.set_Down(bool)).MethodHandle;
				}
				if (value)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_down = true;
				}
				else
				{
					this.m_down = false;
				}
			}
		}
	}

	public bool BeingUsed
	{
		get
		{
			return this.m_value || this.m_up || this.m_down;
		}
	}

	public string GetDebugString()
	{
		string arg;
		if (this.m_down)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControllerButtonState.GetDebugString()).MethodHandle;
			}
			arg = "_";
		}
		else if (this.m_up)
		{
			arg = "^";
		}
		else
		{
			arg = "=";
		}
		string arg2;
		if (this.m_value)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			arg2 = "1";
		}
		else
		{
			arg2 = "0";
		}
		return string.Format("{0}{1}", arg, arg2);
	}
}
