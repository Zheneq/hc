using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIVoiceListMenu : MonoBehaviour
{
	public RectTransform m_container;

	public _SelectableBtn m_groupBtn;

	public _SelectableBtn m_teamBtn;

	public _SelectableBtn m_groupTeamToggleBtn;

	public Animator m_groupTeamToggleAnimator;

	public Slider m_voiceSlider;

	public TextMeshProUGUI m_voiceSliderText;

	public Slider m_micSlider;

	public TextMeshProUGUI m_micSliderText;

	public _ToggleSwap m_muteBtn;

	public _ToggleSwap m_pushToTalkBtn;

	public _SelectableBtn m_connectBtn;

	public _SelectableBtn m_disconnectBtn;

	public GameObject[] m_disabledStates;

	public RectTransform m_pushToTalkClickBlocker;

	public RectTransform m_playersContainer;

	public UIVoiceListPlayerEntry[] m_playerEntries;

	private bool m_visible;

	private bool m_initializedOptions;

	private bool m_canConnect;

	private float m_voiceVolume;

	private float m_micVolume;

	private SettingsState.VoiceChatMode m_chatMode;

	private void Start()
	{
		UIManager.SetGameObjectActive(m_container, false);
		m_visible = false;
		UIManager.SetGameObjectActive(m_playersContainer, false);
		UIManager.SetGameObjectActive(m_disconnectBtn, false);
		UIManager.SetGameObjectActive(m_connectBtn, false);
		UIManager.SetGameObjectActive(m_pushToTalkClickBlocker, false);
		DiscordClientInterface discordClientInterface = DiscordClientInterface.Get();
		discordClientInterface.OnJoined = (Action)Delegate.Combine(discordClientInterface.OnJoined, new Action(OnJoined));
		DiscordClientInterface discordClientInterface2 = DiscordClientInterface.Get();
		discordClientInterface2.OnDisconnected = (Action)Delegate.Combine(discordClientInterface2.OnDisconnected, new Action(OnDisconnected));
		DiscordClientInterface discordClientInterface3 = DiscordClientInterface.Get();
		discordClientInterface3.OnUserJoined = (Action<DiscordUserInfo>)Delegate.Combine(discordClientInterface3.OnUserJoined, new Action<DiscordUserInfo>(OnUserJoined));
		DiscordClientInterface discordClientInterface4 = DiscordClientInterface.Get();
		discordClientInterface4.OnUserLeft = (Action<DiscordUserInfo>)Delegate.Combine(discordClientInterface4.OnUserLeft, new Action<DiscordUserInfo>(OnUserLeft));
		DiscordClientInterface discordClientInterface5 = DiscordClientInterface.Get();
		discordClientInterface5.OnUserSpeakingChanged = (Action<DiscordUserInfo>)Delegate.Combine(discordClientInterface5.OnUserSpeakingChanged, new Action<DiscordUserInfo>(OnUserSpeakingChanged));
		int num;
		if (DiscordClientInterface.Get().IsConnected)
		{
			num = ((DiscordClientInterface.Get().ChannelInfo != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		if (num != 0)
		{
			OnJoined();
			RefreshPlayersList();
		}
		else
		{
			OnDisconnected();
		}
		_ButtonSwapSprite spriteController = m_connectBtn.spriteController;
		
		spriteController.callback = delegate
			{
				if (!DiscordClientInterface.CanJoinTeamChat)
				{
					if (!DiscordClientInterface.CanJoinGroupChat)
					{
						TextConsole.Get().Write("Failed to join Discord chat. You are not in a team or group.");
						UIFrontEnd.PlaySound(FrontEndButtonSounds.Cancel);
						return;
					}
				}
				ClientGameManager.Get().JoinDiscord();
				UIFrontEnd.PlaySound(FrontEndButtonSounds.Generic);
			};
		m_disconnectBtn.spriteController.callback = delegate
		{
			ClientGameManager.Get().LeaveDiscord();
			UIFrontEnd.PlaySound(FrontEndButtonSounds.Close);
		};
		InitializeOptions();
	}

	private void OnDestroy()
	{
		if (!(DiscordClientInterface.Get() != null))
		{
			return;
		}
		while (true)
		{
			DiscordClientInterface discordClientInterface = DiscordClientInterface.Get();
			discordClientInterface.OnJoined = (Action)Delegate.Remove(discordClientInterface.OnJoined, new Action(OnJoined));
			DiscordClientInterface discordClientInterface2 = DiscordClientInterface.Get();
			discordClientInterface2.OnLeft = (Action)Delegate.Remove(discordClientInterface2.OnLeft, new Action(OnDisconnected));
			return;
		}
	}

	private void InitializeOptions()
	{
		if (m_initializedOptions)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		Options_UI optionsUI = Options_UI.Get();
		if (optionsUI == null)
		{
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
		m_pushToTalkBtn.SetOn(optionsUI.GetVoicePushToTalk());
		m_pushToTalkBtn.changedNotify = delegate(_ToggleSwap btn)
		{
			bool flag2 = btn.IsChecked();
			optionsUI.SetVoicePushToTalk(flag2);
			int value3;
			if (flag2)
			{
				value3 = 1;
			}
			else
			{
				value3 = 0;
			}
			PlayerPrefs.SetInt("VoicePushToTalk", value3);
			if (DiscordClientInterface.Get() != null)
			{
				DiscordClientInterface.Get().RefreshSettings();
			}
			if (flag2)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (DiscordClientInterface.Get().ScanPushToTalkKey(true, delegate
						{
							UIManager.SetGameObjectActive(m_pushToTalkClickBlocker, false);
						}))
						{
							UIManager.SetGameObjectActive(m_pushToTalkClickBlocker, true);
						}
						return;
					}
				}
			}
		};
		m_muteBtn.SetOn(optionsUI.GetVoiceMute());
		m_muteBtn.changedNotify = delegate(_ToggleSwap btn)
		{
			bool flag = btn.IsChecked();
			optionsUI.SetVoicePushToTalk(flag);
			int value2;
			if (flag)
			{
				value2 = 1;
			}
			else
			{
				value2 = 0;
			}
			PlayerPrefs.SetInt("VoiceMute", value2);
			if (DiscordClientInterface.Get() != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						DiscordClientInterface.Get().RefreshSettings();
						return;
					}
				}
			}
		};
		m_voiceSlider.minValue = 0f;
		m_voiceSlider.maxValue = 100f;
		m_voiceSlider.onValueChanged.AddListener(delegate(float value)
		{
			m_voiceSliderText.text = Mathf.CeilToInt(value).ToString();
			if (m_voiceVolume == value)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			m_voiceVolume = value;
			PlayerPrefs.SetFloat("VoiceVolume", m_voiceVolume);
		});
		m_voiceSlider.value = optionsUI.GetVoiceVolume();
		GameObject gameObject = m_voiceSlider.gameObject;
		
		UIEventTriggerUtils.AddListener(gameObject, EventTriggerType.PointerUp, delegate
			{
				if (DiscordClientInterface.Get() != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							DiscordClientInterface.Get().RefreshSettings();
							return;
						}
					}
				}
			});
		m_micSlider.minValue = 0f;
		m_micSlider.maxValue = 100f;
		m_micSlider.onValueChanged.AddListener(delegate(float value)
		{
			m_micSliderText.text = Mathf.CeilToInt(value).ToString();
			if (m_micVolume == value)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			m_micVolume = value;
			PlayerPrefs.SetFloat("MicVolume", m_micVolume);
		});
		m_micSlider.value = optionsUI.GetMicVolume();
		GameObject gameObject2 = m_micSlider.gameObject;
		
		UIEventTriggerUtils.AddListener(gameObject2, EventTriggerType.PointerUp, delegate
			{
				if (DiscordClientInterface.Get() != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							DiscordClientInterface.Get().RefreshSettings();
							return;
						}
					}
				}
			});
		m_groupBtn.spriteController.callback = delegate
		{
			ChangeChatMode(SettingsState.VoiceChatMode.Group);
		};
		m_teamBtn.spriteController.callback = delegate
		{
			ChangeChatMode(SettingsState.VoiceChatMode.Team);
		};
		m_groupTeamToggleBtn.spriteController.callback = delegate
		{
			if (m_chatMode == SettingsState.VoiceChatMode.Team)
			{
				ChangeChatMode(SettingsState.VoiceChatMode.Group);
			}
			else
			{
				ChangeChatMode(SettingsState.VoiceChatMode.Team);
			}
		};
		m_initializedOptions = true;
	}

	private void SetSettingsDisabled(bool disabled)
	{
		for (int i = 0; i < m_disabledStates.Length; i++)
		{
			UIManager.SetGameObjectActive(m_disabledStates[i], disabled);
		}
	}

	private void ChangeChatMode(SettingsState.VoiceChatMode mode, bool update = true)
	{
		if (update)
		{
			if (m_chatMode != mode)
			{
				UIAnimationEventManager uIAnimationEventManager = UIAnimationEventManager.Get();
				Animator groupTeamToggleAnimator = m_groupTeamToggleAnimator;
				object str;
				if (mode == SettingsState.VoiceChatMode.Group)
				{
					str = "Left";
				}
				else
				{
					str = "Right";
				}
				uIAnimationEventManager.PlayAnimation(groupTeamToggleAnimator, "SwitchHorizontal" + (string)str + "DefaultIN", null, string.Empty);
				Options_UI.Get().SetGameModeVoiceChat(mode);
				PlayerPrefs.SetInt("OptionsGameModeVoiceChat", (int)mode);
			}
		}
		else
		{
			UIAnimationEventManager uIAnimationEventManager2 = UIAnimationEventManager.Get();
			Animator groupTeamToggleAnimator2 = m_groupTeamToggleAnimator;
			object str2;
			if (mode == SettingsState.VoiceChatMode.Group)
			{
				str2 = "Left";
			}
			else
			{
				str2 = "Right";
			}
			uIAnimationEventManager2.PlayAnimation(groupTeamToggleAnimator2, "SwitchHorizontal" + (string)str2 + "DefaultIDLE", null, string.Empty);
		}
		m_chatMode = mode;
	}

	public void SetVisible(bool visible)
	{
		if (m_visible == visible)
		{
			return;
		}
		m_visible = visible;
		UIManager.SetGameObjectActive(m_container, m_visible);
		if (m_visible)
		{
			InitializeOptions();
			ChangeChatMode(Options_UI.Get().GetGameModeVoiceChat(), false);
		}
		int sound;
		if (m_visible)
		{
			sound = 25;
		}
		else
		{
			sound = 28;
		}
		UIFrontEnd.PlaySound((FrontEndButtonSounds)sound);
	}

	public bool IsVisible()
	{
		return m_visible;
	}

	private void RefreshPlayersList()
	{
		for (int i = 0; i < m_playerEntries.Length; i++)
		{
			if (i < DiscordClientInterface.Get().ChannelUsers.Count)
			{
				UIManager.SetGameObjectActive(m_playerEntries[i], true);
				m_playerEntries[i].Setup(DiscordClientInterface.Get().ChannelUsers[i]);
			}
			else
			{
				UIManager.SetGameObjectActive(m_playerEntries[i], false);
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

	private void OnJoined()
	{
		m_canConnect = true;
		UIManager.SetGameObjectActive(m_connectBtn, false);
		UIManager.SetGameObjectActive(m_disconnectBtn, true);
		SetSettingsDisabled(false);
		RefreshPlayersList();
		UIManager.SetGameObjectActive(m_playersContainer, true);
	}

	private void OnDisconnected()
	{
		UIManager.SetGameObjectActive(m_connectBtn, m_canConnect);
		UIManager.SetGameObjectActive(m_disconnectBtn, false);
		SetSettingsDisabled(true);
		for (int i = 0; i < m_playerEntries.Length; i++)
		{
			UIManager.SetGameObjectActive(m_playerEntries[i], false);
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_playersContainer, false);
			RefreshPlayersList();
			return;
		}
	}

	private void OnUserJoined(DiscordUserInfo userThatLeft)
	{
		int num = DiscordClientInterface.Get().ChannelUsers.Count - 1;
		UIManager.SetGameObjectActive(m_playerEntries[num], true);
		m_playerEntries[num].Setup(userThatLeft);
	}

	private void OnUserLeft(DiscordUserInfo userThatLeft)
	{
		RefreshPlayersList();
	}

	private void OnUserSpeakingChanged(DiscordUserInfo userInfo)
	{
		for (int i = 0; i < m_playerEntries.Length; i++)
		{
			if (m_playerEntries[i].IsUser(userInfo))
			{
				m_playerEntries[i].SetSpeaking(userInfo.IsSpeaking);
				return;
			}
		}
		while (true)
		{
			return;
		}
	}

	private void Update()
	{
		bool flag;
		UIVoiceListMenu componentInParent;
		bool flag2;
		if (Input.GetMouseButtonDown(0))
		{
			flag = true;
			if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
			{
				StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
				if (component != null)
				{
					if (component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
					{
						componentInParent = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<UIVoiceListMenu>();
						flag2 = false;
						if (componentInParent == null)
						{
							_SelectableBtn componentInParent2 = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<_SelectableBtn>();
							if (UIFrontEnd.Get() != null)
							{
								if (!(componentInParent2 == UIFrontEnd.Get().m_frontEndNavPanel.m_microphoneConnectedBtn))
								{
									if (!(componentInParent2 == UIFrontEnd.Get().m_frontEndNavPanel.m_microphoneOfflineBtn))
									{
										goto IL_014f;
									}
								}
								flag2 = true;
							}
						}
						goto IL_014f;
					}
				}
			}
			goto IL_0169;
		}
		goto IL_017d;
		IL_0169:
		if (flag)
		{
			SetVisible(false);
		}
		goto IL_017d;
		IL_014f:
		if (!(componentInParent != null))
		{
			if (!flag2)
			{
				goto IL_0169;
			}
		}
		flag = false;
		goto IL_0169;
		IL_01d8:
		if (!m_canConnect)
		{
			return;
		}
		while (true)
		{
			if (DiscordClientInterface.CanJoinGroupChat)
			{
				return;
			}
			while (true)
			{
				if (!DiscordClientInterface.CanJoinTeamChat)
				{
					while (true)
					{
						m_canConnect = false;
						UIManager.SetGameObjectActive(m_connectBtn, false);
						UIManager.SetGameObjectActive(m_disconnectBtn, false);
						return;
					}
				}
				return;
			}
		}
		IL_017d:
		if (!m_canConnect)
		{
			if (!DiscordClientInterface.CanJoinGroupChat)
			{
				if (!DiscordClientInterface.CanJoinTeamChat)
				{
					goto IL_01d8;
				}
			}
			m_canConnect = true;
			UIManager.SetGameObjectActive(m_connectBtn, true);
			UIManager.SetGameObjectActive(m_disconnectBtn, false);
			return;
		}
		goto IL_01d8;
	}
}
