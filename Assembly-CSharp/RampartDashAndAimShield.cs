using System.Collections.Generic;
using UnityEngine;

public class RampartDashAndAimShield : Ability
{
	[Header("-- Charge Size")]
	public float m_chargeRadius = 2f;
	public float m_radiusAroundStart;
	public float m_radiusAroundEnd;
	public bool m_chargePenetrateLos;
	[Header("-- Hit Damage and Effect (in Charge)")]
	public int m_damageAmount = 10;
	public StandardEffectInfo m_enemyHitEffect;
	public int m_allyHealAmount;
	public StandardEffectInfo m_allyHitEffect;
	[Header("-- Shield Barrier (Barrier Data specified on Passive)")]
	public bool m_allowAimAtDiagonals;
	[Header("-- Cooldown by distance, [ Cooldown = Max(minCooldown, distance+cooldownModifierAdd) ], add modifier can be negative")]
	public bool m_setCooldownByDistance = true;
	public int m_minCooldown = 1;
	public int m_cooldownModifierAdd;
	[Header("-- Distance by Energy")]
	public bool m_useEnergyForMoveDistance;
	public int m_minEnergyToCast = 30;
	public int m_energyPerMove = 15;
	public bool m_useAllEnergyIfUsedForDistance = true;
	[Header("-- For Hitting In Front of Shield (damage is added to base damage)")]
	public bool m_hitInFrontOfShield;
	public float m_shieldFrontHitLength = 1.5f;
	public int m_damageForShieldFront;
	public StandardEffectInfo m_shieldFrontEnemyEffect;
	public bool m_shieldFrontLangthIgnoreLos;
	[Header("-- Sequences")]
	public GameObject m_applyShieldSequencePrefab;

