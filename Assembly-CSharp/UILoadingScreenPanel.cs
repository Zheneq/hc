using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILoadingScreenPanel : UIScene
{
	public TextMeshProUGUI m_gameMode;

	public TextMeshProUGUI m_gameSubType;

	public TextMeshProUGUI m_mapName;

	public Image m_mapLoadingImage;

	public RectTransform m_container;

	public _ButtonSwapSprite m_nextTip;

	public _ButtonSwapSprite m_previousTip;

	public TextMeshProUGUI m_tipText;

	public Image m_tipTimerFill;

	public UILoadscreenProfile[] m_allyTeamPlayers;

	public UILoadscreenProfile[] m_enemyTeamPlayers;

	public UILoadingScreenPanel.InstructionPair[] m_instructionPairs;

	public LoadingScreenSubtypeTooltip[] m_tooltips;

	public TextMeshProUGUI m_tooltipSubtypeName;

	private float m_timeStart;

	private float m_visibleStartTime;

	private const float m_countdownDuration = 10f;

	private static UILoadingScreenPanel s_instance;

	private int m_tipIndex;

	public override SceneType GetSceneType()
	{
		return SceneType.GameLoadingScreen;
	}

	public override void Awake()
	{
		UILoadingScreenPanel.s_instance = this;
		this.m_timeStart = 0f;
		if (this.m_tipTimerFill != null)
		{
			this.m_tipTimerFill.fillAmount = 0f;
		}
		if (this.m_nextTip != null)
		{
			this.m_nextTip.callback = new _ButtonSwapSprite.ButtonClickCallback(this.NextTipClicked);
		}
		if (this.m_previousTip != null)
		{
			this.m_previousTip.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PrevTipClicked);
		}
		this.m_visibleStartTime = Time.time;
		this.m_tipIndex = UnityEngine.Random.Range(0, GameWideData.Get().m_loadingTips.Length);
		base.Awake();
		this.SetVisible(false);
	}

	private void OnDestroy()
	{
		UILoadingScreenPanel.s_instance = null;
	}

	public static UILoadingScreenPanel Get()
	{
		return UILoadingScreenPanel.s_instance;
	}

	public void SetVisible(bool visible)
	{
		bool activeSelf = this.m_container.gameObject.activeSelf;
		if (activeSelf != visible)
		{
			UIManager.SetGameObjectActive(this.m_container, visible, null);
			if (visible)
			{
				this.m_visibleStartTime = Time.time;
			}
			else
			{
				Log.Info(Log.Category.Loading, "Loading screen displayed for {0} seconds.", new object[]
				{
					(Time.time - this.m_visibleStartTime).ToString("F1")
				});
				if (this.m_timeStart != 0f)
				{
					this.m_tipIndex++;
					this.m_timeStart = 0f;
				}
			}
		}
		if (visible && UIMainMenu.Get() != null)
		{
			if (UIMainMenu.Get().IsOpen())
			{
				UIMainMenu.Get().SetMenuVisible(false, false);
			}
		}
	}

	private void NextTipClicked(BaseEventData data)
	{
		this.m_tipIndex++;
		this.SetTutorialTip();
	}

	private void PrevTipClicked(BaseEventData data)
	{
		this.m_tipIndex--;
		this.SetTutorialTip();
	}

	private void SetTutorialTip()
	{
		if (this.m_tipIndex < 0)
		{
			this.m_tipIndex = GameWideData.Get().m_loadingTips.Length - 1;
		}
		else if (this.m_tipIndex >= GameWideData.Get().m_loadingTips.Length)
		{
			this.m_tipIndex = 0;
		}
		this.m_tipText.text = GameWideData.Get().GetLoadingScreenTip(this.m_tipIndex);
		this.StartTimer();
	}

	private UILoadscreenProfile GetProfile(int playerId)
	{
		if (this.m_allyTeamPlayers != null)
		{
			for (int i = 0; i < this.m_allyTeamPlayers.Length; i++)
			{
				if (this.m_allyTeamPlayers[i].m_playerId == playerId)
				{
					return this.m_allyTeamPlayers[i];
				}
			}
		}
		if (this.m_enemyTeamPlayers != null)
		{
			for (int j = 0; j < this.m_enemyTeamPlayers.Length; j++)
			{
				if (this.m_enemyTeamPlayers[j].m_playerId == playerId)
				{
					return this.m_enemyTeamPlayers[j];
				}
			}
		}
		return null;
	}

	private UILoadscreenProfile GetProfile(LobbyPlayerInfo playerInfo)
	{
		if (this.m_allyTeamPlayers != null)
		{
			for (int i = 0; i < this.m_allyTeamPlayers.Length; i++)
			{
				if (this.m_allyTeamPlayers[i].GetPlayerInfo() == playerInfo)
				{
					return this.m_allyTeamPlayers[i];
				}
			}
		}
		if (this.m_enemyTeamPlayers != null)
		{
			for (int j = 0; j < this.m_enemyTeamPlayers.Length; j++)
			{
				if (this.m_enemyTeamPlayers[j].GetPlayerInfo() == playerInfo)
				{
					return this.m_enemyTeamPlayers[j];
				}
			}
		}
		return null;
	}

	private IEnumerable<UILoadscreenProfile> GetBotsMasqueradeAsHumansProfiles()
	{
		if (this.m_allyTeamPlayers != null)
		{
			for (int i = 0; i < this.m_allyTeamPlayers.Length; i++)
			{
				LobbyPlayerInfo playerInfo = this.m_allyTeamPlayers[i].GetPlayerInfo();
				if (playerInfo != null)
				{
					if (playerInfo.IsNPCBot)
					{
						if (playerInfo.BotsMasqueradeAsHumans)
						{
							yield return this.m_allyTeamPlayers[i];
						}
					}
				}
			}
		}
		if (this.m_enemyTeamPlayers != null)
		{
			for (int j = 0; j < this.m_enemyTeamPlayers.Length; j++)
			{
				LobbyPlayerInfo playerInfo2 = this.m_enemyTeamPlayers[j].GetPlayerInfo();
				if (playerInfo2 != null && playerInfo2.IsNPCBot)
				{
					if (playerInfo2.BotsMasqueradeAsHumans)
					{
						yield return this.m_enemyTeamPlayers[j];
					}
				}
			}
		}
		yield break;
	}

	public void SetLoadingProgress(int playerId, float loadingProgress, bool isLocal)
	{
		UILoadscreenProfile profile = this.GetProfile(playerId);
		this.SetLoadingProgress(profile, loadingProgress, isLocal);
	}

	public void SetLoadingProgress(LobbyPlayerInfo playerInfo, float loadingProgress, bool isLocal)
	{
		UILoadscreenProfile profile = this.GetProfile(playerInfo);
		this.SetLoadingProgress(profile, loadingProgress, isLocal);
	}

	public void SetLoadingProgress(UILoadscreenProfile profile, float loadingProgress, bool isLocal)
	{
		if (profile != null)
		{
			profile.m_slider.fillAmount = Mathf.Max(profile.m_slider.fillAmount, loadingProgress);
			if (isLocal)
			{
				LobbyPlayerInfo playerInfo = profile.GetPlayerInfo();
				if (playerInfo.BotsMasqueradeAsHumans)
				{
					IEnumerable<UILoadscreenProfile> botsMasqueradeAsHumansProfiles = this.GetBotsMasqueradeAsHumansProfiles();
					IEnumerator<UILoadscreenProfile> enumerator = botsMasqueradeAsHumansProfiles.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							UILoadscreenProfile uiloadscreenProfile = enumerator.Current;
							uiloadscreenProfile.m_slider.fillAmount = Mathf.Max(uiloadscreenProfile.m_slider.fillAmount, loadingProgress);
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
			}
		}
	}

	private void Update()
	{
		if (this.m_container.gameObject.activeSelf)
		{
			if (this.m_timeStart != 0f)
			{
				float num = 10f - (Time.time - this.m_timeStart);
				if (this.m_tipTimerFill != null)
				{
					this.m_tipTimerFill.fillAmount = num / 10f;
				}
				if (num <= 0f)
				{
					this.m_tipIndex++;
					this.SetTutorialTip();
				}
			}
		}
	}

	public void StartTimer()
	{
		this.m_timeStart = Time.time;
		if (this.m_tipTimerFill != null)
		{
			this.m_tipTimerFill.fillAmount = 0f;
		}
	}

	private bool DoInstructionTypesMatch(UILoadingScreenPanel.UIInstructionDisplayTipType UIDisplayType, GameSubType.GameLoadScreenInstructions GameTypeInstructions)
	{
		if (UIDisplayType == UILoadingScreenPanel.UIInstructionDisplayTipType.Default)
		{
			return GameTypeInstructions == GameSubType.GameLoadScreenInstructions.Default;
		}
		if (UIDisplayType == UILoadingScreenPanel.UIInstructionDisplayTipType.GeneralGameSpecific)
		{
			bool result;
			if (GameTypeInstructions != GameSubType.GameLoadScreenInstructions.Extraction && GameTypeInstructions != GameSubType.GameLoadScreenInstructions.OverpoweredUp)
			{
				result = (GameTypeInstructions == GameSubType.GameLoadScreenInstructions.LightsOut);
			}
			else
			{
				result = true;
			}
			return result;
		}
		return false;
	}

	public void ShowTeams()
	{
		GameManager gameManager = GameManager.Get();
		LobbyPlayerInfo playerInfo = gameManager.PlayerInfo;
		LobbyGameInfo gameInfo = gameManager.GameInfo;
		LobbyTeamInfo teamInfo = gameManager.TeamInfo;
		if (teamInfo != null)
		{
			if (playerInfo != null)
			{
				if (gameInfo != null)
				{
					IEnumerable<LobbyPlayerInfo> enumerable;
					IEnumerable<LobbyPlayerInfo> enumerable2;
					if (playerInfo.TeamId == Team.TeamB)
					{
						enumerable = teamInfo.TeamBPlayerInfo;
						enumerable2 = teamInfo.TeamAPlayerInfo;
					}
					else
					{
						enumerable = teamInfo.TeamAPlayerInfo;
						enumerable2 = teamInfo.TeamBPlayerInfo;
					}
					int num = enumerable.Count<LobbyPlayerInfo>();
					int num2 = enumerable2.Count<LobbyPlayerInfo>();
					int i = 0;
					IEnumerator<LobbyPlayerInfo> enumerator = enumerable.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
							CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(lobbyPlayerInfo.CharacterType);
							this.m_allyTeamPlayers[i].Setup(characterResourceLink, lobbyPlayerInfo, false, false);
							if (lobbyPlayerInfo.AccountId == ClientGameManager.Get().GetPlayerAccountData().AccountId)
							{
								if (i > 0)
								{
									CharacterType charType = this.m_allyTeamPlayers[0].GetCharType();
									LobbyPlayerInfo playerInfo2 = this.m_allyTeamPlayers[0].GetPlayerInfo();
									this.m_allyTeamPlayers[0].Setup(GameWideData.Get().GetCharacterResourceLink(this.m_allyTeamPlayers[i].GetCharType()), this.m_allyTeamPlayers[i].GetPlayerInfo(), false, false);
									this.m_allyTeamPlayers[i].Setup(GameWideData.Get().GetCharacterResourceLink(charType), playerInfo2, false, false);
								}
							}
							i++;
							if (i >= num)
							{
								break;
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					while (i < this.m_allyTeamPlayers.Length)
					{
						UIManager.SetGameObjectActive(this.m_allyTeamPlayers[i], false, null);
						i++;
					}
					int j = 0;
					IEnumerator<LobbyPlayerInfo> enumerator2 = enumerable2.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							LobbyPlayerInfo lobbyPlayerInfo2 = enumerator2.Current;
							CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(lobbyPlayerInfo2.CharacterType);
							this.m_enemyTeamPlayers[j].Setup(characterResourceLink2, lobbyPlayerInfo2, true, false);
							j++;
							if (j >= num2)
							{
								goto IL_281;
							}
						}
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					IL_281:
					while (j < this.m_enemyTeamPlayers.Length)
					{
						UIManager.SetGameObjectActive(this.m_enemyTeamPlayers[j], false, null);
						j++;
					}
					MapData mapData = GameWideData.Get().GetMapData(gameInfo.GameConfig.Map);
					if (mapData != null)
					{
						this.m_mapLoadingImage.sprite = (Resources.Load(mapData.ResourceImageSpriteLocation, typeof(Sprite)) as Sprite);
					}
					else
					{
						this.m_mapLoadingImage.sprite = (Resources.Load("Stages/information_stage_image", typeof(Sprite)) as Sprite);
					}
					this.m_mapName.text = "- " + GameWideData.Get().GetMapDisplayName(gameInfo.GameConfig.Map) + " -";
					this.SetTutorialTip();
					this.m_gameMode.text = gameInfo.GameConfig.GameType.GetDisplayName();
					GameSubType.GameLoadScreenInstructions instructionsToDisplay = gameInfo.GameConfig.InstanceSubType.InstructionsToDisplay;
					if (instructionsToDisplay == GameSubType.GameLoadScreenInstructions.Default)
					{
						UIManager.SetGameObjectActive(this.m_instructionPairs[0].m_container, true, null);
						for (int k = 1; k < this.m_instructionPairs.Length; k++)
						{
							UIManager.SetGameObjectActive(this.m_instructionPairs[k].m_container, false, null);
						}
						UIManager.SetGameObjectActive(this.m_gameSubType, false, null);
					}
					else
					{
						bool flag = false;
						for (int l = this.m_instructionPairs.Length - 1; l > 0; l--)
						{
							if (this.DoInstructionTypesMatch(this.m_instructionPairs[l].m_InstructionsType, instructionsToDisplay))
							{
								UIManager.SetGameObjectActive(this.m_instructionPairs[l].m_container, true, null);
								flag = true;
							}
							else
							{
								UIManager.SetGameObjectActive(this.m_instructionPairs[l].m_container, false, null);
							}
						}
						Component container = this.m_instructionPairs[0].m_container;
						bool doActive;
						if (flag)
						{
							doActive = (instructionsToDisplay == GameSubType.GameLoadScreenInstructions.Default);
						}
						else
						{
							doActive = true;
						}
						UIManager.SetGameObjectActive(container, doActive, null);
						UIManager.SetGameObjectActive(this.m_gameSubType, true, null);
						if (gameInfo.GameConfig.InstanceSubType.LocalizedName != null)
						{
							this.m_gameSubType.text = StringUtil.TR(gameInfo.GameConfig.InstanceSubType.LocalizedName);
						}
						else
						{
							this.m_gameSubType.text = string.Empty;
						}
						this.m_tooltipSubtypeName.text = StringUtil.TR(gameInfo.GameConfig.InstanceSubType.LocalizedName);
						GameSubTypeData.GameSubTypeInstructions instructionSet = GameSubTypeData.Get().GetInstructionSet(instructionsToDisplay);
						if (instructionSet != null)
						{
							for (int m = 0; m < this.m_tooltips.Length; m++)
							{
								if (m < instructionSet.DisplayInfos.Length)
								{
									UIManager.SetGameObjectActive(this.m_tooltips[m], true, null);
									this.m_tooltips[m].Setup(instructionSet.DisplayInfos[m]);
								}
								else
								{
									UIManager.SetGameObjectActive(this.m_tooltips[m], false, null);
								}
							}
						}
					}
					return;
				}
			}
		}
	}

	public void SetTipIndex(int index)
	{
		this.m_tipIndex = index;
	}

	public enum UIInstructionDisplayTipType
	{
		Default,
		GeneralGameSpecific
	}

	[Serializable]
	public class InstructionPair
	{
		public UILoadingScreenPanel.UIInstructionDisplayTipType m_InstructionsType;

		public RectTransform m_container;
	}
}
