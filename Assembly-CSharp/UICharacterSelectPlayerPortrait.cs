using System;
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

	public bool IsSelected
	{
		get
		{
			return this.m_isSelected;
		}
	}

	public CharacterType CharType
	{
		get
		{
			if (this.m_isOutsideGame)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectPlayerPortrait.get_CharType()).MethodHandle;
				}
				return this.m_charType;
			}
			return this.m_playerInfo.CharacterType;
		}
	}

	private void Start()
	{
		UIManager.SetGameObjectActive(this.m_selectedArrows, false, null);
		UIEventTriggerUtils.AddListener(this.m_hitbox.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnPortraitClick));
		this.m_cogBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnCogBtnClick);
	}

	public bool SetArrowsSelected(bool setArrowsVisible)
	{
		bool flag = setArrowsVisible;
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectPlayerPortrait.SetArrowsSelected(bool)).MethodHandle;
			}
			if (this.m_selectedArrows.gameObject.activeSelf)
			{
				flag = false;
			}
		}
		UIManager.SetGameObjectActive(this.m_selectedArrows, flag, null);
		this.m_isSelected = flag;
		return flag;
	}

	private bool IsClickable()
	{
		if (this.m_isOutsideGame)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectPlayerPortrait.IsClickable()).MethodHandle;
			}
			return !this.m_isMainCharacter;
		}
		bool flag = true;
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
			if (AppState.GetCurrent() == AppState_CharacterSelect.Get())
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
					if (GameManager.Get().PlayerInfo != null)
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
						if (GameManager.Get().GameStatus == GameStatus.LoadoutSelecting)
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
						}
						else if (this.m_playerInfo == null)
						{
							flag = false;
						}
						else if (this.m_playerInfo.PlayerId == GameManager.Get().PlayerInfo.PlayerId)
						{
							flag = (GameManager.Get().PlayerInfo.ReadyState != ReadyState.Ready);
						}
						else
						{
							if (GameManager.Get().PlayerInfo.IsGameOwner)
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
								if (!this.m_playerInfo.IsNPCBot)
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
									if (!this.m_playerInfo.IsRemoteControlled)
									{
										goto IL_14A;
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
								return true;
							}
							IL_14A:
							flag = false;
						}
					}
				}
			}
		}
		return flag;
	}

	public void OnPortraitClick(BaseEventData data)
	{
		if (this.IsClickable())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectPlayerPortrait.OnPortraitClick(BaseEventData)).MethodHandle;
			}
			if (this.m_isOutsideGame)
			{
				UICharacterScreen.Get().m_partyListPanel.NotifyOutOfGamePortraitClicked(this, this.m_charType);
			}
			else
			{
				if (this.m_playerInfo != null)
				{
					if (!this.m_playerInfo.IsNPCBot)
					{
						if (!this.m_playerInfo.IsRemoteControlled)
						{
							goto IL_8A;
						}
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
					UICharacterScreen.Get().m_partyListPanel.NotifyBotPortraitClicked(this, this.m_playerInfo);
					return;
				}
				IL_8A:
				UICharacterScreen.Get().m_partyListPanel.NotifyPlayerPortraitClicked(this.m_playerInfo);
			}
		}
	}

	public void OnCogBtnClick(BaseEventData data)
	{
		if (this.IsClickable())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectPlayerPortrait.OnCogBtnClick(BaseEventData)).MethodHandle;
			}
			if (this.m_isOutsideGame)
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
				if (!this.m_isMainCharacter)
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
					bool flag;
					if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent() || AppState_LandingPage.Get() == AppState.GetCurrent())
					{
						flag = AppState_GroupCharacterSelect.Get().IsReady();
					}
					else
					{
						flag = AppState_CharacterSelect.IsReady();
					}
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
						return;
					}
					if (!UICharacterScreen.Get().m_partyListPanel.NotifySwapMainCharacter())
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
						return;
					}
					UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
					CharacterType characterTypeToDisplay = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay;
					UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
					{
						ClientRequestToServerSelectCharacter = new CharacterType?(this.m_charType)
					});
					UICharacterSelectPlayerPortrait[] allyPortraits = UICharacterScreen.Get().m_partyListPanel.m_allyPortraits;
					for (int i = 1; i < allyPortraits.Length; i++)
					{
						if (this == allyPortraits[i])
						{
							ClientGameManager.Get().UpdateRemoteCharacter(characterTypeToDisplay, i - 1, null);
							goto IL_138;
						}
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
				IL_138:;
			}
		}
	}

	public void SetEnabled(bool enabled)
	{
		UIManager.SetGameObjectActive(base.gameObject, enabled, null);
	}

	public void Setup(LobbyPlayerInfo info)
	{
		this.m_playerInfo = info;
		this.m_isOutsideGame = false;
		UIManager.SetGameObjectActive(this.m_cogBtn, false, null);
		if (info != null)
		{
			if (info.CharacterType == CharacterType.None)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectPlayerPortrait.Setup(LobbyPlayerInfo)).MethodHandle;
				}
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(CharacterType.Gremlins);
				this.m_characterImage.sprite = characterResourceLink.GetCharacterSelectIconBW();
			}
			else
			{
				CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(info.CharacterType);
				this.m_characterImage.sprite = characterResourceLink2.GetCharacterSelectIcon();
			}
			UIManager.SetGameObjectActive(this.m_characterImage, true, null);
			if (!info.IsNPCBot)
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
				if (info.IsRemoteControlled)
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
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_readyImage, info.ReadyState == ReadyState.Ready, null);
					UIManager.SetGameObjectActive(this.m_notReadyImage, info.ReadyState != ReadyState.Ready, null);
					foreach (Image component in this.m_stars)
					{
						UIManager.SetGameObjectActive(component, false, null);
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						goto IL_177;
					}
				}
			}
			UIManager.SetGameObjectActive(this.m_readyImage, true, null);
			UIManager.SetGameObjectActive(this.m_notReadyImage, false, null);
			for (int j = 0; j < 5; j++)
			{
				Component component2 = this.m_stars[j];
				bool doActive;
				if (!info.IsRemoteControlled)
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
					doActive = (j <= (int)info.Difficulty);
				}
				else
				{
					doActive = false;
				}
				UIManager.SetGameObjectActive(component2, doActive, null);
			}
			IL_177:
			Component partyLeaderImage = this.m_partyLeaderImage;
			bool doActive2;
			if (info.IsGameOwner)
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
				doActive2 = (GameManager.Get().GameConfig.GameType == GameType.Custom);
			}
			else
			{
				doActive2 = false;
			}
			UIManager.SetGameObjectActive(partyLeaderImage, doActive2, null);
			this.m_playerName.text = info.GetHandle();
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_characterImage, false, null);
			UIManager.SetGameObjectActive(this.m_readyImage, false, null);
			UIManager.SetGameObjectActive(this.m_notReadyImage, false, null);
			UIManager.SetGameObjectActive(this.m_partyLeaderImage, false, null);
			this.m_playerName.text = StringUtil.TR("Empty", "Global");
			foreach (Image component3 in this.m_stars)
			{
				UIManager.SetGameObjectActive(component3, false, null);
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

	public void Setup(CharacterType charType, bool isMainCharacter)
	{
		Component cogBtn = this.m_cogBtn;
		bool doActive;
		if (!isMainCharacter)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectPlayerPortrait.Setup(CharacterType, bool)).MethodHandle;
			}
			doActive = charType.IsValidForHumanGameplay();
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(cogBtn, doActive, null);
		this.m_isMainCharacter = isMainCharacter;
		this.m_isOutsideGame = true;
		this.m_charType = charType;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()) && !(clientGameManager == null))
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
			if (clientGameManager.IsPlayerAccountDataAvailable())
			{
				if (charType != CharacterType.None)
				{
					CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charType);
					this.m_characterImage.sprite = characterResourceLink.GetCharacterSelectIcon();
					UIManager.SetGameObjectActive(this.m_characterImage, true, null);
					this.m_playerName.text = clientGameManager.Handle;
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_characterImage, false, null);
					this.m_playerName.text = StringUtil.TR("Empty", "Global");
				}
				UIManager.SetGameObjectActive(this.m_readyImage, false, null);
				UIManager.SetGameObjectActive(this.m_notReadyImage, false, null);
				UIManager.SetGameObjectActive(this.m_partyLeaderImage, false, null);
				foreach (Image component in this.m_stars)
				{
					UIManager.SetGameObjectActive(component, false, null);
				}
				return;
			}
		}
	}
}
