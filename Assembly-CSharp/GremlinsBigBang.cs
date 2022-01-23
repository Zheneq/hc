using System.Collections.Generic;
using UnityEngine;

public class GremlinsBigBang : Ability
{
	public int m_bombDamageAmount = 5;

	public AbilityAreaShape m_bombShape = AbilityAreaShape.Three_x_Three;

	public bool m_bombPenetrateLineOfSight;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	public float m_knockbackDistance = 2f;

	public AbilityAreaShape m_knockbackShape = AbilityAreaShape.Five_x_Five;

	[Header("-- Sequences -----------------------------------")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_GremlinsBigBang m_abilityMod;

	public AbilityMod_GremlinsBigBang GetMod()
	{
		return m_abilityMod;
	}

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Big Bang";
		}
		SetupTargeter();
	}

	protected void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_KnockbackRingAOE_SingleTargetBoost(this, ModdedBombShape(), m_bombPenetrateLineOfSight, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Never, AbilityUtil_Targeter.AffectsActor.Possible, ModdedKnockbackShape(), GetKnockbackDistance(), m_knockbackType, false, true, ModdedKnockbackDistanceForSingleTarget());
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_GremlinsBigBang))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_GremlinsBigBang);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private AbilityAreaShape ModdedBombShape()
	{
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_abilityMod.m_bombShape.GetModifiedValue(m_bombShape);
				}
			}
		}
		return m_bombShape;
	}

	private AbilityAreaShape ModdedKnockbackShape()
	{
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_abilityMod.m_knockbackShape.GetModifiedValue(m_knockbackShape);
				}
			}
		}
		return m_knockbackShape;
	}

	public float GetKnockbackDistance()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance);
		}
		else
		{
			result = m_knockbackDistance;
		}
		return result;
	}

	private int ModdedExtraDamagePerTarget()
	{
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_abilityMod.m_extraDamagePerTarget.GetModifiedValue(0);
				}
			}
		}
		return 0;
	}

	private int ModdedDamageForSingleTarget()
	{
		if (m_abilityMod != null)
		{
			return m_abilityMod.m_damageIfSingleTarget.GetModifiedValue(m_bombDamageAmount);
		}
		return m_bombDamageAmount;
	}

	private float ModdedKnockbackDistanceForSingleTarget()
	{
		if (m_abilityMod != null)
		{
			return m_abilityMod.m_knockbackDistanceIfSingleTarget.GetModifiedValue(GetKnockbackDistance());
		}
		return GetKnockbackDistance();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_bombDamageAmount);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeter.GetActorsInRange();
		int num = 0;
		foreach (AbilityUtil_Targeter.ActorTarget item in actorsInRange)
		{
			if (item.m_actor.IsActorVisibleToClient())
			{
				num++;
			}
		}
		int num2 = (num - 1) * ModdedExtraDamagePerTarget();
		int num3 = m_bombDamageAmount;
		if (num == 1)
		{
			num3 = ModdedDamageForSingleTarget();
		}
		Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, num3 + num2);
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Damage", string.Empty, m_bombDamageAmount);
	}
}
