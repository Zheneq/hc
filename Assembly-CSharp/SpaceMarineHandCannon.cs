using System;
using System.Collections.Generic;
using UnityEngine;

public class SpaceMarineHandCannon : Ability
{
	public int m_primaryDamage;

	public float m_primaryWidth = 1f;

	public float m_primaryLength = 3f;

	public int m_coneDamage;

	public float m_coneWidthAngle = 60f;

	public float m_coneLength = 4f;

	public float m_coneBackwardOffset;

	public bool m_penetrateLineOfSight;

	public StandardEffectInfo m_effectInfoOnPrimaryTarget;

	public StandardEffectInfo m_effectInfoOnConeTargets;

	private AbilityMod_SpaceMarineHandCannon m_abilityMod;

	private void Start()
	{
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.ShouldExplode())
		{
			base.Targeter = new AbilityUtil_Targeter_LaserWithCone(this, this.ModdedLaserWidth(), this.ModdedLaserLength(), this.m_penetrateLineOfSight, false, this.ModdedConeAngle(), this.ModdedConeLength(), this.m_coneBackwardOffset);
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_Laser(this, this.ModdedLaserWidth(), this.ModdedLaserLength(), this.m_penetrateLineOfSight, 1, false, false);
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		float num = 0f;
		if (this.ShouldExplode())
		{
			num = this.ModdedConeLength();
		}
		return this.ModdedLaserLength() + num;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.m_primaryDamage));
		this.m_effectInfoOnPrimaryTarget.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Primary);
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, this.m_coneDamage));
		this.m_effectInfoOnConeTargets.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Secondary);
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				dictionary[AbilityTooltipSymbol.Damage] = this.ModdedLaserDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				dictionary[AbilityTooltipSymbol.Damage] = this.ModdedConeDamage();
			}
			return dictionary;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SpaceMarineHandCannon abilityMod_SpaceMarineHandCannon = modAsBase as AbilityMod_SpaceMarineHandCannon;
		string name = "PrimaryDamage";
		string empty = string.Empty;
		int val;
		if (abilityMod_SpaceMarineHandCannon)
		{
			val = abilityMod_SpaceMarineHandCannon.m_laserDamageMod.GetModifiedValue(this.m_primaryDamage);
		}
		else
		{
			val = this.m_primaryDamage;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "ConeDamage";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_SpaceMarineHandCannon)
		{
			val2 = abilityMod_SpaceMarineHandCannon.m_coneDamageMod.GetModifiedValue(this.m_coneDamage);
		}
		else
		{
			val2 = this.m_coneDamage;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectInfoOnPrimaryTarget, "EffectInfoOnPrimaryTarget", this.m_effectInfoOnPrimaryTarget, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectInfoOnConeTargets, "EffectInfoOnConeTargets", this.m_effectInfoOnConeTargets, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SpaceMarineHandCannon))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SpaceMarineHandCannon);
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

	public int ModdedLaserDamage()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_primaryDamage;
		}
		else
		{
			result = this.m_abilityMod.m_laserDamageMod.GetModifiedValue(this.m_primaryDamage);
		}
		return result;
	}

	public int ModdedConeDamage()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_coneDamage;
		}
		else
		{
			result = this.m_abilityMod.m_coneDamageMod.GetModifiedValue(this.m_coneDamage);
		}
		return result;
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
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_coneLengthMod.GetModifiedValue(this.m_coneLength) : this.m_coneLength;
	}

	public float ModdedLaserWidth()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_primaryWidth;
		}
		else
		{
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_primaryWidth);
		}
		return result;
	}

	public float ModdedLaserLength()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_laserLengthMod.GetModifiedValue(this.m_primaryLength) : this.m_primaryLength;
	}

	public bool ShouldExplode()
	{
		return this.m_abilityMod == null || this.m_abilityMod.m_shouldExplodeMod.GetModifiedValue(true);
	}

	public StandardEffectInfo GetLaserEffectInfo()
	{
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_useLaserHitEffectOverride)
			{
				return this.m_abilityMod.m_laserHitEffectOverride;
			}
		}
		return this.m_effectInfoOnPrimaryTarget;
	}

	public StandardEffectInfo GetConeEffectInfo()
	{
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_useConeHitEffectOverride)
			{
				return this.m_abilityMod.m_coneHitEffectOverride;
			}
		}
		return this.m_effectInfoOnConeTargets;
	}
}
