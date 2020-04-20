using System;
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
		if (this.m_progressionSlider != null)
		{
			this.m_progressionSlider.interactable = false;
		}
		if (this.m_theButton != null)
		{
			this.m_theButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnSkinClicked);
		}
	}

	public void OnSkinClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
		this.m_uiSkinBrowserPanel.SkinClicked(this);
	}

	public void Setup(CharacterResourceLink m_theCharacter, UISkinData skinData, int skinIndex, UISkinBrowserPanel parent)
	{
		this.m_skinIndex = skinIndex;
		this.m_skinData = skinData;
		UIManager.SetGameObjectActive(this.m_lockedIcon, !skinData.m_isAvailable, null);
		this.m_uiSkinBrowserPanel = parent;
		if (this.m_characterIcon != null)
		{
			if (skinData.m_skinImage == null)
			{
				this.m_characterIcon.sprite = m_theCharacter.GetLoadingProfileIcon();
			}
			else
			{
				this.m_characterIcon.sprite = skinData.m_skinImage;
			}
		}
		if (this.m_progressionSlider != null)
		{
			this.m_progressionSlider.minValue = 0f;
			this.m_progressionSlider.maxValue = 1f;
			this.m_progressionSlider.value = skinData.m_progressPct;
		}
		this.m_unlockTooltipTitle = string.Format(StringUtil.TR("SkinName", "Global"), m_theCharacter.GetSkinName(skinIndex));
		this.m_unlockTooltipText = m_theCharacter.GetSkinDescription(skinIndex);
		if (this.m_unlockTooltipText.IsNullOrEmpty())
		{
			this.m_unlockTooltipText = string.Empty;
			int num = 0;
			if (skinData.m_unlockCharacterLevel > 1)
			{
				this.m_unlockTooltipText = this.m_unlockTooltipText + string.Format(StringUtil.TR("UnlockedAtCharacterLevel", "Global"), skinData.m_unlockCharacterLevel) + Environment.NewLine;
				num++;
			}
			if (skinData.m_gameCurrencyCost > 0)
			{
				this.m_unlockTooltipText = this.m_unlockTooltipText + string.Format(StringUtil.TR("BuyForNumberISO", "Global"), skinData.m_gameCurrencyCost) + Environment.NewLine;
				num++;
			}
			if (num > 1)
			{
				this.m_unlockTooltipText = StringUtil.TR("ObtainedByMethods", "Global") + Environment.NewLine + this.m_unlockTooltipText + Environment.NewLine;
			}
		}
		else
		{
			this.m_unlockTooltipText += Environment.NewLine;
		}
		if (!skinData.m_flavorText.IsNullOrEmpty())
		{
			this.m_unlockTooltipText = this.m_unlockTooltipText + "<i>" + skinData.m_flavorText + "</i>";
		}
	}
}
