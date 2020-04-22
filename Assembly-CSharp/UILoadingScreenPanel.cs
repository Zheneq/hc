using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILoadingScreenPanel : UIScene
{
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
			while (true)
			{
				switch (5)
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
			m_tipTimerFill.fillAmount = 0f;
		}
		if (m_nextTip != null)
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
			m_nextTip.callback = NextTipClicked;
		}
		if (m_previousTip != null)
		{
			m_previousTip.callback = PrevTipClicked;
		}
		m_visibleStartTime = Time.time;
		m_tipIndex = UnityEngine.Random.Range(0, GameWideData.Get().m_loadingTips.Length);
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
			UIManager.SetGameObjectActive(m_container, visible);
			if (visible)
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
				m_visibleStartTime = Time.time;
			}
			else
			{
				Log.Info(Log.Category.Loading, "Loading screen displayed for {0} seconds.", (Time.time - m_visibleStartTime).ToString("F1"));
				if (m_timeStart != 0f)
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
					m_tipIndex++;
					m_timeStart = 0f;
				}
			}
		}
		if (!visible || !(UIMainMenu.Get() != null))
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
			if (UIMainMenu.Get().IsOpen())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					UIMainMenu.Get().SetMenuVisible(false);
					return;
				}
			}
			return;
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
			m_tipIndex = GameWideData.Get().m_loadingTips.Length - 1;
		}
		else if (m_tipIndex >= GameWideData.Get().m_loadingTips.Length)
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
			m_tipIndex = 0;
		}
		m_tipText.text = GameWideData.Get().GetLoadingScreenTip(m_tipIndex);
		StartTimer();
	}

	private UILoadscreenProfile GetProfile(int playerId)
	{
		if (m_allyTeamPlayers != null)
		{
			for (int i = 0; i < m_allyTeamPlayers.Length; i++)
			{
				if (m_allyTeamPlayers[i].m_playerId != playerId)
				{
					continue;
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
					return m_allyTeamPlayers[i];
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (m_enemyTeamPlayers != null)
		{
			for (int j = 0; j < m_enemyTeamPlayers.Length; j++)
			{
				if (m_enemyTeamPlayers[j].m_playerId != playerId)
				{
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					return m_enemyTeamPlayers[j];
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return null;
	}

	private UILoadscreenProfile GetProfile(LobbyPlayerInfo playerInfo)
	{
		if (m_allyTeamPlayers != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < m_allyTeamPlayers.Length; i++)
			{
				if (m_allyTeamPlayers[i].GetPlayerInfo() != playerInfo)
				{
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					return m_allyTeamPlayers[i];
				}
			}
		}
		if (m_enemyTeamPlayers != null)
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
			for (int j = 0; j < m_enemyTeamPlayers.Length; j++)
			{
				if (m_enemyTeamPlayers[j].GetPlayerInfo() == playerInfo)
				{
					return m_enemyTeamPlayers[j];
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return null;
	}

	private IEnumerable<UILoadscreenProfile> GetBotsMasqueradeAsHumansProfiles()
	{
		if (m_allyTeamPlayers != null)
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
			for (int j = 0; j < m_allyTeamPlayers.Length; j++)
			{
				LobbyPlayerInfo playerInfo2 = m_allyTeamPlayers[j].GetPlayerInfo();
				if (playerInfo2 == null)
				{
					continue;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!playerInfo2.IsNPCBot)
				{
					continue;
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
				if (!playerInfo2.BotsMasqueradeAsHumans)
				{
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					yield return m_allyTeamPlayers[j];
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (m_enemyTeamPlayers == null)
		{
			yield break;
		}
		for (int i = 0; i < m_enemyTeamPlayers.Length; i++)
		{
			LobbyPlayerInfo playerInfo = m_enemyTeamPlayers[i].GetPlayerInfo();
			if (playerInfo == null || !playerInfo.IsNPCBot)
			{
				continue;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!playerInfo.BotsMasqueradeAsHumans)
			{
				continue;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				yield return m_enemyTeamPlayers[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				yield break;
			case 0:
				break;
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
		if (!(profile != null))
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
			profile.m_slider.fillAmount = Mathf.Max(profile.m_slider.fillAmount, loadingProgress);
			if (isLocal)
			{
				LobbyPlayerInfo playerInfo = profile.GetPlayerInfo();
				if (playerInfo.BotsMasqueradeAsHumans)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						IEnumerable<UILoadscreenProfile> botsMasqueradeAsHumansProfiles = GetBotsMasqueradeAsHumansProfiles();
						IEnumerator<UILoadscreenProfile> enumerator = botsMasqueradeAsHumansProfiles.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								UILoadscreenProfile current = enumerator.Current;
								current.m_slider.fillAmount = Mathf.Max(current.m_slider.fillAmount, loadingProgress);
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
						finally
						{
							if (enumerator != null)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										enumerator.Dispose();
										goto end_IL_00b1;
									}
								}
							}
							end_IL_00b1:;
						}
					}
				}
				return;
			}
			return;
		}
	}

	private void Update()
	{
		if (!m_container.gameObject.activeSelf)
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
			if (m_timeStart == 0f)
			{
				return;
			}
			float num = 10f - (Time.time - m_timeStart);
			if (m_tipTimerFill != null)
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
				m_tipTimerFill.fillAmount = num / 10f;
			}
			if (num <= 0f)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					m_tipIndex++;
					SetTutorialTip();
					return;
				}
			}
			return;
		}
	}

	public void StartTimer()
	{
		m_timeStart = Time.time;
		if (!(m_tipTimerFill != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_tipTimerFill.fillAmount = 0f;
			return;
		}
	}

	private bool DoInstructionTypesMatch(UIInstructionDisplayTipType UIDisplayType, GameSubType.GameLoadScreenInstructions GameTypeInstructions)
	{
		switch (UIDisplayType)
		{
		case UIInstructionDisplayTipType.Default:
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return GameTypeInstructions == GameSubType.GameLoadScreenInstructions.Default;
			}
		case UIInstructionDisplayTipType.GeneralGameSpecific:
		{
			int result;
			if (GameTypeInstructions != GameSubType.GameLoadScreenInstructions.Extraction && GameTypeInstructions != GameSubType.GameLoadScreenInstructions.OverpoweredUp)
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
				result = ((GameTypeInstructions == GameSubType.GameLoadScreenInstructions.LightsOut) ? 1 : 0);
			}
			else
			{
				result = 1;
			}
			return (byte)result != 0;
		}
		default:
			return false;
		}
	}

	public void ShowTeams()
	{
		GameManager gameManager = GameManager.Get();
		LobbyPlayerInfo playerInfo = gameManager.PlayerInfo;
		LobbyGameInfo gameInfo = gameManager.GameInfo;
		LobbyTeamInfo teamInfo = gameManager.TeamInfo;
		if (teamInfo == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (playerInfo == null)
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
				if (gameInfo == null)
				{
					while (true)
					{
						switch (5)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				IEnumerable<LobbyPlayerInfo> enumerable;
				IEnumerable<LobbyPlayerInfo> enumerable2;
				if (playerInfo.TeamId == Team.TeamB)
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
						LobbyPlayerInfo current = enumerator.Current;
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(current.CharacterType);
						m_allyTeamPlayers[i].Setup(characterResourceLink, current, false);
						if (current.AccountId == ClientGameManager.Get().GetPlayerAccountData().AccountId)
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
							if (i > 0)
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
								CharacterType charType = m_allyTeamPlayers[0].GetCharType();
								LobbyPlayerInfo playerInfo2 = m_allyTeamPlayers[0].GetPlayerInfo();
								m_allyTeamPlayers[0].Setup(GameWideData.Get().GetCharacterResourceLink(m_allyTeamPlayers[i].GetCharType()), m_allyTeamPlayers[i].GetPlayerInfo(), false);
								m_allyTeamPlayers[i].Setup(GameWideData.Get().GetCharacterResourceLink(charType), playerInfo2, false);
							}
						}
						i++;
						if (i >= num)
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
							break;
						}
					}
				}
				finally
				{
					if (enumerator != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								enumerator.Dispose();
								goto end_IL_01c8;
							}
						}
					}
					end_IL_01c8:;
				}
				for (; i < m_allyTeamPlayers.Length; i++)
				{
					UIManager.SetGameObjectActive(m_allyTeamPlayers[i], false);
				}
				int j = 0;
				IEnumerator<LobbyPlayerInfo> enumerator2 = enumerable2.GetEnumerator();
				try
				{
					do
					{
						if (!enumerator2.MoveNext())
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
							break;
						}
						LobbyPlayerInfo current2 = enumerator2.Current;
						CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(current2.CharacterType);
						m_enemyTeamPlayers[j].Setup(characterResourceLink2, current2, true);
						j++;
					}
					while (j < num2);
				}
				finally
				{
					if (enumerator2 != null)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								enumerator2.Dispose();
								goto end_IL_026b;
							}
						}
					}
					end_IL_026b:;
				}
				for (; j < m_enemyTeamPlayers.Length; j++)
				{
					UIManager.SetGameObjectActive(m_enemyTeamPlayers[j], false);
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					MapData mapData = GameWideData.Get().GetMapData(gameInfo.GameConfig.Map);
					if (mapData != null)
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
						m_mapLoadingImage.sprite = (Resources.Load(mapData.ResourceImageSpriteLocation, typeof(Sprite)) as Sprite);
					}
					else
					{
						m_mapLoadingImage.sprite = (Resources.Load("Stages/information_stage_image", typeof(Sprite)) as Sprite);
					}
					m_mapName.text = "- " + GameWideData.Get().GetMapDisplayName(gameInfo.GameConfig.Map) + " -";
					SetTutorialTip();
					m_gameMode.text = gameInfo.GameConfig.GameType.GetDisplayName();
					GameSubType.GameLoadScreenInstructions instructionsToDisplay = gameInfo.GameConfig.InstanceSubType.InstructionsToDisplay;
					if (instructionsToDisplay == GameSubType.GameLoadScreenInstructions.Default)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
							{
								UIManager.SetGameObjectActive(m_instructionPairs[0].m_container, true);
								for (int k = 1; k < m_instructionPairs.Length; k++)
								{
									UIManager.SetGameObjectActive(m_instructionPairs[k].m_container, false);
								}
								UIManager.SetGameObjectActive(m_gameSubType, false);
								return;
							}
							}
						}
					}
					bool flag = false;
					for (int num3 = m_instructionPairs.Length - 1; num3 > 0; num3--)
					{
						if (DoInstructionTypesMatch(m_instructionPairs[num3].m_InstructionsType, instructionsToDisplay))
						{
							UIManager.SetGameObjectActive(m_instructionPairs[num3].m_container, true);
							flag = true;
						}
						else
						{
							UIManager.SetGameObjectActive(m_instructionPairs[num3].m_container, false);
						}
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						RectTransform container = m_instructionPairs[0].m_container;
						int doActive;
						if (flag)
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
							doActive = ((instructionsToDisplay == GameSubType.GameLoadScreenInstructions.Default) ? 1 : 0);
						}
						else
						{
							doActive = 1;
						}
						UIManager.SetGameObjectActive(container, (byte)doActive != 0);
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
						if (instructionSet == null)
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
							for (int l = 0; l < m_tooltips.Length; l++)
							{
								if (l < instructionSet.DisplayInfos.Length)
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
									UIManager.SetGameObjectActive(m_tooltips[l], true);
									m_tooltips[l].Setup(instructionSet.DisplayInfos[l]);
								}
								else
								{
									UIManager.SetGameObjectActive(m_tooltips[l], false);
								}
							}
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
					}
				}
			}
		}
	}

	public void SetTipIndex(int index)
	{
		m_tipIndex = index;
	}
}
