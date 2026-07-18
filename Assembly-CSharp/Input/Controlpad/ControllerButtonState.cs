using UnityEngine;

public class ControllerButtonState
{
    private bool m_value;
    private bool m_up;
    private bool m_down;

    public bool Value
    {
        get => m_value;
        private set
        {
            if (m_value != value)
            {
                m_value = value;
            }
        }
    }

    public bool Up
    {
        get => m_up;
        private set
        {
            if (m_up != value)
            {
                m_up = value;
            }
        }
    }

    public bool Down
    {
        get => m_down;
        private set
        {
            if (m_down != value)
            {
                m_down = value;
            }
        }
    }

    public bool BeingUsed => m_value || m_up || m_down;

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
        return $"{(!m_down ? !m_up ? "=" : "^" : "_")}{(m_value ? "1" : "0")}";
    }
}