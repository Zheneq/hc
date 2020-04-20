using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class _InputFieldCursorFix : MonoBehaviour
{
	private InputField m_inputField;

	private RectTransform m_caretObject;

	private CanvasScaler m_theScalar;

	private Canvas m_theCanvas;

	private float m_originalYScaleAmt;

	private void Awake()
	{
		this.m_inputField = base.gameObject.GetComponent<InputField>();
		this.m_theScalar = base.gameObject.GetComponentInParent<CanvasScaler>();
		this.m_originalYScaleAmt = this.m_inputField.textComponent.transform.localScale.y;
	}

	public void Update()
	{
		if (this.m_caretObject == null)
		{
			if (base.gameObject.transform.Find(base.gameObject.name + " Input Caret") != null)
			{
				this.m_caretObject = (base.gameObject.transform.Find(base.gameObject.name + " Input Caret").transform as RectTransform);
			}
		}
		if (this.m_theScalar == null)
		{
			this.m_theScalar = base.gameObject.GetComponentInParent<CanvasScaler>();
		}
		if (this.m_theCanvas == null)
		{
			this.m_theCanvas = this.m_theScalar.GetComponent<Canvas>();
		}
		if (this.m_caretObject != null && this.m_theScalar != null)
		{
			if (this.m_theCanvas != null)
			{
				Vector3 localScale = this.m_caretObject.transform.localScale;
				float num = localScale.y;
				if (this.m_theCanvas.renderMode == RenderMode.ScreenSpaceCamera)
				{
					num = (this.m_theScalar.referenceResolution.y / (float)Screen.height + this.m_theScalar.referenceResolution.x / (float)Screen.width) * 0.5f;
				}
				else if (this.m_theCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					num = 1f / this.m_theScalar.transform.localScale.y;
				}
				num *= this.m_originalYScaleAmt;
				this.m_caretObject.transform.localScale = new Vector3(localScale.x, num, localScale.z);
			}
		}
	}
}
