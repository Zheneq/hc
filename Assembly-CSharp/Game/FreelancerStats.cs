// ROGUES
// SERVER
using System;
using System.Collections.Generic;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class FreelancerStats : NetworkBehaviour
{
	public enum GeneralStats
	{
		STAT0,
		STAT1,
		STAT2,
		STAT3
	}

	public enum ArcherStats
	{
		ShieldArrowEffectiveShieldAndWeakenedMitigation,
		DashAndShootDamageDealtAndDodged,
		ArrowRainNumEnemiesRooted,
		HealArrowHealTotal
	}

	public enum BattleMonkStats
	{
		DamageReturnedByShield,
		AssistsWithRoot,
		DamageDealtPlusDodgedByCharge,
		ShieldsGrantedByUlt
	}

	public enum BazookaGirlStats
	{
		BigOneHits,
		DashesOutOfBigOne,
		UltKills,
		StickyBombCombos
	}

	public enum BlasterStats
	{
		DamageAddedFromOvercharge,
		LurkerDroneDamage,
		DamageDodgedByRoll,
		NumAssistsUsingUlt
	}

	public enum ClaymoreStats
	{
		DamageMitigatedByShout,
		DirectHitsWithCharge,
		AssistsWithDagger,
		UltDamage
	}

	public enum ClericStats
	{
		ReforgeHealFromMissingHealth,
		BoneShatterMovementDenied,
		SolarStrikeNumRevealApplied,
		UltEffectiveShielding
	}

	public enum DigitalSorceressStats
	{
		KnockbacksFromHeal,
		TimesPrimaryHitTwoOrMoreTargets,  // TODO AURORA freelancer stats
		MitigationFromDebuffLaser,
		UltDamagePlusHealing
	}

	// removed in rogues
	public enum DinoStats
	{
		MarkedAreaAttackDamage, // How much damage you dealt to enemies with [ABILITY_1]
		DashOrShieldEffectiveShieldAndDamageEvaded, // How much damage you absorb, dodge, and dealt with [ABILITY_2]
		KnockbackDamageOnCastAndKnockback, // Damage you dealt with [ABILITY_3], plus damage from any traps you knocked the target through
		ForceChaseNumChases // How many targets are forced to chase you with [ABILITY_4]
	}

	public enum ExoStats
	{
		TetherTrapTriggers,
		EffectiveShieldingFromSelfShield,
		UltDamage,
		MaxConsecutiveUltSweeps
	}

	// removed in rogues
	public enum FireborgStats
	{
		IgniteDamage, // How much [^ignite^] damage you dealt to enemies.
		GroundFireDamage, // How much [^ground fire^] damage you dealt to enemies.
		FireAuraDamage, // How much damage you dealt to enemies using [ABILITY_1].
		BlastwaveDamage // How much damage you dealt to enemies using [ABILITY_3].
	}

	public enum FishManStats
	{
		HealingFromHealCone,
		EelDamage,
		EnemiesHitByBubbleAoe,
		EnemiesKnockbackedByUlt
	}

	public enum GremlinsStats
	{
		MinesTriggeredByMovers,
		MinesTriggeredByKnockbacksFromMe,
		TurnsDirectlyHittingTwoEnemiesWithPrimary,
		DamageDoneByUlt
	}

	// removed in rogues
	public enum IceborgStats
	{
		NumCoresTriggered, // How many [^Cryo Cores^] were detonated.
		NumSlowsPlusRootsApplied, // How many enemies were [^slowed^] or [^rooted^] by your abilities.
		UltDamage, // How much damage you dealt to enemies with [ABILITY_4].
		SelfShieldEffectiveShielding // How much damage you absorbed with [ABILITY_3].
	}

	public enum MantaStats
	{
		NumHitsThroughWalls,
		NumDamagingPutridSprayHits,
		HealingFromSelfHeal,
		NumEnemiesHitByUltCast
	}

	public enum MartyrStats
	{
		DamageRedirected,
		EnemiesDamagedByAoeOnHitEffect,
		TurnsWithMaxEnergy,
		UltDamagePlusHealing
	}

	public enum NanoSmithStats
	{
		EnergyFromChainLightning,
		VacuumBombHits,
		BarrierHits,
		PercentageOfTimesShieldedAllyWasDamaged
	}

	// removed in rogues
	public enum NekoStats
	{
		NormalDiscNumDistinctEnemiesHitByReturn,
		EnlargeDiscExtraDamage,
		SeekerDiscDamage,
		FlipDashDamageDoneAndDodged,
		HomingDiscNumDistinctEnemiesHitByReturn // TODO NEKO unused
	}

	public enum RageBeastStats
	{
		EnergyGainedFromDamageTaken,
		HealingFromSelfHeal,
		KnockbackAssists,
		ChargeDamageDonePlusDodged
	}

	public enum RampartStats
	{
		HitsBlockedByShield,
		GrabAssists,
		MovementDebuffsAndKnockbacksPreventedByUnstoppable,
		UltDamageDealtPlusDodged
	}

	public enum RobotAnimalStats
	{
		HealingFromPrimary,
		UltHits,
		DragAssists,
		DamageDonePlusDodgedByPounce
	}

	public enum SamuraiStats
	{
		EffectiveShielding_RensFury,
		NumEnemiesHit_WindBlade,
		DamageDealtAndDodged_RushingSteel,
		DamageDealtAndDodged_Ult
	}

	// removed in rogues
	public enum ScampStats
	{
		DashDamageDoneAndAvoided, // How much damage you dodged and dealt with [ABILITY_1]
		TetherNumKnockbacks, // Number of enemies knocked back with [ABILITY_2]
		DelayedAoeDamageDone, // How much damage you dealt to enemies with [ABILITY_3].
		UltShieldGenerated // How many shields you generated with [ABILITY_4].
	}

	public enum ScoundrelStats
	{
		TargetsHitByConeAoe,
		DashersHitByTrapwire,
		DamageDodgedByEvasionRoll,
		DamageDodgedPlusDealtByUlt
	}

	public enum SenseiStats
	{
		NumAlliesHitByHeal,
		NumBuffsPlusDebuffsFromAppendStatus,
		DamageDodgedPlusDamageDealtPlusHealingDealtByDash,
		DamageDoneByUlt
	}

	public enum SniperStats
	{
		DecisionPhasesNotVisibleToEnemies,
		DecisionPhasesWithNoNearbyEnemies,
		EnergyGainedByVortexRound,
		DamageDoneByUlt
	}

	public enum SoldierStats
	{
		DamageDealtPlusDodgedByDash,
		DamageFromGrenadesThrownOverWalls,
		DamageAddedByStimMight,
		TargetsHitByUlt
	}

	public enum SpaceMarineStats
	{
		MissilesHit,
		NumSlowsPlusRootsApplied,
		HealingDealtByUlt,
		DamageDealtByUlt
	}

	public enum SparkStats
	{
		TurnsTetheredToAllies,
		TurnsTetheredToEnemies,
		DamageDodgedPlusDamageDealtPlusHealingDealtByDash,
		HealingFromUlt
	}

	public enum TeleportingNinjaStats
	{
		DamageDodgedWithDash,
		NumDetonationsOfMark,
		TurnsNotVisibleToEnemies,
		DamageDonePlusDodgedWithUlt
	}

	public enum ThiefStats
	{
		PowerUpsStolen,
		ProximityMineHits,
		TimesSmokeBombHitsHidAllies,
		UltDamage
	}

	public enum TrackerStats
	{
		DamageDoneByDrone,
		NumTrackingProjectileHits,
		DamageMitigatedByTranqDart,
		DamageDodgedWithEvade
	}

	public enum TricksterStats
	{
		DamageDodgedWithSwap,
		TargetsHitByThreeImages,
		TargetsHitByPhotonSpray,
		TargetsHitByZapTrap
	}

	public enum ValkyrieStats
	{
		DamageMitigatedByCoverOnTurnsWithGuard,
		DamageDoneByThrowShieldAndKnockback,
		DamageMitigatedFromWeakenedAndDodgedDashAoe,
		NumKnockbackTargetsWithUlt
	}

	[Header("-- Whether to skip for localization for stat descriptions --")]
	public bool m_ignoreForLocalization;
	public List<string> m_name;
	public List<string> m_descriptions;
	private SyncListInt m_values = new SyncListInt();
	private string m_freelancerTypeStr = "[unset]";

	// removed in rogues
	private static int kListm_values = 86738482;

	// removed in rogues
	static FreelancerStats()
	{
		RegisterSyncListDelegate(typeof(FreelancerStats), kListm_values, InvokeSyncListm_values);
		NetworkCRC.RegisterBehaviour("FreelancerStats", 0);
	}

	private void Start()
	{
		ActorData actorData = GetComponent<ActorData>();
		if (actorData != null)
		{
			m_freelancerTypeStr = actorData.m_characterType.ToString();
		}

		// moved from Start in rogues
		//if (NetworkServer.active)
		//{
		//	while (m_values.Count < 4)
		//	{
		//		m_values.Add(0);
		//	}
		//}
	}

	// moved into Start in rogues
	public override void OnStartServer()
	{
		while (m_values.Count < 4)
		{
			m_values.Add(0);
		}
	}

	public virtual string GetDisplayNameOfStat(int statIndex)
	{
		if (statIndex < 0 || statIndex >= m_name.Count)
		{
			Log.Warning($"Calling GetDisplayNameOfStat of freelancer {m_freelancerTypeStr} for stat index {statIndex}, but the index is out-of-bounds.");
			return "";
		}
		return StringUtil.TR_FreelancerStatName(m_freelancerTypeStr, statIndex);
	}

	public virtual string GetDisplayNameOfStat(Enum statEnum)
	{
		return GetDisplayNameOfStat(Convert.ToInt32(statEnum));
	}

	public virtual string GetLocalizedDescriptionOfStat(int statIndex)
	{
		if (statIndex < 0 || statIndex >= m_descriptions.Count)
		{
			Log.Warning($"Calling GetLocalizedDescriptionOfStat of freelancer {m_freelancerTypeStr} for stat index {statIndex}, but the index is out-of-bounds.");
			return "";
		}
		return StringUtil.TR_FreelancerStatDescription(m_freelancerTypeStr, statIndex);
	}

	public virtual string GetLocalizedDescriptionOfStat(Enum statEnum)
	{
		return GetLocalizedDescriptionOfStat(Convert.ToInt32(statEnum));
	}

	public virtual int GetValueOfStat(int statIndex)
	{
		if (statIndex < 0 || statIndex >= m_values.Count)
		{
			Debug.LogError($"Calling GetValueOfStat for stat index {statIndex}, but the index is out-of-bounds.");
			return 0;
		}
		return m_values[statIndex];
	}

	public virtual int GetValueOfStat(Enum statEnum)
	{
		return GetValueOfStat(Convert.ToInt32(statEnum));
	}

#if SERVER
	// added in rogues
	public virtual void IncrementValueOfStat(int statIndex)
	{
		if (statIndex < 0 || statIndex >= m_values.Count)
		{
			Debug.LogError(string.Format("Calling IncrementValueOfStat for stat index {0}, but the index is out-of-bounds.", statIndex));
			return;
		}
		SyncListInt values = m_values;
		int num = values[statIndex] + 1;
		values[statIndex] = num;
	}
#endif

	// added in rogues
#if SERVER
	public virtual void IncrementValueOfStat(Enum statEnum)
    {
        IncrementValueOfStat(Convert.ToInt32(statEnum));
    }
#endif

	// added in rogues
#if SERVER
	public virtual void AddToValueOfStat(int statIndex, int addAmount)
    {
        if (statIndex >= 0 && statIndex < m_values.Count)
        {
            if (addAmount != 0)
            {
                SyncListInt values = m_values;
                values[statIndex] += addAmount;
                return;
            }
        }
        else
        {
            Debug.LogError(string.Format("Calling AddToValueOfStat for stat index {0}, but the index is out-of-bounds.", statIndex));
        }
    }
#endif

	// added in rogues
#if SERVER
	public virtual void AddToValueOfStat(Enum statEnum, int addAmount)
    {
        AddToValueOfStat(Convert.ToInt32(statEnum), addAmount);
    }
#endif

	// added in rogues
#if SERVER
	public virtual void SetValueOfStat(int statIndex, int newVal)
    {
        if (statIndex >= 0 && statIndex < m_values.Count)
        {
            if (m_values[statIndex] != newVal)
            {
                m_values[statIndex] = newVal;
                return;
            }
        }
        else
        {
            Debug.LogError(string.Format("Calling SetValueOfStat for stat index {0}, but the index is out-of-bounds.", statIndex));
        }
    }
#endif

	// added in rogues
#if SERVER
	public virtual void SetValueOfStat(Enum statEnum, int newVal)
    {
        SetValueOfStat(Convert.ToInt32(statEnum), newVal);
    }
#endif

    public virtual int GetNumStats()
	{
		if (m_values == null)
		{
			return 0;
		}
		return m_values.Count;
	}

	// rogues
	//public FreelancerStats()
	//{
	//	base.InitSyncObject(m_values);
	//}

	// reactor
	private void UNetVersion()
	{
	}
	// rogues
	//private void MirrorProcessed()
	//{
	//}

	// removed in rogues
	protected static void InvokeSyncListm_values(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_values called on server.");
			return;
		}
		((FreelancerStats)obj).m_values.HandleMsg(reader);
	}


	// removed in rogues
	private void Awake()
	{
		m_values.InitializeBehaviour(this, kListm_values);
	}


	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListInt.WriteInstance(writer, m_values);
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
			SyncListInt.WriteInstance(writer, m_values);
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
			SyncListInt.ReadReference(reader, m_values);
			return;
		}
		int dirtyBits = (int)reader.ReadPackedUInt32();
		if ((dirtyBits & 1) != 0)
		{
			SyncListInt.ReadReference(reader, m_values);
		}
	}
}
