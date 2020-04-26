using System.Collections.Generic;
using UnityEngine;

public class RageBeastCharge : Ability
{
	[Space(10f)]
	public int m_damageAmount = 20;

	public int m_damageNearChargeEnd;

	public float m_damageRadius = 5f;

	public float m_radiusBehindStart;

	public float m_radiusBeyondEnd;

	public bool m_penetrateLineOfSight;

	public StandardEffectInfo m_enemyHitEffectNearChargeEnd;

	private AbilityMod_RageBeastCharge m_abilityMod;

	private StandardEffectInfo m_cachedEnemyHitEffectNearChargeEnd;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Pain Train";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_ChargeAoE(this, m_radiusBehindStart, ModdedChargeEndRadius(), ModdedChargeLineRadius(), -1, false, m_penetrateLineOfSight);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, ModdedDamage()));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Far) && ModdedDamageNearChargeEnd() > 0)
			{
				dictionary[AbilityTooltipSymbol.Damage] = ModdedDamageNearChargeEnd();
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastCharge abilityMod_RageBeastCharge = modAsBase as AbilityMod_RageBeastCharge;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_RageBeastCharge)
		{
			val = abilityMod_RageBeastCharge.m_damageMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			val = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_RageBeastCharge)
		{
			val2 = abilityMod_RageBeastCharge.m_damageNearChargeEndMod.GetModifiedValue(m_damageNearChargeEnd);
		}
		else
		{
			val2 = m_damageNearChargeEnd;
		}
		AddTokenInt(tokens, "DamageNearChargeEnd", empty2, val2);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffectNearChargeEnd, "EnemyHitEffectNearChargeEnd", m_enemyHitEffectNearChargeEnd);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.IsBaselineHeight() && boardSquareSafe != caster.GetCurrentBoardSquare())
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
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RageBeastCharge))
		{
			m_abilityMod = (abilityMod as AbilityMod_RageBeastCharge);
			SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyHitEffectNearChargeEnd;
		if ((bool)m_abilityMod)
		{
			cachedEnemyHitEffectNearChargeEnd = m_abilityMod.m_enemyHitEffectNearChargeEndMod.GetModifiedValue(m_enemyHitEffectNearChargeEnd);
		}
		else
		{
			cachedEnemyHitEffectNearChargeEnd = m_enemyHitEffectNearChargeEnd;
		}
		m_cachedEnemyHitEffectNearChargeEnd = cachedEnemyHitEffectNearChargeEnd;
	}

	public int ModdedDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_damageAmount;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount);
		}
		return result;
	}

	public int ModdedDamageNearChargeEnd()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_damageNearChargeEndMod.GetModifiedValue(m_damageNearChargeEnd) : m_damageNearChargeEnd;
	}

	public float ModdedChargeLineRadius()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_damageRadius;
		}
		else
		{
			result = m_abilityMod.m_chargeLineRadiusMod.GetModifiedValue(m_damageRadius);
		}
		return result;
	}

	public float ModdedChargeEndRadius()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_chargeEndRadius.GetModifiedValue(m_radiusBeyondEnd) : m_radiusBeyondEnd;
	}

	public StandardEffectInfo GetEnemyHitEffectNearChargeEnd()
	{
		return (m_cachedEnemyHitEffectNearChargeEnd == null) ? m_enemyHitEffectNearChargeEnd : m_cachedEnemyHitEffectNearChargeEnd;
	}
}
