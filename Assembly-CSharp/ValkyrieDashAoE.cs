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
		       && !caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ValkyrieGuard));
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
		if (targetIndex != 1)
		{
			return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
		}
		BoardSquare targetSquare = Board.Get().GetSquare(targetsSoFar[0].GridPos);
		overridePos = targetSquare.ToVector3();
		return true;
	}
}
