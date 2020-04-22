using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSelectPartyList : MonoBehaviour
{
	public UICharacterSelectPlayerPortrait[] m_allyPortraits;

	public UICharacterSelectPlayerPortrait[] m_enemyPortraits;

	public RectTransform m_characterSelect;

	public UIStarsPanel m_starsPanel;

	public TextMeshProUGUI m_botSkillLabel;

	public LayoutGroup m_botCharacterGridContainer;

	public UIPartyPanelCharacterSelect m_botCharacterPrefab;

	public RectTransform[] m_containers;

	private LobbyPlayerInfo m_playerInfo;

	private LobbyPlayerInfo m_selectedBotInfo;

	private List<UIPartyPanelCharacterSelect> m_CharacterButtons = new List<UIPartyPanelCharacterSelect>();

	private Animator m_animator;

	private bool m_visible;

	private bool m_isDuplicateCharsAllowed;

	private int m_selectedPortraitIndex;

	private int m_numAllyPortraits;

	private int m_numAccountUpdatesToSkipForSwap;

	private bool m_isSwappingMainCharacter;

	private void Start()
	{
		UIManager.SetGameObjectActive(m_characterSelect, false);
		int num;
		if (GameManager.Get() != null)
		{
			num = (GameManager.Get().GameplayOverrides.EnableHiddenCharacters ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		CharacterType[] array = (CharacterType[])Enum.GetValues(typeof(CharacterType));
		List<CharacterType> list = new List<CharacterType>();
		for (int i = 0; i < array.Length; i++)
		{
			try
			{
				CharacterType characterType = array[i];
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(characterType);
				if (characterType.IsValidForHumanGameplay())
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							if (characterResourceLink.m_allowForPlayers)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										{
											if (!flag && characterResourceLink.m_isHidden)
											{
											}
											else
											{
												CharacterConfig characterConfig = GameManager.Get().GameplayOverrides.GetCharacterConfig(characterResourceLink.m_characterType);
												if (characterConfig.AllowForPlayers)
												{
													if (flag)
													{
														goto IL_0102;
													}
													if (!characterConfig.IsHidden)
													{
														goto IL_0102;
													}
												}
											}
											goto end_IL_0068;
										}
										IL_0102:
										list.Add(characterType);
										goto end_IL_0068;
									}
								}
							}
							goto end_IL_0068;
						}
					}
				}
				end_IL_0068:;
			}
			catch
			{
			}
		}
		while (true)
		{
			if (_003C_003Ef__am_0024cache0 == null)
			{
				_003C_003Ef__am_0024cache0 = ((CharacterType first, CharacterType second) => first.GetDisplayName().CompareTo(second.GetDisplayName()));
			}
			list.Sort(_003C_003Ef__am_0024cache0);
			for (int j = 0; j < list.Count; j++)
			{
				UIPartyPanelCharacterSelect uIPartyPanelCharacterSelect = UnityEngine.Object.Instantiate(m_botCharacterPrefab);
				UIManager.ReparentTransform(uIPartyPanelCharacterSelect.gameObject.transform, m_botCharacterGridContainer.gameObject.transform, new Vector3(0.55f, 0.55f, 0.55f));
				uIPartyPanelCharacterSelect.m_characterType = list[j];
				m_CharacterButtons.Add(uIPartyPanelCharacterSelect);
			}
			while (true)
			{
				ClientGameManager.Get().OnAccountDataUpdated += OnAccountDataUpdated;
				m_visible = false;
				return;
			}
		}
	}

	private void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= OnAccountDataUpdated;
			return;
		}
	}

	private bool IsOutOfGame()
	{
		return GameManager.Get().GameStatus == GameStatus.Stopped;
	}

	public bool NotifySwapMainCharacter()
	{
		if (m_isSwappingMainCharacter)
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
		m_numAccountUpdatesToSkipForSwap++;
		m_isSwappingMainCharacter = true;
		return true;
	}

	private void OnAccountDataUpdated(PersistedAccountData data)
	{
		if (!IsOutOfGame())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_isSwappingMainCharacter)
		{
			if (m_numAccountUpdatesToSkipForSwap > 0)
			{
				while (true)
				{
					m_numAccountUpdatesToSkipForSwap--;
					return;
				}
			}
			UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.Abilities);
			m_isSwappingMainCharacter = false;
		}
		SetupForOutOfGame(m_numAllyPortraits, m_isDuplicateCharsAllowed);
	}

	public void SelectedBotCharacter(CharacterResourceLink theChar)
	{
		if (IsOutOfGame() && CanCharacterBeSelectedOutOfGame(theChar.m_characterType))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					ClientGameManager.Get().UpdateRemoteCharacter(theChar.m_characterType, m_selectedPortraitIndex - 1);
					UnselectPortraits();
					return;
				}
			}
		}
		if (m_selectedBotInfo == null || !CanCharacterBeSelectedByBot(theChar.m_characterType))
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().UpdateSelectedCharacter(theChar.m_characterType, m_selectedBotInfo.PlayerId);
			UnselectPortraits();
			return;
		}
	}

	public void SelectBotDifficulty(BotDifficulty difficulty)
	{
		if (m_selectedBotInfo == null)
		{
			return;
		}
		while (true)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			clientGameManager.UpdateBotDifficulty(difficulty, difficulty, m_selectedBotInfo.PlayerId);
			return;
		}
	}

	private bool CanCharacterBeSelectedByBot(CharacterType type)
	{
		GameManager gameManager = GameManager.Get();
		int num;
		if (!gameManager.IsCharacterAllowedForBots(type))
		{
			num = (m_selectedBotInfo.IsRemoteControlled ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool result = (byte)num != 0;
		if (!GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.AllowDuplicateCharacters))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					using (List<LobbyPlayerInfo>.Enumerator enumerator = gameManager.TeamInfo.TeamPlayerInfo.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							LobbyPlayerInfo current = enumerator.Current;
							if (current.CharacterType == type)
							{
								if (current.TeamId == m_selectedBotInfo.TeamId)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											return false;
										}
									}
								}
							}
						}
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return result;
							}
						}
					}
				}
				}
			}
		}
		return result;
	}

	private bool CanCharacterBeSelectedOutOfGame(CharacterType charType)
	{
		bool flag = ClientGameManager.Get().IsCharacterAvailable(charType, ClientGameManager.Get().GroupInfo.SelectedQueueType);
		if (flag && !m_isDuplicateCharsAllowed)
		{
			UICharacterSelectPlayerPortrait[] allyPortraits = m_allyPortraits;
			foreach (UICharacterSelectPlayerPortrait uICharacterSelectPlayerPortrait in allyPortraits)
			{
				if (uICharacterSelectPlayerPortrait.gameObject.activeSelf && uICharacterSelectPlayerPortrait.CharType == charType)
				{
					flag = false;
					break;
				}
			}
		}
		return flag;
	}

	private void ShowBotCharacterSelect(bool reveal)
	{
		int num;
		if (reveal)
		{
			num = ((m_selectedBotInfo != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		UIManager.SetGameObjectActive(m_characterSelect, flag);
		if (!flag)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_CharacterButtons.Count; i++)
			{
				if (GameManager.Get().IsCharacterVisible(m_CharacterButtons[i].m_characterType))
				{
					UIManager.SetGameObjectActive(m_CharacterButtons[i], true);
					m_CharacterButtons[i].Setup(CanCharacterBeSelectedByBot(m_CharacterButtons[i].m_characterType));
				}
				else
				{
					UIManager.SetGameObjectActive(m_CharacterButtons[i], false);
				}
			}
			m_starsPanel.SetCurrentValue((int)(m_selectedBotInfo.Difficulty + 1));
			UIManager.SetGameObjectActive(m_starsPanel, true);
			UIManager.SetGameObjectActive(m_botSkillLabel, true);
			return;
		}
	}

	public void UnselectPortraits()
	{
		for (int i = 0; i < m_enemyPortraits.Length; i++)
		{
			m_enemyPortraits[i].SetArrowsSelected(false);
		}
		while (true)
		{
			for (int j = 0; j < m_allyPortraits.Length; j++)
			{
				m_allyPortraits[j].SetArrowsSelected(false);
			}
			while (true)
			{
				ShowBotCharacterSelect(false);
				return;
			}
		}
	}

	public void NotifyPlayerPortraitClicked(LobbyPlayerInfo playerInfo)
	{
		m_selectedBotInfo = null;
		if (playerInfo != null)
		{
			if (playerInfo.PlayerId == m_playerInfo.PlayerId)
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
		}
		UnselectPortraits();
	}

	public void NotifyBotPortraitClicked(UICharacterSelectPlayerPortrait ui_element, LobbyPlayerInfo botInfo)
	{
		bool reveal = false;
		if (m_playerInfo != null)
		{
			if (m_playerInfo.IsGameOwner)
			{
				m_selectedBotInfo = botInfo;
				for (int i = 0; i < m_enemyPortraits.Length; i++)
				{
					if (m_enemyPortraits[i].SetArrowsSelected(ui_element == m_enemyPortraits[i]))
					{
						reveal = true;
					}
				}
				for (int j = 0; j < m_allyPortraits.Length; j++)
				{
					if (m_allyPortraits[j].SetArrowsSelected(ui_element == m_allyPortraits[j]))
					{
						reveal = true;
					}
				}
			}
		}
		ShowBotCharacterSelect(reveal);
	}

	public void NotifyOutOfGamePortraitClicked(UICharacterSelectPlayerPortrait ui_element, CharacterType charType)
	{
		bool flag = false;
		for (int i = 0; i < m_enemyPortraits.Length; i++)
		{
			m_enemyPortraits[i].SetArrowsSelected(false);
		}
		while (true)
		{
			for (int j = 0; j < m_allyPortraits.Length; j++)
			{
				if (m_allyPortraits[j].SetArrowsSelected(ui_element == m_allyPortraits[j]))
				{
					flag = true;
					m_selectedPortraitIndex = j;
				}
			}
			while (true)
			{
				UIManager.SetGameObjectActive(m_characterSelect, flag);
				if (!flag)
				{
					return;
				}
				for (int k = 0; k < m_CharacterButtons.Count; k++)
				{
					if (GameManager.Get().IsCharacterVisible(m_CharacterButtons[k].m_characterType))
					{
						UIManager.SetGameObjectActive(m_CharacterButtons[k], true);
						m_CharacterButtons[k].Setup(CanCharacterBeSelectedOutOfGame(m_CharacterButtons[k].m_characterType));
					}
					else
					{
						UIManager.SetGameObjectActive(m_CharacterButtons[k], false);
					}
				}
				while (true)
				{
					UIManager.SetGameObjectActive(m_starsPanel, false);
					UIManager.SetGameObjectActive(m_botSkillLabel, false);
					return;
				}
			}
		}
	}

	public void SetActive(bool active)
	{
		for (int i = 0; i < m_containers.Length; i++)
		{
			if (m_containers[i] != null)
			{
				UIManager.SetGameObjectActive(m_containers[i], active);
			}
		}
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

	public void SetVisible(bool visible, bool force = false)
	{
		if (visible == m_visible)
		{
			if (!force)
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
		}
		m_visible = visible;
		if (m_animator == null)
		{
			m_animator = GetComponent<Animator>();
		}
		if (m_visible)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					UIAnimationEventManager.Get().PlayAnimation(m_animator, "Showing", null, string.Empty);
					return;
				}
			}
		}
		UIAnimationEventManager.Get().PlayAnimation(m_animator, "Hiding", null, string.Empty);
		UnselectPortraits();
	}

	public void UpdateCharacterList(LobbyPlayerInfo playerInfo, LobbyTeamInfo teamInfo, LobbyGameInfo gameInfo)
	{
		int i = 0;
		int j = 0;
		int num = 0;
		int num2 = 0;
		m_playerInfo = playerInfo;
		if (playerInfo == null)
		{
			return;
		}
		while (true)
		{
			if (teamInfo == null)
			{
				return;
			}
			while (true)
			{
				if (gameInfo == null)
				{
					return;
				}
				while (true)
				{
					if (teamInfo.TeamPlayerInfo == null)
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
					Team team = Team.TeamA;
					Team team2 = Team.TeamB;
					if (playerInfo.TeamId == Team.TeamA)
					{
						team = Team.TeamA;
						team2 = Team.TeamB;
						num = gameInfo.GameConfig.TeamAPlayers;
						num2 = gameInfo.GameConfig.TeamBPlayers;
					}
					else if (playerInfo.TeamId == Team.TeamB)
					{
						team = Team.TeamB;
						team2 = Team.TeamA;
						num = gameInfo.GameConfig.TeamBPlayers;
						num2 = gameInfo.GameConfig.TeamAPlayers;
					}
					using (List<LobbyPlayerInfo>.Enumerator enumerator = teamInfo.TeamPlayerInfo.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							LobbyPlayerInfo current = enumerator.Current;
							if (current.PlayerId == m_playerInfo.PlayerId)
							{
								if (current.TeamId == team)
								{
									m_allyPortraits[i].SetEnabled(true);
									m_allyPortraits[i].Setup(current);
									i++;
								}
								else if (current.TeamId == team2)
								{
									m_enemyPortraits[j].SetEnabled(true);
									m_enemyPortraits[j].Setup(current);
									j++;
								}
							}
						}
					}
					foreach (LobbyPlayerInfo item in teamInfo.TeamPlayerInfo)
					{
						if (item.PlayerId != m_playerInfo.PlayerId)
						{
							if (item.TeamId == team)
							{
								m_allyPortraits[i].SetEnabled(true);
								m_allyPortraits[i].Setup(item);
								i++;
							}
							else if (item.TeamId == team2)
							{
								m_enemyPortraits[j].SetEnabled(true);
								m_enemyPortraits[j].Setup(item);
								j++;
							}
						}
					}
					for (; j < m_enemyPortraits.Length; j++)
					{
						if (j < num2)
						{
							m_enemyPortraits[j].SetEnabled(true);
							m_enemyPortraits[j].Setup(null);
						}
						else
						{
							m_enemyPortraits[j].SetEnabled(false);
						}
					}
					for (; i < m_allyPortraits.Length; i++)
					{
						if (i < num)
						{
							m_allyPortraits[i].SetEnabled(true);
							m_allyPortraits[i].Setup(null);
						}
						else
						{
							m_allyPortraits[i].SetEnabled(false);
						}
					}
					while (true)
					{
						switch (1)
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

	public void SetupForOutOfGame(int numAllyPortraits, bool isDuplicateCharsAllowed)
	{
		m_isDuplicateCharsAllowed = isDuplicateCharsAllowed;
		m_numAllyPortraits = numAllyPortraits;
		for (int i = 0; i < m_enemyPortraits.Length; i++)
		{
			m_enemyPortraits[i].SetEnabled(false);
		}
		while (true)
		{
			AccountComponent accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
			m_allyPortraits[0].Setup(accountComponent.LastCharacter, true);
			m_allyPortraits[0].SetEnabled(true);
			for (int j = 1; j < m_allyPortraits.Length; j++)
			{
				CharacterType charType = CharacterType.None;
				if (j - 1 < accountComponent.LastRemoteCharacters.Count)
				{
					charType = accountComponent.LastRemoteCharacters[j - 1];
				}
				m_allyPortraits[j].Setup(charType, false);
				m_allyPortraits[j].SetEnabled(j < numAllyPortraits);
			}
			while (true)
			{
				if (!isDuplicateCharsAllowed)
				{
					List<CharacterType> list = new List<CharacterType>();
					List<int> list2 = new List<int>();
					List<CharacterType> list3 = new List<CharacterType>();
					list3.Add(accountComponent.LastCharacter);
					for (int k = 0; k < accountComponent.LastRemoteCharacters.Count; k++)
					{
						CharacterType item = accountComponent.LastRemoteCharacters[k];
						if (list3.Contains(item))
						{
							for (int l = 0; l < accountComponent.FreeRotationCharacters.Length; l++)
							{
								item = accountComponent.FreeRotationCharacters[l];
								if (list3.Contains(item))
								{
									continue;
								}
								if (!accountComponent.LastRemoteCharacters.Contains(item))
								{
									break;
								}
							}
							list.Add(item);
							list2.Add(k);
						}
						list3.Add(item);
					}
					if (list.Count > 0)
					{
						ClientGameManager.Get().UpdateRemoteCharacter(list.ToArray(), list2.ToArray());
					}
				}
				UnselectPortraits();
				return;
			}
		}
	}
}
