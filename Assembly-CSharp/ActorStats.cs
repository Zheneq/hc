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

	private static int kListm_modifiedStats;

	static ActorStats()
	{
		kListm_modifiedStats = -1034899976;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorStats), kListm_modifiedStats, InvokeSyncListm_modifiedStats);
		NetworkCRC.RegisterBehaviour("ActorStats", 0);
	}

	private void MarkAllForUpdate()
	{
		m_shouldUpdateFull = true;
	}

	private void Awake()
	{
		
		Func<StatType, StatType, bool> equals = ((StatType a, StatType b) => a == b);
		
		FuncEqualityComparer<StatType> comparer = new FuncEqualityComparer<StatType>(equals, ((StatType a) => (int)a));
		m_statMods = new Dictionary<StatType, List<StatMod>>(comparer);
		for (int i = 0; i < 24; i++)
		{
			List<StatMod> value = new List<StatMod>();
			m_statMods.Add((StatType)i, value);
		}
		m_modifiedStatsPrevious = new float[24];
		m_actorData = GetComponent<ActorData>();
		m_modifiedStats.InitializeBehaviour(this, kListm_modifiedStats);
	}

	private void Start()
	{
		for (int i = 0; i < 24; i++)
		{
			m_modifiedStatsPrevious[i] = GetStatBaseValueFloat((StatType)i);
		}
		while (true)
		{
			if (!NetworkServer.active)
			{
				return;
			}
			while (true)
			{
				for (int j = 0; j < 24; j++)
				{
					m_modifiedStats.Add(GetStatBaseValueFloat((StatType)j));
				}
				while (true)
				{
					MarkAllForUpdate();
					return;
				}
			}
		}
	}

	public override void OnStartClient()
	{
		m_modifiedStats.Callback = SyncListCallbackModifiedStats;
	}

	private void Update()
	{
		if (!NetworkServer.active || !m_shouldUpdateFull)
		{
			return;
		}
		while (true)
		{
			SendFullUpdateData();
			m_shouldUpdateFull = false;
			return;
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
		for (int i = 0; i < 24; i++)
		{
			StatType stat = (StatType)i;
			float modifiedStatFloat = GetModifiedStatFloat(stat);
			if (modifiedStatFloat == m_modifiedStatsPrevious[i])
			{
				continue;
			}
			if (m_modifiedStats[i] != modifiedStatFloat)
			{
				m_modifiedStats[i] = modifiedStatFloat;
				m_modifiedStatsPrevious[i] = modifiedStatFloat;
			}
		}
	}

	private void SyncListCallbackModifiedStats(SyncList<float>.Operation op, int _incorrectIndexBugIn51And52)
	{
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			if (NetworkServer.active)
			{
				return;
			}
			int num = 0;
			while (num < m_modifiedStats.Count)
			{
				while (true)
				{
					if (num >= m_modifiedStatsPrevious.Length)
					{
						return;
					}
					if (m_modifiedStats[num] != m_modifiedStatsPrevious[num])
					{
						StatType stat = (StatType)num;
						float oldStatValue = m_modifiedStatsPrevious[num];
						m_modifiedStatsPrevious[num] = m_modifiedStats[num];
						OnStatModified(stat, oldStatValue, false);
					}
					num++;
					goto IL_007f;
				}
				IL_007f:;
			}
			return;
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					float modifiedStatFloat = GetModifiedStatFloat(stat);
					StatMod statMod = new StatMod();
					statMod.Setup(mod, val);
					m_statMods[stat].Add(statMod);
					OnStatModified(stat, modifiedStatFloat, true);
					return;
				}
				}
			}
		}
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			Log.Error("called AddStatMod when server is not active");
			return;
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					List<StatMod> list = m_statMods[stat];
					using (List<StatMod>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							StatMod current = enumerator.Current;
							if (current.mod == mod)
							{
								if (current.val == val)
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											break;
										default:
										{
											float modifiedStatFloat = GetModifiedStatFloat(stat);
											list.Remove(current);
											OnStatModified(stat, modifiedStatFloat, false);
											return;
										}
										}
									}
								}
							}
						}
						while (true)
						{
							switch (6)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
				}
				}
			}
		}
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			Log.Error("called RemoveStat when server is not active");
			return;
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
		float baseValue2 = baseValue;
		float f = CalculateModifiedStatValue(stat, baseValue2);
		return Mathf.FloorToInt(f);
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
		if (m_statMods == null)
		{
			return;
		}
		while (true)
		{
			if (!m_statMods.ContainsKey(stat))
			{
				return;
			}
			while (true)
			{
				List<StatMod> list = m_statMods[stat];
				for (int i = 0; i < list.Count; i++)
				{
					CalculateAdjustmentForStatMod(list[i], ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers);
				}
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	private void CalculateAdjustments(StatType stat, ref float baseAdd, ref float bonusAdd, ref float percentAdd, ref float multipliers, StatModFilterDelegate filterDelegate)
	{
		if (m_statMods == null)
		{
			return;
		}
		while (true)
		{
			if (!m_statMods.ContainsKey(stat))
			{
				return;
			}
			List<StatMod> list = m_statMods[stat];
			for (int i = 0; i < list.Count; i++)
			{
				if (filterDelegate(list[i]))
				{
					CalculateAdjustmentForStatMod(list[i], ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers);
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
	}

	private float CalculateModifiedStatValue(StatType stat, float baseValue)
	{
		if (m_statMods != null)
		{
			if (m_statMods.ContainsKey(stat))
			{
				if (NetworkServer.active)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
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
						}
					}
				}
				float result;
				if ((int)stat < m_modifiedStats.Count)
				{
					result = m_modifiedStats[(int)stat];
				}
				else
				{
					result = m_modifiedStatsPrevious[(int)stat];
				}
				return result;
			}
		}
		return baseValue;
	}

	public int GetStatBaseValueInt(StatType stat)
	{
		float statBaseValueFloat = GetStatBaseValueFloat(stat);
		return Mathf.RoundToInt(statBaseValueFloat);
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
		int b = baseDamage;
		ActorStatus actorStatus = m_actorData.GetActorStatus();
		int num;
		if (!actorStatus.HasStatus(StatusType.Empowered))
		{
			if (m_actorData.GetAbilityData() != null)
			{
				num = (m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Empowered) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		bool flag2 = actorStatus.HasStatus(StatusType.Weakened);
		AbilityModPropertyInt empoweredOutgoingDamageMod;
		if (flag)
		{
			if (!flag2)
			{
				if (!(GameplayMutators.Get() == null))
				{
					if (GameplayMutators.Get().m_useEmpoweredOverride)
					{
						empoweredOutgoingDamageMod = GameplayMutators.Get().m_empoweredOutgoingDamageMod;
						goto IL_00d7;
					}
				}
				empoweredOutgoingDamageMod = GameWideData.Get().m_empoweredOutgoingDamageMod;
				goto IL_00d7;
			}
		}
		if (!flag)
		{
			if (flag2)
			{
				AbilityModPropertyInt abilityModPropertyInt = (!(GameplayMutators.Get() == null) && GameplayMutators.Get().m_useWeakenedOverride) ? GameplayMutators.Get().m_weakenedOutgoingDamageMod : GameWideData.Get().m_weakenedOutgoingDamageMod;
				b = abilityModPropertyInt.GetModifiedValue(baseDamage);
			}
		}
		goto IL_0142;
		IL_0142:
		return Mathf.Max(0, b);
		IL_00d7:
		b = empoweredOutgoingDamageMod.GetModifiedValue(baseDamage);
		goto IL_0142;
	}

	public int CalculateOutgoingHealForTargeter(int baseHeal)
	{
		int b = baseHeal;
		ActorStatus actorStatus = m_actorData.GetActorStatus();
		int num;
		if (!actorStatus.HasStatus(StatusType.Empowered))
		{
			num = ((m_actorData.GetAbilityData() != null && m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Empowered)) ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		bool flag2 = actorStatus.HasStatus(StatusType.Weakened);
		AbilityModPropertyInt empoweredOutgoingHealingMod;
		if (flag)
		{
			if (!flag2)
			{
				if (!(GameplayMutators.Get() == null))
				{
					if (GameplayMutators.Get().m_useEmpoweredOverride)
					{
						empoweredOutgoingHealingMod = GameplayMutators.Get().m_empoweredOutgoingHealingMod;
						goto IL_00d8;
					}
				}
				empoweredOutgoingHealingMod = GameWideData.Get().m_empoweredOutgoingHealingMod;
				goto IL_00d8;
			}
		}
		if (!flag)
		{
			if (flag2)
			{
				AbilityModPropertyInt abilityModPropertyInt = (!(GameplayMutators.Get() == null) && GameplayMutators.Get().m_useWeakenedOverride) ? GameplayMutators.Get().m_weakenedOutgoingHealingMod : GameWideData.Get().m_weakenedOutgoingHealingMod;
				b = abilityModPropertyInt.GetModifiedValue(baseHeal);
			}
		}
		goto IL_0139;
		IL_00d8:
		b = empoweredOutgoingHealingMod.GetModifiedValue(baseHeal);
		goto IL_0139;
		IL_0139:
		return Mathf.Max(0, b);
	}

	public int CalculateOutgoingAbsorbForTargeter(int baseAbsorb)
	{
		int b = baseAbsorb;
		ActorStatus actorStatus = m_actorData.GetActorStatus();
		int num;
		if (!actorStatus.HasStatus(StatusType.Empowered))
		{
			if (m_actorData.GetAbilityData() != null)
			{
				num = (m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Empowered) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		bool flag2 = actorStatus.HasStatus(StatusType.Weakened);
		if (flag)
		{
			if (!flag2)
			{
				AbilityModPropertyInt abilityModPropertyInt = (!(GameplayMutators.Get() == null) && GameplayMutators.Get().m_useEmpoweredOverride) ? GameplayMutators.Get().m_empoweredOutgoingAbsorbMod : GameWideData.Get().m_empoweredOutgoingAbsorbMod;
				b = abilityModPropertyInt.GetModifiedValue(baseAbsorb);
				goto IL_0141;
			}
		}
		AbilityModPropertyInt weakenedOutgoingAbsorbMod;
		if (!flag)
		{
			if (flag2)
			{
				if (!(GameplayMutators.Get() == null))
				{
					if (GameplayMutators.Get().m_useWeakenedOverride)
					{
						weakenedOutgoingAbsorbMod = GameplayMutators.Get().m_weakenedOutgoingAbsorbMod;
						goto IL_0136;
					}
				}
				weakenedOutgoingAbsorbMod = GameWideData.Get().m_weakenedOutgoingAbsorbMod;
				goto IL_0136;
			}
		}
		goto IL_0141;
		IL_0141:
		return Mathf.Max(0, b);
		IL_0136:
		b = weakenedOutgoingAbsorbMod.GetModifiedValue(baseAbsorb);
		goto IL_0141;
	}

	public int CalculateIncomingDamageForTargeter(int baseDamage)
	{
		int num = baseDamage;
		ActorStatus component = GetComponent<ActorStatus>();
		bool flag = component.HasStatus(StatusType.Vulnerable);
		bool flag2 = component.HasStatus(StatusType.Armored);
		float vulnerableDamageMultiplier;
		int vulnerableDamageFlatAdd;
		if (flag)
		{
			if (!flag2)
			{
				if (!(GameplayMutators.Get() == null))
				{
					if (GameplayMutators.Get().m_useVulnerableOverride)
					{
						vulnerableDamageMultiplier = GameplayMutators.Get().m_vulnerableDamageMultiplier;
						vulnerableDamageFlatAdd = GameplayMutators.Get().m_vulnerableDamageFlatAdd;
						goto IL_00b1;
					}
				}
				vulnerableDamageMultiplier = GameWideData.Get().m_vulnerableDamageMultiplier;
				vulnerableDamageFlatAdd = GameWideData.Get().m_vulnerableDamageFlatAdd;
				goto IL_00b1;
			}
		}
		AbilityModPropertyInt armoredIncomingDamageMod;
		if (!flag)
		{
			if (flag2)
			{
				if (!(GameplayMutators.Get() == null))
				{
					if (GameplayMutators.Get().m_useArmoredOverride)
					{
						armoredIncomingDamageMod = GameplayMutators.Get().m_armoredIncomingDamageMod;
						goto IL_0140;
					}
				}
				armoredIncomingDamageMod = GameWideData.Get().m_armoredIncomingDamageMod;
				goto IL_0140;
			}
		}
		goto IL_014b;
		IL_014b:
		return Mathf.Max(0, num);
		IL_00b1:
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
		goto IL_014b;
		IL_0140:
		num = armoredIncomingDamageMod.GetModifiedValue(baseDamage);
		goto IL_014b;
	}

	public int CalculateLifeOnDamage(int finalDamage)
	{
		float modifiedStatFloat = GetModifiedStatFloat(StatType.LifestealPerHit);
		float num = GetModifiedStatFloat(StatType.LifestealPerDamage) * (float)finalDamage;
		return MathUtil.RoundToIntPadded(modifiedStatFloat + num);
	}

	public void CalculateAdjustmentsForMovementHorizontal(ref float baseAdd, ref float bonusAdd, ref float percentAdd, ref float multipliers)
	{
		ActorStatus component = GetComponent<ActorStatus>();
		if (component.HasStatus(StatusType.MovementDebuffSuppression))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					
					StatModFilterDelegate filterDelegate = delegate(StatMod statMod)
						{
							bool flag = false;
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
					CalculateAdjustments(StatType.Movement_Horizontal, ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers, filterDelegate);
					return;
				}
				}
			}
		}
		CalculateAdjustments(StatType.Movement_Horizontal, ref baseAdd, ref bonusAdd, ref percentAdd, ref multipliers);
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_modifiedStats(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Debug.LogError("SyncList m_modifiedStats called on server.");
					return;
				}
			}
		}
		((ActorStats)obj).m_modifiedStats.HandleMsg(reader);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					SyncListFloat.WriteInstance(writer, m_modifiedStats);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListFloat.WriteInstance(writer, m_modifiedStats);
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
