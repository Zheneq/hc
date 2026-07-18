using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestWideData : MonoBehaviour
{
    private static QuestWideData s_instance;

    [Header("Quest Global Values")] public int m_generalSlotCount;
    public int m_questResetHour;
    public DayOfWeek m_questResetDayOfWeek;
    public int m_questBonusPerRejection = 10;
    public int m_questMaxRejectPercentage = 50;
    public int m_notIncludedWeightIncrease;
    public int m_rejectedWeightDecrease;
    public int m_completedWeightDecrease;
    public int m_abandonedWeightDecrease;
    [Header("Quests")]
    public List<QuestTemplate> m_quests;
    [Header("Style Groups")]
    public List<QuestStyleGroup> m_styleGroups;
    [Header("Daily Quest Pools")]
    public List<QuestPool> m_dailyQuestPools;

    private static Dictionary<int, bool> m_dailyQuestsCache = new Dictionary<int, bool>();

    public static QuestWideData Get()
    {
        return s_instance;
    }

    private void Awake()
    {
        s_instance = this;
        if (m_quests.Count == 0)
        {
            throw new Exception("QuestWideData failed to load");
        }

        m_dailyQuestsCache = new Dictionary<int, bool>();
        foreach (QuestPool questPool in m_dailyQuestPools)
        {
            if (!questPool.Valid)
            {
                continue;
            }

            foreach (QuestPool.Quest quest in questPool.Quests)
            {
                if (!m_dailyQuestsCache.ContainsKey(quest.QuestId))
                {
                    m_dailyQuestsCache.Add(quest.QuestId, true);
                }
            }
        }
    }

    public QuestTemplate GetQuestTemplate(int templateId)
    {
        return templateId > 0 && m_quests.Count >= templateId
            ? m_quests[templateId - 1]
            : null;
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

    public bool IsDailyQuest(int questId)
    {
        return m_dailyQuestsCache.TryGetValue(questId, out bool value) && value;
    }

    public bool CheckAllIndices()
    {
        if (!Application.isEditor)
        {
            return false;
        }

        int num = 1;
        foreach (QuestTemplate quest in m_quests)
        {
            if (quest.Index >= num)
            {
                num = quest.Index + 1;
            }
        }

        bool result = false;
        foreach (QuestTemplate quest in m_quests)
        {
            bool flag = false;
            if (quest.Index == 0)
            {
                flag = true;
            }
            else
            {
                int num2 = 0;
                foreach (QuestTemplate quest2 in m_quests)
                {
                    if (quest2.Index == quest.Index)
                    {
                        num2++;
                    }
                }

                if (num2 > 1)
                {
                    flag = true;
                }
            }

            if (flag)
            {
                quest.Index = num;
                num++;
                result = true;
            }
        }

        return result;
    }

    public static bool IsCharacterQuest(List<QuestObjective> Objectives, CharacterResourceLink charLink)
    {
        foreach (QuestObjective questObjective in Objectives)
        {
            foreach (QuestTrigger trigger in questObjective.Triggers)
            {
                foreach (QuestCondition condition in trigger.Conditions)
                {
                    if (condition.ConditionType == QuestConditionType.UsingCharacter
                        && condition.typeSpecificData == (int)charLink.m_characterType)
                    {
                        return true;
                    }

                    if (condition.ConditionType == QuestConditionType.HasCharacterLevel
                        && condition.typeSpecificData == (int)charLink.m_characterType)
                    {
                        return true;
                    }

                    if (condition.ConditionType == QuestConditionType.UsingCharacterRole
                        && condition.typeSpecificData == (int)charLink.m_characterRole)
                    {
                        return true;
                    }

                    if (condition.ConditionType == QuestConditionType.UsingCharacterFaction)
                    {
                        FactionCompetition factionCompetition =
                            FactionWideData.Get().GetFactionCompetition(condition.typeSpecificData);
                        FactionGroup factionGroup = FactionWideData.Get()
                            .GetFactionGroup(factionCompetition.Factions[condition.typeSpecificData2]
                                .FactionGroupIDToUse);
                        if (factionGroup.Characters.Exists((CharacterType x) => x == charLink.m_characterType))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public static int GetEndLevel(QuestPrerequisites prereqs, int seasonIndex)
    {
        Queue<LogicOpClass> queue = new Queue<LogicOpClass>();
        string text = prereqs.LogicStatement;
        if (text.IsNullOrEmpty())
        {
            int num = 'A';
            for (int i = 0; i < prereqs.Conditions.Count; i++)
            {
                text = !text.IsNullOrEmpty()
                    ? text + " & " + Convert.ToChar(num)
                    : Convert.ToChar(num).ToString();
                num++;
            }
        }

        queue.Enqueue(LogicStatement.EvaluateLogicStatement(text));
        int result = 0;
        while (queue.Count > 0)
        {
            LogicOpClass logicOpClass = queue.Dequeue();
            if (logicOpClass is ConstantLogicOpClass)
            {
                continue;
            }

            if (logicOpClass is AndLogicOpClass)
            {
                AndLogicOpClass andLogicOpClass = (AndLogicOpClass)logicOpClass;
                queue.Enqueue(andLogicOpClass.m_left);
                queue.Enqueue(andLogicOpClass.m_right);
                continue;
            }

            if (logicOpClass is OrLogicOpClass)
            {
                OrLogicOpClass orLogicOpClass = (OrLogicOpClass)logicOpClass;
                queue.Enqueue(orLogicOpClass.m_left);
                queue.Enqueue(orLogicOpClass.m_right);
                continue;
            }

            if (!(logicOpClass is NegateLogicOpClass))
            {
                continue;
            }

            NegateLogicOpClass negateLogicOpClass = (NegateLogicOpClass)logicOpClass;
            if (negateLogicOpClass.m_target is ConstantLogicOpClass)
            {
                int myIndex = ((ConstantLogicOpClass)negateLogicOpClass.m_target).myIndex;
                if (prereqs.Conditions[myIndex].ConditionType != QuestConditionType.HasSeasonLevel)
                {
                    continue;
                }

                if (prereqs.Conditions[myIndex].typeSpecificData == seasonIndex)
                {
                    result = prereqs.Conditions[myIndex].typeSpecificData2;
                }
            }
            else if (negateLogicOpClass.m_target is AndLogicOpClass)
            {
                AndLogicOpClass andLogicOpClass2 = (AndLogicOpClass)negateLogicOpClass.m_target;
                queue.Enqueue(new NegateLogicOpClass
                {
                    m_target = andLogicOpClass2.m_left
                });
                queue.Enqueue(new NegateLogicOpClass
                {
                    m_target = andLogicOpClass2.m_right
                });
            }
            else if (negateLogicOpClass.m_target is OrLogicOpClass)
            {
                OrLogicOpClass orLogicOpClass2 = (OrLogicOpClass)negateLogicOpClass.m_target;
                queue.Enqueue(new NegateLogicOpClass
                {
                    m_target = orLogicOpClass2.m_left
                });
                queue.Enqueue(new NegateLogicOpClass
                {
                    m_target = orLogicOpClass2.m_right
                });
            }
            else if (negateLogicOpClass.m_target is NegateLogicOpClass)
            {
                negateLogicOpClass = (NegateLogicOpClass)negateLogicOpClass.m_target;
                queue.Enqueue(negateLogicOpClass.m_target);
            }
        }

        return result;
    }

    public static bool AreConditionsMet(List<QuestCondition> conditions, string logicStatement,
        bool tryUseCharDataOnInitialLoad = false)
    {
        List<bool> evaluatedConditions = new List<bool>(conditions.Count);
        foreach (QuestCondition condition in conditions)
        {
            switch (condition.ConditionType)
            {
                case QuestConditionType.HasDateTimePassed:
                {
                    DateTime t = new DateTime(
                        condition.typeSpecificDate[0],
                        condition.typeSpecificDate[1],
                        condition.typeSpecificDate[2],
                        condition.typeSpecificDate[3],
                        condition.typeSpecificDate[4],
                        condition.typeSpecificDate[5]);
                    evaluatedConditions.Add(ClientGameManager.Get().PacificNow() > t);
                    break;
                }
                case QuestConditionType.HasCompletedQuest:
                    evaluatedConditions.Add(ClientGameManager.Get().GetPlayerAccountData().QuestComponent
                        .GetCompletedCount(condition.typeSpecificData) > 0);
                    break;
                case QuestConditionType.HasPurchasedGame:
                    evaluatedConditions.Add(ClientGameManager.Get().HasPurchasedGame);
                    break;
                case QuestConditionType.HasUnlockedCharacter:
                    evaluatedConditions.Add(ClientGameManager.Get()
                        .GetPlayerCharacterData((CharacterType)condition.typeSpecificData).CharacterComponent.Unlocked);
                    break;
                case QuestConditionType.HasUnlockedStyle:
                {
                    bool item = false;
                    CharacterType charType = (CharacterType)condition.typeSpecificData;
                    PersistedCharacterData characterData = tryUseCharDataOnInitialLoad
                        ? ClientGameManager.Get().GetCharacterDataOnInitialLoad(charType)
                        : ClientGameManager.Get().GetPlayerCharacterData(charType);
                    if (characterData != null)
                    {
                        item = characterData.CharacterComponent.IsStyleUnlocked(
                            condition.typeSpecificData2,
                            condition.typeSpecificData3,
                            condition.typeSpecificData4);
                    }

                    evaluatedConditions.Add(item);
                    break;
                }
                case QuestConditionType.HasUnlockedTitle:
                    evaluatedConditions.Add(ClientGameManager.Get().GetPlayerAccountData().AccountComponent
                        .IsTitleUnlocked(condition.typeSpecificData));
                    break;
                case QuestConditionType.HasUnlockedChatEmoji:
                    evaluatedConditions.Add(ClientGameManager.Get().GetPlayerAccountData().AccountComponent
                        .IsChatEmojiUnlocked(condition.typeSpecificData));
                    break;
                case QuestConditionType.HasUnlockedOvercon:
                    evaluatedConditions.Add(ClientGameManager.Get().GetPlayerAccountData().AccountComponent
                        .IsOverconUnlocked(condition.typeSpecificData));
                    break;
                case QuestConditionType.HasUnlockedTaunt:
                {
                    CharacterType charType = (CharacterType)condition.typeSpecificData;
                    PersistedCharacterData characterData = tryUseCharDataOnInitialLoad
                        ? ClientGameManager.Get().GetCharacterDataOnInitialLoad(charType)
                        : ClientGameManager.Get().GetPlayerCharacterData(charType);
                    bool isTauntUnlocked = characterData != null
                                           && condition.typeSpecificData2 <
                                           characterData.CharacterComponent.Taunts.Count
                                           && characterData.CharacterComponent.GetTaunt(condition.typeSpecificData2)
                                               .Unlocked;
                    evaluatedConditions.Add(isTauntUnlocked);
                    break;
                }
                default:
                {
                    if (condition.ConditionType != QuestConditionType.HasSeasonAccess)
                    {
                        evaluatedConditions.Add(false);
                        throw new Exception("Unimplemented quest condition: " + condition.ConditionType);
                    }

                    evaluatedConditions.Add(
                        ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason ==
                        condition.typeSpecificData);
                    break;
                }
            }
        }

        if (logicStatement == string.Empty)
        {
            return evaluatedConditions.All(evalCond => evalCond);
        }
        else
        {
            return LogicStatement.EvaluateLogicStatement(logicStatement).GetValue(evaluatedConditions);
        }
    }
}