using System;
using UnityEngine;
using UnityEngine.UI;

public class _CurvedSlider : MonoBehaviour
{
	public Mask m_maskObject;

	public RectTransform m_rotatingImage;

	[Range(0f, 1f)]
	public float m_currentVal;

	public float m_minAngleToRotate;

	public float m_maxAngleToRotate = 360f;

	public int m_numSteps;

	private float previousVal;

	private int previousSteps;

	private _CurvedSlider.UpdateValue OnValueChanged;

	private void Start()
	{
	}

	public void SetOnValueChangedCallback(_CurvedSlider.UpdateValue callback)
	{
		this.OnValueChanged = callback;
	}

	public void UpdateSliderAmount(float pct)
	{
		if (this.m_rotatingImage == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_CurvedSlider.UpdateSliderAmount(float)).MethodHandle;
			}
			return;
		}
		this.previousSteps = this.m_numSteps;
		this.m_currentVal = pct;
		this.previousVal = this.m_currentVal;
		if (this.m_numSteps > 0)
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
			this.m_currentVal = Mathf.Round(this.m_currentVal * (float)this.m_numSteps) * (1f / (float)this.m_numSteps);
		}
		this.m_currentVal = Mathf.Clamp(this.m_currentVal, 0f, 1f);
		Vector3 localEulerAngles = this.m_rotatingImage.transform.localEulerAngles;
		localEulerAngles.z = this.m_currentVal * (this.m_maxAngleToRotate - this.m_minAngleToRotate) + this.m_minAngleToRotate;
		this.m_rotatingImage.transform.localEulerAngles = localEulerAngles;
	}

	private void CallValuedChanged()
	{
		if (this.OnValueChanged != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(_CurvedSlider.CallValuedChanged()).MethodHandle;
			}
			this.OnValueChanged(this.m_currentVal);
		}
	}

	private void UpdateNewVal()
	{
		this.previousVal = this.m_currentVal;
		this.UpdateSliderAmount(this.m_currentVal);
		this.CallValuedChanged();
	}

	private void Update()
	{
		if (this.previousVal == this.m_currentVal)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_CurvedSlider.Update()).MethodHandle;
			}
			if (this.previousSteps == this.m_numSteps)
			{
				return;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.UpdateNewVal();
	}

	public delegate void UpdateValue(float newF);
}
