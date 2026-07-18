using UnityEngine;

public class ControlpadAimingSpeed
{
    public float m_startSpeed = 1f;
    public float m_maxSpeed = 1f;
    public float m_acceleration;
    public float m_minSpeed;

    public float GetSpeed(float timeHeldDown, float multiplier = 1f)
    {
        float speed = (m_startSpeed + timeHeldDown * m_acceleration) * Mathf.Abs(multiplier);
        return Mathf.Clamp(speed, m_minSpeed, m_maxSpeed);
    }

    public static ControlpadAimingSpeed DefaultAnalogStickDepth()
    {
        return new ControlpadAimingSpeed
        {
            m_startSpeed = 6f,
            m_maxSpeed = 6f,
            m_acceleration = 0f,
            m_minSpeed = 0f
        };
    }

    public static ControlpadAimingSpeed DefaultDigitalButtonRotation()
    {
        return new ControlpadAimingSpeed
        {
            m_startSpeed = 2f,
            m_maxSpeed = 360f,
            m_acceleration = 180f,
            m_minSpeed = 0f
        };
    }

    public static ControlpadAimingSpeed DefaultAnalogStickRotation()
    {
        return new ControlpadAimingSpeed
        {
            m_startSpeed = 20f,
            m_maxSpeed = 360f,
            m_acceleration = 720f,
            m_minSpeed = 0f
        };
    }

    public static ControlpadAimingSpeed DefaultAnalogStickTranslation()
    {
        return new ControlpadAimingSpeed
        {
            m_startSpeed = 10f,
            m_maxSpeed = 10f,
            m_acceleration = 0f,
            m_minSpeed = 0f
        };
    }
}