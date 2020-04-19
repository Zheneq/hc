using System;
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
	public int m_damageAmount = 0xA;

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

	public int m_minEnergyToCast = 0x1E;

	public int m_energyPerMove = 0xF;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Intercept";
		}
		if (base.GetNumTargets() != 2)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.Start()).MethodHandle;
			}
			Debug.LogError("RampartDashAndAimShield: Expected 2 entries in Target Data");
		}
		this.SetupTargeter();
		base.ResetTooltipAndTargetingNumbers();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		if (this.m_passive == null)
		{
			this.m_passive = (base.GetComponent<PassiveData>().GetPassiveOfType(typeof(Passive_Rampart)) as Passive_Rampart);
		}
		float width = (!(this.m_passive != null)) ? 3f : this.m_passive.GetShieldBarrierData().m_width;
		base.ClearTargeters();
		AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, this.GetRadiusAroundStart(), this.GetRadiusAroundEnd(), this.GetChargeRadius(), 0, false, this.ChargePenetrateLos());
		abilityUtil_Targeter_ChargeAoE.SetAffectedGroups(true, this.IncludeAllies(), false);
		base.Targeters.Add(abilityUtil_Targeter_ChargeAoE);
		if (this.HitInFrontOfShield())
		{
			AbilityUtil_Targeter_RampartKnockbackBarrier abilityUtil_Targeter_RampartKnockbackBarrier = new AbilityUtil_Targeter_RampartKnockbackBarrier(this, width, this.GetShieldFrontHitLength(), this.m_shieldFrontLangthIgnoreLos, 0f, KnockbackType.AwayFromSource, false, true, false);
			abilityUtil_Targeter_RampartKnockbackBarrier.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_RampartKnockbackBarrier.SetTooltipSubjectType(AbilityTooltipSubject.Primary);
			base.Targeters.Add(abilityUtil_Targeter_RampartKnockbackBarrier);
		}
		else
		{
			AbilityUtil_Targeter_Barrier abilityUtil_Targeter_Barrier = new AbilityUtil_Targeter_Barrier(this, width, this.m_snapToGrid, this.AllowAimAtDiagonals(), false);
			abilityUtil_Targeter_Barrier.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_Barrier);
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
		StandardEffectInfo cachedEnemyHitEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.SetCachedFields()).MethodHandle;
			}
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		StandardEffectInfo cachedShieldFrontEnemyEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedShieldFrontEnemyEffect = this.m_abilityMod.m_shieldFrontEnemyEffectMod.GetModifiedValue(this.m_shieldFrontEnemyEffect);
		}
		else
		{
			cachedShieldFrontEnemyEffect = this.m_shieldFrontEnemyEffect;
		}
		this.m_cachedShieldFrontEnemyEffect = cachedShieldFrontEnemyEffect;
		StandardEffectInfo cachedAllyHitEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedAllyHitEffect = this.m_abilityMod.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = this.m_allyHitEffect;
		}
		this.m_cachedAllyHitEffect = cachedAllyHitEffect;
	}

	public bool IncludeAllies()
	{
		return this.GetAllyHitEffect().m_applyEffect || this.GetAllyHealAmount() > 0;
	}

	public float GetChargeRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetChargeRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_chargeRadiusMod.GetModifiedValue(this.m_chargeRadius);
		}
		else
		{
			result = this.m_chargeRadius;
		}
		return result;
	}

	public float GetRadiusAroundStart()
	{
		return (!this.m_abilityMod) ? this.m_radiusAroundStart : this.m_abilityMod.m_radiusAroundStartMod.GetModifiedValue(this.m_radiusAroundStart);
	}

	public float GetRadiusAroundEnd()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetRadiusAroundEnd()).MethodHandle;
			}
			result = this.m_abilityMod.m_radiusAroundEndMod.GetModifiedValue(this.m_radiusAroundEnd);
		}
		else
		{
			result = this.m_radiusAroundEnd;
		}
		return result;
	}

	public bool ChargePenetrateLos()
	{
		bool result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.ChargePenetrateLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_chargePenetrateLosMod.GetModifiedValue(this.m_chargePenetrateLos);
		}
		else
		{
			result = this.m_chargePenetrateLos;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			result = this.m_damageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffect != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public int GetAllyHealAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetAllyHealAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_allyHealAmountMod.GetModifiedValue(this.m_allyHealAmount);
		}
		else
		{
			result = this.m_allyHealAmount;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyHitEffect != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetAllyHitEffect()).MethodHandle;
			}
			result = this.m_cachedAllyHitEffect;
		}
		else
		{
			result = this.m_allyHitEffect;
		}
		return result;
	}

	public bool AllowAimAtDiagonals()
	{
		bool result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.AllowAimAtDiagonals()).MethodHandle;
			}
			result = this.m_abilityMod.m_allowAimAtDiagonalsMod.GetModifiedValue(this.m_allowAimAtDiagonals);
		}
		else
		{
			result = this.m_allowAimAtDiagonals;
		}
		return result;
	}

	public bool SetCooldownByDistance()
	{
		return (!this.m_abilityMod) ? this.m_setCooldownByDistance : this.m_abilityMod.m_setCooldownByDistanceMod.GetModifiedValue(this.m_setCooldownByDistance);
	}

	public int GetMinCooldown()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetMinCooldown()).MethodHandle;
			}
			result = this.m_abilityMod.m_minCooldownMod.GetModifiedValue(this.m_minCooldown);
		}
		else
		{
			result = this.m_minCooldown;
		}
		return result;
	}

	public int GetCooldownModifierAdd()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetCooldownModifierAdd()).MethodHandle;
			}
			result = this.m_abilityMod.m_cooldownModifierAddMod.GetModifiedValue(this.m_cooldownModifierAdd);
		}
		else
		{
			result = this.m_cooldownModifierAdd;
		}
		return result;
	}

	public bool UseEnergyForMoveDistance()
	{
		bool result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.UseEnergyForMoveDistance()).MethodHandle;
			}
			result = this.m_abilityMod.m_useEnergyForMoveDistanceMod.GetModifiedValue(this.m_useEnergyForMoveDistance);
		}
		else
		{
			result = this.m_useEnergyForMoveDistance;
		}
		return result;
	}

	public int GetMinEnergyToCast()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetMinEnergyToCast()).MethodHandle;
			}
			result = this.m_abilityMod.m_minEnergyToCastMod.GetModifiedValue(this.m_minEnergyToCast);
		}
		else
		{
			result = this.m_minEnergyToCast;
		}
		return result;
	}

	public int GetEnergyPerMove()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetEnergyPerMove()).MethodHandle;
			}
			result = this.m_abilityMod.m_energyPerMoveMod.GetModifiedValue(this.m_energyPerMove);
		}
		else
		{
			result = this.m_energyPerMove;
		}
		return result;
	}

	public bool UseAllEnergyIfUsedForDistance()
	{
		return (!this.m_abilityMod) ? this.m_useAllEnergyIfUsedForDistance : this.m_abilityMod.m_useAllEnergyIfUsedForDistanceMod.GetModifiedValue(this.m_useAllEnergyIfUsedForDistance);
	}

	public bool HitInFrontOfShield()
	{
		bool result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.HitInFrontOfShield()).MethodHandle;
			}
			result = this.m_abilityMod.m_hitInFrontOfShieldMod.GetModifiedValue(this.m_hitInFrontOfShield);
		}
		else
		{
			result = this.m_hitInFrontOfShield;
		}
		return result;
	}

	public float GetShieldFrontHitLength()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetShieldFrontHitLength()).MethodHandle;
			}
			result = this.m_abilityMod.m_shieldFrontHitLengthMod.GetModifiedValue(this.m_shieldFrontHitLength);
		}
		else
		{
			result = this.m_shieldFrontHitLength;
		}
		return result;
	}

	public int GetDamageForShieldFront()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetDamageForShieldFront()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageForShieldFrontMod.GetModifiedValue(this.m_damageForShieldFront);
		}
		else
		{
			result = this.m_damageForShieldFront;
		}
		return result;
	}

	public StandardEffectInfo GetShieldFrontEnemyEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedShieldFrontEnemyEffect != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetShieldFrontEnemyEffect()).MethodHandle;
			}
			result = this.m_cachedShieldFrontEnemyEffect;
		}
		else
		{
			result = this.m_shieldFrontEnemyEffect;
		}
		return result;
	}

	public float GetShieldFrontLaserWidth()
	{
		return this.m_passive.GetShieldBarrierData().m_width;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartDashAndAimShield abilityMod_RampartDashAndAimShield = modAsBase as AbilityMod_RampartDashAndAimShield;
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_RampartDashAndAimShield) ? this.m_damageAmount : abilityMod_RampartDashAndAimShield.m_damageAmountMod.GetModifiedValue(this.m_damageAmount), false);
		StandardEffectInfo effectInfo;
		if (abilityMod_RampartDashAndAimShield)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			effectInfo = abilityMod_RampartDashAndAimShield.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", this.m_enemyHitEffect, true);
		string name = "AllyHealAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_RampartDashAndAimShield)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			val = abilityMod_RampartDashAndAimShield.m_allyHealAmountMod.GetModifiedValue(this.m_allyHealAmount);
		}
		else
		{
			val = this.m_allyHealAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_RampartDashAndAimShield)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo2 = abilityMod_RampartDashAndAimShield.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			effectInfo2 = this.m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "AllyHitEffect", this.m_allyHitEffect, true);
		string name2 = "MinCooldown";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_RampartDashAndAimShield)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = abilityMod_RampartDashAndAimShield.m_minCooldownMod.GetModifiedValue(this.m_minCooldown);
		}
		else
		{
			val2 = this.m_minCooldown;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "CooldownModifierAdd";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_RampartDashAndAimShield)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			val3 = abilityMod_RampartDashAndAimShield.m_cooldownModifierAddMod.GetModifiedValue(this.m_cooldownModifierAdd);
		}
		else
		{
			val3 = this.m_cooldownModifierAdd;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "MinEnergyToCast";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_RampartDashAndAimShield)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			val4 = abilityMod_RampartDashAndAimShield.m_minEnergyToCastMod.GetModifiedValue(this.m_minEnergyToCast);
		}
		else
		{
			val4 = this.m_minEnergyToCast;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		string name5 = "EnergyPerMove";
		string empty5 = string.Empty;
		int val5;
		if (abilityMod_RampartDashAndAimShield)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			val5 = abilityMod_RampartDashAndAimShield.m_energyPerMoveMod.GetModifiedValue(this.m_energyPerMove);
		}
		else
		{
			val5 = this.m_energyPerMove;
		}
		base.AddTokenInt(tokens, name5, empty5, val5, false);
		base.AddTokenInt(tokens, "DamageForShieldFront", string.Empty, (!abilityMod_RampartDashAndAimShield) ? this.m_damageForShieldFront : abilityMod_RampartDashAndAimShield.m_damageForShieldFrontMod.GetModifiedValue(this.m_damageForShieldFront), false);
		StandardEffectInfo effectInfo3;
		if (abilityMod_RampartDashAndAimShield)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo3 = abilityMod_RampartDashAndAimShield.m_shieldFrontEnemyEffectMod.GetModifiedValue(this.m_shieldFrontEnemyEffect);
		}
		else
		{
			effectInfo3 = this.m_shieldFrontEnemyEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "ShieldFrontEnemyEffect", this.m_shieldFrontEnemyEffect, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDamageAmount());
		this.GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetAllyHealAmount());
		this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		if (this.m_passive != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			this.m_passive.GetShieldBarrierData().ReportAbilityTooltipNumbers(ref result);
		}
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		List<AbilityTooltipSubject> tooltipSubjectTypes2 = base.Targeters[1].GetTooltipSubjectTypes(targetActor);
		dictionary[AbilityTooltipSymbol.Damage] = 0;
		if (tooltipSubjectTypes != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy) && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				Dictionary<AbilityTooltipSymbol, int> dictionary2;
				(dictionary2 = dictionary)[AbilityTooltipSymbol.Damage] = dictionary2[AbilityTooltipSymbol.Damage] + this.GetDamageAmount();
			}
		}
		if (tooltipSubjectTypes2 != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (tooltipSubjectTypes2.Contains(AbilityTooltipSubject.Enemy))
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (tooltipSubjectTypes2.Contains(AbilityTooltipSubject.Primary))
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					Dictionary<AbilityTooltipSymbol, int> dictionary2;
					(dictionary2 = dictionary)[AbilityTooltipSymbol.Damage] = dictionary2[AbilityTooltipSymbol.Damage] + this.GetDamageForShieldFront();
				}
			}
		}
		return dictionary;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool result = true;
		if (this.UseEnergyForMoveDistance())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			result = (caster.TechPoints >= this.GetMinEnergyToCast());
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (!(boardSquare == null))
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (boardSquare.\u0016())
			{
				if (!(boardSquare == caster.\u0012()))
				{
					bool result = false;
					if (targetIndex == 0)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(caster, boardSquare);
						if (boardSquarePathInfo != null)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							result = true;
							if (this.UseEnergyForMoveDistance() && this.GetEnergyPerMove() > 0)
							{
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								int num = caster.TechPoints / this.GetEnergyPerMove();
								int num2 = 0;
								BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
								while (boardSquarePathInfo2.next != null)
								{
									boardSquarePathInfo2 = boardSquarePathInfo2.next;
									num2++;
								}
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								result = (num2 <= num);
							}
						}
					}
					else
					{
						BoardSquare x = Board.\u000E().\u000E(currentTargets[0].GridPos);
						result = (x == boardSquare);
					}
					return result;
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartDashAndAimShield))
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartDashAndAimShield.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_RampartDashAndAimShield);
		}
		this.SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
