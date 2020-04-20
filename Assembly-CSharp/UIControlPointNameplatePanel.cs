using System;
using System.Collections.Generic;
using UnityEngine;

public class UIControlPointNameplatePanel : MonoBehaviour
{
	public UIControlPointNameplateItem m_controlPointNameplateItemPrefab;

	private Dictionary<ControlPoint, UIControlPointNameplateItem> m_controlPointNameplates = new Dictionary<ControlPoint, UIControlPointNameplateItem>();

	public void AddControlPoint(ControlPoint controlPoint)
	{
		UIControlPointNameplateItem uicontrolPointNameplateItem = UnityEngine.Object.Instantiate<UIControlPointNameplateItem>(this.m_controlPointNameplateItemPrefab);
		uicontrolPointNameplateItem.Setup(controlPoint);
		this.m_controlPointNameplates[controlPoint] = uicontrolPointNameplateItem;
		uicontrolPointNameplateItem.transform.SetParent(base.transform);
		(uicontrolPointNameplateItem.transform as RectTransform).localScale = Vector3.one;
		(uicontrolPointNameplateItem.transform as RectTransform).localPosition = Vector3.zero;
	}

	public void RemoveControlPoint(ControlPoint controlPoint)
	{
		if (this.m_controlPointNameplates.ContainsKey(controlPoint))
		{
			UIControlPointNameplateItem uicontrolPointNameplateItem = this.m_controlPointNameplates[controlPoint];
			this.m_controlPointNameplates.Remove(controlPoint);
			if (uicontrolPointNameplateItem != null)
			{
				UnityEngine.Object.Destroy(uicontrolPointNameplateItem.gameObject);
			}
		}
	}
}
