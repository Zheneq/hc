using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINewReward : UIScene
{
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

	private List<UINewReward.RewardAnnouncementDisplayInfo> rewardTypesToAnnounce;

	private List<UINewReward.SeasonReward> m_seasonRewardsToAnnounce;

	private List<UINewReward.FactionRewardAnnounceInfo> m_trustRewardsToAnnounce;

	private bool m_rewardAnnouncementInProgress;

	private float m_rewardStartTime;

	private bool m_autoPlayNextReward;

	public override SceneType GetSceneType()
	{
		return SceneType.RewardsEffects;
	}

	public override void Awake()
	{
		UINewReward.s_instance = this;
		this.rewardTypesToAnnounce = new List<UINewReward.RewardAnnouncementDisplayInfo>();
		this.m_seasonRewardsToAnnounce = new List<UINewReward.SeasonReward>();
		this.m_trustRewardsToAnnounce = new List<UINewReward.FactionRewardAnnounceInfo>();
		UIManager.SetGameObjectActive(this.m_container, false, null);
		UIManager.SetGameObjectActive(this.m_seasonContainer, false, null);
		UIManager.SetGameObjectActive(this.m_trustContainer, false, null);
		ClientGameManager.Get().OnPlayerFactionContributionChangeNotification += this.OnPlayerFactionContributionChangeNotification;
		ClientGameManager.Get().OnQuestCompleteNotification += this.HandleQuestCompleteNotification;
		base.Awake();
	}

	private void OnDestroy()
	{
		UINewReward.s_instance = null;
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnPlayerFactionContributionChangeNotification -= this.OnPlayerFactionContributionChangeNotification;
			ClientGameManager.Get().OnQuestCompleteNotification -= this.HandleQuestCompleteNotification;
		}
	}

	public static UINewReward Get()
	{
		return UINewReward.s_instance;
	}

	private void OnPlayerFactionContributionChangeNotification(PlayerFactionContributionChangeNotification notification)
	{
		FactionPlayerData playerCompetitionFactionData = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetPlayerCompetitionFactionData(notification.CompetitionId, notification.FactionId);
		Faction faction = FactionWideData.Get().GetFaction(notification.CompetitionId, notification.FactionId);
		int currentLevel = playerCompetitionFactionData.GetCurrentLevel(faction.FactionPlayerProgressInfo, 0);
		int currentLevel2 = playerCompetitionFactionData.GetCurrentLevel(faction.FactionPlayerProgressInfo, notification.AmountChanged * -1);
		if (currentLevel > currentLevel2)
		{
			FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(faction.FactionGroupIDToUse);
			int i = currentLevel2;
			while (i < currentLevel)
			{
				if (i > faction.FactionPlayerProgressInfo.Length)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						return;
					}
				}
				else
				{
					this.NotifyNewTrustReward(faction.FactionPlayerProgressInfo[i - 1].LevelUpRewards, i, factionGroup.IconPath, false);
					i++;
				}
			}
		}
	}

	public void NotifyNewRewardReceived(RewardUtils.RewardData data, CharacterType charType = CharacterType.None, int unlockLevel = -1, int currencyAmount = -1)
	{
		UINewReward.RewardAnnouncementDisplayInfo item;
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
		this.rewardTypesToAnnounce.Add(item);
	}

	public void NotifyNewRewardReceived(RewardUtils.RewardType type)
	{
		UINewReward.RewardAnnouncementDisplayInfo item;
		item.m_rewardData = new RewardUtils.RewardData
		{
			Name = string.Empty,
			SpritePath = string.Empty,
			Type = type,
			Amount = -1
		};
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
		this.rewardTypesToAnnounce.Add(item);
	}

	public void NotifySeasonReward(InventoryItemTemplate template, int unlockLevel, int amount)
	{
		UINewReward.SeasonReward item;
		item.m_itemTemplate = template;
		item.m_amount = amount;
		item.m_unlockLevel = unlockLevel;
		this.m_seasonRewardsToAnnounce.Add(item);
	}

	public void NotifyNewTrustReward(FactionRewards rewards, int levelObtained, string FactionIconResourcePath, bool displayDescription = false)
	{
		using (List<FactionReward>.Enumerator enumerator = rewards.GetAllRewards().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FactionReward factionReward = enumerator.Current;
				UINewReward.FactionRewardAnnounceInfo factionRewardAnnounceInfo = new UINewReward.FactionRewardAnnounceInfo();
				factionRewardAnnounceInfo.FactionReward = factionReward;
				factionRewardAnnounceInfo.Level = levelObtained;
				factionRewardAnnounceInfo.FactionSpritePath = FactionIconResourcePath;
				factionRewardAnnounceInfo.DisplayDescription = displayDescription;
				this.m_trustRewardsToAnnounce.Add(factionRewardAnnounceInfo);
			}
		}
	}

	public void NotifyNewQuestReward(QuestRewards rewards, int levelObtained = -1, bool displayDescription = false)
	{
		using (List<QuestItemReward>.Enumerator enumerator = rewards.ItemRewards.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QuestItemReward itemReward = enumerator.Current;
				UINewReward.FactionRewardAnnounceInfo factionRewardAnnounceInfo = new UINewReward.FactionRewardAnnounceInfo();
				factionRewardAnnounceInfo.FactionReward = new FactionItemReward
				{
					ItemReward = itemReward
				};
				factionRewardAnnounceInfo.Level = levelObtained;
				factionRewardAnnounceInfo.FactionSpritePath = string.Empty;
				factionRewardAnnounceInfo.DisplayDescription = displayDescription;
				this.m_trustRewardsToAnnounce.Add(factionRewardAnnounceInfo);
			}
		}
		using (List<QuestUnlockReward>.Enumerator enumerator2 = rewards.UnlockRewards.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				QuestUnlockReward unlockReward = enumerator2.Current;
				UINewReward.FactionRewardAnnounceInfo factionRewardAnnounceInfo2 = new UINewReward.FactionRewardAnnounceInfo();
				factionRewardAnnounceInfo2.FactionReward = new FactionUnlockReward
				{
					UnlockReward = unlockReward
				};
				factionRewardAnnounceInfo2.Level = levelObtained;
				factionRewardAnnounceInfo2.FactionSpritePath = string.Empty;
				factionRewardAnnounceInfo2.DisplayDescription = displayDescription;
				this.m_trustRewardsToAnnounce.Add(factionRewardAnnounceInfo2);
			}
		}
		using (List<QuestCurrencyReward>.Enumerator enumerator3 = rewards.CurrencyRewards.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				QuestCurrencyReward currencyReward = enumerator3.Current;
				UINewReward.FactionRewardAnnounceInfo factionRewardAnnounceInfo3 = new UINewReward.FactionRewardAnnounceInfo();
				factionRewardAnnounceInfo3.FactionReward = new FactionCurrencyReward
				{
					CurrencyReward = currencyReward
				};
				factionRewardAnnounceInfo3.Level = levelObtained;
				factionRewardAnnounceInfo3.FactionSpritePath = string.Empty;
				factionRewardAnnounceInfo3.DisplayDescription = true;
				this.m_trustRewardsToAnnounce.Add(factionRewardAnnounceInfo3);
			}
		}
	}

	private void HandleQuestCompleteNotification(QuestCompleteNotification message)
	{
		QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(message.questId);
		if (questTemplate == null || !questTemplate.DisplayRewardNotification)
		{
			return;
		}
		this.NotifyNewQuestReward(questTemplate.Rewards, -1, true);
	}

	private void SetupReward(UINewReward.RewardAnnouncementDisplayInfo info)
	{
		string text = info.m_rewardData.Name;
		string text2 = string.Empty;
		if (info.m_rewardData.Type != RewardUtils.RewardType.ISO)
		{
			if (info.m_rewardData.Type != RewardUtils.RewardType.ModToken)
			{
				if (info.m_rewardData.Type != RewardUtils.RewardType.GGBoost)
				{
					if (info.m_rewardData.Type != RewardUtils.RewardType.System)
					{
						text2 = RewardUtils.GetTypeDisplayString(info.m_rewardData.Type, false);
					}
				}
			}
		}
		string text3 = string.Empty;
		if (info.m_unlockLevel > 0)
		{
			text3 = string.Format(StringUtil.TR("LevelRequirement", "Rewards"), info.m_unlockLevel);
		}
		if (info.m_unlockCharType != CharacterType.None)
		{
			text3 = text3 + " " + GameWideData.Get().GetCharacterResourceLink(info.m_unlockCharType).GetDisplayName();
		}
		else
		{
			text3 = text3 + " " + info.m_displayName;
		}
		text = RewardUtils.GetDisplayString(info.m_rewardData, true);
		if (info.m_rewardData.SpritePath == null)
		{
			this.m_rewardIcon.sprite = this.m_generalSprite;
		}
		else
		{
			Sprite sprite = (Sprite)Resources.Load(info.m_rewardData.SpritePath, typeof(Sprite));
			if (info.m_rewardData.Type == RewardUtils.RewardType.Banner)
			{
				UIManager.SetGameObjectActive(this.m_rewardIcon, false, null);
				UIManager.SetGameObjectActive(this.m_rewardBanner, true, null);
				this.m_rewardBanner.sprite = sprite;
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_rewardIcon, true, null);
				UIManager.SetGameObjectActive(this.m_rewardBanner, false, null);
				this.m_rewardIcon.sprite = sprite;
			}
		}
		if (info.m_rewardData.Foreground == null)
		{
			UIManager.SetGameObjectActive(this.m_rewardFG, false, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_rewardFG, true, null);
			this.m_rewardFG.sprite = info.m_rewardData.Foreground;
		}
		this.m_rewardRequirements.text = text3;
		this.m_rewardName.text = text;
		this.m_rewardType.text = text2;
	}

	private void SetupSeasonReward(List<UINewReward.SeasonReward> rewards)
	{
		for (int i = 0; i < this.m_seasonRewards.Length; i++)
		{
			if (i >= rewards.Count)
			{
				UIManager.SetGameObjectActive(this.m_seasonRewards[i], false, null);
			}
			else
			{
				this.m_seasonRewards[i].SetupHack(rewards[i].m_itemTemplate, InventoryWideData.GetSpritePath(rewards[i].m_itemTemplate), rewards[i].m_amount);
				UIManager.SetGameObjectActive(this.m_seasonRewards[i], true, null);
			}
		}
	}

	private void SetupTrustReward(UINewReward.FactionRewardAnnounceInfo info)
	{
		this.m_trustRewardTierLevel.text = string.Format(StringUtil.TR("PersonalTierLevel", "TrustWar"), info.Level);
		Sprite sprite = Resources.Load(info.FactionSpritePath, typeof(Sprite)) as Sprite;
		UIManager.SetGameObjectActive(this.m_FactionContainer, sprite != null, null);
		UIManager.SetGameObjectActive(this.m_TierContainer, sprite != null, null);
		UIManager.SetGameObjectActive(this.m_trustFactionIcon, sprite != null, null);
		this.m_trustFactionIcon.sprite = sprite;
		UIManager.SetGameObjectActive(this.m_trustRewardDescription, info.DisplayDescription, null);
		FactionItemReward factionItemReward = info.FactionReward as FactionItemReward;
		if (factionItemReward != null)
		{
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(factionItemReward.ItemReward.ItemTemplateId);
			this.m_trustRewardName.text = itemTemplate.GetDisplayName();
			this.m_trustRewardDescription.text = itemTemplate.GetObtainDescription();
			this.m_trustRewardIcon.sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate));
		}
		FactionUnlockReward factionUnlockReward = info.FactionReward as FactionUnlockReward;
		if (factionUnlockReward != null)
		{
			this.m_trustRewardIcon.sprite = Resources.Load<Sprite>(factionUnlockReward.UnlockReward.resourceString);
			this.m_trustRewardName.text = RewardUtils.GetRewardDisplayName(factionUnlockReward.UnlockReward.purchaseType, factionUnlockReward.UnlockReward.typeSpecificData);
			this.m_trustRewardDescription.text = StringUtil.TR("Unlocked", "OverlayScreensScene");
		}
		FactionCurrencyReward factionCurrencyReward = info.FactionReward as FactionCurrencyReward;
		if (factionCurrencyReward != null)
		{
			this.m_trustRewardIcon.sprite = Resources.Load<Sprite>(RewardUtils.GetCurrencyIconPath(factionCurrencyReward.CurrencyReward.Type));
			this.m_trustRewardName.text = factionCurrencyReward.CurrencyReward.Amount.ToString();
			RewardUtils.RewardType type = RewardUtils.RewardType.ISO;
			CurrencyType type2 = factionCurrencyReward.CurrencyReward.Type;
			switch (type2)
			{
			case CurrencyType.ISO:
				type = RewardUtils.RewardType.ISO;
				break;
			case CurrencyType.ModToken:
				type = RewardUtils.RewardType.ModToken;
				break;
			default:
				if (type2 != CurrencyType.FreelancerCurrency)
				{
					if (type2 != CurrencyType.UnlockFreelancerToken)
					{
					}
					else
					{
						type = RewardUtils.RewardType.UnlockFreelancerToken;
					}
				}
				else
				{
					type = RewardUtils.RewardType.FreelancerCurrency;
				}
				break;
			case CurrencyType.GGPack:
				type = RewardUtils.RewardType.GGBoost;
				break;
			}
			this.m_trustRewardDescription.text = RewardUtils.GetTypeDisplayString(type, false);
		}
	}

	public bool RewardIsBeingAnnounced()
	{
		bool result;
		if (!this.m_rewardAnnouncementInProgress)
		{
			result = (this.rewardTypesToAnnounce.Count > 0);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public void AutoPlayNextAnimation()
	{
		this.m_autoPlayNextReward = true;
	}

	public void Update()
	{
		if (this.m_chapterNotifications.IsCurrentlyDisplaying())
		{
			return;
		}
		if (this.m_rewardAnnouncementInProgress)
		{
			float num = Time.time - this.m_rewardStartTime;
			if (!this.m_autoPlayNextReward)
			{
				if (num >= this.m_timeBeforeCanCloseReward)
				{
					if (Input.GetMouseButtonDown(0))
					{
						goto IL_128;
					}
				}
				if (this.m_container.gameObject.activeInHierarchy)
				{
					if (num > this.m_animationController.GetCurrentAnimatorStateInfo(0).length)
					{
						goto IL_128;
					}
				}
				if (this.m_seasonContainer.gameObject.activeInHierarchy)
				{
					if (num > this.m_seasonAnimationController.GetCurrentAnimatorStateInfo(0).length)
					{
						goto IL_128;
					}
				}
				if (!this.m_trustContainer.gameObject.activeInHierarchy || num <= this.m_trustAnimationController.GetCurrentAnimatorStateInfo(0).length)
				{
					goto IL_170;
				}
			}
			IL_128:
			this.m_rewardAnnouncementInProgress = false;
			if (this.rewardTypesToAnnounce.Count <= 0)
			{
				UIManager.SetGameObjectActive(this.m_container, false, null);
				UIManager.SetGameObjectActive(this.m_seasonContainer, false, null);
				UIManager.SetGameObjectActive(this.m_trustContainer, false, null);
			}
			IL_170:;
		}
		else
		{
			if (this.rewardTypesToAnnounce.Count <= 0)
			{
				if (this.m_seasonRewardsToAnnounce.Count <= 0)
				{
					if (this.m_trustRewardsToAnnounce.Count <= 0)
					{
						goto IL_3CC;
					}
				}
			}
			this.m_autoPlayNextReward = false;
			if (this.m_seasonRewardsToAnnounce.Count > 0)
			{
				int unlockLevel = this.m_seasonRewardsToAnnounce[0].m_unlockLevel;
				List<UINewReward.SeasonReward> list = new List<UINewReward.SeasonReward>();
				for (int i = this.m_seasonRewardsToAnnounce.Count - 1; i >= 0; i--)
				{
					if (this.m_seasonRewardsToAnnounce[i].m_unlockLevel == unlockLevel)
					{
						list.Add(this.m_seasonRewardsToAnnounce[i]);
						this.m_seasonRewardsToAnnounce.RemoveAt(i);
						if (list.Count >= this.m_seasonRewards.Length)
						{
							IL_282:
							this.SetupSeasonReward(list);
							UIManager.SetGameObjectActive(this.m_seasonContainer, true, null);
							UIManager.SetGameObjectActive(this.m_container, false, null);
							UIManager.SetGameObjectActive(this.m_trustContainer, false, null);
							this.m_seasonAnimationController.Play("NewReward", -1, 0f);
							goto IL_3A0;
						}
					}
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					goto IL_282;
				}
			}
			else if (this.m_trustRewardsToAnnounce.Count > 0)
			{
				this.SetupTrustReward(this.m_trustRewardsToAnnounce[0]);
				UIManager.SetGameObjectActive(this.m_seasonContainer, false, null);
				UIManager.SetGameObjectActive(this.m_container, false, null);
				UIManager.SetGameObjectActive(this.m_trustContainer, true, null);
				this.m_trustAnimationController.Play("NewTrustRewardDefaultIN", -1, 0f);
				this.m_trustRewardsToAnnounce.RemoveAt(0);
			}
			else
			{
				this.SetupReward(this.rewardTypesToAnnounce[0]);
				UIManager.SetGameObjectActive(this.m_container, true, null);
				UIManager.SetGameObjectActive(this.m_seasonContainer, false, null);
				UIManager.SetGameObjectActive(this.m_trustContainer, false, null);
				this.m_animationController.Play("NewReward", -1, 0f);
				this.rewardTypesToAnnounce.RemoveAt(0);
			}
			IL_3A0:
			UIScreenManager.Get().EndAllLoopSounds();
			AudioManager.PostEvent("ui/endgame/unlock", null);
			this.m_rewardAnnouncementInProgress = true;
			this.m_rewardStartTime = Time.time;
		}
		IL_3CC:
		if (this.m_rewardAnnouncementInProgress)
		{
			UIManager.SetGameObjectActive(this.m_frontendContainer, UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_frontendContainer, false, null);
		}
	}

	public void Clear()
	{
		this.m_rewardAnnouncementInProgress = false;
		UIManager.SetGameObjectActive(this.m_container, false, null);
		UIManager.SetGameObjectActive(this.m_seasonContainer, false, null);
		UIManager.SetGameObjectActive(this.m_trustContainer, false, null);
		this.rewardTypesToAnnounce.Clear();
	}

	public bool IsActive()
	{
		bool result;
		if (this.rewardTypesToAnnounce.Count <= 0)
		{
			result = this.m_rewardAnnouncementInProgress;
		}
		else
		{
			result = true;
		}
		return result;
	}

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
}
