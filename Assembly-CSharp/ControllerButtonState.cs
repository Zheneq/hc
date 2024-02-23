using System.Text;
using UnityEngine;

public class ControllerButtonState
{
	private bool m_value;

	private bool m_up;

	private bool m_down;

	public bool Value
	{
		get
		{
			return m_value;
		}
		private set
		{
			if (m_value == value)
			{
				return;
			}
			while (true)
			{
				if (value)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							m_value = true;
							return;
						}
					}
				}
				m_value = false;
				return;
			}
		}
	}

	public bool Up
	{
		get
		{
			return m_up;
		}
		private set
		{
			if (m_up == value)
			{
				return;
			}
			while (true)
			{
				if (value)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							m_up = true;
							return;
						}
					}
				}
				m_up = false;
				return;
			}
		}
	}

	public bool Down
	{
		get
		{
			return m_down;
		}
		private set
		{
			if (m_down == value)
			{
				return;
			}
			while (true)
			{
				if (value)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							m_down = true;
							return;
						}
					}
				}
				m_down = false;
				return;
			}
		}
	}

	public bool BeingUsed
	{
		get { return m_value || m_up || m_down; }
	}

	public void GatherState(string buttonStr)
	{
		Value = Input.GetButton(buttonStr);
		Up = Input.GetButtonUp(buttonStr);
		Down = Input.GetButtonDown(buttonStr);
	}

	public void GatherStateFromMouseButton(int mouseButtonIndex)
	{
		Value = Input.GetMouseButton(mouseButtonIndex);
		Down = Input.GetMouseButtonDown(mouseButtonIndex);
		Up = Input.GetMouseButtonUp(mouseButtonIndex);
	}

	public void ClearAllValues()
	{
		m_value = false;
		m_up = false;
		m_down = false;
	}

	public void CopyButtonValuesFrom(ControllerButtonState other)
	{
		Value = other.Value;
		Up = other.Up;
		Down = other.Down;
	}

	public string GetDebugString()
	{
		string arg;
		if (!m_down)
		{
			arg = ((!m_up) ? "=" : "^");
		}
		else
		{
			arg = "_";
		}
		string arg2;
		if (m_value)
		{
			arg2 = "1";
		}
		else
		{
			arg2 = "0";
		}
		return new StringBuilder().Append(arg).Append(arg2).ToString();
	}
}
