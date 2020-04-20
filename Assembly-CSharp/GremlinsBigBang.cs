using System;
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
		return this.m_abilityMod;
	}

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Big Bang";
		}
		this.SetupTargeter();
	}

	protected void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_KnockbackRingAOE_SingleTargetBoost(this, this.ModdedBombShape(), this.m_bombPenetrateLineOfSight, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Never, AbilityUtil_Targeter.AffectsActor.Possible, this.ModdedKnockbackShape(), this.GetKnockbackDistance(), this.m_knockbackType, false, true, this.ModdedKnockbackDistanceForSingleTarget());
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_GremlinsBigBang))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_GremlinsBigBang);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private AbilityAreaShape ModdedBombShape()
	{
		if (this.m_abilityMod != null)
		{
			return this.m_abilityMod.m_bombShape.GetModifiedValue(this.m_bombShape);
		}
		return this.m_bombShape;
	}

	private AbilityAreaShape ModdedKnockbackShape()
	{
		if (this.m_abilityMod != null)
		{
			return this.m_abilityMod.m_knockbackShape.GetModifiedValue(this.m_knockbackShape);
		}
		return this.m_knockbackShape;
	}

	public float GetKnockbackDistance()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(this.m_knockbackDistance);
		}
		else
		{
			result = this.m_knockbackDistance;
		}
		return result;
	}

	private int ModdedExtraDamagePerTarget()
	{
		if (this.m_abilityMod != null)
		{
			return this.m_abilityMod.m_extraDamagePerTarget.GetModifiedValue(0);
		}
		return 0;
	}

	private int ModdedDamageForSingleTarget()
	{
		if (this.m_abilityMod != null)
		{
			return this.m_abilityMod.m_damageIfSingleTarget.GetModifiedValue(this.m_bombDamageAmount);
		}
		return this.m_bombDamageAmount;
	}

	private float ModdedKnockbackDistanceForSingleTarget()
	{
		if (this.m_abilityMod != null)
		{
			return this.m_abilityMod.m_knockbackDistanceIfSingleTarget.GetModifiedValue(this.GetKnockbackDistance());
		}
		return this.GetKnockbackDistance();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.m_bombDamageAmount);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeter.GetActorsInRange();
		int num = 0;
		foreach (AbilityUtil_Targeter.ActorTarget actorTarget in actorsInRange)
		{
			if (actorTarget.m_actor.IsVisibleToClient())
			{
				num++;
			}
		}
		int num2 = (num - 1) * this.ModdedExtraDamagePerTarget();
		int num3 = this.m_bombDamageAmount;
		if (num == 1)
		{
			num3 = this.ModdedDamageForSingleTarget();
		}
		Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, num3 + num2, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "Damage", string.Empty, this.m_bombDamageAmount, false);
	}
}
