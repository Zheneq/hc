using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalDrag : Ability
{
	public float m_width = 1f;

	public float m_distance = 3f;

	public int m_maxTargets = 1;

	public bool m_penetrateLineOfSight;

	public int m_damage;

	public StandardEffectInfo m_casterEffect;

	public StandardEffectInfo m_targetEffect;

	private AbilityMod_RobotAnimalDrag m_abilityMod;

	private StandardEffectInfo m_cachedCasterEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Death Snuggle";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Laser(this, this.GetLaserWidth(), this.GetLaserDistance(), this.m_penetrateLineOfSight, this.m_maxTargets, false, this.GetCasterEffect().m_applyEffect);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserDistance();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedCasterEffect;
		if (this.m_abilityMod)
		{
			cachedCasterEffect = this.m_abilityMod.m_casterEffectMod.GetModifiedValue(this.m_casterEffect);
		}
		else
		{
			cachedCasterEffect = this.m_casterEffect;
		}
		this.m_cachedCasterEffect = cachedCasterEffect;
	}

	public StandardEffectInfo GetCasterEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedCasterEffect != null)
		{
			result = this.m_cachedCasterEffect;
		}
		else
		{
			result = this.m_casterEffect;
		}
		return result;
	}

	private float GetLaserDistance()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_distance;
		}
		else
		{
			result = this.m_abilityMod.m_distanceMod.GetModifiedValue(this.m_distance);
		}
		return result;
	}

	private float GetLaserWidth()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_widthMod.GetModifiedValue(this.m_width) : this.m_width;
	}

	public int GetDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damage);
		}
		else
		{
			result = this.m_damage;
		}
		return result;
	}

	public bool HasEffectOnNextTurnStart()
	{
		bool result;
		if (this.m_abilityMod == null)
		{
			result = false;
		}
		else if (!this.m_abilityMod.m_enemyEffectOnNextTurnStart.m_applyEffect)
		{
			if (this.m_abilityMod.m_powerUpsToSpawn != null)
			{
				result = (this.m_abilityMod.m_powerUpsToSpawn.Count > 0);
			}
			else
			{
				result = false;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	public StandardEffectInfo EffectInfoOnNextTurnStart()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
		{
			result = new StandardEffectInfo();
		}
		else
		{
			result = this.m_abilityMod.m_enemyEffectOnNextTurnStart;
		}
		return result;
	}

	public List<PowerUp> GetModdedPowerUpsToSpawn()
	{
		List<PowerUp> result;
		if (this.m_abilityMod == null)
		{
			result = null;
		}
		else
		{
			result = this.m_abilityMod.m_powerUpsToSpawn;
		}
		return result;
	}

	public AbilityAreaShape GetModdedPowerUpsToSpawnShape()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_powerUpsSpawnShape : AbilityAreaShape.SingleSquare;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDamage());
		this.GetCasterEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		this.m_targetEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RobotAnimalDrag abilityMod_RobotAnimalDrag = modAsBase as AbilityMod_RobotAnimalDrag;
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "Damage", string.Empty, this.m_damage, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_RobotAnimalDrag)
		{
			effectInfo = abilityMod_RobotAnimalDrag.m_casterEffectMod.GetModifiedValue(this.m_casterEffect);
		}
		else
		{
			effectInfo = this.m_casterEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "CasterEffect", this.m_casterEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_targetEffect, "TargetEffect", null, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RobotAnimalDrag))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_RobotAnimalDrag);
			this.Setup();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
