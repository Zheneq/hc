using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

	public InstructionPair[] m_instructionPairs;

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
		s_instance = this;
		m_timeStart = 0f;
		if (m_tipTimerFill != null)
		{
			m_tipTimerFill.fillAmount = 0f;
		}
		if (m_nextTip != null)
		{
			m_nextTip.callback = NextTipClicked;
		}
		if (m_previousTip != null)
		{
			m_previousTip.callback = PrevTipClicked;
		}
		m_visibleStartTime = Time.time;
		m_tipIndex = Random.Range(0, GameWideData.Get().m_loadingTips.Length);
		base.Awake();
		SetVisible(false);
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public static UILoadingScreenPanel Get()
	{
		return s_instance;
	}

	public void SetVisible(bool visible)
	{
		bool activeSelf = m_container.gameObject.activeSelf;
		if (activeSelf != visible)
		{
			UIManager.SetGameObjectActive(m_container, visible);
			if (visible)
			{
				m_visibleStartTime = Time.time;
			}
			else
			{
				Log.Info(Log.Category.Loading, "Loading screen displayed for {0} seconds.", (Time.time - m_visibleStartTime).ToString("F1"));
				if (m_timeStart != 0f)
				{
					m_tipIndex++;
					m_timeStart = 0f;
				}
			}
		}
		if (visible && UIMainMenu.Get() != null)
		{
			if (UIMainMenu.Get().IsOpen())
			{
				UIMainMenu.Get().SetMenuVisible(false);
			}
		}
	}

	private void NextTipClicked(BaseEventData data)
	{
		m_tipIndex++;
		SetTutorialTip();
	}

	private void PrevTipClicked(BaseEventData data)
	{
		m_tipIndex--;
		SetTutorialTip();
	}

	private void SetTutorialTip()
	{
		if (m_tipIndex < 0)
		{
			m_tipIndex = GameWideData.Get().m_loadingTips.Length - 1;
		}
		else if (m_tipIndex >= GameWideData.Get().m_loadingTips.Length)
		{
			m_tipIndex = 0;
		}
		m_tipText.text = GameWideData.Get().GetLoadingScreenTip(m_tipIndex);
		StartTimer();
	}

	private UILoadscreenProfile GetProfile(int playerId)
	{
		if (m_allyTeamPlayers != null)
		{
			foreach (UILoadscreenProfile profile in m_allyTeamPlayers)
			{
				if (profile.m_playerId == playerId)
				{
					return profile;
				}
			}
		}
		if (m_enemyTeamPlayers != null)
		{
			foreach (UILoadscreenProfile profile in m_enemyTeamPlayers)
			{
				if (profile.m_playerId == playerId)
				{
					return profile;
				}
			}
		}
		return null;
	}

	private UILoadscreenProfile GetProfile(LobbyPlayerInfo playerInfo)
	{
		if (m_allyTeamPlayers != null)
		{
			foreach (UILoadscreenProfile profile in m_allyTeamPlayers)
			{
				if (profile.GetPlayerInfo() == playerInfo)
				{
					return profile;
				}
			}
		}
		if (m_enemyTeamPlayers != null)
		{
			foreach (UILoadscreenProfile profile in m_enemyTeamPlayers)
			{
				if (profile.GetPlayerInfo() == playerInfo)
				{
					return profile;
				}
			}
		}
		return null;
	}

	private IEnumerable<UILoadscreenProfile> GetBotsMasqueradeAsHumansProfiles()
	{
		if (m_allyTeamPlayers != null)
		{
			for (int i = 0; i < m_allyTeamPlayers.Length; i++)
			{
				LobbyPlayerInfo playerInfo = m_allyTeamPlayers[i].GetPlayerInfo();
				if (playerInfo != null)
				{
					if (playerInfo.IsNPCBot)
					{
						if (playerInfo.BotsMasqueradeAsHumans)
						{
							yield return m_allyTeamPlayers[i];
						}
					}
				}
			}
		}
		if (m_enemyTeamPlayers != null)
		{
			for (int j = 0; j < m_enemyTeamPlayers.Length; j++)
			{
				LobbyPlayerInfo playerInfo2 = m_enemyTeamPlayers[j].GetPlayerInfo();
				if (playerInfo2 != null && playerInfo2.IsNPCBot)
				{
					if (playerInfo2.BotsMasqueradeAsHumans)
					{
						yield return m_enemyTeamPlayers[j];
					}
				}
			}
		}
	}

	public void SetLoadingProgress(int playerId, float loadingProgress, bool isLocal)
	{
		UILoadscreenProfile profile = GetProfile(playerId);
		SetLoadingProgress(profile, loadingProgress, isLocal);
	}

	public void SetLoadingProgress(LobbyPlayerInfo playerInfo, float loadingProgress, bool isLocal)
	{
		UILoadscreenProfile profile = GetProfile(playerInfo);
		SetLoadingProgress(profile, loadingProgress, isLocal);
	}

	public void SetLoadingProgress(UILoadscreenProfile profile, float loadingProgress, bool isLocal)
	{
		if (profile == null)
		{
			return;
		}
		profile.m_slider.fillAmount = Mathf.Max(profile.m_slider.fillAmount, loadingProgress);
		if (!isLocal)
		{
			return;
		}
		LobbyPlayerInfo playerInfo = profile.GetPlayerInfo();
		if (playerInfo.BotsMasqueradeAsHumans)
		{
			foreach (UILoadscreenProfile uiloadscreenProfile in GetBotsMasqueradeAsHumansProfiles())
			{
				uiloadscreenProfile.m_slider.fillAmount = Mathf.Max(uiloadscreenProfile.m_slider.fillAmount, loadingProgress);
			}
		}
	}

	private void Update()
	{
		if (m_container.gameObject.activeSelf)
		{
			if (m_timeStart != 0f)
			{
				float num = 10f - (Time.time - m_timeStart);
				if (m_tipTimerFill != null)
				{
					m_tipTimerFill.fillAmount = num / 10f;
				}
				if (num <= 0f)
				{
					m_tipIndex++;
					SetTutorialTip();
				}
			}
		}
	}

	public void StartTimer()
	{
		m_timeStart = Time.time;
		if (m_tipTimerFill != null)
		{
			m_tipTimerFill.fillAmount = 0f;
		}
	}

	private bool DoInstructionTypesMatch(UIInstructionDisplayTipType UIDisplayType, GameSubType.GameLoadScreenInstructions GameTypeInstructions)
	{
		if (UIDisplayType == UIInstructionDisplayTipType.Default)
		{
			return GameTypeInstructions == GameSubType.GameLoadScreenInstructions.Default;
		}
		if (UIDisplayType == UIInstructionDisplayTipType.GeneralGameSpecific)
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
					int num = enumerable.Count();
					int num2 = enumerable2.Count();
					int i = 0;
					IEnumerator<LobbyPlayerInfo> enumerator = enumerable.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
							CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(lobbyPlayerInfo.CharacterType);
							m_allyTeamPlayers[i].Setup(characterResourceLink, lobbyPlayerInfo, false);
							if (lobbyPlayerInfo.AccountId == ClientGameManager.Get().GetPlayerAccountData().AccountId)
							{
								if (i > 0)
								{
									CharacterType charType = m_allyTeamPlayers[0].GetCharType();
									LobbyPlayerInfo playerInfo2 = m_allyTeamPlayers[0].GetPlayerInfo();
									m_allyTeamPlayers[0].Setup(GameWideData.Get().GetCharacterResourceLink(m_allyTeamPlayers[i].GetCharType()), m_allyTeamPlayers[i].GetPlayerInfo(), false);
									m_allyTeamPlayers[i].Setup(GameWideData.Get().GetCharacterResourceLink(charType), playerInfo2, false);
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
					while (i < m_allyTeamPlayers.Length)
					{
						UIManager.SetGameObjectActive(m_allyTeamPlayers[i], false);
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
							m_enemyTeamPlayers[j].Setup(characterResourceLink2, lobbyPlayerInfo2, true);
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
					while (j < m_enemyTeamPlayers.Length)
					{
						UIManager.SetGameObjectActive(m_enemyTeamPlayers[j], false);
						j++;
					}
					MapData mapData = GameWideData.Get().GetMapData(gameInfo.GameConfig.Map);
					if (mapData != null)
					{
						m_mapLoadingImage.sprite = (Resources.Load(mapData.ResourceImageSpriteLocation, typeof(Sprite)) as Sprite);
					}
					else
					{
						m_mapLoadingImage.sprite = (Resources.Load("Stages/information_stage_image", typeof(Sprite)) as Sprite);
					}

					m_mapName.text = new StringBuilder().Append("- ").Append(GameWideData.Get().GetMapDisplayName(gameInfo.GameConfig.Map)).Append(" -").ToString();
					SetTutorialTip();
					m_gameMode.text = gameInfo.GameConfig.GameType.GetDisplayName();
					GameSubType.GameLoadScreenInstructions instructionsToDisplay = gameInfo.GameConfig.InstanceSubType.InstructionsToDisplay;
					if (instructionsToDisplay == GameSubType.GameLoadScreenInstructions.Default)
					{
						UIManager.SetGameObjectActive(m_instructionPairs[0].m_container, true);
						for (int k = 1; k < m_instructionPairs.Length; k++)
						{
							UIManager.SetGameObjectActive(m_instructionPairs[k].m_container, false);
						}
						UIManager.SetGameObjectActive(m_gameSubType, false);
					}
					else
					{
						bool flag = false;
						for (int l = m_instructionPairs.Length - 1; l > 0; l--)
						{
							if (DoInstructionTypesMatch(m_instructionPairs[l].m_InstructionsType, instructionsToDisplay))
							{
								UIManager.SetGameObjectActive(m_instructionPairs[l].m_container, true);
								flag = true;
							}
							else
							{
								UIManager.SetGameObjectActive(m_instructionPairs[l].m_container, false);
							}
						}
						Component container = m_instructionPairs[0].m_container;
						bool doActive;
						if (flag)
						{
							doActive = (instructionsToDisplay == GameSubType.GameLoadScreenInstructions.Default);
						}
						else
						{
							doActive = true;
						}
						UIManager.SetGameObjectActive(container, doActive);
						UIManager.SetGameObjectActive(m_gameSubType, true);
						if (gameInfo.GameConfig.InstanceSubType.LocalizedName != null)
						{
							m_gameSubType.text = StringUtil.TR(gameInfo.GameConfig.InstanceSubType.LocalizedName);
						}
						else
						{
							m_gameSubType.text = string.Empty;
						}
						m_tooltipSubtypeName.text = StringUtil.TR(gameInfo.GameConfig.InstanceSubType.LocalizedName);
						GameSubTypeData.GameSubTypeInstructions instructionSet = GameSubTypeData.Get().GetInstructionSet(instructionsToDisplay);
						if (instructionSet != null)
						{
							for (int m = 0; m < m_tooltips.Length; m++)
							{
								if (m < instructionSet.DisplayInfos.Length)
								{
									UIManager.SetGameObjectActive(m_tooltips[m], true);
									m_tooltips[m].Setup(instructionSet.DisplayInfos[m]);
								}
								else
								{
									UIManager.SetGameObjectActive(m_tooltips[m], false);
								}
							}
						}
					}
				}
			}
		}
	}

	public void SetTipIndex(int index)
	{
		m_tipIndex = index;
	}

	public enum UIInstructionDisplayTipType
	{
		Default,
		GeneralGameSpecific
	}

	[Serializable]
	public class InstructionPair
	{
		public UIInstructionDisplayTipType m_InstructionsType;

		public RectTransform m_container;
	}
}
