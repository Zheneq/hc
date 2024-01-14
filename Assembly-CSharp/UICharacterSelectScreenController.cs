using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterSelectScreenController : MonoBehaviour
{
	public RectTransform m_backgroundContainer;
	public RectTransform buttonContainer;
	public TextMeshProUGUI[] m_nameLabels;
	public TextMeshProUGUI[] m_accountNameLabels;
	public TextMeshProUGUI[] m_timeElapsedLabels;
	public UICharacterSelectCharacterSettingsPanel m_charSettingsPanel;
	public RectTransform m_miscCharSelectButtons;
	public Animator[] m_mainLobbyControllers;
	public _SelectableBtn m_highlightFreelancerBtn;
	public _SelectableBtn m_changeFreelancerBtn;
	public _SelectableBtn m_readyBtn;
	public Image m_highlightFreelancerImage;
	public _SelectableBtn m_cannotReadyBtn;
	public RectTransform m_buyButtonsContainer;
	public TextMeshProUGUI m_buyButtonsHeader;
	public _SelectableBtn m_buyInGameButton;
	public TextMeshProUGUI m_buyInGameLabel;
	public _SelectableBtn m_buyForCashButton;
	public TextMeshProUGUI m_buyForCashLabel;
	public _SelectableBtn m_buyForToken;
	public UITooltipClickObject[] m_groupMenuClickListeners;

	private bool m_characterLoading;
	private UIOneButtonDialog m_selectModsDialog;
	private static UICharacterSelectScreenController s_instance;
	private bool m_visible;
	private bool m_requireGroupUpdate;
	private long[] m_createGameTimestamps;
	private bool m_needToRepickCharacter;
	private string m_cannotReadyReasonString;
	private UIOneButtonDialog m_messageBox;
	private Queue<Action> m_preReadyActions;
	private List<UICharacterSelectFactionFilter> m_filterButtons;
	private UICharacterSelectFactionFilter m_lastFilterBtnClicked;

	public Canvas m_nameLabelCanvas => GetComponentInParent<Canvas>();

	public bool DoesASelectedGame_OverrideFreelancerSelection
	{
		get
		{
			LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
			if (!groupInfo.InAGroup)
			{
				return true;
			}
			Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(groupInfo.SelectedQueueType);
			foreach (KeyValuePair<ushort, GameSubType> current in gameTypeSubTypes)
			{
				if ((current.Key & groupInfo.SubTypeMask) == 0)
				{
					continue;
				}

				bool flag = current.Value.HasMod(GameSubType.SubTypeMods.OverrideFreelancerSelection)
				            || current.Value.DuplicationRule == FreelancerDuplicationRuleTypes.allow
				            || current.Value.DuplicationRule == FreelancerDuplicationRuleTypes.alwaysDupAcrossGame
				            || current.Value.DuplicationRule == FreelancerDuplicationRuleTypes.alwaysDupAcrossTeam;
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}
	}

	public static UICharacterSelectScreenController Get()
	{
		return s_instance;
	}

	public void Start()
	{
		m_preReadyActions = new Queue<Action>();
		m_groupMenuClickListeners[0].Setup(TooltipType.PlayerGroupMenu, (UITooltipBase tooltip) => OnGroupMenuClick(tooltip, 0));
		m_groupMenuClickListeners[1].Setup(TooltipType.PlayerGroupMenu, (UITooltipBase tooltip) => OnGroupMenuClick(tooltip, 1));
		m_groupMenuClickListeners[2].Setup(TooltipType.PlayerGroupMenu, (UITooltipBase tooltip) => OnGroupMenuClick(tooltip, 2));
		m_groupMenuClickListeners[3].Setup(TooltipType.PlayerGroupMenu, (UITooltipBase tooltip) => OnGroupMenuClick(tooltip, 3));
		m_changeFreelancerBtn.spriteController.pointerEnterCallback = HighlightCharacter;
		m_changeFreelancerBtn.spriteController.pointerExitCallback = UnhighlightCharacter;
		m_highlightFreelancerBtn.spriteController.pointerEnterCallback = HighlightCharacter;
		m_highlightFreelancerBtn.spriteController.pointerExitCallback = UnhighlightCharacter;
		UIEventTriggerUtils.AddListener(m_highlightFreelancerImage.gameObject, EventTriggerType.PointerEnter, HighlightCharacter);
		UIEventTriggerUtils.AddListener(m_highlightFreelancerImage.gameObject, EventTriggerType.PointerExit, UnhighlightCharacter);
		m_cannotReadyBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
		{
			(tooltip as UITitledTooltip).Setup(
				StringUtil.TR("CannotReady", "Global"),
				GetCannotReadyReason(),
				string.Empty);
			return true;
		});
		UIManager.SetGameObjectActive(m_cannotReadyBtn, false);
		foreach (TextMeshProUGUI textMesh in m_nameLabels)
		{
			textMesh.raycastTarget = false;
		}
		foreach (TextMeshProUGUI textMesh in m_accountNameLabels)
		{
			textMesh.raycastTarget = false;
		}
		foreach (TextMeshProUGUI textMesh in m_timeElapsedLabels)
		{
			textMesh.raycastTarget = false;
		}
	}

	public void SetupReadyButton()
	{
		if (!CanReady())
		{
			return;
		}
		bool flag = true;
		if (m_readyBtn.m_animationController.isActiveAndEnabled)
		{
			AnimatorClipInfo[] currentAnimatorClipInfo = m_readyBtn.m_animationController.GetCurrentAnimatorClipInfo(1);
			if (currentAnimatorClipInfo != null
			    && currentAnimatorClipInfo.Length > 0
			    && (currentAnimatorClipInfo[0].clip.name == "readyBtnOnIN" || currentAnimatorClipInfo[0].clip.name == "readyBtnOnIDLE"))
			{
				flag = false;
			}
			if (flag)
			{
				m_readyBtn.m_animationController.Play("readyBtnOnIN", 1, 0f);
			}
		}
		m_readyBtn.spriteController.SetClickable(true);
		UIManager.SetGameObjectActive(m_readyBtn, true);
	}

	public void NotifyCharacterDoneLoading()
	{
		m_characterLoading = false;
		UpdateReadyButton();
	}

	private bool OnGroupMenuClick(UITooltipBase tooltip, int indexClicked)
	{
		UIPlayerPanelGroupMenu uIPlayerPanelGroupMenu = tooltip as UIPlayerPanelGroupMenu;
		List<UpdateGroupMemberData> members = ClientGameManager.Get().GroupInfo.Members;
		if (GameManager.Get() != null && GameManager.Get().GameStatus != GameStatus.Stopped)
		{
			GameManager gameManager = GameManager.Get();
			LobbyPlayerInfo playerInfo = gameManager.PlayerInfo;
			Team team = playerInfo.TeamId;
			if (team == Team.Spectator)
			{
				team = Team.TeamA;
			}
			List<LobbyPlayerInfo> list = gameManager.TeamInfo.TeamInfo(team)
				.OrderBy(ti => ti.PlayerId == playerInfo.PlayerId ? 0 : 1).ToList();
			int num = 1;
			int index = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].AccountId == ClientGameManager.Get().GetPlayerAccountData().AccountId)
				{
					if (indexClicked == 0)
					{
						index = i;
						break;
					}
				}
				else
				{
					if (indexClicked == num)
					{
						index = i;
						break;
					}
					num++;
				}
			}
			uIPlayerPanelGroupMenu.Setup(list[index]);
			return true;
		}
		if (members.Count > 1 && indexClicked < members.Count)
		{
			uIPlayerPanelGroupMenu.Setup(members[indexClicked]);
			return true;
		}
		return false;
	}

	public void HighlightCharacter(BaseEventData data)
	{
		UIActorModelData componentInChildren = UICharacterSelectWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
		if (componentInChildren != null)
		{
			componentInChildren.SetMouseIsOver(true);
		}
	}

	public void UnhighlightCharacter(BaseEventData data)
	{
		UIActorModelData componentInChildren = UICharacterSelectWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
		if (componentInChildren != null)
		{
			componentInChildren.SetMouseIsOver(false);
		}
	}

	public void NotifyGameIsLoading()
	{
		m_charSettingsPanel.SetVisible(false);
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			SideButtonsClickable = false
		});
	}

	private bool CanReady()
	{
		return CanReady(out _, false);
	}

	private bool IsReady()
	{
		return AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()
		       || AppState_LandingPage.Get() == AppState.GetCurrent()
			? AppState_GroupCharacterSelect.Get().IsReady()
			: AppState_CharacterSelect.IsReady();
	}

	private bool CanReady(out string reason, bool needsReason)
	{
		reason = string.Empty;
		if (AppState.GetCurrent() != AppState_CharacterSelect.Get() && AppState.GetCurrent() != AppState_GroupCharacterSelect.Get())
		{
			if (needsReason)
			{
				reason = string.Empty;
			}
			return true;
		}
		if (IsReady())
		{
			if (needsReason)
			{
				reason = StringUtil.TR("AlreadyReady", "Global");
			}
			return false;
		}
		if (m_characterLoading)
		{
			if (needsReason)
			{
				reason = StringUtil.TR("LOADING", "Global");
			}
			return false;
		}
		CharacterResourceLink characterResourceLinkOfCharacterTypeToDisplay =
			UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay;
		if (characterResourceLinkOfCharacterTypeToDisplay == null
		    && UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.RankedModeSelect)
		{
			if (needsReason)
			{
				reason = StringUtil.TR("SelectFreelancer", "Global");
			}
			return false;
		}
		GameManager gameManager = GameManager.Get();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		GameType num = gameManager != null && gameManager.GameConfig != null && gameManager.GameStatus != GameStatus.Stopped
			? gameManager.GameConfig.GameType
			: clientGameManager.GroupInfo.SelectedQueueType;
		goto IL_0169;
		IL_0482:
		bool flag;
		int num2;
		GameType gameType;
		if (AppState.GetCurrent() == AppState_CharacterSelect.Get())
		{
			if (gameManager.TeamPlayerInfo != null)
			{
				flag = false;
				if (gameManager.GameConfig != null && gameManager.GameConfig.GameType == GameType.Custom)
				{
					if (!gameManager.GameConfig.GameOptionFlags.HasGameOption(GameOptionFlag.AllowDuplicateCharacters) && !gameManager.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.RankedFreelancerSelection))
					{
						num2 = (gameManager.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.OverrideFreelancerSelection) ? 1 : 0);
					}
					else
					{
						num2 = 1;
					}
					goto IL_0529;
				}
				else
				{
					goto IL_052b;
				}
			}
		}
		else if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
		{
			if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.RankedModeSelect)
			{
				GameTypeAvailability value2;
				if (clientGameManager.GroupInfo.InAGroup)
				{
					long accountId = clientGameManager.GetPlayerAccountData().AccountId;
					int num3 = 0;
					if (!DoesASelectedGame_OverrideFreelancerSelection)
					{
						foreach (UpdateGroupMemberData member in clientGameManager.GroupInfo.Members)
						{
							if (member.AccountID != accountId)
							{
								if (member.MemberDisplayCharacter == clientGameManager.GroupInfo.ChararacterInfo.CharacterType)
								{
									if (member.IsReady)
									{
										num3++;
									}
								}
							}
						}
					}
					if (num3 > 0)
					{
						if (!clientGameManager.GroupInfo.ChararacterInfo.CharacterType.IsWillFill())
						{
							while (true)
							{
								if (needsReason)
								{
									reason = StringUtil.TR("GroupConflict", "Global");
								}
								return false;
							}
						}
						if (clientGameManager.GameTypeAvailabilies.TryGetValue(clientGameManager.GroupInfo.SelectedQueueType, out GameTypeAvailability value))
						{
							int maxWillFillPerTeam = value.MaxWillFillPerTeam;
							if (maxWillFillPerTeam < num3 + 1)
							{
								if (needsReason)
								{
									LocalizationArg_GameType localizationArg_GameType = LocalizationArg_GameType.Create(gameType);
									LocalizationArg_Int32 localizationArg_Int = LocalizationArg_Int32.Create(maxWillFillPerTeam);
									reason = LocalizationPayload.Create("GroupHasTooManyWillFill", "Matchmaking", localizationArg_GameType, localizationArg_Int).ToString();
								}
								return false;
							}
						}
						else
						{
							Log.Warning("We have no GameTypeAvailability for {0}, why so?", clientGameManager.GroupInfo.SelectedQueueType);
						}
					}
				}
				else if (clientGameManager.GameTypeAvailabilies.TryGetValue(clientGameManager.GroupInfo.SelectedQueueType, out value2))
				{
					if (value2.MaxWillFillPerTeam == 0)
					{
						if (UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay.IsWillFill())
						{
							if (needsReason)
							{
								LocalizationArg_GameType localizationArg_GameType2 = LocalizationArg_GameType.Create(gameType);
								reason = LocalizationPayload.Create("QueueDoesNotAllowWillFill", "Matchmaking", localizationArg_GameType2).ToString();
							}
							return false;
						}
					}
				}
				else
				{
					Log.Warning("We have no GameTypeAvailability for {0}, why?", clientGameManager.GroupInfo.SelectedQueueType);
				}
			}
		}
		goto IL_08d8;
		IL_02b7:
		if (!gameManager.IsCharacterAllowedForGameType(characterResourceLinkOfCharacterTypeToDisplay.m_characterType, gameType, null, null))
		{
			if (needsReason)
			{
				LocalizationArg_Freelancer localizationArg_Freelancer = LocalizationArg_Freelancer.Create(characterResourceLinkOfCharacterTypeToDisplay.m_characterType);
				LocalizationArg_GameType localizationArg_GameType3 = LocalizationArg_GameType.Create(gameType);
				reason = LocalizationPayload.Create("GameTypeDoesNotAllowCharacterType", "Global", localizationArg_Freelancer, localizationArg_GameType3).ToString();
			}
			return false;
		}
		goto IL_0318;
		IL_0529:
		flag = ((byte)num2 != 0);
		goto IL_052b;
		IL_052b:
		if (!flag)
		{
			using (List<LobbyPlayerInfo>.Enumerator enumerator2 = gameManager.TeamPlayerInfo.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					LobbyPlayerInfo current2 = enumerator2.Current;
					if (current2.PlayerId != gameManager.PlayerInfo.PlayerId)
					{
						if (!current2.IsNPCBot)
						{
							if (characterResourceLinkOfCharacterTypeToDisplay != null)
							{
								if (current2.CharacterType == characterResourceLinkOfCharacterTypeToDisplay.m_characterType && current2.ReadyState == ReadyState.Ready)
								{
									if (needsReason)
									{
										reason = StringUtil.TR("DuplicateFreelancer", "Global");
									}
									return false;
								}
							}
						}
					}
				}
			}
		}
		goto IL_08d8;
		IL_0169:
		gameType = num;
		bool flag3;
		if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.RankedModeSelect)
		{
			int num4;
			if (gameManager != null)
			{
				num4 = ((gameManager.GameStatus != GameStatus.Stopped) ? 1 : 0);
			}
			else
			{
				num4 = 0;
			}
			bool flag2 = (byte)num4 != 0;
			flag3 = (flag2 && gameManager.GameConfig.GameType == GameType.Custom);
			if (!flag2)
			{
				if (!gameManager.IsValidForHumanPreGameSelection(characterResourceLinkOfCharacterTypeToDisplay.m_characterType))
				{
					goto IL_021b;
				}
			}
			if (flag2)
			{
				if (!gameManager.IsCharacterAllowedForPlayers(characterResourceLinkOfCharacterTypeToDisplay.m_characterType))
				{
					goto IL_021b;
				}
			}
			goto IL_02b7;
		}
		goto IL_0318;
		IL_021b:
		if (flag3)
		{
			if (gameManager.IsCharacterAllowedForGameType(characterResourceLinkOfCharacterTypeToDisplay.m_characterType, gameType, null, null))
			{
				goto IL_02b7;
			}
		}
		if (flag3)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (needsReason)
					{
						LocalizationArg_Freelancer localizationArg_Freelancer2 = LocalizationArg_Freelancer.Create(characterResourceLinkOfCharacterTypeToDisplay.m_characterType);
						LocalizationArg_GameType localizationArg_GameType4 = LocalizationArg_GameType.Create(gameType);
						reason = LocalizationPayload.Create("GameTypeDoesNotAllowCharacterType", "Global", localizationArg_Freelancer2, localizationArg_GameType4).ToString();
					}
					return false;
				}
			}
		}
		if (needsReason)
		{
			reason = StringUtil.TR("FreelancerDenied", "Global");
		}
		return false;
		IL_0318:
		if (UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay != null)
		{
			if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.RankedModeSelect)
			{
				CharacterType characterTypeToDisplay = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay;
				if (!clientGameManager.IsCharacterAvailable(characterTypeToDisplay, gameType))
				{
					if (!characterTypeToDisplay.IsWillFill())
					{
						if (!(gameManager != null))
						{
							if (clientGameManager.GroupInfo != null)
							{
								if (clientGameManager.GroupInfo.SelectedQueueType == GameType.Practice)
								{
									goto IL_0482;
								}
							}
						}
						bool flag4 = false;
						if (gameManager.GameConfig != null && gameManager.GameStatus != GameStatus.Stopped && gameManager.GameConfig.GameType == GameType.Custom)
						{
							int num5;
							if (!gameManager.GameConfig.GameOptionFlags.HasGameOption(GameOptionFlag.AllowDuplicateCharacters) && !gameManager.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AllowPlayingLockedCharacters))
							{
								num5 = (gameManager.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.OverrideFreelancerSelection) ? 1 : 0);
							}
							else
							{
								num5 = 1;
							}
							flag4 = ((byte)num5 != 0);
						}
						if (!flag4)
						{
							if (needsReason)
							{
								reason = StringUtil.TR("FreelancerNotAvailable", "Global");
							}
							return false;
						}
					}
				}
			}
		}
		goto IL_0482;
		IL_08d8:
		if (needsReason)
		{
			LocalizationPayload blockingQueueRestriction = clientGameManager.GetBlockingQueueRestriction(gameType);
			if (blockingQueueRestriction != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						reason = blockingQueueRestriction.ToString();
						return false;
					}
				}
			}
		}
		else if (!clientGameManager.MeetsAllQueueRequirements(gameType))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		reason = string.Empty;
		return true;
	}

	public void CancelButtonClickCallback(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.Cancel);
		if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.CharacterSelect)
		{
			if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.RankedModeSelect)
			{
				if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.GroupCharacterSelect)
				{
					if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.LandingPage)
					{
						if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.RankedModeSelect)
						{
							return;
						}
					}
				}
				if (!IsReady())
				{
					return;
				}
				while (true)
				{
					UIManager.SetGameObjectActive(m_readyBtn, true);
					SearchQueueTextExit();
					NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnOUT", 0, 0f);
					m_readyBtn.m_animationController.Play("readyBtnOnIN", 1, 0f);
					m_readyBtn.spriteController.SetClickable(true);
					NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(false);
					SetReady(false);
					return;
				}
			}
		}
		if (IsReady())
		{
			UIManager.SetGameObjectActive(m_readyBtn, true);
			SearchQueueTextExit();
			NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnOUT", 0, 0f);
			m_readyBtn.m_animationController.Play("readyBtnOnIN", 1, 0f);
			m_readyBtn.spriteController.SetClickable(true);
			NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(false);
			SetReady(false);
		}
	}

	public void NotifiedEnteredQueue()
	{
		m_needToRepickCharacter = false;
	}

	public bool RepickingCharacter()
	{
		return m_needToRepickCharacter;
	}

	public void AllowCharacterSwapForConflict()
	{
		m_needToRepickCharacter = true;
		SearchQueueTextExit();
		NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnOUT", 0, 0f);
		m_readyBtn.m_animationController.Play("readyBtnOnIN", 1, 0f);
		m_readyBtn.spriteController.SetClickable(true);
		UIManager.SetGameObjectActive(m_readyBtn, true);
		NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(false);
		SetCharacterSelectVisible(true);
		Log.Info("Setting self not ready because I need to re pick characters");
		SetReady(false);
	}

	public void CancelButtonAnimDone()
	{
		UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, false);
	}

	private bool ReadyModeSupportsCancel()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (GameManager.Get().GameStatus.IsActiveStatus())
		{
			if (clientGameManager.PlayerInfo != null)
			{
				if (clientGameManager.PlayerInfo.TeamId == Team.Spectator)
				{
					return false;
				}
			}
		}
		if (!GameManager.Get().GameStatus.IsActiveStatus())
		{
			if (clientGameManager.GroupInfo.SelectedQueueType != GameType.Solo)
			{
				if (clientGameManager.GroupInfo.SelectedQueueType != GameType.Practice)
				{
					goto IL_00a9;
				}
			}
			return false;
		}
		goto IL_00a9;
		IL_00a9:
		return true;
	}

	public void DoReadyClick(FrontEndButtonSounds soundToPlay = FrontEndButtonSounds.StartGameReady)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		ushort num = 0;
		GameType gameType;
		if (GameManager.Get().GameConfig != null)
		{
			if (GameManager.Get().GameStatus != GameStatus.Stopped)
			{
				gameType = GameManager.Get().GameConfig.GameType;
				num = GameManager.Get().GameConfig.InstanceSubTypeBit;
				goto IL_0081;
			}
		}
		gameType = clientGameManager.GroupInfo.SelectedQueueType;
		goto IL_0081;
		IL_0081:
		if (!UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay.IsWillFill())
		{
			bool flag = true;
			bool flag2 = false;
			ushort soloSubGameMask = clientGameManager.GetSoloSubGameMask(gameType);
			using (Dictionary<ushort, GameSubType>.Enumerator enumerator = clientGameManager.GetGameTypeSubTypes(gameType).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ushort, GameSubType> current = enumerator.Current;
					if ((soloSubGameMask & current.Key) == 0)
					{
						continue;
					}
					if (current.Value.Mods == null)
					{
						continue;
					}
					if (!current.Value.Mods.Contains(GameSubType.SubTypeMods.OverrideFreelancerSelection))
					{
						if (!current.Value.Mods.Contains(GameSubType.SubTypeMods.RankedFreelancerSelection))
						{
							goto IL_0151;
						}
					}
					flag = false;
					goto IL_0151;
					IL_0151:
					if (current.Value.Mods.Contains(GameSubType.SubTypeMods.StricterMods))
					{
						flag2 = true;
					}
				}
			}
			if (flag)
			{
				bool flag3 = false;
				if (UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.ActorDataPrefab != null)
				{
					flag3 = AbilityModHelper.HasAnyAvailableMods(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.ActorDataPrefab.GetComponent<ActorData>());
				}
				CharacterModInfo characterModInfo;
				if (flag2)
				{
					characterModInfo = clientGameManager.GetPlayerCharacterData(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay).CharacterComponent.LastRankedMods;
				}
				else
				{
					characterModInfo = clientGameManager.GetPlayerCharacterData(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay).CharacterComponent.LastMods;
				}
				if (flag3)
				{
					if (characterModInfo.ModForAbility0 < 0)
					{
						if (characterModInfo.ModForAbility1 < 0 && characterModInfo.ModForAbility2 < 0 && characterModInfo.ModForAbility3 < 0 && characterModInfo.ModForAbility4 < 0)
						{
							m_preReadyActions.Enqueue(delegate
							{
								UIDialogPopupManager.OpenTwoButtonDialog(string.Empty, StringUtil.TR("NoModSelectedWarning", "Global"), StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate
								{
									ReadyHelper(soundToPlay);
								}, delegate
								{
									m_preReadyActions.Clear();
								});
							});
						}
					}
				}
			}
		}
		Dictionary<ushort, GameSubType> gameTypeSubTypes = clientGameManager.GetGameTypeSubTypes(gameType);
		if (!gameTypeSubTypes.IsNullOrEmpty())
		{
			if (num == 0)
			{
				if (gameTypeSubTypes.Count > 1)
				{
					Log.Warning("FAKING Selection of SubType for {0} XP penalty warning", gameType);
				}
			}
			if (clientGameManager.GameTypeAvailabilies.TryGetValue(gameType, out GameTypeAvailability value))
			{
				if (!value.XPPenaltyTimeout.IsNullOrEmpty() && value.XPPenaltyTimeout.TryGetValue(num, out DateTime value2))
				{
					if (value2 > clientGameManager.UtcNow())
					{
						TimeSpan span = value2 - clientGameManager.UtcNow();
						string arg = LocalizationArg_TimeSpan.Create(span).TR();
						string timeString = string.Format(StringUtil.TR("NoExperienceForThisGameTypeForHoursMinutes", "Global"), gameType.GetDisplayName(), arg);
						m_preReadyActions.Enqueue(delegate
						{
							UIDialogPopupManager.OpenTwoButtonDialog(string.Empty, timeString, StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate
							{
								ReadyHelper(soundToPlay);
							}, delegate
							{
								m_preReadyActions.Clear();
							});
						});
					}
				}
			}
		}
		ReadyHelper(soundToPlay);
	}

	private void ReadyHelper(FrontEndButtonSounds soundToPlay)
	{
		if (m_preReadyActions.Count > 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					m_preReadyActions.Dequeue()();
					return;
				}
			}
		}
		if (UIFrontEnd.Get().m_currentScreen == FrontEndScreenState.CharacterSelect)
		{
			if (CanReady())
			{
				UIManager.SetGameObjectActive(m_readyBtn, true);
				if (!m_needToRepickCharacter)
				{
					if (ReadyModeSupportsCancel())
					{
						UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, true);
						CheckCancelBtnLabels();
					}
				}
				NavigationBar.Get().m_searchQueueText.text = string.Empty;
				UIFrontEnd.PlaySound(soundToPlay);
				GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndReady, null);
				if (ReadyModeSupportsCancel())
				{
					SearchQueueTextExit();
					NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnIN", 0, 0f);
				}
				m_readyBtn.m_animationController.Play("readyBtnOffIN", 1, 0f);
				m_readyBtn.spriteController.SetClickable(false);
				NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(true);
				SetReady(true);
			}
		}
		else if (UIFrontEnd.Get().m_currentScreen == FrontEndScreenState.GroupCharacterSelect)
		{
			if (AppState.GetCurrent() != AppState_CharacterSelect.Get())
			{
				UIMatchStartPanel.Get().NotifyDuplicateFreelancer(false);
			}
			if (CanReady())
			{
				UIFrontEnd.PlaySound(soundToPlay);
				GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndReady, null);
				if (ReadyModeSupportsCancel())
				{
					SearchQueueTextExit();
					NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnIN", 0, 0f);
				}
				m_readyBtn.m_animationController.Play("readyBtnOffIN", 1, 0f);
				UIManager.SetGameObjectActive(m_readyBtn, false);
				m_readyBtn.spriteController.SetClickable(false);
				if (!m_needToRepickCharacter)
				{
					if (ReadyModeSupportsCancel())
					{
						UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, true);
						CheckCancelBtnLabels();
					}
				}
				NavigationBar.Get().m_searchQueueText.text = string.Empty;
				NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(true);
				SetReady(true);
			}
		}
		else if (UIFrontEnd.Get().m_currentScreen == FrontEndScreenState.RankedModeSelect)
		{
			if (CanReady())
			{
				UIFrontEnd.PlaySound(soundToPlay);
				GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndReady, null);
				if (ReadyModeSupportsCancel())
				{
					SearchQueueTextExit();
					NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnIN", 0, 0f);
				}
				m_readyBtn.m_animationController.Play("readyBtnOffIN", 1, 0f);
				UIManager.SetGameObjectActive(m_readyBtn, false);
				m_readyBtn.spriteController.SetClickable(false);
				if (!m_needToRepickCharacter)
				{
					if (ReadyModeSupportsCancel())
					{
						UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, true);
						CheckCancelBtnLabels();
					}
				}
				NavigationBar.Get().m_searchQueueText.text = string.Empty;
				NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(true);
				SetReady(true);
			}
		}
		m_needToRepickCharacter = false;
		UINewUserFlowManager.OnDoneWithReadyButton();
	}

	public void ReadyButtonClickCallback(BaseEventData data)
	{
		DoReadyClick();
	}

	public void SetReady(bool ready, bool UpdateSideMenus = true)
	{
		if (m_characterLoading)
		{
			return;
		}
		if (UICharacterSelectWorldObjects.Get().CharacterIsLoading())
		{
			m_characterLoading = true;
		}
		else
		{
			UICharacterSelectWorldObjects.Get().SetCharacterReady(0, ready);
			UICharacterSelectWorldObjects.Get().SetReadyPose();
		}
		if (ready)
		{
			UICharacterSelectWorldObjects.Get().m_ringAnimations[0].PlayAnimation("ReadyIn");
			if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
			{
				AppState_GroupCharacterSelect.Get().UpdateReadyState(true);
			}
			else if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
			{
				AppState_CharacterSelect.Get().UpdateReadyState(true);
			}
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				CharacterSelectButtonsVisible = false
			});
			m_charSettingsPanel.SetVisible(false);
		}
		else
		{
			UICharacterSelectWorldObjects.Get().m_ringAnimations[0].PlayAnimation("ReadyOut");
			if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
			{
				AppState_CharacterSelect.Get().UpdateReadyState(false);
			}
			else if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()
			         || AppState_LandingPage.Get() == AppState.GetCurrent())
			{
				AppState_GroupCharacterSelect.Get().UpdateReadyState(false);
			}
		}
		UICharacterScreen.Get().RefreshCharacterButtons();
	}

	private void PlayCharacterSelectButtonAnimation(bool toVisible)
	{
	}

	public void SetCharacterSelectVisible(bool visible)
	{
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			CharacterSelectButtonsVisible = visible
		});
	}

	public void ToggleCharacterSelect(BaseEventData data)
	{
		if (UIFrontEnd.Get().IsDraggingModel() || !Input.GetMouseButtonUp(0))
		{
			return;
		}
		while (true)
		{
			bool? characterSelectButtonsVisible = UICharacterScreen.GetCurrentSpecificState().CharacterSelectButtonsVisible;
			SetCharacterSelectVisible(!characterSelectButtonsVisible.Value);
			return;
		}
	}

	public void QuickPlaySetup(LobbyGameInfo gameInfo)
	{
		if (ClientGameManager.Get() == null || ClientGameManager.Get().GroupInfo == null)
		{
			return;
		}
		while (true)
		{
			if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
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
			return;
		}
	}

	public bool IsCharacterSelectable(CharacterResourceLink selectedCharacter)
	{
		bool result = false;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		GameType num;
		if (selectedCharacter != null)
		{
			if (clientGameManager != null)
			{
				if (UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay != null)
				{
					if (GameManager.Get() != null)
					{
						GameManager gameManager = GameManager.Get();
						if (gameManager != null)
						{
							if (gameManager.GameConfig != null)
							{
								if (gameManager.GameStatus != GameStatus.Stopped)
								{
									num = gameManager.GameConfig.GameType;
									goto IL_00d8;
								}
							}
						}
						num = clientGameManager.GroupInfo.SelectedQueueType;
						goto IL_00d8;
					}
				}
			}
		}
		goto IL_00e7;
		IL_00e7:
		return result;
		IL_00d8:
		GameType gameType = num;
		result = clientGameManager.IsCharacterAvailable(selectedCharacter.m_characterType, gameType);
		goto IL_00e7;
	}

	private void Update()
	{
		if (!(AppState_CharacterSelect.Get() == AppState.GetCurrent()))
		{
			if (!(AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()))
			{
				if (!(AppState_LandingPage.Get() == AppState.GetCurrent()))
				{
					goto IL_0380;
				}
			}
		}
		_SelectableBtn changeFreelancerBtn;
		int doActive;
		if (!UIGameSettingsPanel.Get().m_lastVisible)
		{
			for (int i = 0; i < m_nameLabels.Length; i++)
			{
				if (i >= UICharacterSelectWorldObjects.Get().m_ringAnimations.Length)
				{
					continue;
				}
				if (!(UIManager.Get().GetEnvirontmentCamera() != null))
				{
					continue;
				}
				if (UICharacterSelectWorldObjects.Get().m_ringAnimations[i].m_nameObject.activeInHierarchy)
				{
					UIManager.SetGameObjectActive(m_accountNameLabels[i], true);
					UIManager.SetGameObjectActive(m_nameLabels[i], true);
					if (!m_timeElapsedLabels.IsNullOrEmpty())
					{
						UIManager.SetGameObjectActive(m_timeElapsedLabels[i], AppState.GetCurrent() == AppState_GroupCharacterSelect.Get());
					}
					Vector3 vector = UIManager.Get().GetEnvirontmentCamera().WorldToViewportPoint(UICharacterSelectWorldObjects.Get().m_ringAnimations[i].m_nameObject.transform.position);
					float x = vector.x;
					Vector2 sizeDelta = (m_nameLabelCanvas.gameObject.transform as RectTransform).sizeDelta;
					float x2 = x * sizeDelta.x;
					float y = vector.y;
					Vector2 sizeDelta2 = (m_nameLabelCanvas.gameObject.transform as RectTransform).sizeDelta;
					Vector2 anchoredPosition = new Vector2(x2, y * sizeDelta2.y);
					(m_nameLabels[i].transform as RectTransform).anchoredPosition = anchoredPosition;
				}
				else
				{
					UIManager.SetGameObjectActive(m_nameLabels[i], false);
					UIManager.SetGameObjectActive(m_timeElapsedLabels[i], false);
				}
			}
			GameManager gameManager = GameManager.Get();
			bool flag = false;
			if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
			{
				if (m_requireGroupUpdate)
				{
					if (!UIMatchStartPanel.Get().IsVisible())
					{
						if (!GameManager.Get().IsGameLoading())
						{
							m_requireGroupUpdate = false;
							UpdateGroupMemberCharacters(gameManager.PlayerInfo);
						}
					}
				}
				flag = AppState_GroupCharacterSelect.Get().IsReady();
				UIPlayCategoryMenu.Get().SetMenuButtonsClickable(!flag && !UIMatchStartPanel.IsMatchCountdownStarting());
				UIManager.SetGameObjectActive(m_changeFreelancerBtn, !flag);
			}
			else if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
			{
				flag = AppState_CharacterSelect.IsReady();
				UIPlayCategoryMenu uIPlayCategoryMenu = UIPlayCategoryMenu.Get();
				int menuButtonsClickable;
				if (flag)
				{
					menuButtonsClickable = ((!UIMatchStartPanel.IsMatchCountdownStarting()) ? 1 : 0);
				}
				else
				{
					menuButtonsClickable = 0;
				}
				uIPlayCategoryMenu.SetMenuButtonsClickable((byte)menuButtonsClickable != 0);
			}
			changeFreelancerBtn = m_changeFreelancerBtn;
			if (!flag)
			{
				if (!UIStorePanel.Get().IsVisible())
				{
					doActive = ((!UIPlayerProgressPanel.Get().IsVisible()) ? 1 : 0);
					goto IL_036f;
				}
			}
			doActive = 0;
			goto IL_036f;
		}
		goto IL_0380;
		IL_0409:
		UpdateReadyCancelButtonStates();
		if (AppState_RankModeDraft.Get() == AppState.GetCurrent())
		{
			SetVisible(false);
		}
		if (!(GameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (GameManager.Get().GameplayOverrides.DisableControlPadInput)
			{
				return;
			}
			while (true)
			{
				if (m_readyBtn.spriteController.IsClickable())
				{
					if (Input.GetButtonDown("GamepadButtonY"))
					{
						DoReadyClick();
					}
				}
				if (!NavigationBar.Get().m_cancelBtn.spriteController.IsClickable())
				{
					return;
				}
				while (true)
				{
					if (Input.GetButtonDown("GamepadButtonB"))
					{
						while (true)
						{
							CancelButtonClickCallback(null);
							return;
						}
					}
					return;
				}
			}
		}
		IL_0380:
		for (int j = 0; j < m_nameLabels.Length; j++)
		{
			UIManager.SetGameObjectActive(m_nameLabels[j], false);
		}
		for (int k = 0; k < m_accountNameLabels.Length; k++)
		{
			UIManager.SetGameObjectActive(m_accountNameLabels[k], false);
		}
		for (int l = 0; l < m_timeElapsedLabels.Length; l++)
		{
			UIManager.SetGameObjectActive(m_timeElapsedLabels[l], false);
		}
		goto IL_0409;
		IL_036f:
		UIManager.SetGameObjectActive(changeFreelancerBtn, (byte)doActive != 0);
		UpdateTimeElapsedLabels();
		goto IL_0409;
	}

	private void UpdateGroupMemberCharacters(LobbyPlayerInfo playerInfo)
	{
		LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
		int num = UICharacterSelectWorldObjects.Get().m_ringAnimations.Length;
		int i = 0;
		if (!groupInfo.InAGroup)
		{
			bool isReady = false;
			if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
			{
				isReady = AppState_GroupCharacterSelect.Get().IsReady();
			}
			if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
			{
				isReady = AppState_CharacterSelect.IsReady();
			}
			UICharacterScreen.CharacterSelectSceneStateParameters currentSpecificState = UICharacterScreen.GetCurrentSpecificState();
			groupInfo.ChararacterInfo.CharacterSkin = currentSpecificState.CharacterVisualInfoToDisplay;
			SetClientCharacter(
				0,
				groupInfo.MemberDisplayName,
				currentSpecificState.CharacterResourceLinkOfCharacterTypeToDisplay,
				groupInfo.ChararacterInfo,
				isReady,
				false,
				-1L);
			i++;
		}
		else if (playerInfo != null)
		{
			if (playerInfo.TeamId == Team.Spectator)
			{
				SetClientCharacter(-1, playerInfo.GetHandle(), null, null, true, false, -1L);
			}
		}
		int num2 = 1;
		for (int j = 0; j < groupInfo.Members.Count; i++, j++)
		{
			bool flag = groupInfo.Members[j].AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId;
			bool isBot = false;
			bool isReady3 = groupInfo.Members[j].IsReady;
			bool isInGame = groupInfo.Members[j].IsInGame;
			long createGameTimestamp = groupInfo.Members[j].CreateGameTimestamp;
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(groupInfo.Members[j].MemberDisplayCharacter);
			if (flag)
			{
				if (groupInfo.ChararacterInfo.CharacterType.IsWillFill())
				{
				}
				else
				{
					if (ClientGameManager.Get().IsWaitingForSkinResponse())
					{
						continue;
					}
					if (groupInfo.ChararacterInfo.CharacterType == characterResourceLink.m_characterType)
					{
						if (UICharacterSelectWorldObjects.Get().CharacterTypeInSlot(0) == characterResourceLink.m_characterType)
						{
							continue;
						}
					}
					SetClientCharacter(0, groupInfo.Members[j].MemberDisplayName, characterResourceLink, groupInfo.ChararacterInfo, isReady3, isInGame, createGameTimestamp);
				}
			}
			else
			{
				SetCharacter(num2, groupInfo.Members[j].MemberDisplayName, characterResourceLink, groupInfo.Members[j].VisualInfo, isBot, isReady3, isInGame, createGameTimestamp);
				num2++;
			}
		}
		UICharacterSelectWorldObjects.Get().SetReadyPose();
		UICharacterSelectWorldObjects.Get().SetSkins();
		UICharacterSelectWorldObjects.Get().SetIsInGameRings();
		for (; i < num; i++)
		{
			int num3 = 4;
			if (i < num3)
			{
				UICharacterSelectWorldObjects.Get().m_ringAnimations[i].gameObject.SetActive(true);
			}
			else
			{
				UICharacterSelectWorldObjects.Get().m_ringAnimations[i].gameObject.SetActive(false);
			}
			UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(null, i, string.Empty, default(CharacterVisualInfo), false, false);
			SetCharacterLabel(i, string.Empty, string.Empty, -1L);
		}
		while (true)
		{
			UpdateReadyCancelButtonStates();
			return;
		}
	}

	public string GetCannotReadyReason()
	{
		CanReady(out m_cannotReadyReasonString, true);
		if (m_cannotReadyReasonString == StringUtil.TR("GroupConflict", "Global"))
		{
			m_cannotReadyReasonString = StringUtil.TR("FreelancerAlreadyChosen", "Global");
		}
		return m_cannotReadyReasonString;
	}

	public void UpdateReadyCancelButtonStates()
	{
		if (!ClientGameManager.Get().IsReady)
		{
			return;
		}
		LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
		bool flag = CanReady();
		bool flag2 = true;
		bool flag3 = false;
		int num;
		if (UIMatchStartPanel.Get() != null)
		{
			if (!UIMatchStartPanel.Get().IsVisible())
			{
				num = 1;
				goto IL_0080;
			}
		}
		num = ((GameManager.Get().GameInfo != null) ? 1 : 0);
		goto IL_0080;
		IL_0622:
		int num2;
		bool doActive = (byte)num2 != 0;
		UIManager.SetGameObjectActive(m_cannotReadyBtn, doActive);
		bool flag4;
		if (NavigationBar.Get() != null)
		{
			if (!NavigationBar.Get().m_cancelBtn.gameObject.activeSelf && flag2)
			{
				UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, true);
				CheckCancelBtnLabels();
				NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(true);
				SearchQueueTextExit();
				if (NavigationBar.Get().m_cancelBtn.m_animationController.isActiveAndEnabled)
				{
					NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnIN", 0, 0f);
				}
			}
			if (!flag4)
			{
				if (!NavigationBar.Get().m_cancelBtn.gameObject.activeSelf)
				{
					goto IL_0788;
				}
			}
			if (!flag2)
			{
				UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, false);
				NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(false);
				SearchQueueTextExit();
				if (NavigationBar.Get().m_cancelBtn.m_animationController.isActiveAndEnabled)
				{
					NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnOUT", 0, 0f);
				}
			}
		}
		goto IL_0788;
		IL_0788:
		if (UIRankedModeSelectScreen.Get() != null)
		{
			UIRankedModeSelectScreen.Get().UpdateReadyButton(flag);
		}
		return;
		IL_0080:
		bool flag5 = (byte)num != 0;
		CharacterType item = CharacterType.None;
		List<CharacterType> list = new List<CharacterType>();
		if (groupInfo != null)
		{
			if (groupInfo.Members.Count > 0)
			{
				if (!flag5)
				{
					for (int i = 0; i < groupInfo.Members.Count; i++)
					{
						if (groupInfo.Members[i].AccountID != ClientGameManager.Get().GetPlayerAccountData().AccountId)
						{
							if (!groupInfo.Members[i].IsReady)
							{
								continue;
							}
							if (!groupInfo.Members[i].MemberDisplayCharacter.IsWillFill() && !DoesASelectedGame_OverrideFreelancerSelection)
							{
								list.Add(groupInfo.Members[i].MemberDisplayCharacter);
							}
							continue;
						}
						item = groupInfo.Members[i].MemberDisplayCharacter;
						if (groupInfo.Members[i].IsReady)
						{
							if (!m_needToRepickCharacter)
							{
								flag = false;
								flag3 = true;
								continue;
							}
						}
						flag2 = false;
					}
					if (list.Contains(item))
					{
						if (groupInfo.SelectedQueueType != GameType.Ranked)
						{
							m_cannotReadyReasonString = StringUtil.TR("AnotherPlayerSelectedFreelancer", "Global");
							flag = false;
						}
					}
					goto IL_03a0;
				}
			}
		}
		if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
		{
			if (AppState_CharacterSelect.IsReady())
			{
				flag = false;
				flag3 = true;
			}
			else
			{
				flag2 = false;
			}
		}
		else if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
		{
			if (AppState_GroupCharacterSelect.Get().IsReady())
			{
				flag = false;
				flag3 = true;
			}
			else
			{
				flag2 = false;
			}
			if (!ReadyModeSupportsCancel())
			{
				flag2 = false;
			}
		}
		else if (AppState_LandingPage.Get() == AppState.GetCurrent())
		{
			flag = false;
			int num3;
			if (GameManager.Get() != null)
			{
				num3 = ((GameManager.Get().QueueInfo != null) ? 1 : 0);
			}
			else
			{
				num3 = 0;
			}
			flag2 = ((byte)num3 != 0);
		}
		else
		{
			if (!(AppState_CreateGame.Get() == AppState.GetCurrent()))
			{
				if (!(AppState_JoinGame.Get() == AppState.GetCurrent()))
				{
					if (!(AppState_FrontendLoadingScreen.Get() == AppState.GetCurrent()))
					{
						if (!(AppState_GameLoading.Get() == AppState.GetCurrent()))
						{
							if (!UIMatchStartPanel.Get().IsVisible())
							{
								goto IL_03a0;
							}
						}
					}
				}
			}
			flag = false;
			flag2 = false;
		}
		goto IL_03a0;
		IL_03a0:
		if (UIMatchStartPanel.Get() != null && UIMatchStartPanel.Get().IsVisible())
		{
			if (!UIMatchStartPanel.Get().IsDuplicateFreelancerResolving())
			{
				flag3 = true;
				flag = false;
			}
		}
		if (!ReadyModeSupportsCancel())
		{
			flag2 = false;
		}
		flag4 = false;
		if (UIGameSettingsPanel.Get() != null && UIGameSettingsPanel.Get().m_lastVisible)
		{
			flag = false;
			flag2 = false;
			flag4 = true;
		}
		if (flag4)
		{
			UIManager.SetGameObjectActive(m_buyButtonsContainer, false);
		}
		else
		{
			UpdateBuyButtons();
		}
		if (!m_readyBtn.gameObject.activeSelf)
		{
			if (flag)
			{
				UIManager.SetGameObjectActive(m_readyBtn, true);
				m_readyBtn.spriteController.SetClickable(true);
				m_readyBtn.m_animationController.Play("readyBtnOnIN", 1, 0f);
				UICharacterSelectWorldObjects.Get().SetCharacterReady(0, false);
				UICharacterSelectWorldObjects.Get().SetReadyPose();
			}
		}
		if (!flag4)
		{
			if (!m_readyBtn.gameObject.activeSelf)
			{
				goto IL_0600;
			}
		}
		if (!flag)
		{
			UIManager.SetGameObjectActive(m_readyBtn, false);
			m_readyBtn.spriteController.SetClickable(false);
			if (m_readyBtn.m_animationController.isActiveAndEnabled)
			{
				m_readyBtn.m_animationController.Play("readyBtnOffIN", 1, 0f);
			}
			if (groupInfo != null && groupInfo.Members.Count > 0)
			{
				if (list.Count > 0)
				{
					UICharacterSelectWorldObjects.Get().SetCharacterReady(0, !list.Contains(item));
				}
				else
				{
					UpdateGroupMemberData updateGroupMemberData = groupInfo.Members.Find((UpdateGroupMemberData x) => x.AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId);
					UICharacterSelectWorldObjects uICharacterSelectWorldObjects = UICharacterSelectWorldObjects.Get();
					bool isReady;
					if (updateGroupMemberData != null)
					{
						isReady = updateGroupMemberData.IsReady;
					}
					else
					{
						isReady = flag3;
					}
					uICharacterSelectWorldObjects.SetCharacterReady(0, isReady);
				}
			}
			else
			{
				UICharacterSelectWorldObjects.Get().SetCharacterReady(0, flag3);
			}
			UICharacterSelectWorldObjects.Get().SetReadyPose();
		}
		goto IL_0600;
		IL_0600:
		if (!flag)
		{
			if (!flag3)
			{
				num2 = ((!flag4) ? 1 : 0);
				goto IL_0622;
			}
		}
		num2 = 0;
		goto IL_0622;
	}

	public void NotifyDroppedGroup()
	{
		UIManager.SetGameObjectActive(m_changeFreelancerBtn, true);
	}

	public void NotifyGroupUpdate()
	{
		m_requireGroupUpdate = true;
		m_charSettingsPanel.Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay);
	}

	private void SetCharacterLabel(int nameIndex, string playerName, string characterName, long gameStartTimestamp)
	{
		if (!m_nameLabels.IsNullOrEmpty() && nameIndex < m_nameLabels.Length)
		{
			if (nameIndex > 0)
			{
				m_nameLabels[nameIndex].text = characterName;
			}
			else
			{
				m_nameLabels[nameIndex].text = string.Empty;
			}
		}
		if (!m_accountNameLabels.IsNullOrEmpty() && nameIndex < m_accountNameLabels.Length)
		{
			if (nameIndex > 0)
			{
				m_accountNameLabels[nameIndex].text = playerName;
			}
			else
			{
				m_accountNameLabels[nameIndex].text = string.Empty;
			}
		}
		if (m_timeElapsedLabels.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			if (nameIndex >= m_timeElapsedLabels.Length)
			{
				return;
			}
			while (true)
			{
				if (nameIndex > 0)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							m_createGameTimestamps[nameIndex] = gameStartTimestamp;
							return;
						}
					}
				}
				m_createGameTimestamps[nameIndex] = 0L;
				return;
			}
		}
	}

	private void UpdateTimeElapsedLabels()
	{
		for (int i = 0; i < m_createGameTimestamps.Length; i++)
		{
			if (m_createGameTimestamps[i] <= 0)
			{
				m_timeElapsedLabels[i].text = string.Empty;
			}
			else
			{
				DateTime d = new DateTime(m_createGameTimestamps[i]);
				TimeSpan timeSpan = DateTime.UtcNow - d;
				m_timeElapsedLabels[i].text = StringUtil.TR("TimeElapsed", "Global") + ": " + string.Format(StringUtil.TR("TimeFormat", "Global"), Mathf.FloorToInt((float)timeSpan.TotalMinutes), timeSpan.Seconds);
			}
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

	private void SetCharacter(int playerIndex, string playerHandle, CharacterResourceLink charLink, CharacterVisualInfo skin, bool isBot, bool isReady, bool isInGame, long gameStartTimestamp)
	{
		UICharacterSelectWorldObjects.Get().m_ringAnimations[playerIndex].gameObject.SetActive(true);
		if (playerIndex == 0)
		{
			if (ClientGameManager.Get().IsWaitingForSkinResponse())
			{
				goto IL_0059;
			}
		}
		UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(charLink, playerIndex, playerHandle, skin, isBot, true);
		goto IL_0059;
		IL_0059:
		SetCharacterLabel(playerIndex, playerHandle, charLink.GetDisplayName(), gameStartTimestamp);
		UICharacterSelectWorldObjects.Get().SetCharacterReady(playerIndex, isReady);
		UICharacterSelectWorldObjects.Get().CheckReadyBand(playerIndex, isReady);
		UICharacterSelectWorldObjects.Get().SetCharacterInGame(playerIndex, isInGame);
	}

	private void SetClientCharacter(
		int playerIndex,
		string playerHandle,
		CharacterResourceLink charLink,
		LobbyCharacterInfo charInfo,
		bool isReady,
		bool isInGame,
		long gameStartTimestamp)
	{
		UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters = new UICharacterScreen.CharacterSelectSceneStateParameters();
		if (playerIndex >= 0)
		{
			SetCharacter(playerIndex, playerHandle, charLink, charInfo.CharacterSkin, false, isReady, isInGame, gameStartTimestamp);
			UICharacterScreen.Get().RefreshSelectedCharacterButton();
			if (!m_charSettingsPanel.m_spellsSubPanel.GetDisplayedCardInfo().Equals(charInfo.CharacterCards))
			{
				m_charSettingsPanel.m_spellsSubPanel.Setup(charLink.m_characterType, charInfo.CharacterCards);
			}

			if (m_charSettingsPanel.m_abilitiesSubPanel.GetDisplayedCharacter() == null
			    || !m_charSettingsPanel.m_abilitiesSubPanel.GetDisplayedCharacter().m_characterType.Equals(charLink.m_characterType))
			{
				m_charSettingsPanel.m_abilitiesSubPanel.Setup(charLink);
			}

			if (!m_charSettingsPanel.m_skinsSubPanel.GetDisplayedCharacterType().Equals(charLink.m_characterType)
			    || !m_charSettingsPanel.m_skinsSubPanel.GetDisplayedVisualInfo().Equals(charInfo.CharacterSkin))
			{
				m_charSettingsPanel.m_skinsSubPanel.Setup(charLink, charInfo.CharacterSkin);
			}

			if (m_charSettingsPanel.m_tauntsSubPanel.GetDisplayedCharacter() == null
			    || !m_charSettingsPanel.m_tauntsSubPanel.GetDisplayedCharacter().m_characterType.Equals(charLink.m_characterType))
			{
				m_charSettingsPanel.m_tauntsSubPanel.Setup(charLink);
			}

			if (m_charSettingsPanel.m_generalSubPanel != null
			    && (m_charSettingsPanel.m_generalSubPanel.GetDisplayedCharacter() == null 
			        || !m_charSettingsPanel.m_generalSubPanel.GetDisplayedCharacter().m_characterType.Equals(charLink.m_characterType)))
			{
				m_charSettingsPanel.m_generalSubPanel.Setup(charLink);
			}
			characterSelectSceneStateParameters.ClientSelectedCharacter = charLink.m_characterType;
			characterSelectSceneStateParameters.SideButtonsVisible = true;
		}
		else
		{
			characterSelectSceneStateParameters.SideButtonsVisible = false;
		}
		UIManager.Get().HandleNewSceneStateParameter(characterSelectSceneStateParameters);
	}

	public void UpdatePrimaryCharacter(LobbyCharacterInfo characterInfo)
	{
		SetClientCharacter(
			0,
			ClientGameManager.Get().Handle,
			UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay,
			characterInfo,
			IsReady(),
			false,
			-1L);
		m_charSettingsPanel.Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay);
	}

	public void UpdateCharacters(LobbyPlayerInfo playerInfo, List<LobbyPlayerInfo> teamPlayerInfos, LobbyGameplayOverrides gameplayOverrides)
	{
		if (GameManager.Get().GameConfig.GameType == GameType.Ranked || playerInfo == null)
		{
			return;
		}
		GameManager gameManager = GameManager.Get();
		LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
		int num = UICharacterSelectWorldObjects.Get().m_ringAnimations.Length;
		bool updatedPrimaryCharacter = false;
		if (playerInfo != null
		    && !groupInfo.InAGroup
		    && playerInfo.TeamId != Team.Spectator
		    && playerInfo.CharacterType != CharacterType.None)
		{
			bool isReady = playerInfo.ReadyState == ReadyState.Ready;
			CharacterType? clientSelectedCharacter = UICharacterScreen.GetCurrentSpecificState().ClientSelectedCharacter;
			if (clientSelectedCharacter.Value != playerInfo.CharacterType
			    || UICharacterSelectWorldObjects.Get().IsCharReady(0) != isReady)
			{
				UIManager.Get().HandleNewSceneStateParameter(
					new UICharacterScreen.CharacterSelectSceneStateParameters
					{
						ClientSelectedCharacter = playerInfo.CharacterType
					});
				SetClientCharacter(
					0,
					playerInfo.GetHandle(),
					UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay,
					playerInfo.CharacterInfo,
					isReady,
					false,
					-1L);
			}
			m_charSettingsPanel.Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay);
			UICharacterSelectWorldObjects.Get().SetCharSelectTriggerForSlot(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, 0);
			updatedPrimaryCharacter = true;
		}
		else if (playerInfo != null && playerInfo.TeamId == Team.Spectator)
		{
			bool isReady = true;
			SetClientCharacter(-1, playerInfo.GetHandle(), null, null, isReady, false, -1L);
		}
		bool flag3 = false;
		bool flag4 = false;
		int numLoadedCharacters = UICharacterSelectWorldObjects.Get().GetNumLoadedCharacters();
		if (teamPlayerInfos != null)
		{
			int i = 0;
			if (updatedPrimaryCharacter)
			{
				i++;
			}

			if (GameManager.Get().GameConfig.GameType != GameType.Practice
			    && GameManager.Get().GameConfig.GameType != GameType.Solo)
			{
				if (!updatedPrimaryCharacter)
				{
					foreach (LobbyPlayerInfo player in teamPlayerInfos)
					{
						if (player.CharacterType != CharacterType.None)
						{
							bool isReady = player.IsNPCBot || player.IsReady;
							CharacterResourceLink characterResourceLink =
								GameWideData.Get().GetCharacterResourceLink(player.CharacterType);
							if (player.PlayerId == playerInfo.PlayerId)
							{
								SetClientCharacter(i, player.GetHandle(), characterResourceLink,
									playerInfo.CharacterInfo, isReady, false, -1L);
								i++;
								updatedPrimaryCharacter = true;
								break;
							}
						}
					}
				}
				foreach (LobbyPlayerInfo player in teamPlayerInfos)
				{
					if (player.CharacterType != CharacterType.None)
					{
						bool isPrimary = player.PlayerId == playerInfo.PlayerId;
						bool isNPCBot = player.IsNPCBot;
						bool isReady = isNPCBot || player.IsReady;
						CharacterResourceLink characterResourceLink =
							GameWideData.Get().GetCharacterResourceLink(player.CharacterType);
						if (isPrimary)
						{
							if (!updatedPrimaryCharacter)
							{
								SetClientCharacter(
									i,
									player.GetHandle(),
									characterResourceLink,
									playerInfo.CharacterInfo,
									isReady,
									false,
									-1L);
								i++;
								updatedPrimaryCharacter = true;
							}
						}
						else
						{
							SetCharacter(
								i,
								player.GetHandle(),
								characterResourceLink,
								player.CharacterInfo.CharacterSkin,
								isNPCBot,
								isReady,
								false,
								-1L);
							i++;
						}

						if (i >= num)
						{
							break;
						}
					}
				}
				if (numLoadedCharacters > 0)
				{
					if (numLoadedCharacters > i)
					{
						flag4 = true;
					}
					if (numLoadedCharacters < i)
					{
						flag3 = true;
					}
				}
				if (flag3)
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.TeamMemberSelect);
				}
				if (flag4)
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.TeamMemberCancel);
				}
			}
			UICharacterSelectWorldObjects.Get().SetReadyPose();
			UICharacterSelectWorldObjects.Get().SetSkins();
			int num5 = 0;
			if (playerInfo.TeamId == Team.TeamA)
			{
				num5 = ((gameManager.GameInfo.GameConfig.GameType != 0) ? (gameManager.GameInfo.GameConfig.TeamAPlayers - gameManager.GameInfo.GameConfig.TeamABots) : gameManager.GameInfo.GameConfig.TeamAPlayers);
			}
			else if (playerInfo.TeamId == Team.TeamB)
			{
				if (gameManager.GameInfo.GameConfig.GameType == GameType.Custom)
				{
					num5 = gameManager.GameInfo.GameConfig.TeamBPlayers;
				}
				else
				{
					num5 = gameManager.GameInfo.GameConfig.TeamBPlayers - gameManager.GameInfo.GameConfig.TeamBBots;
				}
			}
			for (; i < num; i++)
			{
				if (i < num5)
				{
					UICharacterSelectWorldObjects.Get().m_ringAnimations[i].gameObject.SetActive(true);
					UICharacterSelectWorldObjects.Get().m_ringAnimations[i].PlayAnimation("TransitionOut");
				}
				else
				{
					UICharacterSelectWorldObjects.Get().m_ringAnimations[i].gameObject.SetActive(false);
				}
				UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(null, i, string.Empty, default(CharacterVisualInfo), false, false);
				SetCharacterLabel(i, string.Empty, string.Empty, -1L);
			}
			UICharacterScreen.Get().RefreshCharacterButtons();
		}
		UpdateReadyButton();
	}

	public void UpdateReadyButton()
	{
	}

	public void UpdateBuyButtons()
	{
		if (!(m_buyButtonsContainer != null))
		{
			return;
		}
		while (true)
		{
			if (!(ClientGameManager.Get() != null))
			{
				return;
			}
			while (true)
			{
				bool flag = false;
				if (!ClientGameManager.Get().HasPurchasedGame && UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay != null)
				{
					if (!ClientGameManager.Get().IsPlayerCharacterDataAvailable(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay))
					{
						return;
					}
					PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay);
					int num;
					if (!playerCharacterData.CharacterComponent.Unlocked)
					{
						num = (GameManager.Get().IsCharacterAllowedForPlayers(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay) ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					flag = ((byte)num != 0);
				}
				UIManager.SetGameObjectActive(m_buyButtonsContainer, flag);
				if (!(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay != null) || !flag)
				{
					return;
				}
				int unlockFreelancerCurrencyPrice = UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.m_charUnlockData.GetUnlockFreelancerCurrencyPrice();
				m_buyInGameLabel.text = "<sprite name=credit>" + unlockFreelancerCurrencyPrice;
				UIManager.SetGameObjectActive(m_buyInGameButton, unlockFreelancerCurrencyPrice > 0);
				string accountCurrency = HydrogenConfig.Get().Ticket.AccountCurrency;
				float freelancerPrice = CommerceClient.Get().GetFreelancerPrice(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay, accountCurrency);
				m_buyForCashLabel.text = UIStorePanel.GetLocalizedPriceString(freelancerPrice, accountCurrency);
				UIManager.SetGameObjectActive(m_buyForCashButton, freelancerPrice > 0f);
				bool flag2 = ClientGameManager.Get().PlayerWallet.GetValue(CurrencyType.UnlockFreelancerToken).m_Amount > 0;
				UIManager.SetGameObjectActive(m_buyForToken, flag2);
				RectTransform buyButtonsContainer = m_buyButtonsContainer;
				int doActive;
				if (unlockFreelancerCurrencyPrice <= 0 && !(freelancerPrice > 0f))
				{
					doActive = (flag2 ? 1 : 0);
				}
				else
				{
					doActive = 1;
				}
				UIManager.SetGameObjectActive(buyButtonsContainer, (byte)doActive != 0);
				return;
			}
		}
	}

	public bool HaveUsedAnyModPoints()
	{
		int num = m_charSettingsPanel.m_abilitiesSubPanel.CalculateTotalModEquipCost();
		bool flag = false;
		if ((bool)UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay)
		{
			GameObject actorDataPrefab = UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.ActorDataPrefab;
			ActorData component = actorDataPrefab.GetComponent<ActorData>();
			if (GameManager.Get().GameConfig != null)
			{
				flag = AbilityModHelper.HasAnyAvailableMods(component);
			}
		}
		int result;
		if (num <= 0)
		{
			result = ((!flag) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public void QuickPlayUpdateCharacters(LobbyGameplayOverrides gameplayOverrides, bool switchedChars = false, bool isFromServerResponse = false)
	{
		if (ClientGameManager.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (ClientGameManager.Get().GroupInfo == null)
			{
				return;
			}
			while (true)
			{
				if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					while (true)
					{
						switch (7)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
				UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters = new UICharacterScreen.CharacterSelectSceneStateParameters();
				characterSelectSceneStateParameters.ClientSelectedCharacter = SceneStateParameters.SelectedCharacterFromPlayerData;
				if (UILandingPageScreen.Get() != null)
				{
					if (UILandingPageScreen.Get().CharacterInfoClicked.HasValue)
					{
						characterSelectSceneStateParameters.ClientSelectedCharacter = UILandingPageScreen.Get().CharacterInfoClicked.Value;
						UILandingPageScreen.Get().CharacterInfoClicked = null;
					}
				}
				if (!isFromServerResponse)
				{
					if (characterSelectSceneStateParameters.ClientSelectedCharacter.HasValue && ClientGameManager.Get().GetAllPlayerCharacterData().ContainsKey(characterSelectSceneStateParameters.ClientSelectedCharacter.Value))
					{
						characterSelectSceneStateParameters.ClientSelectedVisualInfo = ClientGameManager.Get().GetAllPlayerCharacterData()[characterSelectSceneStateParameters.ClientSelectedCharacter.Value].CharacterComponent.LastSkin;
					}
				}
				UIManager.Get().HandleNewSceneStateParameter(characterSelectSceneStateParameters);
				CharacterVisualInfo characterVisualInfo = default(CharacterVisualInfo);
				if (groupInfo != null)
				{
					if (groupInfo.InAGroup)
					{
						characterVisualInfo = groupInfo.ChararacterInfo.CharacterSkin;
						goto IL_01a5;
					}
				}
				characterVisualInfo = UICharacterScreen.GetCurrentSpecificState().CharacterVisualInfoToDisplay;
				goto IL_01a5;
				IL_01a5:
				UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, 0, groupInfo.MemberDisplayName, characterVisualInfo, false, true);
				SetCharacterLabel(0, groupInfo.MemberDisplayName, UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.GetDisplayName(), (groupInfo.Members.Count <= 1) ? 0 : groupInfo.Members[0].CreateGameTimestamp);
				UICharacterScreen.Get().RefreshSelectedCharacterButton();
				UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					SideButtonsVisible = true,
					ClientSelectedCharacter = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay
				});
				m_charSettingsPanel.Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, false, switchedChars);
				UICharacterScreen.Get().RefreshCharacterButtons();
				return;
			}
		}
	}

	public void ShowPleaseEquipModsDialog()
	{
		string description = StringUtil.TR("SelectYourAbilityMods", "Global");
		if (!(m_selectModsDialog == null))
		{
			return;
		}
		while (true)
		{
			m_selectModsDialog = UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("MustSelectAbilityMods", "Global"), description, StringUtil.TR("Ok", "Global"));
			return;
		}
	}

	public void SetupForRanked(bool ranked)
	{
		if (!UIFrontEnd.Get().m_frontEndNavPanel.m_PlayBtn.IsSelected())
		{
			return;
		}
		if (!(AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()))
		{
			if (!(AppState_CharacterSelect.Get() == AppState.GetCurrent()))
			{
				if (!(UIRankedModeSelectScreen.Get() != null))
				{
					return;
				}
				while (true)
				{
					UIRankedModeSelectScreen.Get().SetVisible(false);
					return;
				}
			}
		}
		if (ranked)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					UIFrontEnd.Get().ShowScreen(FrontEndScreenState.RankedModeSelect);
					if (UIRankedModeSelectScreen.Get() != null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								UIRankedModeSelectScreen.Get().SetVisible(true);
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.GroupCharacterSelect)
		{
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.GroupCharacterSelect);
		}
		if (UIRankedModeSelectScreen.Get() != null)
		{
			UIRankedModeSelectScreen.Get().SetVisible(false);
		}
	}

	public bool IsVisible()
	{
		return m_visible;
	}

	public void SetVisible(bool visible, bool refreshOnly = false)
	{
		if (buttonContainer == null)
		{
			return;
		}
		while (true)
		{
			if (buttonContainer.gameObject == null)
			{
				while (true)
				{
					switch (4)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			CharacterType characterType;
			if (m_visible != visible)
			{
				UIManager.SetGameObjectActive(buttonContainer, visible);
				m_visible = visible;
				for (int i = 0; i < m_mainLobbyControllers.Length; i++)
				{
					UIManager.SetGameObjectActive(m_mainLobbyControllers[i], visible);
				}
				LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
				characterType = CharacterType.None;
				if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
				{
					if (groupInfo != null)
					{
						if (groupInfo.InAGroup)
						{
							characterType = groupInfo.ChararacterInfo.CharacterType;
							goto IL_011d;
						}
					}
				}
				if (UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay != null)
				{
					characterType = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay;
				}
				goto IL_011d;
			}
			goto IL_0263;
			IL_0263:
			if (visible)
			{
				if (!refreshOnly)
				{
					UIFrontEnd.Get().m_frontEndNavPanel.SetNavButtonSelected(UIFrontEnd.Get().m_frontEndNavPanel.m_PlayBtn);
				}
			}
			if (m_selectModsDialog != null)
			{
				while (true)
				{
					UIDialogPopupManager.Get().CloseDialog(m_selectModsDialog);
					return;
				}
			}
			return;
			IL_011d:
			UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters = new UICharacterScreen.CharacterSelectSceneStateParameters();
			int value;
			if (visible)
			{
				if (UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay != null)
				{
					value = ((characterType != CharacterType.None) ? 1 : 0);
				}
				else
				{
					value = 0;
				}
			}
			else
			{
				value = 0;
			}
			characterSelectSceneStateParameters.SideButtonsVisible = ((byte)value != 0);
			characterSelectSceneStateParameters.CharacterSelectButtonsVisible = visible && !IsReady();
			UIManager.Get().HandleNewSceneStateParameter(characterSelectSceneStateParameters);
			m_charSettingsPanel.SetVisible(false);
			UIManager.SetGameObjectActive(m_readyBtn, !IsReady());
			m_readyBtn.spriteController.SetClickable(!IsReady());
			UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, IsReady());
			NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(IsReady());
			NavigationBar.Get().m_searchQueueText.text = string.Empty;
			if (visible)
			{
				m_charSettingsPanel.m_abilitiesSubPanel.RefreshSelectedMods();
			}
			if (!visible)
			{
				UIManager.SetGameObjectActive(m_buyButtonsContainer, false);
			}
			else
			{
				UpdateBuyButtons();
			}
			goto IL_0263;
		}
	}

	public void PlayMainLobbyControllerAnim(string animName, int layer)
	{
		for (int i = 0; i < m_mainLobbyControllers.Length; i++)
		{
			m_mainLobbyControllers[i].Play(animName, 1, 0f);
		}
		while (true)
		{
			return;
		}
	}

	public void UpdateSkinsPanel()
	{
		m_charSettingsPanel.m_skinsSubPanel.RePopulateCharacterData();
	}

	public void PurchaseCharacterTintResponseHandler(PurchaseTintResponse response)
	{
		if (!response.Success)
		{
			return;
		}
		while (true)
		{
			if (response.Result == PurchaseResult.Success)
			{
				while (true)
				{
					m_charSettingsPanel.m_skinsSubPanel.RePopulateCharacterData();
					AppState_CharacterSelect.Get().UpdateSelectedSkin(m_charSettingsPanel.m_skinsSubPanel.GetCurrentSelection());
					return;
				}
			}
			return;
		}
	}

	public void PurchaseCharacterTintResponseHandler(PurchaseTintForCashResponse response)
	{
		if (!response.Success)
		{
			return;
		}
		while (true)
		{
			if (response.Result == PurchaseResult.Success)
			{
				while (true)
				{
					m_charSettingsPanel.m_skinsSubPanel.RePopulateCharacterData();
					AppState_CharacterSelect.Get().UpdateSelectedSkin(m_charSettingsPanel.m_skinsSubPanel.GetCurrentSelection());
					return;
				}
			}
			return;
		}
	}

	public void PurchaseCharacterSkinResponseHandler(PurchaseSkinResponse response)
	{
		if (!response.Success)
		{
			return;
		}
		while (true)
		{
			if (response.Result == PurchaseResult.Success)
			{
				while (true)
				{
					AppState_CharacterSelect.Get().UpdateSelectedSkin(m_charSettingsPanel.m_skinsSubPanel.GetCurrentSelection());
					return;
				}
			}
			return;
		}
	}

	public void OnCharacterDataUpdated(PersistedCharacterData data)
	{
		UICharacterScreen.Get().RefreshCharacterButtons();
	}

	public void SearchQueueTextExit()
	{
		if (!(NavigationBar.Get() != null))
		{
			return;
		}
		while (true)
		{
			NavigationBar.Get().SearchQueueTextExit();
			return;
		}
	}

	public void HandleGameAssemblingUIChanges()
	{
		UIFrontEnd.Get().m_frontEndNavPanel.ToggleUiForGameStarting(false);
		UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, false);
		NavigationBar.Get().m_searchQueueText.text = string.Empty;
	}

	private void HandleGameInfoNotification(GameInfoNotification notification)
	{
		if (UIMatchStartPanel.Get() != null)
		{
			UIMatchStartPanel.Get().HandleGameStatusChanged(notification);
		}
		if (notification.GameInfo.GameStatus != GameStatus.LoadoutSelecting)
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, false);
			NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(false);
			UIManager.SetGameObjectActive(m_readyBtn, false);
			m_readyBtn.spriteController.SetClickable(false);
			UINewUserFlowManager.OnDoneWithReadyButton();
			return;
		}
	}

	private void HandleBankBalanceChange(CurrencyData data)
	{
		if (data.m_Type == CurrencyType.UnlockFreelancerToken)
		{
			UpdateBuyButtons();
		}
	}

	private bool IsSinglePlayer(GameInfoNotification notification)
	{
		return notification.GameInfo.GameConfig.GameType == GameType.Practice || notification.GameInfo.GameConfig.GameType == GameType.Solo || notification.GameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial);
	}

	public void CheckCancelBtnLabels()
	{
		bool isInCustomGame = SceneStateParameters.IsInCustomGame;
		if (!isInCustomGame)
		{
			return;
		}
		TextMeshProUGUI[] componentsInChildren = NavigationBar.Get().m_cancelBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			bool flag = true;
			if (componentsInChildren[i] == NavigationBar.Get().m_searchQueueText)
			{
				flag = false;
			}
			int num = 0;
			while (true)
			{
				if (num < NavigationBar.Get().m_timeInQueueLabel.Length)
				{
					if (NavigationBar.Get().m_timeInQueueLabel[num] == componentsInChildren[i])
					{
						flag = false;
						break;
					}
					num++;
					continue;
				}
				break;
			}
			if (!flag)
			{
				continue;
			}
			if (isInCustomGame)
			{
				componentsInChildren[i].alignment = TextAlignmentOptions.Center;
			}
			else
			{
				componentsInChildren[i].alignment = TextAlignmentOptions.Right;
			}
		}
	}

	public void RestoreFunctionalityOnGameStop(GameResult gameResult)
	{
		Log.Info("Restoring Functionality on game stop. GameResult: " + gameResult);
		NavigationBar.Get().m_searchQueueText.text = string.Empty;
		UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, true);
		CheckCancelBtnLabels();
		m_readyBtn.spriteController.SetClickable(true);
		NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(true);
		UIFrontEnd.Get().m_frontEndNavPanel.ToggleUiForGameStarting(true);
		AppState_GameTeardown.Get().Enter();
	}

	private void BuyInGameClicked(BaseEventData data)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.Character;
		uIPurchaseableItem.m_charLink = UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay;
		uIPurchaseableItem.m_currencyType = CurrencyType.FreelancerCurrency;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem, PurchaseCharacterResponseHandler);
	}

	private void BuyForCashClicked(BaseEventData data)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.Character;
		uIPurchaseableItem.m_charLink = UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay;
		uIPurchaseableItem.m_purchaseForCash = true;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem, PurchaseCharacterResponseHandler);
	}

	private void BuyForTokenClicked(BaseEventData data)
	{
		ClientGameManager.Get().PurchaseCharacter(CurrencyType.UnlockFreelancerToken, UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay, delegate(PurchaseCharacterResponse response)
		{
			PurchaseCharacterResponseHandler(response.Success, response.Result, response.CharacterType);
		});
	}

	private bool BuyTooltipSetup(UITooltipBase tooltip)
	{
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		uITitledTooltip.Setup(StringUtil.TR("BuyFreelancer", "CharacterSelect"), StringUtil.TR("BuyFreelancerDesc", "CharacterSelect"), string.Empty);
		return true;
	}

	public void OpenOneButtonDialog(string title, string message, UIDialogBox.DialogButtonCallback callback = null)
	{
		m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(title, message, StringUtil.TR("Ok", "Global"), callback);
	}

	public void PurchaseCharacterResponseHandler(bool success, PurchaseResult result, CharacterType characterType)
	{
		if (!success)
		{
			return;
		}
		while (true)
		{
			if (result == PurchaseResult.Success)
			{
				while (true)
				{
					UIManager.SetGameObjectActive(m_buyButtonsContainer, false);
					UICharacterScreen.Get().RefreshCharacterButtons();
					UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
					{
						ClientRequestToServerSelectCharacter = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay
					});
					return;
				}
			}
			return;
		}
	}

	private void Awake()
	{
		if (m_buyButtonsContainer != null)
		{
			m_buyInGameButton.spriteController.callback = BuyInGameClicked;
			m_buyInGameButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, BuyTooltipSetup);
			m_buyForCashButton.spriteController.callback = BuyForCashClicked;
			m_buyForCashButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, BuyTooltipSetup);
			m_buyForToken.spriteController.callback = BuyForTokenClicked;
			m_buyForToken.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, BuyTooltipSetup);
			m_buyButtonsHeader.text = StringUtil.TR("BuyFreelancer", "CharacterSelect");
		}
		s_instance = this;
		m_visible = false;
		for (int i = 0; i < m_mainLobbyControllers.Length; i++)
		{
			UIManager.SetGameObjectActive(m_mainLobbyControllers[i], false);
		}
		while (true)
		{
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				SideButtonsVisible = false,
				CharacterSelectButtonsVisible = false
			});
			UIManager.SetGameObjectActive(m_readyBtn, false);
			m_changeFreelancerBtn.spriteController.callback = ToggleCharacterSelect;
			m_readyBtn.spriteController.callback = ReadyButtonClickCallback;
			ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterDataUpdated;
			ClientGameManager.Get().OnGameInfoNotification += HandleGameInfoNotification;
			ClientGameManager.Get().OnBankBalanceChange += HandleBankBalanceChange;
			GameManager.Get().OnGameAssembling += HandleGameAssemblingUIChanges;
			GameManager.Get().OnGameStopped += RestoreFunctionalityOnGameStop;
			if (m_timeElapsedLabels.IsNullOrEmpty())
			{
				return;
			}
			while (true)
			{
				m_createGameTimestamps = new long[m_timeElapsedLabels.Length];
				for (int j = 0; j < m_timeElapsedLabels.Length; j++)
				{
					m_timeElapsedLabels[j].text = string.Empty;
				}
				while (true)
				{
					switch (7)
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

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnCharacterDataUpdated -= OnCharacterDataUpdated;
			ClientGameManager.Get().OnGameInfoNotification -= HandleGameInfoNotification;
			ClientGameManager.Get().OnBankBalanceChange -= HandleBankBalanceChange;
		}
		if (GameManager.Get() != null)
		{
			GameManager.Get().OnGameAssembling -= HandleGameAssemblingUIChanges;
			GameManager.Get().OnGameStopped -= RestoreFunctionalityOnGameStop;
		}
		if (!(m_messageBox != null))
		{
			return;
		}
		while (true)
		{
			m_messageBox.Close();
			m_messageBox = null;
			return;
		}
	}
}
