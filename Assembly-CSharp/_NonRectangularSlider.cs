using System;
using UnityEngine;

[RequireComponent(typeof(_MaskChildFreezer))]
public class _NonRectangularSlider : MonoBehaviour
{
	[Range(0f, 1f)]
	public float m_val;

	private float m_lastValSet = -1f;

	private _MaskChildFreezer m_theMaskChildFreezer;

	private void Start()
	{
		this.m_theMaskChildFreezer = base.GetComponent<_MaskChildFreezer>();
	}

	private void SetBarPercentVisual(_MaskChildFreezer mask, float percent)
	{
		this.m_lastValSet = percent;
		float width = (mask.transform as RectTransform).rect.width;
		float x = (percent - 1f) * width;
		(mask.transform as RectTransform).anchoredPosition = this.m_theMaskChildFreezer.m_originalMaskTransform.anchoredPosition + new Vector2(x, 0f);
	}

	private void Update()
	{
		if (this.m_val != this.m_lastValSet)
		{
			this.SetBarPercentVisual(this.m_theMaskChildFreezer, this.m_val);
		}
	}
}
