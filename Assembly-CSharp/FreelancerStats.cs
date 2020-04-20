using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FreelancerStats : NetworkBehaviour
{
	[Header("-- Whether to skip for localization for stat descriptions --")]
	public bool m_ignoreForLocalization;

	public List<string> m_name;

	public List<string> m_descriptions;

	private SyncListInt m_values = new SyncListInt();

	private string m_freelancerTypeStr = "[unset]";

	private static int kListm_values = 0x52B8632;

	static FreelancerStats()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(FreelancerStats), FreelancerStats.kListm_values, new NetworkBehaviour.CmdDelegate(FreelancerStats.InvokeSyncListm_values));
		NetworkCRC.RegisterBehaviour("FreelancerStats", 0);
	}

	private void Start()
	{
		ActorData component = base.GetComponent<ActorData>();
		if (component != null)
		{
			this.m_freelancerTypeStr = component.m_characterType.ToString();
		}
	}

	public override void OnStartServer()
	{
		while (this.m_values.Count < 4)
		{
			this.m_values.Add(0);
		}
	}

	public virtual string GetDisplayNameOfStat(int statIndex)
	{
		if (statIndex >= 0)
		{
			if (statIndex < this.m_name.Count)
			{
				return StringUtil.TR_FreelancerStatName(this.m_freelancerTypeStr, statIndex);
			}
		}
		Log.Warning(string.Format("Calling GetDisplayNameOfStat of freelancer {0} for stat index {1}, but the index is out-of-bounds.", this.m_freelancerTypeStr, statIndex), new object[0]);
		return string.Empty;
	}

	public virtual string GetDisplayNameOfStat(Enum statEnum)
	{
		return this.GetDisplayNameOfStat(Convert.ToInt32(statEnum));
	}

	public virtual string GetLocalizedDescriptionOfStat(int statIndex)
	{
		if (statIndex >= 0)
		{
			if (statIndex < this.m_descriptions.Count)
			{
				return StringUtil.TR_FreelancerStatDescription(this.m_freelancerTypeStr, statIndex);
			}
		}
		Log.Warning(string.Format("Calling GetLocalizedDescriptionOfStat of freelancer {0} for stat index {1}, but the index is out-of-bounds.", this.m_freelancerTypeStr, statIndex), new object[0]);
		return string.Empty;
	}

	public virtual string GetLocalizedDescriptionOfStat(Enum statEnum)
	{
		return this.GetLocalizedDescriptionOfStat(Convert.ToInt32(statEnum));
	}

	public virtual int GetValueOfStat(int statIndex)
	{
		if (statIndex >= 0)
		{
			if (statIndex < this.m_values.Count)
			{
				return this.m_values[statIndex];
			}
		}
		Debug.LogError(string.Format("Calling GetValueOfStat for stat index {0}, but the index is out-of-bounds.", statIndex));
		return 0;
	}

	public virtual int GetValueOfStat(Enum statEnum)
	{
		return this.GetValueOfStat(Convert.ToInt32(statEnum));
	}

	public virtual int GetNumStats()
	{
		if (this.m_values == null)
		{
			return 0;
		}
		return this.m_values.Count;
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
	}

	private void Awake()
	{
		this.m_values.InitializeBehaviour(this, FreelancerStats.kListm_values);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListInt.WriteInstance(writer, this.m_values);
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
			SyncListInt.WriteInstance(writer, this.m_values);
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
			SyncListInt.ReadReference(reader, this.m_values);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListInt.ReadReference(reader, this.m_values);
		}
	}

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
}
