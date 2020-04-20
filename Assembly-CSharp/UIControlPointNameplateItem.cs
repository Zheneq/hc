using System;
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
			UIManager.SetGameObjectActive(gameObject, visible, null);
		}
	}

	private void Update()
	{
		Vector3 guiposition = this.m_controlPoint.GetGUIPosition(this.m_verticalOffset);
		Vector3 vector;
		if (Camera.main != null)
		{
			vector = Camera.main.WorldToViewportPoint(guiposition);
		}
		else
		{
			vector = guiposition;
		}
		bool flag;
		if (vector.z >= 0f)
		{
			if (this.m_controlPoint.CurrentControlPointState != ControlPoint.State.Disabled)
			{
				flag = true;
				goto IL_76;
			}
		}
		flag = false;
		IL_76:
		this.SetVisible(flag);
		if (flag)
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			RectTransform rectTransform = componentInParent.transform as RectTransform;
			Vector2 anchoredPosition = new Vector2(vector.x * rectTransform.sizeDelta.x - rectTransform.sizeDelta.x * 0.5f, vector.y * rectTransform.sizeDelta.y - rectTransform.sizeDelta.y * 0.5f);
			(base.gameObject.transform as RectTransform).anchoredPosition = anchoredPosition;
			this.m_controlPoint.SetupRectNameplate(ref this.m_controllerLabel, ref this.m_progressLabel, ref this.m_bar);
		}
	}

	public void Setup(ControlPoint controlPoint)
	{
		this.m_controlPoint = controlPoint;
		this.m_nameLabel.text = this.m_controlPoint.m_displayName;
	}
}
