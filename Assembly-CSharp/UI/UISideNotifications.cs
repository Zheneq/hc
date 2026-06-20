using LobbyGameClientMessages;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISideNotifications : MonoBehaviour
{
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

	private List<NotificationDisplay> activeDisplays;

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
		float width = (m_grid.transform as RectTransform).rect.width;
		Vector2 cellSize = m_grid.cellSize;
		float num = width / cellSize.x;
		m_grid.cellSize *= num;
		activeDisplays = new List<NotificationDisplay>();
		inActiveDisplays = new List<UINotificationDisplay>();
		activeGGPackDisplays = new List<UIGGPackNotification>();
		m_ggPackUsers = new Dictionary<string, int>();
		m_characterProfiles = new List<UISideCharacterProfile>();
		ClientGameManager.Get().OnUseGGPackNotification += NotifyOtherUsedGGPack;
		GameFlowData.s_onGameStateChanged += delegate(GameState gameState)
		{
			if (gameState == GameState.StartingGame)
			{
				SetupSideProfiles(GameManager.Get().PlayerInfo.TeamId);
			}
		};
		UIManager.SetGameObjectActive(m_characterProfileGrid, false);
	}

	public void RemoveHandleMessage()
	{
		ClientGameManager.Get().OnUseGGPackNotification -= NotifyOtherUsedGGPack;
	}

	public void SetBonusGGPackText(string text)
	{
		for (int i = 0; i < m_bonusLabel.Length; i++)
		{
			m_bonusLabel[i].text = text;
		}
		while (true)
		{
			return;
		}
	}

	private void Update()
	{
		GameFlowData gameFlowData = GameFlowData.Get();
		for (int i = 0; i < activeDisplays.Count; i++)
		{
			NotificationDisplay notificationDisplay = activeDisplays[i];
			float num = m_timeToDisplayNotification - (Time.time - notificationDisplay.timeActiveDisplayed);
			if (num <= 0f)
			{
				activeDisplays.RemoveAt(i);
				UIManager.SetGameObjectActive(notificationDisplay.theDisplay, false);
				notificationDisplay.theDisplay.transform.SetParent(m_grid.transform.parent);
				inActiveDisplays.Add(notificationDisplay.theDisplay);
			}
			else if (num < 1f)
			{
				notificationDisplay.theDisplay.SetAlpha(num);
			}
		}
		while (true)
		{
			bool flag = true;
			int num2 = 0;
			while (true)
			{
				if (num2 < activeGGPackDisplays.Count)
				{
					if (!activeGGPackDisplays[num2].DoneAnimating)
					{
						flag = false;
						break;
					}
					num2++;
					continue;
				}
				break;
			}
			if (flag)
			{
				m_ggPackNotificationActive = false;
				if (m_ggPackAnimator.enabled)
				{
					if (m_ggPackAnimator.gameObject.activeInHierarchy)
					{
						AnimatorClipInfo[] currentAnimatorClipInfo = m_ggPackAnimator.GetCurrentAnimatorClipInfo(0);
						if (currentAnimatorClipInfo != null)
						{
							if (currentAnimatorClipInfo.Length > 0)
							{
								if (currentAnimatorClipInfo[0].clip.name != "ggPackListOUT")
								{
									m_ggPackAnimator.Play("ggPackListOUT");
								}
							}
						}
					}
				}
			}
			if (!(gameFlowData != null))
			{
				return;
			}
			while (true)
			{
				if (!(gameFlowData.activeOwnedActorData != null))
				{
					return;
				}
				while (true)
				{
					if (gameFlowData.activeOwnedActorData.GetTeam() != m_currentTeam)
					{
						SetupSideProfiles(gameFlowData.activeOwnedActorData.GetTeam());
					}
					return;
				}
			}
		}
	}

	public int NumberGGPacksUsed(string username)
	{
		if (m_ggPackUsers.ContainsKey(username))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_ggPackUsers[username];
				}
			}
		}
		return 0;
	}

	public void GGPackAnimListOutAnimDone()
	{
		UIManager.SetGameObjectActive(m_ggPackAnimator, false);
		if (activeGGPackDisplays != null)
		{
			for (int i = 0; i < activeGGPackDisplays.Count; i++)
			{
				Object.Destroy(activeGGPackDisplays[i].gameObject);
			}
			activeGGPackDisplays.Clear();
		}
	}

	public int NumSelfGGPacksUsed()
	{
		return m_numSelfGGPackUsed;
	}

	private void GGPackNotificationHelper(UIGGPackNotificationInfo info)
	{
		float gGPackXPMultiplier = GameBalanceVars.Get().GetGGPackXPMultiplier(m_totalNumGGPacksUsed, m_numSelfGGPackUsed);
		if (!m_ggPackNotificationActive)
		{
			UIManager.SetGameObjectActive(m_ggPackAnimator, true);
			m_ggPackAnimator.Play("ggPackListIN");
			m_ggPackNotificationActive = true;
		}
		else
		{
			m_ggPackAnimator.Play("ggPackListINCREASE", -1, 0f);
		}
		if (info.GGPackUserName == ClientGameManager.Get().SessionInfo.Handle)
		{
			m_numSelfGGPackUsed++;
		}
		if (m_ggPackUsers.ContainsKey(info.GGPackUserName))
		{
			m_ggPackUsers[info.GGPackUserName]++;
		}
		else
		{
			m_ggPackUsers[info.GGPackUserName] = 1;
		}
		m_totalNumGGPacksUsed++;
		int gGPackBonusISO = GameBalanceVars.Get().GetGGPackBonusISO(m_totalNumGGPacksUsed, m_numSelfGGPackUsed);
		float gGPackXPMultiplier2 = GameBalanceVars.Get().GetGGPackXPMultiplier(m_totalNumGGPacksUsed, m_numSelfGGPackUsed);
		UIGameOverScreen.Get().UpdateEndGameGGBonuses(gGPackBonusISO, gGPackXPMultiplier2);
		UIGGPackNotification uIGGPackNotification = Object.Instantiate(m_ggPackDisplayPrefab);
		uIGGPackNotification.transform.SetParent(m_ggPackGrid.transform);
		uIGGPackNotification.transform.localPosition = Vector3.zero;
		uIGGPackNotification.transform.localEulerAngles = Vector3.zero;
		uIGGPackNotification.transform.localScale = Vector3.one;
		uIGGPackNotification.transform.SetAsFirstSibling();
		uIGGPackNotification.Setup(info);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GGPackUsedNotification);
		activeGGPackDisplays.Add(uIGGPackNotification);
		SetBonusGGPackText(string.Format(StringUtil.TR("InGameGGBonuses", "HUD"), Mathf.RoundToInt((gGPackXPMultiplier2 - 1f) * 100f)));
		if (gGPackXPMultiplier2 < 2f)
		{
			UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoostBluePercentageDefaultIN", null, string.Empty);
		}
		else if (gGPackXPMultiplier2 < 3f)
		{
			UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoostSilverPercentageDefaultIN", null, string.Empty);
			if (gGPackXPMultiplier < 2f)
			{
				UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoost100PercentageDefaultIN", null, string.Empty, 1);
			}
			else
			{
				UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoost100PercentageDefaultIDLE", null, string.Empty, 1);
			}
		}
		else if (gGPackXPMultiplier2 < 4f)
		{
			UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoostGoldPercentageDefaultIN", null, string.Empty);
			if (gGPackXPMultiplier < 3f)
			{
				UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoost200PercentageDefaultIN", null, string.Empty, 1);
			}
			else
			{
				UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoost200PercentageDefaultIDLE", null, string.Empty, 1);
			}
		}
		else
		{
			UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoostMaxPercentageDefaultIN", null, string.Empty);
			if (gGPackXPMultiplier < 4f)
			{
				UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoost300PercentageDefaultIN", null, string.Empty, 1);
			}
			else
			{
				UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoost300PercentageDefaultIDLE", null, string.Empty, 1);
			}
		}
		if (UIGameOverScreen.Get().IsVisible)
		{
			UIGameOverScreen.Get().UpdateGGBoostPlayerList();
			return;
		}
		UIOverconData uIOverconData = UIOverconData.Get();
		List<ActorData> actors = GameFlowData.Get().GetActors();
		int actorIndex = -1;
		for (int i = 0; i < actors.Count; i++)
		{
			if (actors[i].PlayerData.PlayerHandle == info.GGPackUserName)
			{
				actorIndex = actors[i].ActorIndex;
				break;
			}
		}
		uIOverconData.UseOvercon(uIOverconData.GetOverconIdByName("gg"), actorIndex, true);
	}

	public void NotifyOtherUsedGGPack(UseGGPackNotification notification)
	{
		UIGGPackNotificationInfo info = default(UIGGPackNotificationInfo);
		info.GGPackUserBannerBackground = notification.GGPackUserBannerBackground;
		info.GGPackUserBannerForeground = notification.GGPackUserBannerForeground;
		info.GGPackUserName = notification.GGPackUserName;
		info.GGPackUserTitle = notification.GGPackUserTitle;
		info.GGPackUserTitleLevel = notification.GGPackUserTitleLevel;
		info.GGPackUserRibbon = notification.GGPackUserRibbon;
		info.NumGGPacksUsed = notification.NumGGPacksUsed;
		GGPackNotificationHelper(info);
	}

	public void NotifyGGPackUsed(UseGGPackResponse response)
	{
		UIGGPackNotificationInfo info = default(UIGGPackNotificationInfo);
		info.GGPackUserBannerBackground = response.GGPackUserBannerBackground;
		info.GGPackUserBannerForeground = response.GGPackUserBannerForeground;
		info.GGPackUserName = response.GGPackUserName;
		info.GGPackUserTitle = response.GGPackUserTitle;
		info.GGPackUserTitleLevel = response.GGPackUserTitleLevel;
		info.GGPackUserRibbon = response.GGPackUserRibbon;
		info.NumGGPacksUsed = m_numSelfGGPackUsed + 1;
		GGPackNotificationHelper(info);
	}

	public void Setup()
	{
		UIManager.SetGameObjectActive(m_ggPackAnimator, false);
		GGPackAnimListOutAnimDone();
		m_totalNumGGPacksUsed = 0;
		m_numSelfGGPackUsed = 0;
		m_ggPackUsers.Clear();
		if (!(GameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (GameManager.Get().GameInfo == null)
			{
				return;
			}
			while (true)
			{
				if (GameManager.Get().GameInfo.ggPackUsedAccountIDs != null)
				{
					using (Dictionary<long, int>.Enumerator enumerator = GameManager.Get().GameInfo.ggPackUsedAccountIDs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<long, int> current = enumerator.Current;
							LobbyPlayerInfo player = GameManager.Get().TeamInfo.GetPlayer(current.Key);
							if (player != null)
							{
								if (m_ggPackUsers.ContainsKey(player.Handle))
								{
									m_ggPackUsers[player.Handle]++;
								}
								else
								{
									m_ggPackUsers[player.Handle] = 1;
								}
								if (player.Handle == ClientGameManager.Get().SessionInfo.Handle)
								{
									m_numSelfGGPackUsed++;
								}
							}
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
				}
				return;
			}
		}
	}

	public void OnActorVisualDeath(ActorData actorDied)
	{
		if (!GameplayUtils.IsValidPlayer(actorDied))
		{
			return;
		}
		if (inActiveDisplays.Count == 0)
		{
			UINotificationDisplay item = Object.Instantiate(m_displayPrefab);
			inActiveDisplays.Add(item);
		}
		UINotificationDisplay uINotificationDisplay = inActiveDisplays[0];
		uINotificationDisplay.transform.SetParent(m_grid.transform);
		uINotificationDisplay.transform.localPosition = Vector3.zero;
		uINotificationDisplay.transform.localScale = Vector3.one;
		UIManager.SetGameObjectActive(uINotificationDisplay, true);
		inActiveDisplays.RemoveAt(0);
		uINotificationDisplay.Setup(actorDied);
		NotificationDisplay item2 = default(NotificationDisplay);
		item2.theDisplay = uINotificationDisplay;
		item2.timeActiveDisplayed = Time.time;
		activeDisplays.Add(item2);
	}

	public void SetupSideProfiles(Team team)
	{
		m_currentTeam = team;
		for (int i = 0; i < m_characterProfiles.Count; i++)
		{
			Object.Destroy(m_characterProfiles[i].gameObject);
		}
		ActorData currentActor;
		while (true)
		{
			m_characterProfiles.Clear();
			GameFlowData gameFlowData = GameFlowData.Get();
			currentActor = GameFlowData.Get().firstOwnedFriendlyActorData;
			List<ActorData> list = gameFlowData.m_ownedActorDatas.Where((ActorData x) => x.GetTeam() == currentActor.GetTeam()).ToList();
			if (list.Count > 1)
			{
				int ownedActorNum = 0;
				list.ForEach(delegate(ActorData actor)
				{
					KeyPreference keyPreference = KeyPreference.Freelancer1;
					keyPreference += ownedActorNum++;
					string fullKeyString = InputManager.Get().GetFullKeyString(keyPreference, true);
					AddSideProfileForActor(actor, fullKeyString, true);
				});
				List<ActorData> allTeamMembers = gameFlowData.GetAllTeamMembers(m_currentTeam);
				for (int j = 0; j < allTeamMembers.Count; j++)
				{
					if (!gameFlowData.IsActorDataOwned(allTeamMembers[j]))
					{
						AddSideProfileForActor(allTeamMembers[j], string.Empty, false);
					}
				}
				m_characterProfileInstructions.text = string.Format(StringUtil.TR("SwitchCharacterInstructions", "HUDScene"), InputManager.Get().GetFullKeyString(KeyPreference.SwitchFreelancer, true));
			}
			UIManager.SetGameObjectActive(m_characterProfileGrid, list.Count > 1);
			return;
		}
	}

	public void AddSideProfileForActor(ActorData actor, string hotkeyText, bool isOwned)
	{
		if (GameplayUtils.IsValidPlayer(actor))
		{
			UISideCharacterProfile uISideCharacterProfile = Object.Instantiate(m_characterProfilePrefab);
			uISideCharacterProfile.transform.SetParent(m_characterProfileGrid.transform);
			uISideCharacterProfile.transform.localPosition = Vector3.zero;
			uISideCharacterProfile.transform.localScale = Vector3.one;
			uISideCharacterProfile.gameObject.SetActive(true);
			uISideCharacterProfile.Setup(actor, hotkeyText, isOwned);
			m_characterProfiles.Add(uISideCharacterProfile);
		}
	}
}
