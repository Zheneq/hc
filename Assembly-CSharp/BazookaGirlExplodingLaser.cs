// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// empty in rouges
public class BazookaGirlExplodingLaser : Ability
{
	public enum ExplosionType
	{
		Shape,
		Cone
	}

	[Header("-- Targeting")]
	public bool m_clampMaxRangeToCursorPos;
	public bool m_snapToTargetShapeCenterWhenClampRange;
	public bool m_snapToTargetSquareWhenClampRange;
	[Header("-- Targeting: If using Shape")]
	public AbilityAreaShape m_explosionShape = AbilityAreaShape.Three_x_Three;
	[Header("-- Targeting: If using Cone")]
	public float m_coneWidthAngle = 60f;
	public float m_coneLength = 4f;
	public float m_coneBackwardOffset;
	[Header("-- Laser Params")]
	public float m_laserWidth = 0.5f;
	public float m_laserRange = 5f;
	public bool m_laserIgnoreCover;
	public bool m_laserPenetrateLos;
	[Header("-- Laser Hit")]
	public int m_laserDamageAmount = 5;
	public StandardEffectInfo m_effectOnLaserHitTargets;
	[Header("-- Cooldown reduction on direct laser hit --")]
	public int m_cdrOnDirectHit;
	public AbilityData.ActionType m_cdrTargetActionType = AbilityData.ActionType.INVALID_ACTION;
	[Header("-- Explosion Params")]
	public ExplosionType m_explosionType = ExplosionType.Cone;
	public bool m_alwaysExplodeOnPathEnd;
	public bool m_explodeOnEnvironmentHit;
	public bool m_explosionIgnoreCover;
	public bool m_explosionPenetrateLos;
	[Header("-- Explosion Hit")]
	public int m_explosionDamageAmount = 3;
	public StandardEffectInfo m_effectOnExplosionHitTargets;

	private AbilityMod_BazookaGirlExplodingLaser m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Exploding Laser";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_explosionType == ExplosionType.Cone)
		{
			AbilityUtil_Targeter_LaserWithCone targeter = new AbilityUtil_Targeter_LaserWithCone(this, GetLaserWidth(), GetLaserRange(), LaserPenetrateLos(), false, GetConeWidthAngle(), GetConeLength(), GetConeBackwardOffset());
			targeter.SetExplodeOnPathEnd(m_alwaysExplodeOnPathEnd);
			targeter.SetExplodeOnEnvironmentHit(m_explodeOnEnvironmentHit);
			targeter.SetClampToCursorPos(m_clampMaxRangeToCursorPos);
			targeter.SetSnapToTargetSquareWhenClampRange(m_snapToTargetSquareWhenClampRange);
			targeter.SetAddDirectHitActorAsPrimary(GetLaserDamage() > 0);
			targeter.SetCoverAndLosConfig(LaserIgnoreCover(), ExplosionIgnoresCover(), ExplosionPenetrateLos());
			Targeter = targeter;
		}
		else
		{
			LaserTargetingInfo laserTargetingInfo = new LaserTargetingInfo
			{
				maxTargets = 1,
				penetrateLos = LaserPenetrateLos(),
				range = GetLaserRange(),
				width = GetLaserWidth()
			};
			AbilityUtil_Targeter_LaserWithShape targeter = new AbilityUtil_Targeter_LaserWithShape(this, laserTargetingInfo, m_explosionShape);
			targeter.SetExplodeOnPathEnd(m_alwaysExplodeOnPathEnd);
			targeter.SetExplodeOnEnvironmentHit(m_explodeOnEnvironmentHit);
			targeter.SetClampToCursorPos(m_clampMaxRangeToCursorPos);
			targeter.SetSnapToTargetShapeCenterWhenClampRange(m_snapToTargetShapeCenterWhenClampRange);
			targeter.SetSnapToTargetSquareWhenClampRange(m_snapToTargetSquareWhenClampRange);
			targeter.SetAddDirectHitActorAsPrimary(GetLaserDamage() > 0);
			Targeter = targeter;
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange() + GetConeLength();
	}

