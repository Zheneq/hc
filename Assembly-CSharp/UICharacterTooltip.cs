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
		m_name.text = charLink.GetDisplayName();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		string text = string.Empty;
		if (clientGameManager.IsCharacterInFreeRotation(charLink.m_characterType, gameType))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			text = StringUtil.TR("FreeRotation", "Global");
		}
		m_freeRotationLabel.text = text;
		m_description.text = charLink.GetCharSelectTooltipDescription();
		m_description.CalculateLayoutInputVertical();
		m_roleIcon.sprite = charLink.GetCharacterRoleIcon();
		m_healthFill.Setup(charLink.m_statHealth);
		m_damageFill.Setup(charLink.m_statDamage);
		m_survivalFill.Setup(charLink.m_statSurvival);
		m_difficultyFill.Setup(charLink.m_statDifficulty);
		Queue<QuestItem> queue = new Queue<QuestItem>();
		QuestItem[] componentsInChildren = GetComponentsInChildren<QuestItem>(true);
		foreach (QuestItem questItem in componentsInChildren)
		{
			UIManager.SetGameObjectActive(questItem, false);
			queue.Enqueue(questItem);
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			QuestComponent questComponent = clientGameManager.GetPlayerAccountData().QuestComponent;
			List<int> list = new List<int>(questComponent.Progress.Keys);
			QuestComponent questComponent2 = clientGameManager.GetPlayerAccountData().QuestComponent;
			SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(questComponent2.ActiveSeason);
			if (questComponent2.UnlockedSeasonChapters.ContainsKey(questComponent2.ActiveSeason))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int j = 0; j < questComponent2.HighestSeasonChapter; j++)
				{
					if (seasonTemplate.Chapters[j].NormalQuests.Count <= 0)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (j != questComponent2.HighestSeasonChapter - 1)
						{
							continue;
						}
					}
					list.AddRange(UISeasonsPanel.GetChapterQuests(seasonTemplate.Chapters[j], seasonTemplate.Index, j));
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			IEnumerator<int> enumerator = list.Distinct().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					if (questComponent.GetCompletedCount(current) > 0)
					{
						while (true)
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
						QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(current);
						if (!questTemplate.HideCompletion && questTemplate.Enabled)
						{
							bool flag = false;
							using (List<QuestObjective>.Enumerator enumerator2 = questTemplate.Objectives.GetEnumerator())
							{
								while (true)
								{
									if (!enumerator2.MoveNext())
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										break;
									}
									QuestObjective current2 = enumerator2.Current;
									if (!current2.Hidden)
									{
										if (current2.SuperHidden)
										{
											while (true)
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
											foreach (QuestTrigger trigger in current2.Triggers)
											{
												using (List<QuestCondition>.Enumerator enumerator4 = trigger.Conditions.GetEnumerator())
												{
													while (true)
													{
														if (!enumerator4.MoveNext())
														{
															while (true)
															{
																switch (1)
																{
																case 0:
																	continue;
																}
																break;
															}
															break;
														}
														QuestCondition current4 = enumerator4.Current;
														if (current4.ConditionType == QuestConditionType.UsingCharacter)
														{
															while (true)
															{
																switch (6)
																{
																case 0:
																	continue;
																}
																break;
															}
															if (current4.typeSpecificData == (int)charLink.m_characterType)
															{
																while (true)
																{
																	switch (4)
																	{
																	case 0:
																		break;
																	default:
																		flag = true;
																		goto end_IL_0310;
																	}
																}
															}
														}
														if (current4.ConditionType == QuestConditionType.UsingGameType)
														{
															flag = true;
															break;
														}
														if (current4.ConditionType == QuestConditionType.UsingCharacterRole && current4.typeSpecificData == (int)charLink.m_characterRole)
														{
															while (true)
															{
																switch (1)
																{
																case 0:
																	break;
																default:
																	flag = true;
																	goto end_IL_0310;
																}
															}
														}
														if (current4.ConditionType == QuestConditionType.UsingCharacterFaction)
														{
															while (true)
															{
																switch (3)
																{
																case 0:
																	continue;
																}
																break;
															}
															FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(current4.typeSpecificData);
															FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(factionCompetition.Factions[current4.typeSpecificData2].FactionGroupIDToUse);
															flag = factionGroup.Characters.Exists((CharacterType x) => x == charLink.m_characterType);
															if (flag)
															{
																while (true)
																{
																	switch (6)
																	{
																	case 0:
																		continue;
																	}
																	break;
																}
																break;
															}
														}
													}
													end_IL_0310:;
												}
												if (flag)
												{
													while (true)
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
												while (true)
												{
													switch (6)
													{
													case 0:
														continue;
													}
													break;
												}
												break;
											}
										}
									}
								}
							}
							if (flag)
							{
								while (true)
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
									while (true)
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
									questItem2 = Object.Instantiate(m_questPrefab);
									questItem2.transform.SetParent(base.transform);
									questItem2.transform.localScale = Vector3.one;
									questItem2.transform.localPosition = Vector3.zero;
								}
								questItem2.SetQuestId(current);
								UIManager.SetGameObjectActive(questItem2, true);
							}
						}
					}
				}
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			finally
			{
				if (enumerator != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							enumerator.Dispose();
							goto end_IL_054d;
						}
					}
				}
				end_IL_054d:;
			}
		}
	}
}
