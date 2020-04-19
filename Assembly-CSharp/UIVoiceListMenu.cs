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
		UIManager.SetGameObjectActive(this.m_container, false, null);
		this.m_visible = false;
		UIManager.SetGameObjectActive(this.m_playersContainer, false, null);
		UIManager.SetGameObjectActive(this.m_disconnectBtn, false, null);
		UIManager.SetGameObjectActive(this.m_connectBtn, false, null);
		UIManager.SetGameObjectActive(this.m_pushToTalkClickBlocker, false, null);
		DiscordClientInterface discordClientInterface = DiscordClientInterface.Get();
		discordClientInterface.OnJoined = (Action)Delegate.Combine(discordClientInterface.OnJoined, new Action(this.OnJoined));
		DiscordClientInterface discordClientInterface2 = DiscordClientInterface.Get();
		discordClientInterface2.OnDisconnected = (Action)Delegate.Combine(discordClientInterface2.OnDisconnected, new Action(this.OnDisconnected));
		DiscordClientInterface discordClientInterface3 = DiscordClientInterface.Get();
		discordClientInterface3.OnUserJoined = (Action<DiscordUserInfo>)Delegate.Combine(discordClientInterface3.OnUserJoined, new Action<DiscordUserInfo>(this.OnUserJoined));
		DiscordClientInterface discordClientInterface4 = DiscordClientInterface.Get();
		discordClientInterface4.OnUserLeft = (Action<DiscordUserInfo>)Delegate.Combine(discordClientInterface4.OnUserLeft, new Action<DiscordUserInfo>(this.OnUserLeft));
		DiscordClientInterface discordClientInterface5 = DiscordClientInterface.Get();
		discordClientInterface5.OnUserSpeakingChanged = (Action<DiscordUserInfo>)Delegate.Combine(discordClientInterface5.OnUserSpeakingChanged, new Action<DiscordUserInfo>(this.OnUserSpeakingChanged));
		bool flag;
		if (DiscordClientInterface.Get().IsConnected)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIVoiceListMenu.Start()).MethodHandle;
			}
			flag = (DiscordClientInterface.Get().ChannelInfo != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (flag2)
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
			this.OnJoined();
			this.RefreshPlayersList();
		}
		else
		{
			this.OnDisconnected();
		}
		_ButtonSwapSprite spriteController = this.m_connectBtn.spriteController;
		if (UIVoiceListMenu.<>f__am$cache0 == null)
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
			UIVoiceListMenu.<>f__am$cache0 = delegate(BaseEventData x)
			{
				if (!DiscordClientInterface.CanJoinTeamChat)
				{
					if (!DiscordClientInterface.CanJoinGroupChat)
					{
						TextConsole.Get().Write("Failed to join Discord chat. You are not in a team or group.", ConsoleMessageType.SystemMessage);
						UIFrontEnd.PlaySound(FrontEndButtonSounds.Cancel);
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIVoiceListMenu.<Start>m__0(BaseEventData)).MethodHandle;
					}
				}
				ClientGameManager.Get().JoinDiscord();
				UIFrontEnd.PlaySound(FrontEndButtonSounds.Generic);
			};
		}
		spriteController.callback = UIVoiceListMenu.<>f__am$cache0;
		this.m_disconnectBtn.spriteController.callback = delegate(BaseEventData x)
		{
			ClientGameManager.Get().LeaveDiscord();
			UIFrontEnd.PlaySound(FrontEndButtonSounds.Close);
		};
		this.InitializeOptions();
	}

	private void OnDestroy()
	{
		if (DiscordClientInterface.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIVoiceListMenu.OnDestroy()).MethodHandle;
			}
			DiscordClientInterface discordClientInterface = DiscordClientInterface.Get();
			discordClientInterface.OnJoined = (Action)Delegate.Remove(discordClientInterface.OnJoined, new Action(this.OnJoined));
			DiscordClientInterface discordClientInterface2 = DiscordClientInterface.Get();
			discordClientInterface2.OnLeft = (Action)Delegate.Remove(discordClientInterface2.OnLeft, new Action(this.OnDisconnected));
		}
	}

	private void InitializeOptions()
	{
		if (this.m_initializedOptions)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIVoiceListMenu.InitializeOptions()).MethodHandle;
			}
			return;
		}
		Options_UI optionsUI = Options_UI.Get();
		if (optionsUI == null)
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
			return;
		}
		this.m_pushToTalkBtn.SetOn(optionsUI.GetVoicePushToTalk(), false);
		this.m_pushToTalkBtn.changedNotify = delegate(_ToggleSwap btn)
		{
			bool flag = btn.IsChecked();
			optionsUI.SetVoicePushToTalk(flag);
			string key = "VoicePushToTalk";
			int value;
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIVoiceListMenu.<InitializeOptions>c__AnonStorey0.<>m__0(_ToggleSwap)).MethodHandle;
				}
				value = 1;
			}
			else
			{
				value = 0;
			}
			PlayerPrefs.SetInt(key, value);
			if (DiscordClientInterface.Get() != null)
			{
				DiscordClientInterface.Get().RefreshSettings();
			}
			if (flag)
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
				if (DiscordClientInterface.Get().ScanPushToTalkKey(true, delegate(int type, int code, string name)
				{
					UIManager.SetGameObjectActive(this.m_pushToTalkClickBlocker, false, null);
				}))
				{
					UIManager.SetGameObjectActive(this.m_pushToTalkClickBlocker, true, null);
				}
			}
		};
		this.m_muteBtn.SetOn(optionsUI.GetVoiceMute(), false);
		this.m_muteBtn.changedNotify = delegate(_ToggleSwap btn)
		{
			bool flag = btn.IsChecked();
			optionsUI.SetVoicePushToTalk(flag);
			string key = "VoiceMute";
			int value;
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIVoiceListMenu.<InitializeOptions>c__AnonStorey0.<>m__1(_ToggleSwap)).MethodHandle;
				}
				value = 1;
			}
			else
			{
				value = 0;
			}
			PlayerPrefs.SetInt(key, value);
			if (DiscordClientInterface.Get() != null)
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
				DiscordClientInterface.Get().RefreshSettings();
			}
		};
		this.m_voiceSlider.minValue = 0f;
		this.m_voiceSlider.maxValue = 100f;
		this.m_voiceSlider.onValueChanged.AddListener(delegate(float value)
		{
			this.m_voiceSliderText.text = Mathf.CeilToInt(value).ToString();
			if (this.m_voiceVolume == value)
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
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIVoiceListMenu.<InitializeOptions>c__AnonStorey0.<>m__2(float)).MethodHandle;
				}
				return;
			}
			this.m_voiceVolume = value;
			PlayerPrefs.SetFloat("VoiceVolume", this.m_voiceVolume);
		});
		this.m_voiceSlider.value = optionsUI.GetVoiceVolume();
		GameObject gameObject = this.m_voiceSlider.gameObject;
		EventTriggerType triggerType = EventTriggerType.PointerUp;
		if (UIVoiceListMenu.<>f__am$cache2 == null)
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
			UIVoiceListMenu.<>f__am$cache2 = delegate(BaseEventData x)
			{
				if (DiscordClientInterface.Get() != null)
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
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIVoiceListMenu.<InitializeOptions>m__2(BaseEventData)).MethodHandle;
					}
					DiscordClientInterface.Get().RefreshSettings();
				}
			};
		}
		UIEventTriggerUtils.AddListener(gameObject, triggerType, UIVoiceListMenu.<>f__am$cache2);
		this.m_micSlider.minValue = 0f;
		this.m_micSlider.maxValue = 100f;
		this.m_micSlider.onValueChanged.AddListener(delegate(float value)
		{
			this.m_micSliderText.text = Mathf.CeilToInt(value).ToString();
			if (this.m_micVolume == value)
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
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIVoiceListMenu.<InitializeOptions>c__AnonStorey0.<>m__3(float)).MethodHandle;
				}
				return;
			}
			this.m_micVolume = value;
			PlayerPrefs.SetFloat("MicVolume", this.m_micVolume);
		});
		this.m_micSlider.value = optionsUI.GetMicVolume();
		GameObject gameObject2 = this.m_micSlider.gameObject;
		EventTriggerType triggerType2 = EventTriggerType.PointerUp;
		if (UIVoiceListMenu.<>f__am$cache3 == null)
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
			UIVoiceListMenu.<>f__am$cache3 = delegate(BaseEventData x)
			{
				if (DiscordClientInterface.Get() != null)
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
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIVoiceListMenu.<InitializeOptions>m__3(BaseEventData)).MethodHandle;
					}
					DiscordClientInterface.Get().RefreshSettings();
				}
			};
		}
		UIEventTriggerUtils.AddListener(gameObject2, triggerType2, UIVoiceListMenu.<>f__am$cache3);
		this.m_groupBtn.spriteController.callback = delegate(BaseEventData x)
		{
			this.ChangeChatMode(SettingsState.VoiceChatMode.Group, true);
		};
		this.m_teamBtn.spriteController.callback = delegate(BaseEventData x)
		{
			this.ChangeChatMode(SettingsState.VoiceChatMode.Team, true);
		};
		this.m_groupTeamToggleBtn.spriteController.callback = delegate(BaseEventData x)
		{
			if (this.m_chatMode == SettingsState.VoiceChatMode.Team)
			{
				this.ChangeChatMode(SettingsState.VoiceChatMode.Group, true);
			}
			else
			{
				this.ChangeChatMode(SettingsState.VoiceChatMode.Team, true);
			}
		};
		this.m_initializedOptions = true;
	}

	private void SetSettingsDisabled(bool disabled)
	{
		for (int i = 0; i < this.m_disabledStates.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_disabledStates[i], disabled, null);
		}
	}

	private void ChangeChatMode(SettingsState.VoiceChatMode mode, bool update = true)
	{
		if (update)
		{
			if (this.m_chatMode != mode)
			{
				UIAnimationEventManager uianimationEventManager = UIAnimationEventManager.Get();
				Animator groupTeamToggleAnimator = this.m_groupTeamToggleAnimator;
				string str = "SwitchHorizontal";
				string str2;
				if (mode == SettingsState.VoiceChatMode.Group)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIVoiceListMenu.ChangeChatMode(SettingsState.VoiceChatMode, bool)).MethodHandle;
					}
					str2 = "Left";
				}
				else
				{
					str2 = "Right";
				}
				uianimationEventManager.PlayAnimation(groupTeamToggleAnimator, str + str2 + "DefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
				Options_UI.Get().SetGameModeVoiceChat(mode);
				PlayerPrefs.SetInt("OptionsGameModeVoiceChat", (int)mode);
			}
		}
		else
		{
			UIAnimationEventManager uianimationEventManager2 = UIAnimationEventManager.Get();
			Animator groupTeamToggleAnimator2 = this.m_groupTeamToggleAnimator;
			string str3 = "SwitchHorizontal";
			string str4;
			if (mode == SettingsState.VoiceChatMode.Group)
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
				str4 = "Left";
			}
			else
			{
				str4 = "Right";
			}
			uianimationEventManager2.PlayAnimation(groupTeamToggleAnimator2, str3 + str4 + "DefaultIDLE", null, string.Empty, 0, 0f, true, false, null, null);
		}
		this.m_chatMode = mode;
	}

	public void SetVisible(bool visible)
	{
		if (this.m_visible == visible)
		{
			return;
		}
		this.m_visible = visible;
		UIManager.SetGameObjectActive(this.m_container, this.m_visible, null);
		if (this.m_visible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIVoiceListMenu.SetVisible(bool)).MethodHandle;
			}
			this.InitializeOptions();
			this.ChangeChatMode(Options_UI.Get().GetGameModeVoiceChat(), false);
		}
		FrontEndButtonSounds sound;
		if (this.m_visible)
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
			sound = FrontEndButtonSounds.MenuOpen;
		}
		else
		{
			sound = FrontEndButtonSounds.Close;
		}
		UIFrontEnd.PlaySound(sound);
	}

	public bool IsVisible()
	{
		return this.m_visible;
	}

	private void RefreshPlayersList()
	{
		for (int i = 0; i < this.m_playerEntries.Length; i++)
		{
			if (i < DiscordClientInterface.Get().ChannelUsers.Count)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIVoiceListMenu.RefreshPlayersList()).MethodHandle;
				}
				UIManager.SetGameObjectActive(this.m_playerEntries[i], true, null);
				this.m_playerEntries[i].Setup(DiscordClientInterface.Get().ChannelUsers[i]);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_playerEntries[i], false, null);
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

	private void OnJoined()
	{
		this.m_canConnect = true;
		UIManager.SetGameObjectActive(this.m_connectBtn, false, null);
		UIManager.SetGameObjectActive(this.m_disconnectBtn, true, null);
		this.SetSettingsDisabled(false);
		this.RefreshPlayersList();
		UIManager.SetGameObjectActive(this.m_playersContainer, true, null);
	}

	private void OnDisconnected()
	{
		UIManager.SetGameObjectActive(this.m_connectBtn, this.m_canConnect, null);
		UIManager.SetGameObjectActive(this.m_disconnectBtn, false, null);
		this.SetSettingsDisabled(true);
		for (int i = 0; i < this.m_playerEntries.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_playerEntries[i], false, null);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIVoiceListMenu.OnDisconnected()).MethodHandle;
		}
		UIManager.SetGameObjectActive(this.m_playersContainer, false, null);
		this.RefreshPlayersList();
	}

	private void OnUserJoined(DiscordUserInfo userThatLeft)
	{
		int num = DiscordClientInterface.Get().ChannelUsers.Count - 1;
		UIManager.SetGameObjectActive(this.m_playerEntries[num], true, null);
		this.m_playerEntries[num].Setup(userThatLeft);
	}

	private void OnUserLeft(DiscordUserInfo userThatLeft)
	{
		this.RefreshPlayersList();
	}

	private void OnUserSpeakingChanged(DiscordUserInfo userInfo)
	{
		for (int i = 0; i < this.m_playerEntries.Length; i++)
		{
			if (this.m_playerEntries[i].IsUser(userInfo))
			{
				this.m_playerEntries[i].SetSpeaking(userInfo.IsSpeaking);
				return;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIVoiceListMenu.OnUserSpeakingChanged(DiscordUserInfo)).MethodHandle;
			return;
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIVoiceListMenu.Update()).MethodHandle;
			}
			bool flag = true;
			if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
			{
				StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
				if (component != null)
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
					if (component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
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
						UIVoiceListMenu componentInParent = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<UIVoiceListMenu>();
						bool flag2 = false;
						if (componentInParent == null)
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
							_SelectableBtn componentInParent2 = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<_SelectableBtn>();
							if (UIFrontEnd.Get() != null)
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
								if (!(componentInParent2 == UIFrontEnd.Get().m_frontEndNavPanel.m_microphoneConnectedBtn))
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
									if (!(componentInParent2 == UIFrontEnd.Get().m_frontEndNavPanel.m_microphoneOfflineBtn))
									{
										goto IL_14F;
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
								flag2 = true;
							}
						}
						IL_14F:
						if (!(componentInParent != null))
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
							if (!flag2)
							{
								goto IL_169;
							}
						}
						flag = false;
					}
				}
			}
			IL_169:
			if (flag)
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
				this.SetVisible(false);
			}
		}
		if (!this.m_canConnect)
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
			if (!DiscordClientInterface.CanJoinGroupChat)
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
				if (!DiscordClientInterface.CanJoinTeamChat)
				{
					goto IL_1D8;
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
			this.m_canConnect = true;
			UIManager.SetGameObjectActive(this.m_connectBtn, true, null);
			UIManager.SetGameObjectActive(this.m_disconnectBtn, false, null);
			return;
		}
		IL_1D8:
		if (this.m_canConnect)
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
			if (!DiscordClientInterface.CanJoinGroupChat)
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
				if (!DiscordClientInterface.CanJoinTeamChat)
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
					this.m_canConnect = false;
					UIManager.SetGameObjectActive(this.m_connectBtn, false, null);
					UIManager.SetGameObjectActive(this.m_disconnectBtn, false, null);
				}
			}
		}
	}
}
