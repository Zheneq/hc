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
		// TODO DECOMP this one was tricky

		if (!(obj is UIBaseQuestDisplayInfo))
		{
			return false;
		}
		UIBaseQuestDisplayInfo info = (UIBaseQuestDisplayInfo)obj;
		bool flag = false;
		if (this.QuestProgressRef == null)
		{
			if (info.QuestProgressRef == null)
			{
				flag = true;
				goto IL_39F;
			}
		}
		if (this.QuestProgressRef != null && info.QuestProgressRef != null)
		{
			bool flag2 = true;
			if (this.QuestProgressRef.ObjectiveProgressLastDate != null && info.QuestProgressRef.ObjectiveProgressLastDate != null)
			{
				if (this.QuestProgressRef.ObjectiveProgressLastDate.Count != info.QuestProgressRef.ObjectiveProgressLastDate.Count)
				{
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
								flag2 = false;
							}
							else
							{
								if (!(info.QuestProgressRef.ObjectiveProgressLastDate[key] != this.QuestProgressRef.ObjectiveProgressLastDate[key]))
								{
									continue;
								}
								flag2 = false;
							}
							goto IL_1AF;
						}
					}
				}
				IL_1AF:;
			}
			else if (this.QuestProgressRef.ObjectiveProgressLastDate == null && info.QuestProgressRef.ObjectiveProgressLastDate != null)
			{
				flag2 = false;
			}
			else if (this.QuestProgressRef.ObjectiveProgressLastDate != null && info.QuestProgressRef.ObjectiveProgressLastDate == null)
			{
				flag2 = false;
			}
			if (this.QuestProgressRef.ObjectiveProgress != null)
			{
				if (info.QuestProgressRef.ObjectiveProgress != null)
				{
					if (this.QuestProgressRef.ObjectiveProgress.Count != info.QuestProgressRef.ObjectiveProgress.Count)
					{
						flag2 = false;
					}
					else
					{
						foreach (int key2 in this.QuestProgressRef.ObjectiveProgress.Keys)
						{
							if (!info.QuestProgressRef.ObjectiveProgress.ContainsKey(key2))
							{
								flag2 = false;
								break;
							}
							if (info.QuestProgressRef.ObjectiveProgress[key2] != this.QuestProgressRef.ObjectiveProgress[key2])
							{
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
				if (info.QuestProgressRef.ObjectiveProgress != null)
				{
					flag2 = false;
					goto IL_39D;
				}
			}
			if (this.QuestProgressRef.ObjectiveProgress != null && info.QuestProgressRef.ObjectiveProgress == null)
			{
				flag2 = false;
			}
			IL_39D:
			flag = flag2;
		}
		IL_39F:
		bool flag3 = false;
		if (info.QuestTemplateRef != null)
		{
			if (this.QuestTemplateRef != null)
			{
				flag3 = (this.QuestTemplateRef.Index == info.QuestTemplateRef.Index);
			}
		}
		bool flag4 = false;
		if (info.QuestRewardsRef != null && this.QuestRewardsRef != null &&
			info.QuestRewardsRef.CurrencyRewards.Count == this.QuestRewardsRef.CurrencyRewards.Count &&
			info.QuestRewardsRef.ItemRewards.Count == this.QuestRewardsRef.ItemRewards.Count &&
			info.QuestRewardsRef.UnlockRewards.Count == this.QuestRewardsRef.UnlockRewards.Count)
		{
			flag4 = true;
			
			for (int i = 0; i < info.QuestRewardsRef.CurrencyRewards.Count; i++)
			{
				if (!this.QuestRewardsRef.CurrencyRewards.ContainsWhere(delegate(QuestCurrencyReward reward)
				{
					bool result;
					if (reward.Type == info.QuestRewardsRef.CurrencyRewards[i].Type)
					{
						result = (reward.Amount == info.QuestRewardsRef.CurrencyRewards[i].Amount);
					}
					else
					{
						result = false;
					}
					return result;
				}))
				{
					flag4 = false;
					break;

				}
			}
			if (flag4)
			{
				for (int ik = 0; ik < info.QuestRewardsRef.ItemRewards.Count; ik++)
				{
					if (!this.QuestRewardsRef.ItemRewards.ContainsWhere(delegate (QuestItemReward reward)
					{
						bool result;
						if (reward.ItemTemplateId == info.QuestRewardsRef.ItemRewards[ik].ItemTemplateId)
						{
							result = (reward.Amount == info.QuestRewardsRef.ItemRewards[ik].Amount);
						}
						else
						{
							result = false;
						}
						return result;
					}))
					{
						flag4 = false;
						break;
					}
				}
				if (flag4)
				{
					int ii;
					for (ii = 0; ii < info.QuestRewardsRef.UnlockRewards.Count; ii++)
					{
						if (!this.QuestRewardsRef.UnlockRewards.ContainsWhere(delegate (QuestUnlockReward reward)
						{
							if (reward.purchaseType == info.QuestRewardsRef.UnlockRewards[ii].purchaseType &&
								reward.typeSpecificData.Length == info.QuestRewardsRef.UnlockRewards[ii].typeSpecificData.Length)
							{
								for (int ij = 0; ij < reward.typeSpecificData.Length; ij++)
								{
									if (reward.typeSpecificData[ij] != info.QuestRewardsRef.UnlockRewards[ii].typeSpecificData[ij])
									{
										return false;
									}
								}
								return true;
							}
							return false;
						}))
						{
							flag4 = false;
							break;
						}
					}
				}
			}
		}
		if (this.Completed == info.Completed)
		{
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
			if (this.QuestProgressRef.ObjectiveProgressLastDate != null && this.QuestProgressRef.ObjectiveProgress != null)
			{
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
			persistedAccountData = ClientGameManager.Get().GetPlayerAccountData();
		}
		this.QuestTemplateRef = null;
		if (-1 < QuestIndex - 1)
		{
			if (QuestIndex - 1 < QuestWideData.Get().m_quests.Count)
			{
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
				DateTime.TryParse(this.QuestTemplateRef.AbandonDateTime, out this.QuestAbandonDate);
			}
			else if (persistedAccountData != null)
			{
				if (persistedAccountData.QuestComponent.GetOrCreateQuestMetaData(QuestIndex).PstAbandonDate != null)
				{
					this.QuestAbandonDate = persistedAccountData.QuestComponent.GetOrCreateQuestMetaData(QuestIndex).PstAbandonDate.Value;
				}
			}
			return;
		}
		throw new Exception("could not find quest");
	}
}
