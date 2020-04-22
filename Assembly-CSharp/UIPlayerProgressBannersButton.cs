using LobbyGameClientMessages;
using System.Collections.Generic;
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
		m_selectableButton.spriteController.callback = BannerClicked;
		m_tooltipHoverObj = m_selectableButton.spriteController.GetComponent<UITooltipHoverObject>();
		m_tooltipHoverObj.Setup(TooltipType.Titled, BannerTooltipSetup);
		if (!(m_factionIcon != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClientGameManager.Get().OnFactionCompetitionNotification += OnFactionCompetitionNotification;
			return;
		}
	}

	private void OnDestroy()
	{
		if (!(m_factionIcon != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (ClientGameManager.Get() != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					ClientGameManager.Get().OnFactionCompetitionNotification -= OnFactionCompetitionNotification;
					return;
				}
			}
			return;
		}
	}

	private void OnFactionCompetitionNotification(FactionCompetitionNotification notification)
	{
		int activeFactionCompetition = ClientGameManager.Get().ActiveFactionCompetition;
		FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(activeFactionCompetition);
		if (factionCompetition != null)
		{
			while (true)
			{
				switch (7)
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
			for (int i = 0; i < factionCompetition.Factions.Count; i++)
			{
				for (int j = 0; j < factionCompetition.Factions[i].BannerIds.Count; j++)
				{
					if (factionCompetition.Factions[i].BannerIds[j] != m_selectedID)
					{
						continue;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						UIManager.SetGameObjectActive(m_factionIcon, true);
						FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(factionCompetition.Factions[i].FactionGroupIDToUse);
						m_factionIcon.sprite = Resources.Load<Sprite>(factionGroup.BannerIconPath);
						return;
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						goto end_IL_00d4;
					}
					continue;
					end_IL_00d4:
					break;
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
		UIManager.SetGameObjectActive(m_factionIcon, false);
	}

	public void BannerClicked(BaseEventData data)
	{
		if (!m_unlocked || !m_valid)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
			UIPlayerProgressPanel.Get().m_bannersPanel.BannerClicked(this);
			if (m_type == UIPlayerProgressBanners.CurrentList.Title)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						ClientGameManager clientGameManager = ClientGameManager.Get();
						int selectedID = m_selectedID;
						if (_003C_003Ef__am_0024cache0 == null)
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
							_003C_003Ef__am_0024cache0 = delegate(SelectTitleResponse response)
							{
								if (!response.Success)
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											break;
										default:
											if (1 == 0)
											{
												/*OpCode not supported: LdMemberToken*/;
											}
											Log.Error("Title change request was rejected: " + response.ErrorMessage);
											UIPlayerProgressPanel.Get().m_bannersPanel.ResetPage();
											ClientGameManager.Get().UpdatePlayerStatus(FriendListPanel.Get().m_panelHeader.m_statusLabels[0].text);
											return;
										}
									}
								}
							};
						}
						clientGameManager.RequestTitleSelect(selectedID, _003C_003Ef__am_0024cache0);
						return;
					}
					}
				}
			}
			if (m_type != UIPlayerProgressBanners.CurrentList.Background)
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
				if (m_type != 0)
				{
					if (m_type == UIPlayerProgressBanners.CurrentList.Ribbon)
					{
						ClientGameManager.Get().RequestRibbonSelect(m_selectedID, delegate(SelectRibbonResponse response)
						{
							if (!response.Success)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										if (1 == 0)
										{
											/*OpCode not supported: LdMemberToken*/;
										}
										Log.Error("Ribbon change request was rejected: " + response.ErrorMessage);
										UIPlayerProgressPanel.Get().m_bannersPanel.ResetPage();
										ClientGameManager.Get().UpdatePlayerStatus(FriendListPanel.Get().m_panelHeader.m_statusLabels[0].text);
										return;
									}
								}
							}
						});
					}
					return;
				}
			}
			ClientGameManager clientGameManager2 = ClientGameManager.Get();
			int selectedID2 = m_selectedID;
			if (_003C_003Ef__am_0024cache1 == null)
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
				_003C_003Ef__am_0024cache1 = delegate(SelectBannerResponse response)
				{
					if (!response.Success)
					{
						Log.Error("Banner change request was rejected: " + response.ErrorMessage);
						UIPlayerProgressPanel.Get().m_bannersPanel.ResetPage();
						ClientGameManager.Get().UpdatePlayerStatus(FriendListPanel.Get().m_panelHeader.m_statusLabels[0].text);
					}
				};
			}
			clientGameManager2.RequestBannerSelect(selectedID2, _003C_003Ef__am_0024cache1);
			return;
		}
	}

	private bool BannerTooltipSetup(UITooltipBase tooltip)
	{
		if (!m_name.IsNullOrEmpty())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
					uITitledTooltip.Setup(m_name, m_description, string.Empty);
					return true;
				}
				}
			}
		}
		return false;
	}

	public void SetSelected(bool isSelected)
	{
		UIManager.SetGameObjectActive(m_selected, isSelected);
		UIManager.SetGameObjectActive(m_selectedMain, isSelected);
	}

	public void SetupTitle(GameBalanceVars.PlayerTitle title)
	{
		m_type = UIPlayerProgressBanners.CurrentList.Title;
		if (m_bannerImage != null)
		{
			while (true)
			{
				switch (7)
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
			UIManager.SetGameObjectActive(m_bannerImage, false);
		}
		if (title == null)
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
			m_selectedID = -1;
			m_titleLabel.text = StringUtil.TR("TitleNone", "Global");
			m_unlocked = true;
			UIManager.SetGameObjectActive(m_lockIcon, false);
			UIManager.SetGameObjectActive(m_progressSlider, false);
			UIManager.SetGameObjectActive(m_incompleteRequirementLabel.transform.parent, false);
			UIManager.SetGameObjectActive(m_completeRequirementLabel.transform.parent, true);
			m_completeRequirementLabel.text = string.Empty;
			m_selectableButton.spriteController.ResetMouseState();
			m_selectableButton.m_ignoreHoverAnimationCall = false;
			m_selectableButton.m_ignorePressAnimationCall = false;
			m_name = string.Empty;
			m_description = string.Empty;
		}
		else
		{
			m_selectedID = title.ID;
			m_titleLabel.text = title.GetTitleText();
			m_unlocked = ClientGameManager.Get().IsTitleUnlocked(title, out List<GameBalanceVars.UnlockConditionValue> unlockConditionValues);
			SetDisplay(title.m_unlockData, unlockConditionValues, false);
			m_name = title.GetTitleText();
			if (!title.GetObtainedDescription().IsNullOrEmpty())
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
				m_description = title.GetObtainedDescription();
			}
		}
		UIManager.SetGameObjectActive(m_titleLabel, true);
		m_valid = true;
		SetSelected(ClientGameManager.Get().GetCurrentTitleID() == m_selectedID);
		if (!(m_tooltipHoverObj != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			m_tooltipHoverObj.Refresh();
			return;
		}
	}

	public void SetupBanner(GameBalanceVars.PlayerBanner banner)
	{
		List<GameBalanceVars.UnlockConditionValue> unlockConditionValues = null;
		if (banner == null)
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
			m_selectedID = -1;
			if (m_factionIcon != null)
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
				UIManager.SetGameObjectActive(m_factionIcon, false);
			}
			m_unlocked = true;
			m_valid = false;
			if (m_bannerImage != null)
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
				UIManager.SetGameObjectActive(m_bannerImage, false);
			}
			if (m_titleLabel != null)
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
				UIManager.SetGameObjectActive(m_titleLabel, false);
			}
			SetDisplay(null, unlockConditionValues, true);
			SetSelected(false);
			m_name = string.Empty;
		}
		else
		{
			int type;
			if (banner.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
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
				type = 1;
			}
			else
			{
				type = 0;
			}
			m_type = (UIPlayerProgressBanners.CurrentList)type;
			m_selectedID = banner.ID;
			Sprite sprite = (Sprite)Resources.Load(banner.m_iconResourceString, typeof(Sprite));
			if (sprite == null)
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
				Log.Warning(Log.Category.UI, $"Could not load banner resource from [{banner.m_iconResourceString}] as sprite.");
			}
			m_bannerImage.sprite = sprite;
			UIManager.SetGameObjectActive(m_bannerImage, true);
			m_unlocked = ClientGameManager.Get().IsBannerUnlocked(banner, out unlockConditionValues);
			SetDisplay(banner.m_unlockData, unlockConditionValues, true);
			if (m_factionIcon != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				OnFactionCompetitionNotification(null);
				float num;
				if (m_unlocked)
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
					num = 1f;
				}
				else
				{
					num = 0.25f;
				}
				float num2 = num;
				m_factionIcon.color = new Color(num2, num2, num2);
			}
			GameBalanceVars.PlayerBanner playerBanner;
			if (m_type == UIPlayerProgressBanners.CurrentList.Background)
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
				playerBanner = ClientGameManager.Get().GetCurrentBackgroundBanner();
			}
			else
			{
				playerBanner = ClientGameManager.Get().GetCurrentForegroundBanner();
			}
			int selected;
			if (playerBanner != null)
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
				selected = ((banner.ID == playerBanner.ID) ? 1 : 0);
			}
			else
			{
				selected = 0;
			}
			SetSelected((byte)selected != 0);
			m_valid = true;
			string colorHexString = banner.Rarity.GetColorHexString();
			m_name = "<color=" + colorHexString + ">" + banner.GetBannerName() + "</color>";
			if (!banner.GetObtainedDescription().IsNullOrEmpty())
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
				m_description = banner.GetObtainedDescription();
			}
		}
		m_requirementProgressLabel.text = string.Empty;
		TextMeshProUGUI incompleteRequirementLabel = m_incompleteRequirementLabel;
		string name = m_name;
		m_completeRequirementLabel.text = name;
		incompleteRequirementLabel.text = name;
		if (!(m_tooltipHoverObj != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			m_tooltipHoverObj.Refresh();
			return;
		}
	}

	public void SetupRibbon(GameBalanceVars.PlayerRibbon ribbon)
	{
		List<GameBalanceVars.UnlockConditionValue> unlockConditionValues = null;
		m_type = UIPlayerProgressBanners.CurrentList.Ribbon;
		m_selectableButton.m_ignoreHoverAnimationCall = false;
		m_selectableButton.m_ignorePressAnimationCall = false;
		UIManager.SetGameObjectActive(m_progressSlider, false);
		UIManager.SetGameObjectActive(m_incompleteRequirementLabel.transform.parent, false);
		UIManager.SetGameObjectActive(m_completeRequirementLabel.transform.parent, false);
		m_completeRequirementLabel.text = string.Empty;
		m_selectableButton.spriteController.ResetMouseState();
		if (ribbon == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_selectedID = -1;
			if (m_factionIcon != null)
			{
				UIManager.SetGameObjectActive(m_factionIcon, false);
			}
			m_unlocked = true;
			m_valid = true;
			if (m_bannerImage != null)
			{
				m_bannerImage.sprite = Resources.Load<Sprite>("QuestRewards/unequipIcon");
				UIManager.SetGameObjectActive(m_bannerImage, true);
			}
			if (m_titleLabel != null)
			{
				UIManager.SetGameObjectActive(m_titleLabel, false);
			}
			GameBalanceVars.PlayerRibbon currentRibbon = ClientGameManager.Get().GetCurrentRibbon();
			int selected;
			if (currentRibbon != null)
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
				selected = ((currentRibbon.ID == -1) ? 1 : 0);
			}
			else
			{
				selected = 1;
			}
			SetSelected((byte)selected != 0);
			UIManager.SetGameObjectActive(m_lockIcon, false);
			m_name = StringUtil.TR("TitleNone", "Global");
			m_description = string.Empty;
		}
		else
		{
			m_selectedID = ribbon.ID;
			Sprite sprite = Resources.Load<Sprite>(ribbon.m_resourceIconString);
			if (sprite == null)
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
				Log.Warning(Log.Category.UI, $"Could not load banner resource from [{ribbon.m_resourceIconString}] as sprite.");
			}
			m_bannerImage.sprite = sprite;
			UIManager.SetGameObjectActive(m_bannerImage, true);
			m_unlocked = ClientGameManager.Get().IsRibbonUnlocked(ribbon, out unlockConditionValues);
			SetDisplay(ribbon.m_unlockData, unlockConditionValues, false);
			if (m_factionIcon != null)
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
				UIManager.SetGameObjectActive(m_factionIcon, false);
			}
			GameBalanceVars.PlayerRibbon currentRibbon2 = ClientGameManager.Get().GetCurrentRibbon();
			SetSelected(currentRibbon2 != null && ribbon.ID == currentRibbon2.ID);
			m_valid = true;
			m_name = ribbon.GetRibbonName();
			m_description = ribbon.GetObtainedDescription();
			UIManager.SetGameObjectActive(m_lockIcon, !m_unlocked);
		}
		m_requirementProgressLabel.text = string.Empty;
		TextMeshProUGUI incompleteRequirementLabel = m_incompleteRequirementLabel;
		string name = m_name;
		m_completeRequirementLabel.text = name;
		incompleteRequirementLabel.text = name;
		if (!(m_tooltipHoverObj != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			m_tooltipHoverObj.Refresh();
			return;
		}
	}

	private void SetDisplay(GameBalanceVars.UnlockData unlockData, List<GameBalanceVars.UnlockConditionValue> unlockConditionValues, bool banner)
	{
		if (unlockData != null && !unlockData.UnlockConditions.IsNullOrEmpty())
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!unlockConditionValues.IsNullOrEmpty())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
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
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							num3 = i;
							break;
						}
					}
					GameBalanceVars.UnlockCondition unlockCondition = unlockData.UnlockConditions[num3];
					GameBalanceVars.UnlockConditionValue unlockConditionValue = unlockConditionValues[num3];
					if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.Custom)
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
						text = ((!banner) ? StringUtil.TR_PlayerTitleUnlockCondition(m_selectedID, num3 + 1) : StringUtil.TR_BannerUnlockCondition(m_selectedID, num3 + 1));
						if (m_unlocked)
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
							num = (num2 = 1);
						}
						else
						{
							num = (num2 = 0);
						}
						unlockType = unlockCondition.ConditionType;
					}
					else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.CharacterLevel)
					{
						CharacterType typeSpecificData = (CharacterType)unlockCondition.typeSpecificData;
						if (typeSpecificData != 0)
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
							num = unlockCondition.typeSpecificData2;
							num2 = unlockConditionValue.typeSpecificData2;
							unlockType = unlockCondition.ConditionType;
							text = string.Format(StringUtil.TR("UnlockCharacterLevelNeeded", "Global"), num, GameWideData.Get().GetCharacterResourceLink(typeSpecificData).GetDisplayName());
						}
					}
					else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.PlayerLevel)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						num = unlockCondition.typeSpecificData;
						num2 = unlockConditionValue.typeSpecificData;
						unlockType = unlockCondition.ConditionType;
						text = string.Format(StringUtil.TR("UnlockAccountLevelNeeded", "Global"), num);
					}
					else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.ELO)
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
						num = unlockCondition.typeSpecificData;
						num2 = unlockConditionValue.typeSpecificData;
						unlockType = unlockCondition.ConditionType;
						text = string.Format(StringUtil.TR("UnlockELONeeded", "Global"), num);
					}
					else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.Purchase)
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
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
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
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						num2 = unlockConditionValue.typeSpecificData2;
						num = unlockConditionValue.typeSpecificData3;
						string text2;
						if (banner)
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
							text2 = StringUtil.TR_BannerUnlockCondition(m_selectedID, num3 + 1);
						}
						else
						{
							text2 = StringUtil.TR_PlayerTitleUnlockCondition(m_selectedID, num3 + 1);
						}
						text = text2;
						unlockType = unlockCondition.ConditionType;
					}
					m_description = text;
					m_selectableButton.spriteController.ResetMouseState();
					if (m_unlocked)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								UIManager.SetGameObjectActive(m_lockIcon, false);
								UIManager.SetGameObjectActive(m_progressSlider, false);
								UIManager.SetGameObjectActive(m_incompleteRequirementLabel.transform.parent, false);
								UIManager.SetGameObjectActive(m_completeRequirementLabel.transform.parent, true);
								m_completeRequirementLabel.text = text;
								m_selectableButton.m_ignoreHoverAnimationCall = false;
								m_selectableButton.m_ignorePressAnimationCall = false;
								return;
							}
						}
					}
					UIManager.SetGameObjectActive(m_lockIcon, true);
					if (num > 0)
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
						UIManager.SetGameObjectActive(m_progressSlider, true);
						m_progressSlider.fillAmount = (float)num2 / (float)num;
					}
					else
					{
						UIManager.SetGameObjectActive(m_progressSlider, false);
					}
					UIManager.SetGameObjectActive(m_incompleteRequirementLabel.transform.parent, true);
					UIManager.SetGameObjectActive(m_completeRequirementLabel.transform.parent, false);
					m_incompleteRequirementLabel.text = text;
					m_selectableButton.m_ignoreHoverAnimationCall = true;
					m_selectableButton.m_ignorePressAnimationCall = true;
					if (unlockType != GameBalanceVars.UnlockData.UnlockType.ELO)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (num > 0)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									m_requirementProgressLabel.text = $"{num2} / {num}";
									m_description = m_description + " (" + m_requirementProgressLabel.text + ")";
									return;
								}
							}
						}
					}
					m_requirementProgressLabel.text = string.Empty;
					return;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		UIManager.SetGameObjectActive(m_lockIcon, false);
		UIManager.SetGameObjectActive(m_progressSlider, false);
		UIManager.SetGameObjectActive(m_incompleteRequirementLabel.transform.parent, false);
		UIManager.SetGameObjectActive(m_completeRequirementLabel.transform.parent, false);
		m_completeRequirementLabel.text = string.Empty;
		m_selectableButton.spriteController.ResetMouseState();
		m_selectableButton.m_ignoreHoverAnimationCall = true;
		m_selectableButton.m_ignorePressAnimationCall = true;
		m_name = string.Empty;
		m_description = string.Empty;
	}
}
