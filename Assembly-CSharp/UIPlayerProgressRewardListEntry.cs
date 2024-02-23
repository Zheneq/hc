using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerProgressRewardListEntry : MonoBehaviour
{
	public RectTransform m_offContainer;

	public RectTransform m_onContainer;

	public Image m_icon;

	public TextMeshProUGUI[] m_levelTexts;

	public TextMeshProUGUI[] m_descriptionTexts;

	public void Setup(RewardUtils.RewardData reward, int currentLevel)
	{
		m_icon.sprite = (Sprite)Resources.Load(reward.SpritePath, typeof(Sprite));
		UIManager.SetGameObjectActive(m_offContainer, currentLevel < reward.Level);
		UIManager.SetGameObjectActive(m_onContainer, currentLevel >= reward.Level);
		if (reward.isRepeating)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					TextMeshProUGUI[] levelTexts = m_levelTexts;
					foreach (TextMeshProUGUI textMeshProUGUI in levelTexts)
					{
						textMeshProUGUI.text = new StringBuilder().Append("+").Append(reward.repeatLevels.ToString()).ToString();
					}
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
						{
							string text = RewardUtils.GetDisplayString(reward);
							if (reward.InventoryTemplate != null)
							{
								text = reward.InventoryTemplate.GetDisplayName();
							}
							TextMeshProUGUI[] descriptionTexts = m_descriptionTexts;
							foreach (TextMeshProUGUI textMeshProUGUI2 in descriptionTexts)
							{
								textMeshProUGUI2.text = text;
							}
							while (true)
							{
								switch (6)
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
				}
				}
			}
		}
		TextMeshProUGUI[] levelTexts2 = m_levelTexts;
		foreach (TextMeshProUGUI textMeshProUGUI3 in levelTexts2)
		{
			textMeshProUGUI3.text = reward.Level.ToString();
		}
		while (true)
		{
			string text2 = RewardUtils.GetDisplayString(reward);
			if (reward.InventoryTemplate != null)
			{
				text2 = reward.InventoryTemplate.GetDisplayName();
			}
			TextMeshProUGUI[] descriptionTexts2 = m_descriptionTexts;
			foreach (TextMeshProUGUI textMeshProUGUI4 in descriptionTexts2)
			{
				textMeshProUGUI4.text = text2;
			}
			while (true)
			{
				switch (2)
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
