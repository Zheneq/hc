// ROGUES
// SERVER
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

	// removed in rogues
	private static int kListm_modifiedStats = -1034899976;

	// removed in rogues
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

		// removed in rogues
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
		// reactor
		m_modifiedStats.Callback = SyncListCallbackModifiedStats;
		// rogues
		//m_modifiedStats.Callback += new SyncList<float>.SyncListChanged(this.SyncListCallbackModifiedStats);
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

	private void SyncListCallbackModifiedStats(SyncList<float>.Operation op, int index)  // , float item in rogues
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
			case StatType.CreditsPerTurn:  // removed in rogues
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
					// NOTE CHANGE
					component.OnMaxTechPointsChanged((int)oldStatValue);  // was bugged in reactor: OnMaxHitPointsChanged was called. Fixed in rogues
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
				// unconditional in reactor
				//if (NetworkServer.active)
				//{
				GetComponent<FogOfWar>().MarkForRecalculateVisibility();
				//}
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

	// added in rogues
#if SERVER
	public int CalculateOutgoingDamage(int baseDamage, bool casterInCoverWrtTarget, bool targetInCoverWrtCaster, out int modifiedDamageNormal, out int modifiedDamageEmpowered, out int modifiedDamageWeakened)
	{
		if (!NetworkServer.active)
		{
			if (NetworkClient.active)
			{
				Debug.LogError("Client calling CalculateOutgoingDamage.  Only the server should call this.");
			}
			modifiedDamageNormal = baseDamage;
			modifiedDamageEmpowered = baseDamage;
			modifiedDamageWeakened = baseDamage;
			return baseDamage;
		}
		AbilityModPropertyInt empoweredOutgoingDamageMod;
		if (GameplayMutators.Get() == null || !GameplayMutators.Get().m_useEmpoweredOverride)
		{
			empoweredOutgoingDamageMod = GameWideData.Get().m_empoweredOutgoingDamageMod;
		}
		else
		{
			empoweredOutgoingDamageMod = GameplayMutators.Get().m_empoweredOutgoingDamageMod;
		}
		AbilityModPropertyInt weakenedOutgoingDamageMod;
		if (GameplayMutators.Get() == null || !GameplayMutators.Get().m_useWeakenedOverride)
		{
			weakenedOutgoingDamageMod = GameWideData.Get().m_weakenedOutgoingDamageMod;
		}
		else
		{
			weakenedOutgoingDamageMod = GameplayMutators.Get().m_weakenedOutgoingDamageMod;
		}
		float num = 0f;
		float num2 = 0f;
		float num3 = 1f;
		float num4 = 1f;
		CalculateAdjustments(StatType.OutgoingDamage, ref num, ref num2, ref num3, ref num4);
		if (casterInCoverWrtTarget)
		{
			CalculateAdjustments(StatType.OutgoingDamage_FromCover, ref num, ref num2, ref num3, ref num4);
		}
		if (targetInCoverWrtCaster)
		{
			CalculateAdjustments(StatType.OutgoingDamage_ToCover, ref num, ref num2, ref num3, ref num4);
		}
		float num5 = baseDamage;
		num5 += num;
		num5 *= num3;
		num5 *= num4;
		num5 += num2;
		modifiedDamageNormal = Mathf.RoundToInt(num5);
		float num6 = num;
		float num7 = num4;
		if (empoweredOutgoingDamageMod.operation == AbilityModPropertyInt.ModOp.Add)
		{
			num6 += empoweredOutgoingDamageMod.value;
		}
		else if (empoweredOutgoingDamageMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndCeil || empoweredOutgoingDamageMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndFloor || empoweredOutgoingDamageMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndRound)
		{
			num7 *= empoweredOutgoingDamageMod.value;
		}
		float num8 = baseDamage;
		num8 += num6;
		num8 *= num3;
		num8 *= num7;
		num8 += num2;
		modifiedDamageEmpowered = Mathf.RoundToInt(num8);
		float num9 = num;
		float num10 = num4;
		if (weakenedOutgoingDamageMod.operation == AbilityModPropertyInt.ModOp.Add)
		{
			num9 += weakenedOutgoingDamageMod.value;
		}
		else if (weakenedOutgoingDamageMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndCeil || weakenedOutgoingDamageMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndFloor || weakenedOutgoingDamageMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndRound)
		{
			num10 *= weakenedOutgoingDamageMod.value;
		}
		float num11 = baseDamage;
		num11 += num9;
		num11 *= num3;
		num11 *= num10;
		num11 += num2;
		modifiedDamageWeakened = Mathf.RoundToInt(num11);
		ActorStatus component = GetComponent<ActorStatus>();
		bool flag = component.HasStatus(StatusType.Empowered, true);
		bool flag2 = component.HasStatus(StatusType.Weakened, true);
		if (flag && !flag2)
		{
			return modifiedDamageEmpowered;
		}
		if (!flag && flag2)
		{
			return modifiedDamageWeakened;
		}
		return modifiedDamageNormal;
	}
