using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterTooltip : UITooltipBase
{
	public TextMeshProUGUI m_name;

	public Image m_roleIcon;

	public TextMeshProUGUI m_description;

	public TextMeshProUGUI m_freeRotationLabel;

	public UINotchedFillBar m_healthFill;

	public UINotchedFillBar m_damageFill;

	public UINotchedFillBar m_survivalFill;

	public UINotchedFillBar m_difficultyFill;

	public QuestItem m_questPrefab;

	public void Setup(CharacterResourceLink charLink, GameType gameType)
	{
		this.m_name.text = charLink.GetDisplayName();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		string text = string.Empty;
		if (clientGameManager.IsCharacterInFreeRotation(charLink.m_characterType, gameType))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterTooltip.Setup(CharacterResourceLink, GameType)).MethodHandle;
			}
			text = StringUtil.TR("FreeRotation", "Global");
		}
		this.m_freeRotationLabel.text = text;
		this.m_description.text = charLink.GetCharSelectTooltipDescription();
		this.m_description.CalculateLayoutInputVertical();
		this.m_roleIcon.sprite = charLink.GetCharacterRoleIcon();
		this.m_healthFill.Setup(charLink.m_statHealth);
		this.m_damageFill.Setup(charLink.m_statDamage);
		this.m_survivalFill.Setup(charLink.m_statSurvival);
		this.m_difficultyFill.Setup(charLink.m_statDifficulty);
		Queue<QuestItem> queue = new Queue<QuestItem>();
		foreach (QuestItem questItem in base.GetComponentsInChildren<QuestItem>(true))
		{
			UIManager.SetGameObjectActive(questItem, false, null);
			queue.Enqueue(questItem);
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
		QuestComponent questComponent = clientGameManager.GetPlayerAccountData().QuestComponent;
		List<int> list = new List<int>(questComponent.Progress.Keys);
		QuestComponent questComponent2 = clientGameManager.GetPlayerAccountData().QuestComponent;
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(questComponent2.ActiveSeason);
		if (questComponent2.UnlockedSeasonChapters.ContainsKey(questComponent2.ActiveSeason))
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
			int j = 0;
			while (j < questComponent2.HighestSeasonChapter)
			{
				if (seasonTemplate.Chapters[j].NormalQuests.Count > 0)
				{
					goto IL_1F8;
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
				if (j == questComponent2.HighestSeasonChapter - 1)
				{
					goto IL_1F8;
				}
				IL_21F:
				j++;
				continue;
				IL_1F8:
				list.AddRange(UISeasonsPanel.GetChapterQuests(seasonTemplate.Chapters[j], seasonTemplate.Index, j));
				goto IL_21F;
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
		IEnumerator<int> enumerator = list.Distinct<int>().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				int num = enumerator.Current;
				if (questComponent.GetCompletedCount(num) > 0)
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
				}
				else
				{
					QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(num);
					if (!questTemplate.HideCompletion && questTemplate.Enabled)
					{
						bool flag = false;
						using (List<QuestObjective>.Enumerator enumerator2 = questTemplate.Objectives.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								QuestObjective questObjective = enumerator2.Current;
								if (!questObjective.Hidden)
								{
									if (questObjective.SuperHidden)
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
									}
									else
									{
										foreach (QuestTrigger questTrigger in questObjective.Triggers)
										{
											using (List<QuestCondition>.Enumerator enumerator4 = questTrigger.Conditions.GetEnumerator())
											{
												while (enumerator4.MoveNext())
												{
													QuestCondition questCondition = enumerator4.Current;
													if (questCondition.ConditionType == QuestConditionType.UsingCharacter)
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
														if (questCondition.typeSpecificData == (int)charLink.m_characterType)
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
															flag = true;
															goto IL_434;
														}
													}
													if (questCondition.ConditionType == QuestConditionType.UsingGameType)
													{
														flag = true;
													}
													else if (questCondition.ConditionType == QuestConditionType.UsingCharacterRole && questCondition.typeSpecificData == (int)charLink.m_characterRole)
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
														flag = true;
													}
													else
													{
														if (questCondition.ConditionType != QuestConditionType.UsingCharacterFaction)
														{
															continue;
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
														FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(questCondition.typeSpecificData);
														FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(factionCompetition.Factions[questCondition.typeSpecificData2].FactionGroupIDToUse);
														flag = factionGroup.Characters.Exists((CharacterType x) => x == charLink.m_characterType);
														if (!flag)
														{
															continue;
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
													IL_434:
													goto IL_444;
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
											IL_444:
											if (flag)
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
												break;
											}
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
											goto IL_4A8;
										}
									}
								}
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
						}
						IL_4A8:
						if (flag)
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
							QuestItem questItem2;
							if (queue.Count > 0)
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
								questItem2 = queue.Dequeue();
							}
							else
							{
								questItem2 = UnityEngine.Object.Instantiate<QuestItem>(this.m_questPrefab);
								questItem2.transform.SetParent(base.transform);
								questItem2.transform.localScale = Vector3.one;
								questItem2.transform.localPosition = Vector3.zero;
							}
							questItem2.SetQuestId(num);
							UIManager.SetGameObjectActive(questItem2, true, null);
						}
					}
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
		finally
		{
			if (enumerator != null)
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
				enumerator.Dispose();
			}
		}
	}
}