	public float GetConeWidthAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public float GetConeLength()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength) 
			: m_coneLength;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset) 
			: m_coneBackwardOffset;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth) 
			: m_laserWidth;
	}

	public float GetLaserRange()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange) 
			: m_laserRange;
	}

	public bool LaserPenetrateLos()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_laserPenetrateLosMod.GetModifiedValue(m_laserPenetrateLos) 
			: m_laserPenetrateLos;
	}

	public int GetLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result = m_abilityMod != null
			? m_abilityMod.m_laserHitEffectOverride.GetModifiedValue(m_effectOnLaserHitTargets)
			: m_effectOnLaserHitTargets;
		return result;
	}

	public bool LaserIgnoreCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserIgnoreCoverMod.GetModifiedValue(m_laserIgnoreCover)
			: m_laserIgnoreCover;
	}

	public int GetCdrOnDirectHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrOnDirectHitMod.GetModifiedValue(m_cdrOnDirectHit)
			: m_cdrOnDirectHit;
	}

	public int GetExplosionDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount;
	}

	public StandardEffectInfo GetExplosionHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionEffectOverride.GetModifiedValue(m_effectOnExplosionHitTargets)
			: m_effectOnExplosionHitTargets;
	}

	public bool ExplosionIgnoresCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionIgnoreCoverMod.GetModifiedValue(m_explosionIgnoreCover)
			: m_explosionIgnoreCover;
	}

	public bool ExplosionPenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionIgnoreLosMod.GetModifiedValue(m_explosionPenetrateLos)
			: m_explosionPenetrateLos;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		m_effectOnLaserHitTargets.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_explosionDamageAmount);
		m_effectOnExplosionHitTargets.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null) return null;
		int damage = 0;
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
		{
			damage += GetLaserDamage();
		}
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
		{
			damage += GetExplosionDamage();
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>
		{
			[AbilityTooltipSymbol.Damage] = damage
		};
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlExplodingLaser abilityMod_BazookaGirlExplodingLaser = modAsBase as AbilityMod_BazookaGirlExplodingLaser;
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, abilityMod_BazookaGirlExplodingLaser != null
			? abilityMod_BazookaGirlExplodingLaser.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BazookaGirlExplodingLaser != null
			? abilityMod_BazookaGirlExplodingLaser.m_laserHitEffectOverride.GetModifiedValue(m_effectOnLaserHitTargets)
			: m_effectOnLaserHitTargets, "EffectOnLaserHitTargets", m_effectOnLaserHitTargets);
		AddTokenInt(tokens, "ExplosionDamageAmount", string.Empty, abilityMod_BazookaGirlExplodingLaser != null
			? abilityMod_BazookaGirlExplodingLaser.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BazookaGirlExplodingLaser != null
			? abilityMod_BazookaGirlExplodingLaser.m_explosionEffectOverride.GetModifiedValue(m_effectOnExplosionHitTargets)
			: m_effectOnExplosionHitTargets, "EffectOnExplosionHitTargets", m_effectOnExplosionHitTargets);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BazookaGirlExplodingLaser))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_BazookaGirlExplodingLaser;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// custom
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 targetPos;
		Vector3 aimDirection = m_explosionType == ExplosionType.Cone 
			? GetAimDirectionLaserWithCone(caster, targets, out targetPos)
			: GetAimDirectionLaserWithShape(caster, targets, out targetPos);
		GetHitActorsLaser(caster, targetPos, aimDirection, null, out var adjustedCoords, out _);

		Sequence.IExtraSequenceParams[] extraParams = {
			new SplineProjectileSequence.DelayedProjectileExtraParams
			{
				startDelay = -1.0f
			},
			new SplineProjectileSequence.ProjectilePropertyParams
			{
				projectileWidthInWorld = 1.125f
			}
		};
		
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			adjustedCoords.end,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource,
			extraParams);
	}
	
	// custom
	public override void GatherAbilityResults(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		if (m_explosionType == ExplosionType.Cone)
		{
			GatherAbilityResultsLaserWithCone(targets, caster, ref abilityResults);
		}
		else
		{
			GatherAbilityResultsLaserWithShape(targets, caster, ref abilityResults);
		}
	}
	
	// custom
	private void GatherAbilityResultsLaserWithCone(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Vector3 aimDirection = GetAimDirectionLaserWithCone(caster, targets, out Vector3 targetPos);
		List<ActorData> actorsInLaser = GetHitActorsLaser(
			caster, targetPos, aimDirection, nonActorTargetInfo, out var adjustedCoords, out bool hitEnv);
		if (GetLaserDamage() > 0)
		{
			foreach (ActorData target in actorsInLaser)
			{
				ActorHitParameters hitParams = new ActorHitParameters(target, caster.GetFreePos());
				ActorHitResults hitResults = new ActorHitResults(GetLaserDamage(), HitActionType.Damage, GetLaserHitEffect(), hitParams);
				abilityResults.StoreActorHit(hitResults);
			}
		}
		
		Vector3 coneStart = adjustedCoords.end;
		Vector3 losOverridePos = coneStart;
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDirection);
		if (m_alwaysExplodeOnPathEnd
		    || hitEnv && m_explodeOnEnvironmentHit
		    || actorsInLaser.Count > 0)
		{
			if (!m_explosionPenetrateLos)
			{
				losOverridePos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(adjustedCoords.start, coneStart);
			}
			List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(
				coneStart,
				coneCenterAngleDegrees,
				GetConeWidthAngle(),
				GetConeLength(),
				GetConeBackwardOffset(),
				m_explosionPenetrateLos,
				caster, 
				null,
				null,
				true,
				losOverridePos);
			foreach (ActorData target in actorsInCone)
			{
				if (target != null && target.GetTeam() != caster.GetTeam())
				{
					ActorHitParameters hitParams = new ActorHitParameters(target, coneStart);
					ActorHitResults hitResults = new ActorHitResults(GetExplosionDamage(), HitActionType.Damage, GetExplosionHitEffect(), hitParams);
					abilityResults.StoreActorHit(hitResults);
				}
			}
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
	
	// custom
	private void GatherAbilityResultsLaserWithShape(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Vector3 aimDirection = GetAimDirectionLaserWithShape(caster, targets, out Vector3 targetPos);
		List<ActorData> actorsInLaser = GetHitActorsLaser(
			caster, targetPos, aimDirection, nonActorTargetInfo, out var adjustedCoords, out bool hitEnv);
		foreach (ActorData target in actorsInLaser)
		{
			ActorHitParameters hitParams = new ActorHitParameters(target, caster.GetFreePos());
			ActorHitResults hitResults = new ActorHitResults(GetLaserDamage(), HitActionType.Damage, GetLaserHitEffect(), hitParams);
			abilityResults.StoreActorHit(hitResults);
		}
		if (m_alwaysExplodeOnPathEnd
		    || hitEnv && m_explodeOnEnvironmentHit
		    || actorsInLaser.Count > 0)
		{
			AreaEffectUtils.GetEndPointForValidGameplaySquare(adjustedCoords.start, adjustedCoords.end, out Vector3 adjustedEndPoint);
			BoardSquare endPointSquare = Board.Get().GetSquareFromVec3(adjustedEndPoint);
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_explosionShape, adjustedEndPoint, endPointSquare);
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
				m_explosionShape,
				centerOfShape,
				endPointSquare, 
				false,
				caster,
				caster.GetOtherTeams(),
				nonActorTargetInfo);
			foreach (ActorData target in actorsInShape)
			{
				if (!actorsInLaser.Contains(target))
				{
					ActorHitParameters hitParams = new ActorHitParameters(target, centerOfShape);
					ActorHitResults hitResults = new ActorHitResults(GetExplosionDamage(), HitActionType.Damage, GetExplosionHitEffect(), hitParams);
					abilityResults.StoreActorHit(hitResults);
				}
			}
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// custom
	private Vector3 GetAimDirectionLaserWithCone(ActorData caster, List<AbilityTarget> targets, out Vector3 targetPos)
	{
		AbilityTarget currentTarget = targets[0];
		Vector3 aimDirection = currentTarget?.AimDirection ?? caster.transform.forward;
		targetPos = currentTarget.FreePos;
		BoardSquare targetSquare = Board.Get().GetSquare(currentTarget.GridPos);
		if (m_clampMaxRangeToCursorPos
		    && m_snapToTargetSquareWhenClampRange
		    && targetSquare != null
		    && targetSquare != caster.GetCurrentBoardSquare())
		{
			aimDirection = targetSquare.ToVector3() - caster.GetFreePos();
			aimDirection.y = 0f;
			aimDirection.Normalize();
			targetPos = targetSquare.ToVector3();
		}

		targetPos.y = 1.0f;

		return aimDirection;
	}

	// custom
	private Vector3 GetAimDirectionLaserWithShape(ActorData caster, List<AbilityTarget> targets, out Vector3 targetPos)
	{
		AbilityTarget currentTarget = targets[0];
		Vector3 aimDirection = currentTarget?.AimDirection ?? caster.transform.forward;
		targetPos = currentTarget.FreePos;
		BoardSquare targetSquare = Board.Get().GetSquare(currentTarget.GridPos);
		if (SnapAimDirection()
		    && targetSquare != null
		    && targetSquare != caster.GetCurrentBoardSquare())
		{
			Vector3 centerOfShape =
				AreaEffectUtils.GetCenterOfShape(m_explosionShape, targetSquare.ToVector3(), targetSquare);
			Vector3 snapTargetPos = SnapToTargetShapeCenter() ? centerOfShape : targetSquare.ToVector3();
			aimDirection = snapTargetPos - caster.GetFreePos();
			aimDirection.y = 0f;
			aimDirection.Normalize();
			targetPos = snapTargetPos;
		}
		return aimDirection;
	}

	// custom
	private List<ActorData> GetHitActorsLaser(
		ActorData caster,
		Vector3 targetPos,
		Vector3 aimDirection,
		List<NonActorTargetInfo> nonActorTargetInfo,
		out VectorUtils.LaserCoords adjustedCoords,
		out bool hitEnv)
	{
		float distance = GetLaserRange();
		if (m_clampMaxRangeToCursorPos)
		{
			float clampedDistance = VectorUtils.HorizontalPlaneDistInSquares(caster.GetFreePos(), targetPos);
			distance = Mathf.Min(clampedDistance, distance);
		}

		adjustedCoords = default(VectorUtils.LaserCoords);
		adjustedCoords.start = caster.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			adjustedCoords.start,
			aimDirection,
			distance,
			GetLaserWidth(),
			caster,
			caster.GetOtherTeams(),
			LaserPenetrateLos(),
			1,
			false,
			true,
			out adjustedCoords.end,
			nonActorTargetInfo);
		hitEnv = AreaEffectUtils.LaserHitWorldGeo(distance, adjustedCoords, LaserPenetrateLos(), actorsInLaser);
		return actorsInLaser;
	}

	// custom
	private bool SnapAimDirection()
	{
		return SnapToTargetSquare() || SnapToTargetShapeCenter();
	}

	// custom
	private bool SnapToTargetSquare()
	{
		return m_clampMaxRangeToCursorPos
		       && m_snapToTargetSquareWhenClampRange
		       && !m_snapToTargetShapeCenterWhenClampRange;
	}

	// custom
	private bool SnapToTargetShapeCenter()
	{
		return m_clampMaxRangeToCursorPos && m_snapToTargetShapeCenterWhenClampRange;
	}
#endif
}
