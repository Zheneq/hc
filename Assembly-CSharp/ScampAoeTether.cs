using System;
using System.Collections.Generic;
using UnityEngine;

public class ScampAoeTether : Ability
{
	[Separator("Targeting", true)]
	public float m_aoeRadius = 5f;

	public bool m_ignoreLos;

	[Header("-- if > 0, will use this as distance to check whether tether should break")]
	public float m_tetherBreakDistanceOverride = -1f;

	[Separator("Whether to pull towards caster if target is out of range. If true, no longer do movement hits", true)]
	public bool m_pullToCasterInKnockback;

	public float m_maxKnockbackDist = 10f;

	[Separator("Disable in Shield Down mode?", true)]
	public bool m_disableIfShieldDown = true;

	[Separator("On Tether Apply", true)]
	public StandardEffectInfo m_tetherApplyEnemyEffect;

	[Separator("On Tether Break", true)]
	public int m_tetherBreakDamage = 0xA;

	public StandardEffectInfo m_tetherBreakEnemyEffecf;

	[Separator("Cdr if not triggered", true)]
	public int m_cdrIfNoTetherTrigger;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	public GameObject m_tetherBreakTriggerSequencePrefab;

	private Scamp_SyncComponent m_syncComp;

	private AbilityMod_ScampAoeTether m_abilityMod;

	private StandardEffectInfo m_cachedTetherApplyEnemyEffect;

	private StandardEffectInfo m_cachedTetherBreakEnemyEffecf;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "ScampAoeTether";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_syncComp = base.GetComponent<Scamp_SyncComponent>();
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, this.GetAoeRadius(), this.IgnoreLos(), true, false, -1);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, this.m_tetherApplyEnemyEffect, "TetherApplyEnemyEffect", this.m_tetherApplyEnemyEffect, true);
		base.AddTokenInt(tokens, "TetherBreakDamage", string.Empty, this.m_tetherBreakDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_tetherBreakEnemyEffecf, "TetherBreakEnemyEffecf", this.m_tetherBreakEnemyEffecf, true);
		base.AddTokenInt(tokens, "CdrIfNoTetherTrigger", string.Empty, this.m_cdrIfNoTetherTrigger, false);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_syncComp != null)
		{
			if (this.m_syncComp.m_suitWasActiveOnTurnStart)
			{
				return true;
			}
		}
		return !this.DisableIfShieldDown();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedTetherApplyEnemyEffect;
		if (this.m_abilityMod != null)
		{
			cachedTetherApplyEnemyEffect = this.m_abilityMod.m_tetherApplyEnemyEffectMod.GetModifiedValue(this.m_tetherApplyEnemyEffect);
		}
		else
		{
			cachedTetherApplyEnemyEffect = this.m_tetherApplyEnemyEffect;
		}
		this.m_cachedTetherApplyEnemyEffect = cachedTetherApplyEnemyEffect;
		StandardEffectInfo cachedTetherBreakEnemyEffecf;
		if (this.m_abilityMod != null)
		{
			cachedTetherBreakEnemyEffecf = this.m_abilityMod.m_tetherBreakEnemyEffecfMod.GetModifiedValue(this.m_tetherBreakEnemyEffecf);
		}
		else
		{
			cachedTetherBreakEnemyEffecf = this.m_tetherBreakEnemyEffecf;
		}
		this.m_cachedTetherBreakEnemyEffecf = cachedTetherBreakEnemyEffecf;
	}

	public float GetAoeRadius()
	{
		float result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_aoeRadiusMod.GetModifiedValue(this.m_aoeRadius);
		}
		else
		{
			result = this.m_aoeRadius;
		}
		return result;
	}

	public bool IgnoreLos()
	{
		return (!(this.m_abilityMod != null)) ? this.m_ignoreLos : this.m_abilityMod.m_ignoreLosMod.GetModifiedValue(this.m_ignoreLos);
	}

	public float GetTetherBreakDistanceOverride()
	{
		float result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_tetherBreakDistanceOverrideMod.GetModifiedValue(this.m_tetherBreakDistanceOverride);
		}
		else
		{
			result = this.m_tetherBreakDistanceOverride;
		}
		return result;
	}

	public bool PullToCasterInKnockback()
	{
		return (!(this.m_abilityMod != null)) ? this.m_pullToCasterInKnockback : this.m_abilityMod.m_pullToCasterInKnockbackMod.GetModifiedValue(this.m_pullToCasterInKnockback);
	}

	public float GetMaxKnockbackDist()
	{
		float result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_maxKnockbackDistMod.GetModifiedValue(this.m_maxKnockbackDist);
		}
		else
		{
			result = this.m_maxKnockbackDist;
		}
		return result;
	}

	public bool DisableIfShieldDown()
	{
		return (!(this.m_abilityMod != null)) ? this.m_disableIfShieldDown : this.m_abilityMod.m_disableIfShieldDownMod.GetModifiedValue(this.m_disableIfShieldDown);
	}

	public StandardEffectInfo GetTetherApplyEnemyEffect()
	{
		return (this.m_cachedTetherApplyEnemyEffect == null) ? this.m_tetherApplyEnemyEffect : this.m_cachedTetherApplyEnemyEffect;
	}

	public int GetTetherBreakDamage()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_tetherBreakDamageMod.GetModifiedValue(this.m_tetherBreakDamage);
		}
		else
		{
			result = this.m_tetherBreakDamage;
		}
		return result;
	}

	public StandardEffectInfo GetTetherBreakEnemyEffecf()
	{
		StandardEffectInfo result;
		if (this.m_cachedTetherBreakEnemyEffecf != null)
		{
			result = this.m_cachedTetherBreakEnemyEffecf;
		}
		else
		{
			result = this.m_tetherBreakEnemyEffecf;
		}
		return result;
	}

	public int GetCdrIfNoTetherTrigger()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_cdrIfNoTetherTriggerMod.GetModifiedValue(this.m_cdrIfNoTetherTrigger);
		}
		else
		{
			result = this.m_cdrIfNoTetherTrigger;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetAoeRadius();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScampAoeTether))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ScampAoeTether);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
