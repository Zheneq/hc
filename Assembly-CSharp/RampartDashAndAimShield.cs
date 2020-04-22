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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			m_passive = (GetComponent<PassiveData>().GetPassiveOfType(typeof(Passive_Rampart)) as Passive_Rampart);
		}
		float width = (!(m_passive != null)) ? 3f : m_passive.GetShieldBarrierData().m_width;
		ClearTargeters();
		AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, GetRadiusAroundStart(), GetRadiusAroundEnd(), GetChargeRadius(), 0, false, ChargePenetrateLos());
		abilityUtil_Targeter_ChargeAoE.SetAffectedGroups(true, IncludeAllies(), false);
		base.Targeters.Add(abilityUtil_Targeter_ChargeAoE);
		if (HitInFrontOfShield())
		{
			AbilityUtil_Targeter_RampartKnockbackBarrier abilityUtil_Targeter_RampartKnockbackBarrier = new AbilityUtil_Targeter_RampartKnockbackBarrier(this, width, GetShieldFrontHitLength(), m_shieldFrontLangthIgnoreLos, 0f, KnockbackType.AwayFromSource, false, true, false);
			abilityUtil_Targeter_RampartKnockbackBarrier.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_RampartKnockbackBarrier.SetTooltipSubjectType(AbilityTooltipSubject.Primary);
			base.Targeters.Add(abilityUtil_Targeter_RampartKnockbackBarrier);
		}
		else
		{
			AbilityUtil_Targeter_Barrier abilityUtil_Targeter_Barrier = new AbilityUtil_Targeter_Barrier(this, width, m_snapToGrid, AllowAimAtDiagonals(), false);
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
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		StandardEffectInfo cachedShieldFrontEnemyEffect;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedShieldFrontEnemyEffect = m_abilityMod.m_shieldFrontEnemyEffectMod.GetModifiedValue(m_shieldFrontEnemyEffect);
		}
		else
		{
			cachedShieldFrontEnemyEffect = m_shieldFrontEnemyEffect;
		}
		m_cachedShieldFrontEnemyEffect = cachedShieldFrontEnemyEffect;
		StandardEffectInfo cachedAllyHitEffect;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedAllyHitEffect = m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = m_allyHitEffect;
		}
		m_cachedAllyHitEffect = cachedAllyHitEffect;
	}

	public bool IncludeAllies()
	{
		return GetAllyHitEffect().m_applyEffect || GetAllyHealAmount() > 0;
	}

	public float GetChargeRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_chargeRadiusMod.GetModifiedValue(m_chargeRadius);
		}
		else
		{
			result = m_chargeRadius;
		}
		return result;
	}

	public float GetRadiusAroundStart()
	{
		return (!m_abilityMod) ? m_radiusAroundStart : m_abilityMod.m_radiusAroundStartMod.GetModifiedValue(m_radiusAroundStart);
	}

	public float GetRadiusAroundEnd()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_radiusAroundEndMod.GetModifiedValue(m_radiusAroundEnd);
		}
		else
		{
			result = m_radiusAroundEnd;
		}
		return result;
	}

	public bool ChargePenetrateLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_chargePenetrateLosMod.GetModifiedValue(m_chargePenetrateLos);
		}
		else
		{
			result = m_chargePenetrateLos;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			result = m_damageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public int GetAllyHealAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount);
		}
		else
		{
			result = m_allyHealAmount;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllyHitEffect != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedAllyHitEffect;
		}
		else
		{
			result = m_allyHitEffect;
		}
		return result;
	}

	public bool AllowAimAtDiagonals()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_allowAimAtDiagonalsMod.GetModifiedValue(m_allowAimAtDiagonals);
		}
		else
		{
			result = m_allowAimAtDiagonals;
		}
		return result;
	}

	public bool SetCooldownByDistance()
	{
		return (!m_abilityMod) ? m_setCooldownByDistance : m_abilityMod.m_setCooldownByDistanceMod.GetModifiedValue(m_setCooldownByDistance);
	}

	public int GetMinCooldown()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_minCooldownMod.GetModifiedValue(m_minCooldown);
		}
		else
		{
			result = m_minCooldown;
		}
		return result;
	}

	public int GetCooldownModifierAdd()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_cooldownModifierAddMod.GetModifiedValue(m_cooldownModifierAdd);
		}
		else
		{
			result = m_cooldownModifierAdd;
		}
		return result;
	}

	public bool UseEnergyForMoveDistance()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_useEnergyForMoveDistanceMod.GetModifiedValue(m_useEnergyForMoveDistance);
		}
		else
		{
			result = m_useEnergyForMoveDistance;
		}
		return result;
	}

	public int GetMinEnergyToCast()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_minEnergyToCastMod.GetModifiedValue(m_minEnergyToCast);
		}
		else
		{
			result = m_minEnergyToCast;
		}
		return result;
	}

	public int GetEnergyPerMove()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_energyPerMoveMod.GetModifiedValue(m_energyPerMove);
		}
		else
		{
			result = m_energyPerMove;
		}
		return result;
	}

	public bool UseAllEnergyIfUsedForDistance()
	{
		return (!m_abilityMod) ? m_useAllEnergyIfUsedForDistance : m_abilityMod.m_useAllEnergyIfUsedForDistanceMod.GetModifiedValue(m_useAllEnergyIfUsedForDistance);
	}

	public bool HitInFrontOfShield()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_hitInFrontOfShieldMod.GetModifiedValue(m_hitInFrontOfShield);
		}
		else
		{
			result = m_hitInFrontOfShield;
		}
		return result;
	}

	public float GetShieldFrontHitLength()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_shieldFrontHitLengthMod.GetModifiedValue(m_shieldFrontHitLength);
		}
		else
		{
			result = m_shieldFrontHitLength;
		}
		return result;
	}

	public int GetDamageForShieldFront()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_damageForShieldFrontMod.GetModifiedValue(m_damageForShieldFront);
		}
		else
		{
			result = m_damageForShieldFront;
		}
		return result;
	}

	public StandardEffectInfo GetShieldFrontEnemyEffect()
	{
		StandardEffectInfo result;
		if (m_cachedShieldFrontEnemyEffect != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedShieldFrontEnemyEffect;
		}
		else
		{
			result = m_shieldFrontEnemyEffect;
		}
		return result;
	}

	public float GetShieldFrontLaserWidth()
	{
		return m_passive.GetShieldBarrierData().m_width;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartDashAndAimShield abilityMod_RampartDashAndAimShield = modAsBase as AbilityMod_RampartDashAndAimShield;
		AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_RampartDashAndAimShield) ? m_damageAmount : abilityMod_RampartDashAndAimShield.m_damageAmountMod.GetModifiedValue(m_damageAmount));
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_RampartDashAndAimShield)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			effectInfo = abilityMod_RampartDashAndAimShield.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			effectInfo = m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", m_enemyHitEffect);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_RampartDashAndAimShield)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			val = abilityMod_RampartDashAndAimShield.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount);
		}
		else
		{
			val = m_allyHealAmount;
		}
		AddTokenInt(tokens, "AllyHealAmount", empty, val);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_RampartDashAndAimShield)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo2 = abilityMod_RampartDashAndAimShield.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			effectInfo2 = m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "AllyHitEffect", m_allyHitEffect);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_RampartDashAndAimShield)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = abilityMod_RampartDashAndAimShield.m_minCooldownMod.GetModifiedValue(m_minCooldown);
		}
		else
		{
			val2 = m_minCooldown;
		}
		AddTokenInt(tokens, "MinCooldown", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_RampartDashAndAimShield)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			val3 = abilityMod_RampartDashAndAimShield.m_cooldownModifierAddMod.GetModifiedValue(m_cooldownModifierAdd);
		}
		else
		{
			val3 = m_cooldownModifierAdd;
		}
		AddTokenInt(tokens, "CooldownModifierAdd", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_RampartDashAndAimShield)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			val4 = abilityMod_RampartDashAndAimShield.m_minEnergyToCastMod.GetModifiedValue(m_minEnergyToCast);
		}
		else
		{
			val4 = m_minEnergyToCast;
		}
		AddTokenInt(tokens, "MinEnergyToCast", empty4, val4);
		string empty5 = string.Empty;
		int val5;
		if ((bool)abilityMod_RampartDashAndAimShield)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			val5 = abilityMod_RampartDashAndAimShield.m_energyPerMoveMod.GetModifiedValue(m_energyPerMove);
		}
		else
		{
			val5 = m_energyPerMove;
		}
		AddTokenInt(tokens, "EnergyPerMove", empty5, val5);
		AddTokenInt(tokens, "DamageForShieldFront", string.Empty, (!abilityMod_RampartDashAndAimShield) ? m_damageForShieldFront : abilityMod_RampartDashAndAimShield.m_damageForShieldFrontMod.GetModifiedValue(m_damageForShieldFront));
		StandardEffectInfo effectInfo3;
		if ((bool)abilityMod_RampartDashAndAimShield)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo3 = abilityMod_RampartDashAndAimShield.m_shieldFrontEnemyEffectMod.GetModifiedValue(m_shieldFrontEnemyEffect);
		}
		else
		{
			effectInfo3 = m_shieldFrontEnemyEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "ShieldFrontEnemyEffect", m_shieldFrontEnemyEffect);
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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_passive.GetShieldBarrierData().ReportAbilityTooltipNumbers(ref numbers);
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		List<AbilityTooltipSubject> tooltipSubjectTypes2 = base.Targeters[1].GetTooltipSubjectTypes(targetActor);
		dictionary[AbilityTooltipSymbol.Damage] = 0;
		if (tooltipSubjectTypes != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy) && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				dictionary[AbilityTooltipSymbol.Damage] += GetDamageAmount();
			}
		}
		if (tooltipSubjectTypes2 != null)
		{
			while (true)
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
				while (true)
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
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					dictionary[AbilityTooltipSymbol.Damage] += GetDamageForShieldFront();
				}
			}
		}
		return dictionary;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool result = true;
		if (UseEnergyForMoveDistance())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = (caster.TechPoints >= GetMinEnergyToCast());
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (boardSquareSafe.IsBaselineHeight())
			{
				if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
				{
					bool result = false;
					if (targetIndex == 0)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe);
						if (boardSquarePathInfo != null)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							result = true;
							if (UseEnergyForMoveDistance() && GetEnergyPerMove() > 0)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								int num = caster.TechPoints / GetEnergyPerMove();
								int num2 = 0;
								BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
								while (boardSquarePathInfo2.next != null)
								{
									boardSquarePathInfo2 = boardSquarePathInfo2.next;
									num2++;
								}
								while (true)
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
						BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[0].GridPos);
						result = (boardSquareSafe2 == boardSquareSafe);
					}
					return result;
				}
				while (true)
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
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_RampartDashAndAimShield);
		}
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
