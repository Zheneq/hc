using System;
using System.Collections.Generic;
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
		TimesPrimaryHitTwoOrMoreTargets,
		MitigationFromDebuffLaser,
		UltDamagePlusHealing
	}

	public enum DinoStats
	{
		MarkedAreaAttackDamage,
		DashOrShieldEffectiveShieldAndDamageEvaded,
		KnockbackDamageOnCastAndKnockback,
		ForceChaseNumChases
	}

	public enum ExoStats
	{
		TetherTrapTriggers,
		EffectiveShieldingFromSelfShield,
		UltDamage,
		MaxConsecutiveUltSweeps
	}

	public enum FireborgStats
	{
		IgniteDamage,
		GroundFireDamage,
		FireAuraDamage,
		BlastwaveDamage
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

	public enum IceborgStats
	{
		NumCoresTriggered,
		NumSlowsPlusRootsApplied,
		UltDamage,
		SelfShieldEffectiveShielding
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

	public enum NekoStats
	{
		NormalDiscNumDistinctEnemiesHitByReturn,
		EnlargeDiscExtraDamage,
		SeekerDiscDamage,
		FlipDashDamageDoneAndDodged,
		HomingDiscNumDistinctEnemiesHitByReturn
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

	public enum ScampStats
	{
		DashDamageDoneAndAvoided,
		TetherNumKnockbacks,
		DelayedAoeDamageDone,
		UltShieldGenerated
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

	private static int kListm_values = 86738482;

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
	}

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

	public virtual int GetNumStats()
	{
		if (m_values == null)
		{
			return 0;
		}
		return m_values.Count;
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_values(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_values called on server.");
			return;
		}
		((FreelancerStats)obj).m_values.HandleMsg(reader);
		Log.Info($"[JSON] {{\"values\":{DefaultJsonSerializer.Serialize(((FreelancerStats)obj).m_values)}}}");
	}

	private void Awake()
	{
		m_values.InitializeBehaviour(this, kListm_values);
	}

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

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListInt.ReadReference(reader, m_values);
			LogJson();
			return;
		}
		int dirtyBits = (int)reader.ReadPackedUInt32();
		if ((dirtyBits & 1) != 0)
		{
			SyncListInt.ReadReference(reader, m_values);
		}
		LogJson(dirtyBits);
	}

	private void LogJson(int mask = Int32.MaxValue)
	{
		var jsonLog = new List<string>();
		if ((mask & 1) != 0)
		{
			jsonLog.Add($"\"values\":{DefaultJsonSerializer.Serialize(m_values)}");
		}

		Log.Info($"[JSON] {{\"freelancerStats\":{{{String.Join(",", jsonLog.ToArray())}}}}}");
	}
}
