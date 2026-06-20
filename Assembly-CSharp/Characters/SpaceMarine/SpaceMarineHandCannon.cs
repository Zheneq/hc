// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SpaceMarineHandCannon : Ability
{
	public int m_primaryDamage;
	public float m_primaryWidth = 1f;
	public float m_primaryLength = 3f;
	public int m_coneDamage;
	public float m_coneWidthAngle = 60f;
	public float m_coneLength = 4f;
	public float m_coneBackwardOffset;
	public bool m_penetrateLineOfSight;
	public StandardEffectInfo m_effectInfoOnPrimaryTarget;
	public StandardEffectInfo m_effectInfoOnConeTargets;

	private AbilityMod_SpaceMarineHandCannon m_abilityMod;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (ShouldExplode())
		{
			Targeter = new AbilityUtil_Targeter_LaserWithCone(
				this,
				ModdedLaserWidth(),
				ModdedLaserLength(),
				m_penetrateLineOfSight,
				false,
				ModdedConeAngle(),
				ModdedConeLength(),
				m_coneBackwardOffset);
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_Laser(
				this,
				ModdedLaserWidth(),
				ModdedLaserLength(),
				m_penetrateLineOfSight,
				1);
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		float coneLength = 0f;
		if (ShouldExplode())
		{
			coneLength = ModdedConeLength();
		}
		return ModdedLaserLength() + coneLength;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_primaryDamage));
		m_effectInfoOnPrimaryTarget.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, m_coneDamage));
		m_effectInfoOnConeTargets.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
		{
			dictionary[AbilityTooltipSymbol.Damage] = ModdedLaserDamage();
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
		{
			dictionary[AbilityTooltipSymbol.Damage] = ModdedConeDamage();
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SpaceMarineHandCannon abilityMod_SpaceMarineHandCannon = modAsBase as AbilityMod_SpaceMarineHandCannon;
		AddTokenInt(tokens, "PrimaryDamage", string.Empty, abilityMod_SpaceMarineHandCannon != null
			? abilityMod_SpaceMarineHandCannon.m_laserDamageMod.GetModifiedValue(m_primaryDamage)
			: m_primaryDamage);
		AddTokenInt(tokens, "ConeDamage", string.Empty, abilityMod_SpaceMarineHandCannon != null
			? abilityMod_SpaceMarineHandCannon.m_coneDamageMod.GetModifiedValue(m_coneDamage)
			: m_coneDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectInfoOnPrimaryTarget, "EffectInfoOnPrimaryTarget", m_effectInfoOnPrimaryTarget);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectInfoOnConeTargets, "EffectInfoOnConeTargets", m_effectInfoOnConeTargets);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SpaceMarineHandCannon))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_SpaceMarineHandCannon;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public int ModdedLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageMod.GetModifiedValue(m_primaryDamage)
			: m_primaryDamage;
	}

	public int ModdedConeDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneDamageMod.GetModifiedValue(m_coneDamage)
			: m_coneDamage;
	}

	public float ModdedConeAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public float ModdedConeLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength)
			: m_coneLength;
	}

	public float ModdedLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_primaryWidth)
			: m_primaryWidth;
	}

	public float ModdedLaserLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserLengthMod.GetModifiedValue(m_primaryLength)
			: m_primaryLength;
	}

	public bool ShouldExplode()
	{
		return m_abilityMod == null || m_abilityMod.m_shouldExplodeMod.GetModifiedValue(true);
	}

	public StandardEffectInfo GetLaserEffectInfo()
	{
		return m_abilityMod != null && m_abilityMod.m_useLaserHitEffectOverride
			? m_abilityMod.m_laserHitEffectOverride
			: m_effectInfoOnPrimaryTarget;
	}

	public StandardEffectInfo GetConeEffectInfo()
	{
		return m_abilityMod != null && m_abilityMod.m_useConeHitEffectOverride
			? m_abilityMod.m_coneHitEffectOverride
			: m_effectInfoOnConeTargets;
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		FindHitActors(targets, caster, out _, out Vector3 coneOrigin, null);
		SplineProjectileSequence.ProjectilePropertyParams projectilePropertyParams = new SplineProjectileSequence.ProjectilePropertyParams
			{
				projectileWidthInWorld = ModdedLaserWidth() * Board.Get().squareSize
			};
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			coneOrigin,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource,
			projectilePropertyParams.ToArray());
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = FindHitActors(targets, caster, out ActorData primaryTarget, out Vector3 coneOrigin, nonActorTargetInfo);
		if (!hitActors.Contains(primaryTarget) && primaryTarget != null)
		{
			hitActors.Add(primaryTarget);
		}
		foreach (ActorData hitActor in hitActors)
		{
			int baseDamage;
			Vector3 origin;
			StandardEffectInfo effectInfo;
			if (hitActor == primaryTarget)
			{
				baseDamage = ModdedLaserDamage();
				origin = caster.GetLoSCheckPos();
				effectInfo = GetLaserEffectInfo();
			}
			else
			{
				baseDamage = ModdedConeDamage();
				origin = coneOrigin;
				effectInfo = GetConeEffectInfo();
			}
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, origin));
			actorHitResults.SetBaseDamage(baseDamage);
			actorHitResults.AddStandardEffectInfo(effectInfo);
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> FindHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out ActorData primaryTarget,
		out Vector3 coneOrigin,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 aimDirection = targets[0].AimDirection;
		new List<Team>().AddRange(caster.GetOtherTeams());
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
			1,
			false,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		primaryTarget = null;
		if (actorsInLaser.Count > 0)
		{
			primaryTarget = actorsInLaser[0];
		}
		coneOrigin = laserCoords.end;
		List<ActorData> result;
		if (primaryTarget != null && ShouldExplode())
		{
			AreaEffectUtils.GetEndPointForValidGameplaySquare(laserCoords.start, laserCoords.end, out Vector3 adjustedEndPoint);
			BoardSquare endPointSquare = Board.Get().GetSquareFromVec3(adjustedEndPoint);
			result = AreaEffectUtils.GetActorsInCone(
				coneOrigin,
				VectorUtils.HorizontalAngle_Deg(aimDirection),
				ModdedConeAngle(),
				ModdedConeLength(),
				m_coneBackwardOffset,
				m_penetrateLineOfSight,
				caster,
				caster.GetOtherTeams(),
				nonActorTargetInfo);
			TargeterUtils.RemoveActorsWithoutLosToSquare(ref result, endPointSquare, caster);
		}
		else
		{
			result = new List<ActorData>();
		}
		return result;
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.AppliedStatus(StatusType.Rooted) || results.AppliedStatus(StatusType.Snared))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.SpaceMarineStats.NumSlowsPlusRootsApplied);
		}
	}
#endif
}
