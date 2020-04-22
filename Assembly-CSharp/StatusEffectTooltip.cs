using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectTooltip : MonoBehaviour
{
	public LayoutGroup m_layoutGroup;

	public StatusEffectTooltipEntry m_entryPrefab;

	public void Awake()
	{
		float num = 0f;
		List<HUD_UIResources.StatusTypeIcon> list = new List<HUD_UIResources.StatusTypeIcon>();
		List<HUD_UIResources.StatusTypeIcon> list2 = new List<HUD_UIResources.StatusTypeIcon>();
		if (HUD_UIResources.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < 58; i++)
			{
				HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType((StatusType)i);
				if (iconForStatusType.displayInStatusList)
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
					if (iconForStatusType.isDebuff)
					{
						list2.Add(iconForStatusType);
					}
					else
					{
						list.Add(iconForStatusType);
					}
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			HUD_UIResources.StatusTypeIcon statusTypeIcon = list[j];
			StatusEffectTooltipEntry statusEffectTooltipEntry = Object.Instantiate(m_entryPrefab);
			statusEffectTooltipEntry.transform.SetParent(m_layoutGroup.transform);
			statusEffectTooltipEntry.transform.localEulerAngles = Vector3.zero;
			statusEffectTooltipEntry.transform.localPosition = Vector3.zero;
			statusEffectTooltipEntry.transform.localScale = Vector3.one;
			statusEffectTooltipEntry.m_statusEffectImage.sprite = statusTypeIcon.icon;
			statusEffectTooltipEntry.m_statusEffectText.text = $"<color=orange>{statusTypeIcon.buffName}</color> - {statusTypeIcon.buffDescription}";
			LayoutElement layoutElement = statusEffectTooltipEntry.m_layoutElement;
			Vector2 preferredValues = statusEffectTooltipEntry.m_statusEffectText.GetPreferredValues();
			layoutElement.preferredHeight = preferredValues.y;
			num += statusEffectTooltipEntry.m_layoutElement.preferredHeight + 4f;
			if (j + 1 == list.Count)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				statusEffectTooltipEntry.m_layoutElement.preferredHeight += 20f;
				num += 20f;
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			for (int k = 0; k < list2.Count; k++)
			{
				HUD_UIResources.StatusTypeIcon statusTypeIcon2 = list2[k];
				StatusEffectTooltipEntry statusEffectTooltipEntry2 = Object.Instantiate(m_entryPrefab);
				statusEffectTooltipEntry2.transform.SetParent(m_layoutGroup.transform);
				statusEffectTooltipEntry2.transform.localEulerAngles = Vector3.zero;
				statusEffectTooltipEntry2.transform.localPosition = Vector3.zero;
				statusEffectTooltipEntry2.transform.localScale = Vector3.one;
				statusEffectTooltipEntry2.m_statusEffectImage.sprite = statusTypeIcon2.icon;
				statusEffectTooltipEntry2.m_statusEffectText.text = $"<color=orange>{statusTypeIcon2.buffName}</color> - {statusTypeIcon2.buffDescription}";
				LayoutElement layoutElement2 = statusEffectTooltipEntry2.m_layoutElement;
				Vector2 preferredValues2 = statusEffectTooltipEntry2.m_statusEffectText.GetPreferredValues();
				layoutElement2.preferredHeight = preferredValues2.y;
				num += statusEffectTooltipEntry2.m_layoutElement.preferredHeight + 4f;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				num += 4f;
				RectTransform obj = base.gameObject.transform as RectTransform;
				Vector2 sizeDelta = (base.gameObject.transform as RectTransform).sizeDelta;
				obj.sizeDelta = new Vector2(sizeDelta.x, num);
				return;
			}
		}
	}
}
