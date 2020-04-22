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
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (base.gameObject.transform.Find(base.gameObject.name + " Input Caret") != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				m_caretObject = (base.gameObject.transform.Find(base.gameObject.name + " Input Caret").transform as RectTransform);
			}
		}
		if (m_theScalar == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			m_theScalar = base.gameObject.GetComponentInParent<CanvasScaler>();
		}
		if (m_theCanvas == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			m_theCanvas = m_theScalar.GetComponent<Canvas>();
		}
		if (!(m_caretObject != null) || !(m_theScalar != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (!(m_theCanvas != null))
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				Vector3 localScale = m_caretObject.transform.localScale;
				float num = localScale.y;
				if (m_theCanvas.renderMode == RenderMode.ScreenSpaceCamera)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
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
