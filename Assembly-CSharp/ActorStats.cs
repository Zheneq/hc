using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActorStats : NetworkBehaviour
{
	private bool m_shouldUpdateFull;

	private Dictionary<StatType, List<StatMod>> m_statMods;

	private SyncListFloat m_modifiedStats = new SyncListFloat();

	private float[] m_modifiedStatsPrevious;

	private ActorData m_actorData;

	private static int kListm_modifiedStats = -0x3DAF5208;

	static ActorStats()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorStats), ActorStats.kListm_modifiedStats, new NetworkBehaviour.CmdDelegate(ActorStats.InvokeSyncListm_modifiedStats));
		NetworkCRC.RegisterBehaviour("ActorStats", 0);
	}

	private void MarkAllForUpdate()
	{
		this.m_shouldUpdateFull = true;
	}

	private void Awake()
	{
		
		Func<StatType, StatType, bool> equals = ((StatType a, StatType b) => a == b);
		
		FuncEqualityComparer<StatType> comparer = new FuncEqualityComparer<StatType>(equals, ((StatType a) => (int)a));
		this.m_statMods = new Dictionary<StatType, List<StatMod>>(comparer);
		for (int i = 0; i < 0x18; i++)
		{
			List<StatMod> value = new List<StatMod>();
			this.m_statMods.Add((StatType)i, value);
		}
		this.m_modifiedStatsPrevious = new float[0x18];
		this.m_actorData = base.GetComponent<ActorData>();
		this.m_modifiedStats.InitializeBehaviour(this, ActorStats.kListm_modifiedStats);
	}

	private void Start()
	{
		for (int i = 0; i < 0x18; i++)
		{
			this.m_modifiedStatsPrevious[i] = this.GetStatBaseValueFloat((StatType)i);
		}
		if (NetworkServer.active)
		{
			for (int j = 0; j < 0x18; j++)
			{
				this.m_modifiedStats.Add(this.GetStatBaseValueFloat((StatType)j));
			}
			this.MarkAllForUpdate();
		}
	}

	public override void OnStartClient()
	{
		this.m_modifiedStats.Callback = new SyncList<float>.SyncListChanged(this.SyncListCallbackModifiedStats);
	}

	private void Update()
	{
		if (NetworkServer.active && this.m_shouldUpdateFull)
		{
			this.SendFullUpdateData();
			this.m_shouldUpdateFull = false;
		}
	}

	[Server]
	private void SendFullUpdateData()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorStats::SendFullUpdateData()' called on client");
			return;
		}
		for (int i = 0; i < 0x18; i++)
		{
			StatType stat = (StatType)i;
			float modifiedStatFloat = this.GetModifiedStatFloat(stat);
			if (modifiedStatFloat != this.m_modifiedStatsPrevious[i])
			{
				if (this.m_modifiedStats[i] != modifiedStatFloat)
				{
					this.m_modifiedStats[i] = modifiedStatFloat;
					this.m_modifiedStatsPrevious[i] = modifiedStatFloat;
				}
			}
		}
	}

	private void SyncListCallbackModifiedStats(SyncList<float>.Operation op, int _incorrectIndexBugIn51And52)
	{
		if (NetworkClient.active)
		{
			if (!NetworkServer.active)
			{
				for (int i = 0; i < this.m_modifiedStats.Count; i++)
				{
					if (i >= this.m_modifiedStatsPrevious.Length)
					{
						break;
					}
					if (this.m_modifiedStats[i] != this.m_modifiedStatsPrevious[i])
					{
						StatType stat = (StatType)i;
						float oldStatValue = this.m_modifiedStatsPrevious[i];
						this.m_modifiedStatsPrevious[i] = this.m_modifiedStats[i];
						this.OnStatModified(stat, oldStatValue, false);
					}
				}
			}
		}
	}

	public void AddStatMod(AbilityStatMod statMod)
	{
		this.AddStatMod(statMod.stat, statMod.modType, statMod.modValue);
	}

	public void AddStatMod(StatType stat, ModType mod, float val)
	{
		if (NetworkServer.active)
		{
			float modifiedStatFloat = this.GetModifiedStatFloat(stat);
			StatMod statMod = new StatMod();
			statMod.Setup(mod, val);
			this.m_statMods[stat].Add(statMod);
			this.OnStatModified(stat, modifiedStatFloat, true);
		}
		else if (NetworkClient.active)
		{
			Log.Error("called AddStatMod when server is not active", new object[0]);
		}
	}

	public void RemoveStatMod(AbilityStatMod statMod)
	{
		this.RemoveStatMod(statMod.stat, statMod.modType, statMod.modValue);
	}

	public void RemoveStatMod(StatType stat, ModType mod, float val)
	{
		if (NetworkServer.active)
		{
			List<StatMod> list = this.m_statMods[stat];
			using (List<StatMod>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					StatMod statMod = enumerator.Current;
					if (statMod.mod == mod)
					{
						if (statMod.val == val)
						{
							float modifiedStatFloat = this.GetModifiedStatFloat(stat);
							list.Remove(statMod);
							this.OnStatModified(stat, modifiedStatFloat, false);
							goto IL_AD;
						}
					}
				}
			}
			IL_AD:;
		}
		else if (NetworkClient.active)
		{
			Log.Error("called RemoveStat when server is not active", new object[0]);
		}
	}

	public float GetModifiedStatFloat(StatType stat)
	{
		float statBaseValueFloat = this.GetStatBaseValueFloat(stat);
		return this.CalculateModifiedStatValue(stat, statBaseValueFloat);
	}

	public int GetModifiedStatInt(StatType stat)
	{
		int statBaseValueInt = this.GetStatBaseValueInt(stat);
		return this.GetModifiedStatInt(stat, statBaseValueInt);
	}

	public int GetModifiedStatInt(StatType stat, int baseValue)
	{
		float baseValue2 = (float)baseValue;
		float f = this.CalculateModifiedStatValue(stat, baseValue2);
		return Mathf.FloorToInt(f);
	}

	private void CalculateAdjustmentForStatMod(StatMod statMod, ref float baseAdd, ref float bonusAdd, ref float percentAdd, ref float multipliers)
	{
		switch (statMod.mod)
		{
		case ModType.BaseAdd:
			baseAdd += statMod.val;
			break;
		case ModType.PercentAdd:
			percentAdd += statMod.val;
			break;
		case ModType.Multiplier:
			multipliers *= statMod.val;
			break;
		case ModType.BonusAdd:
			bonusAdd += statMod.val;
			break;
		}
	}

	private unsafe void CalculateAdjustments(StatType stat, ref float baseAdd, ref float bonusAdd, ref float percentAdd, ref float multipliers)
	{
		if (this.m_statMods != null)
		{
			if (this.m_statMods.ContainsKey(stat))
			{
				List<StatMod> list = this.m_statMods[stat];
				for (int i = 0; i < list.Count; i++)
				{
					this.CalculateAdjustmentForStatMod(list[i], ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers);
				}
			}
		}
	}

	private unsafe void CalculateAdjustments(StatType stat, ref float baseAdd, ref float bonusAdd, ref float percentAdd, ref float multipliers, ActorStats.StatModFilterDelegate filterDelegate)
	{
		if (this.m_statMods != null)
		{
			if (this.m_statMods.ContainsKey(stat))
			{
				List<StatMod> list = this.m_statMods[stat];
				for (int i = 0; i < list.Count; i++)
				{
					if (filterDelegate(list[i]))
					{
						this.CalculateAdjustmentForStatMod(list[i], ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers);
					}
				}
			}
		}
	}

	private float CalculateModifiedStatValue(StatType stat, float baseValue)
	{
		if (this.m_statMods != null)
		{
			if (this.m_statMods.ContainsKey(stat))
			{
				if (NetworkServer.active)
				{
					float num = 0f;
					float num2 = 0f;
					float num3 = 1f;
					float num4 = 1f;
					if (stat == StatType.Movement_Horizontal)
					{
						this.CalculateAdjustmentsForMovementHorizontal(ref num, ref num2, ref num3, ref num4);
					}
					else
					{
						this.CalculateAdjustments(stat, ref num, ref num2, ref num3, ref num4);
					}
					float num5 = baseValue + num;
					num5 *= num3;
					num5 *= num4;
					return num5 + num2;
				}
				float result;
				if (stat < (StatType)this.m_modifiedStats.Count)
				{
					result = this.m_modifiedStats[(int)stat];
				}
				else
				{
					result = this.m_modifiedStatsPrevious[(int)stat];
				}
				return result;
			}
		}
		return baseValue;
	}

	public int GetStatBaseValueInt(StatType stat)
	{
		float statBaseValueFloat = this.GetStatBaseValueFloat(stat);
		return Mathf.RoundToInt(statBaseValueFloat);
	}

	public float GetStatBaseValueFloat(StatType stat)
	{
		float result = 0f;
		switch (stat)
		{
		case StatType.Movement_Horizontal:
			result = base.GetComponent<ActorData>().m_maxHorizontalMovement;
			break;
		case StatType.Movement_Upward:
			result = (float)base.GetComponent<ActorData>().m_maxVerticalUpwardMovement;
			break;
		case StatType.Movement_Downward:
			result = (float)base.GetComponent<ActorData>().m_maxVerticalDownwardMovement;
			break;
		case StatType.MaxHitPoints:
			result = (float)base.GetComponent<ActorData>().m_maxHitPoints;
			break;
		case StatType.MaxTechPoints:
			result = (float)base.GetComponent<ActorData>().m_maxTechPoints;
			break;
		case StatType.HitPointRegen:
			result = (float)base.GetComponent<ActorData>().m_hitPointRegen;
			break;
		case StatType.TechPointRegen:
			result = (float)base.GetComponent<ActorData>().m_techPointRegen;
			break;
		case StatType.SightRange:
			result = base.GetComponent<ActorData>().m_sightRange;
			break;
		case StatType.CreditsPerTurn:
			result = (float)GameplayData.Get().m_creditsPerTurn;
			break;
		case StatType.ControlPointCaptureSpeed:
			result = (float)GameplayData.Get().m_capturePointsPerTurn;
			break;
		case StatType.CoverIncomingDamageMultiplier:
			result = GameplayData.Get().m_coverProtectionDmgMultiplier;
			break;
		case StatType.HitPointRegenPercentOfMax:
			result = 0f;
			break;
		}
		return result;
	}

	private void OnStatModified(StatType stat, float oldStatValue, bool addingMod)
	{
		ActorData component = base.GetComponent<ActorData>();
		switch (stat)
		{
		case StatType.Movement_Horizontal:
			base.GetComponent<ActorMovement>().UpdateSquaresCanMoveTo();
			break;
		case StatType.Movement_Upward:
			base.GetComponent<ActorMovement>().UpdateSquaresCanMoveTo();
			break;
		case StatType.Movement_Downward:
			base.GetComponent<ActorMovement>().UpdateSquaresCanMoveTo();
			break;
		case StatType.MaxHitPoints:
			if (NetworkServer.active)
			{
				component.OnMaxHitPointsChanged((int)oldStatValue);
			}
			break;
		case StatType.MaxTechPoints:
			if (NetworkServer.active)
			{
				component.OnMaxHitPointsChanged((int)oldStatValue);
			}
			break;
		case StatType.SightRange:
			base.GetComponent<FogOfWar>().MarkForRecalculateVisibility();
			break;
		}
		if (NetworkServer.active)
		{
			this.MarkAllForUpdate();
		}
		Board.Get().MarkForUpdateValidSquares(true);
	}

	public int CalculateOutgoingDamageForTargeter(int baseDamage)
	{
		int b = baseDamage;
		ActorStatus actorStatus = this.m_actorData.GetActorStatus();
		bool flag;
		if (!actorStatus.HasStatus(StatusType.Empowered, true))
		{
			if (this.m_actorData.GetAbilityData() != null)
			{
				flag = this.m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Empowered);
			}
			else
			{
				flag = false;
			}
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		bool flag3 = actorStatus.HasStatus(StatusType.Weakened, true);
		if (flag2)
		{
			if (!flag3)
			{
				AbilityModPropertyInt empoweredOutgoingDamageMod;
				if (!(GameplayMutators.Get() == null))
				{
					if (GameplayMutators.Get().m_useEmpoweredOverride)
					{
						empoweredOutgoingDamageMod = GameplayMutators.Get().m_empoweredOutgoingDamageMod;
						goto IL_D7;
					}
				}
				empoweredOutgoingDamageMod = GameWideData.Get().m_empoweredOutgoingDamageMod;
				IL_D7:
				b = empoweredOutgoingDamageMod.GetModifiedValue(baseDamage);
				goto IL_142;
			}
		}
		if (!flag2)
		{
			if (flag3)
			{
				AbilityModPropertyInt weakenedOutgoingDamageMod;
				if (GameplayMutators.Get() == null || !GameplayMutators.Get().m_useWeakenedOverride)
				{
					weakenedOutgoingDamageMod = GameWideData.Get().m_weakenedOutgoingDamageMod;
				}
				else
				{
					weakenedOutgoingDamageMod = GameplayMutators.Get().m_weakenedOutgoingDamageMod;
				}
				b = weakenedOutgoingDamageMod.GetModifiedValue(baseDamage);
			}
		}
		IL_142:
		return Mathf.Max(0, b);
	}

	public int CalculateOutgoingHealForTargeter(int baseHeal)
	{
		int b = baseHeal;
		ActorStatus actorStatus = this.m_actorData.GetActorStatus();
		bool flag;
		if (!actorStatus.HasStatus(StatusType.Empowered, true))
		{
			flag = (this.m_actorData.GetAbilityData() != null && this.m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Empowered));
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		bool flag3 = actorStatus.HasStatus(StatusType.Weakened, true);
		if (flag2)
		{
			if (!flag3)
			{
				AbilityModPropertyInt empoweredOutgoingHealingMod;
				if (!(GameplayMutators.Get() == null))
				{
					if (GameplayMutators.Get().m_useEmpoweredOverride)
					{
						empoweredOutgoingHealingMod = GameplayMutators.Get().m_empoweredOutgoingHealingMod;
						goto IL_D8;
					}
				}
				empoweredOutgoingHealingMod = GameWideData.Get().m_empoweredOutgoingHealingMod;
				IL_D8:
				b = empoweredOutgoingHealingMod.GetModifiedValue(baseHeal);
				goto IL_139;
			}
		}
		if (!flag2)
		{
			if (flag3)
			{
				AbilityModPropertyInt weakenedOutgoingHealingMod;
				if (GameplayMutators.Get() == null || !GameplayMutators.Get().m_useWeakenedOverride)
				{
					weakenedOutgoingHealingMod = GameWideData.Get().m_weakenedOutgoingHealingMod;
				}
				else
				{
					weakenedOutgoingHealingMod = GameplayMutators.Get().m_weakenedOutgoingHealingMod;
				}
				b = weakenedOutgoingHealingMod.GetModifiedValue(baseHeal);
			}
		}
		IL_139:
		return Mathf.Max(0, b);
	}

	public int CalculateOutgoingAbsorbForTargeter(int baseAbsorb)
	{
		int b = baseAbsorb;
		ActorStatus actorStatus = this.m_actorData.GetActorStatus();
		bool flag;
		if (!actorStatus.HasStatus(StatusType.Empowered, true))
		{
			if (this.m_actorData.GetAbilityData() != null)
			{
				flag = this.m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Empowered);
			}
			else
			{
				flag = false;
			}
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		bool flag3 = actorStatus.HasStatus(StatusType.Weakened, true);
		if (flag2)
		{
			if (!flag3)
			{
				AbilityModPropertyInt empoweredOutgoingAbsorbMod;
				if (GameplayMutators.Get() == null || !GameplayMutators.Get().m_useEmpoweredOverride)
				{
					empoweredOutgoingAbsorbMod = GameWideData.Get().m_empoweredOutgoingAbsorbMod;
				}
				else
				{
					empoweredOutgoingAbsorbMod = GameplayMutators.Get().m_empoweredOutgoingAbsorbMod;
				}
				b = empoweredOutgoingAbsorbMod.GetModifiedValue(baseAbsorb);
				goto IL_141;
			}
		}
		if (!flag2)
		{
			if (flag3)
			{
				AbilityModPropertyInt weakenedOutgoingAbsorbMod;
				if (!(GameplayMutators.Get() == null))
				{
					if (GameplayMutators.Get().m_useWeakenedOverride)
					{
						weakenedOutgoingAbsorbMod = GameplayMutators.Get().m_weakenedOutgoingAbsorbMod;
						goto IL_136;
					}
				}
				weakenedOutgoingAbsorbMod = GameWideData.Get().m_weakenedOutgoingAbsorbMod;
				IL_136:
				b = weakenedOutgoingAbsorbMod.GetModifiedValue(baseAbsorb);
			}
		}
		IL_141:
		return Mathf.Max(0, b);
	}

	public int CalculateIncomingDamageForTargeter(int baseDamage)
	{
		int num = baseDamage;
		ActorStatus component = base.GetComponent<ActorStatus>();
		bool flag = component.HasStatus(StatusType.Vulnerable, true);
		bool flag2 = component.HasStatus(StatusType.Armored, true);
		if (flag)
		{
			if (!flag2)
			{
				float vulnerableDamageMultiplier;
				int vulnerableDamageFlatAdd;
				if (!(GameplayMutators.Get() == null))
				{
					if (GameplayMutators.Get().m_useVulnerableOverride)
					{
						vulnerableDamageMultiplier = GameplayMutators.Get().m_vulnerableDamageMultiplier;
						vulnerableDamageFlatAdd = GameplayMutators.Get().m_vulnerableDamageFlatAdd;
						goto IL_B1;
					}
				}
				vulnerableDamageMultiplier = GameWideData.Get().m_vulnerableDamageMultiplier;
				vulnerableDamageFlatAdd = GameWideData.Get().m_vulnerableDamageFlatAdd;
				IL_B1:
				if (vulnerableDamageMultiplier > 0f)
				{
					num = MathUtil.RoundToIntPadded((float)baseDamage * vulnerableDamageMultiplier);
				}
				if (vulnerableDamageFlatAdd > 0)
				{
					if (baseDamage > 0)
					{
						num += vulnerableDamageFlatAdd;
					}
				}
				goto IL_14B;
			}
		}
		if (!flag)
		{
			if (flag2)
			{
				AbilityModPropertyInt armoredIncomingDamageMod;
				if (!(GameplayMutators.Get() == null))
				{
					if (GameplayMutators.Get().m_useArmoredOverride)
					{
						armoredIncomingDamageMod = GameplayMutators.Get().m_armoredIncomingDamageMod;
						goto IL_140;
					}
				}
				armoredIncomingDamageMod = GameWideData.Get().m_armoredIncomingDamageMod;
				IL_140:
				num = armoredIncomingDamageMod.GetModifiedValue(baseDamage);
			}
		}
		IL_14B:
		return Mathf.Max(0, num);
	}

	public int CalculateLifeOnDamage(int finalDamage)
	{
		float modifiedStatFloat = this.GetModifiedStatFloat(StatType.LifestealPerHit);
		float num = this.GetModifiedStatFloat(StatType.LifestealPerDamage) * (float)finalDamage;
		return MathUtil.RoundToIntPadded(modifiedStatFloat + num);
	}

	public unsafe void CalculateAdjustmentsForMovementHorizontal(ref float baseAdd, ref float bonusAdd, ref float percentAdd, ref float multipliers)
	{
		ActorStatus component = base.GetComponent<ActorStatus>();
		if (component.HasStatus(StatusType.MovementDebuffSuppression, true))
		{
			
			ActorStats.StatModFilterDelegate filterDelegate = delegate(StatMod statMod)
				{
					bool flag;
					if (statMod.mod == ModType.Multiplier)
					{
						flag = (statMod.val < 1f);
					}
					else
					{
						flag = (statMod.val < 0f);
					}
					return !flag;
				};
			this.CalculateAdjustments(StatType.Movement_Horizontal, ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers, filterDelegate);
		}
		else
		{
			this.CalculateAdjustments(StatType.Movement_Horizontal, ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers);
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_modifiedStats(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_modifiedStats called on server.");
			return;
		}
		((ActorStats)obj).m_modifiedStats.HandleMsg(reader);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListFloat.WriteInstance(writer, this.m_modifiedStats);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListFloat.WriteInstance(writer, this.m_modifiedStats);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListFloat.ReadReference(reader, this.m_modifiedStats);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListFloat.ReadReference(reader, this.m_modifiedStats);
		}
	}

	private delegate bool StatModFilterDelegate(StatMod statMod);
}
