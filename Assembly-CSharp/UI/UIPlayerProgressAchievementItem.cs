using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerProgressAchievementItem : MonoBehaviour
{
	public LayoutElement m_layoutElement;

	public _SelectableBtn m_selectableBtn;

	public Image m_icon;

	public TextMeshProUGUI m_description;

	public TextMeshProUGUI m_dateCompleted;

	public TextMeshProUGUI m_points;

	public QuestReward m_questReward;

	public TextMeshProUGUI m_progressText;

	public Image m_progressBar;

	public Image m_completedBar;

	public RectTransform m_expandedContainer;

	public RectTransform m_expandDownArrow;

	public float m_defaultHeight;

	public float m_expandedHeight;

	private bool m_isInitialized;

	private bool m_isExpanded;

	private bool m_hasExpandData;

	private Sprite m_defaultSprite;

	private QuestTemplate m_quest;

	private TextMeshProUGUI[] m_expandedTexts;

	private string m_datePattern;

	private void Init()
	{
		if (m_isInitialized)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_isInitialized = true;
		m_selectableBtn.spriteController.callback = delegate
		{
			m_isExpanded = !m_isExpanded;
			UpdateExpandedVisuals();
		};
		m_defaultSprite = m_icon.sprite;
		m_expandedTexts = m_expandedContainer.GetComponentsInChildren<TextMeshProUGUI>(true);
		_SelectableBtn component = m_questReward.GetComponent<_SelectableBtn>();
		m_selectableBtn.spriteController.AddSubButton(component.spriteController);
		string currentLanguagecode = StringUtil.GetCurrentLanguagecode();
		CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(currentLanguagecode);
		m_datePattern = cultureInfo.DateTimeFormat.ShortDatePattern.Replace("yyyy", "yy");
	}

	public int Setup(QuestTemplate quest, Dictionary<int, QuestMetaData> questMetaData)
	{
		Init();
		m_quest = quest;
		m_selectableBtn.SetSelected(false, false, string.Empty, string.Empty);
		m_isExpanded = false;
		List<string> list = new List<string>();
		foreach (QuestObjective objective in m_quest.Objectives)
		{
			using (List<QuestTrigger>.Enumerator enumerator2 = objective.Triggers.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					QuestTrigger current2 = enumerator2.Current;
					foreach (QuestCondition condition in current2.Conditions)
					{
						if (condition.ConditionType == QuestConditionType.HasCharacterLevel)
						{
							m_hasExpandData = true;
							CharacterType typeSpecificData = (CharacterType)condition.typeSpecificData;
							int typeSpecificData2 = condition.typeSpecificData2;
							if (ClientGameManager.Get().GetPlayerCharacterData(typeSpecificData).ExperienceComponent.Level >= typeSpecificData2)
							{
								string displayName = typeSpecificData.GetDisplayName();
								if (!list.Contains(displayName))
								{
									list.Add(displayName);
								}
							}
						}
					}
				}
			}
		}
		bool flag = false;
		int num = 0;
		int num2 = 0;
		string currentLanguagecode = StringUtil.GetCurrentLanguagecode();
		while (quest != null)
		{
			if (!flag)
			{
				if (questMetaData.ContainsKey(quest.Index))
				{
					QuestMetaData questMetaData2 = questMetaData[quest.Index];
					flag = (questMetaData2.CompletedCount > 0);
					CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(currentLanguagecode);
					m_dateCompleted.text = questMetaData2.UtcFirstCompleted.ToLocalTime().ToString(m_datePattern, cultureInfo.DateTimeFormat);
				}
			}
			if (flag)
			{
				num += quest.AchievementPoints;
				num2 += quest.AchievementPoints;
				if (quest != m_quest)
				{
					string text = StringUtil.TR_QuestName(quest.Index);
					if (text.IsNullOrEmpty())
					{
						text = StringUtil.TR_QuestDescription(quest.Index);
					}
					string text2 = text;
					text = text2 + " [" + quest.AchievementPoints + "]";
					list.Add(text);
				}
			}
			else
			{
				m_quest = quest;
				num2 = quest.AchievementPoints;
			}
			if (quest.AchievementPrevious > 0)
			{
				quest = QuestWideData.Get().GetQuestTemplate(quest.AchievementPrevious);
			}
			else
			{
				quest = null;
			}
		}
		while (true)
		{
			m_points.text = num2.ToString();
			if (m_quest != null && questMetaData.ContainsKey(m_quest.Index))
			{
				flag = (questMetaData[m_quest.Index].CompletedCount > 0);
			}
			else
			{
				flag = false;
			}
			string text3 = StringUtil.TR_QuestName(m_quest.Index);
			string text4 = StringUtil.TR_QuestDescription(m_quest.Index);
			if (text3 != string.Empty)
			{
				m_description.text = $"<size=20>{text3}</size>\n<#a9a9a9>{text4}";
			}
			else
			{
				m_description.text = text4;
			}
			Sprite sprite;
			if (flag)
			{
				sprite = Resources.Load<Sprite>(m_quest.IconFilename);
			}
			else
			{
				sprite = Resources.Load<Sprite>(m_quest.UnfinishedIconFilename);
			}
			if ((bool)sprite)
			{
				m_icon.sprite = sprite;
			}
			else
			{
				m_icon.sprite = m_defaultSprite;
			}
			UIManager.SetGameObjectActive(m_dateCompleted, flag);
			int currentProgress = 0;
			int maxProgress = 0;
			QuestItem.GetQuestProgress(m_quest.Index, out currentProgress, out maxProgress);
			if (flag)
			{
				currentProgress = maxProgress;
			}
			string text5 = $"{currentProgress}/{maxProgress}";
			m_progressText.text = text5;
			m_progressBar.fillAmount = (float)currentProgress / (1f * (float)maxProgress);
			UIManager.SetGameObjectActive(m_progressBar, !flag);
			UIManager.SetGameObjectActive(m_completedBar, flag);
			SetupQuestReward(m_quest);
			m_hasExpandData = (list.Count > 0);
			if (m_hasExpandData)
			{
				for (int i = 0; i < m_expandedTexts.Length; i++)
				{
					UIManager.SetGameObjectActive(m_expandedTexts[i], i < list.Count);
					if (i < list.Count)
					{
						UIManager.SetGameObjectActive(m_expandedTexts[i], true);
						m_expandedTexts[i].text = "- " + list[i];
					}
				}
			}
			UpdateExpandedVisuals();
			m_selectableBtn.spriteController.SetClickable(m_hasExpandData);
			return num;
		}
	}

	private void SetupQuestReward(QuestTemplate quest)
	{
		bool flag = SetupReward(quest.Rewards);
		if (!flag)
		{
			if (quest.ConditionalRewards != null)
			{
				int num = 0;
				while (true)
				{
					if (num < quest.ConditionalRewards.Length)
					{
						if (QuestWideData.AreConditionsMet(quest.ConditionalRewards[num].Prerequisites.Conditions, quest.ConditionalRewards[num].Prerequisites.LogicStatement))
						{
							flag = SetupReward(quest.ConditionalRewards[num]);
							if (flag)
							{
								break;
							}
						}
						num++;
						continue;
					}
					break;
				}
			}
		}
		UIManager.SetGameObjectActive(m_questReward, flag);
	}

	private bool SetupReward(QuestRewards questRewards)
	{
		if (questRewards.CurrencyRewards.Count > 0)
		{
			m_questReward.Setup(questRewards.CurrencyRewards[0], 0);
		}
		else if (questRewards.UnlockRewards.Count > 0)
		{
			m_questReward.SetupHack(questRewards.UnlockRewards[0].resourceString);
		}
		else
		{
			if (questRewards.ItemRewards.Count <= 0)
			{
				return false;
			}
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(questRewards.ItemRewards[0].ItemTemplateId);
			m_questReward.SetupHack(itemTemplate, itemTemplate.IconPath);
		}
		return true;
	}

	private void UpdateExpandedVisuals()
	{
		if (m_isExpanded && !m_hasExpandData)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		LayoutElement layoutElement = m_layoutElement;
		float preferredHeight;
		if (m_isExpanded)
		{
			preferredHeight = m_expandedHeight;
		}
		else
		{
			preferredHeight = m_defaultHeight;
		}
		layoutElement.preferredHeight = preferredHeight;
		UIManager.SetGameObjectActive(m_expandedContainer, m_isExpanded);
		UIManager.SetGameObjectActive(m_expandDownArrow, !m_isExpanded);
	}
}
