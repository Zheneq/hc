using System;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestOffer : MonoBehaviour
{
	public TextMeshProUGUI m_contractText;

	public GameObject m_bonus;

	public TextMeshProUGUI m_bonusText;

	public GameObject m_bonusArrowsMask;

	public TextMeshProUGUI m_rewardText;

	public GridLayoutGroup m_rewardGrid;

	public Image m_contractIcon;

	private Sprite m_defaultSprite;

	public QuestReward[] m_questRewards;

	private int m_questId;

	private Animator m_BonusAnimator;

	private int m_rejectedCount = -1;

	public bool Clickable { get; set; }

	private void Start()
	{
		this.m_defaultSprite = this.m_contractIcon.sprite;
	}

	private void Awake()
	{
		this.Clickable = true;
		this.m_BonusAnimator = base.gameObject.GetComponent<Animator>();
		_SelectableBtn component = base.GetComponent<_SelectableBtn>();
		component.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.QuestButtonClicked);
		for (int i = 0; i < this.m_questRewards.Length; i++)
		{
			_SelectableBtn component2 = this.m_questRewards[i].GetComponent<_SelectableBtn>();
			component.spriteController.AddSubButton(component2.spriteController);
		}
	}

	public void RejectAnimDone()
	{
		QuestOfferPanel.Get().SetVisible(false);
	}

	public void IncreaseRejectBonusNumber()
	{
		int questMaxRejectPercentage = QuestWideData.Get().m_questMaxRejectPercentage;
		int questBonusPerRejection = QuestWideData.Get().m_questBonusPerRejection;
		this.m_bonusText.text = string.Format("{0}%", Mathf.Min(questMaxRejectPercentage, (this.m_rejectedCount + 1) * questBonusPerRejection));
		QuestTemplate questTemplate = QuestWideData.Get().m_quests[this.m_questId - 1];
		QuestRewards rewards = questTemplate.Rewards;
		int count = rewards.CurrencyRewards.Count;
		int count2 = rewards.UnlockRewards.Count;
		int count3 = rewards.ItemRewards.Count;
		if (count + count2 + count3 != 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOffer.IncreaseRejectBonusNumber()).MethodHandle;
			}
			for (int i = 0; i < this.m_questRewards.Length; i++)
			{
				if (i < count)
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
					this.m_questRewards[i].Setup(rewards.CurrencyRewards[i], this.m_rejectedCount + 1);
				}
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
		this.m_rejectedCount = -1;
	}

	public bool NotifyRejectedQuest()
	{
		bool result = false;
		if (this.m_BonusAnimator == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOffer.NotifyRejectedQuest()).MethodHandle;
			}
			this.m_rejectedCount = -1;
		}
		if (this.m_rejectedCount > -1)
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
			if (base.gameObject.activeInHierarchy)
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
				int questMaxRejectPercentage = QuestWideData.Get().m_questMaxRejectPercentage;
				int questBonusPerRejection = QuestWideData.Get().m_questBonusPerRejection;
				float num = (float)(this.m_rejectedCount * questBonusPerRejection);
				if (this.m_rejectedCount == 0)
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
					UIManager.SetGameObjectActive(this.m_bonus, true, null);
					UIManager.SetGameObjectActive(this.m_bonusArrowsMask, true, null);
					this.IncreaseRejectBonusNumber();
					UIAnimationEventManager.Get().PlayAnimation(this.m_BonusAnimator, "PickContractItemBonusIN", new UIAnimationEventManager.AnimationDoneCallback(this.RejectAnimDone), "PickContractItemBonusIDLE", 1, 0f, true, false, null, null);
					result = true;
				}
				else if (num < (float)questMaxRejectPercentage)
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
					UIAnimationEventManager.Get().PlayAnimation(this.m_BonusAnimator, "PickContractItemBonusOUT", new UIAnimationEventManager.AnimationDoneCallback(this.RejectAnimDone), "PickContractItemBonusIDLE", 1, 0f, true, false, null, null);
					result = true;
				}
			}
		}
		return result;
	}

	public void QuestButtonClicked(BaseEventData data)
	{
		if (this.Clickable)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOffer.QuestButtonClicked(BaseEventData)).MethodHandle;
			}
			this.m_BonusAnimator.Play("PickContractItemSelectIN", 1, 0f);
			ClientGameManager.Get().SelectDailyQuest(this.m_questId, null);
			QuestOfferPanel.Get().NotifyOfferClicked(this);
		}
	}

	public void HandleSelectDailyQuestResponse(PickDailyQuestResponse response)
	{
		if (response.Success)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOffer.HandleSelectDailyQuestResponse(PickDailyQuestResponse)).MethodHandle;
			}
			QuestListPanel.Get().HandleQuestAdded(this.m_questId);
		}
	}

	public void SetupDailyQuest(int questId, int rejectedCount)
	{
		this.Setup(questId, rejectedCount);
	}

	private void Setup(int questId, int rejectedCount)
	{
		this.m_questId = questId;
		QuestTemplate questTemplate = QuestWideData.Get().m_quests[questId - 1];
		string text = string.Empty;
		string text2 = StringUtil.TR_QuestName(questId);
		string text3 = StringUtil.TR_QuestDescription(questId);
		if (text2 != string.Empty)
		{
			text = string.Format("<size=48>{0}</size>\n{1}", text2, text3);
		}
		else
		{
			text = text3;
		}
		this.m_contractText.text = text;
		Sprite sprite = (Sprite)Resources.Load(questTemplate.IconFilename, typeof(Sprite));
		if (sprite)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOffer.Setup(int, int)).MethodHandle;
			}
			this.m_contractIcon.sprite = sprite;
		}
		else
		{
			this.m_contractIcon.sprite = this.m_defaultSprite;
		}
		this.m_rejectedCount = rejectedCount;
		if (rejectedCount > 0)
		{
			int questMaxRejectPercentage = QuestWideData.Get().m_questMaxRejectPercentage;
			int questBonusPerRejection = QuestWideData.Get().m_questBonusPerRejection;
			UIManager.SetGameObjectActive(this.m_bonus, true, null);
			this.m_bonusText.text = string.Format("{0}%", Mathf.Min(questMaxRejectPercentage, rejectedCount * questBonusPerRejection));
			UIManager.SetGameObjectActive(this.m_bonusArrowsMask, true, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_bonus, false, null);
			UIManager.SetGameObjectActive(this.m_bonusArrowsMask, false, null);
		}
		QuestRewards rewards = questTemplate.Rewards;
		int count = rewards.CurrencyRewards.Count;
		int count2 = rewards.UnlockRewards.Count;
		int count3 = rewards.ItemRewards.Count;
		if (count + count2 + count3 == 0)
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
			UIManager.SetGameObjectActive(this.m_rewardGrid, false, null);
			UIManager.SetGameObjectActive(this.m_rewardText, false, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_rewardGrid, true, null);
			UIManager.SetGameObjectActive(this.m_rewardText, true, null);
			for (int i = 0; i < this.m_questRewards.Length; i++)
			{
				if (i < count)
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
					UIManager.SetGameObjectActive(this.m_questRewards[i], true, null);
					this.m_questRewards[i].Setup(rewards.CurrencyRewards[i], rejectedCount);
				}
				else if (i - count < count2)
				{
					UIManager.SetGameObjectActive(this.m_questRewards[i], true, null);
					this.m_questRewards[i].SetupHack(rewards.UnlockRewards[i - count].resourceString, 0);
				}
				else if (i - count - count2 < count3)
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
					QuestItemReward questItemReward = rewards.ItemRewards[i - count - count2];
					InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(questItemReward.ItemTemplateId);
					UIManager.SetGameObjectActive(this.m_questRewards[i], true, null);
					QuestReward questReward = this.m_questRewards[i];
					InventoryItemTemplate itemTemplate2 = itemTemplate;
					string iconPath = itemTemplate.IconPath;
					int amount;
					if (itemTemplate.Type == InventoryItemType.Experience)
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
						amount = itemTemplate.TypeSpecificData[0];
					}
					else
					{
						amount = questItemReward.Amount;
					}
					questReward.SetupHack(itemTemplate2, iconPath, amount);
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_questRewards[i], false, null);
				}
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
	}
}
