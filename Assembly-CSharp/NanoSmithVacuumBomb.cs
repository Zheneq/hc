using System;
using System.Collections.Generic;
using UnityEngine;

public class NanoSmithVacuumBomb : Ability
{
	[Header("-- Bomb Hit")]
	public int m_bombDamageAmount;

	public StandardEffectInfo m_enemyHitEffect;

	public AbilityAreaShape m_bombShape = AbilityAreaShape.Three_x_Three;

	public bool m_bombPenetrateLineOfSight;

	[Header("-- Effects")]
	public StandardEffectInfo m_onCenterActorEffect;

	[Header("-- Knockback")]
	public NanoSmithVacuumBomb.KnockbackCenterType m_knockbackCenterType;

	public int m_knockbackDelay;

	public KnockbackType m_knockbackType = KnockbackType.PullToSource;

	public float m_knockbackDistance = 2f;

	[Header("-- Only relevant for PullToSource, if checked, pull adjacent actors over to opposite side")]
	public bool m_knockbackAdjacentActorsIfPull = true;

	[Header("-- Sequences -----------------------------------")]
	public GameObject m_castSequencePrefab;

	public GameObject m_delayedKnockbackMarkerSequencePrefab;

	public GameObject m_delayedKnockbackHitSequencePrefab;

	private AbilityMod_NanoSmithVacuumBomb m_abilityMod;

	private NanoSmith_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedOnCenterActorEffect;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithVacuumBomb.Start()).MethodHandle;
			}
			this.m_abilityName = "Vacuum Bomb";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.m_syncComp = base.GetComponent<NanoSmith_SyncComponent>();
		this.SetCachedFields();
		float knockbackDistance = (this.m_knockbackDelay > 0) ? 0f : this.m_knockbackDistance;
		AbilityUtil_Targeter.AffectsActor affectsTargetOnGridposSquare = AbilityUtil_Targeter.AffectsActor.Never;
		if (base.GetModdedEffectForAllies() != null)
		{
			if (base.GetModdedEffectForAllies().m_applyEffect)
			{
				goto IL_6E;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithVacuumBomb.SetupTargeter()).MethodHandle;
			}
		}
		if (!this.GetCenterActorEffect().m_applyEffect)
		{
			goto IL_70;
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		IL_6E:
		affectsTargetOnGridposSquare = AbilityUtil_Targeter.AffectsActor.Always;
		IL_70:
		base.Targeter = new AbilityUtil_Targeter_KnockbackRingAoE(this, this.m_bombShape, this.m_bombPenetrateLineOfSight, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Never, affectsTargetOnGridposSquare, this.m_bombShape, knockbackDistance, this.m_knockbackType, this.m_knockbackAdjacentActorsIfPull, true);
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		base.AppendTooltipNumbersFromBaseModEffects(ref result, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamageAmount());
		StandardEffectInfo centerActorEffect = this.GetCenterActorEffect();
		if (centerActorEffect.m_applyEffect)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithVacuumBomb.CalculateNameplateTargetingNumbers()).MethodHandle;
			}
			centerActorEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
			centerActorEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		}
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (this.m_syncComp != null && this.m_syncComp.m_extraAbsorbOnVacuumBomb > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithVacuumBomb.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) == 0)
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
				StandardEffectInfo centerActorEffect = this.GetCenterActorEffect();
				int num = centerActorEffect.m_effectData.m_absorbAmount;
				num += this.m_syncComp.m_extraAbsorbOnVacuumBomb;
				results.m_absorb = num;
				return true;
			}
		}
		return false;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, false, true, true, Ability.ValidateCheckPath.Ignore, true, true, false);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithVacuumBomb abilityMod_NanoSmithVacuumBomb = modAsBase as AbilityMod_NanoSmithVacuumBomb;
		string name = "BombDamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_NanoSmithVacuumBomb)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithVacuumBomb.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_NanoSmithVacuumBomb.m_damageMod.GetModifiedValue(this.m_bombDamageAmount);
		}
		else
		{
			val = this.m_bombDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_NanoSmithVacuumBomb)
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
			effectInfo = abilityMod_NanoSmithVacuumBomb.m_enemyHitEffectOverride.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", this.m_enemyHitEffect, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_NanoSmithVacuumBomb)
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
			effectInfo2 = abilityMod_NanoSmithVacuumBomb.m_onCenterActorEffectOverride.GetModifiedValue(this.m_onCenterActorEffect);
		}
		else
		{
			effectInfo2 = this.m_onCenterActorEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "OnCenterActorEffect", this.m_onCenterActorEffect, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NanoSmithVacuumBomb))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_NanoSmithVacuumBomb);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedOnCenterActorEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithVacuumBomb.SetCachedFields()).MethodHandle;
			}
			cachedOnCenterActorEffect = this.m_abilityMod.m_onCenterActorEffectOverride.GetModifiedValue(this.m_onCenterActorEffect);
		}
		else
		{
			cachedOnCenterActorEffect = this.m_onCenterActorEffect;
		}
		this.m_cachedOnCenterActorEffect = cachedOnCenterActorEffect;
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
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectOverride.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
	}

	private int GetDamageAmount()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithVacuumBomb.GetDamageAmount()).MethodHandle;
			}
			result = this.m_bombDamageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_bombDamageAmount);
		}
		return result;
	}

	private int GetCooldownChangePerHit()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithVacuumBomb.GetCooldownChangePerHit()).MethodHandle;
			}
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_cooldownChangePerHitMod.GetModifiedValue(0);
		}
		return result;
	}

	private StandardEffectInfo GetCenterActorEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedOnCenterActorEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithVacuumBomb.GetCenterActorEffect()).MethodHandle;
			}
			result = this.m_cachedOnCenterActorEffect;
		}
		else
		{
			result = this.m_onCenterActorEffect;
		}
		return result;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		return (this.m_cachedEnemyHitEffect == null) ? this.m_enemyHitEffect : this.m_cachedEnemyHitEffect;
	}

	public int GetExtraAbsorb()
	{
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithVacuumBomb.GetExtraAbsorb()).MethodHandle;
			}
			return this.m_syncComp.m_extraAbsorbOnVacuumBomb;
		}
		return 0;
	}

	public enum KnockbackCenterType
	{
		FromTargetSquare,
		FromTargetActor
	}
}
