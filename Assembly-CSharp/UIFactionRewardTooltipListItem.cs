using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFactionRewardTooltipListItem : MonoBehaviour
{
	public RectTransform[] m_rewardContainers;

	public Image[] m_rewardImages;

	public TextMeshProUGUI[] m_levelText;

	public CanvasGroup m_characterListHighlight;

	public RectTransform m_obtainedContainer;

	public void Setup(List<int> itemRewardList, int rewardTeirLevel, bool obtained)
	{
		int i = 0;
		using (List<int>.Enumerator enumerator = itemRewardList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int templateId = enumerator.Current;
				if (i >= this.m_rewardImages.Length)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIFactionRewardTooltipListItem.Setup(List<int>, int, bool)).MethodHandle;
					}
					goto IL_AD;
				}
				InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(templateId);
				if (itemTemplate == null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				else if (itemTemplate != null)
				{
					this.m_rewardImages[i].sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate));
					UIManager.SetGameObjectActive(this.m_rewardContainers[i], true, null);
					i++;
				}
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
		}
		IL_AD:
		while (i < this.m_rewardImages.Length)
		{
			UIManager.SetGameObjectActive(this.m_rewardContainers[i], false, null);
			i++;
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
		UIManager.SetGameObjectActive(this.m_obtainedContainer, obtained, null);
		if (obtained)
		{
			this.m_characterListHighlight.alpha = 1f;
		}
		else
		{
			this.m_characterListHighlight.alpha = 0.3f;
		}
		for (int j = 0; j < this.m_levelText.Length; j++)
		{
			this.m_levelText[j].text = (rewardTeirLevel + 1).ToString();
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
	}
}
