using System.Text;
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
		m_inputField = base.gameObject.GetComponent<InputField>();
		m_theScalar = base.gameObject.GetComponentInParent<CanvasScaler>();
		Vector3 localScale = m_inputField.textComponent.transform.localScale;
		m_originalYScaleAmt = localScale.y;
	}

	public void Update()
	{
		if (m_caretObject == null)
		{
			if (base.gameObject.transform.Find(new StringBuilder().Append(base.gameObject.name).Append(" Input Caret").ToString()) != null)
			{
				m_caretObject = (base.gameObject.transform.Find(new StringBuilder().Append(base.gameObject.name).Append(" Input Caret").ToString()).transform as RectTransform);
			}
		}
		if (m_theScalar == null)
		{
			m_theScalar = base.gameObject.GetComponentInParent<CanvasScaler>();
		}
		if (m_theCanvas == null)
		{
			m_theCanvas = m_theScalar.GetComponent<Canvas>();
		}
		if (!(m_caretObject != null) || !(m_theScalar != null))
		{
			return;
		}
		while (true)
		{
			if (!(m_theCanvas != null))
			{
				return;
			}
			while (true)
			{
				Vector3 localScale = m_caretObject.transform.localScale;
				float num = localScale.y;
				if (m_theCanvas.renderMode == RenderMode.ScreenSpaceCamera)
				{
					Vector2 referenceResolution = m_theScalar.referenceResolution;
					float num2 = referenceResolution.y / (float)Screen.height;
					Vector2 referenceResolution2 = m_theScalar.referenceResolution;
					num = (num2 + referenceResolution2.x / (float)Screen.width) * 0.5f;
				}
				else if (m_theCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					Vector3 localScale2 = m_theScalar.transform.localScale;
					num = 1f / localScale2.y;
				}
				num *= m_originalYScaleAmt;
				m_caretObject.transform.localScale = new Vector3(localScale.x, num, localScale.z);
				return;
			}
		}
	}
}
