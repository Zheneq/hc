using System;
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
		this.m_icon.sprite = (Sprite)Resources.Load(reward.SpritePath, typeof(Sprite));
		UIManager.SetGameObjectActive(this.m_offContainer, currentLevel < reward.Level, null);
		UIManager.SetGameObjectActive(this.m_onContainer, currentLevel >= reward.Level, null);
		if (reward.isRepeating)
		{
			foreach (TextMeshProUGUI textMeshProUGUI in this.m_levelTexts)
			{
				textMeshProUGUI.text = string.Format("+{0}", reward.repeatLevels.ToString());
			}
			string text = RewardUtils.GetDisplayString(reward, false);
			if (reward.InventoryTemplate != null)
			{
				text = reward.InventoryTemplate.GetDisplayName();
			}
			foreach (TextMeshProUGUI textMeshProUGUI2 in this.m_descriptionTexts)
			{
				textMeshProUGUI2.text = text;
			}
		}
		else
		{
			foreach (TextMeshProUGUI textMeshProUGUI3 in this.m_levelTexts)
			{
				textMeshProUGUI3.text = reward.Level.ToString();
			}
			string text2 = RewardUtils.GetDisplayString(reward, false);
			if (reward.InventoryTemplate != null)
			{
				text2 = reward.InventoryTemplate.GetDisplayName();
			}
			foreach (TextMeshProUGUI textMeshProUGUI4 in this.m_descriptionTexts)
			{
				textMeshProUGUI4.text = text2;
			}
		}
	}
}
