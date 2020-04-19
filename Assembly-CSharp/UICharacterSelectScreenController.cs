using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
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

	public Canvas m_nameLabelCanvas
	{
		get
		{
			return base.GetComponentInParent<Canvas>();
		}
	}

	public static UICharacterSelectScreenController Get()
	{
		return UICharacterSelectScreenController.s_instance;
	}

	public void Start()
	{
		this.m_preReadyActions = new Queue<Action>();
		this.m_groupMenuClickListeners[0].Setup(TooltipType.PlayerGroupMenu, (UITooltipBase tooltip) => this.OnGroupMenuClick(tooltip, 0), null);
		this.m_groupMenuClickListeners[1].Setup(TooltipType.PlayerGroupMenu, (UITooltipBase tooltip) => this.OnGroupMenuClick(tooltip, 1), null);
		this.m_groupMenuClickListeners[2].Setup(TooltipType.PlayerGroupMenu, (UITooltipBase tooltip) => this.OnGroupMenuClick(tooltip, 2), null);
		this.m_groupMenuClickListeners[3].Setup(TooltipType.PlayerGroupMenu, (UITooltipBase tooltip) => this.OnGroupMenuClick(tooltip, 3), null);
		this.m_changeFreelancerBtn.spriteController.pointerEnterCallback = new _ButtonSwapSprite.ButtonClickCallback(this.HighlightCharacter);
		this.m_changeFreelancerBtn.spriteController.pointerExitCallback = new _ButtonSwapSprite.ButtonClickCallback(this.UnhighlightCharacter);
		this.m_highlightFreelancerBtn.spriteController.pointerEnterCallback = new _ButtonSwapSprite.ButtonClickCallback(this.HighlightCharacter);
		this.m_highlightFreelancerBtn.spriteController.pointerExitCallback = new _ButtonSwapSprite.ButtonClickCallback(this.UnhighlightCharacter);
		UIEventTriggerUtils.AddListener(this.m_highlightFreelancerImage.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.HighlightCharacter));
		UIEventTriggerUtils.AddListener(this.m_highlightFreelancerImage.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.UnhighlightCharacter));
		this.m_cannotReadyBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
		{
			UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
			uititledTooltip.Setup(StringUtil.TR("CannotReady", "Global"), this.GetCannotReadyReason(), string.Empty);
			return true;
		}, null);
		UIManager.SetGameObjectActive(this.m_cannotReadyBtn, false, null);
		for (int i = 0; i < this.m_nameLabels.Length; i++)
		{
			this.m_nameLabels[i].raycastTarget = false;
		}
		for (int j = 0; j < this.m_accountNameLabels.Length; j++)
		{
			this.m_accountNameLabels[j].raycastTarget = false;
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.Start()).MethodHandle;
		}
		for (int k = 0; k < this.m_timeElapsedLabels.Length; k++)
		{
			this.m_timeElapsedLabels[k].raycastTarget = false;
		}
	}

	public void SetupReadyButton()
	{
		if (this.CanReady())
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.SetupReadyButton()).MethodHandle;
			}
			bool flag = true;
			if (this.m_readyBtn.m_animationController.isActiveAndEnabled)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				AnimatorClipInfo[] currentAnimatorClipInfo = this.m_readyBtn.m_animationController.GetCurrentAnimatorClipInfo(1);
				if (currentAnimatorClipInfo != null)
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
					if (currentAnimatorClipInfo.Length > 0)
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
						if (currentAnimatorClipInfo[0].clip.name == "readyBtnOnIN" || currentAnimatorClipInfo[0].clip.name == "readyBtnOnIDLE")
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					this.m_readyBtn.m_animationController.Play("readyBtnOnIN", 1, 0f);
				}
			}
			this.m_readyBtn.spriteController.SetClickable(true);
			UIManager.SetGameObjectActive(this.m_readyBtn, true, null);
		}
	}

	public void NotifyCharacterDoneLoading()
	{
		this.m_characterLoading = false;
		this.UpdateReadyButton();
	}

	private bool OnGroupMenuClick(UITooltipBase tooltip, int indexClicked)
	{
		UIPlayerPanelGroupMenu uiplayerPanelGroupMenu = tooltip as UIPlayerPanelGroupMenu;
		List<UpdateGroupMemberData> members = ClientGameManager.Get().GroupInfo.Members;
		if (GameManager.Get() != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.OnGroupMenuClick(UITooltipBase, int)).MethodHandle;
			}
			if (GameManager.Get().GameStatus != GameStatus.Stopped)
			{
				GameManager gameManager = GameManager.Get();
				LobbyPlayerInfo playerInfo = gameManager.PlayerInfo;
				Team team = playerInfo.TeamId;
				if (team == Team.Spectator)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					team = Team.TeamA;
				}
				List<LobbyPlayerInfo> list = gameManager.TeamInfo.TeamInfo(team).OrderBy(delegate(LobbyPlayerInfo ti)
				{
					int result;
					if (ti.PlayerId == playerInfo.PlayerId)
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
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle2 = methodof(UICharacterSelectScreenController.<OnGroupMenuClick>c__AnonStorey0.<>m__0(LobbyPlayerInfo)).MethodHandle;
						}
						result = 0;
					}
					else
					{
						result = 1;
					}
					return result;
				}).ToList<LobbyPlayerInfo>();
				int num = 1;
				int index = 0;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].AccountId == ClientGameManager.Get().GetPlayerAccountData().AccountId)
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
						if (indexClicked == 0)
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
				uiplayerPanelGroupMenu.Setup(list[index]);
				return true;
			}
		}
		if (members.Count > 1)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (indexClicked < members.Count)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				uiplayerPanelGroupMenu.Setup(members[indexClicked]);
				return true;
			}
		}
		return false;
	}

	public void HighlightCharacter(BaseEventData data)
	{
		UIActorModelData componentInChildren = UICharacterSelectWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
		if (componentInChildren != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.HighlightCharacter(BaseEventData)).MethodHandle;
			}
			componentInChildren.SetMouseIsOver(true);
		}
	}

	public void UnhighlightCharacter(BaseEventData data)
	{
		UIActorModelData componentInChildren = UICharacterSelectWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
		if (componentInChildren != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.UnhighlightCharacter(BaseEventData)).MethodHandle;
			}
			componentInChildren.SetMouseIsOver(false);
		}
	}

	public void NotifyGameIsLoading()
	{
		this.m_charSettingsPanel.SetVisible(false, UICharacterSelectCharacterSettingsPanel.TabPanel.None);
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			SideButtonsClickable = new bool?(false)
		});
	}

	private bool CanReady()
	{
		string text;
		return this.CanReady(out text, false);
	}

	private bool IsReady()
	{
		if (!(AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.IsReady()).MethodHandle;
			}
			if (!(AppState_LandingPage.Get() == AppState.GetCurrent()))
			{
				return AppState_CharacterSelect.IsReady();
			}
		}
		return AppState_GroupCharacterSelect.Get().IsReady();
	}

	private unsafe bool CanReady(out string reason, bool needsReason)
	{
		reason = string.Empty;
		if (AppState.GetCurrent() != AppState_CharacterSelect.Get() && AppState.GetCurrent() != AppState_GroupCharacterSelect.Get())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.CanReady(string*, bool)).MethodHandle;
			}
			if (needsReason)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				reason = string.Empty;
			}
			return true;
		}
		if (this.IsReady())
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
			if (needsReason)
			{
				reason = StringUtil.TR("AlreadyReady", "Global");
			}
			return false;
		}
		if (this.m_characterLoading)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (needsReason)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				reason = StringUtil.TR("LOADING", "Global");
			}
			return false;
		}
		CharacterResourceLink characterResourceLinkOfCharacterTypeToDisplay = UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay;
		if (characterResourceLinkOfCharacterTypeToDisplay == null)
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
			if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.RankedModeSelect)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (needsReason)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					reason = StringUtil.TR("SelectFreelancer", "Global");
				}
				return false;
			}
		}
		GameManager gameManager = GameManager.Get();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		GameType gameType;
		if (gameManager != null && gameManager.GameConfig != null)
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
			if (gameManager.GameStatus != GameStatus.Stopped)
			{
				gameType = gameManager.GameConfig.GameType;
				goto IL_169;
			}
		}
		gameType = clientGameManager.GroupInfo.SelectedQueueType;
		IL_169:
		GameType gameType2 = gameType;
		if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.RankedModeSelect)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag;
			if (gameManager != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				flag = (gameManager.GameStatus != GameStatus.Stopped);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			bool flag3 = flag2 && gameManager.GameConfig.GameType == GameType.Custom;
			if (!flag2)
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
				if (!gameManager.IsValidForHumanPreGameSelection(characterResourceLinkOfCharacterTypeToDisplay.m_characterType))
				{
					goto IL_21B;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (!flag2)
			{
				goto IL_2B7;
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
			if (gameManager.IsCharacterAllowedForPlayers(characterResourceLinkOfCharacterTypeToDisplay.m_characterType))
			{
				goto IL_2B7;
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
			IL_21B:
			if (flag3)
			{
				if (gameManager.IsCharacterAllowedForGameType(characterResourceLinkOfCharacterTypeToDisplay.m_characterType, gameType2, null, null))
				{
					goto IL_2B7;
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (flag3)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (needsReason)
				{
					LocalizationArg_Freelancer localizationArg_Freelancer = LocalizationArg_Freelancer.Create(characterResourceLinkOfCharacterTypeToDisplay.m_characterType);
					LocalizationArg_GameType localizationArg_GameType = LocalizationArg_GameType.Create(gameType2);
					reason = LocalizationPayload.Create("GameTypeDoesNotAllowCharacterType", "Global", new LocalizationArg[]
					{
						localizationArg_Freelancer,
						localizationArg_GameType
					}).ToString();
				}
				return false;
			}
			if (needsReason)
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
				reason = StringUtil.TR("FreelancerDenied", "Global");
			}
			return false;
			IL_2B7:
			if (!gameManager.IsCharacterAllowedForGameType(characterResourceLinkOfCharacterTypeToDisplay.m_characterType, gameType2, null, null))
			{
				if (needsReason)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					LocalizationArg_Freelancer localizationArg_Freelancer2 = LocalizationArg_Freelancer.Create(characterResourceLinkOfCharacterTypeToDisplay.m_characterType);
					LocalizationArg_GameType localizationArg_GameType2 = LocalizationArg_GameType.Create(gameType2);
					reason = LocalizationPayload.Create("GameTypeDoesNotAllowCharacterType", "Global", new LocalizationArg[]
					{
						localizationArg_Freelancer2,
						localizationArg_GameType2
					}).ToString();
				}
				return false;
			}
		}
		if (UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay != null)
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
			if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.RankedModeSelect)
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
				CharacterType characterTypeToDisplay = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay;
				if (!clientGameManager.IsCharacterAvailable(characterTypeToDisplay, gameType2))
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!characterTypeToDisplay.IsWillFill())
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
						if (!(gameManager != null))
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
							if (clientGameManager.GroupInfo != null)
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
								if (clientGameManager.GroupInfo.SelectedQueueType == GameType.Practice)
								{
									goto IL_482;
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
						}
						bool flag4 = false;
						if (gameManager.GameConfig != null && gameManager.GameStatus != GameStatus.Stopped && gameManager.GameConfig.GameType == GameType.Custom)
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
							bool flag5;
							if (!gameManager.GameConfig.GameOptionFlags.HasGameOption(GameOptionFlag.AllowDuplicateCharacters) && !gameManager.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AllowPlayingLockedCharacters))
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
								flag5 = gameManager.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.OverrideFreelancerSelection);
							}
							else
							{
								flag5 = true;
							}
							flag4 = flag5;
						}
						if (!flag4)
						{
							if (needsReason)
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
								reason = StringUtil.TR("FreelancerNotAvailable", "Global");
							}
							return false;
						}
					}
				}
			}
		}
		IL_482:
		if (AppState.GetCurrent() == AppState_CharacterSelect.Get())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (gameManager.TeamPlayerInfo != null)
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
				bool flag6 = false;
				if (gameManager.GameConfig != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (gameManager.GameConfig.GameType == GameType.Custom)
					{
						bool flag7;
						if (!gameManager.GameConfig.GameOptionFlags.HasGameOption(GameOptionFlag.AllowDuplicateCharacters))
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
							if (!gameManager.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.RankedFreelancerSelection))
							{
								flag7 = gameManager.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.OverrideFreelancerSelection);
								goto IL_529;
							}
						}
						flag7 = true;
						IL_529:
						flag6 = flag7;
					}
				}
				if (!flag6)
				{
					using (List<LobbyPlayerInfo>.Enumerator enumerator = gameManager.TeamPlayerInfo.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
							if (lobbyPlayerInfo.PlayerId != gameManager.PlayerInfo.PlayerId)
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
								if (!lobbyPlayerInfo.IsNPCBot)
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
									if (characterResourceLinkOfCharacterTypeToDisplay != null)
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
										if (lobbyPlayerInfo.CharacterType == characterResourceLinkOfCharacterTypeToDisplay.m_characterType && lobbyPlayerInfo.ReadyState == ReadyState.Ready)
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
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
			}
		}
		else if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
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
			if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.RankedModeSelect)
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
				GameTypeAvailability gameTypeAvailability2;
				if (clientGameManager.GroupInfo.InAGroup)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					long accountId = clientGameManager.GetPlayerAccountData().AccountId;
					int num = 0;
					if (!this.DoesASelectedGame_OverrideFreelancerSelection)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						foreach (UpdateGroupMemberData updateGroupMemberData in clientGameManager.GroupInfo.Members)
						{
							if (updateGroupMemberData.AccountID != accountId)
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
								if (updateGroupMemberData.MemberDisplayCharacter == clientGameManager.GroupInfo.ChararacterInfo.CharacterType)
								{
									for (;;)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
									if (updateGroupMemberData.IsReady)
									{
										for (;;)
										{
											switch (7)
											{
											case 0:
												continue;
											}
											break;
										}
										num++;
									}
								}
							}
						}
					}
					if (num > 0)
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
						if (!clientGameManager.GroupInfo.ChararacterInfo.CharacterType.IsWillFill())
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (needsReason)
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
								reason = StringUtil.TR("GroupConflict", "Global");
							}
							return false;
						}
						GameTypeAvailability gameTypeAvailability;
						if (clientGameManager.GameTypeAvailabilies.TryGetValue(clientGameManager.GroupInfo.SelectedQueueType, out gameTypeAvailability))
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
							int maxWillFillPerTeam = gameTypeAvailability.MaxWillFillPerTeam;
							if (maxWillFillPerTeam < num + 1)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								if (needsReason)
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									LocalizationArg_GameType localizationArg_GameType3 = LocalizationArg_GameType.Create(gameType2);
									LocalizationArg_Int32 localizationArg_Int = LocalizationArg_Int32.Create(maxWillFillPerTeam);
									reason = LocalizationPayload.Create("GroupHasTooManyWillFill", "Matchmaking", new LocalizationArg[]
									{
										localizationArg_GameType3,
										localizationArg_Int
									}).ToString();
								}
								return false;
							}
						}
						else
						{
							Log.Warning("We have no GameTypeAvailability for {0}, why so?", new object[]
							{
								clientGameManager.GroupInfo.SelectedQueueType
							});
						}
					}
				}
				else if (clientGameManager.GameTypeAvailabilies.TryGetValue(clientGameManager.GroupInfo.SelectedQueueType, out gameTypeAvailability2))
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (gameTypeAvailability2.MaxWillFillPerTeam == 0)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay.IsWillFill())
						{
							if (needsReason)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								LocalizationArg_GameType localizationArg_GameType4 = LocalizationArg_GameType.Create(gameType2);
								reason = LocalizationPayload.Create("QueueDoesNotAllowWillFill", "Matchmaking", new LocalizationArg[]
								{
									localizationArg_GameType4
								}).ToString();
							}
							return false;
						}
					}
				}
				else
				{
					Log.Warning("We have no GameTypeAvailability for {0}, why?", new object[]
					{
						clientGameManager.GroupInfo.SelectedQueueType
					});
				}
			}
		}
		if (needsReason)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			LocalizationPayload blockingQueueRestriction = clientGameManager.GetBlockingQueueRestriction(gameType2);
			if (blockingQueueRestriction != null)
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
				reason = blockingQueueRestriction.ToString();
				return false;
			}
		}
		else if (!clientGameManager.MeetsAllQueueRequirements(gameType2))
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
			return false;
		}
		reason = string.Empty;
		return true;
	}

	public void CancelButtonClickCallback(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.Cancel);
		if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.CharacterSelect)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.CancelButtonClickCallback(BaseEventData)).MethodHandle;
			}
			if (UIFrontEnd.Get().m_currentScreen == FrontEndScreenState.RankedModeSelect)
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
			else
			{
				if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.GroupCharacterSelect)
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
					if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.LandingPage)
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
						if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.RankedModeSelect)
						{
							return;
						}
					}
				}
				if (this.IsReady())
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					UIManager.SetGameObjectActive(this.m_readyBtn, true, null);
					this.SearchQueueTextExit();
					NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnOUT", 0, 0f);
					this.m_readyBtn.m_animationController.Play("readyBtnOnIN", 1, 0f);
					this.m_readyBtn.spriteController.SetClickable(true);
					NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(false);
					this.SetReady(false, true);
					return;
				}
				return;
			}
		}
		if (this.IsReady())
		{
			UIManager.SetGameObjectActive(this.m_readyBtn, true, null);
			this.SearchQueueTextExit();
			NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnOUT", 0, 0f);
			this.m_readyBtn.m_animationController.Play("readyBtnOnIN", 1, 0f);
			this.m_readyBtn.spriteController.SetClickable(true);
			NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(false);
			this.SetReady(false, true);
		}
	}

	public void NotifiedEnteredQueue()
	{
		this.m_needToRepickCharacter = false;
	}

	public bool RepickingCharacter()
	{
		return this.m_needToRepickCharacter;
	}

	public void AllowCharacterSwapForConflict()
	{
		this.m_needToRepickCharacter = true;
		this.SearchQueueTextExit();
		NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnOUT", 0, 0f);
		this.m_readyBtn.m_animationController.Play("readyBtnOnIN", 1, 0f);
		this.m_readyBtn.spriteController.SetClickable(true);
		UIManager.SetGameObjectActive(this.m_readyBtn, true, null);
		NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(false);
		this.SetCharacterSelectVisible(true);
		Log.Info("Setting self not ready because I need to re pick characters", new object[0]);
		this.SetReady(false, true);
	}

	public void CancelButtonAnimDone()
	{
		UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, false, null);
	}

	private bool ReadyModeSupportsCancel()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (GameManager.Get().GameStatus.IsActiveStatus())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.ReadyModeSupportsCancel()).MethodHandle;
			}
			if (clientGameManager.PlayerInfo != null)
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
				if (clientGameManager.PlayerInfo.TeamId == Team.Spectator)
				{
					return false;
				}
			}
		}
		if (!GameManager.Get().GameStatus.IsActiveStatus())
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
			if (clientGameManager.GroupInfo.SelectedQueueType != GameType.Solo)
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
				if (clientGameManager.GroupInfo.SelectedQueueType != GameType.Practice)
				{
					return true;
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
			}
			return false;
		}
		return true;
	}

	public void DoReadyClick(FrontEndButtonSounds soundToPlay = FrontEndButtonSounds.StartGameReady)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		ushort num = 0;
		GameType gameType;
		if (GameManager.Get().GameConfig != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.DoReadyClick(FrontEndButtonSounds)).MethodHandle;
			}
			if (GameManager.Get().GameStatus != GameStatus.Stopped)
			{
				gameType = GameManager.Get().GameConfig.GameType;
				num = GameManager.Get().GameConfig.InstanceSubTypeBit;
				goto IL_81;
			}
		}
		gameType = clientGameManager.GroupInfo.SelectedQueueType;
		IL_81:
		if (!UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay.IsWillFill())
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag = true;
			bool flag2 = false;
			ushort soloSubGameMask = clientGameManager.GetSoloSubGameMask(gameType);
			using (Dictionary<ushort, GameSubType>.Enumerator enumerator = clientGameManager.GetGameTypeSubTypes(gameType).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ushort, GameSubType> keyValuePair = enumerator.Current;
					if ((soloSubGameMask & keyValuePair.Key) != 0)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (keyValuePair.Value.Mods != null)
						{
							if (keyValuePair.Value.Mods.Contains(GameSubType.SubTypeMods.OverrideFreelancerSelection))
							{
								goto IL_14E;
							}
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (keyValuePair.Value.Mods.Contains(GameSubType.SubTypeMods.RankedFreelancerSelection))
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									goto IL_14E;
								}
							}
							IL_151:
							if (keyValuePair.Value.Mods.Contains(GameSubType.SubTypeMods.StricterMods))
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
								flag2 = true;
								continue;
							}
							continue;
							IL_14E:
							flag = false;
							goto IL_151;
						}
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (flag)
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
				bool flag3 = false;
				if (UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.ActorDataPrefab != null)
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
					flag3 = AbilityModHelper.HasAnyAvailableMods(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.ActorDataPrefab.GetComponent<ActorData>());
				}
				CharacterModInfo characterModInfo;
				if (flag2)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					characterModInfo = clientGameManager.GetPlayerCharacterData(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay).CharacterComponent.LastRankedMods;
				}
				else
				{
					characterModInfo = clientGameManager.GetPlayerCharacterData(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay).CharacterComponent.LastMods;
				}
				if (flag3)
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
					if (characterModInfo.ModForAbility0 < 0)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (characterModInfo.ModForAbility1 < 0 && characterModInfo.ModForAbility2 < 0 && characterModInfo.ModForAbility3 < 0 && characterModInfo.ModForAbility4 < 0)
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
							this.m_preReadyActions.Enqueue(delegate
							{
								UIDialogPopupManager.OpenTwoButtonDialog(string.Empty, StringUtil.TR("NoModSelectedWarning", "Global"), StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate(UIDialogBox UIDialogBox)
								{
									this.ReadyHelper(soundToPlay);
								}, delegate(UIDialogBox UIDialogBox)
								{
									this.m_preReadyActions.Clear();
								}, false, false);
							});
						}
					}
				}
			}
		}
		Dictionary<ushort, GameSubType> gameTypeSubTypes = clientGameManager.GetGameTypeSubTypes(gameType);
		if (!gameTypeSubTypes.IsNullOrEmpty<KeyValuePair<ushort, GameSubType>>())
		{
			if (num == 0)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (gameTypeSubTypes.Count > 1)
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
					Log.Warning("FAKING Selection of SubType for {0} XP penalty warning", new object[]
					{
						gameType
					});
				}
			}
			GameTypeAvailability gameTypeAvailability;
			if (clientGameManager.GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability))
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
				DateTime dateTime;
				if (!gameTypeAvailability.XPPenaltyTimeout.IsNullOrEmpty<KeyValuePair<ushort, DateTime>>() && gameTypeAvailability.XPPenaltyTimeout.TryGetValue(num, out dateTime))
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
					if (dateTime > clientGameManager.UtcNow())
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						TimeSpan span = dateTime - clientGameManager.UtcNow();
						string arg = LocalizationArg_TimeSpan.Create(span).TR();
						string timeString = string.Format(StringUtil.TR("NoExperienceForThisGameTypeForHoursMinutes", "Global"), gameType.GetDisplayName(), arg);
						this.m_preReadyActions.Enqueue(delegate
						{
							UIDialogPopupManager.OpenTwoButtonDialog(string.Empty, timeString, StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate(UIDialogBox UIDialogBox)
							{
								this.ReadyHelper(soundToPlay);
							}, delegate(UIDialogBox UIDialogBox)
							{
								this.m_preReadyActions.Clear();
							}, false, false);
						});
					}
				}
			}
		}
		this.ReadyHelper(soundToPlay);
	}

	private void ReadyHelper(FrontEndButtonSounds soundToPlay)
	{
		if (this.m_preReadyActions.Count > 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.ReadyHelper(FrontEndButtonSounds)).MethodHandle;
			}
			this.m_preReadyActions.Dequeue()();
			return;
		}
		if (UIFrontEnd.Get().m_currentScreen == FrontEndScreenState.CharacterSelect)
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
			if (this.CanReady())
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(this.m_readyBtn, true, null);
				if (!this.m_needToRepickCharacter)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.ReadyModeSupportsCancel())
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
						UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, true, null);
						this.CheckCancelBtnLabels();
					}
				}
				NavigationBar.Get().m_searchQueueText.text = string.Empty;
				UIFrontEnd.PlaySound(soundToPlay);
				GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndReady, null);
				if (this.ReadyModeSupportsCancel())
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
					this.SearchQueueTextExit();
					NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnIN", 0, 0f);
				}
				this.m_readyBtn.m_animationController.Play("readyBtnOffIN", 1, 0f);
				this.m_readyBtn.spriteController.SetClickable(false);
				NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(true);
				this.SetReady(true, true);
			}
		}
		else if (UIFrontEnd.Get().m_currentScreen == FrontEndScreenState.GroupCharacterSelect)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (AppState.GetCurrent() != AppState_CharacterSelect.Get())
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				UIMatchStartPanel.Get().NotifyDuplicateFreelancer(false);
			}
			if (this.CanReady())
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
				UIFrontEnd.PlaySound(soundToPlay);
				GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndReady, null);
				if (this.ReadyModeSupportsCancel())
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					this.SearchQueueTextExit();
					NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnIN", 0, 0f);
				}
				this.m_readyBtn.m_animationController.Play("readyBtnOffIN", 1, 0f);
				UIManager.SetGameObjectActive(this.m_readyBtn, false, null);
				this.m_readyBtn.spriteController.SetClickable(false);
				if (!this.m_needToRepickCharacter)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.ReadyModeSupportsCancel())
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, true, null);
						this.CheckCancelBtnLabels();
					}
				}
				NavigationBar.Get().m_searchQueueText.text = string.Empty;
				NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(true);
				this.SetReady(true, true);
			}
		}
		else if (UIFrontEnd.Get().m_currentScreen == FrontEndScreenState.RankedModeSelect)
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
			if (this.CanReady())
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
				UIFrontEnd.PlaySound(soundToPlay);
				GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndReady, null);
				if (this.ReadyModeSupportsCancel())
				{
					this.SearchQueueTextExit();
					NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnIN", 0, 0f);
				}
				this.m_readyBtn.m_animationController.Play("readyBtnOffIN", 1, 0f);
				UIManager.SetGameObjectActive(this.m_readyBtn, false, null);
				this.m_readyBtn.spriteController.SetClickable(false);
				if (!this.m_needToRepickCharacter)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.ReadyModeSupportsCancel())
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
						UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, true, null);
						this.CheckCancelBtnLabels();
					}
				}
				NavigationBar.Get().m_searchQueueText.text = string.Empty;
				NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(true);
				this.SetReady(true, true);
			}
		}
		this.m_needToRepickCharacter = false;
		UINewUserFlowManager.OnDoneWithReadyButton();
	}

	public void ReadyButtonClickCallback(BaseEventData data)
	{
		this.DoReadyClick(FrontEndButtonSounds.StartGameReady);
	}

	public void SetReady(bool ready, bool UpdateSideMenus = true)
	{
		if (this.m_characterLoading)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.SetReady(bool, bool)).MethodHandle;
			}
			return;
		}
		if (UICharacterSelectWorldObjects.Get().CharacterIsLoading())
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_characterLoading = true;
		}
		else
		{
			UICharacterSelectWorldObjects.Get().SetCharacterReady(0, ready);
			UICharacterSelectWorldObjects.Get().SetReadyPose();
		}
		if (ready)
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
			UICharacterSelectWorldObjects.Get().m_ringAnimations[0].PlayAnimation("ReadyIn");
			if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
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
				AppState_GroupCharacterSelect.Get().UpdateReadyState(true);
			}
			else if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
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
				AppState_CharacterSelect.Get().UpdateReadyState(true);
			}
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				CharacterSelectButtonsVisible = new bool?(false)
			});
			this.m_charSettingsPanel.SetVisible(false, UICharacterSelectCharacterSettingsPanel.TabPanel.None);
		}
		else
		{
			UICharacterSelectWorldObjects.Get().m_ringAnimations[0].PlayAnimation("ReadyOut");
			if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
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
				AppState_CharacterSelect.Get().UpdateReadyState(false);
			}
			else
			{
				if (!(AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()))
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(AppState_LandingPage.Get() == AppState.GetCurrent()))
					{
						goto IL_195;
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				AppState_GroupCharacterSelect.Get().UpdateReadyState(false);
			}
		}
		IL_195:
		UICharacterScreen.Get().RefreshCharacterButtons();
	}

	private void PlayCharacterSelectButtonAnimation(bool toVisible)
	{
	}

	public void SetCharacterSelectVisible(bool visible)
	{
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			CharacterSelectButtonsVisible = new bool?(visible)
		});
	}

	public void ToggleCharacterSelect(BaseEventData data)
	{
		if (!UIFrontEnd.Get().IsDraggingModel() && Input.GetMouseButtonUp(0))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.ToggleCharacterSelect(BaseEventData)).MethodHandle;
			}
			bool? characterSelectButtonsVisible = UICharacterScreen.GetCurrentSpecificState().CharacterSelectButtonsVisible;
			this.SetCharacterSelectVisible(!characterSelectButtonsVisible.Value);
		}
	}

	public void QuickPlaySetup(LobbyGameInfo gameInfo)
	{
		if (!(ClientGameManager.Get() == null) && ClientGameManager.Get().GroupInfo != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.QuickPlaySetup(LobbyGameInfo)).MethodHandle;
			}
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				return;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public bool IsCharacterSelectable(CharacterResourceLink selectedCharacter)
	{
		bool result = false;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (selectedCharacter != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.IsCharacterSelectable(CharacterResourceLink)).MethodHandle;
			}
			if (clientGameManager != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay != null)
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
					if (GameManager.Get() != null)
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
						GameManager gameManager = GameManager.Get();
						GameType gameType;
						if (gameManager != null)
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
							if (gameManager.GameConfig != null)
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
								if (gameManager.GameStatus != GameStatus.Stopped)
								{
									gameType = gameManager.GameConfig.GameType;
									goto IL_D8;
								}
							}
						}
						gameType = clientGameManager.GroupInfo.SelectedQueueType;
						IL_D8:
						GameType gameType2 = gameType;
						result = clientGameManager.IsCharacterAvailable(selectedCharacter.m_characterType, gameType2);
					}
				}
			}
		}
		return result;
	}

	private void Update()
	{
		if (!(AppState_CharacterSelect.Get() == AppState.GetCurrent()))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.Update()).MethodHandle;
			}
			if (!(AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()))
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
				if (!(AppState_LandingPage.Get() == AppState.GetCurrent()))
				{
					goto IL_380;
				}
			}
		}
		if (!UIGameSettingsPanel.Get().m_lastVisible)
		{
			for (int i = 0; i < this.m_nameLabels.Length; i++)
			{
				if (i < UICharacterSelectWorldObjects.Get().m_ringAnimations.Length)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (UIManager.Get().GetEnvirontmentCamera() != null)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (UICharacterSelectWorldObjects.Get().m_ringAnimations[i].m_nameObject.activeInHierarchy)
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
							UIManager.SetGameObjectActive(this.m_accountNameLabels[i], true, null);
							UIManager.SetGameObjectActive(this.m_nameLabels[i], true, null);
							if (!this.m_timeElapsedLabels.IsNullOrEmpty<TextMeshProUGUI>())
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
								UIManager.SetGameObjectActive(this.m_timeElapsedLabels[i], AppState.GetCurrent() == AppState_GroupCharacterSelect.Get(), null);
							}
							Vector3 vector = UIManager.Get().GetEnvirontmentCamera().WorldToViewportPoint(UICharacterSelectWorldObjects.Get().m_ringAnimations[i].m_nameObject.transform.position);
							Vector2 anchoredPosition = new Vector2(vector.x * (this.m_nameLabelCanvas.gameObject.transform as RectTransform).sizeDelta.x, vector.y * (this.m_nameLabelCanvas.gameObject.transform as RectTransform).sizeDelta.y);
							(this.m_nameLabels[i].transform as RectTransform).anchoredPosition = anchoredPosition;
						}
						else
						{
							UIManager.SetGameObjectActive(this.m_nameLabels[i], false, null);
							UIManager.SetGameObjectActive(this.m_timeElapsedLabels[i], false, null);
						}
					}
				}
			}
			GameManager gameManager = GameManager.Get();
			bool flag = false;
			if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_requireGroupUpdate)
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
					if (!UIMatchStartPanel.Get().IsVisible())
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
						if (!GameManager.Get().IsGameLoading())
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
							this.m_requireGroupUpdate = false;
							this.UpdateGroupMemberCharacters(gameManager.PlayerInfo);
						}
					}
				}
				flag = AppState_GroupCharacterSelect.Get().IsReady();
				UIPlayCategoryMenu.Get().SetMenuButtonsClickable(!flag && !UIMatchStartPanel.IsMatchCountdownStarting());
				UIManager.SetGameObjectActive(this.m_changeFreelancerBtn, !flag, null);
			}
			else if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
			{
				flag = AppState_CharacterSelect.IsReady();
				UIPlayCategoryMenu uiplayCategoryMenu = UIPlayCategoryMenu.Get();
				bool menuButtonsClickable;
				if (flag)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					menuButtonsClickable = !UIMatchStartPanel.IsMatchCountdownStarting();
				}
				else
				{
					menuButtonsClickable = false;
				}
				uiplayCategoryMenu.SetMenuButtonsClickable(menuButtonsClickable);
			}
			Component changeFreelancerBtn = this.m_changeFreelancerBtn;
			bool doActive;
			if (!flag)
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
				if (!UIStorePanel.Get().IsVisible())
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
					doActive = !UIPlayerProgressPanel.Get().IsVisible();
					goto IL_36F;
				}
			}
			doActive = false;
			IL_36F:
			UIManager.SetGameObjectActive(changeFreelancerBtn, doActive, null);
			this.UpdateTimeElapsedLabels();
			goto IL_409;
		}
		IL_380:
		for (int j = 0; j < this.m_nameLabels.Length; j++)
		{
			UIManager.SetGameObjectActive(this.m_nameLabels[j], false, null);
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int k = 0; k < this.m_accountNameLabels.Length; k++)
		{
			UIManager.SetGameObjectActive(this.m_accountNameLabels[k], false, null);
		}
		for (int l = 0; l < this.m_timeElapsedLabels.Length; l++)
		{
			UIManager.SetGameObjectActive(this.m_timeElapsedLabels[l], false, null);
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
		IL_409:
		this.UpdateReadyCancelButtonStates();
		if (AppState_RankModeDraft.Get() == AppState.GetCurrent())
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
			this.SetVisible(false, false);
		}
		if (GameManager.Get() != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!GameManager.Get().GameplayOverrides.DisableControlPadInput)
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
				if (this.m_readyBtn.spriteController.IsClickable())
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (Input.GetButtonDown("GamepadButtonY"))
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
						this.DoReadyClick(FrontEndButtonSounds.StartGameReady);
					}
				}
				if (NavigationBar.Get().m_cancelBtn.spriteController.IsClickable())
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
					if (Input.GetButtonDown("GamepadButtonB"))
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
						this.CancelButtonClickCallback(null);
					}
				}
			}
		}
	}

	private void UpdateGroupMemberCharacters(LobbyPlayerInfo playerInfo)
	{
		LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
		int num = UICharacterSelectWorldObjects.Get().m_ringAnimations.Length;
		int i = 0;
		if (!groupInfo.InAGroup)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.UpdateGroupMemberCharacters(LobbyPlayerInfo)).MethodHandle;
			}
			bool isReady = false;
			if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
			{
				isReady = AppState_GroupCharacterSelect.Get().IsReady();
			}
			if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				isReady = AppState_CharacterSelect.IsReady();
			}
			UICharacterScreen.CharacterSelectSceneStateParameters currentSpecificState = UICharacterScreen.GetCurrentSpecificState();
			groupInfo.ChararacterInfo.CharacterSkin = currentSpecificState.CharacterVisualInfoToDisplay;
			this.SetClientCharacter(0, groupInfo.MemberDisplayName, currentSpecificState.CharacterResourceLinkOfCharacterTypeToDisplay, groupInfo.ChararacterInfo, isReady, false, -1L);
			i++;
		}
		else if (playerInfo != null)
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
			if (playerInfo.TeamId == Team.Spectator)
			{
				bool isReady2 = true;
				this.SetClientCharacter(-1, playerInfo.GetHandle(), null, null, isReady2, false, -1L);
			}
		}
		int num2 = 1;
		for (int j = 0; j < groupInfo.Members.Count; j++)
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
				else if (!ClientGameManager.Get().IsWaitingForSkinResponse())
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (groupInfo.ChararacterInfo.CharacterType == characterResourceLink.m_characterType)
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
						if (UICharacterSelectWorldObjects.Get().CharacterTypeInSlot(0) == characterResourceLink.m_characterType)
						{
							goto IL_253;
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
					this.SetClientCharacter(0, groupInfo.Members[j].MemberDisplayName, characterResourceLink, groupInfo.ChararacterInfo, isReady3, isInGame, createGameTimestamp);
				}
				IL_253:;
			}
			else
			{
				this.SetCharacter(num2, groupInfo.Members[j].MemberDisplayName, characterResourceLink, groupInfo.Members[j].VisualInfo, isBot, isReady3, isInGame, createGameTimestamp);
				num2++;
			}
			i++;
		}
		UICharacterSelectWorldObjects.Get().SetReadyPose();
		UICharacterSelectWorldObjects.Get().SetSkins();
		UICharacterSelectWorldObjects.Get().SetIsInGameRings();
		while (i < num)
		{
			int num3 = 4;
			if (i < num3)
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
				UICharacterSelectWorldObjects.Get().m_ringAnimations[i].gameObject.SetActive(true);
			}
			else
			{
				UICharacterSelectWorldObjects.Get().m_ringAnimations[i].gameObject.SetActive(false);
			}
			UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(null, i, string.Empty, default(CharacterVisualInfo), false, false);
			this.SetCharacterLabel(i, string.Empty, string.Empty, -1L);
			i++;
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		this.UpdateReadyCancelButtonStates();
	}

	public string GetCannotReadyReason()
	{
		this.CanReady(out this.m_cannotReadyReasonString, true);
		if (this.m_cannotReadyReasonString == StringUtil.TR("GroupConflict", "Global"))
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.GetCannotReadyReason()).MethodHandle;
			}
			this.m_cannotReadyReasonString = StringUtil.TR("FreelancerAlreadyChosen", "Global");
		}
		return this.m_cannotReadyReasonString;
	}

	public bool DoesASelectedGame_OverrideFreelancerSelection
	{
		get
		{
			bool flag = true;
			LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
			if (groupInfo.InAGroup)
			{
				Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(groupInfo.SelectedQueueType);
				using (Dictionary<ushort, GameSubType>.Enumerator enumerator = gameTypeSubTypes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ushort, GameSubType> keyValuePair = enumerator.Current;
						if ((keyValuePair.Key & groupInfo.SubTypeMask) != 0)
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
							if (!true)
							{
								RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.get_DoesASelectedGame_OverrideFreelancerSelection()).MethodHandle;
							}
							if (keyValuePair.Value.HasMod(GameSubType.SubTypeMods.OverrideFreelancerSelection))
							{
								goto IL_D2;
							}
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (keyValuePair.Value.DuplicationRule == FreelancerDuplicationRuleTypes.allow)
							{
								goto IL_D2;
							}
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (keyValuePair.Value.DuplicationRule == FreelancerDuplicationRuleTypes.alwaysDupAcrossGame)
							{
								goto IL_D2;
							}
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							bool flag2 = keyValuePair.Value.DuplicationRule == FreelancerDuplicationRuleTypes.alwaysDupAcrossTeam;
							IL_D3:
							flag = flag2;
							if (!flag)
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
								return flag;
							}
							continue;
							IL_D2:
							flag2 = true;
							goto IL_D3;
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
			}
			return flag;
		}
	}

	public void UpdateReadyCancelButtonStates()
	{
		if (!ClientGameManager.Get().IsReady)
		{
			return;
		}
		LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
		bool flag = this.CanReady();
		bool flag2 = true;
		bool flag3 = false;
		bool flag4;
		if (UIMatchStartPanel.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.UpdateReadyCancelButtonStates()).MethodHandle;
			}
			if (!UIMatchStartPanel.Get().IsVisible())
			{
				flag4 = true;
				goto IL_80;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		flag4 = (GameManager.Get().GameInfo != null);
		IL_80:
		bool flag5 = flag4;
		CharacterType item = CharacterType.None;
		List<CharacterType> list = new List<CharacterType>();
		if (groupInfo != null)
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
			if (groupInfo.Members.Count > 0)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!flag5)
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
					for (int i = 0; i < groupInfo.Members.Count; i++)
					{
						if (groupInfo.Members[i].AccountID != ClientGameManager.Get().GetPlayerAccountData().AccountId)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (groupInfo.Members[i].IsReady)
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
								if (!groupInfo.Members[i].MemberDisplayCharacter.IsWillFill() && !this.DoesASelectedGame_OverrideFreelancerSelection)
								{
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									list.Add(groupInfo.Members[i].MemberDisplayCharacter);
								}
							}
						}
						else
						{
							item = groupInfo.Members[i].MemberDisplayCharacter;
							if (groupInfo.Members[i].IsReady)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!this.m_needToRepickCharacter)
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
									flag = false;
									flag3 = true;
									goto IL_1C4;
								}
							}
							flag2 = false;
						}
						IL_1C4:;
					}
					if (list.Contains(item))
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
						if (groupInfo.SelectedQueueType != GameType.Ranked)
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
							this.m_cannotReadyReasonString = StringUtil.TR("AnotherPlayerSelectedFreelancer", "Global");
							flag = false;
						}
					}
					goto IL_3A0;
				}
			}
		}
		if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
		{
			if (AppState_CharacterSelect.IsReady())
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (AppState_GroupCharacterSelect.Get().IsReady())
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
				flag = false;
				flag3 = true;
			}
			else
			{
				flag2 = false;
			}
			if (!this.ReadyModeSupportsCancel())
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
				flag2 = false;
			}
		}
		else if (AppState_LandingPage.Get() == AppState.GetCurrent())
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
			flag = false;
			bool flag6;
			if (GameManager.Get() != null)
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
				flag6 = (GameManager.Get().QueueInfo != null);
			}
			else
			{
				flag6 = false;
			}
			flag2 = flag6;
		}
		else
		{
			if (!(AppState_CreateGame.Get() == AppState.GetCurrent()))
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
				if (!(AppState_JoinGame.Get() == AppState.GetCurrent()))
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
					if (!(AppState_FrontendLoadingScreen.Get() == AppState.GetCurrent()))
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
						if (!(AppState_GameLoading.Get() == AppState.GetCurrent()))
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
							if (!UIMatchStartPanel.Get().IsVisible())
							{
								goto IL_3A0;
							}
						}
					}
				}
			}
			flag = false;
			flag2 = false;
		}
		IL_3A0:
		if (UIMatchStartPanel.Get() != null && UIMatchStartPanel.Get().IsVisible())
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
			if (!UIMatchStartPanel.Get().IsDuplicateFreelancerResolving())
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
				flag3 = true;
				flag = false;
			}
		}
		if (!this.ReadyModeSupportsCancel())
		{
			flag2 = false;
		}
		bool flag7 = false;
		if (UIGameSettingsPanel.Get() != null && UIGameSettingsPanel.Get().m_lastVisible)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			flag = false;
			flag2 = false;
			flag7 = true;
		}
		if (flag7)
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
			UIManager.SetGameObjectActive(this.m_buyButtonsContainer, false, null);
		}
		else
		{
			this.UpdateBuyButtons();
		}
		if (!this.m_readyBtn.gameObject.activeSelf)
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
			if (flag)
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
				UIManager.SetGameObjectActive(this.m_readyBtn, true, null);
				this.m_readyBtn.spriteController.SetClickable(true);
				this.m_readyBtn.m_animationController.Play("readyBtnOnIN", 1, 0f);
				UICharacterSelectWorldObjects.Get().SetCharacterReady(0, false);
				UICharacterSelectWorldObjects.Get().SetReadyPose();
			}
		}
		if (!flag7)
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
			if (!this.m_readyBtn.gameObject.activeSelf)
			{
				goto IL_600;
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
		if (!flag)
		{
			UIManager.SetGameObjectActive(this.m_readyBtn, false, null);
			this.m_readyBtn.spriteController.SetClickable(false);
			if (this.m_readyBtn.m_animationController.isActiveAndEnabled)
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
				this.m_readyBtn.m_animationController.Play("readyBtnOffIN", 1, 0f);
			}
			if (groupInfo != null && groupInfo.Members.Count > 0)
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
				if (list.Count > 0)
				{
					UICharacterSelectWorldObjects.Get().SetCharacterReady(0, !list.Contains(item));
				}
				else
				{
					UpdateGroupMemberData updateGroupMemberData = groupInfo.Members.Find((UpdateGroupMemberData x) => x.AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId);
					UICharacterWorldObjects uicharacterWorldObjects = UICharacterSelectWorldObjects.Get();
					int index = 0;
					bool isReady;
					if (updateGroupMemberData != null)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						isReady = updateGroupMemberData.IsReady;
					}
					else
					{
						isReady = flag3;
					}
					uicharacterWorldObjects.SetCharacterReady(index, isReady);
				}
			}
			else
			{
				UICharacterSelectWorldObjects.Get().SetCharacterReady(0, flag3);
			}
			UICharacterSelectWorldObjects.Get().SetReadyPose();
		}
		IL_600:
		bool flag8;
		if (!flag)
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
			if (!flag3)
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
				flag8 = !flag7;
				goto IL_622;
			}
		}
		flag8 = false;
		IL_622:
		bool doActive = flag8;
		UIManager.SetGameObjectActive(this.m_cannotReadyBtn, doActive, null);
		if (NavigationBar.Get() != null)
		{
			if (!NavigationBar.Get().m_cancelBtn.gameObject.activeSelf && flag2)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, true, null);
				this.CheckCancelBtnLabels();
				NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(true);
				this.SearchQueueTextExit();
				if (NavigationBar.Get().m_cancelBtn.m_animationController.isActiveAndEnabled)
				{
					NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnIN", 0, 0f);
				}
			}
			if (!flag7)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!NavigationBar.Get().m_cancelBtn.gameObject.activeSelf)
				{
					goto IL_788;
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
			if (!flag2)
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
				UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, false, null);
				NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(false);
				this.SearchQueueTextExit();
				if (NavigationBar.Get().m_cancelBtn.m_animationController.isActiveAndEnabled)
				{
					NavigationBar.Get().m_cancelBtn.m_animationController.Play("CancelBtnOUT", 0, 0f);
				}
			}
		}
		IL_788:
		if (UIRankedModeSelectScreen.Get() != null)
		{
			UIRankedModeSelectScreen.Get().UpdateReadyButton(flag);
		}
	}

	public void NotifyDroppedGroup()
	{
		UIManager.SetGameObjectActive(this.m_changeFreelancerBtn, true, null);
	}

	public void NotifyGroupUpdate()
	{
		this.m_requireGroupUpdate = true;
		this.m_charSettingsPanel.Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, false, false);
	}

	private void SetCharacterLabel(int nameIndex, string playerName, string characterName, long gameStartTimestamp)
	{
		if (!this.m_nameLabels.IsNullOrEmpty<TextMeshProUGUI>() && nameIndex < this.m_nameLabels.Length)
		{
			if (nameIndex > 0)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.SetCharacterLabel(int, string, string, long)).MethodHandle;
				}
				this.m_nameLabels[nameIndex].text = characterName;
			}
			else
			{
				this.m_nameLabels[nameIndex].text = string.Empty;
			}
		}
		if (!this.m_accountNameLabels.IsNullOrEmpty<TextMeshProUGUI>() && nameIndex < this.m_accountNameLabels.Length)
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
			if (nameIndex > 0)
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
				this.m_accountNameLabels[nameIndex].text = playerName;
			}
			else
			{
				this.m_accountNameLabels[nameIndex].text = string.Empty;
			}
		}
		if (!this.m_timeElapsedLabels.IsNullOrEmpty<TextMeshProUGUI>())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (nameIndex < this.m_timeElapsedLabels.Length)
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
				if (nameIndex > 0)
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
					this.m_createGameTimestamps[nameIndex] = gameStartTimestamp;
				}
				else
				{
					this.m_createGameTimestamps[nameIndex] = 0L;
				}
			}
		}
	}

	private void UpdateTimeElapsedLabels()
	{
		for (int i = 0; i < this.m_createGameTimestamps.Length; i++)
		{
			if (this.m_createGameTimestamps[i] <= 0L)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.UpdateTimeElapsedLabels()).MethodHandle;
				}
				this.m_timeElapsedLabels[i].text = string.Empty;
			}
			else
			{
				DateTime d = new DateTime(this.m_createGameTimestamps[i]);
				TimeSpan timeSpan = DateTime.UtcNow - d;
				this.m_timeElapsedLabels[i].text = StringUtil.TR("TimeElapsed", "Global") + ": " + string.Format(StringUtil.TR("TimeFormat", "Global"), Mathf.FloorToInt((float)timeSpan.TotalMinutes), timeSpan.Seconds);
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

	private void SetCharacter(int playerIndex, string playerHandle, CharacterResourceLink charLink, CharacterVisualInfo skin, bool isBot, bool isReady, bool isInGame, long gameStartTimestamp)
	{
		UICharacterSelectWorldObjects.Get().m_ringAnimations[playerIndex].gameObject.SetActive(true);
		if (playerIndex == 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.SetCharacter(int, string, CharacterResourceLink, CharacterVisualInfo, bool, bool, bool, long)).MethodHandle;
			}
			if (ClientGameManager.Get().IsWaitingForSkinResponse())
			{
				goto IL_59;
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
		}
		UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(charLink, playerIndex, playerHandle, skin, isBot, true);
		IL_59:
		this.SetCharacterLabel(playerIndex, playerHandle, charLink.GetDisplayName(), gameStartTimestamp);
		UICharacterSelectWorldObjects.Get().SetCharacterReady(playerIndex, isReady);
		UICharacterSelectWorldObjects.Get().CheckReadyBand(playerIndex, isReady);
		UICharacterSelectWorldObjects.Get().SetCharacterInGame(playerIndex, isInGame);
	}

	private void SetClientCharacter(int playerIndex, string playerHandle, CharacterResourceLink charLink, LobbyCharacterInfo charInfo, bool isReady, bool isInGame, long gameStartTimestamp)
	{
		UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters = new UICharacterScreen.CharacterSelectSceneStateParameters();
		if (playerIndex >= 0)
		{
			this.SetCharacter(playerIndex, playerHandle, charLink, charInfo.CharacterSkin, false, isReady, isInGame, gameStartTimestamp);
			UICharacterScreen.Get().RefreshSelectedCharacterButton();
			if (!this.m_charSettingsPanel.m_spellsSubPanel.GetDisplayedCardInfo().Equals(charInfo.CharacterCards))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.SetClientCharacter(int, string, CharacterResourceLink, LobbyCharacterInfo, bool, bool, long)).MethodHandle;
				}
				this.m_charSettingsPanel.m_spellsSubPanel.Setup(charLink.m_characterType, charInfo.CharacterCards, false, false);
			}
			if (!(this.m_charSettingsPanel.m_abilitiesSubPanel.GetDisplayedCharacter() == null))
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
				if (this.m_charSettingsPanel.m_abilitiesSubPanel.GetDisplayedCharacter().m_characterType.Equals(charLink.m_characterType))
				{
					goto IL_FF;
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
			this.m_charSettingsPanel.m_abilitiesSubPanel.Setup(charLink, false);
			IL_FF:
			if (this.m_charSettingsPanel.m_skinsSubPanel.GetDisplayedCharacterType().Equals(charLink.m_characterType))
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
				if (this.m_charSettingsPanel.m_skinsSubPanel.GetDisplayedVisualInfo().Equals(charInfo.CharacterSkin))
				{
					goto IL_18B;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_charSettingsPanel.m_skinsSubPanel.Setup(charLink, charInfo.CharacterSkin, false);
			IL_18B:
			if (!(this.m_charSettingsPanel.m_tauntsSubPanel.GetDisplayedCharacter() == null))
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_charSettingsPanel.m_tauntsSubPanel.GetDisplayedCharacter().m_characterType.Equals(charLink.m_characterType))
				{
					goto IL_1F4;
				}
			}
			this.m_charSettingsPanel.m_tauntsSubPanel.Setup(charLink, false);
			IL_1F4:
			if (this.m_charSettingsPanel.m_generalSubPanel != null)
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
				if (!(this.m_charSettingsPanel.m_generalSubPanel.GetDisplayedCharacter() == null))
				{
					if (this.m_charSettingsPanel.m_generalSubPanel.GetDisplayedCharacter().m_characterType.Equals(charLink.m_characterType))
					{
						goto IL_279;
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				this.m_charSettingsPanel.m_generalSubPanel.Setup(charLink);
			}
			IL_279:
			characterSelectSceneStateParameters.ClientSelectedCharacter = new CharacterType?(charLink.m_characterType);
			characterSelectSceneStateParameters.SideButtonsVisible = new bool?(true);
		}
		else
		{
			characterSelectSceneStateParameters.SideButtonsVisible = new bool?(false);
		}
		UIManager.Get().HandleNewSceneStateParameter(characterSelectSceneStateParameters);
	}

	public void UpdatePrimaryCharacter(LobbyCharacterInfo characterInfo)
	{
		this.SetClientCharacter(0, ClientGameManager.Get().Handle, UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, characterInfo, this.IsReady(), false, -1L);
		this.m_charSettingsPanel.Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, false, false);
	}

	public void UpdateCharacters(LobbyPlayerInfo playerInfo, List<LobbyPlayerInfo> teamPlayerInfos, LobbyGameplayOverrides gameplayOverrides)
	{
		if (GameManager.Get().GameConfig.GameType != GameType.Ranked)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.UpdateCharacters(LobbyPlayerInfo, List<LobbyPlayerInfo>, LobbyGameplayOverrides)).MethodHandle;
			}
			if (playerInfo != null)
			{
				GameManager gameManager = GameManager.Get();
				LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
				int num = UICharacterSelectWorldObjects.Get().m_ringAnimations.Length;
				bool flag = false;
				if (playerInfo != null)
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
					if (!groupInfo.InAGroup)
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
						if (playerInfo.TeamId != Team.Spectator)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (playerInfo.CharacterType != CharacterType.None)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								bool flag2 = playerInfo.ReadyState == ReadyState.Ready;
								CharacterType? clientSelectedCharacter = UICharacterScreen.GetCurrentSpecificState().ClientSelectedCharacter;
								if (clientSelectedCharacter.Value == playerInfo.CharacterType)
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
									if (UICharacterSelectWorldObjects.Get().IsCharReady(0) == flag2)
									{
										goto IL_152;
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
								UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
								{
									ClientSelectedCharacter = new CharacterType?(playerInfo.CharacterType)
								});
								this.SetClientCharacter(0, playerInfo.GetHandle(), UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, playerInfo.CharacterInfo, flag2, false, -1L);
								IL_152:
								this.m_charSettingsPanel.Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, false, false);
								UICharacterSelectWorldObjects.Get().SetCharSelectTriggerForSlot(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, 0);
								flag = true;
								goto IL_1BB;
							}
						}
					}
				}
				if (playerInfo != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (playerInfo.TeamId == Team.Spectator)
					{
						bool isReady = true;
						this.SetClientCharacter(-1, playerInfo.GetHandle(), null, null, isReady, false, -1L);
					}
				}
				IL_1BB:
				bool flag3 = false;
				bool flag4 = false;
				int numLoadedCharacters = UICharacterSelectWorldObjects.Get().GetNumLoadedCharacters();
				if (teamPlayerInfos != null)
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
					int i = 0;
					if (flag)
					{
						i++;
					}
					bool flag5;
					if (GameManager.Get().GameConfig.GameType != GameType.Practice)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						flag5 = (GameManager.Get().GameConfig.GameType == GameType.Solo);
					}
					else
					{
						flag5 = true;
					}
					if (!flag5)
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
						if (!flag)
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
							using (List<LobbyPlayerInfo>.Enumerator enumerator = teamPlayerInfos.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
									if (lobbyPlayerInfo.CharacterType == CharacterType.None)
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
									else
									{
										bool flag6;
										if (!lobbyPlayerInfo.IsNPCBot)
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
											flag6 = lobbyPlayerInfo.IsReady;
										}
										else
										{
											flag6 = true;
										}
										bool isReady2 = flag6;
										CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(lobbyPlayerInfo.CharacterType);
										if (lobbyPlayerInfo.PlayerId == playerInfo.PlayerId)
										{
											for (;;)
											{
												switch (4)
												{
												case 0:
													continue;
												}
												break;
											}
											this.SetClientCharacter(i, lobbyPlayerInfo.GetHandle(), characterResourceLink, playerInfo.CharacterInfo, isReady2, false, -1L);
											i++;
											flag = true;
											goto IL_320;
										}
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
							}
						}
						IL_320:
						using (List<LobbyPlayerInfo>.Enumerator enumerator2 = teamPlayerInfos.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								LobbyPlayerInfo lobbyPlayerInfo2 = enumerator2.Current;
								if (lobbyPlayerInfo2.CharacterType == CharacterType.None)
								{
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								else
								{
									bool flag7 = lobbyPlayerInfo2.PlayerId == playerInfo.PlayerId;
									bool isNPCBot = lobbyPlayerInfo2.IsNPCBot;
									bool flag8;
									if (!isNPCBot)
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
										flag8 = lobbyPlayerInfo2.IsReady;
									}
									else
									{
										flag8 = true;
									}
									bool isReady3 = flag8;
									CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(lobbyPlayerInfo2.CharacterType);
									if (flag7)
									{
										if (!flag)
										{
											this.SetClientCharacter(i, lobbyPlayerInfo2.GetHandle(), characterResourceLink2, playerInfo.CharacterInfo, isReady3, false, -1L);
											i++;
											flag = true;
										}
									}
									else
									{
										this.SetCharacter(i, lobbyPlayerInfo2.GetHandle(), characterResourceLink2, lobbyPlayerInfo2.CharacterInfo.CharacterSkin, isNPCBot, isReady3, false, -1L);
										i++;
									}
									if (i >= num)
									{
										for (;;)
										{
											switch (3)
											{
											case 0:
												continue;
											}
											break;
										}
										goto IL_433;
									}
								}
							}
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						IL_433:
						if (numLoadedCharacters > 0)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (numLoadedCharacters > i)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								flag4 = true;
							}
							if (numLoadedCharacters < i)
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
					int num2 = 0;
					if (playerInfo.TeamId == Team.TeamA)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (gameManager.GameInfo.GameConfig.GameType == GameType.Custom)
						{
							num2 = gameManager.GameInfo.GameConfig.TeamAPlayers;
						}
						else
						{
							num2 = gameManager.GameInfo.GameConfig.TeamAPlayers - gameManager.GameInfo.GameConfig.TeamABots;
						}
					}
					else if (playerInfo.TeamId == Team.TeamB)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (gameManager.GameInfo.GameConfig.GameType == GameType.Custom)
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
							num2 = gameManager.GameInfo.GameConfig.TeamBPlayers;
						}
						else
						{
							num2 = gameManager.GameInfo.GameConfig.TeamBPlayers - gameManager.GameInfo.GameConfig.TeamBBots;
						}
					}
					while (i < num)
					{
						if (i < num2)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							UICharacterSelectWorldObjects.Get().m_ringAnimations[i].gameObject.SetActive(true);
							UICharacterSelectWorldObjects.Get().m_ringAnimations[i].PlayAnimation("TransitionOut");
						}
						else
						{
							UICharacterSelectWorldObjects.Get().m_ringAnimations[i].gameObject.SetActive(false);
						}
						UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(null, i, string.Empty, default(CharacterVisualInfo), false, false);
						this.SetCharacterLabel(i, string.Empty, string.Empty, -1L);
						i++;
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					UICharacterScreen.Get().RefreshCharacterButtons();
				}
				this.UpdateReadyButton();
				return;
			}
			for (;;)
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

	public void UpdateReadyButton()
	{
	}

	public void UpdateBuyButtons()
	{
		if (this.m_buyButtonsContainer != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.UpdateBuyButtons()).MethodHandle;
			}
			if (ClientGameManager.Get() != null)
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
				bool flag = false;
				if (!ClientGameManager.Get().HasPurchasedGame && UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!ClientGameManager.Get().IsPlayerCharacterDataAvailable(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay))
					{
						return;
					}
					PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay);
					bool flag2;
					if (!playerCharacterData.CharacterComponent.Unlocked)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						flag2 = GameManager.Get().IsCharacterAllowedForPlayers(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay);
					}
					else
					{
						flag2 = false;
					}
					flag = flag2;
				}
				UIManager.SetGameObjectActive(this.m_buyButtonsContainer, flag, null);
				if (UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay != null && flag)
				{
					int unlockFreelancerCurrencyPrice = UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.m_charUnlockData.GetUnlockFreelancerCurrencyPrice();
					this.m_buyInGameLabel.text = "<sprite name=credit>" + unlockFreelancerCurrencyPrice;
					UIManager.SetGameObjectActive(this.m_buyInGameButton, unlockFreelancerCurrencyPrice > 0, null);
					string accountCurrency = HydrogenConfig.Get().Ticket.AccountCurrency;
					float freelancerPrice = CommerceClient.Get().GetFreelancerPrice(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay, accountCurrency);
					this.m_buyForCashLabel.text = UIStorePanel.GetLocalizedPriceString(freelancerPrice, accountCurrency);
					UIManager.SetGameObjectActive(this.m_buyForCashButton, freelancerPrice > 0f, null);
					bool flag3 = ClientGameManager.Get().PlayerWallet.GetValue(CurrencyType.UnlockFreelancerToken).m_Amount > 0;
					UIManager.SetGameObjectActive(this.m_buyForToken, flag3, null);
					Component buyButtonsContainer = this.m_buyButtonsContainer;
					bool doActive;
					if (unlockFreelancerCurrencyPrice <= 0 && freelancerPrice <= 0f)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						doActive = flag3;
					}
					else
					{
						doActive = true;
					}
					UIManager.SetGameObjectActive(buyButtonsContainer, doActive, null);
				}
			}
		}
	}

	public bool HaveUsedAnyModPoints()
	{
		int num = this.m_charSettingsPanel.m_abilitiesSubPanel.CalculateTotalModEquipCost();
		bool flag = false;
		if (UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.HaveUsedAnyModPoints()).MethodHandle;
			}
			GameObject actorDataPrefab = UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.ActorDataPrefab;
			ActorData component = actorDataPrefab.GetComponent<ActorData>();
			if (GameManager.Get().GameConfig != null)
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
				flag = AbilityModHelper.HasAnyAvailableMods(component);
			}
		}
		bool result;
		if (num <= 0)
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
			result = !flag;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public void QuickPlayUpdateCharacters(LobbyGameplayOverrides gameplayOverrides, bool switchedChars = false, bool isFromServerResponse = false)
	{
		if (!(ClientGameManager.Get() == null))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.QuickPlayUpdateCharacters(LobbyGameplayOverrides, bool, bool)).MethodHandle;
			}
			if (ClientGameManager.Get().GroupInfo != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
					UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters = new UICharacterScreen.CharacterSelectSceneStateParameters();
					characterSelectSceneStateParameters.ClientSelectedCharacter = new CharacterType?(SceneStateParameters.SelectedCharacterFromPlayerData);
					if (UILandingPageScreen.Get() != null)
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
						if (UILandingPageScreen.Get().CharacterInfoClicked != null)
						{
							characterSelectSceneStateParameters.ClientSelectedCharacter = new CharacterType?(UILandingPageScreen.Get().CharacterInfoClicked.Value);
							UILandingPageScreen.Get().CharacterInfoClicked = null;
						}
					}
					if (!isFromServerResponse)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (characterSelectSceneStateParameters.ClientSelectedCharacter != null && ClientGameManager.Get().GetAllPlayerCharacterData().ContainsKey(characterSelectSceneStateParameters.ClientSelectedCharacter.Value))
						{
							characterSelectSceneStateParameters.ClientSelectedVisualInfo = new CharacterVisualInfo?(ClientGameManager.Get().GetAllPlayerCharacterData()[characterSelectSceneStateParameters.ClientSelectedCharacter.Value].CharacterComponent.LastSkin);
						}
					}
					UIManager.Get().HandleNewSceneStateParameter(characterSelectSceneStateParameters);
					CharacterVisualInfo visualInfo = default(CharacterVisualInfo);
					if (groupInfo != null)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (groupInfo.InAGroup)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							visualInfo = groupInfo.ChararacterInfo.CharacterSkin;
							goto IL_1A5;
						}
					}
					visualInfo = UICharacterScreen.GetCurrentSpecificState().CharacterVisualInfoToDisplay;
					IL_1A5:
					UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, 0, groupInfo.MemberDisplayName, visualInfo, false, true);
					this.SetCharacterLabel(0, groupInfo.MemberDisplayName, UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.GetDisplayName(), (groupInfo.Members.Count <= 1) ? 0L : groupInfo.Members[0].CreateGameTimestamp);
					UICharacterScreen.Get().RefreshSelectedCharacterButton();
					UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
					{
						SideButtonsVisible = new bool?(true),
						ClientSelectedCharacter = new CharacterType?(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay)
					});
					this.m_charSettingsPanel.Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, false, switchedChars);
					UICharacterScreen.Get().RefreshCharacterButtons();
					return;
				}
				for (;;)
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
	}

	public void ShowPleaseEquipModsDialog()
	{
		string description = StringUtil.TR("SelectYourAbilityMods", "Global");
		if (this.m_selectModsDialog == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.ShowPleaseEquipModsDialog()).MethodHandle;
			}
			this.m_selectModsDialog = UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("MustSelectAbilityMods", "Global"), description, StringUtil.TR("Ok", "Global"), null, -1, false);
		}
	}

	public void SetupForRanked(bool ranked)
	{
		if (UIFrontEnd.Get().m_frontEndNavPanel.m_PlayBtn.IsSelected())
		{
			if (!(AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()))
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.SetupForRanked(bool)).MethodHandle;
				}
				if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
				{
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
				else
				{
					if (UIRankedModeSelectScreen.Get() != null)
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
						UIRankedModeSelectScreen.Get().SetVisible(false);
						return;
					}
					return;
				}
			}
			if (ranked)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				UIFrontEnd.Get().ShowScreen(FrontEndScreenState.RankedModeSelect, false);
				if (UIRankedModeSelectScreen.Get() != null)
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
					UIRankedModeSelectScreen.Get().SetVisible(true);
				}
			}
			else
			{
				if (UIFrontEnd.Get().m_currentScreen != FrontEndScreenState.GroupCharacterSelect)
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
					UIFrontEnd.Get().ShowScreen(FrontEndScreenState.GroupCharacterSelect, false);
				}
				if (UIRankedModeSelectScreen.Get() != null)
				{
					UIRankedModeSelectScreen.Get().SetVisible(false);
				}
			}
		}
	}

	public bool IsVisible()
	{
		return this.m_visible;
	}

	public void SetVisible(bool visible, bool refreshOnly = false)
	{
		if (!(this.buttonContainer == null))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.SetVisible(bool, bool)).MethodHandle;
			}
			if (!(this.buttonContainer.gameObject == null))
			{
				if (this.m_visible != visible)
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
					UIManager.SetGameObjectActive(this.buttonContainer, visible, null);
					this.m_visible = visible;
					for (int i = 0; i < this.m_mainLobbyControllers.Length; i++)
					{
						UIManager.SetGameObjectActive(this.m_mainLobbyControllers[i], visible, null);
					}
					LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
					CharacterType characterType = CharacterType.None;
					if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
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
						if (groupInfo != null)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (groupInfo.InAGroup)
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								characterType = groupInfo.ChararacterInfo.CharacterType;
								goto IL_11D;
							}
						}
					}
					if (UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay != null)
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
						characterType = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay;
					}
					IL_11D:
					UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters = new UICharacterScreen.CharacterSelectSceneStateParameters();
					UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters2 = characterSelectSceneStateParameters;
					bool value;
					if (visible)
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
						if (UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay != null)
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
							value = (characterType != CharacterType.None);
						}
						else
						{
							value = false;
						}
					}
					else
					{
						value = false;
					}
					characterSelectSceneStateParameters2.SideButtonsVisible = new bool?(value);
					UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters3 = characterSelectSceneStateParameters;
					bool value2;
					if (visible)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						value2 = !this.IsReady();
					}
					else
					{
						value2 = false;
					}
					characterSelectSceneStateParameters3.CharacterSelectButtonsVisible = new bool?(value2);
					UIManager.Get().HandleNewSceneStateParameter(characterSelectSceneStateParameters);
					this.m_charSettingsPanel.SetVisible(false, UICharacterSelectCharacterSettingsPanel.TabPanel.None);
					UIManager.SetGameObjectActive(this.m_readyBtn, !this.IsReady(), null);
					this.m_readyBtn.spriteController.SetClickable(!this.IsReady());
					UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, this.IsReady(), null);
					NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(this.IsReady());
					NavigationBar.Get().m_searchQueueText.text = string.Empty;
					if (visible)
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
						this.m_charSettingsPanel.m_abilitiesSubPanel.RefreshSelectedMods();
					}
					if (!visible)
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
						UIManager.SetGameObjectActive(this.m_buyButtonsContainer, false, null);
					}
					else
					{
						this.UpdateBuyButtons();
					}
				}
				if (visible)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!refreshOnly)
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
						UIFrontEnd.Get().m_frontEndNavPanel.SetNavButtonSelected(UIFrontEnd.Get().m_frontEndNavPanel.m_PlayBtn);
					}
				}
				if (this.m_selectModsDialog != null)
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
					UIDialogPopupManager.Get().CloseDialog(this.m_selectModsDialog);
				}
				return;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public void PlayMainLobbyControllerAnim(string animName, int layer)
	{
		for (int i = 0; i < this.m_mainLobbyControllers.Length; i++)
		{
			this.m_mainLobbyControllers[i].Play(animName, 1, 0f);
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.PlayMainLobbyControllerAnim(string, int)).MethodHandle;
		}
	}

	public void UpdateSkinsPanel()
	{
		this.m_charSettingsPanel.m_skinsSubPanel.RePopulateCharacterData();
	}

	public void PurchaseCharacterTintResponseHandler(PurchaseTintResponse response)
	{
		if (response.Success)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.PurchaseCharacterTintResponseHandler(PurchaseTintResponse)).MethodHandle;
			}
			if (response.Result == PurchaseResult.Success)
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
				this.m_charSettingsPanel.m_skinsSubPanel.RePopulateCharacterData();
				AppState_CharacterSelect.Get().UpdateSelectedSkin(this.m_charSettingsPanel.m_skinsSubPanel.GetCurrentSelection());
			}
		}
	}

	public void PurchaseCharacterTintResponseHandler(PurchaseTintForCashResponse response)
	{
		if (response.Success)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.PurchaseCharacterTintResponseHandler(PurchaseTintForCashResponse)).MethodHandle;
			}
			if (response.Result == PurchaseResult.Success)
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
				this.m_charSettingsPanel.m_skinsSubPanel.RePopulateCharacterData();
				AppState_CharacterSelect.Get().UpdateSelectedSkin(this.m_charSettingsPanel.m_skinsSubPanel.GetCurrentSelection());
			}
		}
	}

	public void PurchaseCharacterSkinResponseHandler(PurchaseSkinResponse response)
	{
		if (response.Success)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.PurchaseCharacterSkinResponseHandler(PurchaseSkinResponse)).MethodHandle;
			}
			if (response.Result == PurchaseResult.Success)
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
				AppState_CharacterSelect.Get().UpdateSelectedSkin(this.m_charSettingsPanel.m_skinsSubPanel.GetCurrentSelection());
			}
		}
	}

	public void OnCharacterDataUpdated(PersistedCharacterData data)
	{
		UICharacterScreen.Get().RefreshCharacterButtons();
	}

	public void SearchQueueTextExit()
	{
		if (NavigationBar.Get() != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.SearchQueueTextExit()).MethodHandle;
			}
			NavigationBar.Get().SearchQueueTextExit();
		}
	}

	public void HandleGameAssemblingUIChanges()
	{
		UIFrontEnd.Get().m_frontEndNavPanel.ToggleUiForGameStarting(false);
		UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, false, null);
		NavigationBar.Get().m_searchQueueText.text = string.Empty;
	}

	private void HandleGameInfoNotification(GameInfoNotification notification)
	{
		if (UIMatchStartPanel.Get() != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.HandleGameInfoNotification(GameInfoNotification)).MethodHandle;
			}
			UIMatchStartPanel.Get().HandleGameStatusChanged(notification);
		}
		if (notification.GameInfo.GameStatus == GameStatus.LoadoutSelecting)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, false, null);
			NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(false);
			UIManager.SetGameObjectActive(this.m_readyBtn, false, null);
			this.m_readyBtn.spriteController.SetClickable(false);
			UINewUserFlowManager.OnDoneWithReadyButton();
		}
	}

	private void HandleBankBalanceChange(CurrencyData data)
	{
		if (data.m_Type == CurrencyType.UnlockFreelancerToken)
		{
			this.UpdateBuyButtons();
		}
	}

	private bool IsSinglePlayer(GameInfoNotification notification)
	{
		return notification.GameInfo.GameConfig.GameType == GameType.Practice || notification.GameInfo.GameConfig.GameType == GameType.Solo || notification.GameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial);
	}

	public void CheckCancelBtnLabels()
	{
		bool isInCustomGame = SceneStateParameters.IsInCustomGame;
		if (isInCustomGame)
		{
			TextMeshProUGUI[] componentsInChildren = NavigationBar.Get().m_cancelBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
			int i = 0;
			IL_D1:
			while (i < componentsInChildren.Length)
			{
				bool flag = true;
				if (componentsInChildren[i] == NavigationBar.Get().m_searchQueueText)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.CheckCancelBtnLabels()).MethodHandle;
					}
					flag = false;
				}
				for (int j = 0; j < NavigationBar.Get().m_timeInQueueLabel.Length; j++)
				{
					if (NavigationBar.Get().m_timeInQueueLabel[j] == componentsInChildren[i])
					{
						flag = false;
						IL_97:
						if (flag)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (isInCustomGame)
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
								componentsInChildren[i].alignment = TextAlignmentOptions.Center;
							}
							else
							{
								componentsInChildren[i].alignment = TextAlignmentOptions.Right;
							}
						}
						i++;
						goto IL_D1;
					}
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					goto IL_97;
				}
			}
		}
	}

	public void RestoreFunctionalityOnGameStop(GameResult gameResult)
	{
		Log.Info("Restoring Functionality on game stop. GameResult: " + gameResult.ToString(), new object[0]);
		NavigationBar.Get().m_searchQueueText.text = string.Empty;
		UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, true, null);
		this.CheckCancelBtnLabels();
		this.m_readyBtn.spriteController.SetClickable(true);
		NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(true);
		UIFrontEnd.Get().m_frontEndNavPanel.ToggleUiForGameStarting(true);
		AppState_GameTeardown.Get().Enter();
	}

	private void BuyInGameClicked(BaseEventData data)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.Character;
		uipurchaseableItem.m_charLink = UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay;
		uipurchaseableItem.m_currencyType = CurrencyType.FreelancerCurrency;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, new UIStorePanel.PurchaseCharacterCallback(this.PurchaseCharacterResponseHandler));
	}

	private void BuyForCashClicked(BaseEventData data)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.Character;
		uipurchaseableItem.m_charLink = UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay;
		uipurchaseableItem.m_purchaseForCash = true;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, new UIStorePanel.PurchaseCharacterCallback(this.PurchaseCharacterResponseHandler));
	}

	private void BuyForTokenClicked(BaseEventData data)
	{
		ClientGameManager.Get().PurchaseCharacter(CurrencyType.UnlockFreelancerToken, UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay, delegate(PurchaseCharacterResponse response)
		{
			this.PurchaseCharacterResponseHandler(response.Success, response.Result, response.CharacterType);
		});
	}

	private bool BuyTooltipSetup(UITooltipBase tooltip)
	{
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		uititledTooltip.Setup(StringUtil.TR("BuyFreelancer", "CharacterSelect"), StringUtil.TR("BuyFreelancerDesc", "CharacterSelect"), string.Empty);
		return true;
	}

	public void OpenOneButtonDialog(string title, string message, UIDialogBox.DialogButtonCallback callback = null)
	{
		this.m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(title, message, StringUtil.TR("Ok", "Global"), callback, -1, false);
	}

	public void PurchaseCharacterResponseHandler(bool success, PurchaseResult result, CharacterType characterType)
	{
		if (success)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.PurchaseCharacterResponseHandler(bool, PurchaseResult, CharacterType)).MethodHandle;
			}
			if (result == PurchaseResult.Success)
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
				UIManager.SetGameObjectActive(this.m_buyButtonsContainer, false, null);
				UICharacterScreen.Get().RefreshCharacterButtons();
				UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					ClientRequestToServerSelectCharacter = new CharacterType?(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay)
				});
			}
		}
	}

	private void Awake()
	{
		if (this.m_buyButtonsContainer != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.Awake()).MethodHandle;
			}
			this.m_buyInGameButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BuyInGameClicked);
			this.m_buyInGameButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.BuyTooltipSetup), null);
			this.m_buyForCashButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BuyForCashClicked);
			this.m_buyForCashButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.BuyTooltipSetup), null);
			this.m_buyForToken.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BuyForTokenClicked);
			this.m_buyForToken.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.BuyTooltipSetup), null);
			this.m_buyButtonsHeader.text = StringUtil.TR("BuyFreelancer", "CharacterSelect");
		}
		UICharacterSelectScreenController.s_instance = this;
		this.m_visible = false;
		for (int i = 0; i < this.m_mainLobbyControllers.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_mainLobbyControllers[i], false, null);
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			SideButtonsVisible = new bool?(false),
			CharacterSelectButtonsVisible = new bool?(false)
		});
		UIManager.SetGameObjectActive(this.m_readyBtn, false, null);
		this.m_changeFreelancerBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ToggleCharacterSelect);
		this.m_readyBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ReadyButtonClickCallback);
		ClientGameManager.Get().OnCharacterDataUpdated += this.OnCharacterDataUpdated;
		ClientGameManager.Get().OnGameInfoNotification += this.HandleGameInfoNotification;
		ClientGameManager.Get().OnBankBalanceChange += this.HandleBankBalanceChange;
		GameManager.Get().OnGameAssembling += this.HandleGameAssemblingUIChanges;
		GameManager.Get().OnGameStopped += this.RestoreFunctionalityOnGameStop;
		if (!this.m_timeElapsedLabels.IsNullOrEmpty<TextMeshProUGUI>())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_createGameTimestamps = new long[this.m_timeElapsedLabels.Length];
			for (int j = 0; j < this.m_timeElapsedLabels.Length; j++)
			{
				this.m_timeElapsedLabels[j].text = string.Empty;
			}
			for (;;)
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

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectScreenController.OnDestroy()).MethodHandle;
			}
			ClientGameManager.Get().OnCharacterDataUpdated -= this.OnCharacterDataUpdated;
			ClientGameManager.Get().OnGameInfoNotification -= this.HandleGameInfoNotification;
			ClientGameManager.Get().OnBankBalanceChange -= this.HandleBankBalanceChange;
		}
		if (GameManager.Get() != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			GameManager.Get().OnGameAssembling -= this.HandleGameAssemblingUIChanges;
			GameManager.Get().OnGameStopped -= this.RestoreFunctionalityOnGameStop;
		}
		if (this.m_messageBox != null)
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
			this.m_messageBox.Close();
			this.m_messageBox = null;
		}
	}
}