#endif

	// added in rogues
#if SERVER
	public int CalculateOutgoingHealing(int baseHealing, out int modifiedHealingNormal, out int modifiedHealingEmpowered, out int modifiedHealingWeakened)
	{
		if (!NetworkServer.active)
		{
			if (NetworkClient.active)
			{
				Debug.LogError("Client calling CalculateOutgoingHealing.  Only the server should call this.");
			}
			modifiedHealingNormal = baseHealing;
			modifiedHealingEmpowered = baseHealing;
			modifiedHealingWeakened = baseHealing;
			return baseHealing;
		}
		AbilityModPropertyInt empoweredOutgoingHealingMod;
		if (GameplayMutators.Get() == null || !GameplayMutators.Get().m_useEmpoweredOverride)
		{
			empoweredOutgoingHealingMod = GameWideData.Get().m_empoweredOutgoingHealingMod;
		}
		else
		{
			empoweredOutgoingHealingMod = GameplayMutators.Get().m_empoweredOutgoingHealingMod;
		}
		AbilityModPropertyInt weakenedOutgoingHealingMod;
		if (GameplayMutators.Get() == null || !GameplayMutators.Get().m_useWeakenedOverride)
		{
			weakenedOutgoingHealingMod = GameWideData.Get().m_weakenedOutgoingHealingMod;
		}
		else
		{
			weakenedOutgoingHealingMod = GameplayMutators.Get().m_weakenedOutgoingHealingMod;
		}
		float num = 0f;
		float num2 = 0f;
		float num3 = 1f;
		float num4 = 1f;
		CalculateAdjustments(StatType.OutgoingHealing, ref num, ref num2, ref num3, ref num4);
		float num5 = baseHealing;
		num5 += num;
		num5 *= num3;
		num5 *= num4;
		num5 += num2;
		modifiedHealingNormal = Mathf.RoundToInt(num5);
		float num6 = num;
		float num7 = num4;
		if (empoweredOutgoingHealingMod.operation == AbilityModPropertyInt.ModOp.Add)
		{
			num6 += empoweredOutgoingHealingMod.value;
		}
		else if (empoweredOutgoingHealingMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndCeil || empoweredOutgoingHealingMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndFloor || empoweredOutgoingHealingMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndRound)
		{
			num7 *= empoweredOutgoingHealingMod.value;
		}
		float num8 = baseHealing;
		num8 += num6;
		num8 *= num3;
		num8 *= num7;
		num8 += num2;
		modifiedHealingEmpowered = Mathf.RoundToInt(num8);
		float num9 = num;
		float num10 = num4;
		if (weakenedOutgoingHealingMod.operation == AbilityModPropertyInt.ModOp.Add)
		{
			num9 += weakenedOutgoingHealingMod.value;
		}
		else if (weakenedOutgoingHealingMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndCeil || weakenedOutgoingHealingMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndFloor || weakenedOutgoingHealingMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndRound)
		{
			num10 *= weakenedOutgoingHealingMod.value;
		}
		float num11 = baseHealing;
		num11 += num9;
		num11 *= num3;
		num11 *= num10;
		num11 += num2;
		modifiedHealingWeakened = Mathf.RoundToInt(num11);
		ActorStatus component = GetComponent<ActorStatus>();
		bool flag = component.HasStatus(StatusType.Empowered, true);
		bool flag2 = component.HasStatus(StatusType.Weakened, true);
		if (flag && !flag2)
		{
			return modifiedHealingEmpowered;
		}
		if (!flag && flag2)
		{
			return modifiedHealingWeakened;
		}
		return modifiedHealingNormal;
	}
