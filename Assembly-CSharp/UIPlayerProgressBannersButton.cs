using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerProgressBannersButton : MonoBehaviour
{
	public Image m_bannerImage;

	public GameObject m_selected;

	public GameObject m_selectedMain;

	public Image m_lockIcon;

	public Image m_factionIcon;

	public TextMeshProUGUI m_titleLabel;

	public TextMeshProUGUI m_incompleteRequirementLabel;

	public TextMeshProUGUI m_completeRequirementLabel;

	public TextMeshProUGUI m_requirementProgressLabel;

	public Image m_progressSlider;

	public _SelectableBtn m_selectableButton;

	public bool m_unlocked;

	private UIPlayerProgressBanners.CurrentList m_type;

	protected int m_selectedID;

	private bool m_valid = true;

	private string m_name = string.Empty;

	private string m_description = string.Empty;

	private UITooltipHoverObject m_tooltipHoverObj;

	private void Start()
	{
		this.m_selectableButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BannerClicked);
		this.m_tooltipHoverObj = this.m_selectableButton.spriteController.GetComponent<UITooltipHoverObject>();
		this.m_tooltipHoverObj.Setup(TooltipType.Titled, new TooltipPopulateCall(this.BannerTooltipSetup), null);
		if (this.m_factionIcon != null)
		{
			ClientGameManager.Get().OnFactionCompetitionNotification += this.OnFactionCompetitionNotification;
		}
	}

	private void OnDestroy()
	{
		if (this.m_factionIcon != null)
		{
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager.Get().OnFactionCompetitionNotification -= this.OnFactionCompetitionNotification;
			}
		}
	}

	private void OnFactionCompetitionNotification(FactionCompetitionNotification notification)
	{
		int activeFactionCompetition = ClientGameManager.Get().ActiveFactionCompetition;
		FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(activeFactionCompetition);
		if (factionCompetition != null)
		{
			for (int i = 0; i < factionCompetition.Factions.Count; i++)
			{
				for (int j = 0; j < factionCompetition.Factions[i].BannerIds.Count; j++)
				{
					if (factionCompetition.Factions[i].BannerIds[j] == this.m_selectedID)
					{
						UIManager.SetGameObjectActive(this.m_factionIcon, true, null);
						FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(factionCompetition.Factions[i].FactionGroupIDToUse);
						this.m_factionIcon.sprite = Resources.Load<Sprite>(factionGroup.BannerIconPath);
						return;
					}
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_factionIcon, false, null);
	}

	public void BannerClicked(BaseEventData data)
	{
		if (this.m_unlocked && this.m_valid)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
			UIPlayerProgressPanel.Get().m_bannersPanel.BannerClicked(this);
			if (this.m_type == UIPlayerProgressBanners.CurrentList.Title)
			{
				ClientGameManager clientGameManager = ClientGameManager.Get();
				int selectedID = this.m_selectedID;
				
				clientGameManager.RequestTitleSelect(selectedID, delegate(SelectTitleResponse response)
					{
						if (!response.Success)
						{
							Log.Error("Title change request was rejected: " + response.ErrorMessage, new object[0]);
							UIPlayerProgressPanel.Get().m_bannersPanel.ResetPage();
							ClientGameManager.Get().UpdatePlayerStatus(FriendListPanel.Get().m_panelHeader.m_statusLabels[0].text);
						}
					});
			}
			else
			{
				if (this.m_type != UIPlayerProgressBanners.CurrentList.Background)
				{
					if (this.m_type != UIPlayerProgressBanners.CurrentList.Foreground)
					{
						if (this.m_type == UIPlayerProgressBanners.CurrentList.Ribbon)
						{
							ClientGameManager.Get().RequestRibbonSelect(this.m_selectedID, delegate(SelectRibbonResponse response)
							{
								if (!response.Success)
								{
									Log.Error("Ribbon change request was rejected: " + response.ErrorMessage, new object[0]);
									UIPlayerProgressPanel.Get().m_bannersPanel.ResetPage();
									ClientGameManager.Get().UpdatePlayerStatus(FriendListPanel.Get().m_panelHeader.m_statusLabels[0].text);
								}
							});
							return;
						}
						return;
					}
				}
				ClientGameManager clientGameManager2 = ClientGameManager.Get();
				int selectedID2 = this.m_selectedID;
				
				clientGameManager2.RequestBannerSelect(selectedID2, delegate(SelectBannerResponse response)
					{
						if (!response.Success)
						{
							Log.Error("Banner change request was rejected: " + response.ErrorMessage, new object[0]);
							UIPlayerProgressPanel.Get().m_bannersPanel.ResetPage();
							ClientGameManager.Get().UpdatePlayerStatus(FriendListPanel.Get().m_panelHeader.m_statusLabels[0].text);
						}
					});
			}
		}
	}

	private bool BannerTooltipSetup(UITooltipBase tooltip)
	{
		if (!this.m_name.IsNullOrEmpty())
		{
			UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
			uititledTooltip.Setup(this.m_name, this.m_description, string.Empty);
			return true;
		}
		return false;
	}

	public void SetSelected(bool isSelected)
	{
		UIManager.SetGameObjectActive(this.m_selected, isSelected, null);
		UIManager.SetGameObjectActive(this.m_selectedMain, isSelected, null);
	}

	public void SetupTitle(GameBalanceVars.PlayerTitle title)
	{
		this.m_type = UIPlayerProgressBanners.CurrentList.Title;
		if (this.m_bannerImage != null)
		{
			UIManager.SetGameObjectActive(this.m_bannerImage, false, null);
		}
		if (title == null)
		{
			this.m_selectedID = -1;
			this.m_titleLabel.text = StringUtil.TR("TitleNone", "Global");
			this.m_unlocked = true;
			UIManager.SetGameObjectActive(this.m_lockIcon, false, null);
			UIManager.SetGameObjectActive(this.m_progressSlider, false, null);
			UIManager.SetGameObjectActive(this.m_incompleteRequirementLabel.transform.parent, false, null);
			UIManager.SetGameObjectActive(this.m_completeRequirementLabel.transform.parent, true, null);
			this.m_completeRequirementLabel.text = string.Empty;
			this.m_selectableButton.spriteController.ResetMouseState();
			this.m_selectableButton.m_ignoreHoverAnimationCall = false;
			this.m_selectableButton.m_ignorePressAnimationCall = false;
			this.m_name = string.Empty;
			this.m_description = string.Empty;
		}
		else
		{
			this.m_selectedID = title.ID;
			this.m_titleLabel.text = title.GetTitleText(-1);
			List<GameBalanceVars.UnlockConditionValue> unlockConditionValues;
			this.m_unlocked = ClientGameManager.Get().IsTitleUnlocked(title, out unlockConditionValues);
			this.SetDisplay(title.m_unlockData, unlockConditionValues, false);
			this.m_name = title.GetTitleText(-1);
			if (!title.GetObtainedDescription().IsNullOrEmpty())
			{
				this.m_description = title.GetObtainedDescription();
			}
		}
		UIManager.SetGameObjectActive(this.m_titleLabel, true, null);
		this.m_valid = true;
		this.SetSelected(ClientGameManager.Get().GetCurrentTitleID() == this.m_selectedID);
		if (this.m_tooltipHoverObj != null)
		{
			this.m_tooltipHoverObj.Refresh();
		}
	}

	public void SetupBanner(GameBalanceVars.PlayerBanner banner)
	{
		List<GameBalanceVars.UnlockConditionValue> unlockConditionValues = null;
		if (banner == null)
		{
			this.m_selectedID = -1;
			if (this.m_factionIcon != null)
			{
				UIManager.SetGameObjectActive(this.m_factionIcon, false, null);
			}
			this.m_unlocked = true;
			this.m_valid = false;
			if (this.m_bannerImage != null)
			{
				UIManager.SetGameObjectActive(this.m_bannerImage, false, null);
			}
			if (this.m_titleLabel != null)
			{
				UIManager.SetGameObjectActive(this.m_titleLabel, false, null);
			}
			this.SetDisplay(null, unlockConditionValues, true);
			this.SetSelected(false);
			this.m_name = string.Empty;
		}
		else
		{
			UIPlayerProgressBanners.CurrentList type;
			if (banner.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
			{
				type = UIPlayerProgressBanners.CurrentList.Background;
			}
			else
			{
				type = UIPlayerProgressBanners.CurrentList.Foreground;
			}
			this.m_type = type;
			this.m_selectedID = banner.ID;
			Sprite sprite = (Sprite)Resources.Load(banner.m_iconResourceString, typeof(Sprite));
			if (sprite == null)
			{
				Log.Warning(Log.Category.UI, string.Format("Could not load banner resource from [{0}] as sprite.", banner.m_iconResourceString), new object[0]);
			}
			this.m_bannerImage.sprite = sprite;
			UIManager.SetGameObjectActive(this.m_bannerImage, true, null);
			this.m_unlocked = ClientGameManager.Get().IsBannerUnlocked(banner, out unlockConditionValues);
			this.SetDisplay(banner.m_unlockData, unlockConditionValues, true);
			if (this.m_factionIcon != null)
			{
				this.OnFactionCompetitionNotification(null);
				float num;
				if (this.m_unlocked)
				{
					num = 1f;
				}
				else
				{
					num = 0.25f;
				}
				float num2 = num;
				this.m_factionIcon.color = new Color(num2, num2, num2);
			}
			GameBalanceVars.PlayerBanner playerBanner;
			if (this.m_type == UIPlayerProgressBanners.CurrentList.Background)
			{
				playerBanner = ClientGameManager.Get().GetCurrentBackgroundBanner();
			}
			else
			{
				playerBanner = ClientGameManager.Get().GetCurrentForegroundBanner();
			}
			bool selected;
			if (playerBanner != null)
			{
				selected = (banner.ID == playerBanner.ID);
			}
			else
			{
				selected = false;
			}
			this.SetSelected(selected);
			this.m_valid = true;
			string colorHexString = banner.Rarity.GetColorHexString();
			this.m_name = string.Concat(new string[]
			{
				"<color=",
				colorHexString,
				">",
				banner.GetBannerName(),
				"</color>"
			});
			if (!banner.GetObtainedDescription().IsNullOrEmpty())
			{
				this.m_description = banner.GetObtainedDescription();
			}
		}
		this.m_requirementProgressLabel.text = string.Empty;
		TMP_Text incompleteRequirementLabel = this.m_incompleteRequirementLabel;
		string name = this.m_name;
		this.m_completeRequirementLabel.text = name;
		incompleteRequirementLabel.text = name;
		if (this.m_tooltipHoverObj != null)
		{
			this.m_tooltipHoverObj.Refresh();
		}
	}

	public void SetupRibbon(GameBalanceVars.PlayerRibbon ribbon)
	{
		List<GameBalanceVars.UnlockConditionValue> unlockConditionValues = null;
		this.m_type = UIPlayerProgressBanners.CurrentList.Ribbon;
		this.m_selectableButton.m_ignoreHoverAnimationCall = false;
		this.m_selectableButton.m_ignorePressAnimationCall = false;
		UIManager.SetGameObjectActive(this.m_progressSlider, false, null);
		UIManager.SetGameObjectActive(this.m_incompleteRequirementLabel.transform.parent, false, null);
		UIManager.SetGameObjectActive(this.m_completeRequirementLabel.transform.parent, false, null);
		this.m_completeRequirementLabel.text = string.Empty;
		this.m_selectableButton.spriteController.ResetMouseState();
		if (ribbon == null)
		{
			this.m_selectedID = -1;
			if (this.m_factionIcon != null)
			{
				UIManager.SetGameObjectActive(this.m_factionIcon, false, null);
			}
			this.m_unlocked = true;
			this.m_valid = true;
			if (this.m_bannerImage != null)
			{
				this.m_bannerImage.sprite = Resources.Load<Sprite>("QuestRewards/unequipIcon");
				UIManager.SetGameObjectActive(this.m_bannerImage, true, null);
			}
			if (this.m_titleLabel != null)
			{
				UIManager.SetGameObjectActive(this.m_titleLabel, false, null);
			}
			GameBalanceVars.PlayerRibbon currentRibbon = ClientGameManager.Get().GetCurrentRibbon();
			bool selected;
			if (currentRibbon != null)
			{
				selected = (currentRibbon.ID == -1);
			}
			else
			{
				selected = true;
			}
			this.SetSelected(selected);
			UIManager.SetGameObjectActive(this.m_lockIcon, false, null);
			this.m_name = StringUtil.TR("TitleNone", "Global");
			this.m_description = string.Empty;
		}
		else
		{
			this.m_selectedID = ribbon.ID;
			Sprite sprite = Resources.Load<Sprite>(ribbon.m_resourceIconString);
			if (sprite == null)
			{
				Log.Warning(Log.Category.UI, string.Format("Could not load banner resource from [{0}] as sprite.", ribbon.m_resourceIconString), new object[0]);
			}
			this.m_bannerImage.sprite = sprite;
			UIManager.SetGameObjectActive(this.m_bannerImage, true, null);
			this.m_unlocked = ClientGameManager.Get().IsRibbonUnlocked(ribbon, out unlockConditionValues);
			this.SetDisplay(ribbon.m_unlockData, unlockConditionValues, false);
			if (this.m_factionIcon != null)
			{
				UIManager.SetGameObjectActive(this.m_factionIcon, false, null);
			}
			GameBalanceVars.PlayerRibbon currentRibbon2 = ClientGameManager.Get().GetCurrentRibbon();
			this.SetSelected(currentRibbon2 != null && ribbon.ID == currentRibbon2.ID);
			this.m_valid = true;
			this.m_name = ribbon.GetRibbonName();
			this.m_description = ribbon.GetObtainedDescription();
			UIManager.SetGameObjectActive(this.m_lockIcon, !this.m_unlocked, null);
		}
		this.m_requirementProgressLabel.text = string.Empty;
		TMP_Text incompleteRequirementLabel = this.m_incompleteRequirementLabel;
		string name = this.m_name;
		this.m_completeRequirementLabel.text = name;
		incompleteRequirementLabel.text = name;
		if (this.m_tooltipHoverObj != null)
		{
			this.m_tooltipHoverObj.Refresh();
		}
	}

	private void SetDisplay(GameBalanceVars.UnlockData unlockData, List<GameBalanceVars.UnlockConditionValue> unlockConditionValues, bool banner)
	{
		if (unlockData != null && !unlockData.UnlockConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
		{
			if (!unlockConditionValues.IsNullOrEmpty<GameBalanceVars.UnlockConditionValue>())
			{
				if (unlockData.UnlockConditions.Length == unlockConditionValues.Count)
				{
					string text = string.Empty;
					GameBalanceVars.UnlockData.UnlockType unlockType = GameBalanceVars.UnlockData.UnlockType.Custom;
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					for (int i = 0; i < unlockData.UnlockConditions.Length; i++)
					{
						if (!GameBalanceVarsExtensions.IsUnlockConditionMet(unlockData.UnlockConditions[i], unlockConditionValues[i]))
						{
							num3 = i;
							break;
						}
					}
					GameBalanceVars.UnlockCondition unlockCondition = unlockData.UnlockConditions[num3];
					GameBalanceVars.UnlockConditionValue unlockConditionValue = unlockConditionValues[num3];
					if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.Custom)
					{
						text = ((!banner) ? StringUtil.TR_PlayerTitleUnlockCondition(this.m_selectedID, num3 + 1) : StringUtil.TR_BannerUnlockCondition(this.m_selectedID, num3 + 1));
						if (this.m_unlocked)
						{
							num2 = (num = 1);
						}
						else
						{
							num2 = (num = 0);
						}
						unlockType = unlockCondition.ConditionType;
					}
					else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.CharacterLevel)
					{
						CharacterType typeSpecificData = (CharacterType)unlockCondition.typeSpecificData;
						if (typeSpecificData != CharacterType.None)
						{
							num = unlockCondition.typeSpecificData2;
							num2 = unlockConditionValue.typeSpecificData2;
							unlockType = unlockCondition.ConditionType;
							text = string.Format(StringUtil.TR("UnlockCharacterLevelNeeded", "Global"), num, GameWideData.Get().GetCharacterResourceLink(typeSpecificData).GetDisplayName());
						}
					}
					else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.PlayerLevel)
					{
						num = unlockCondition.typeSpecificData;
						num2 = unlockConditionValue.typeSpecificData;
						unlockType = unlockCondition.ConditionType;
						text = string.Format(StringUtil.TR("UnlockAccountLevelNeeded", "Global"), num);
					}
					else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.ELO)
					{
						num = unlockCondition.typeSpecificData;
						num2 = unlockConditionValue.typeSpecificData;
						unlockType = unlockCondition.ConditionType;
						text = string.Format(StringUtil.TR("UnlockELONeeded", "Global"), num);
					}
					else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.Purchase)
					{
						num = unlockCondition.typeSpecificData2;
						num2 = unlockConditionValue.typeSpecificData2;
						unlockType = unlockCondition.ConditionType;
						text = StringUtil.TR("Purchase", "Global");
					}
					else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.FactionTierReached)
					{
						num2 = unlockCondition.typeSpecificData3;
						num = unlockConditionValue.typeSpecificData3;
						unlockType = unlockCondition.ConditionType;
						text = string.Format(StringUtil.TR("UnlockFactionTierNeeded", "Global"), num);
					}
					else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.TitleLevelReached)
					{
						num = unlockCondition.typeSpecificData;
						num2 = unlockConditionValue.typeSpecificData2;
						unlockType = unlockCondition.ConditionType;
						text = "Title Level";
					}
					else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.CurrentSeason)
					{
						num = unlockCondition.typeSpecificData;
						num2 = unlockConditionValue.typeSpecificData;
						unlockType = unlockCondition.ConditionType;
						text = "Current Season";
					}
					else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.Quest)
					{
						num2 = unlockConditionValue.typeSpecificData2;
						num = unlockConditionValue.typeSpecificData3;
						string text2;
						if (banner)
						{
							text2 = StringUtil.TR_BannerUnlockCondition(this.m_selectedID, num3 + 1);
						}
						else
						{
							text2 = StringUtil.TR_PlayerTitleUnlockCondition(this.m_selectedID, num3 + 1);
						}
						text = text2;
						unlockType = unlockCondition.ConditionType;
					}
					this.m_description = text;
					this.m_selectableButton.spriteController.ResetMouseState();
					if (this.m_unlocked)
					{
						UIManager.SetGameObjectActive(this.m_lockIcon, false, null);
						UIManager.SetGameObjectActive(this.m_progressSlider, false, null);
						UIManager.SetGameObjectActive(this.m_incompleteRequirementLabel.transform.parent, false, null);
						UIManager.SetGameObjectActive(this.m_completeRequirementLabel.transform.parent, true, null);
						this.m_completeRequirementLabel.text = text;
						this.m_selectableButton.m_ignoreHoverAnimationCall = false;
						this.m_selectableButton.m_ignorePressAnimationCall = false;
					}
					else
					{
						UIManager.SetGameObjectActive(this.m_lockIcon, true, null);
						if (num > 0)
						{
							UIManager.SetGameObjectActive(this.m_progressSlider, true, null);
							this.m_progressSlider.fillAmount = (float)num2 / (float)num;
						}
						else
						{
							UIManager.SetGameObjectActive(this.m_progressSlider, false, null);
						}
						UIManager.SetGameObjectActive(this.m_incompleteRequirementLabel.transform.parent, true, null);
						UIManager.SetGameObjectActive(this.m_completeRequirementLabel.transform.parent, false, null);
						this.m_incompleteRequirementLabel.text = text;
						this.m_selectableButton.m_ignoreHoverAnimationCall = true;
						this.m_selectableButton.m_ignorePressAnimationCall = true;
						if (unlockType != GameBalanceVars.UnlockData.UnlockType.ELO)
						{
							if (num > 0)
							{
								this.m_requirementProgressLabel.text = string.Format("{0} / {1}", num2, num);
								this.m_description = this.m_description + " (" + this.m_requirementProgressLabel.text + ")";
								return;
							}
						}
						this.m_requirementProgressLabel.text = string.Empty;
					}
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_lockIcon, false, null);
		UIManager.SetGameObjectActive(this.m_progressSlider, false, null);
		UIManager.SetGameObjectActive(this.m_incompleteRequirementLabel.transform.parent, false, null);
		UIManager.SetGameObjectActive(this.m_completeRequirementLabel.transform.parent, false, null);
		this.m_completeRequirementLabel.text = string.Empty;
		this.m_selectableButton.spriteController.ResetMouseState();
		this.m_selectableButton.m_ignoreHoverAnimationCall = true;
		this.m_selectableButton.m_ignorePressAnimationCall = true;
		this.m_name = string.Empty;
		this.m_description = string.Empty;
	}
}
