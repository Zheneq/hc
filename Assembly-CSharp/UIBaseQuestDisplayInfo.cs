using System;
using System.Collections.Generic;
using UnityEngine;

public class UIBaseQuestDisplayInfo
{
	public bool Completed;

	public string Reward;

	public string QuestDescription;

	public string LongQuestDescription;

	public string TypeDisplayName;

	public QuestRewards QuestRewardsRef;

	public QuestProgress QuestProgressRef;

	public QuestTemplate QuestTemplateRef;

	public DateTime QuestAbandonDate;

	public override bool Equals(object obj)
	{
		if (!(obj is UIBaseQuestDisplayInfo))
		{
			while (true)
			{
				return false;
			}
		}
		UIBaseQuestDisplayInfo info = (UIBaseQuestDisplayInfo)obj;
		bool flag = false;
		if (QuestProgressRef == null)
		{
			if (info.QuestProgressRef == null)
			{
				flag = true;
				goto IL_039f;
			}
		}
		bool flag2;
		if (QuestProgressRef != null && info.QuestProgressRef != null)
		{
			flag2 = true;
			if (QuestProgressRef.ObjectiveProgressLastDate != null && info.QuestProgressRef.ObjectiveProgressLastDate != null)
			{
				if (QuestProgressRef.ObjectiveProgressLastDate.Count != info.QuestProgressRef.ObjectiveProgressLastDate.Count)
				{
					flag2 = false;
				}
				else
				{
					using (Dictionary<int, DateTime>.KeyCollection.Enumerator enumerator = QuestProgressRef.ObjectiveProgressLastDate.Keys.GetEnumerator())
					{
						while (true)
						{
							if (!enumerator.MoveNext())
							{
								break;
							}
							int current = enumerator.Current;
							if (!info.QuestProgressRef.ObjectiveProgressLastDate.ContainsKey(current))
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
										flag2 = false;
										goto end_IL_010a;
									}
								}
							}
							if (info.QuestProgressRef.ObjectiveProgressLastDate[current] != QuestProgressRef.ObjectiveProgressLastDate[current])
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										flag2 = false;
										goto end_IL_010a;
									}
								}
							}
						}
						end_IL_010a:;
					}
				}
			}
			else if (QuestProgressRef.ObjectiveProgressLastDate == null && info.QuestProgressRef.ObjectiveProgressLastDate != null)
			{
				flag2 = false;
			}
			else if (QuestProgressRef.ObjectiveProgressLastDate != null && info.QuestProgressRef.ObjectiveProgressLastDate == null)
			{
				flag2 = false;
			}
			if (QuestProgressRef.ObjectiveProgress != null)
			{
				if (info.QuestProgressRef.ObjectiveProgress != null)
				{
					if (QuestProgressRef.ObjectiveProgress.Count != info.QuestProgressRef.ObjectiveProgress.Count)
					{
						flag2 = false;
					}
					else
					{
						foreach (int key in QuestProgressRef.ObjectiveProgress.Keys)
						{
							if (!info.QuestProgressRef.ObjectiveProgress.ContainsKey(key))
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										flag2 = false;
										goto end_IL_02a7;
									}
								}
							}
							if (info.QuestProgressRef.ObjectiveProgress[key] != QuestProgressRef.ObjectiveProgress[key])
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										flag2 = false;
										goto end_IL_02a7;
									}
								}
							}
						}
					}
					goto IL_039d;
				}
			}
			if (QuestProgressRef.ObjectiveProgress == null)
			{
				if (info.QuestProgressRef.ObjectiveProgress != null)
				{
					flag2 = false;
					goto IL_039d;
				}
			}
			if (QuestProgressRef.ObjectiveProgress != null && info.QuestProgressRef.ObjectiveProgress == null)
			{
				flag2 = false;
			}
			goto IL_039d;
		}
		goto IL_039f;
		IL_068d:
		int result;
		return (byte)result != 0;
		IL_039f:
		bool flag3 = false;
		if (info.QuestTemplateRef != null)
		{
			if (QuestTemplateRef != null)
			{
				flag3 = (QuestTemplateRef.Index == info.QuestTemplateRef.Index);
			}
		}
		bool flag4 = false;
		if (info.QuestRewardsRef != null)
		{
			if (QuestRewardsRef != null)
			{
				if (info.QuestRewardsRef.CurrencyRewards.Count == QuestRewardsRef.CurrencyRewards.Count)
				{
					if (info.QuestRewardsRef.ItemRewards.Count == QuestRewardsRef.ItemRewards.Count)
					{
						if (info.QuestRewardsRef.UnlockRewards.Count == QuestRewardsRef.UnlockRewards.Count)
						{
							flag4 = true;
							int j = 0;
							while (true)
							{
								if (j < info.QuestRewardsRef.CurrencyRewards.Count)
								{
									if (!QuestRewardsRef.CurrencyRewards.ContainsWhere(delegate(QuestCurrencyReward reward)
									{
										int result3;
										if (reward.Type == info.QuestRewardsRef.CurrencyRewards[j].Type)
										{
											result3 = ((reward.Amount == info.QuestRewardsRef.CurrencyRewards[j].Amount) ? 1 : 0);
										}
										else
										{
											result3 = 0;
										}
										return (byte)result3 != 0;
									}))
									{
										flag4 = false;
										break;
									}
									j++;
									continue;
								}
								break;
							}
							if (flag4)
							{
								int k = 0;
								while (true)
								{
									if (k < info.QuestRewardsRef.ItemRewards.Count)
									{
										if (!QuestRewardsRef.ItemRewards.ContainsWhere(delegate(QuestItemReward reward)
										{
											int result2;
											if (reward.ItemTemplateId == info.QuestRewardsRef.ItemRewards[k].ItemTemplateId)
											{
												result2 = ((reward.Amount == info.QuestRewardsRef.ItemRewards[k].Amount) ? 1 : 0);
											}
											else
											{
												result2 = 0;
											}
											return (byte)result2 != 0;
										}))
										{
											flag4 = false;
											break;
										}
										k++;
										continue;
									}
									break;
								}
								if (flag4)
								{
									int i = 0;
									while (true)
									{
										if (i < info.QuestRewardsRef.UnlockRewards.Count)
										{
											if (!QuestRewardsRef.UnlockRewards.ContainsWhere(delegate(QuestUnlockReward reward)
											{
												if (reward.purchaseType == info.QuestRewardsRef.UnlockRewards[i].purchaseType)
												{
													if (reward.typeSpecificData.Length == info.QuestRewardsRef.UnlockRewards[i].typeSpecificData.Length)
													{
														for (int l = 0; l < reward.typeSpecificData.Length; l++)
														{
															if (reward.typeSpecificData[l] != info.QuestRewardsRef.UnlockRewards[i].typeSpecificData[l])
															{
																return false;
															}
														}
														while (true)
														{
															switch (5)
															{
															case 0:
																break;
															default:
																return true;
															}
														}
													}
												}
												return false;
											}))
											{
												flag4 = false;
												break;
											}
											i++;
											continue;
										}
										break;
									}
								}
							}
						}
					}
				}
			}
		}
		if (Completed == info.Completed)
		{
			if (flag && flag3)
			{
				result = (flag4 ? 1 : 0);
				goto IL_068d;
			}
		}
		result = 0;
		goto IL_068d;
		IL_039d:
		flag = flag2;
		goto IL_039f;
	}

	public override int GetHashCode()
	{
		if (QuestProgressRef != null)
		{
			if (QuestProgressRef.ObjectiveProgressLastDate != null && QuestProgressRef.ObjectiveProgress != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return Completed.GetHashCode() ^ QuestProgressRef.ObjectiveProgressLastDate.GetHashCode() ^ QuestProgressRef.ObjectiveProgress.GetHashCode();
					}
				}
			}
		}
		return Completed.GetHashCode();
	}

	public virtual void Setup(int QuestIndex)
	{
		Reward = string.Format(StringUtil.TR("NewReward", "Seasons"), Mathf.FloorToInt(UnityEngine.Random.value * 100f));
		QuestDescription = string.Format(StringUtil.TR("DoActionToComplete", "Seasons"), Mathf.FloorToInt(UnityEngine.Random.value * 100f));
		PersistedAccountData persistedAccountData = null;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			persistedAccountData = ClientGameManager.Get().GetPlayerAccountData();
		}
		QuestTemplateRef = null;
		if (-1 < QuestIndex - 1)
		{
			if (QuestIndex - 1 < QuestWideData.Get().m_quests.Count)
			{
				QuestTemplateRef = QuestWideData.Get().m_quests[QuestIndex - 1];
			}
		}
		if (QuestTemplateRef != null)
		{
			QuestDescription = StringUtil.TR_QuestDescription(QuestIndex);
			LongQuestDescription = StringUtil.TR_QuestLongDescription(QuestIndex);
			TypeDisplayName = StringUtil.TR_QuestTypeDisplayName(QuestIndex);
			QuestRewardsRef = new QuestRewards();
			QuestRewardsRef.CurrencyRewards = new List<QuestCurrencyReward>(QuestTemplateRef.Rewards.CurrencyRewards);
			QuestRewardsRef.ItemRewards = new List<QuestItemReward>(QuestTemplateRef.Rewards.ItemRewards);
			QuestRewardsRef.UnlockRewards = new List<QuestUnlockReward>(QuestTemplateRef.Rewards.UnlockRewards);
			if (QuestTemplateRef.ConditionalRewards != null)
			{
				for (int i = 0; i < QuestTemplateRef.ConditionalRewards.Length; i++)
				{
					if (QuestWideData.AreConditionsMet(QuestTemplateRef.ConditionalRewards[i].Prerequisites.Conditions, QuestTemplateRef.ConditionalRewards[i].Prerequisites.LogicStatement))
					{
						QuestRewardsRef.CurrencyRewards.AddRange(QuestTemplateRef.ConditionalRewards[i].CurrencyRewards);
						QuestRewardsRef.ItemRewards.AddRange(QuestTemplateRef.ConditionalRewards[i].ItemRewards);
						QuestRewardsRef.UnlockRewards.AddRange(QuestTemplateRef.ConditionalRewards[i].UnlockRewards);
					}
				}
			}
			if (persistedAccountData != null)
			{
				if (persistedAccountData.QuestComponent.Progress.ContainsKey(QuestTemplateRef.Index))
				{
					QuestProgressRef = persistedAccountData.QuestComponent.Progress[QuestTemplateRef.Index];
				}
				Completed = (persistedAccountData.QuestComponent.GetCompletedCount(QuestTemplateRef.Index) > 0);
			}
			QuestAbandonDate = DateTime.MinValue;
			if (!QuestTemplateRef.AbandonDateTime.IsNullOrEmpty())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						DateTime.TryParse(QuestTemplateRef.AbandonDateTime, out QuestAbandonDate);
						return;
					}
				}
			}
			if (persistedAccountData == null)
			{
				return;
			}
			while (true)
			{
				if (persistedAccountData.QuestComponent.GetOrCreateQuestMetaData(QuestIndex).PstAbandonDate.HasValue)
				{
					while (true)
					{
						QuestAbandonDate = persistedAccountData.QuestComponent.GetOrCreateQuestMetaData(QuestIndex).PstAbandonDate.Value;
						return;
					}
				}
				return;
			}
		}
		throw new Exception("could not find quest");
	}
}
