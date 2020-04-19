using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMonkBoundingLeap : Ability
{
	[Header("-- On Hit Damage, Effect, etc")]
	public int m_damageAmount = 0x14;

	public int m_damageAfterFirstHit;

	[Space(10f)]
	public StandardEffectInfo m_targetEffect;

	public int m_cooldownOnHit = -1;

	[Separator("Chase On Hit Data", true)]
	public bool m_chaseHitActor;

	public StandardEffectInfo m_chaserEffect;

	[Separator("Bounce", true)]
	public float m_width = 1f;

	public float m_maxDistancePerBounce = 15f;

	public float m_maxTotalDistance = 50f;

	public int m_maxBounces = 1;

	public int m_maxTargetsHit = 1;

	public bool m_bounceOffEnemyActor;

	[Separator("Bounce Anim", true)]
	public float m_recoveryTime = 0.5f;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	public GameObject m_sequenceOnCaster;

	private const bool c_penetrateLoS = false;

	private AbilityMod_BattleMonkBoundingLeap m_abilityMod;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkBoundingLeap.Start()).MethodHandle;
			}
			this.m_abilityName = "Bounding Leap";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		StandardEffectInfo moddedEffectForSelf = base.GetModdedEffectForSelf();
		float width = this.m_width;
		float maxDistancePerBounce = this.GetMaxDistancePerBounce();
		float maxTotalDistance = this.GetMaxTotalDistance();
		int maxBounces = this.GetMaxBounces();
		int maxTargets = this.GetMaxTargets();
		bool bounceOnEnemyActor = this.ShouldBounceOffEnemyActors();
		bool includeAlliesInBetween = this.IncludeAlliesInBetween();
		bool includeSelf;
		if (moddedEffectForSelf != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkBoundingLeap.SetupTargeter()).MethodHandle;
			}
			includeSelf = moddedEffectForSelf.m_applyEffect;
		}
		else
		{
			includeSelf = false;
		}
		base.Targeter = new AbilityUtil_Targeter_BounceActor(this, width, maxDistancePerBounce, maxTotalDistance, maxBounces, maxTargets, bounceOnEnemyActor, includeAlliesInBetween, includeSelf);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetMaxDistancePerBounce();
	}

	public int GetDamageAmount()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkBoundingLeap.GetDamageAmount()).MethodHandle;
			}
			result = this.m_damageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damageAmount);
		}
		return result;
	}

	public int GetDamageAfterFirstHit()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkBoundingLeap.GetDamageAfterFirstHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageAfterFirstHitMod.GetModifiedValue(this.m_damageAfterFirstHit);
		}
		else
		{
			result = this.m_damageAfterFirstHit;
		}
		return result;
	}

	public int GetMaxTargets()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkBoundingLeap.GetMaxTargets()).MethodHandle;
			}
			result = this.m_maxTargetsHit;
		}
		else
		{
			result = this.m_abilityMod.m_maxHitTargetsMod.GetModifiedValue(this.m_maxTargetsHit);
		}
		return result;
	}

	public bool ShouldBounceOffEnemyActors()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_bounceOffEnemyActorMod.GetModifiedValue(this.m_bounceOffEnemyActor) : this.m_bounceOffEnemyActor;
	}

	public bool IncludeAlliesInBetween()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkBoundingLeap.IncludeAlliesInBetween()).MethodHandle;
			}
			result = false;
		}
		else
		{
			result = this.m_abilityMod.m_hitAlliesInBetween.GetModifiedValue(false);
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkBoundingLeap.GetAllyHitEffect()).MethodHandle;
			}
			result = null;
		}
		else
		{
			result = this.m_abilityMod.m_allyHitEffect;
		}
		return result;
	}

	public int GetHealAmountIfNotDamagedThisTurn()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_healAmountIfNotDamagedThisTurn.GetModifiedValue(0) : 0;
	}

	public float GetMaxDistancePerBounce()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkBoundingLeap.GetMaxDistancePerBounce()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxDistancePerBounceMod.GetModifiedValue(this.m_maxDistancePerBounce);
		}
		else
		{
			result = this.m_maxDistancePerBounce;
		}
		return result;
	}

	public float GetMaxTotalDistance()
	{
		return (!this.m_abilityMod) ? this.m_maxTotalDistance : this.m_abilityMod.m_maxTotalDistanceMod.GetModifiedValue(this.m_maxTotalDistance);
	}

	public int GetMaxBounces()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkBoundingLeap.GetMaxBounces()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxBouncesMod.GetModifiedValue(this.m_maxBounces);
		}
		else
		{
			result = this.m_maxBounces;
		}
		return result;
	}

	public int CalcDamageForOrderIndex(int hitOrder)
	{
		int damageAfterFirstHit = this.GetDamageAfterFirstHit();
		if (damageAfterFirstHit > 0 && hitOrder > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkBoundingLeap.CalcDamageForOrderIndex(int)).MethodHandle;
			}
			return damageAfterFirstHit;
		}
		return this.GetDamageAmount();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_damageAmount);
		if (this.GetAllyHitEffect() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkBoundingLeap.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		}
		base.AppendTooltipNumbersFromBaseModEffects(ref result, AbilityTooltipSubject.Primary, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			AbilityUtil_Targeter_BounceActor abilityUtil_Targeter_BounceActor = base.Targeter as AbilityUtil_Targeter_BounceActor;
			if (abilityUtil_Targeter_BounceActor != null)
			{
				List<AbilityUtil_Targeter_BounceActor.HitActorContext> hitActorContext = abilityUtil_Targeter_BounceActor.GetHitActorContext();
				for (int i = 0; i < hitActorContext.Count; i++)
				{
					if (hitActorContext[i].actor == targetActor)
					{
						results.m_damage = this.CalcDamageForOrderIndex(i);
						return true;
					}
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkBoundingLeap.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
					return true;
				}
				return true;
			}
		}
		return false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
		base.AddTokenInt(tokens, "DamageAfterFirstHit", string.Empty, this.m_damageAfterFirstHit, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_targetEffect, "TargetEffect", this.m_targetEffect, true);
		base.AddTokenInt(tokens, "CooldownOnHit", string.Empty, this.m_cooldownOnHit, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_chaserEffect, "ChaserEffect", this.m_chaserEffect, true);
		base.AddTokenInt(tokens, "MaxBounces", string.Empty, this.m_maxBounces, false);
		base.AddTokenInt(tokens, "MaxTargetsHit", string.Empty, this.m_maxTargetsHit, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BattleMonkBoundingLeap))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkBoundingLeap.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_BattleMonkBoundingLeap);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