#endif


	// added in rogues
#if SERVER
	public int CalculateOutgoingAbsorb(int baseAbsorb, out int modifiedAbsorbNormal, out int modifiedAbsorbEmpowered, out int modifiedAbsorbWeakened)
	{
		if (!NetworkServer.active)
		{
			if (NetworkClient.active)
			{
				Debug.LogError("Client calling CalculateOutgoingAbsorb.  Only the server should call this.");
			}
			modifiedAbsorbNormal = baseAbsorb;
			modifiedAbsorbEmpowered = baseAbsorb;
			modifiedAbsorbWeakened = baseAbsorb;
			return baseAbsorb;
		}
		AbilityModPropertyInt empoweredOutgoingAbsorbMod;
		if (GameplayMutators.Get() == null || !GameplayMutators.Get().m_useEmpoweredOverride)
		{
			empoweredOutgoingAbsorbMod = GameWideData.Get().m_empoweredOutgoingAbsorbMod;
		}
		else
		{
			empoweredOutgoingAbsorbMod = GameplayMutators.Get().m_empoweredOutgoingAbsorbMod;
		}
		AbilityModPropertyInt weakenedOutgoingAbsorbMod;
		if (GameplayMutators.Get() == null || !GameplayMutators.Get().m_useWeakenedOverride)
		{
			weakenedOutgoingAbsorbMod = GameWideData.Get().m_weakenedOutgoingAbsorbMod;
		}
		else
		{
			weakenedOutgoingAbsorbMod = GameplayMutators.Get().m_weakenedOutgoingAbsorbMod;
		}
		float num = 0f;
		float num2 = 0f;
		float num3 = 1f;
		float num4 = 1f;
		CalculateAdjustments(StatType.OutgoingAbsorb, ref num, ref num2, ref num3, ref num4);
		float num5 = baseAbsorb;
		num5 += num;
		num5 *= num3;
		num5 *= num4;
		num5 += num2;
		modifiedAbsorbNormal = Mathf.RoundToInt(num5);
		float num6 = num;
		float num7 = num4;
		if (empoweredOutgoingAbsorbMod.operation == AbilityModPropertyInt.ModOp.Add)
		{
			num6 += empoweredOutgoingAbsorbMod.value;
		}
		else if (empoweredOutgoingAbsorbMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndCeil || empoweredOutgoingAbsorbMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndFloor || empoweredOutgoingAbsorbMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndRound)
		{
			num7 *= empoweredOutgoingAbsorbMod.value;
		}
		float num8 = baseAbsorb;
		num8 += num6;
		num8 *= num3;
		num8 *= num7;
		num8 += num2;
		modifiedAbsorbEmpowered = Mathf.RoundToInt(num8);
		float num9 = num;
		float num10 = num4;
		if (weakenedOutgoingAbsorbMod.operation == AbilityModPropertyInt.ModOp.Add)
		{
			num9 += weakenedOutgoingAbsorbMod.value;
		}
		else if (weakenedOutgoingAbsorbMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndCeil || weakenedOutgoingAbsorbMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndFloor || weakenedOutgoingAbsorbMod.operation == AbilityModPropertyInt.ModOp.MultiplyAndRound)
		{
			num10 *= weakenedOutgoingAbsorbMod.value;
		}
		float num11 = baseAbsorb;
		num11 += num9;
		num11 *= num3;
		num11 *= num10;
		num11 += num2;
		modifiedAbsorbWeakened = Mathf.RoundToInt(num11);
		ActorStatus component = GetComponent<ActorStatus>();
		bool flag = component.HasStatus(StatusType.Empowered, true);
		bool flag2 = component.HasStatus(StatusType.Weakened, true);
		if (flag && !flag2)
		{
			return modifiedAbsorbEmpowered;
		}
		if (!flag && flag2)
		{
			return modifiedAbsorbWeakened;
		}
		return modifiedAbsorbNormal;
	}
