﻿using System;
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
		UIManager.SetGameObjectActive(this.m_characterSelect, false, null);
		bool flag;
		if (GameManager.Get() != null)
		{
			flag = GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
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
					if (characterResourceLink.m_allowForPlayers)
					{
						if (!flag2 && characterResourceLink.m_isHidden)
						{
						}
						else
						{
							CharacterConfig characterConfig = GameManager.Get().GameplayOverrides.GetCharacterConfig(characterResourceLink.m_characterType);
							if (characterConfig.AllowForPlayers)
							{
								if (!flag2)
								{
									if (characterConfig.IsHidden)
									{
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											goto IL_100;
										}
									}
								}
								list.Add(characterType);
							}
							IL_100:;
						}
					}
				}
			}
			catch
			{
			}
		}
		List<CharacterType> list2 = list;
		
		list2.Sort(((CharacterType first, CharacterType second) => first.GetDisplayName().CompareTo(second.GetDisplayName())));
		for (int j = 0; j < list.Count; j++)
		{
			UIPartyPanelCharacterSelect uipartyPanelCharacterSelect = UnityEngine.Object.Instantiate<UIPartyPanelCharacterSelect>(this.m_botCharacterPrefab);
			UIManager.ReparentTransform(uipartyPanelCharacterSelect.gameObject.transform, this.m_botCharacterGridContainer.gameObject.transform, new Vector3(0.55f, 0.55f, 0.55f));
			uipartyPanelCharacterSelect.m_characterType = list[j];
			this.m_CharacterButtons.Add(uipartyPanelCharacterSelect);
		}
		ClientGameManager.Get().OnAccountDataUpdated += this.OnAccountDataUpdated;
		this.m_visible = false;
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= this.OnAccountDataUpdated;
		}
	}

	private bool IsOutOfGame()
	{
		return GameManager.Get().GameStatus == GameStatus.Stopped;
	}

	public bool NotifySwapMainCharacter()
	{
		if (this.m_isSwappingMainCharacter)
		{
			return false;
		}
		this.m_numAccountUpdatesToSkipForSwap++;
		this.m_isSwappingMainCharacter = true;
		return true;
	}

	private void OnAccountDataUpdated(PersistedAccountData data)
	{
		if (!this.IsOutOfGame())
		{
			return;
		}
		if (this.m_isSwappingMainCharacter)
		{
			if (this.m_numAccountUpdatesToSkipForSwap > 0)
			{
				this.m_numAccountUpdatesToSkipForSwap--;
				return;
			}
			UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.Abilities);
			this.m_isSwappingMainCharacter = false;
		}
		this.SetupForOutOfGame(this.m_numAllyPortraits, this.m_isDuplicateCharsAllowed);
	}

	public void SelectedBotCharacter(CharacterResourceLink theChar)
	{
		if (this.IsOutOfGame() && this.CanCharacterBeSelectedOutOfGame(theChar.m_characterType))
		{
			ClientGameManager.Get().UpdateRemoteCharacter(theChar.m_characterType, this.m_selectedPortraitIndex - 1, null);
			this.UnselectPortraits();
		}
		else if (this.m_selectedBotInfo != null && this.CanCharacterBeSelectedByBot(theChar.m_characterType))
		{
			ClientGameManager.Get().UpdateSelectedCharacter(theChar.m_characterType, this.m_selectedBotInfo.PlayerId);
			this.UnselectPortraits();
		}
	}

	public void SelectBotDifficulty(BotDifficulty difficulty)
	{
		if (this.m_selectedBotInfo != null)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			clientGameManager.UpdateBotDifficulty(new BotDifficulty?(difficulty), new BotDifficulty?(difficulty), this.m_selectedBotInfo.PlayerId);
		}
	}

	private bool CanCharacterBeSelectedByBot(CharacterType type)
	{
		GameManager gameManager = GameManager.Get();
		bool flag;
		if (!gameManager.IsCharacterAllowedForBots(type))
		{
			flag = this.m_selectedBotInfo.IsRemoteControlled;
		}
		else
		{
			flag = true;
		}
		bool result = flag;
		if (!GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.AllowDuplicateCharacters))
		{
			using (List<LobbyPlayerInfo>.Enumerator enumerator = gameManager.TeamInfo.TeamPlayerInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
					if (lobbyPlayerInfo.CharacterType == type)
					{
						if (lobbyPlayerInfo.TeamId == this.m_selectedBotInfo.TeamId)
						{
							return false;
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
		if (flag && !this.m_isDuplicateCharsAllowed)
		{
			foreach (UICharacterSelectPlayerPortrait uicharacterSelectPlayerPortrait in this.m_allyPortraits)
			{
				if (uicharacterSelectPlayerPortrait.gameObject.activeSelf && uicharacterSelectPlayerPortrait.CharType == charType)
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
		bool flag;
		if (reveal)
		{
			flag = (this.m_selectedBotInfo != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		UIManager.SetGameObjectActive(this.m_characterSelect, flag2, null);
		if (flag2)
		{
			for (int i = 0; i < this.m_CharacterButtons.Count; i++)
			{
				if (GameManager.Get().IsCharacterVisible(this.m_CharacterButtons[i].m_characterType))
				{
					UIManager.SetGameObjectActive(this.m_CharacterButtons[i], true, null);
					this.m_CharacterButtons[i].Setup(this.CanCharacterBeSelectedByBot(this.m_CharacterButtons[i].m_characterType));
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_CharacterButtons[i], false, null);
				}
			}
			this.m_starsPanel.SetCurrentValue((int)(this.m_selectedBotInfo.Difficulty + 1));
			UIManager.SetGameObjectActive(this.m_starsPanel, true, null);
			UIManager.SetGameObjectActive(this.m_botSkillLabel, true, null);
		}
	}

	public void UnselectPortraits()
	{
		for (int i = 0; i < this.m_enemyPortraits.Length; i++)
		{
			this.m_enemyPortraits[i].SetArrowsSelected(false);
		}
		for (int j = 0; j < this.m_allyPortraits.Length; j++)
		{
			this.m_allyPortraits[j].SetArrowsSelected(false);
		}
		this.ShowBotCharacterSelect(false);
	}

	public void NotifyPlayerPortraitClicked(LobbyPlayerInfo playerInfo)
	{
		this.m_selectedBotInfo = null;
		if (playerInfo != null)
		{
			if (playerInfo.PlayerId == this.m_playerInfo.PlayerId)
			{
				return;
			}
		}
		this.UnselectPortraits();
	}

	public void NotifyBotPortraitClicked(UICharacterSelectPlayerPortrait ui_element, LobbyPlayerInfo botInfo)
	{
		bool reveal = false;
		if (this.m_playerInfo != null)
		{
			if (this.m_playerInfo.IsGameOwner)
			{
				this.m_selectedBotInfo = botInfo;
				for (int i = 0; i < this.m_enemyPortraits.Length; i++)
				{
					if (this.m_enemyPortraits[i].SetArrowsSelected(ui_element == this.m_enemyPortraits[i]))
					{
						reveal = true;
					}
				}
				for (int j = 0; j < this.m_allyPortraits.Length; j++)
				{
					if (this.m_allyPortraits[j].SetArrowsSelected(ui_element == this.m_allyPortraits[j]))
					{
						reveal = true;
					}
				}
			}
		}
		this.ShowBotCharacterSelect(reveal);
	}

	public void NotifyOutOfGamePortraitClicked(UICharacterSelectPlayerPortrait ui_element, CharacterType charType)
	{
		bool flag = false;
		for (int i = 0; i < this.m_enemyPortraits.Length; i++)
		{
			this.m_enemyPortraits[i].SetArrowsSelected(false);
		}
		for (int j = 0; j < this.m_allyPortraits.Length; j++)
		{
			if (this.m_allyPortraits[j].SetArrowsSelected(ui_element == this.m_allyPortraits[j]))
			{
				flag = true;
				this.m_selectedPortraitIndex = j;
			}
		}
		UIManager.SetGameObjectActive(this.m_characterSelect, flag, null);
		if (flag)
		{
			for (int k = 0; k < this.m_CharacterButtons.Count; k++)
			{
				if (GameManager.Get().IsCharacterVisible(this.m_CharacterButtons[k].m_characterType))
				{
					UIManager.SetGameObjectActive(this.m_CharacterButtons[k], true, null);
					this.m_CharacterButtons[k].Setup(this.CanCharacterBeSelectedOutOfGame(this.m_CharacterButtons[k].m_characterType));
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_CharacterButtons[k], false, null);
				}
			}
			UIManager.SetGameObjectActive(this.m_starsPanel, false, null);
			UIManager.SetGameObjectActive(this.m_botSkillLabel, false, null);
		}
	}

	public void SetActive(bool active)
	{
		for (int i = 0; i < this.m_containers.Length; i++)
		{
			if (this.m_containers[i] != null)
			{
				UIManager.SetGameObjectActive(this.m_containers[i], active, null);
			}
		}
	}

	public void SetVisible(bool visible, bool force = false)
	{
		if (visible == this.m_visible)
		{
			if (!force)
			{
				return;
			}
		}
		this.m_visible = visible;
		if (this.m_animator == null)
		{
			this.m_animator = base.GetComponent<Animator>();
		}
		if (this.m_visible)
		{
			UIAnimationEventManager.Get().PlayAnimation(this.m_animator, "Showing", null, string.Empty, 0, 0f, true, false, null, null);
		}
		else
		{
			UIAnimationEventManager.Get().PlayAnimation(this.m_animator, "Hiding", null, string.Empty, 0, 0f, true, false, null, null);
			this.UnselectPortraits();
		}
	}

	public void UpdateCharacterList(LobbyPlayerInfo playerInfo, LobbyTeamInfo teamInfo, LobbyGameInfo gameInfo)
	{
		int i = 0;
		int j = 0;
		int num = 0;
		int num2 = 0;
		this.m_playerInfo = playerInfo;
		if (playerInfo != null)
		{
			if (teamInfo != null)
			{
				if (gameInfo != null)
				{
					if (teamInfo.TeamPlayerInfo != null)
					{
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
								LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
								if (lobbyPlayerInfo.PlayerId == this.m_playerInfo.PlayerId)
								{
									if (lobbyPlayerInfo.TeamId == team)
									{
										this.m_allyPortraits[i].SetEnabled(true);
										this.m_allyPortraits[i].Setup(lobbyPlayerInfo);
										i++;
									}
									else if (lobbyPlayerInfo.TeamId == team2)
									{
										this.m_enemyPortraits[j].SetEnabled(true);
										this.m_enemyPortraits[j].Setup(lobbyPlayerInfo);
										j++;
									}
								}
							}
						}
						foreach (LobbyPlayerInfo lobbyPlayerInfo2 in teamInfo.TeamPlayerInfo)
						{
							if (lobbyPlayerInfo2.PlayerId != this.m_playerInfo.PlayerId)
							{
								if (lobbyPlayerInfo2.TeamId == team)
								{
									this.m_allyPortraits[i].SetEnabled(true);
									this.m_allyPortraits[i].Setup(lobbyPlayerInfo2);
									i++;
								}
								else if (lobbyPlayerInfo2.TeamId == team2)
								{
									this.m_enemyPortraits[j].SetEnabled(true);
									this.m_enemyPortraits[j].Setup(lobbyPlayerInfo2);
									j++;
								}
							}
						}
						while (j < this.m_enemyPortraits.Length)
						{
							if (j < num2)
							{
								this.m_enemyPortraits[j].SetEnabled(true);
								this.m_enemyPortraits[j].Setup(null);
							}
							else
							{
								this.m_enemyPortraits[j].SetEnabled(false);
							}
							j++;
						}
						while (i < this.m_allyPortraits.Length)
						{
							if (i < num)
							{
								this.m_allyPortraits[i].SetEnabled(true);
								this.m_allyPortraits[i].Setup(null);
							}
							else
							{
								this.m_allyPortraits[i].SetEnabled(false);
							}
							i++;
						}
						return;
					}
				}
			}
		}
	}

	public void SetupForOutOfGame(int numAllyPortraits, bool isDuplicateCharsAllowed)
	{
		this.m_isDuplicateCharsAllowed = isDuplicateCharsAllowed;
		this.m_numAllyPortraits = numAllyPortraits;
		for (int i = 0; i < this.m_enemyPortraits.Length; i++)
		{
			this.m_enemyPortraits[i].SetEnabled(false);
		}
		AccountComponent accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
		this.m_allyPortraits[0].Setup(accountComponent.LastCharacter, true);
		this.m_allyPortraits[0].SetEnabled(true);
		for (int j = 1; j < this.m_allyPortraits.Length; j++)
		{
			CharacterType charType = CharacterType.None;
			if (j - 1 < accountComponent.LastRemoteCharacters.Count)
			{
				charType = accountComponent.LastRemoteCharacters[j - 1];
			}
			this.m_allyPortraits[j].Setup(charType, false);
			this.m_allyPortraits[j].SetEnabled(j < numAllyPortraits);
		}
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
						if (!list3.Contains(item))
						{
							if (!accountComponent.LastRemoteCharacters.Contains(item))
							{
								break;
							}
						}
					}
					list.Add(item);
					list2.Add(k);
				}
				list3.Add(item);
			}
			if (list.Count > 0)
			{
				ClientGameManager.Get().UpdateRemoteCharacter(list.ToArray(), list2.ToArray(), null);
			}
		}
		this.UnselectPortraits();
	}
}
