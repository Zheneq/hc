using System;
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(StatusEffectTooltip.Awake()).MethodHandle;
			}
			for (int i = 0; i < 0x3A; i++)
			{
				HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType((StatusType)i);
				if (iconForStatusType.displayInStatusList)
				{
					for (;;)
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
			for (;;)
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
			StatusEffectTooltipEntry statusEffectTooltipEntry = UnityEngine.Object.Instantiate<StatusEffectTooltipEntry>(this.m_entryPrefab);
			statusEffectTooltipEntry.transform.SetParent(this.m_layoutGroup.transform);
			statusEffectTooltipEntry.transform.localEulerAngles = Vector3.zero;
			statusEffectTooltipEntry.transform.localPosition = Vector3.zero;
			statusEffectTooltipEntry.transform.localScale = Vector3.one;
			statusEffectTooltipEntry.m_statusEffectImage.sprite = statusTypeIcon.icon;
			statusEffectTooltipEntry.m_statusEffectText.text = string.Format("<color=orange>{0}</color> - {1}", statusTypeIcon.buffName, statusTypeIcon.buffDescription);
			statusEffectTooltipEntry.m_layoutElement.preferredHeight = statusEffectTooltipEntry.m_statusEffectText.GetPreferredValues().y;
			num += statusEffectTooltipEntry.m_layoutElement.preferredHeight + 4f;
			if (j + 1 == list.Count)
			{
				for (;;)
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
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int k = 0; k < list2.Count; k++)
		{
			HUD_UIResources.StatusTypeIcon statusTypeIcon2 = list2[k];
			StatusEffectTooltipEntry statusEffectTooltipEntry2 = UnityEngine.Object.Instantiate<StatusEffectTooltipEntry>(this.m_entryPrefab);
			statusEffectTooltipEntry2.transform.SetParent(this.m_layoutGroup.transform);
			statusEffectTooltipEntry2.transform.localEulerAngles = Vector3.zero;
			statusEffectTooltipEntry2.transform.localPosition = Vector3.zero;
			statusEffectTooltipEntry2.transform.localScale = Vector3.one;
			statusEffectTooltipEntry2.m_statusEffectImage.sprite = statusTypeIcon2.icon;
			statusEffectTooltipEntry2.m_statusEffectText.text = string.Format("<color=orange>{0}</color> - {1}", statusTypeIcon2.buffName, statusTypeIcon2.buffDescription);
			statusEffectTooltipEntry2.m_layoutElement.preferredHeight = statusEffectTooltipEntry2.m_statusEffectText.GetPreferredValues().y;
			num += statusEffectTooltipEntry2.m_layoutElement.preferredHeight + 4f;
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		num += 4f;
		(base.gameObject.transform as RectTransform).sizeDelta = new Vector2((base.gameObject.transform as RectTransform).sizeDelta.x, num);
	}
}
