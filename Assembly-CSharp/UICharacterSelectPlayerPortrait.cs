using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterSelectPlayerPortrait : MonoBehaviour
{
	public Image m_characterImage;

	public Image m_notReadyImage;

	public Image m_readyImage;

	public Image m_partyLeaderImage;

	public TextMeshProUGUI m_playerName;

	public Button m_hitbox;

	public RectTransform m_selectedArrows;

	public Image[] m_stars;

	public _SelectableBtn m_cogBtn;

	private bool m_isMainCharacter;

	private bool m_isOutsideGame;

	private LobbyPlayerInfo m_playerInfo;

	private bool m_isSelected;

	private CharacterType m_charType;

	public bool IsSelected => m_isSelected;

	public CharacterType CharType
	{
		get
		{
			if (m_isOutsideGame)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return m_charType;
					}
				}
			}
			return m_playerInfo.CharacterType;
		}
	}

	private void Start()
	{
		UIManager.SetGameObjectActive(m_selectedArrows, false);
		UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerClick, OnPortraitClick);
		m_cogBtn.spriteController.callback = OnCogBtnClick;
	}

	public bool SetArrowsSelected(bool setArrowsVisible)
	{
		bool flag = setArrowsVisible;
		if (flag)
		{
			if (m_selectedArrows.gameObject.activeSelf)
			{
				flag = false;
			}
		}
		UIManager.SetGameObjectActive(m_selectedArrows, flag);
		m_isSelected = flag;
		return flag;
	}

	private bool IsClickable()
	{
		if (m_isOutsideGame)
		{
			return !m_isMainCharacter;
		}
		if (AppState.GetCurrent() != AppState_CharacterSelect.Get())
		{
			return true;
		}
		if (GameManager.Get() == null || GameManager.Get().PlayerInfo == null)
		{
			return true;
		}
		if (GameManager.Get().GameStatus == GameStatus.LoadoutSelecting)
		{
			return false;
		}
		if (m_playerInfo == null)
		{
			return false;
		}
		if (m_playerInfo.PlayerId == GameManager.Get().PlayerInfo.PlayerId)
		{
			return GameManager.Get().PlayerInfo.ReadyState != ReadyState.Ready;
		}
		return GameManager.Get().PlayerInfo.IsGameOwner
		       && (m_playerInfo.IsNPCBot || m_playerInfo.IsRemoteControlled);
	}

	public void OnPortraitClick(BaseEventData data)
	{
		if (!IsClickable())
		{
			return;
		}
		while (true)
		{
			if (m_isOutsideGame)
			{
				UICharacterScreen.Get().m_partyListPanel.NotifyOutOfGamePortraitClicked(this, m_charType);
				return;
			}
			if (m_playerInfo != null)
			{
				if (!m_playerInfo.IsNPCBot)
				{
					if (!m_playerInfo.IsRemoteControlled)
					{
						goto IL_008a;
					}
				}
				UICharacterScreen.Get().m_partyListPanel.NotifyBotPortraitClicked(this, m_playerInfo);
				return;
			}
			goto IL_008a;
			IL_008a:
			UICharacterScreen.Get().m_partyListPanel.NotifyPlayerPortraitClicked(m_playerInfo);
			return;
		}
	}

	public void OnCogBtnClick(BaseEventData data)
	{
		if (!IsClickable())
		{
			return;
		}
		while (true)
		{
			if (!m_isOutsideGame)
			{
				return;
			}
			while (true)
			{
				if (m_isMainCharacter)
				{
					return;
				}
				while (true)
				{
					if ((!(AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()) && !(AppState_LandingPage.Get() == AppState.GetCurrent())) ? AppState_CharacterSelect.IsReady() : AppState_GroupCharacterSelect.Get().IsReady())
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
					if (!UICharacterScreen.Get().m_partyListPanel.NotifySwapMainCharacter())
					{
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
					UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
					CharacterType characterTypeToDisplay = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay;
					UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
					{
						ClientRequestToServerSelectCharacter = m_charType
					});
					UICharacterSelectPlayerPortrait[] allyPortraits = UICharacterScreen.Get().m_partyListPanel.m_allyPortraits;
					for (int i = 1; i < allyPortraits.Length; i++)
					{
						if (this == allyPortraits[i])
						{
							ClientGameManager.Get().UpdateRemoteCharacter(characterTypeToDisplay, i - 1);
							return;
						}
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
	}

	public void SetEnabled(bool enabled)
	{
		UIManager.SetGameObjectActive(base.gameObject, enabled);
	}

	public void Setup(LobbyPlayerInfo info)
	{
		m_playerInfo = info;
		m_isOutsideGame = false;
		UIManager.SetGameObjectActive(m_cogBtn, false);
		if (info != null)
		{
			if (info.CharacterType == CharacterType.None)
			{
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(CharacterType.Gremlins);
				m_characterImage.sprite = characterResourceLink.GetCharacterSelectIconBW();
			}
			else
			{
				CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(info.CharacterType);
				m_characterImage.sprite = characterResourceLink2.GetCharacterSelectIcon();
			}
			UIManager.SetGameObjectActive(m_characterImage, true);
			if (!info.IsNPCBot)
			{
				if (!info.IsRemoteControlled)
				{
					UIManager.SetGameObjectActive(m_readyImage, info.ReadyState == ReadyState.Ready);
					UIManager.SetGameObjectActive(m_notReadyImage, info.ReadyState != ReadyState.Ready);
					Image[] stars = m_stars;
					foreach (Image component in stars)
					{
						UIManager.SetGameObjectActive(component, false);
					}
					goto IL_0177;
				}
			}
			UIManager.SetGameObjectActive(m_readyImage, true);
			UIManager.SetGameObjectActive(m_notReadyImage, false);
			for (int j = 0; j < 5; j++)
			{
				Image component2 = m_stars[j];
				int doActive;
				if (!info.IsRemoteControlled)
				{
					doActive = ((j <= (int)info.Difficulty) ? 1 : 0);
				}
				else
				{
					doActive = 0;
				}
				UIManager.SetGameObjectActive(component2, (byte)doActive != 0);
			}
			goto IL_0177;
		}
		UIManager.SetGameObjectActive(m_characterImage, false);
		UIManager.SetGameObjectActive(m_readyImage, false);
		UIManager.SetGameObjectActive(m_notReadyImage, false);
		UIManager.SetGameObjectActive(m_partyLeaderImage, false);
		m_playerName.text = StringUtil.TR("Empty", "Global");
		Image[] stars2 = m_stars;
		foreach (Image component3 in stars2)
		{
			UIManager.SetGameObjectActive(component3, false);
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
		IL_0177:
		Image partyLeaderImage = m_partyLeaderImage;
		int doActive2;
		if (info.IsGameOwner)
		{
			doActive2 = ((GameManager.Get().GameConfig.GameType == GameType.Custom) ? 1 : 0);
		}
		else
		{
			doActive2 = 0;
		}
		UIManager.SetGameObjectActive(partyLeaderImage, (byte)doActive2 != 0);
		m_playerName.text = info.GetHandle();
	}

	public void Setup(CharacterType charType, bool isMainCharacter)
	{
		_SelectableBtn cogBtn = m_cogBtn;
		int doActive;
		if (!isMainCharacter)
		{
			doActive = (charType.IsValidForHumanGameplay() ? 1 : 0);
		}
		else
		{
			doActive = 0;
		}
		UIManager.SetGameObjectActive(cogBtn, (byte)doActive != 0);
		m_isMainCharacter = isMainCharacter;
		m_isOutsideGame = true;
		m_charType = charType;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (AppState.GetCurrent() == AppState_CharacterSelect.Get() || clientGameManager == null)
		{
			return;
		}
		while (true)
		{
			if (clientGameManager.IsPlayerAccountDataAvailable())
			{
				if (charType != 0)
				{
					CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charType);
					m_characterImage.sprite = characterResourceLink.GetCharacterSelectIcon();
					UIManager.SetGameObjectActive(m_characterImage, true);
					m_playerName.text = clientGameManager.Handle;
				}
				else
				{
					UIManager.SetGameObjectActive(m_characterImage, false);
					m_playerName.text = StringUtil.TR("Empty", "Global");
				}
				UIManager.SetGameObjectActive(m_readyImage, false);
				UIManager.SetGameObjectActive(m_notReadyImage, false);
				UIManager.SetGameObjectActive(m_partyLeaderImage, false);
				Image[] stars = m_stars;
				foreach (Image component in stars)
				{
					UIManager.SetGameObjectActive(component, false);
				}
			}
			return;
		}
	}
}
