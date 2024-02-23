using System;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkinSelectButton : UICharacterVisualsSelectButton
{
	public Image m_characterIcon;

	public Slider m_progressionSlider;

	public _ButtonSwapSprite m_theButton;

	public int m_skinIndex;

	public UISkinData m_skinData;

	private UISkinBrowserPanel m_uiSkinBrowserPanel;

	protected override void Start()
	{
		base.Start();
		if (m_progressionSlider != null)
		{
			m_progressionSlider.interactable = false;
		}
		if (!(m_theButton != null))
		{
			return;
		}
		while (true)
		{
			m_theButton.callback = OnSkinClicked;
			return;
		}
	}

	public void OnSkinClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
		m_uiSkinBrowserPanel.SkinClicked(this);
	}

	public void Setup(CharacterResourceLink m_theCharacter, UISkinData skinData, int skinIndex, UISkinBrowserPanel parent)
	{
		m_skinIndex = skinIndex;
		m_skinData = skinData;
		UIManager.SetGameObjectActive(m_lockedIcon, !skinData.m_isAvailable);
		m_uiSkinBrowserPanel = parent;
		if (m_characterIcon != null)
		{
			if (skinData.m_skinImage == null)
			{
				m_characterIcon.sprite = m_theCharacter.GetLoadingProfileIcon();
			}
			else
			{
				m_characterIcon.sprite = skinData.m_skinImage;
			}
		}
		if (m_progressionSlider != null)
		{
			m_progressionSlider.minValue = 0f;
			m_progressionSlider.maxValue = 1f;
			m_progressionSlider.value = skinData.m_progressPct;
		}
		m_unlockTooltipTitle = string.Format(StringUtil.TR("SkinName", "Global"), m_theCharacter.GetSkinName(skinIndex));
		m_unlockTooltipText = m_theCharacter.GetSkinDescription(skinIndex);
		if (m_unlockTooltipText.IsNullOrEmpty())
		{
			m_unlockTooltipText = string.Empty;
			int num = 0;
			if (skinData.m_unlockCharacterLevel > 1)
			{
				m_unlockTooltipText = new StringBuilder().Append(m_unlockTooltipText).AppendLine(string.Format(StringUtil.TR("UnlockedAtCharacterLevel", "Global"), skinData.m_unlockCharacterLevel)).ToString();
				num++;
			}
			if (skinData.m_gameCurrencyCost > 0)
			{
				m_unlockTooltipText = new StringBuilder().Append(m_unlockTooltipText).AppendLine(string.Format(StringUtil.TR("BuyForNumberISO", "Global"), skinData.m_gameCurrencyCost)).ToString();
				num++;
			}
			if (num > 1)
			{
				m_unlockTooltipText = new StringBuilder().AppendLine(StringUtil.TR("ObtainedByMethods", "Global")).AppendLine(m_unlockTooltipText).ToString();
			}
		}
		else
		{
			m_unlockTooltipText += Environment.NewLine;
		}
		if (skinData.m_flavorText.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			m_unlockTooltipText = new StringBuilder().Append(m_unlockTooltipText).Append("<i>").Append(skinData.m_flavorText).Append("</i>").ToString();
			return;
		}
	}
}