#endif

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
				damage = MathUtil.RoundToIntPadded(baseDamage * vulnerableDamageMultiplier);  // Mathf.RoundToInt in rogues
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

	// added in rogues
#if SERVER
	public int CalculateIncomingDamage(
		int baseDamage,
		out int baseDamageAfterStatus,
		out int modifiedDamageNormal,
		out int modifiedDamageVulnerable,
		out int modifiedDamageArmored)
	{
		baseDamageAfterStatus = baseDamage;
		float vulnerableDamageMultiplier = GameWideData.Get().m_vulnerableDamageMultiplier;
		int vulnerableDamageFlatAdd = GameWideData.Get().m_vulnerableDamageFlatAdd;
		
		int baseDamageVulnerable = baseDamage;
		if (vulnerableDamageMultiplier > 0f)
		{
			baseDamageVulnerable = Mathf.RoundToInt(baseDamage * vulnerableDamageMultiplier);
		}
		if (vulnerableDamageFlatAdd > 0 && baseDamage > 0)
		{
			baseDamageVulnerable += vulnerableDamageFlatAdd;
		}
		
		AbilityModPropertyInt armoredIncomingDamageMod =
			GameplayMutators.Get() == null || !GameplayMutators.Get().m_useArmoredOverride 
				? GameWideData.Get().m_armoredIncomingDamageMod 
				: GameplayMutators.Get().m_armoredIncomingDamageMod;
		int baseDamageArmored = armoredIncomingDamageMod.GetModifiedValue(baseDamage);
		
		if (!NetworkServer.active)
		{
			if (NetworkClient.active)
			{
				Debug.LogError("Client calling CalculateIncomingDamage.  Only the server should call this.");
			}
			modifiedDamageNormal = baseDamage;
			modifiedDamageVulnerable = baseDamage;
			modifiedDamageArmored = baseDamage;
			return baseDamage;
		}
		
		ActorStatus actorStatus = GetComponent<ActorStatus>();
		bool isVulnerable = actorStatus.HasStatus(StatusType.Vulnerable, true);
		bool isArmored = actorStatus.HasStatus(StatusType.Armored, true);
		if (isVulnerable && !isArmored)
		{
			baseDamageAfterStatus = Mathf.Max(0, baseDamageVulnerable);
		}
		else if (!isVulnerable && isArmored)
		{
			baseDamageAfterStatus = Mathf.Max(0, baseDamageArmored);
		}
		else
		{
			baseDamageAfterStatus = Mathf.Max(0, baseDamage);
		}
		
		float baseAdd = 0f;
		float bonusAdd = 0f;
		float percentAdd = 1f;
		float multipliers = 1f;
		CalculateAdjustments(StatType.IncomingDamage, ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers);
		
		float _modifiedDamageNormal = baseDamage;
		_modifiedDamageNormal += baseAdd;
		_modifiedDamageNormal *= percentAdd;
		_modifiedDamageNormal *= multipliers;
		_modifiedDamageNormal += bonusAdd;
		modifiedDamageNormal = Mathf.RoundToInt(_modifiedDamageNormal);
		
		float _modifiedDamageVulnerable = baseDamageVulnerable;
		_modifiedDamageVulnerable += baseAdd;
		_modifiedDamageVulnerable *= percentAdd;
		_modifiedDamageVulnerable *= multipliers;
		_modifiedDamageVulnerable += bonusAdd;
		modifiedDamageVulnerable = Mathf.RoundToInt(_modifiedDamageVulnerable);
		
		float _modifiedDamageArmored = baseDamageArmored;
		_modifiedDamageArmored += baseAdd;
		_modifiedDamageArmored *= percentAdd;
		_modifiedDamageArmored *= multipliers;
		_modifiedDamageArmored += bonusAdd;
		modifiedDamageArmored = Mathf.RoundToInt(_modifiedDamageArmored);
		
		if (isVulnerable && !isArmored)
		{
			return Mathf.RoundToInt(modifiedDamageVulnerable);
		}
		if (!isVulnerable && isArmored)
		{
			return Mathf.RoundToInt(modifiedDamageArmored);
		}
		return Mathf.RoundToInt(modifiedDamageNormal);
	}
