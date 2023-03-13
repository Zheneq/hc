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
		SetCachedFields();
		base.Targeters.Clear();
		if (m_dashTargetingMode == DashTargetingMode.Aoe)
		{
			base.Targeter = new AbilityUtil_Targeter_BattleMonkUltimate(this, GetAoeShape(), AoePenetratesLoS(), GetAoeShape(), AoePenetratesLoS(), true);
			bool affectsEnemies = IncludeEnemies();
			bool affectsAllies = IncludeAllies();
			bool affectsCaster = IncludeSelf();
			base.Targeter.SetAffectedGroups(affectsEnemies, affectsAllies, affectsCaster);
			return;
		}
		AbilityUtil_Targeter_Charge item = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false);
		AbilityUtil_Targeter_ValkyrieGuard abilityUtil_Targeter_ValkyrieGuard = new AbilityUtil_Targeter_ValkyrieGuard(this, 1f, true, false, false);
		abilityUtil_Targeter_ValkyrieGuard.SetConeParams(true, GetConeWidthAngle(), GetConeRadius(), false);
		abilityUtil_Targeter_ValkyrieGuard.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeSelf());
		abilityUtil_Targeter_ValkyrieGuard.SetUseMultiTargetUpdate(true);
		base.Targeters.Add(item);
		base.Targeters.Add(abilityUtil_Targeter_ValkyrieGuard);
	}

	private void SetCachedFields()
	{
		m_cachedShieldEffectInfo = (m_abilityMod ? m_abilityMod.m_shieldEffectInfoMod.GetModifiedValue(m_shieldEffectInfo) : m_shieldEffectInfo);
		m_cachedEnemyDebuff = (m_abilityMod ? m_abilityMod.m_enemyDebuffMod.GetModifiedValue(m_enemyDebuff) : m_enemyDebuff);
		m_cachedAllyBuff = (m_abilityMod ? m_abilityMod.m_allyBuffMod.GetModifiedValue(m_allyBuff) : m_allyBuff);
		m_cachedSelfBuff = (m_abilityMod ? m_abilityMod.m_selfBuffMod.GetModifiedValue(m_selfBuff) : m_selfBuff);
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
		if (m_dashTargetingMode == DashTargetingMode.Aoe)
		{
			return 1;
		}
		return Mathf.Min(GetTargetData().Length, 2);
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
		if (targetActor.GetTeam() == base.ActorData.GetTeam())
		{
			int absorb = GetAbsorb();
			dictionary[AbilityTooltipSymbol.Absorb] = absorb;
		}
		else
		{
			int damage = GetDamage();
			dictionary[AbilityTooltipSymbol.Damage] = damage;
		}
		return dictionary;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare square = Board.Get().GetSquare(target.GridPos);
		return targetIndex != 0 || (square != null && square.IsValidForGameplay() && square != caster.GetCurrentBoardSquare() && KnockbackUtils.BuildStraightLineChargePath(caster, square) != null);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return caster != null && caster.GetAbilityData() != null && !caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ValkyrieGuard));
	}

	public StandardEffectInfo GetShieldEffectInfo()
	{
		return (m_cachedShieldEffectInfo == null) ? m_shieldEffectInfo : m_cachedShieldEffectInfo;
	}

	public AbilityAreaShape GetAoeShape()
	{
		return (!m_abilityMod) ? m_aoeShape : m_abilityMod.m_aoeShapeMod.GetModifiedValue(m_aoeShape);
	}

	public bool AoePenetratesLoS()
	{
		if (!m_abilityMod)
		{
			return m_aoePenetratesLoS;
		}
		return m_abilityMod.m_aoePenetratesLoSMod.GetModifiedValue(m_aoePenetratesLoS);
	}

	public float GetConeWidthAngle()
	{
		if (!m_abilityMod)
		{
			return m_coneWidthAngle;
		}
		return m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle);
	}

	public float GetConeRadius()
	{
		if (!m_abilityMod)
		{
			return m_coneRadius;
		}
		return m_abilityMod.m_coneRadiusMod.GetModifiedValue(m_coneRadius);
	}

	public int GetCoverDuration()
	{
		return (!m_abilityMod) ? m_coverDuration : m_abilityMod.m_coverDurationMod.GetModifiedValue(m_coverDuration);
	}

	public bool CoverIgnoreMinDist()
	{
		if (!m_abilityMod)
		{
			return m_coverIgnoreMinDist;
		}
		return m_abilityMod.m_coverIgnoreMinDistMod.GetModifiedValue(m_coverIgnoreMinDist);
	}

	public bool TriggerCooldownOnGuardAbiity()
	{
		if (!m_abilityMod)
		{
			return m_triggerCooldownOnGuardAbiity;
		}
		return m_abilityMod.m_triggerCooldownOnGuardAbiityMod.GetModifiedValue(m_triggerCooldownOnGuardAbiity);
	}

	public int GetTechPointGainPerCoveredHit()
	{
		if (!m_abilityMod)
		{
			return m_techPointGainPerCoveredHit;
		}
		return m_abilityMod.m_techPointGainPerCoveredHitMod.GetModifiedValue(m_techPointGainPerCoveredHit);
	}

	public int GetTechPointGainPerTooCloseForCoverHit()
	{
		if (!m_abilityMod)
		{
			return m_techPointGainPerTooCloseForCoverHit;
		}
		return m_abilityMod.m_techPointGainPerTooCloseForCoverHitMod.GetModifiedValue(m_techPointGainPerTooCloseForCoverHit);
	}

	public int GetDamage()
	{
		if (!m_abilityMod)
		{
			return m_damage;
		}
		return m_abilityMod.m_damageMod.GetModifiedValue(m_damage);
	}

	public StandardEffectInfo GetEnemyDebuff()
	{
		return (m_cachedEnemyDebuff == null) ? m_enemyDebuff : m_cachedEnemyDebuff;
	}

	public int GetAbsorb()
	{
		if (!m_abilityMod)
		{
			return m_absorb;
		}
		return m_abilityMod.m_absorbMod.GetModifiedValue(m_absorb);
	}

	public StandardEffectInfo GetAllyBuff()
	{
		if (m_cachedAllyBuff == null)
		{
			return m_allyBuff;
		}
		return m_cachedAllyBuff;
	}

	public StandardEffectInfo GetSelfBuff()
	{
		return (m_cachedSelfBuff == null) ? m_selfBuff : m_cachedSelfBuff;
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
			m_abilityMod = (abilityMod as AbilityMod_ValkyrieDashAoE);
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
			BoardSquare square = Board.Get().GetSquare(targetsSoFar[0].GridPos);
			overridePos = square.ToVector3();
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

#if SERVER
	//Added in rouges
	internal override Vector3 GetFacingDirAfterMovement(ServerEvadeUtils.EvadeInfo evade)
	{
		GetConeFacing(evade.m_request.m_targets, evade.GetMover(), out Vector3 result);
		return result;
	}

	//Added in rouges
	private List<ActorData> FindHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		bool flag = IncludeEnemies();
		bool flag2 = IncludeAllies();
		bool flag3 = IncludeSelf();
		List<Team> list = new List<Team>();
		if (flag)
		{
			list.AddRange(caster.GetOtherTeams());
		}
		if (flag2)
		{
			list.Add(caster.GetTeam());
		}
		List<ActorData> list2 = null;
		if (m_dashTargetingMode == ValkyrieDashAoE.DashTargetingMode.Aoe)
		{
			list2 = AreaEffectUtils.GetActorsInShape(GetAoeShape(), targets[0], AoePenetratesLoS(), caster, list, nonActorTargetInfo);
		}
		else
		{
			Vector3 coneStart = Board.Get().GetSquare(targets[0].GridPos).ToVector3();
			Vector3 vec;
			GetConeFacing(targets, caster, out vec);
			list2 = AreaEffectUtils.GetActorsInCone(coneStart, VectorUtils.HorizontalAngle_Deg(vec), GetConeWidthAngle(), GetConeRadius(), 0f, false, caster, list, nonActorTargetInfo);
			list2.Remove(caster);
		}
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref list2);
		if (flag3 && !list2.Contains(caster))
		{
			list2.Add(caster);
		}
		return list2;
	}

	//Added in rouges
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_dashTargetingMode == DashTargetingMode.Aoe)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetAoeShape(), targets[0]);
			return new ServerClientUtils.SequenceStartData(m_castSequencePrefab, centerOfShape, additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource, null);
		}
		Vector3 vec;
		GetConeFacing(targets, caster, out vec);
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		BlasterStretchConeSequence.ExtraParams extraParams = new BlasterStretchConeSequence.ExtraParams();
		extraParams.angleInDegrees = GetConeWidthAngle();
		extraParams.lengthInSquares = GetConeRadius();
		extraParams.forwardAngle = VectorUtils.HorizontalAngle_Deg(vec);
		return new ServerClientUtils.SequenceStartData(m_castSequencePrefab, square, additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource, extraParams.ToArray());
	}

	//Added in rouges
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> list = FindHitActors(targets, caster, nonActorTargetInfo);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetAoeShape(), targets[0]);
		foreach (ActorData actorData in list)
		{
			ActorHitParameters hitParams = new ActorHitParameters(actorData, centerOfShape);
			if (actorData.GetTeam() != caster.GetTeam())
			{
				int damage = GetDamage();
				abilityResults.StoreActorHit(new ActorHitResults(damage, HitActionType.Damage, m_enemyDebuff, hitParams));
			}
			else if (actorData != caster)
			{
				int absorb = GetAbsorb();
				GetAllyBuff().m_effectData.m_absorbAmount = absorb;
				abilityResults.StoreActorHit(new ActorHitResults(GetAllyBuff(), hitParams));
			}
		}
		if (GetSelfBuff().m_applyEffect || m_dashTargetingMode == DashTargetingMode.AimShieldCone)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, centerOfShape));
			actorHitResults.AddStandardEffectInfo(GetSelfBuff());
			if (m_dashTargetingMode == DashTargetingMode.AimShieldCone && m_guardAbility != null)
			{
				ValkyrieGuardEndingEffect valkyrieGuardEndingEffect = ServerEffectManager.Get().GetEffect(caster, typeof(ValkyrieGuardEndingEffect)) as ValkyrieGuardEndingEffect;
				if (valkyrieGuardEndingEffect != null)
				{
					actorHitResults.AddEffectForRemoval(valkyrieGuardEndingEffect);
				}
				ActorCover.CoverDirections coverFacing = GetCoverFacing(targets);
				valkyrieGuardEndingEffect = m_guardAbility.CreateGuardEffect(
					coverFacing, 
					CoverIgnoreMinDist(), 
					caster, 
					GetShieldEffectInfo(), 
					GetCoverDuration(), 
					GetTechPointGainPerCoveredHit(), 
					GetTechPointGainPerTooCloseForCoverHit(), 
					0);
				actorHitResults.AddEffect(valkyrieGuardEndingEffect);
				if (m_triggerCooldownOnGuardAbiity)
				{
					int moddedCooldown = m_guardAbility.GetModdedCooldown();
					MiscHitEventData_OverrideCooldown hitEvent = new MiscHitEventData_OverrideCooldown(m_guardAbilityActionType, moddedCooldown);
					actorHitResults.AddMiscHitEvent(hitEvent);
				}
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	//Added in rouges
	private void GetConeFacing(List<AbilityTarget> targets, ActorData caster, out Vector3 facing)
	{
		facing = targets[0].AimDirection;
		if (targets.Count > 1)
		{
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			if (square != null)
			{
				facing = VectorUtils.GetDirectionAndOffsetToClosestSide(square, targets[1].FreePos, false, out Vector3 vector);
			}
		}
	}

	//Added in rouges
	private ActorCover.CoverDirections GetCoverFacing(List<AbilityTarget> targets)
	{
		ActorCover.CoverDirections result = ActorCover.CoverDirections.INVALID;
		if (m_dashTargetingMode == DashTargetingMode.AimShieldCone && targets.Count > 1)
		{
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			if (square != null)
			{
				VectorUtils.GetDirectionAndOffsetToClosestSide(square, targets[1].FreePos, false, out Vector3 vector);
				Vector3 vec = square.ToVector3() + vector * 2f;
				result = ActorCover.GetCoverDirection(square, Board.Get().GetSquareFromVec3(vec));
			}
		}
		return result;
	}

	//Added in rouges
	public override void OnDodgedDamage(ActorData caster, int damageDodged)
	{
		caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.ValkyrieStats.DamageMitigatedFromWeakenedAndDodgedDashAoe, damageDodged);
	}

	//Added in rouges
	public override void OnCalculatedDamageReducedFromWeakenedGrantedByMyEffect(ActorData effectCaster, ActorData weakenedActor, int damageReduced)
	{
		effectCaster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.ValkyrieStats.DamageMitigatedFromWeakenedAndDodgedDashAoe, damageReduced);
	}
#endif
}
