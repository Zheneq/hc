using System.Collections.Generic;
using UnityEngine;

public class UIControlPointNameplatePanel : MonoBehaviour
{
	public UIControlPointNameplateItem m_controlPointNameplateItemPrefab;

	private Dictionary<ControlPoint, UIControlPointNameplateItem> m_controlPointNameplates = new Dictionary<ControlPoint, UIControlPointNameplateItem>();

	public void AddControlPoint(ControlPoint controlPoint)
	{
		UIControlPointNameplateItem uIControlPointNameplateItem = Object.Instantiate(m_controlPointNameplateItemPrefab);
		uIControlPointNameplateItem.Setup(controlPoint);
		m_controlPointNameplates[controlPoint] = uIControlPointNameplateItem;
		uIControlPointNameplateItem.transform.SetParent(base.transform);
		(uIControlPointNameplateItem.transform as RectTransform).localScale = Vector3.one;
		(uIControlPointNameplateItem.transform as RectTransform).localPosition = Vector3.zero;
	}

	public void RemoveControlPoint(ControlPoint controlPoint)
	{
		if (!m_controlPointNameplates.ContainsKey(controlPoint))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIControlPointNameplateItem uIControlPointNameplateItem = m_controlPointNameplates[controlPoint];
			m_controlPointNameplates.Remove(controlPoint);
			if (uIControlPointNameplateItem != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					Object.Destroy(uIControlPointNameplateItem.gameObject);
					return;
				}
			}
			return;
		}
	}
}
