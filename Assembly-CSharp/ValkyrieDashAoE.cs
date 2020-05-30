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

	[Separator("Dash Target Mode", true)]
	public DashTargetingMode m_dashTargetingMode;

	[Header("-- Targeting")]
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;

	public bool m_aoePenetratesLoS;

	[Separator("Aim Shield and Cone", true)]
	public float m_coneWidthAngle = 110f;

	public float m_coneRadius = 2.5f;

	public int m_coverDuration = 1;

	[Header("-- Cover Ignore Min Dist?")]
	public bool m_coverIgnoreMinDist = true;

	[Header("-- Whether to put guard ability on cooldown")]
	public bool m_triggerCooldownOnGuardAbiity;

	[Separator("Enemy hits", true)]
	public int m_damage = 20;

	public StandardEffectInfo m_enemyDebuff;

	[Separator("Ally & self hits", true)]
	public int m_absorb = 20;

	public AbilityCooldownChangeInfo m_cooldownReductionIfDamagedThisTurn;

	public StandardEffectInfo m_allyBuff;

	public StandardEffectInfo m_selfBuff;

	[Separator("Sequences", true)]
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
		base.Targeters.Clear();
		if (m_dashTargetingMode == DashTargetingMode.Aoe)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					base.Targeter = new AbilityUtil_Targeter_BattleMonkUltimate(this, GetAoeShape(), AoePenetratesLoS(), GetAoeShape(), AoePenetratesLoS(), true);
					bool affectsEnemies = IncludeEnemies();
					bool affectsAllies = IncludeAllies();
					bool affectsCaster = IncludeSelf();
					base.Targeter.SetAffectedGroups(affectsEnemies, affectsAllies, affectsCaster);
					return;
				}
				}
			}
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
		StandardEffectInfo cachedShieldEffectInfo;
		if ((bool)m_abilityMod)
		{
			cachedShieldEffectInfo = m_abilityMod.m_shieldEffectInfoMod.GetModifiedValue(m_shieldEffectInfo);
		}
		else
		{
			cachedShieldEffectInfo = m_shieldEffectInfo;
		}
		m_cachedShieldEffectInfo = cachedShieldEffectInfo;
		StandardEffectInfo cachedEnemyDebuff;
		if ((bool)m_abilityMod)
		{
			cachedEnemyDebuff = m_abilityMod.m_enemyDebuffMod.GetModifiedValue(m_enemyDebuff);
		}
		else
		{
			cachedEnemyDebuff = m_enemyDebuff;
		}
		m_cachedEnemyDebuff = cachedEnemyDebuff;
		StandardEffectInfo cachedAllyBuff;
		if ((bool)m_abilityMod)
		{
			cachedAllyBuff = m_abilityMod.m_allyBuffMod.GetModifiedValue(m_allyBuff);
		}
		else
		{
			cachedAllyBuff = m_allyBuff;
		}
		m_cachedAllyBuff = cachedAllyBuff;
		StandardEffectInfo cachedSelfBuff;
		if ((bool)m_abilityMod)
		{
			cachedSelfBuff = m_abilityMod.m_selfBuffMod.GetModifiedValue(m_selfBuff);
		}
		else
		{
			cachedSelfBuff = m_selfBuff;
		}
		m_cachedSelfBuff = cachedSelfBuff;
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return 1;
				}
			}
		}
		return Mathf.Min(GetTargetData().Length, 2);
	}

	public bool IncludeEnemies()
	{
		int result;
		if (GetDamage() <= 0)
		{
			result = (m_enemyDebuff.m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
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
		if (targetActor.GetTeam() != base.ActorData.GetTeam())
		{
			int num = dictionary[AbilityTooltipSymbol.Damage] = GetDamage();
		}
		else
		{
			int num2 = dictionary[AbilityTooltipSymbol.Absorb] = GetAbsorb();
		}
		return dictionary;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (targetIndex == 0)
		{
			if (boardSquareSafe != null)
			{
				if (boardSquareSafe.IsBaselineHeight())
				{
					if (boardSquareSafe != caster.GetCurrentBoardSquare())
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe) != null;
							}
						}
					}
				}
			}
			return false;
		}
		return true;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		int result;
		if (caster != null && caster.GetAbilityData() != null)
		{
			result = ((!caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ValkyrieGuard))) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
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
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_aoePenetratesLoSMod.GetModifiedValue(m_aoePenetratesLoS);
		}
		else
		{
			result = m_aoePenetratesLoS;
		}
		return result;
	}

	public float GetConeWidthAngle()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle);
		}
		else
		{
			result = m_coneWidthAngle;
		}
		return result;
	}

	public float GetConeRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneRadiusMod.GetModifiedValue(m_coneRadius);
		}
		else
		{
			result = m_coneRadius;
		}
		return result;
	}

	public int GetCoverDuration()
	{
		return (!m_abilityMod) ? m_coverDuration : m_abilityMod.m_coverDurationMod.GetModifiedValue(m_coverDuration);
	}

	public bool CoverIgnoreMinDist()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coverIgnoreMinDistMod.GetModifiedValue(m_coverIgnoreMinDist);
		}
		else
		{
			result = m_coverIgnoreMinDist;
		}
		return result;
	}

	public bool TriggerCooldownOnGuardAbiity()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_triggerCooldownOnGuardAbiityMod.GetModifiedValue(m_triggerCooldownOnGuardAbiity);
		}
		else
		{
			result = m_triggerCooldownOnGuardAbiity;
		}
		return result;
	}

	public int GetTechPointGainPerCoveredHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_techPointGainPerCoveredHitMod.GetModifiedValue(m_techPointGainPerCoveredHit);
		}
		else
		{
			result = m_techPointGainPerCoveredHit;
		}
		return result;
	}

	public int GetTechPointGainPerTooCloseForCoverHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_techPointGainPerTooCloseForCoverHitMod.GetModifiedValue(m_techPointGainPerTooCloseForCoverHit);
		}
		else
		{
			result = m_techPointGainPerTooCloseForCoverHit;
		}
		return result;
	}

	public int GetDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damage);
		}
		else
		{
			result = m_damage;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyDebuff()
	{
		return (m_cachedEnemyDebuff == null) ? m_enemyDebuff : m_cachedEnemyDebuff;
	}

	public int GetAbsorb()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_absorbMod.GetModifiedValue(m_absorb);
		}
		else
		{
			result = m_absorb;
		}
		return result;
	}

	public StandardEffectInfo GetAllyBuff()
	{
		StandardEffectInfo result;
		if (m_cachedAllyBuff != null)
		{
			result = m_cachedAllyBuff;
		}
		else
		{
			result = m_allyBuff;
		}
		return result;
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
		if (abilityMod.GetType() != typeof(AbilityMod_ValkyrieDashAoE))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ValkyrieDashAoE);
			SetupTargeter();
			return;
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					min = 1f;
					max = 1f;
					return true;
				}
			}
		}
		return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	public override bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		if (targetIndex == 1)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					BoardSquare boardSquareSafe = Board.Get().GetSquare(targetsSoFar[0].GridPos);
					overridePos = boardSquareSafe.ToVector3();
					return true;
				}
				}
			}
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}
}
