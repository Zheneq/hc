using LobbyGameClientMessages;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINewReward : UIScene
{
	public struct RewardAnnouncementDisplayInfo
	{
		public RewardUtils.RewardData m_rewardData;

		public CharacterType m_unlockCharType;

		public int m_unlockLevel;

		public string m_displayName;
	}

	public struct SeasonReward
	{
		public InventoryItemTemplate m_itemTemplate;

		public int m_amount;

		public int m_unlockLevel;
	}

	public class FactionRewardAnnounceInfo
	{
		public bool DisplayDescription;

		public string FactionSpritePath;

		public FactionReward FactionReward;

		public int Level;
	}

	private static UINewReward s_instance;

	public RectTransform m_container;

	public RectTransform m_frontendContainer;

	public RectTransform m_seasonContainer;

	public RectTransform m_trustContainer;

	public UIChapterCompletedScreen m_chapterNotifications;

	public Sprite m_generalSprite;

	public TextMeshProUGUI m_rewardRequirements;

	public TextMeshProUGUI m_rewardName;

	public TextMeshProUGUI m_rewardType;

	public Image m_rewardIcon;

	public Image m_rewardFG;

	public Image m_rewardBanner;

	public QuestReward[] m_seasonRewards;

	public Animator m_animationController;

	public Animator m_seasonAnimationController;

	public Animator m_trustAnimationController;

	public float m_timeBeforeCanCloseReward = 2f;

	[Header("Trust Reward")]
	public TextMeshProUGUI m_trustRewardName;

	public TextMeshProUGUI m_trustRewardDescription;

	public TextMeshProUGUI m_trustRewardTierLevel;

	public RectTransform m_FactionContainer;

	public RectTransform m_TierContainer;

	public Image m_trustRewardIcon;

	public Image m_trustFactionIcon;

	private List<RewardAnnouncementDisplayInfo> rewardTypesToAnnounce;

	private List<SeasonReward> m_seasonRewardsToAnnounce;

	private List<FactionRewardAnnounceInfo> m_trustRewardsToAnnounce;

	private bool m_rewardAnnouncementInProgress;

	private float m_rewardStartTime;

	private bool m_autoPlayNextReward;

	public override SceneType GetSceneType()
	{
		return SceneType.RewardsEffects;
	}

	public override void Awake()
	{
		s_instance = this;
		rewardTypesToAnnounce = new List<RewardAnnouncementDisplayInfo>();
		m_seasonRewardsToAnnounce = new List<SeasonReward>();
		m_trustRewardsToAnnounce = new List<FactionRewardAnnounceInfo>();
		UIManager.SetGameObjectActive(m_container, false);
		UIManager.SetGameObjectActive(m_seasonContainer, false);
		UIManager.SetGameObjectActive(m_trustContainer, false);
		ClientGameManager.Get().OnPlayerFactionContributionChangeNotification += OnPlayerFactionContributionChangeNotification;
		ClientGameManager.Get().OnQuestCompleteNotification += HandleQuestCompleteNotification;
		base.Awake();
	}

	private void OnDestroy()
	{
		s_instance = null;
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().OnPlayerFactionContributionChangeNotification -= OnPlayerFactionContributionChangeNotification;
			ClientGameManager.Get().OnQuestCompleteNotification -= HandleQuestCompleteNotification;
			return;
		}
	}

	public static UINewReward Get()
	{
		return s_instance;
	}

	private void OnPlayerFactionContributionChangeNotification(PlayerFactionContributionChangeNotification notification)
	{
		FactionPlayerData playerCompetitionFactionData = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetPlayerCompetitionFactionData(notification.CompetitionId, notification.FactionId);
		Faction faction = FactionWideData.Get().GetFaction(notification.CompetitionId, notification.FactionId);
		int currentLevel = playerCompetitionFactionData.GetCurrentLevel(faction.FactionPlayerProgressInfo);
		int currentLevel2 = playerCompetitionFactionData.GetCurrentLevel(faction.FactionPlayerProgressInfo, notification.AmountChanged * -1);
		if (currentLevel <= currentLevel2)
		{
			return;
		}
		while (true)
		{
			FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(faction.FactionGroupIDToUse);
			int num = currentLevel2;
			while (num < currentLevel)
			{
				while (true)
				{
					if (num <= faction.FactionPlayerProgressInfo.Length)
					{
						NotifyNewTrustReward(faction.FactionPlayerProgressInfo[num - 1].LevelUpRewards, num, factionGroup.IconPath);
						num++;
						goto IL_00c2;
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
				IL_00c2:;
			}
			return;
		}
	}

	public void NotifyNewRewardReceived(RewardUtils.RewardData data, CharacterType charType = CharacterType.None, int unlockLevel = -1, int currencyAmount = -1)
	{
		RewardAnnouncementDisplayInfo item = default(RewardAnnouncementDisplayInfo);
		item.m_rewardData = data;
		item.m_unlockCharType = charType;
		item.m_unlockLevel = unlockLevel;
		if (GameFlowData.Get() == null)
		{
			item.m_displayName = string.Empty;
		}
		else if (GameFlowData.Get().activeOwnedActorData != null)
		{
			item.m_displayName = GameFlowData.Get().activeOwnedActorData.DisplayName;
		}
		else
		{
			item.m_displayName = ClientGameManager.Get().Handle;
		}
		rewardTypesToAnnounce.Add(item);
	}

	public void NotifyNewRewardReceived(RewardUtils.RewardType type)
	{
		RewardUtils.RewardData rewardData = new RewardUtils.RewardData();
		rewardData.Name = string.Empty;
		rewardData.SpritePath = string.Empty;
		rewardData.Type = type;
		rewardData.Amount = -1;
		RewardAnnouncementDisplayInfo item = default(RewardAnnouncementDisplayInfo);
		item.m_rewardData = rewardData;
		item.m_unlockCharType = CharacterType.None;
		item.m_unlockLevel = -1;
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			item.m_displayName = GameFlowData.Get().activeOwnedActorData.DisplayName;
		}
		else
		{
			item.m_displayName = ClientGameManager.Get().Handle;
		}
		rewardTypesToAnnounce.Add(item);
	}

	public void NotifySeasonReward(InventoryItemTemplate template, int unlockLevel, int amount)
	{
		SeasonReward item = default(SeasonReward);
		item.m_itemTemplate = template;
		item.m_amount = amount;
		item.m_unlockLevel = unlockLevel;
		m_seasonRewardsToAnnounce.Add(item);
	}

	public void NotifyNewTrustReward(FactionRewards rewards, int levelObtained, string FactionIconResourcePath, bool displayDescription = false)
	{
		using (List<FactionReward>.Enumerator enumerator = rewards.GetAllRewards().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FactionReward current = enumerator.Current;
				FactionRewardAnnounceInfo factionRewardAnnounceInfo = new FactionRewardAnnounceInfo();
				factionRewardAnnounceInfo.FactionReward = current;
				factionRewardAnnounceInfo.Level = levelObtained;
				factionRewardAnnounceInfo.FactionSpritePath = FactionIconResourcePath;
				factionRewardAnnounceInfo.DisplayDescription = displayDescription;
				m_trustRewardsToAnnounce.Add(factionRewardAnnounceInfo);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
	}

	public void NotifyNewQuestReward(QuestRewards rewards, int levelObtained = -1, bool displayDescription = false)
	{
		using (List<QuestItemReward>.Enumerator enumerator = rewards.ItemRewards.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QuestItemReward current = enumerator.Current;
				FactionRewardAnnounceInfo factionRewardAnnounceInfo = new FactionRewardAnnounceInfo();
				factionRewardAnnounceInfo.FactionReward = new FactionItemReward
				{
					ItemReward = current
				};
				factionRewardAnnounceInfo.Level = levelObtained;
				factionRewardAnnounceInfo.FactionSpritePath = string.Empty;
				factionRewardAnnounceInfo.DisplayDescription = displayDescription;
				m_trustRewardsToAnnounce.Add(factionRewardAnnounceInfo);
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					goto end_IL_000e;
				}
			}
			end_IL_000e:;
		}
		using (List<QuestUnlockReward>.Enumerator enumerator2 = rewards.UnlockRewards.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				QuestUnlockReward current2 = enumerator2.Current;
				FactionRewardAnnounceInfo factionRewardAnnounceInfo2 = new FactionRewardAnnounceInfo();
				factionRewardAnnounceInfo2.FactionReward = new FactionUnlockReward
				{
					UnlockReward = current2
				};
				factionRewardAnnounceInfo2.Level = levelObtained;
				factionRewardAnnounceInfo2.FactionSpritePath = string.Empty;
				factionRewardAnnounceInfo2.DisplayDescription = displayDescription;
				m_trustRewardsToAnnounce.Add(factionRewardAnnounceInfo2);
			}
		}
		using (List<QuestCurrencyReward>.Enumerator enumerator3 = rewards.CurrencyRewards.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				QuestCurrencyReward current3 = enumerator3.Current;
				FactionRewardAnnounceInfo factionRewardAnnounceInfo3 = new FactionRewardAnnounceInfo();
				factionRewardAnnounceInfo3.FactionReward = new FactionCurrencyReward
				{
					CurrencyReward = current3
				};
				factionRewardAnnounceInfo3.Level = levelObtained;
				factionRewardAnnounceInfo3.FactionSpritePath = string.Empty;
				factionRewardAnnounceInfo3.DisplayDescription = true;
				m_trustRewardsToAnnounce.Add(factionRewardAnnounceInfo3);
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

	private void HandleQuestCompleteNotification(QuestCompleteNotification message)
	{
		QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(message.questId);
		if (questTemplate != null && questTemplate.DisplayRewardNotification)
		{
			NotifyNewQuestReward(questTemplate.Rewards, -1, true);
		}
	}

	private void SetupReward(RewardAnnouncementDisplayInfo info)
	{
		string name = info.m_rewardData.Name;
		string text = string.Empty;
		if (info.m_rewardData.Type != RewardUtils.RewardType.ISO)
		{
			if (info.m_rewardData.Type != RewardUtils.RewardType.ModToken)
			{
				if (info.m_rewardData.Type != RewardUtils.RewardType.GGBoost)
				{
					if (info.m_rewardData.Type != RewardUtils.RewardType.System)
					{
						text = RewardUtils.GetTypeDisplayString(info.m_rewardData.Type, false);
					}
				}
			}
		}
		string str = string.Empty;
		if (info.m_unlockLevel > 0)
		{
			str = string.Format(StringUtil.TR("LevelRequirement", "Rewards"), info.m_unlockLevel);
		}
		if (info.m_unlockCharType != 0)
		{
			str = new StringBuilder().Append(str).Append(" ").Append(GameWideData.Get().GetCharacterResourceLink(info.m_unlockCharType).GetDisplayName()).ToString();
		}
		else
		{
			str = new StringBuilder().Append(str).Append(" ").Append(info.m_displayName).ToString();
		}
		name = RewardUtils.GetDisplayString(info.m_rewardData, true);
		if (info.m_rewardData.SpritePath == null)
		{
			m_rewardIcon.sprite = m_generalSprite;
		}
		else
		{
			Sprite sprite = (Sprite)Resources.Load(info.m_rewardData.SpritePath, typeof(Sprite));
			if (info.m_rewardData.Type == RewardUtils.RewardType.Banner)
			{
				UIManager.SetGameObjectActive(m_rewardIcon, false);
				UIManager.SetGameObjectActive(m_rewardBanner, true);
				m_rewardBanner.sprite = sprite;
			}
			else
			{
				UIManager.SetGameObjectActive(m_rewardIcon, true);
				UIManager.SetGameObjectActive(m_rewardBanner, false);
				m_rewardIcon.sprite = sprite;
			}
		}
		if (info.m_rewardData.Foreground == null)
		{
			UIManager.SetGameObjectActive(m_rewardFG, false);
		}
		else
		{
			UIManager.SetGameObjectActive(m_rewardFG, true);
			m_rewardFG.sprite = info.m_rewardData.Foreground;
		}
		m_rewardRequirements.text = str;
		m_rewardName.text = name;
		m_rewardType.text = text;
	}

	private void SetupSeasonReward(List<SeasonReward> rewards)
	{
		for (int i = 0; i < m_seasonRewards.Length; i++)
		{
			if (i >= rewards.Count)
			{
				UIManager.SetGameObjectActive(m_seasonRewards[i], false);
			}
			else
			{
				QuestReward obj = m_seasonRewards[i];
				SeasonReward seasonReward = rewards[i];
				InventoryItemTemplate itemTemplate = seasonReward.m_itemTemplate;
				SeasonReward seasonReward2 = rewards[i];
				string spritePath = InventoryWideData.GetSpritePath(seasonReward2.m_itemTemplate);
				SeasonReward seasonReward3 = rewards[i];
				obj.SetupHack(itemTemplate, spritePath, seasonReward3.m_amount);
				UIManager.SetGameObjectActive(m_seasonRewards[i], true);
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

	private void SetupTrustReward(FactionRewardAnnounceInfo info)
	{
		m_trustRewardTierLevel.text = string.Format(StringUtil.TR("PersonalTierLevel", "TrustWar"), info.Level);
		Sprite sprite = Resources.Load(info.FactionSpritePath, typeof(Sprite)) as Sprite;
		UIManager.SetGameObjectActive(m_FactionContainer, sprite != null);
		UIManager.SetGameObjectActive(m_TierContainer, sprite != null);
		UIManager.SetGameObjectActive(m_trustFactionIcon, sprite != null);
		m_trustFactionIcon.sprite = sprite;
		UIManager.SetGameObjectActive(m_trustRewardDescription, info.DisplayDescription);
		FactionItemReward factionItemReward = info.FactionReward as FactionItemReward;
		if (factionItemReward != null)
		{
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(factionItemReward.ItemReward.ItemTemplateId);
			m_trustRewardName.text = itemTemplate.GetDisplayName();
			m_trustRewardDescription.text = itemTemplate.GetObtainDescription();
			m_trustRewardIcon.sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate));
		}
		FactionUnlockReward factionUnlockReward = info.FactionReward as FactionUnlockReward;
		if (factionUnlockReward != null)
		{
			m_trustRewardIcon.sprite = Resources.Load<Sprite>(factionUnlockReward.UnlockReward.resourceString);
			m_trustRewardName.text = RewardUtils.GetRewardDisplayName(factionUnlockReward.UnlockReward.purchaseType, factionUnlockReward.UnlockReward.typeSpecificData);
			m_trustRewardDescription.text = StringUtil.TR("Unlocked", "OverlayScreensScene");
		}
		FactionCurrencyReward factionCurrencyReward = info.FactionReward as FactionCurrencyReward;
		if (factionCurrencyReward == null)
		{
			return;
		}
		while (true)
		{
			m_trustRewardIcon.sprite = Resources.Load<Sprite>(RewardUtils.GetCurrencyIconPath(factionCurrencyReward.CurrencyReward.Type));
			m_trustRewardName.text = factionCurrencyReward.CurrencyReward.Amount.ToString();
			RewardUtils.RewardType type = RewardUtils.RewardType.ISO;
			CurrencyType type2 = factionCurrencyReward.CurrencyReward.Type;
			switch (type2)
			{
			default:
				if (type2 != CurrencyType.UnlockFreelancerToken)
				{
				}
				else
				{
					type = RewardUtils.RewardType.UnlockFreelancerToken;
				}
				break;
			case CurrencyType.ModToken:
				type = RewardUtils.RewardType.ModToken;
				break;
			case CurrencyType.GGPack:
				type = RewardUtils.RewardType.GGBoost;
				break;
			case CurrencyType.ISO:
				type = RewardUtils.RewardType.ISO;
				break;
			case CurrencyType.FreelancerCurrency:
				type = RewardUtils.RewardType.FreelancerCurrency;
				break;
			}
			m_trustRewardDescription.text = RewardUtils.GetTypeDisplayString(type, false);
			return;
		}
	}

	public bool RewardIsBeingAnnounced()
	{
		int result;
		if (!m_rewardAnnouncementInProgress)
		{
			result = ((rewardTypesToAnnounce.Count > 0) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public void AutoPlayNextAnimation()
	{
		m_autoPlayNextReward = true;
	}

	public void Update()
	{
		if (m_chapterNotifications.IsCurrentlyDisplaying())
		{
			return;
		}
		if (m_rewardAnnouncementInProgress)
		{
			float num = Time.time - m_rewardStartTime;
			if (!m_autoPlayNextReward)
			{
				if (num >= m_timeBeforeCanCloseReward)
				{
					if (Input.GetMouseButtonDown(0))
					{
						goto IL_0128;
					}
				}
				if (m_container.gameObject.activeInHierarchy)
				{
					if (num > m_animationController.GetCurrentAnimatorStateInfo(0).length)
					{
						goto IL_0128;
					}
				}
				if (m_seasonContainer.gameObject.activeInHierarchy)
				{
					if (num > m_seasonAnimationController.GetCurrentAnimatorStateInfo(0).length)
					{
						goto IL_0128;
					}
				}
				if (!m_trustContainer.gameObject.activeInHierarchy || !(num > m_trustAnimationController.GetCurrentAnimatorStateInfo(0).length))
				{
					goto IL_03cc;
				}
			}
			goto IL_0128;
		}
		if (rewardTypesToAnnounce.Count <= 0)
		{
			if (m_seasonRewardsToAnnounce.Count <= 0)
			{
				if (m_trustRewardsToAnnounce.Count <= 0)
				{
					goto IL_03cc;
				}
			}
		}
		m_autoPlayNextReward = false;
		if (m_seasonRewardsToAnnounce.Count > 0)
		{
			SeasonReward seasonReward = m_seasonRewardsToAnnounce[0];
			int unlockLevel = seasonReward.m_unlockLevel;
			List<SeasonReward> list = new List<SeasonReward>();
			int num2 = m_seasonRewardsToAnnounce.Count - 1;
			while (true)
			{
				if (num2 >= 0)
				{
					SeasonReward seasonReward2 = m_seasonRewardsToAnnounce[num2];
					if (seasonReward2.m_unlockLevel == unlockLevel)
					{
						list.Add(m_seasonRewardsToAnnounce[num2]);
						m_seasonRewardsToAnnounce.RemoveAt(num2);
						if (list.Count >= m_seasonRewards.Length)
						{
							break;
						}
					}
					num2--;
					continue;
				}
				break;
			}
			SetupSeasonReward(list);
			UIManager.SetGameObjectActive(m_seasonContainer, true);
			UIManager.SetGameObjectActive(m_container, false);
			UIManager.SetGameObjectActive(m_trustContainer, false);
			m_seasonAnimationController.Play("NewReward", -1, 0f);
		}
		else if (m_trustRewardsToAnnounce.Count > 0)
		{
			SetupTrustReward(m_trustRewardsToAnnounce[0]);
			UIManager.SetGameObjectActive(m_seasonContainer, false);
			UIManager.SetGameObjectActive(m_container, false);
			UIManager.SetGameObjectActive(m_trustContainer, true);
			m_trustAnimationController.Play("NewTrustRewardDefaultIN", -1, 0f);
			m_trustRewardsToAnnounce.RemoveAt(0);
		}
		else
		{
			SetupReward(rewardTypesToAnnounce[0]);
			UIManager.SetGameObjectActive(m_container, true);
			UIManager.SetGameObjectActive(m_seasonContainer, false);
			UIManager.SetGameObjectActive(m_trustContainer, false);
			m_animationController.Play("NewReward", -1, 0f);
			rewardTypesToAnnounce.RemoveAt(0);
		}
		UIScreenManager.Get().EndAllLoopSounds();
		AudioManager.PostEvent("ui/endgame/unlock");
		m_rewardAnnouncementInProgress = true;
		m_rewardStartTime = Time.time;
		goto IL_03cc;
		IL_03cc:
		if (m_rewardAnnouncementInProgress)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_frontendContainer, UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd);
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_frontendContainer, false);
		return;
		IL_0128:
		m_rewardAnnouncementInProgress = false;
		if (rewardTypesToAnnounce.Count <= 0)
		{
			UIManager.SetGameObjectActive(m_container, false);
			UIManager.SetGameObjectActive(m_seasonContainer, false);
			UIManager.SetGameObjectActive(m_trustContainer, false);
		}
		goto IL_03cc;
	}

	public void Clear()
	{
		m_rewardAnnouncementInProgress = false;
		UIManager.SetGameObjectActive(m_container, false);
		UIManager.SetGameObjectActive(m_seasonContainer, false);
		UIManager.SetGameObjectActive(m_trustContainer, false);
		rewardTypesToAnnounce.Clear();
	}

	public bool IsActive()
	{
		int result;
		if (rewardTypesToAnnounce.Count <= 0)
		{
			result = (m_rewardAnnouncementInProgress ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}
}