#endif

	public int CalculateLifeOnDamage(int finalDamage) // , ActorData caster, int abilityIndex in rogues
	{
		float modifiedStatFloat = GetModifiedStatFloat(StatType.LifestealPerHit);
		float num = GetModifiedStatFloat(StatType.LifestealPerDamage) * finalDamage;
		return MathUtil.RoundToIntPadded(modifiedStatFloat + num);  // Mathf.RoundToInt in rogues
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

	// added in rogues
#if SERVER
	public int CalculateEnergyGainOnDamage(int damage, ServerGameplayUtils.EnergyStatAdjustments statAdjustments)
	{
		float modifiedStatFloat = GetModifiedStatFloat(StatType.EnergyOnDamageTaken);
		float modifiedStatFloat2 = GetModifiedStatFloat(StatType.EnergyPerDamageTaken);
		int num = Mathf.RoundToInt(modifiedStatFloat + modifiedStatFloat2 * damage);
		if (num > 0)
		{
			int num2 = num;
			ActorStatus actorStatus = GetComponent<ActorData>().GetActorStatus();
			bool flag = actorStatus.HasStatus(StatusType.Energized, true);
			bool flag2 = actorStatus.HasStatus(StatusType.SlowEnergyGain, true);
			AbilityModPropertyInt energizedEnergyGainMod;
			if (GameplayMutators.Get() == null || !GameplayMutators.Get().m_useEnergizedOverride)
			{
				energizedEnergyGainMod = GameWideData.Get().m_energizedEnergyGainMod;
			}
			else
			{
				energizedEnergyGainMod = GameplayMutators.Get().m_energizedEnergyGainMod;
			}
			AbilityModPropertyInt slowEnergyGainEnergyGainMod;
			if (GameplayMutators.Get() == null || !GameplayMutators.Get().m_useSlowEnergyGainOverride)
			{
				slowEnergyGainEnergyGainMod = GameWideData.Get().m_slowEnergyGainEnergyGainMod;
			}
			else
			{
				slowEnergyGainEnergyGainMod = GameplayMutators.Get().m_slowEnergyGainEnergyGainMod;
			}
			int modifiedValue = energizedEnergyGainMod.GetModifiedValue(num2);
			int modifiedValue2 = slowEnergyGainEnergyGainMod.GetModifiedValue(num2);
			if (flag && !flag2)
			{
				num = modifiedValue;
			}
			else if (!flag && flag2)
			{
				num = modifiedValue2;
			}
			if (statAdjustments != null)
			{
				statAdjustments.IncrementTotals(num, num2, modifiedValue, modifiedValue2);
			}
			return num;
		}
		return 0;
	}
#endif

	//public ActorStats()
	//{
	//	base.InitSyncObject(m_modifiedStats);
	//}

	private void UNetVersion()
	{
	}

	//private void MirrorProcessed()
	//{
	//}

	// removed in rogues
	protected static void InvokeSyncListm_modifiedStats(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_modifiedStats called on server.");
			return;
		}
		((ActorStats)obj).m_modifiedStats.HandleMsg(reader);
	}

	// removed in rogues
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

	// removed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListFloat.ReadReference(reader, m_modifiedStats);
			return;
		}
		int mask = (int)reader.ReadPackedUInt32();
		if ((mask & 1) != 0)
		{
			SyncListFloat.ReadReference(reader, m_modifiedStats);
		}
	}
}
