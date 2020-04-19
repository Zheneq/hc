using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Mask))]
public class _MaskChildFreezer : MonoBehaviour
{
	private Vector3[] m_frozenPosition;

	private Vector3[] m_frozenScale;

	private Vector2 m_previousAnchorPosition;

	private Vector3 m_previousScalePosition;

	private RectTransform maskRectTransform;

	public RectTransform m_originalMaskTransform;

	public RectTransform[] m_frozenChildren;

	private void Awake()
	{
		this.m_frozenPosition = new Vector3[this.m_frozenChildren.Length];
		this.m_frozenScale = new Vector3[this.m_frozenChildren.Length];
		this.maskRectTransform = (base.gameObject.transform as RectTransform);
		this.m_originalMaskTransform.eulerAngles = this.maskRectTransform.eulerAngles;
		this.m_originalMaskTransform.position = this.maskRectTransform.position;
		this.m_originalMaskTransform.localScale = this.maskRectTransform.localScale;
		this.m_originalMaskTransform.anchoredPosition3D = this.maskRectTransform.anchoredPosition3D;
		this.m_originalMaskTransform.anchorMax = this.maskRectTransform.anchorMax;
		this.m_originalMaskTransform.anchorMin = this.maskRectTransform.anchorMin;
		this.m_originalMaskTransform.pivot = this.maskRectTransform.pivot;
		this.m_originalMaskTransform.sizeDelta = this.maskRectTransform.sizeDelta;
		for (int i = 0; i < this.m_frozenChildren.Length; i++)
		{
			this.m_frozenPosition[i] = this.m_frozenChildren[i].GetComponent<RectTransform>().anchoredPosition;
			this.m_frozenScale[i] = this.m_frozenChildren[i].GetComponent<RectTransform>().localScale;
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(_MaskChildFreezer.Awake()).MethodHandle;
		}
	}

	private void LateUpdate()
	{
		if (this.m_previousAnchorPosition != this.maskRectTransform.anchoredPosition)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(_MaskChildFreezer.LateUpdate()).MethodHandle;
			}
			this.m_previousAnchorPosition = this.maskRectTransform.anchoredPosition;
			for (int i = 0; i < this.m_frozenPosition.Length; i++)
			{
				this.m_frozenChildren[i].SetParent(this.m_originalMaskTransform);
				this.m_frozenChildren[i].anchoredPosition = this.m_frozenPosition[i];
				this.m_frozenChildren[i].SetParent(this.maskRectTransform);
			}
		}
		if (this.m_previousScalePosition != this.maskRectTransform.localScale)
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
			this.m_previousScalePosition = this.maskRectTransform.localScale;
			for (int j = 0; j < this.m_frozenScale.Length; j++)
			{
				this.m_frozenChildren[j].SetParent(this.m_originalMaskTransform);
				this.m_frozenChildren[j].localScale = this.m_frozenScale[j];
				this.m_frozenChildren[j].anchoredPosition = this.m_frozenPosition[j];
				this.m_frozenChildren[j].SetParent(this.maskRectTransform);
			}
		}
	}
}
