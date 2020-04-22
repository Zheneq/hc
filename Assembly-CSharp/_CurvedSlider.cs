using UnityEngine;
using UnityEngine.UI;

public class _CurvedSlider : MonoBehaviour
{
	public delegate void UpdateValue(float newF);

	public Mask m_maskObject;

	public RectTransform m_rotatingImage;

	[Range(0f, 1f)]
	public float m_currentVal;

	public float m_minAngleToRotate;

	public float m_maxAngleToRotate = 360f;

	public int m_numSteps;

	private float previousVal;

	private int previousSteps;

	private UpdateValue OnValueChanged;

	private void Start()
	{
	}

	public void SetOnValueChangedCallback(UpdateValue callback)
	{
		OnValueChanged = callback;
	}

	public void UpdateSliderAmount(float pct)
	{
		if (m_rotatingImage == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		previousSteps = m_numSteps;
		m_currentVal = pct;
		previousVal = m_currentVal;
		if (m_numSteps > 0)
		{
			m_currentVal = Mathf.Round(m_currentVal * (float)m_numSteps) * (1f / (float)m_numSteps);
		}
		m_currentVal = Mathf.Clamp(m_currentVal, 0f, 1f);
		Vector3 localEulerAngles = m_rotatingImage.transform.localEulerAngles;
		localEulerAngles.z = m_currentVal * (m_maxAngleToRotate - m_minAngleToRotate) + m_minAngleToRotate;
		m_rotatingImage.transform.localEulerAngles = localEulerAngles;
	}

	private void CallValuedChanged()
	{
		if (OnValueChanged == null)
		{
			return;
		}
		while (true)
		{
			OnValueChanged(m_currentVal);
			return;
		}
	}

	private void UpdateNewVal()
	{
		previousVal = m_currentVal;
		UpdateSliderAmount(m_currentVal);
		CallValuedChanged();
	}

	private void Update()
	{
		if (previousVal == m_currentVal)
		{
			if (previousSteps == m_numSteps)
			{
				return;
			}
		}
		UpdateNewVal();
	}
}
