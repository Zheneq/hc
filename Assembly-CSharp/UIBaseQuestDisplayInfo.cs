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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBaseQuestDisplayInfo.Equals(object)).MethodHandle;
			}
			return false;
		}
		UIBaseQuestDisplayInfo info = (UIBaseQuestDisplayInfo)obj;
		bool flag = false;
		if (this.QuestProgressRef == null)
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
			if (info.QuestProgressRef == null)
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
				flag = true;
				goto IL_39F;
			}
		}
		if (this.QuestProgressRef != null && info.QuestProgressRef != null)
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
			bool flag2 = true;
			if (this.QuestProgressRef.ObjectiveProgressLastDate != null && info.QuestProgressRef.ObjectiveProgressLastDate != null)
			{
				if (this.QuestProgressRef.ObjectiveProgressLastDate.Count != info.QuestProgressRef.ObjectiveProgressLastDate.Count)
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
					flag2 = false;
				}
				else
				{
					using (Dictionary<int, DateTime>.KeyCollection.Enumerator enumerator = this.QuestProgressRef.ObjectiveProgressLastDate.Keys.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							int key = enumerator.Current;
							if (!info.QuestProgressRef.ObjectiveProgressLastDate.ContainsKey(key))
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
								flag2 = false;
							}
							else
							{
								if (!(info.QuestProgressRef.ObjectiveProgressLastDate[key] != this.QuestProgressRef.ObjectiveProgressLastDate[key]))
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
								flag2 = false;
							}
							goto IL_1AF;
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
				}
				IL_1AF:;
			}
			else if (this.QuestProgressRef.ObjectiveProgressLastDate == null && info.QuestProgressRef.ObjectiveProgressLastDate != null)
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
				flag2 = false;
			}
			else if (this.QuestProgressRef.ObjectiveProgressLastDate != null && info.QuestProgressRef.ObjectiveProgressLastDate == null)
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
				flag2 = false;
			}
			if (this.QuestProgressRef.ObjectiveProgress != null)
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
				if (info.QuestProgressRef.ObjectiveProgress != null)
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
					if (this.QuestProgressRef.ObjectiveProgress.Count != info.QuestProgressRef.ObjectiveProgress.Count)
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
						flag2 = false;
					}
					else
					{
						foreach (int key2 in this.QuestProgressRef.ObjectiveProgress.Keys)
						{
							if (!info.QuestProgressRef.ObjectiveProgress.ContainsKey(key2))
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
								flag2 = false;
								break;
							}
							if (info.QuestProgressRef.ObjectiveProgress[key2] != this.QuestProgressRef.ObjectiveProgress[key2])
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
								flag2 = false;
								break;
							}
						}
					}
					goto IL_39D;
				}
			}
			if (this.QuestProgressRef.ObjectiveProgress == null)
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
				if (info.QuestProgressRef.ObjectiveProgress != null)
				{
					flag2 = false;
					goto IL_39D;
				}
			}
			if (this.QuestProgressRef.ObjectiveProgress != null && info.QuestProgressRef.ObjectiveProgress == null)
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
				flag2 = false;
			}
			IL_39D:
			flag = flag2;
		}
		IL_39F:
		bool flag3 = false;
		if (info.QuestTemplateRef != null)
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
			if (this.QuestTemplateRef != null)
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
				flag3 = (this.QuestTemplateRef.Index == info.QuestTemplateRef.Index);
			}
		}
		bool flag4 = false;
		if (info.QuestRewardsRef != null)
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
			if (this.QuestRewardsRef != null)
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
				if (info.QuestRewardsRef.CurrencyRewards.Count == this.QuestRewardsRef.CurrencyRewards.Count)
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
					if (info.QuestRewardsRef.ItemRewards.Count == this.QuestRewardsRef.ItemRewards.Count)
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
						if (info.QuestRewardsRef.UnlockRewards.Count == this.QuestRewardsRef.UnlockRewards.Count)
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
							flag4 = true;
							int i = 0;
							while (i < info.QuestRewardsRef.CurrencyRewards.Count)
							{
								if (!this.QuestRewardsRef.CurrencyRewards.ContainsWhere(delegate(QuestCurrencyReward reward)
								{
									bool result;
									if (reward.Type == info.QuestRewardsRef.CurrencyRewards[i].Type)
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
											RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIBaseQuestDisplayInfo.<Equals>c__AnonStorey1.<>m__0(QuestCurrencyReward)).MethodHandle;
										}
										result = (reward.Amount == info.QuestRewardsRef.CurrencyRewards[i].Amount);
									}
									else
									{
										result = false;
									}
									return result;
								}))
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
									flag4 = false;
									IL_548:
									if (!flag4)
									{
										goto IL_664;
									}
									int i = 0;
									while (i < info.QuestRewardsRef.ItemRewards.Count)
									{
										if (!this.QuestRewardsRef.ItemRewards.ContainsWhere(delegate(QuestItemReward reward)
										{
											bool result;
											if (reward.ItemTemplateId == info.QuestRewardsRef.ItemRewards[i].ItemTemplateId)
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
												if (!true)
												{
													RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIBaseQuestDisplayInfo.<Equals>c__AnonStorey2.<>m__0(QuestItemReward)).MethodHandle;
												}
												result = (reward.Amount == info.QuestRewardsRef.ItemRewards[i].Amount);
											}
											else
											{
												result = false;
											}
											return result;
										}))
										{
											flag4 = false;
											IL_5CC:
											if (!flag4)
											{
												goto IL_664;
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
											int i;
											for (i = 0; i < info.QuestRewardsRef.UnlockRewards.Count; i++)
											{
												if (!this.QuestRewardsRef.UnlockRewards.ContainsWhere(delegate(QuestUnlockReward reward)
												{
													int i;
													if (reward.purchaseType == info.QuestRewardsRef.UnlockRewards[i].purchaseType)
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
														if (!true)
														{
															RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIBaseQuestDisplayInfo.<Equals>c__AnonStorey3.<>m__0(QuestUnlockReward)).MethodHandle;
														}
														if (reward.typeSpecificData.Length == info.QuestRewardsRef.UnlockRewards[i].typeSpecificData.Length)
														{
															for (i = 0; i < reward.typeSpecificData.Length; i++)
															{
																if (reward.typeSpecificData[i] != info.QuestRewardsRef.UnlockRewards[i].typeSpecificData[i])
																{
																	return false;
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
															return true;
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
													return false;
												}))
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
													flag4 = false;
													goto IL_664;
												}
											}
											for (;;)
											{
												switch (6)
												{
												case 0:
													continue;
												}
												goto IL_664;
											}
										}
										else
										{
											i++;
										}
									}
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										goto IL_5CC;
									}
								}
								else
								{
									i++;
								}
							}
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								goto IL_548;
							}
						}
					}
				}
			}
		}
		IL_664:
		if (this.Completed == info.Completed)
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
			if (flag && flag3)
			{
				return flag4;
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		if (this.QuestProgressRef != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBaseQuestDisplayInfo.GetHashCode()).MethodHandle;
			}
			if (this.QuestProgressRef.ObjectiveProgressLastDate != null && this.QuestProgressRef.ObjectiveProgress != null)
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
				return this.Completed.GetHashCode() ^ this.QuestProgressRef.ObjectiveProgressLastDate.GetHashCode() ^ this.QuestProgressRef.ObjectiveProgress.GetHashCode();
			}
		}
		return this.Completed.GetHashCode();
	}

	public virtual void Setup(int QuestIndex)
	{
		this.Reward = string.Format(StringUtil.TR("NewReward", "Seasons"), Mathf.FloorToInt(UnityEngine.Random.value * 100f));
		this.QuestDescription = string.Format(StringUtil.TR("DoActionToComplete", "Seasons"), Mathf.FloorToInt(UnityEngine.Random.value * 100f));
		PersistedAccountData persistedAccountData = null;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBaseQuestDisplayInfo.Setup(int)).MethodHandle;
			}
			persistedAccountData = ClientGameManager.Get().GetPlayerAccountData();
		}
		this.QuestTemplateRef = null;
		if (-1 < QuestIndex - 1)
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
			if (QuestIndex - 1 < QuestWideData.Get().m_quests.Count)
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
				this.QuestTemplateRef = QuestWideData.Get().m_quests[QuestIndex - 1];
			}
		}
		if (this.QuestTemplateRef != null)
		{
			this.QuestDescription = StringUtil.TR_QuestDescription(QuestIndex);
			this.LongQuestDescription = StringUtil.TR_QuestLongDescription(QuestIndex);
			this.TypeDisplayName = StringUtil.TR_QuestTypeDisplayName(QuestIndex);
			this.QuestRewardsRef = new QuestRewards();
			this.QuestRewardsRef.CurrencyRewards = new List<QuestCurrencyReward>(this.QuestTemplateRef.Rewards.CurrencyRewards);
			this.QuestRewardsRef.ItemRewards = new List<QuestItemReward>(this.QuestTemplateRef.Rewards.ItemRewards);
			this.QuestRewardsRef.UnlockRewards = new List<QuestUnlockReward>(this.QuestTemplateRef.Rewards.UnlockRewards);
			if (this.QuestTemplateRef.ConditionalRewards != null)
			{
				for (int i = 0; i < this.QuestTemplateRef.ConditionalRewards.Length; i++)
				{
					if (QuestWideData.AreConditionsMet(this.QuestTemplateRef.ConditionalRewards[i].Prerequisites.Conditions, this.QuestTemplateRef.ConditionalRewards[i].Prerequisites.LogicStatement, false))
					{
						this.QuestRewardsRef.CurrencyRewards.AddRange(this.QuestTemplateRef.ConditionalRewards[i].CurrencyRewards);
						this.QuestRewardsRef.ItemRewards.AddRange(this.QuestTemplateRef.ConditionalRewards[i].ItemRewards);
						this.QuestRewardsRef.UnlockRewards.AddRange(this.QuestTemplateRef.ConditionalRewards[i].UnlockRewards);
					}
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
			if (persistedAccountData != null)
			{
				if (persistedAccountData.QuestComponent.Progress.ContainsKey(this.QuestTemplateRef.Index))
				{
					this.QuestProgressRef = persistedAccountData.QuestComponent.Progress[this.QuestTemplateRef.Index];
				}
				this.Completed = (persistedAccountData.QuestComponent.GetCompletedCount(this.QuestTemplateRef.Index) > 0);
			}
			this.QuestAbandonDate = DateTime.MinValue;
			if (!this.QuestTemplateRef.AbandonDateTime.IsNullOrEmpty())
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
				DateTime.TryParse(this.QuestTemplateRef.AbandonDateTime, out this.QuestAbandonDate);
			}
			else if (persistedAccountData != null)
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
				if (persistedAccountData.QuestComponent.GetOrCreateQuestMetaData(QuestIndex).PstAbandonDate != null)
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
					this.QuestAbandonDate = persistedAccountData.QuestComponent.GetOrCreateQuestMetaData(QuestIndex).PstAbandonDate.Value;
				}
			}
			return;
		}
		throw new Exception("could not find quest");
	}
}
