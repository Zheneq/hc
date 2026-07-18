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

	public bool Clickable
	{
		get;
		set;
	}

	private void Start()
	{
		m_defaultSprite = m_contractIcon.sprite;
	}

	private void Awake()
	{
		Clickable = true;
		m_BonusAnimator = base.gameObject.GetComponent<Animator>();
		_SelectableBtn component = GetComponent<_SelectableBtn>();
		component.spriteController.callback = QuestButtonClicked;
		for (int i = 0; i < m_questRewards.Length; i++)
		{
			_SelectableBtn component2 = m_questRewards[i].GetComponent<_SelectableBtn>();
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
		m_bonusText.text = $"{Mathf.Min(questMaxRejectPercentage, (m_rejectedCount + 1) * questBonusPerRejection)}%";
		QuestTemplate questTemplate = QuestWideData.Get().m_quests[m_questId - 1];
		QuestRewards rewards = questTemplate.Rewards;
		int count = rewards.CurrencyRewards.Count;
		int count2 = rewards.UnlockRewards.Count;
		int count3 = rewards.ItemRewards.Count;
		if (count + count2 + count3 != 0)
		{
			for (int i = 0; i < m_questRewards.Length; i++)
			{
				if (i < count)
				{
					m_questRewards[i].Setup(rewards.CurrencyRewards[i], m_rejectedCount + 1);
				}
			}
		}
		m_rejectedCount = -1;
	}

	public bool NotifyRejectedQuest()
	{
		bool result = false;
		if (m_BonusAnimator == null)
		{
			m_rejectedCount = -1;
		}
		if (m_rejectedCount > -1)
		{
			if (base.gameObject.activeInHierarchy)
			{
				int questMaxRejectPercentage = QuestWideData.Get().m_questMaxRejectPercentage;
				int questBonusPerRejection = QuestWideData.Get().m_questBonusPerRejection;
				float num = m_rejectedCount * questBonusPerRejection;
				if (m_rejectedCount == 0)
				{
					UIManager.SetGameObjectActive(m_bonus, true);
					UIManager.SetGameObjectActive(m_bonusArrowsMask, true);
					IncreaseRejectBonusNumber();
					UIAnimationEventManager.Get().PlayAnimation(m_BonusAnimator, "PickContractItemBonusIN", RejectAnimDone, "PickContractItemBonusIDLE", 1);
					result = true;
				}
				else if (num < (float)questMaxRejectPercentage)
				{
					UIAnimationEventManager.Get().PlayAnimation(m_BonusAnimator, "PickContractItemBonusOUT", RejectAnimDone, "PickContractItemBonusIDLE", 1);
					result = true;
				}
			}
		}
		return result;
	}

	public void QuestButtonClicked(BaseEventData data)
	{
		if (!Clickable)
		{
			return;
		}
		while (true)
		{
			m_BonusAnimator.Play("PickContractItemSelectIN", 1, 0f);
			ClientGameManager.Get().SelectDailyQuest(m_questId);
			QuestOfferPanel.Get().NotifyOfferClicked(this);
			return;
		}
	}

	public void HandleSelectDailyQuestResponse(PickDailyQuestResponse response)
	{
		if (!response.Success)
		{
			return;
		}
		while (true)
		{
			QuestListPanel.Get().HandleQuestAdded(m_questId);
			return;
		}
	}

	public void SetupDailyQuest(int questId, int rejectedCount)
	{
		Setup(questId, rejectedCount);
	}

	private void Setup(int questId, int rejectedCount)
	{
		m_questId = questId;
		QuestTemplate questTemplate = QuestWideData.Get().m_quests[questId - 1];
		string empty = string.Empty;
		string text = StringUtil.TR_QuestName(questId);
		string text2 = StringUtil.TR_QuestDescription(questId);
		empty = ((!(text != string.Empty)) ? text2 : $"<size=48>{text}</size>\n{text2}");
		m_contractText.text = empty;
		Sprite sprite = (Sprite)Resources.Load(questTemplate.IconFilename, typeof(Sprite));
		if ((bool)sprite)
		{
			m_contractIcon.sprite = sprite;
		}
		else
		{
			m_contractIcon.sprite = m_defaultSprite;
		}
		m_rejectedCount = rejectedCount;
		if (rejectedCount > 0)
		{
			int questMaxRejectPercentage = QuestWideData.Get().m_questMaxRejectPercentage;
			int questBonusPerRejection = QuestWideData.Get().m_questBonusPerRejection;
			UIManager.SetGameObjectActive(m_bonus, true);
			m_bonusText.text = $"{Mathf.Min(questMaxRejectPercentage, rejectedCount * questBonusPerRejection)}%";
			UIManager.SetGameObjectActive(m_bonusArrowsMask, true);
		}
		else
		{
			UIManager.SetGameObjectActive(m_bonus, false);
			UIManager.SetGameObjectActive(m_bonusArrowsMask, false);
		}
		QuestRewards rewards = questTemplate.Rewards;
		int count = rewards.CurrencyRewards.Count;
		int count2 = rewards.UnlockRewards.Count;
		int count3 = rewards.ItemRewards.Count;
		if (count + count2 + count3 == 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_rewardGrid, false);
					UIManager.SetGameObjectActive(m_rewardText, false);
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_rewardGrid, true);
		UIManager.SetGameObjectActive(m_rewardText, true);
		for (int i = 0; i < m_questRewards.Length; i++)
		{
			if (i < count)
			{
				UIManager.SetGameObjectActive(m_questRewards[i], true);
				m_questRewards[i].Setup(rewards.CurrencyRewards[i], rejectedCount);
			}
			else if (i - count < count2)
			{
				UIManager.SetGameObjectActive(m_questRewards[i], true);
				m_questRewards[i].SetupHack(rewards.UnlockRewards[i - count].resourceString);
			}
			else if (i - count - count2 < count3)
			{
				QuestItemReward questItemReward = rewards.ItemRewards[i - count - count2];
				InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(questItemReward.ItemTemplateId);
				UIManager.SetGameObjectActive(m_questRewards[i], true);
				QuestReward obj = m_questRewards[i];
				string iconPath = itemTemplate.IconPath;
				int amount;
				if (itemTemplate.Type == InventoryItemType.Experience)
				{
					amount = itemTemplate.TypeSpecificData[0];
				}
				else
				{
					amount = questItemReward.Amount;
				}
				obj.SetupHack(itemTemplate, iconPath, amount);
			}
			else
			{
				UIManager.SetGameObjectActive(m_questRewards[i], false);
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
}
