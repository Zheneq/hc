using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActorStats : NetworkBehaviour
{
	private delegate bool StatModFilterDelegate(StatMod statMod);
	private bool m_shouldUpdateFull;
	private Dictionary<StatType, List<StatMod>> m_statMods;
	private SyncListFloat m_modifiedStats = new SyncListFloat();
	private float[] m_modifiedStatsPrevious;
	private ActorData m_actorData;

	private static int kListm_modifiedStats = -1034899976;

	static ActorStats()
	{
		RegisterSyncListDelegate(typeof(ActorStats), kListm_modifiedStats, InvokeSyncListm_modifiedStats);
		NetworkCRC.RegisterBehaviour("ActorStats", 0);
	}

	private void MarkAllForUpdate()
	{
		m_shouldUpdateFull = true;
	}

	private void Awake()
	{
		FuncEqualityComparer<StatType> comparer = new FuncEqualityComparer<StatType>(
			(StatType a, StatType b) => a == b,
			(StatType a) => (int)a);
		m_statMods = new Dictionary<StatType, List<StatMod>>(comparer);
		for (int i = 0; i < (int)StatType.NUM; i++)
		{
			List<StatMod> value = new List<StatMod>();
			m_statMods.Add((StatType)i, value);
		}
		m_modifiedStatsPrevious = new float[(int)StatType.NUM];
		m_actorData = GetComponent<ActorData>();
		m_modifiedStats.InitializeBehaviour(this, kListm_modifiedStats);
	}

	private void Start()
	{
		for (int i = 0; i < (int)StatType.NUM; i++)
		{
			m_modifiedStatsPrevious[i] = GetStatBaseValueFloat((StatType)i);
		}
		if (NetworkServer.active)
		{
			for (int j = 0; j < (int)StatType.NUM; j++)
			{
				m_modifiedStats.Add(GetStatBaseValueFloat((StatType)j));
			}
			MarkAllForUpdate();
		}
	}

	public override void OnStartClient()
	{
		m_modifiedStats.Callback = SyncListCallbackModifiedStats;
	}

	private void Update()
	{
		if (NetworkServer.active && m_shouldUpdateFull)
		{
			SendFullUpdateData();
			m_shouldUpdateFull = false;
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
		for (int i = 0; i < (int)StatType.NUM; i++)
		{
			StatType stat = (StatType)i;
			float modifiedStatFloat = GetModifiedStatFloat(stat);
			if (modifiedStatFloat != m_modifiedStatsPrevious[i]
				&& m_modifiedStats[i] != modifiedStatFloat)
			{
				m_modifiedStats[i] = modifiedStatFloat;
				m_modifiedStatsPrevious[i] = modifiedStatFloat;
			}
		}
	}

	private void SyncListCallbackModifiedStats(SyncList<float>.Operation op, int index)
	{
		if (!NetworkClient.active || NetworkServer.active)
		{
			return;
		}

		for (int i = 0; i < m_modifiedStats.Count && i < m_modifiedStatsPrevious.Length; i++)
		{
			if (m_modifiedStats[i] != m_modifiedStatsPrevious[i])
			{
				StatType stat = (StatType)i;
				float oldStatValue = m_modifiedStatsPrevious[i];
				m_modifiedStatsPrevious[i] = m_modifiedStats[i];
				OnStatModified(stat, oldStatValue, false);
			}
		}
	}

	public void AddStatMod(AbilityStatMod statMod)
	{
		AddStatMod(statMod.stat, statMod.modType, statMod.modValue);
	}

	public void AddStatMod(StatType stat, ModType mod, float val)
	{
		if (NetworkServer.active)
		{
			float modifiedStatFloat = GetModifiedStatFloat(stat);
			StatMod statMod = new StatMod();
			statMod.Setup(mod, val);
			m_statMods[stat].Add(statMod);
			OnStatModified(stat, modifiedStatFloat, true);
		}
		else if (NetworkClient.active)
		{
			Log.Error("called AddStatMod when server is not active");
		}
	}

	public void RemoveStatMod(AbilityStatMod statMod)
	{
		RemoveStatMod(statMod.stat, statMod.modType, statMod.modValue);
	}

	public void RemoveStatMod(StatType stat, ModType mod, float val)
	{
		if (NetworkServer.active)
		{
			List<StatMod> mods = m_statMods[stat];
			foreach (StatMod current in mods)
			{
				if (current.mod == mod && current.val == val)
				{
					float modifiedStatFloat = GetModifiedStatFloat(stat);
					mods.Remove(current);
					OnStatModified(stat, modifiedStatFloat, false);
					return;
				}
			}
		}
		else if (NetworkClient.active)
		{
			Log.Error("called RemoveStat when server is not active");
		}
	}

	public float GetModifiedStatFloat(StatType stat)
	{
		float statBaseValueFloat = GetStatBaseValueFloat(stat);
		return CalculateModifiedStatValue(stat, statBaseValueFloat);
	}

	public int GetModifiedStatInt(StatType stat)
	{
		int statBaseValueInt = GetStatBaseValueInt(stat);
		return GetModifiedStatInt(stat, statBaseValueInt);
	}

	public int GetModifiedStatInt(StatType stat, int baseValue)
	{
		return Mathf.FloorToInt(CalculateModifiedStatValue(stat, (float)baseValue));
	}

	private void CalculateAdjustmentForStatMod(StatMod statMod, ref float baseAdd, ref float bonusAdd, ref float percentAdd, ref float multipliers)
	{
		switch (statMod.mod)
		{
			case ModType.BaseAdd:
				baseAdd += statMod.val;
				break;
			case ModType.BonusAdd:
				bonusAdd += statMod.val;
				break;
			case ModType.PercentAdd:
				percentAdd += statMod.val;
				break;
			case ModType.Multiplier:
				multipliers *= statMod.val;
				break;
		}
	}

	private void CalculateAdjustments(StatType stat, ref float baseAdd, ref float bonusAdd, ref float percentAdd, ref float multipliers)
	{
		if (m_statMods != null && m_statMods.ContainsKey(stat))
		{
			List<StatMod> list = m_statMods[stat];
			for (int i = 0; i < list.Count; i++)
			{
				CalculateAdjustmentForStatMod(list[i], ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers);
			}
		}
	}

	private void CalculateAdjustments(StatType stat, ref float baseAdd, ref float bonusAdd, ref float percentAdd, ref float multipliers, StatModFilterDelegate filterDelegate)
	{
		if (m_statMods != null && m_statMods.ContainsKey(stat))
		{
			List<StatMod> list = m_statMods[stat];
			for (int i = 0; i < list.Count; i++)
			{
				if (filterDelegate(list[i]))
				{
					CalculateAdjustmentForStatMod(list[i], ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers);
				}
			}
		}
	}

	private float CalculateModifiedStatValue(StatType stat, float baseValue)
	{
		if (m_statMods != null && m_statMods.ContainsKey(stat))
		{
			if (NetworkServer.active)
			{
				float baseAdd = 0f;
				float bonusAdd = 0f;
				float percentAdd = 1f;
				float multipliers = 1f;
				if (stat == StatType.Movement_Horizontal)
				{
					CalculateAdjustmentsForMovementHorizontal(ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers);
				}
				else
				{
					CalculateAdjustments(stat, ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers);
				}
				float num = baseValue;
				num += baseAdd;
				num *= percentAdd;
				num *= multipliers;
				return num + bonusAdd;
			}
			return (int)stat < m_modifiedStats.Count
				? m_modifiedStats[(int)stat]
				: m_modifiedStatsPrevious[(int)stat];
		}
		return baseValue;
	}

	public int GetStatBaseValueInt(StatType stat)
	{
		return Mathf.RoundToInt(GetStatBaseValueFloat(stat));
	}

	public float GetStatBaseValueFloat(StatType stat)
	{
		float result = 0f;
		switch (stat)
		{
			case StatType.Movement_Horizontal:
				result = GetComponent<ActorData>().m_maxHorizontalMovement;
				break;
			case StatType.Movement_Upward:
				result = GetComponent<ActorData>().m_maxVerticalUpwardMovement;
				break;
			case StatType.Movement_Downward:
				result = GetComponent<ActorData>().m_maxVerticalDownwardMovement;
				break;
			case StatType.MaxHitPoints:
				result = GetComponent<ActorData>().m_maxHitPoints;
				break;
			case StatType.HitPointRegen:
				result = GetComponent<ActorData>().m_hitPointRegen;
				break;
			case StatType.HitPointRegenPercentOfMax:
				result = 0f;
				break;
			case StatType.MaxTechPoints:
				result = GetComponent<ActorData>().m_maxTechPoints;
				break;
			case StatType.TechPointRegen:
				result = GetComponent<ActorData>().m_techPointRegen;
				break;
			case StatType.SightRange:
				result = GetComponent<ActorData>().m_sightRange;
				break;
			case StatType.CreditsPerTurn:
				result = GameplayData.Get().m_creditsPerTurn;
				break;
			case StatType.ControlPointCaptureSpeed:
				result = GameplayData.Get().m_capturePointsPerTurn;
				break;
			case StatType.CoverIncomingDamageMultiplier:
				result = GameplayData.Get().m_coverProtectionDmgMultiplier;
				break;
		}
		return result;
	}

	private void OnStatModified(StatType stat, float oldStatValue, bool addingMod)
	{
		ActorData component = GetComponent<ActorData>();
		switch (stat)
		{
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
			case StatType.Movement_Horizontal:
				GetComponent<ActorMovement>().UpdateSquaresCanMoveTo();
				break;
			case StatType.Movement_Upward:
				GetComponent<ActorMovement>().UpdateSquaresCanMoveTo();
				break;
			case StatType.Movement_Downward:
				GetComponent<ActorMovement>().UpdateSquaresCanMoveTo();
				break;
			case StatType.SightRange:
				GetComponent<FogOfWar>().MarkForRecalculateVisibility();
				break;
		}
		if (NetworkServer.active)
		{
			MarkAllForUpdate();
		}
		Board.Get().MarkForUpdateValidSquares();
	}

	public int CalculateOutgoingDamageForTargeter(int baseDamage)
	{
		int damage = baseDamage;
		ActorStatus actorStatus = m_actorData.GetActorStatus();
		bool isEmpowered = actorStatus.HasStatus(StatusType.Empowered)
			|| (m_actorData.GetAbilityData() != null
				&& m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Empowered));
		bool isWeakened = actorStatus.HasStatus(StatusType.Weakened);
		if (isEmpowered && !isWeakened)
		{
			AbilityModPropertyInt empoweredOutgoingDamageMod;
			if (GameplayMutators.Get() != null && GameplayMutators.Get().m_useEmpoweredOverride)
			{
				empoweredOutgoingDamageMod = GameplayMutators.Get().m_empoweredOutgoingDamageMod;
			}
			else
			{
				empoweredOutgoingDamageMod = GameWideData.Get().m_empoweredOutgoingDamageMod;
			}
			damage = empoweredOutgoingDamageMod.GetModifiedValue(baseDamage);
		}
		else if (!isEmpowered && isWeakened)
		{
			AbilityModPropertyInt abilityModPropertyInt;
			if (GameplayMutators.Get() != null && GameplayMutators.Get().m_useWeakenedOverride)
			{
				abilityModPropertyInt = GameplayMutators.Get().m_weakenedOutgoingDamageMod;
			}
			else
			{
				abilityModPropertyInt = GameWideData.Get().m_weakenedOutgoingDamageMod;
			}
			damage = abilityModPropertyInt.GetModifiedValue(baseDamage);
		}
		return Mathf.Max(0, damage);
	}

	public int CalculateOutgoingHealForTargeter(int baseHeal)
	{
		int heal = baseHeal;
		ActorStatus actorStatus = m_actorData.GetActorStatus();
		bool isEmpowered = actorStatus.HasStatus(StatusType.Empowered)
			|| m_actorData.GetAbilityData() != null
				&& m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Empowered);
		bool isWeakened = actorStatus.HasStatus(StatusType.Weakened);
		if (isEmpowered && !isWeakened)
		{
			AbilityModPropertyInt empoweredOutgoingHealingMod;
			if (GameplayMutators.Get() != null && GameplayMutators.Get().m_useEmpoweredOverride)
			{
				empoweredOutgoingHealingMod = GameplayMutators.Get().m_empoweredOutgoingHealingMod;
			}
			else
			{
				empoweredOutgoingHealingMod = GameWideData.Get().m_empoweredOutgoingHealingMod;
			}
			heal = empoweredOutgoingHealingMod.GetModifiedValue(baseHeal);
		}
		else if (!isEmpowered && isWeakened)
		{
			AbilityModPropertyInt abilityModPropertyInt;
			if (GameplayMutators.Get() != null && GameplayMutators.Get().m_useWeakenedOverride)
			{
				abilityModPropertyInt = GameplayMutators.Get().m_weakenedOutgoingHealingMod;
			}
			else
			{
				abilityModPropertyInt = GameWideData.Get().m_weakenedOutgoingHealingMod;
			}
			heal = abilityModPropertyInt.GetModifiedValue(baseHeal);
		}
		return Mathf.Max(0, heal);
	}

	public int CalculateOutgoingAbsorbForTargeter(int baseAbsorb)
	{
		int absorb = baseAbsorb;
		ActorStatus actorStatus = m_actorData.GetActorStatus();
		bool isEmpowered = actorStatus.HasStatus(StatusType.Empowered)
			|| (m_actorData.GetAbilityData() != null
				&& m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Empowered));
		bool isWeakened = actorStatus.HasStatus(StatusType.Weakened);
		if (isEmpowered && !isWeakened)
		{
			AbilityModPropertyInt abilityModPropertyInt;
			if (GameplayMutators.Get() != null && GameplayMutators.Get().m_useEmpoweredOverride)
			{
				abilityModPropertyInt = GameplayMutators.Get().m_empoweredOutgoingAbsorbMod;
			}
			else
			{
				abilityModPropertyInt = GameWideData.Get().m_empoweredOutgoingAbsorbMod;
			}
			absorb = abilityModPropertyInt.GetModifiedValue(baseAbsorb);
		}
		else if (!isEmpowered && isWeakened)
		{
			AbilityModPropertyInt weakenedOutgoingAbsorbMod;
			if (GameplayMutators.Get() != null && GameplayMutators.Get().m_useWeakenedOverride)
			{
				weakenedOutgoingAbsorbMod = GameplayMutators.Get().m_weakenedOutgoingAbsorbMod;
			}
			else
			{
				weakenedOutgoingAbsorbMod = GameWideData.Get().m_weakenedOutgoingAbsorbMod;
			}
			absorb = weakenedOutgoingAbsorbMod.GetModifiedValue(baseAbsorb);
		}
		return Mathf.Max(0, absorb);
	}

	public int CalculateIncomingDamageForTargeter(int baseDamage)
	{
		int damage = baseDamage;
		ActorStatus component = GetComponent<ActorStatus>();
		bool isVulnerable = component.HasStatus(StatusType.Vulnerable);
		bool isArmored = component.HasStatus(StatusType.Armored);
		if (isVulnerable && !isArmored)
		{
			float vulnerableDamageMultiplier;
			int vulnerableDamageFlatAdd;
			if (GameplayMutators.Get() != null && GameplayMutators.Get().m_useVulnerableOverride)
			{
				vulnerableDamageMultiplier = GameplayMutators.Get().m_vulnerableDamageMultiplier;
				vulnerableDamageFlatAdd = GameplayMutators.Get().m_vulnerableDamageFlatAdd;
			}
			else
			{
				vulnerableDamageMultiplier = GameWideData.Get().m_vulnerableDamageMultiplier;
				vulnerableDamageFlatAdd = GameWideData.Get().m_vulnerableDamageFlatAdd;
			}
			if (vulnerableDamageMultiplier > 0f)
			{
				damage = MathUtil.RoundToIntPadded(baseDamage * vulnerableDamageMultiplier);
			}
			if (vulnerableDamageFlatAdd > 0 && baseDamage > 0)
			{
				damage += vulnerableDamageFlatAdd;
			}
		}
		else if (!isVulnerable && isArmored)
		{
			AbilityModPropertyInt armoredIncomingDamageMod;
			if (GameplayMutators.Get() != null && GameplayMutators.Get().m_useArmoredOverride)
			{
				armoredIncomingDamageMod = GameplayMutators.Get().m_armoredIncomingDamageMod;
			}
			else
			{
				armoredIncomingDamageMod = GameWideData.Get().m_armoredIncomingDamageMod;
			}
			damage = armoredIncomingDamageMod.GetModifiedValue(baseDamage);
		}
		return Mathf.Max(0, damage);
	}

	public int CalculateLifeOnDamage(int finalDamage)
	{
		float modifiedStatFloat = GetModifiedStatFloat(StatType.LifestealPerHit);
		float num = GetModifiedStatFloat(StatType.LifestealPerDamage) * finalDamage;
		return MathUtil.RoundToIntPadded(modifiedStatFloat + num);
	}

	public void CalculateAdjustmentsForMovementHorizontal(ref float baseAdd, ref float bonusAdd, ref float percentAdd, ref float multipliers)
	{
		if (GetComponent<ActorStatus>().HasStatus(StatusType.MovementDebuffSuppression))
		{
			StatModFilterDelegate filterDelegate = delegate (StatMod statMod)
			{
				return statMod.mod == ModType.Multiplier ? statMod.val >= 1f : statMod.val > 0f;
			};
			CalculateAdjustments(StatType.Movement_Horizontal, ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers, filterDelegate);
		}
		else
		{
			CalculateAdjustments(StatType.Movement_Horizontal, ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers);
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
		Log.Info($"[JSON] {{\"modifiedStats\":{DefaultJsonSerializer.Serialize(((ActorStats)obj).m_modifiedStats)}}}");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListFloat.WriteInstance(writer, m_modifiedStats);
			return true;
		}
		bool flag = false;
		if ((syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListFloat.WriteInstance(writer, m_modifiedStats);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListFloat.ReadReference(reader, m_modifiedStats);
			LogJson();
			return;
		}
		int mask = (int)reader.ReadPackedUInt32();
		if ((mask & 1) != 0)
		{
			SyncListFloat.ReadReference(reader, m_modifiedStats);
		}
		LogJson(mask);
	}

	private void LogJson(int mask = Int32.MaxValue)
	{
		var jsonLog = new List<string>();
		if ((mask & 1) != 0)
		{
			jsonLog.Add($"\"modifiedStats\":{DefaultJsonSerializer.Serialize(m_modifiedStats)}");
		}

		Log.Info($"[JSON] {{\"actorStats\":{{{String.Join(",", jsonLog.ToArray())}}}}}");
	}
}
