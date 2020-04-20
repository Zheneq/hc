using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMonkBasicAttack : Ability
{
	[Space(10f)]
	public float m_coneWidthAngle = 270f;

	public float m_coneLength = 1.5f;

	public float m_coneBackwardOffset;

	public int m_damageAmount = 0x14;

	public bool m_penetrateLineOfSight;

	public int m_maxTargets = 2;

	public int m_healAmountPerTargetHit;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_BattleMonkBasicAttack m_abilityMod;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Siphon Slash";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, this.ModdedConeAngle(), this.ModdedConeLength(), this.m_coneBackwardOffset, this.m_penetrateLineOfSight, true, true, false, this.ModdedHealPerTargetHit() > 0, -1, false);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.ModdedConeLength();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BattleMonkBasicAttack abilityMod_BattleMonkBasicAttack = modAsBase as AbilityMod_BattleMonkBasicAttack;
		int val = (!abilityMod_BattleMonkBasicAttack) ? this.m_damageAmount : abilityMod_BattleMonkBasicAttack.m_coneDamageMod.GetModifiedValue(this.m_damageAmount);
		tokens.Add(new TooltipTokenInt("Damage", "damage to enemies", val));
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.m_damageAmount)
		};
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.ModdedConeDamage(1));
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.ModdedHealPerTargetHit());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				int num = this.ModdedHealPerTargetHit() * visibleActorsCountByTooltipSubject;
				dictionary[AbilityTooltipSymbol.Healing] = Mathf.RoundToInt((float)num);
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				dictionary[AbilityTooltipSymbol.Damage] = this.ModdedConeDamage(visibleActorsCountByTooltipSubject);
			}
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BattleMonkBasicAttack))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_BattleMonkBasicAttack);
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

	public float ModdedConeAngle()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_coneWidthAngle;
		}
		else
		{
			result = this.m_abilityMod.m_coneAngleMod.GetModifiedValue(this.m_coneWidthAngle);
		}
		return result;
	}

	public float ModdedConeLength()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_coneLength;
		}
		else
		{
			result = this.m_abilityMod.m_coneLengthMod.GetModifiedValue(this.m_coneLength);
		}
		return result;
	}

	public int ModdedConeDamage(int numTargets)
	{
		int num = this.m_damageAmount;
		if (this.m_abilityMod != null)
		{
			num = this.m_abilityMod.m_coneDamageMod.GetModifiedValue(num);
			num += this.m_abilityMod.m_extraDamagePerTarget.GetModifiedValue(0) * (numTargets - 1);
		}
		return num;
	}

	public int ModdedHealPerTargetHit()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_healAmountPerTargetHit;
		}
		else
		{
			result = this.m_abilityMod.m_healPerTargetHitMod.GetModifiedValue(this.m_healAmountPerTargetHit);
		}
		return result;
	}
}
