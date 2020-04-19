using System;
using System.Collections.Generic;
using UnityEngine;

public class SenseiBide : Ability
{
	[Header("-- Targeting --")]
	public bool m_targetingIgnoreLos;

	[Separator("Effect on Cast Target", "cyan")]
	public StandardActorEffectData m_onCastTargetEffectData;

	[Header("-- Additional Effect on targeted actor, for shielding, etc")]
	public StandardEffectInfo m_additionalTargetHitEffect;

	[Separator("For Explosion Hits", "cyan")]
	public float m_explosionRadius = 1.5f;

	public bool m_ignoreLos;

	[Header("-- Explosion Hit --")]
	public int m_maxDamage = 0x32;

	public int m_baseDamage;

	public float m_damageMult = 1f;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Heal portion of absorb remaining")]
	public float m_absorbMultForHeal;

	[Header("-- Damage portion of initial damage, on turns after")]
	public float m_multOnInitialDamageForSubseqHits;

	[Separator("Extra Heal on Heal AoE Ability", true)]
	public int m_extraHealOnHealAoeIfQueued;

	[Header("-- Animation --")]
	public int m_explosionAnimIndex;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	[Header("    Used by effect when actual explosion happens")]
	public GameObject m_onExplosionSequencePrefab;

	private AbilityMod_SenseiBide m_abilityMod;

	private StandardActorEffectData m_cachedOnCastTargetEffectData;

	private StandardEffectInfo m_cachedAdditionalTargetHitEffect;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBide.Start()).MethodHandle;
			}
			this.m_abilityName = "SenseiBide";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		AbilityUtil_Targeter_AoE_AroundActor abilityUtil_Targeter_AoE_AroundActor = new AbilityUtil_Targeter_AoE_AroundActor(this, this.GetExplosionRadius(), this.IgnoreLos(), true, false, -1, false, true, true);
		abilityUtil_Targeter_AoE_AroundActor.SetAffectedGroups(true, false, false);
		abilityUtil_Targeter_AoE_AroundActor.m_allyOccupantSubject = AbilityTooltipSubject.Tertiary;
		abilityUtil_Targeter_AoE_AroundActor.m_enemyOccupantSubject = AbilityTooltipSubject.Quaternary;
		base.Targeter = abilityUtil_Targeter_AoE_AroundActor;
		base.Targeter.SetShowArcToShape(true);
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Art --</color>\nFor Persistent sequence, specify on " + Ability.SetupNoteVarName("On Cast Target Effect Data") + "'s sequence field";
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedOnCastTargetEffectData;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBide.SetCachedFields()).MethodHandle;
			}
			cachedOnCastTargetEffectData = this.m_abilityMod.m_onCastTargetEffectDataMod.GetModifiedValue(this.m_onCastTargetEffectData);
		}
		else
		{
			cachedOnCastTargetEffectData = this.m_onCastTargetEffectData;
		}
		this.m_cachedOnCastTargetEffectData = cachedOnCastTargetEffectData;
		StandardEffectInfo cachedAdditionalTargetHitEffect;
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
			cachedAdditionalTargetHitEffect = this.m_abilityMod.m_additionalTargetHitEffectMod.GetModifiedValue(this.m_additionalTargetHitEffect);
		}
		else
		{
			cachedAdditionalTargetHitEffect = this.m_additionalTargetHitEffect;
		}
		this.m_cachedAdditionalTargetHitEffect = cachedAdditionalTargetHitEffect;
		StandardEffectInfo cachedEnemyHitEffect;
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
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
	}

	public bool TargetingIgnoreLos()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBide.TargetingIgnoreLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_targetingIgnoreLosMod.GetModifiedValue(this.m_targetingIgnoreLos);
		}
		else
		{
			result = this.m_targetingIgnoreLos;
		}
		return result;
	}

	public StandardActorEffectData GetOnCastTargetEffectData()
	{
		return (this.m_cachedOnCastTargetEffectData == null) ? this.m_onCastTargetEffectData : this.m_cachedOnCastTargetEffectData;
	}

	public StandardEffectInfo GetAdditionalTargetHitEffect()
	{
		return (this.m_cachedAdditionalTargetHitEffect == null) ? this.m_additionalTargetHitEffect : this.m_cachedAdditionalTargetHitEffect;
	}

	public float GetExplosionRadius()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBide.GetExplosionRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_explosionRadiusMod.GetModifiedValue(this.m_explosionRadius);
		}
		else
		{
			result = this.m_explosionRadius;
		}
		return result;
	}

	public bool IgnoreLos()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBide.IgnoreLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_ignoreLosMod.GetModifiedValue(this.m_ignoreLos);
		}
		else
		{
			result = this.m_ignoreLos;
		}
		return result;
	}

	public int GetMaxDamage()
	{
		return (!this.m_abilityMod) ? this.m_maxDamage : this.m_abilityMod.m_maxDamageMod.GetModifiedValue(this.m_maxDamage);
	}

	public int GetBaseDamage()
	{
		return (!this.m_abilityMod) ? this.m_baseDamage : this.m_abilityMod.m_baseDamageMod.GetModifiedValue(this.m_baseDamage);
	}

	public float GetDamageMult()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBide.GetDamageMult()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageMultMod.GetModifiedValue(this.m_damageMult);
		}
		else
		{
			result = this.m_damageMult;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBide.GetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public float GetAbsorbMultForHeal()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBide.GetAbsorbMultForHeal()).MethodHandle;
			}
			result = this.m_abilityMod.m_absorbMultForHealMod.GetModifiedValue(this.m_absorbMultForHeal);
		}
		else
		{
			result = this.m_absorbMultForHeal;
		}
		return result;
	}

	public float GetMultOnInitialDamageForSubseqHits()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBide.GetMultOnInitialDamageForSubseqHits()).MethodHandle;
			}
			result = this.m_abilityMod.m_multOnInitialDamageForSubseqHitsMod.GetModifiedValue(this.m_multOnInitialDamageForSubseqHits);
		}
		else
		{
			result = this.m_multOnInitialDamageForSubseqHits;
		}
		return result;
	}

	public int GetExtraHealOnHealAoeIfQueued()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBide.GetExtraHealOnHealAoeIfQueued()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraHealOnHealAoeIfQueuedMod.GetModifiedValue(this.m_extraHealOnHealAoeIfQueued);
		}
		else
		{
			result = this.m_extraHealOnHealAoeIfQueued;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetOnCastTargetEffectData().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Tertiary);
		this.GetAdditionalTargetHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Tertiary);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		this.m_onCastTargetEffectData.AddTooltipTokens(tokens, "OnCastTargetEffectData", false, null);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_additionalTargetHitEffect, "AdditionalTargetHitEffect", this.m_additionalTargetHitEffect, true);
		base.AddTokenInt(tokens, "MaxDamage", string.Empty, this.m_maxDamage, false);
		base.AddTokenInt(tokens, "BaseDamage", string.Empty, this.m_baseDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyHitEffect", this.m_enemyHitEffect, true);
		base.AddTokenInt(tokens, "ExtraHealOnHealAoeIfQueued", string.Empty, this.m_extraHealOnHealAoeIfQueued, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, false, true, true, Ability.ValidateCheckPath.Ignore, this.TargetingIgnoreLos(), true, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SenseiBide))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiBide.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_SenseiBide);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
