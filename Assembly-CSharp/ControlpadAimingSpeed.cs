using UnityEngine;

public class ControlpadAimingSpeed
{
	public float m_startSpeed = 1f;

	public float m_maxSpeed = 1f;

	public float m_acceleration;

	public float m_minSpeed;

	public float GetSpeed(float timeHeldDown, float multiplier = 1f)
	{
		float value = (m_startSpeed + timeHeldDown * m_acceleration) * Mathf.Abs(multiplier);
		return Mathf.Clamp(value, m_minSpeed, m_maxSpeed);
	}

	public static ControlpadAimingSpeed DefaultAnalogStickDepth()
	{
		ControlpadAimingSpeed controlpadAimingSpeed = new ControlpadAimingSpeed();
		controlpadAimingSpeed.m_startSpeed = 6f;
		controlpadAimingSpeed.m_maxSpeed = 6f;
		controlpadAimingSpeed.m_acceleration = 0f;
		controlpadAimingSpeed.m_minSpeed = 0f;
		return controlpadAimingSpeed;
	}

	public static ControlpadAimingSpeed DefaultDigitalButtonRotation()
	{
		ControlpadAimingSpeed controlpadAimingSpeed = new ControlpadAimingSpeed();
		controlpadAimingSpeed.m_startSpeed = 2f;
		controlpadAimingSpeed.m_maxSpeed = 360f;
		controlpadAimingSpeed.m_acceleration = 180f;
		controlpadAimingSpeed.m_minSpeed = 0f;
		return controlpadAimingSpeed;
	}

	public static ControlpadAimingSpeed DefaultAnalogStickRotation()
	{
		ControlpadAimingSpeed controlpadAimingSpeed = new ControlpadAimingSpeed();
		controlpadAimingSpeed.m_startSpeed = 20f;
		controlpadAimingSpeed.m_maxSpeed = 360f;
		controlpadAimingSpeed.m_acceleration = 720f;
		controlpadAimingSpeed.m_minSpeed = 0f;
		return controlpadAimingSpeed;
	}

	public static ControlpadAimingSpeed DefaultAnalogStickTranslation()
	{
		ControlpadAimingSpeed controlpadAimingSpeed = new ControlpadAimingSpeed();
		controlpadAimingSpeed.m_startSpeed = 10f;
		controlpadAimingSpeed.m_maxSpeed = 10f;
		controlpadAimingSpeed.m_acceleration = 0f;
		controlpadAimingSpeed.m_minSpeed = 0f;
		return controlpadAimingSpeed;
	}
}
