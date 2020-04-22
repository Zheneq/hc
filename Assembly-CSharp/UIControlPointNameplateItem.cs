using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControlPointNameplateItem : MonoBehaviour
{
	public TextMeshProUGUI m_nameLabel;

	public TextMeshProUGUI m_controllerLabel;

	public TextMeshProUGUI m_progressLabel;

	public Slider m_bar;

	public float m_verticalOffset = 30f;

	private ControlPoint m_controlPoint;

	private void SetVisible(bool visible)
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			GameObject gameObject = base.transform.GetChild(i).gameObject;
			UIManager.SetGameObjectActive(gameObject, visible);
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	private void Update()
	{
		Vector3 gUIPosition = m_controlPoint.GetGUIPosition(m_verticalOffset);
		Vector3 vector = (!(Camera.main != null)) ? gUIPosition : Camera.main.WorldToViewportPoint(gUIPosition);
		bool flag;
		if (!(vector.z < 0f))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_controlPoint.CurrentControlPointState != ControlPoint.State.Disabled)
			{
				flag = true;
				goto IL_0076;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		flag = false;
		goto IL_0076;
		IL_0076:
		SetVisible(flag);
		if (!flag)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			Canvas componentInParent = GetComponentInParent<Canvas>();
			RectTransform rectTransform = componentInParent.transform as RectTransform;
			float x = vector.x;
			Vector2 sizeDelta = rectTransform.sizeDelta;
			float num = x * sizeDelta.x;
			Vector2 sizeDelta2 = rectTransform.sizeDelta;
			float x2 = num - sizeDelta2.x * 0.5f;
			float y = vector.y;
			Vector2 sizeDelta3 = rectTransform.sizeDelta;
			float num2 = y * sizeDelta3.y;
			Vector2 sizeDelta4 = rectTransform.sizeDelta;
			Vector2 anchoredPosition = new Vector2(x2, num2 - sizeDelta4.y * 0.5f);
			(base.gameObject.transform as RectTransform).anchoredPosition = anchoredPosition;
			m_controlPoint.SetupRectNameplate(ref m_controllerLabel, ref m_progressLabel, ref m_bar);
			return;
		}
	}

	public void Setup(ControlPoint controlPoint)
	{
		m_controlPoint = controlPoint;
		m_nameLabel.text = m_controlPoint.m_displayName;
	}
}
