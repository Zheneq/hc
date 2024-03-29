﻿// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieDashAoE : Ability
{
	public enum DashTargetingMode
	{
		Aoe,
		AimShieldCone
	}

	[Header("-- Shield effect")]
	public StandardEffectInfo m_shieldEffectInfo;
	public int m_techPointGainPerCoveredHit = 5;
	public int m_techPointGainPerTooCloseForCoverHit;
	[Separator("Dash Target Mode")]
	public DashTargetingMode m_dashTargetingMode;
	[Header("-- Targeting")]
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;
	public bool m_aoePenetratesLoS;
	[Separator("Aim Shield and Cone")]
	public float m_coneWidthAngle = 110f;
	public float m_coneRadius = 2.5f;
	public int m_coverDuration = 1;
	[Header("-- Cover Ignore Min Dist?")]
	public bool m_coverIgnoreMinDist = true;
	[Header("-- Whether to put guard ability on cooldown")]
	public bool m_triggerCooldownOnGuardAbiity;
	[Separator("Enemy hits")]
	public int m_damage = 20;
	public StandardEffectInfo m_enemyDebuff;
	[Separator("Ally & self hits")]
	public int m_absorb = 20;
	public AbilityCooldownChangeInfo m_cooldownReductionIfDamagedThisTurn;
	public StandardEffectInfo m_allyBuff;
	public StandardEffectInfo m_selfBuff;
	[Separator("Sequences")]
	public GameObject m_castSequencePrefab;
	
	private AbilityMod_ValkyrieDashAoE m_abilityMod;
	private StandardEffectInfo m_cachedShieldEffectInfo;
	private StandardEffectInfo m_cachedEnemyDebuff;
	private StandardEffectInfo m_cachedAllyBuff;
	private StandardEffectInfo m_cachedSelfBuff;

#if SERVER
	//Added in rouges
	private AbilityData m_abilityData;
	//Added in rouges
	private ValkyrieGuard m_guardAbility;
	//Added in rouges
	private AbilityData.ActionType m_guardAbilityActionType = AbilityData.ActionType.INVALID_ACTION;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Valkyrie Dash AoE";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
#if SERVER
		// added in rogues
		m_abilityData = GetComponent<AbilityData>();
		if (m_abilityData != null)
		{
			m_guardAbility = m_abilityData.GetAbilityOfType(typeof(ValkyrieGuard)) as ValkyrieGuard;
			m_guardAbilityActionType = m_abilityData.GetActionTypeOfAbility(m_guardAbility);
		}
#endif
		SetCachedFields();
		Targeters.Clear();
		if (m_dashTargetingMode == DashTargetingMode.Aoe)
		{
			Targeter = new AbilityUtil_Targeter_BattleMonkUltimate(
				this,
				GetAoeShape(),
				AoePenetratesLoS(),
				GetAoeShape(),
				AoePenetratesLoS(),
				true);
			Targeter.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeSelf());
		}
		else
		{
			AbilityUtil_Targeter_Charge targeterCharge = new AbilityUtil_Targeter_Charge(
				this,
				AbilityAreaShape.SingleSquare,
				true,
				AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
				false);
			AbilityUtil_Targeter_ValkyrieGuard targeterGuard = new AbilityUtil_Targeter_ValkyrieGuard(
				this,
				1f,
				true,
				false,
				false);
			targeterGuard.SetConeParams(true, GetConeWidthAngle(), GetConeRadius(), false);
			targeterGuard.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeSelf());
			targeterGuard.SetUseMultiTargetUpdate(true);
			Targeters.Add(targeterCharge);
			Targeters.Add(targeterGuard);
		}
	}

	private void SetCachedFields()
	{
		m_cachedShieldEffectInfo = m_abilityMod != null
			? m_abilityMod.m_shieldEffectInfoMod.GetModifiedValue(m_shieldEffectInfo)
			: m_shieldEffectInfo;
		m_cachedEnemyDebuff = m_abilityMod != null
			? m_abilityMod.m_enemyDebuffMod.GetModifiedValue(m_enemyDebuff)
			: m_enemyDebuff;
		m_cachedAllyBuff = m_abilityMod != null
			? m_abilityMod.m_allyBuffMod.GetModifiedValue(m_allyBuff)
			: m_allyBuff;
		m_cachedSelfBuff = m_abilityMod != null
			? m_abilityMod.m_selfBuffMod.GetModifiedValue(m_selfBuff)
			: m_selfBuff;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyDebuff, "EnemyDebuff", m_enemyDebuff);
		AddTokenInt(tokens, "Absorb", string.Empty, m_absorb);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyBuff, "AllyBuff", m_allyBuff);
		AbilityMod.AddToken_EffectInfo(tokens, m_selfBuff, "SelfBuff", m_selfBuff);
		AddTokenInt(tokens, "CoverDuration", string.Empty, m_coverDuration);
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_dashTargetingMode == DashTargetingMode.Aoe
			? 1
			: Mathf.Min(GetTargetData().Length, 2);
	}

	public bool IncludeEnemies()
	{
		return GetDamage() > 0 || m_enemyDebuff.m_applyEffect;
	}

	public bool IncludeAllies()
	{
		return m_allyBuff.m_applyEffect;
	}

	public bool IncludeSelf()
	{
		return m_selfBuff.m_applyEffect;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_damage != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damage));
		}
		if (m_absorb != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Ally, m_absorb));
		}
		m_enemyDebuff.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		m_allyBuff.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		m_selfBuff.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor.GetTeam() != ActorData.GetTeam())
		{
			dictionary[AbilityTooltipSymbol.Damage] = GetDamage();
		}
		else
		{
			dictionary[AbilityTooltipSymbol.Absorb] = GetAbsorb();
		}
		return dictionary;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetIndex != 0)
		{
			return true;
		}
		return targetSquare != null
		       && targetSquare.IsValidForGameplay()
		       && targetSquare != caster.GetCurrentBoardSquare()
		       && KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare) != null;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return caster != null
		       && caster.GetAbilityData() != null
		       && !caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ValkyrieGuard)); // , true in rogues
	}

	public StandardEffectInfo GetShieldEffectInfo()
	{
		return m_cachedShieldEffectInfo ?? m_shieldEffectInfo;
	}

	public AbilityAreaShape GetAoeShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeShapeMod.GetModifiedValue(m_aoeShape)
			: m_aoeShape;
	}

	public bool AoePenetratesLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoePenetratesLoSMod.GetModifiedValue(m_aoePenetratesLoS)
			: m_aoePenetratesLoS;
	}

	public float GetConeWidthAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public float GetConeRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneRadiusMod.GetModifiedValue(m_coneRadius)
			: m_coneRadius;
	}

	public int GetCoverDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coverDurationMod.GetModifiedValue(m_coverDuration)
			: m_coverDuration;
	}

	public bool CoverIgnoreMinDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coverIgnoreMinDistMod.GetModifiedValue(m_coverIgnoreMinDist)
			: m_coverIgnoreMinDist;
	}

	public bool TriggerCooldownOnGuardAbiity()
	{
		return m_abilityMod != null
			? m_abilityMod.m_triggerCooldownOnGuardAbiityMod.GetModifiedValue(m_triggerCooldownOnGuardAbiity)
			: m_triggerCooldownOnGuardAbiity;
	}

	public int GetTechPointGainPerCoveredHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGainPerCoveredHitMod.GetModifiedValue(m_techPointGainPerCoveredHit)
			: m_techPointGainPerCoveredHit;
	}

	public int GetTechPointGainPerTooCloseForCoverHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGainPerTooCloseForCoverHitMod.GetModifiedValue(m_techPointGainPerTooCloseForCoverHit)
			: m_techPointGainPerTooCloseForCoverHit;
	}

	public int GetDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	public StandardEffectInfo GetEnemyDebuff()
	{
		return m_cachedEnemyDebuff ?? m_enemyDebuff;
	}

	public int GetAbsorb()
	{
		return m_abilityMod != null
			? m_abilityMod.m_absorbMod.GetModifiedValue(m_absorb)
			: m_absorb;
	}

	public StandardEffectInfo GetAllyBuff()
	{
		return m_cachedAllyBuff ?? m_allyBuff;
	}

	public StandardEffectInfo GetSelfBuff()
	{
		return m_cachedSelfBuff ?? m_selfBuff;
	}

	public int GetCooldownReductionOnHitAmount()
	{
		int num = m_cooldownReductionIfDamagedThisTurn.cooldownAddAmount;
		if (m_abilityMod != null)
		{
			num = m_abilityMod.m_cooldownReductionIfDamagedThisTurnMod.GetModifiedValue(num);
		}
		return num;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyrieDashAoE))
		{
			m_abilityMod = abilityMod as AbilityMod_ValkyrieDashAoE;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			min = 1f;
			max = 1f;
			return true;
		}
		return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	public override bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		if (targetIndex == 1)
		{
			BoardSquare targetSquare = Board.Get().GetSquare(targetsSoFar[0].GridPos);
			overridePos = targetSquare.ToVector3();
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

#if SERVER
	//Added in rouges
	internal override Vector3 GetFacingDirAfterMovement(ServerEvadeUtils.EvadeInfo evade)
	{
		GetConeFacing(evade.m_request.m_targets, evade.GetMover(), out Vector3 facingDir);
		return facingDir;
	}

	//Added in rouges
	private List<ActorData> FindHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		bool includeEnemies = IncludeEnemies();
		bool includeAllies = IncludeAllies();
		bool includeSelf = IncludeSelf();
		List<Team> affectedTeams = new List<Team>();
		if (includeEnemies)
		{
			affectedTeams.AddRange(caster.GetOtherTeams());
		}
		if (includeAllies)
		{
			affectedTeams.Add(caster.GetTeam());
		}
		List<ActorData> hitActors = null;
		if (m_dashTargetingMode == DashTargetingMode.Aoe)
		{
			hitActors = AreaEffectUtils.GetActorsInShape(
				GetAoeShape(),
				targets[0],
				AoePenetratesLoS(),
				caster,
				affectedTeams,
				nonActorTargetInfo);
		}
		else
		{
			Vector3 coneStart = Board.Get().GetSquare(targets[0].GridPos).ToVector3();
			GetConeFacing(targets, caster, out Vector3 vec);
			hitActors = AreaEffectUtils.GetActorsInCone(
				coneStart,
				VectorUtils.HorizontalAngle_Deg(vec),
				GetConeWidthAngle(),
				GetConeRadius(),
				0f,
				false,
				caster,
				affectedTeams,
				nonActorTargetInfo);
			hitActors.Remove(caster);
		}
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref hitActors);
		if (includeSelf && !hitActors.Contains(caster))
		{
			hitActors.Add(caster);
		}
		return hitActors;
	}

	//Added in rouges
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_dashTargetingMode == DashTargetingMode.Aoe)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetAoeShape(), targets[0]);
			return new ServerClientUtils.SequenceStartData(m_castSequencePrefab, centerOfShape, additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource);
		}
		GetConeFacing(targets, caster, out Vector3 facingDir);
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			targetSquare,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource,
			new BlasterStretchConeSequence.ExtraParams
			{
				angleInDegrees = GetConeWidthAngle(),
				lengthInSquares = GetConeRadius(),
				forwardAngle = VectorUtils.HorizontalAngle_Deg(facingDir)
			}.ToArray());
	}

	//Added in rouges
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = FindHitActors(targets, caster, nonActorTargetInfo);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetAoeShape(), targets[0]);
		foreach (ActorData actorData in hitActors)
		{
			ActorHitParameters hitParams = new ActorHitParameters(actorData, centerOfShape);
			if (actorData.GetTeam() != caster.GetTeam())
			{
				ActorHitResults actorHitResults = new ActorHitResults(GetDamage(), HitActionType.Damage, m_enemyDebuff, hitParams);
				actorHitResults.AddStandardEffectInfo(GetEnemyDebuff()); // custom
				abilityResults.StoreActorHit(actorHitResults);
			}
			else if (actorData != caster)
			{
				GetAllyBuff().m_effectData.m_absorbAmount = GetAbsorb();
				abilityResults.StoreActorHit(new ActorHitResults(GetAllyBuff(), hitParams));
			}
		}
		if (GetSelfBuff().m_applyEffect || m_dashTargetingMode == DashTargetingMode.AimShieldCone)
		{
			ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, centerOfShape));
			casterHitResults.AddStandardEffectInfo(GetSelfBuff());
			if (m_dashTargetingMode == DashTargetingMode.AimShieldCone && m_guardAbility != null)
			{
				if (ServerEffectManager.Get().GetEffect(caster, typeof(ValkyrieGuardEndingEffect)) is ValkyrieGuardEndingEffect valkyrieGuardEndingEffect)
				{
					casterHitResults.AddEffectForRemoval(valkyrieGuardEndingEffect);
				}
				ActorCover.CoverDirections coverFacing = GetCoverFacing(targets);
				casterHitResults.AddEffect(m_guardAbility.CreateGuardEffect(
					coverFacing,
					CoverIgnoreMinDist(),
					caster,
					GetShieldEffectInfo(),
					GetCoverDuration(),
					GetTechPointGainPerCoveredHit(),
					GetTechPointGainPerTooCloseForCoverHit(),
					0));
				if (TriggerCooldownOnGuardAbiity()) // m_triggerCooldownOnGuardAbiity in rogues
				{
					casterHitResults.AddMiscHitEvent(new MiscHitEventData_OverrideCooldown(
						m_guardAbilityActionType,
						m_guardAbility.GetModdedCooldown()));
				}
			}
			abilityResults.StoreActorHit(casterHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	//Added in rouges
	private void GetConeFacing(List<AbilityTarget> targets, ActorData caster, out Vector3 facing)
	{
		facing = targets[0].AimDirection;
		if (targets.Count > 1)
		{
			BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
			if (targetSquare != null)
			{
				facing = VectorUtils.GetDirectionAndOffsetToClosestSide(targetSquare, targets[1].FreePos, false, out _);
			}
		}
	}

	//Added in rouges
	private ActorCover.CoverDirections GetCoverFacing(List<AbilityTarget> targets)
	{
		if (m_dashTargetingMode != DashTargetingMode.AimShieldCone || targets.Count <= 1)
		{
			return ActorCover.CoverDirections.INVALID;
		}
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		if (targetSquare == null)
		{
			return ActorCover.CoverDirections.INVALID;
		}
		VectorUtils.GetDirectionAndOffsetToClosestSide(targetSquare, targets[1].FreePos, false, out Vector3 vector);
		Vector3 vec = targetSquare.ToVector3() + vector * 2f;
		return ActorCover.GetCoverDirection(targetSquare, Board.Get().GetSquareFromVec3(vec));
	}

	//Added in rouges
	public override void OnDodgedDamage(ActorData caster, int damageDodged)
	{
		caster.GetFreelancerStats().AddToValueOfStat(
			FreelancerStats.ValkyrieStats.DamageMitigatedFromWeakenedAndDodgedDashAoe,
			damageDodged);
	}

	//Added in rouges
	public override void OnCalculatedDamageReducedFromWeakenedGrantedByMyEffect(ActorData effectCaster, ActorData weakenedActor, int damageReduced)
	{
		effectCaster.GetFreelancerStats().AddToValueOfStat(
			FreelancerStats.ValkyrieStats.DamageMitigatedFromWeakenedAndDodgedDashAoe,
			damageReduced);
	}
#endif
}
