using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISideNotifications : MonoBehaviour
{
	[Header("-- Gameplay Notifications --")]
	public UINotificationDisplay m_displayPrefab;

	public GridLayoutGroup m_grid;

	public float m_timeToDisplayNotification = 3f;

	[Header("-- Character Profiles --")]
	public TextMeshProUGUI m_characterProfileInstructions;

	public UISideCharacterProfile m_characterProfilePrefab;

	public GridLayoutGroup m_characterProfileGrid;

	[Header("-- GG Pack Notifications --")]
	public GridLayoutGroup m_ggPackGrid;

	public Animator m_ggPackAnimator;

	public UIGGPackNotification m_ggPackDisplayPrefab;

	public TextMeshProUGUI[] m_bonusLabel;

	public Animator m_PercentageAnimator;

	private List<UISideNotifications.NotificationDisplay> activeDisplays;

	private List<UINotificationDisplay> inActiveDisplays;

	private List<UISideCharacterProfile> m_characterProfiles;

	private Team m_currentTeam;

	private List<UIGGPackNotification> activeGGPackDisplays;

	private Dictionary<string, int> m_ggPackUsers = new Dictionary<string, int>();

	private bool m_ggPackNotificationActive;

	private int m_totalNumGGPacksUsed;

	private int m_numSelfGGPackUsed;

	private void Start()
	{
		float d = (this.m_grid.transform as RectTransform).rect.width / this.m_grid.cellSize.x;
		this.m_grid.cellSize *= d;
		this.activeDisplays = new List<UISideNotifications.NotificationDisplay>();
		this.inActiveDisplays = new List<UINotificationDisplay>();
		this.activeGGPackDisplays = new List<UIGGPackNotification>();
		this.m_ggPackUsers = new Dictionary<string, int>();
		this.m_characterProfiles = new List<UISideCharacterProfile>();
		ClientGameManager.Get().OnUseGGPackNotification += this.NotifyOtherUsedGGPack;
		GameFlowData.s_onGameStateChanged += delegate(GameState gameState)
		{
			if (gameState == GameState.StartingGame)
			{
				this.SetupSideProfiles(GameManager.Get().PlayerInfo.TeamId);
			}
		};
		UIManager.SetGameObjectActive(this.m_characterProfileGrid, false, null);
	}

	public void RemoveHandleMessage()
	{
		ClientGameManager.Get().OnUseGGPackNotification -= this.NotifyOtherUsedGGPack;
	}

	public void SetBonusGGPackText(string text)
	{
		for (int i = 0; i < this.m_bonusLabel.Length; i++)
		{
			this.m_bonusLabel[i].text = text;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UISideNotifications.SetBonusGGPackText(string)).MethodHandle;
		}
	}

	private void Update()
	{
		GameFlowData gameFlowData = GameFlowData.Get();
		for (int i = 0; i < this.activeDisplays.Count; i++)
		{
			UISideNotifications.NotificationDisplay notificationDisplay = this.activeDisplays[i];
			float num = this.m_timeToDisplayNotification - (Time.time - notificationDisplay.timeActiveDisplayed);
			if (num <= 0f)
			{
				this.activeDisplays.RemoveAt(i);
				UIManager.SetGameObjectActive(notificationDisplay.theDisplay, false, null);
				notificationDisplay.theDisplay.transform.SetParent(this.m_grid.transform.parent);
				this.inActiveDisplays.Add(notificationDisplay.theDisplay);
			}
			else if (num < 1f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UISideNotifications.Update()).MethodHandle;
				}
				notificationDisplay.theDisplay.SetAlpha(num);
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
		bool flag = true;
		for (int j = 0; j < this.activeGGPackDisplays.Count; j++)
		{
			if (!this.activeGGPackDisplays[j].DoneAnimating)
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
				IL_123:
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
					this.m_ggPackNotificationActive = false;
					if (this.m_ggPackAnimator.enabled)
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
						if (this.m_ggPackAnimator.gameObject.activeInHierarchy)
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
							AnimatorClipInfo[] currentAnimatorClipInfo = this.m_ggPackAnimator.GetCurrentAnimatorClipInfo(0);
							if (currentAnimatorClipInfo != null)
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
								if (currentAnimatorClipInfo.Length > 0)
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
									if (currentAnimatorClipInfo[0].clip.name != "ggPackListOUT")
									{
										this.m_ggPackAnimator.Play("ggPackListOUT");
									}
								}
							}
						}
					}
				}
				if (gameFlowData != null)
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
					if (gameFlowData.activeOwnedActorData != null)
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
						if (gameFlowData.activeOwnedActorData.\u000E() != this.m_currentTeam)
						{
							this.SetupSideProfiles(gameFlowData.activeOwnedActorData.\u000E());
						}
					}
				}
				return;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			goto IL_123;
		}
	}

	public int NumberGGPacksUsed(string username)
	{
		if (this.m_ggPackUsers.ContainsKey(username))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideNotifications.NumberGGPacksUsed(string)).MethodHandle;
			}
			return this.m_ggPackUsers[username];
		}
		return 0;
	}

	public void GGPackAnimListOutAnimDone()
	{
		UIManager.SetGameObjectActive(this.m_ggPackAnimator, false, null);
		if (this.activeGGPackDisplays != null)
		{
			for (int i = 0; i < this.activeGGPackDisplays.Count; i++)
			{
				UnityEngine.Object.Destroy(this.activeGGPackDisplays[i].gameObject);
			}
			this.activeGGPackDisplays.Clear();
		}
	}

	public int NumSelfGGPacksUsed()
	{
		return this.m_numSelfGGPackUsed;
	}

	private void GGPackNotificationHelper(UISideNotifications.UIGGPackNotificationInfo info)
	{
		float ggpackXPMultiplier = GameBalanceVars.Get().GetGGPackXPMultiplier(this.m_totalNumGGPacksUsed, this.m_numSelfGGPackUsed);
		if (!this.m_ggPackNotificationActive)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideNotifications.GGPackNotificationHelper(UISideNotifications.UIGGPackNotificationInfo)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_ggPackAnimator, true, null);
			this.m_ggPackAnimator.Play("ggPackListIN");
			this.m_ggPackNotificationActive = true;
		}
		else
		{
			this.m_ggPackAnimator.Play("ggPackListINCREASE", -1, 0f);
		}
		if (info.GGPackUserName == ClientGameManager.Get().SessionInfo.Handle)
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
			this.m_numSelfGGPackUsed++;
		}
		if (this.m_ggPackUsers.ContainsKey(info.GGPackUserName))
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
			Dictionary<string, int> ggPackUsers;
			string ggpackUserName;
			(ggPackUsers = this.m_ggPackUsers)[ggpackUserName = info.GGPackUserName] = ggPackUsers[ggpackUserName] + 1;
		}
		else
		{
			this.m_ggPackUsers[info.GGPackUserName] = 1;
		}
		this.m_totalNumGGPacksUsed++;
		int ggpackBonusISO = GameBalanceVars.Get().GetGGPackBonusISO(this.m_totalNumGGPacksUsed, this.m_numSelfGGPackUsed);
		float ggpackXPMultiplier2 = GameBalanceVars.Get().GetGGPackXPMultiplier(this.m_totalNumGGPacksUsed, this.m_numSelfGGPackUsed);
		UIGameOverScreen.Get().UpdateEndGameGGBonuses(ggpackBonusISO, ggpackXPMultiplier2);
		UIGGPackNotification uiggpackNotification = UnityEngine.Object.Instantiate<UIGGPackNotification>(this.m_ggPackDisplayPrefab);
		uiggpackNotification.transform.SetParent(this.m_ggPackGrid.transform);
		uiggpackNotification.transform.localPosition = Vector3.zero;
		uiggpackNotification.transform.localEulerAngles = Vector3.zero;
		uiggpackNotification.transform.localScale = Vector3.one;
		uiggpackNotification.transform.SetAsFirstSibling();
		uiggpackNotification.Setup(info);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GGPackUsedNotification);
		this.activeGGPackDisplays.Add(uiggpackNotification);
		this.SetBonusGGPackText(string.Format(StringUtil.TR("InGameGGBonuses", "HUD"), Mathf.RoundToInt((ggpackXPMultiplier2 - 1f) * 100f)));
		if (ggpackXPMultiplier2 < 2f)
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
			UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoostBluePercentageDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
		}
		else if (ggpackXPMultiplier2 < 3f)
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
			UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoostSilverPercentageDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
			if (ggpackXPMultiplier < 2f)
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
				UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoost100PercentageDefaultIN", null, string.Empty, 1, 0f, true, false, null, null);
			}
			else
			{
				UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoost100PercentageDefaultIDLE", null, string.Empty, 1, 0f, true, false, null, null);
			}
		}
		else if (ggpackXPMultiplier2 < 4f)
		{
			UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoostGoldPercentageDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
			if (ggpackXPMultiplier < 3f)
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
				UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoost200PercentageDefaultIN", null, string.Empty, 1, 0f, true, false, null, null);
			}
			else
			{
				UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoost200PercentageDefaultIDLE", null, string.Empty, 1, 0f, true, false, null, null);
			}
		}
		else
		{
			UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoostMaxPercentageDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
			if (ggpackXPMultiplier < 4f)
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
				UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoost300PercentageDefaultIN", null, string.Empty, 1, 0f, true, false, null, null);
			}
			else
			{
				UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoost300PercentageDefaultIDLE", null, string.Empty, 1, 0f, true, false, null, null);
			}
		}
		if (UIGameOverScreen.Get().IsVisible)
		{
			UIGameOverScreen.Get().UpdateGGBoostPlayerList(true);
		}
		else
		{
			UIOverconData uioverconData = UIOverconData.Get();
			List<ActorData> actors = GameFlowData.Get().GetActors();
			int actorIndex = -1;
			for (int i = 0; i < actors.Count; i++)
			{
				if (actors[i].PlayerData.PlayerHandle == info.GGPackUserName)
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
					actorIndex = actors[i].ActorIndex;
					break;
				}
			}
			uioverconData.UseOvercon(uioverconData.GetOverconIdByName("gg"), actorIndex, true);
		}
	}

	public void NotifyOtherUsedGGPack(UseGGPackNotification notification)
	{
		UISideNotifications.UIGGPackNotificationInfo info;
		info.GGPackUserBannerBackground = notification.GGPackUserBannerBackground;
		info.GGPackUserBannerForeground = notification.GGPackUserBannerForeground;
		info.GGPackUserName = notification.GGPackUserName;
		info.GGPackUserTitle = notification.GGPackUserTitle;
		info.GGPackUserTitleLevel = notification.GGPackUserTitleLevel;
		info.GGPackUserRibbon = notification.GGPackUserRibbon;
		info.NumGGPacksUsed = notification.NumGGPacksUsed;
		this.GGPackNotificationHelper(info);
	}

	public void NotifyGGPackUsed(UseGGPackResponse response)
	{
		UISideNotifications.UIGGPackNotificationInfo info;
		info.GGPackUserBannerBackground = response.GGPackUserBannerBackground;
		info.GGPackUserBannerForeground = response.GGPackUserBannerForeground;
		info.GGPackUserName = response.GGPackUserName;
		info.GGPackUserTitle = response.GGPackUserTitle;
		info.GGPackUserTitleLevel = response.GGPackUserTitleLevel;
		info.GGPackUserRibbon = response.GGPackUserRibbon;
		info.NumGGPacksUsed = this.m_numSelfGGPackUsed + 1;
		this.GGPackNotificationHelper(info);
	}

	public void Setup()
	{
		UIManager.SetGameObjectActive(this.m_ggPackAnimator, false, null);
		this.GGPackAnimListOutAnimDone();
		this.m_totalNumGGPacksUsed = 0;
		this.m_numSelfGGPackUsed = 0;
		this.m_ggPackUsers.Clear();
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideNotifications.Setup()).MethodHandle;
			}
			if (GameManager.Get().GameInfo != null)
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
				if (GameManager.Get().GameInfo.ggPackUsedAccountIDs != null)
				{
					using (Dictionary<long, int>.Enumerator enumerator = GameManager.Get().GameInfo.ggPackUsedAccountIDs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<long, int> keyValuePair = enumerator.Current;
							LobbyPlayerInfo player = GameManager.Get().TeamInfo.GetPlayer(keyValuePair.Key);
							if (player != null)
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
								if (this.m_ggPackUsers.ContainsKey(player.Handle))
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
									Dictionary<string, int> ggPackUsers;
									string handle;
									(ggPackUsers = this.m_ggPackUsers)[handle = player.Handle] = ggPackUsers[handle] + 1;
								}
								else
								{
									this.m_ggPackUsers[player.Handle] = 1;
								}
								if (player.Handle == ClientGameManager.Get().SessionInfo.Handle)
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
									this.m_numSelfGGPackUsed++;
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
				}
			}
		}
	}

	public void OnActorVisualDeath(ActorData actorDied)
	{
		if (!GameplayUtils.IsValidPlayer(actorDied))
		{
			return;
		}
		if (this.inActiveDisplays.Count == 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISideNotifications.OnActorVisualDeath(ActorData)).MethodHandle;
			}
			UINotificationDisplay item = UnityEngine.Object.Instantiate<UINotificationDisplay>(this.m_displayPrefab);
			this.inActiveDisplays.Add(item);
		}
		UINotificationDisplay uinotificationDisplay = this.inActiveDisplays[0];
		uinotificationDisplay.transform.SetParent(this.m_grid.transform);
		uinotificationDisplay.transform.localPosition = Vector3.zero;
		uinotificationDisplay.transform.localScale = Vector3.one;
		UIManager.SetGameObjectActive(uinotificationDisplay, true, null);
		this.inActiveDisplays.RemoveAt(0);
		uinotificationDisplay.Setup(actorDied);
		UISideNotifications.NotificationDisplay item2 = default(UISideNotifications.NotificationDisplay);
		item2.theDisplay = uinotificationDisplay;
		item2.timeActiveDisplayed = Time.time;
		this.activeDisplays.Add(item2);
	}

	public void SetupSideProfiles(Team team)
	{
		this.m_currentTeam = team;
		for (int i = 0; i < this.m_characterProfiles.Count; i++)
		{
			UnityEngine.Object.Destroy(this.m_characterProfiles[i].gameObject);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UISideNotifications.SetupSideProfiles(Team)).MethodHandle;
		}
		this.m_characterProfiles.Clear();
		GameFlowData gameFlowData = GameFlowData.Get();
		ActorData currentActor = GameFlowData.Get().firstOwnedFriendlyActorData;
		List<ActorData> list = (from x in gameFlowData.m_ownedActorDatas
		where x.\u000E() == currentActor.\u000E()
		select x).ToList<ActorData>();
		if (list.Count > 1)
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
			int ownedActorNum = 0;
			list.ForEach(delegate(ActorData actor)
			{
				KeyPreference keyPreference = KeyPreference.Freelancer1;
				keyPreference += ownedActorNum++;
				string fullKeyString = InputManager.Get().GetFullKeyString(keyPreference, true, false);
				this.AddSideProfileForActor(actor, fullKeyString, true);
			});
			List<ActorData> allTeamMembers = gameFlowData.GetAllTeamMembers(this.m_currentTeam);
			for (int j = 0; j < allTeamMembers.Count; j++)
			{
				if (!gameFlowData.IsActorDataOwned(allTeamMembers[j]))
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
					this.AddSideProfileForActor(allTeamMembers[j], string.Empty, false);
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
			this.m_characterProfileInstructions.text = string.Format(StringUtil.TR("SwitchCharacterInstructions", "HUDScene"), InputManager.Get().GetFullKeyString(KeyPreference.SwitchFreelancer, true, false));
		}
		UIManager.SetGameObjectActive(this.m_characterProfileGrid, list.Count > 1, null);
	}

	public void AddSideProfileForActor(ActorData actor, string hotkeyText, bool isOwned)
	{
		if (!GameplayUtils.IsValidPlayer(actor))
		{
			return;
		}
		UISideCharacterProfile uisideCharacterProfile = UnityEngine.Object.Instantiate<UISideCharacterProfile>(this.m_characterProfilePrefab);
		uisideCharacterProfile.transform.SetParent(this.m_characterProfileGrid.transform);
		uisideCharacterProfile.transform.localPosition = Vector3.zero;
		uisideCharacterProfile.transform.localScale = Vector3.one;
		uisideCharacterProfile.gameObject.SetActive(true);
		uisideCharacterProfile.Setup(actor, hotkeyText, isOwned);
		this.m_characterProfiles.Add(uisideCharacterProfile);
	}

	private struct NotificationDisplay
	{
		public UINotificationDisplay theDisplay;

		public float timeActiveDisplayed;
	}

	public struct UIGGPackNotificationInfo
	{
		public string GGPackUserName;

		public int NumGGPacksUsed;

		public int GGPackUserTitle;

		public int GGPackUserTitleLevel;

		public int GGPackUserBannerForeground;

		public int GGPackUserBannerBackground;

		public int GGPackUserRibbon;
	}
}
