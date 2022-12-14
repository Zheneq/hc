// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SniperCripplingRound : Ability
{
	public enum ExplosionType
	{
		Shape,
		Cone
	}

	[Header("-- Laser info ------------------------------------------")]
	public int m_laserDamageAmount = 5;
	public float m_laserWidth = 0.5f;
	public float m_laserRange = 5f;
	public bool m_laserPenetrateLos;
	[Header("-- Explosion --------------------------------------------")]
	public int m_explosionDamageAmount = 3;
	public bool m_alwaysExplodeOnPathEnd;
	public bool m_explodeOnEnvironmentHit;
	public bool m_clampMaxRangeToCursorPos;
	public bool m_snapToTargetShapeCenterWhenClampRange;
	public bool m_snapToTargetSquareWhenClampRange;
	public ExplosionType m_explosionType = ExplosionType.Cone;
	[Header("-- If using Shape")]
	public AbilityAreaShape m_explosionShape = AbilityAreaShape.Three_x_Three;
	[Header("-- If using Cone")]
	public float m_coneWidthAngle = 60f;
	public float m_coneLength = 4f;
	public float m_coneBackwardOffset;
	[Header("-- Effects ----------------------------------------------")]
	public StandardEffectInfo m_effectOnLaserHitTargets;
	[Header("-----")]
	public StandardEffectInfo m_effectOnExplosionHitTargets;

	private AbilityMod_SniperCripplingRound m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = string.Empty;
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_explosionType == ExplosionType.Cone)
		{
			AbilityUtil_Targeter_LaserWithCone targeter = new AbilityUtil_Targeter_LaserWithCone(
				this,
				m_laserWidth,
				m_laserRange,
				m_laserPenetrateLos, 
				false,
				m_coneWidthAngle,
				m_coneLength,
				m_coneBackwardOffset);
			targeter.SetMaxLaserTargets(GetModdedMaxLaserTargets());
			targeter.SetExplodeOnPathEnd(m_alwaysExplodeOnPathEnd);
			targeter.SetExplodeOnEnvironmentHit(m_explodeOnEnvironmentHit);
			targeter.SetClampToCursorPos(m_clampMaxRangeToCursorPos);
			targeter.SetSnapToTargetSquareWhenClampRange(m_snapToTargetSquareWhenClampRange);
			targeter.SetAddDirectHitActorAsPrimary(GetLaserDamage() > 0);
			Targeter = targeter;
		}
		else
		{
			LaserTargetingInfo laserTargetingInfo = new LaserTargetingInfo
			{
				maxTargets = GetModdedMaxLaserTargets(),
				penetrateLos = m_laserPenetrateLos,
				range = m_laserRange,
				width = m_laserWidth
			};
			AbilityUtil_Targeter_LaserWithShape targeter = new AbilityUtil_Targeter_LaserWithShape(
				this,
				laserTargetingInfo,
				m_explosionShape);
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
		return m_laserRange;
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
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		int damage = 0;
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
		{
			damage += GetLaserDamage();
		}
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
		{
			damage += GetExplosionDamage();
		}
		return new Dictionary<AbilityTooltipSymbol, int>
		{
			[AbilityTooltipSymbol.Damage] = damage
		};
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SniperCripplingRound abilityMod_SniperCripplingRound = modAsBase as AbilityMod_SniperCripplingRound;
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, abilityMod_SniperCripplingRound != null
			? abilityMod_SniperCripplingRound.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount);
		AddTokenInt(tokens, "ExplosionDamageAmount", string.Empty, abilityMod_SniperCripplingRound != null
			? abilityMod_SniperCripplingRound.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnLaserHitTargets, "EffectOnLaserHitTargets", m_effectOnLaserHitTargets);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnExplosionHitTargets, "EffectOnExplosionHitTargets", m_effectOnExplosionHitTargets);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SniperCripplingRound))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}

		m_abilityMod = abilityMod as AbilityMod_SniperCripplingRound;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private int GetLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	private int GetExplosionDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount;
	}

	private int GetLaserEffectDuration()
	{
		int duration = m_effectOnLaserHitTargets.m_effectData.m_duration;
		if (m_abilityMod != null)
		{
			duration = m_abilityMod.m_enemyHitEffectDurationMod.GetModifiedValue(duration);
		}
		return duration;
	}

	private int GetExplosionEffectDuration()
	{
		int duration = m_effectOnExplosionHitTargets.m_effectData.m_duration;
		if (m_abilityMod != null)
		{
			duration = m_abilityMod.m_enemyHitEffectDurationMod.GetModifiedValue(duration);
		}
		return duration;
	}

	private int GetModdedMaxLaserTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(1)
			: 1;
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ActorData> explosionHitActors = FindExplosionHitActors(
			targets,
			caster,
			out var primaryHitActor,
			out var endPoints,
			out var shouldExplode,
			null);
		GetSequenceActorsAndPosition(
			targets,
			caster,
			additionalData,
			primaryHitActor,
			explosionHitActors,
			endPoints, 
			out var sequenceActors,
			out var targetPos);
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			targetPos,
			sequenceActors.ToArray(),
			caster,
			additionalData.m_sequenceSource,
			new Sequence.IExtraSequenceParams[]
		{
			new SplineProjectileSequence.DelayedProjectileExtraParams
			{
				skipImpactFx = !shouldExplode
			},
			new SplineProjectileSequence.ProjectilePropertyParams
			{
				projectileWidthInWorld = m_laserWidth * Board.Get().squareSize
			}
		});
	}

	// added in rogues
	private void GetSequenceActorsAndPosition(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData,
		List<ActorData> primaryHitActor,
		List<ActorData> explosionHitActors,
		VectorUtils.LaserCoords endPoints,
		out List<ActorData> sequenceActors,
		out Vector3 sequenceHitPos)
	{
		List<ActorData> hitActors = new List<ActorData>(explosionHitActors.Count + primaryHitActor.Count);
		foreach (ActorData hitActor in primaryHitActor)
		{
			hitActors.Add(hitActor);
		}
		foreach (ActorData hitActor in explosionHitActors)
		{
			if (!primaryHitActor.Contains(hitActor))
			{
				hitActors.Add(hitActor);
			}
		}
		Vector3 vector = endPoints.end;
		if (m_explosionType == ExplosionType.Shape)
		{
			AreaEffectUtils.GetEndPointForValidGameplaySquare(endPoints.start, endPoints.end, out var adjustedEndPoint);
			BoardSquare squareFromVec = Board.Get().GetSquareFromVec3(adjustedEndPoint);
			vector = AreaEffectUtils.GetCenterOfShape(m_explosionShape, adjustedEndPoint, squareFromVec);
		}
		vector.y = targets[0].FreePos.y;
		sequenceActors = hitActors;
		sequenceHitPos = vector;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> explosionHitActors = FindExplosionHitActors(targets, caster, out var laserTargets, out var laserCoords, out _, nonActorTargetInfo);
		List<ActorData> list3 = new List<ActorData>();
		foreach (ActorData actorData in laserTargets)
		{
			if (actorData != null && explosionHitActors.Contains(actorData))
			{
				list3.Add(actorData);
			}
			if (!explosionHitActors.Contains(actorData) && laserTargets != null)
			{
				explosionHitActors.Add(actorData);
			}
		}
		foreach (ActorData actorData in explosionHitActors)
		{
			int num;
			Vector3 origin;
			if (laserTargets.Contains(actorData))
			{
				num = GetLaserDamage();
				if (list3.Contains(actorData))
				{
					num += GetExplosionDamage();
					origin = laserCoords.end;
				}
				else
				{
					origin = caster.GetFreePos();
				}
			}
			else
			{
				num = GetExplosionDamage();
				origin = laserCoords.end;
			}
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, origin));
			actorHitResults.SetBaseDamage(num);
			if (laserTargets.Contains(actorData))
			{
				StandardActorEffect standardActorEffect = m_effectOnLaserHitTargets.CreateEffect(AsEffectSource(), actorData, caster);
				if (standardActorEffect != null)
				{
					standardActorEffect.SetDurationBeforeStart(GetLaserEffectDuration());
					actorHitResults.AddEffect(standardActorEffect);
				}
			}
			else
			{
				StandardActorEffect standardActorEffect = m_effectOnExplosionHitTargets.CreateEffect(AsEffectSource(), actorData, caster);
				if (standardActorEffect != null)
				{
					standardActorEffect.SetDurationBeforeStart(GetExplosionEffectDuration());
					actorHitResults.AddEffect(standardActorEffect);
				}
			}
			if (m_abilityMod != null && m_abilityMod.m_additionalEnemyHitEffect.m_applyEffect)
			{
				StandardActorEffect effect = new StandardActorEffect(
					AsEffectSource(),
					actorData.GetCurrentBoardSquare(),
					actorData,
					caster,
					m_abilityMod.m_additionalEnemyHitEffect.m_effectData);
				actorHitResults.AddEffect(effect);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private bool SnapToTargetSquare()
	{
		return m_clampMaxRangeToCursorPos
		       && m_snapToTargetSquareWhenClampRange
		       && (m_explosionType == ExplosionType.Cone || !m_snapToTargetShapeCenterWhenClampRange);
	}

	// added in rogues
	private bool SnapToTargetShapeCenter()
	{
		return m_clampMaxRangeToCursorPos
		       && m_explosionType == ExplosionType.Shape
		       && m_snapToTargetShapeCenterWhenClampRange;
	}

	// added in rogues
	private bool SnapAimDirection()
	{
		return SnapToTargetShapeCenter() || SnapToTargetSquare();
	}

	// added in rogues
	private List<ActorData> FindExplosionHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<ActorData> laserTargets,
		out VectorUtils.LaserCoords laserEndPoints,
		out bool shouldExplode,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 vector = targets[0].AimDirection;
		Vector3 b = targets[0].FreePos;
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		if (SnapAimDirection() && square != null && square != caster.GetCurrentBoardSquare())
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_explosionShape, square.ToVector3(), square);
			Vector3 vector2 = SnapToTargetShapeCenter() ? centerOfShape : square.ToVector3();
			vector = vector2 - caster.GetFreePos();
			vector.y = 0f;
			vector.Normalize();
			b = vector2;
		}
		List<Team> list = new List<Team>();
		list.AddRange(caster.GetOtherTeams());
		float num = m_laserRange;
		if (m_clampMaxRangeToCursorPos)
		{
			num = Mathf.Min(VectorUtils.HorizontalPlaneDistInSquares(caster.GetFreePos(), b), num);
		}
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		laserTargets = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targets[0].AimDirection,
			num,
			m_laserWidth,
			caster,
			list,
			m_laserPenetrateLos,
			GetModdedMaxLaserTargets(),
			false,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		bool flag = AreaEffectUtils.LaserHitWorldGeo(num, laserCoords, m_laserPenetrateLos, laserTargets);
		laserEndPoints = laserCoords;
		shouldExplode = m_alwaysExplodeOnPathEnd || (flag && m_explodeOnEnvironmentHit) || laserTargets.Count > 0;
		List<ActorData> explosionHitActors;
		if (shouldExplode)
		{
			Vector3 vector3;
			AreaEffectUtils.GetEndPointForValidGameplaySquare(laserEndPoints.start, laserEndPoints.end, out vector3);
			if (m_explosionType == ExplosionType.Cone)
			{
				BoardSquare squareFromVec = Board.Get().GetSquareFromVec3(vector3);
				explosionHitActors = AreaEffectUtils.GetActorsInCone(
					laserEndPoints.end,
					VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection),
					m_coneWidthAngle,
					m_coneLength,
					m_coneBackwardOffset,
					m_laserPenetrateLos,
					caster,
					caster.GetOtherTeams(),
					nonActorTargetInfo);
				TargeterUtils.RemoveActorsWithoutLosToSquare(ref explosionHitActors, squareFromVec, caster);
				foreach (ActorData item in laserTargets)
				{
					if (explosionHitActors.Contains(item))
					{
						explosionHitActors.Remove(item);
					}
				}
				return explosionHitActors;
			}
			BoardSquare squareFromVec2 = Board.Get().GetSquareFromVec3(vector3);
			Vector3 centerOfShape2 = AreaEffectUtils.GetCenterOfShape(m_explosionShape, vector3, squareFromVec2);
			explosionHitActors = AreaEffectUtils.GetActorsInShape(
				m_explosionShape,
				centerOfShape2,
				squareFromVec2,
				false,
				caster,
				caster.GetOtherTeams(),
				nonActorTargetInfo);
		}
		else
		{
			explosionHitActors = new List<ActorData>();
		}
		return explosionHitActors;
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalTechPointsCasterGain > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SniperStats.EnergyGainedByVortexRound, results.FinalTechPointsCasterGain);
		}
		if (caster == target && results.FinalTechPointsGain > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SniperStats.EnergyGainedByVortexRound, results.FinalTechPointsGain);
		}
	}
#endif
}