	private bool m_snapToGrid = true;
	private Passive_Rampart m_passive;
	private AbilityMod_RampartDashAndAimShield m_abilityMod;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedShieldFrontEnemyEffect;
	private StandardEffectInfo m_cachedAllyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Intercept";
		}
		if (GetNumTargets() != 2)
		{
			Debug.LogError("RampartDashAndAimShield: Expected 2 entries in Target Data");
		}
		SetupTargeter();
		ResetTooltipAndTargetingNumbers();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		if (m_passive == null)
		{
			m_passive = GetComponent<PassiveData>().GetPassiveOfType(typeof(Passive_Rampart)) as Passive_Rampart;
		}
		float width = m_passive != null ? m_passive.GetShieldBarrierData().m_width : 3f;
		ClearTargeters();
		AbilityUtil_Targeter_ChargeAoE targeter1 = new AbilityUtil_Targeter_ChargeAoE(this, GetRadiusAroundStart(), GetRadiusAroundEnd(), GetChargeRadius(), 0, false, ChargePenetrateLos());
		targeter1.SetAffectedGroups(true, IncludeAllies(), false);
		Targeters.Add(targeter1);
		if (HitInFrontOfShield())
		{
			AbilityUtil_Targeter_RampartKnockbackBarrier targeter2 = new AbilityUtil_Targeter_RampartKnockbackBarrier(this, width, GetShieldFrontHitLength(), m_shieldFrontLangthIgnoreLos, 0f, KnockbackType.AwayFromSource, false, true, false);
			targeter2.SetUseMultiTargetUpdate(true);
			targeter2.SetTooltipSubjectType(AbilityTooltipSubject.Primary);
			Targeters.Add(targeter2);
		}
		else
		{
			AbilityUtil_Targeter_Barrier targeter2 = new AbilityUtil_Targeter_Barrier(this, width, m_snapToGrid, AllowAimAtDiagonals(), false);
			targeter2.SetUseMultiTargetUpdate(true);
			Targeters.Add(targeter2);
		}
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedShieldFrontEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_shieldFrontEnemyEffectMod.GetModifiedValue(m_shieldFrontEnemyEffect)
			: m_shieldFrontEnemyEffect;
		m_cachedAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
	}

	public bool IncludeAllies()
	{
		return GetAllyHitEffect().m_applyEffect || GetAllyHealAmount() > 0;
	}

	public float GetChargeRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chargeRadiusMod.GetModifiedValue(m_chargeRadius)
			: m_chargeRadius;
	}

	public float GetRadiusAroundStart()
	{
		return m_abilityMod != null
			? m_abilityMod.m_radiusAroundStartMod.GetModifiedValue(m_radiusAroundStart)
			: m_radiusAroundStart;
	}

	public float GetRadiusAroundEnd()
	{
		return m_abilityMod != null
			? m_abilityMod.m_radiusAroundEndMod.GetModifiedValue(m_radiusAroundEnd)
			: m_radiusAroundEnd;
	}

	public bool ChargePenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chargePenetrateLosMod.GetModifiedValue(m_chargePenetrateLos)
			: m_chargePenetrateLos;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public int GetAllyHealAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount)
			: m_allyHealAmount;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public bool AllowAimAtDiagonals()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allowAimAtDiagonalsMod.GetModifiedValue(m_allowAimAtDiagonals)
			: m_allowAimAtDiagonals;
	}

	public bool SetCooldownByDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_setCooldownByDistanceMod.GetModifiedValue(m_setCooldownByDistance)
			: m_setCooldownByDistance;
	}

	public int GetMinCooldown()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minCooldownMod.GetModifiedValue(m_minCooldown)
			: m_minCooldown;
	}

	public int GetCooldownModifierAdd()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownModifierAddMod.GetModifiedValue(m_cooldownModifierAdd)
			: m_cooldownModifierAdd;
	}

	public bool UseEnergyForMoveDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useEnergyForMoveDistanceMod.GetModifiedValue(m_useEnergyForMoveDistance)
			: m_useEnergyForMoveDistance;
	}

	public int GetMinEnergyToCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minEnergyToCastMod.GetModifiedValue(m_minEnergyToCast)
			: m_minEnergyToCast;
	}

	public int GetEnergyPerMove()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyPerMoveMod.GetModifiedValue(m_energyPerMove)
			: m_energyPerMove;
	}

	public bool UseAllEnergyIfUsedForDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useAllEnergyIfUsedForDistanceMod.GetModifiedValue(m_useAllEnergyIfUsedForDistance)
			: m_useAllEnergyIfUsedForDistance;
	}

	public bool HitInFrontOfShield()
	{
		return m_abilityMod != null
			? m_abilityMod.m_hitInFrontOfShieldMod.GetModifiedValue(m_hitInFrontOfShield)
			: m_hitInFrontOfShield;
	}

	public float GetShieldFrontHitLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldFrontHitLengthMod.GetModifiedValue(m_shieldFrontHitLength)
			: m_shieldFrontHitLength;
	}

	public int GetDamageForShieldFront()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageForShieldFrontMod.GetModifiedValue(m_damageForShieldFront)
			: m_damageForShieldFront;
	}

	public StandardEffectInfo GetShieldFrontEnemyEffect()
	{
		return m_cachedShieldFrontEnemyEffect != null
			? m_cachedShieldFrontEnemyEffect
			: m_shieldFrontEnemyEffect;
	}

	public float GetShieldFrontLaserWidth()
	{
		return m_passive.GetShieldBarrierData().m_width;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartDashAndAimShield abilityMod_RampartDashAndAimShield = modAsBase as AbilityMod_RampartDashAndAimShield;
		AddTokenInt(tokens, "DamageAmount", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "AllyHealAmount", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount)
			: m_allyHealAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AddTokenInt(tokens, "MinCooldown", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_minCooldownMod.GetModifiedValue(m_minCooldown)
			: m_minCooldown);
		AddTokenInt(tokens, "CooldownModifierAdd", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_cooldownModifierAddMod.GetModifiedValue(m_cooldownModifierAdd)
			: m_cooldownModifierAdd);
		AddTokenInt(tokens, "MinEnergyToCast", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_minEnergyToCastMod.GetModifiedValue(m_minEnergyToCast)
			: m_minEnergyToCast);
		AddTokenInt(tokens, "EnergyPerMove", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_energyPerMoveMod.GetModifiedValue(m_energyPerMove)
			: m_energyPerMove);
		AddTokenInt(tokens, "DamageForShieldFront", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_damageForShieldFrontMod.GetModifiedValue(m_damageForShieldFront)
			: m_damageForShieldFront);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_shieldFrontEnemyEffectMod.GetModifiedValue(m_shieldFrontEnemyEffect)
			: m_shieldFrontEnemyEffect, "ShieldFrontEnemyEffect", m_shieldFrontEnemyEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetAllyHealAmount());
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		if (m_passive != null)
		{
			m_passive.GetShieldBarrierData().ReportAbilityTooltipNumbers(ref numbers);
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes1 = Targeter.GetTooltipSubjectTypes(targetActor);
		List<AbilityTooltipSubject> tooltipSubjectTypes2 = Targeters[1].GetTooltipSubjectTypes(targetActor);
		dictionary[AbilityTooltipSymbol.Damage] = 0;
		if (tooltipSubjectTypes1 != null
			&& tooltipSubjectTypes1.Contains(AbilityTooltipSubject.Enemy)
			&& tooltipSubjectTypes1.Contains(AbilityTooltipSubject.Primary))
		{
			dictionary[AbilityTooltipSymbol.Damage] += GetDamageAmount();
		}
		if (tooltipSubjectTypes2 != null
			&& tooltipSubjectTypes2.Contains(AbilityTooltipSubject.Enemy)
			&& tooltipSubjectTypes2.Contains(AbilityTooltipSubject.Primary))
		{
			dictionary[AbilityTooltipSymbol.Damage] += GetDamageForShieldFront();
		}
		return dictionary;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool result = true;
		if (UseEnergyForMoveDistance())
		{
			result = caster.TechPoints >= GetMinEnergyToCast();
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null
			|| !targetSquare.IsValidForGameplay()
			|| targetSquare == caster.GetCurrentBoardSquare())
		{
			return false;
		}
		if (targetIndex == 0)
		{
			BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare);
			if (path == null)
			{
				return false;
			}
			if (!UseEnergyForMoveDistance() || GetEnergyPerMove() <= 0)
			{
				return true;
			}
			int maxLen = caster.TechPoints / GetEnergyPerMove();
			int len = 0;
			BoardSquarePathInfo it = path;
			while (it.next != null)
			{
				it = it.next;
				len++;
			}
			return len <= maxLen;
		}
		else
		{
			return Board.Get().GetSquare(currentTargets[0].GridPos) == targetSquare;
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartDashAndAimShield))
		{
			m_abilityMod = abilityMod as AbilityMod_RampartDashAndAimShield;
		}
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
