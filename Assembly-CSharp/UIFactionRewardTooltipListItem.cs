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
			while (true)
			{
				if (!enumerator.MoveNext())
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					break;
				}
				int current = enumerator.Current;
				if (i >= m_rewardImages.Length)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							goto end_IL_000b;
						}
					}
				}
				InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(current);
				if (itemTemplate == null)
				{
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
				else if (itemTemplate != null)
				{
					m_rewardImages[i].sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate));
					UIManager.SetGameObjectActive(m_rewardContainers[i], true);
					i++;
				}
			}
			end_IL_000b:;
		}
		for (; i < m_rewardImages.Length; i++)
		{
			UIManager.SetGameObjectActive(m_rewardContainers[i], false);
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			UIManager.SetGameObjectActive(m_obtainedContainer, obtained);
			if (obtained)
			{
				m_characterListHighlight.alpha = 1f;
			}
			else
			{
				m_characterListHighlight.alpha = 0.3f;
			}
			for (int j = 0; j < m_levelText.Length; j++)
			{
				m_levelText[j].text = (rewardTeirLevel + 1).ToString();
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}
}
