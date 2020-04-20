using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
		if (this.m_isInitialized)
		{
			return;
		}
		this.m_isInitialized = true;
		this.m_selectableBtn.spriteController.callback = delegate(BaseEventData data)
		{
			this.m_isExpanded = !this.m_isExpanded;
			this.UpdateExpandedVisuals();
		};
		this.m_defaultSprite = this.m_icon.sprite;
		this.m_expandedTexts = this.m_expandedContainer.GetComponentsInChildren<TextMeshProUGUI>(true);
		_SelectableBtn component = this.m_questReward.GetComponent<_SelectableBtn>();
		this.m_selectableBtn.spriteController.AddSubButton(component.spriteController);
		string currentLanguagecode = StringUtil.GetCurrentLanguagecode();
		CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(currentLanguagecode);
		this.m_datePattern = cultureInfo.DateTimeFormat.ShortDatePattern.Replace("yyyy", "yy");
	}

	public int Setup(QuestTemplate quest, Dictionary<int, QuestMetaData> questMetaData)
	{
		this.Init();
		this.m_quest = quest;
		this.m_selectableBtn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_isExpanded = false;
		List<string> list = new List<string>();
		foreach (QuestObjective questObjective in this.m_quest.Objectives)
		{
			using (List<QuestTrigger>.Enumerator enumerator2 = questObjective.Triggers.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					QuestTrigger questTrigger = enumerator2.Current;
					foreach (QuestCondition questCondition in questTrigger.Conditions)
					{
						if (questCondition.ConditionType == QuestConditionType.HasCharacterLevel)
						{
							this.m_hasExpandData = true;
							CharacterType typeSpecificData = (CharacterType)questCondition.typeSpecificData;
							int typeSpecificData2 = questCondition.typeSpecificData2;
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
					this.m_dateCompleted.text = questMetaData2.UtcFirstCompleted.ToLocalTime().ToString(this.m_datePattern, cultureInfo.DateTimeFormat);
				}
			}
			if (flag)
			{
				num += quest.AchievementPoints;
				num2 += quest.AchievementPoints;
				if (quest != this.m_quest)
				{
					string text = StringUtil.TR_QuestName(quest.Index);
					if (text.IsNullOrEmpty())
					{
						text = StringUtil.TR_QuestDescription(quest.Index);
					}
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						" [",
						quest.AchievementPoints,
						"]"
					});
					list.Add(text);
				}
			}
			else
			{
				this.m_quest = quest;
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
		this.m_points.text = num2.ToString();
		if (this.m_quest != null && questMetaData.ContainsKey(this.m_quest.Index))
		{
			flag = (questMetaData[this.m_quest.Index].CompletedCount > 0);
		}
		else
		{
			flag = false;
		}
		string text3 = StringUtil.TR_QuestName(this.m_quest.Index);
		string text4 = StringUtil.TR_QuestDescription(this.m_quest.Index);
		if (text3 != string.Empty)
		{
			this.m_description.text = string.Format("<size=20>{0}</size>\n<#a9a9a9>{1}", text3, text4);
		}
		else
		{
			this.m_description.text = text4;
		}
		Sprite sprite;
		if (flag)
		{
			sprite = Resources.Load<Sprite>(this.m_quest.IconFilename);
		}
		else
		{
			sprite = Resources.Load<Sprite>(this.m_quest.UnfinishedIconFilename);
		}
		if (sprite)
		{
			this.m_icon.sprite = sprite;
		}
		else
		{
			this.m_icon.sprite = this.m_defaultSprite;
		}
		UIManager.SetGameObjectActive(this.m_dateCompleted, flag, null);
		int num3 = 0;
		int num4 = 0;
		QuestItem.GetQuestProgress(this.m_quest.Index, out num3, out num4);
		if (flag)
		{
			num3 = num4;
		}
		string text5 = string.Format("{0}/{1}", num3, num4);
		this.m_progressText.text = text5;
		this.m_progressBar.fillAmount = (float)num3 / (1f * (float)num4);
		UIManager.SetGameObjectActive(this.m_progressBar, !flag, null);
		UIManager.SetGameObjectActive(this.m_completedBar, flag, null);
		this.SetupQuestReward(this.m_quest);
		this.m_hasExpandData = (list.Count > 0);
		if (this.m_hasExpandData)
		{
			for (int i = 0; i < this.m_expandedTexts.Length; i++)
			{
				UIManager.SetGameObjectActive(this.m_expandedTexts[i], i < list.Count, null);
				if (i < list.Count)
				{
					UIManager.SetGameObjectActive(this.m_expandedTexts[i], true, null);
					this.m_expandedTexts[i].text = "- " + list[i];
				}
			}
		}
		this.UpdateExpandedVisuals();
		this.m_selectableBtn.spriteController.SetClickable(this.m_hasExpandData);
		return num;
	}

	private void SetupQuestReward(QuestTemplate quest)
	{
		bool flag = this.SetupReward(quest.Rewards);
		if (!flag)
		{
			if (quest.ConditionalRewards != null)
			{
				for (int i = 0; i < quest.ConditionalRewards.Length; i++)
				{
					if (QuestWideData.AreConditionsMet(quest.ConditionalRewards[i].Prerequisites.Conditions, quest.ConditionalRewards[i].Prerequisites.LogicStatement, false))
					{
						flag = this.SetupReward(quest.ConditionalRewards[i]);
						if (flag)
						{
							goto IL_A6;
						}
					}
				}
			}
		}
		IL_A6:
		UIManager.SetGameObjectActive(this.m_questReward, flag, null);
	}

	private bool SetupReward(QuestRewards questRewards)
	{
		if (questRewards.CurrencyRewards.Count > 0)
		{
			this.m_questReward.Setup(questRewards.CurrencyRewards[0], 0);
		}
		else if (questRewards.UnlockRewards.Count > 0)
		{
			this.m_questReward.SetupHack(questRewards.UnlockRewards[0].resourceString, 0);
		}
		else
		{
			if (questRewards.ItemRewards.Count <= 0)
			{
				return false;
			}
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(questRewards.ItemRewards[0].ItemTemplateId);
			this.m_questReward.SetupHack(itemTemplate, itemTemplate.IconPath, 0);
		}
		return true;
	}

	private void UpdateExpandedVisuals()
	{
		if (this.m_isExpanded && !this.m_hasExpandData)
		{
			return;
		}
		LayoutElement layoutElement = this.m_layoutElement;
		float preferredHeight;
		if (this.m_isExpanded)
		{
			preferredHeight = this.m_expandedHeight;
		}
		else
		{
			preferredHeight = this.m_defaultHeight;
		}
		layoutElement.preferredHeight = preferredHeight;
		UIManager.SetGameObjectActive(this.m_expandedContainer, this.m_isExpanded, null);
		UIManager.SetGameObjectActive(this.m_expandDownArrow, !this.m_isExpanded, null);
	}
}
