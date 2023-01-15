// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RageBeastKnockback : Ability
{
	public float m_laserWidth;
	public float m_laserDistance;
	public bool m_penetrateLineOfSight;
	public int m_maxTargets;
	public float m_knockbackDistanceMin;
	public float m_knockbackDistanceMax;
	public KnockbackType m_knockbackType;
	public int m_damageAmount;
	public StandardEffectInfo m_onHitEffect;
	public int m_damageToMoverOnCollision = 2;
	public int m_damageToOtherOnCollision;
	public int m_damageCollisionWithGeo = 2;
	public GameObject m_hitActorSequencePrefab;
	public GameObject m_hitGeoSequencePrefab;

	private AbilityMod_RageBeastKnockback m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Upheaval";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_KnockbackLaser(
			this,
			ModdedLaserWidth(),
			ModdedLaserLength(),
			m_penetrateLineOfSight,
			ModdedMaxTargets(),
			ModdedKnockbackDistanceMin(),
			ModdedKnockbackDistanceMax(),
			m_knockbackType,
			false);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return ModdedLaserLength();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damageAmount)
		};
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, ModdedOnHitDamage())
		};
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastKnockback abilityMod_RageBeastKnockback = modAsBase as AbilityMod_RageBeastKnockback;
		AddTokenInt(tokens, "MaxTargets", string.Empty, abilityMod_RageBeastKnockback != null
			? abilityMod_RageBeastKnockback.m_maxTargetMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets);
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_RageBeastKnockback != null
			? abilityMod_RageBeastKnockback.m_onHitDamageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RageBeastKnockback))
		{
			m_abilityMod = abilityMod as AbilityMod_RageBeastKnockback;
			SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public int ModdedMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public float ModdedLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targeterWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public float ModdedLaserLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targeterLengthMod.GetModifiedValue(m_laserDistance)
			: m_laserDistance;
	}

	public int ModdedOnHitDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_onHitDamageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public float ModdedKnockbackDistanceMin()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistanceMinMod.GetModifiedValue(m_knockbackDistanceMin)
			: m_knockbackDistanceMin;
	}

	public float ModdedKnockbackDistanceMax()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistanceMaxMod.GetModifiedValue(m_knockbackDistanceMax)
			: m_knockbackDistanceMax;
	}

	private float GetKnockbackDist(AbilityTarget target, Vector3 casterPos, Vector3 knockbackStartPos)
	{
		Vector3 vecToTarget = target.FreePos - casterPos;
		Vector3 vecToKnockback = knockbackStartPos - casterPos;
		vecToTarget.y = 0f;
		vecToKnockback.y = 0f;
		float knockbackDistInSquares = (vecToTarget.magnitude - vecToKnockback.magnitude) / Board.SquareSizeStatic;
		float knockbackDistMin = ModdedKnockbackDistanceMin();
		float knockbackDistMax = ModdedKnockbackDistanceMax();
		return knockbackDistInSquares < knockbackDistMin
			? knockbackDistMin
			: knockbackDistInSquares > knockbackDistMax
				? knockbackDistMax
				: knockbackDistInSquares;
	}
	
#if SERVER
	// added in rogues
	private List<ActorData> GetHitTargets(
		List<AbilityTarget> targets,
		ActorData caster,
		out Vector3 zoneEndPoint,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targets[0].AimDirection,
			ModdedLaserLength(),
			ModdedLaserWidth(),
			caster,
			caster.GetOtherTeams(),
			m_penetrateLineOfSight,
			ModdedMaxTargets(),
			false,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		zoneEndPoint = laserCoords.end;
		return actorsInLaser;
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 casterPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		Vector3 aimDirection = targets[0].AimDirection;
		float maxDistanceInWorld = ModdedLaserLength() * Board.Get().squareSize;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(caster.GetLoSCheckPos(), aimDirection, maxDistanceInWorld, m_penetrateLineOfSight, caster);
		Vector3 targetPos = laserEndPoint;
		ActorData[] hitActorsArray = additionalData.m_abilityResults.HitActorsArray();
		if (hitActorsArray.Length != 0)
		{
			Vector3 vector = hitActorsArray[0].transform.position - casterPos;
			Vector3 vector2 = laserEndPoint - casterPos;
			if (vector.magnitude < vector2.magnitude)
			{
				targetPos = hitActorsArray[0].transform.position;
			}
		}
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			targetPos,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource,
			new Sequence.IExtraSequenceParams[0]);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitTargets = GetHitTargets(targets, caster, out Vector3 knockbackStartPos, nonActorTargetInfo);
		float knockbackDist = GetKnockbackDist(targets[0], caster.GetLoSCheckPos(), knockbackStartPos);
		foreach (ActorData target in hitTargets)
		{
			ActorHitParameters hitParams = new ActorHitParameters(target, caster.GetFreePos());
			ActorHitResults actorHitResults = new ActorHitResults(ModdedOnHitDamage(), HitActionType.Damage, m_onHitEffect, hitParams);
			if (knockbackDist != 0f)
			{
				KnockbackHitData knockbackData = new KnockbackHitData(
					target,
					caster,
					m_knockbackType,
					targets[0].AimDirection,
					caster.GetFreePos(),
					knockbackDist);
				actorHitResults.AddKnockbackData(knockbackData);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	public override void OnAbilityAssistedKill(ActorData caster, ActorData target)
	{
		caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.RageBeastStats.KnockbackAssists);
	}
#endif
}
