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
				if (value)
				{
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
				if (value)
				{
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
				if (value)
				{
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
			arg2 = "1";
		}
		else
		{
			arg2 = "0";
		}
		return string.Format("{0}{1}", arg, arg2);
	}
}
