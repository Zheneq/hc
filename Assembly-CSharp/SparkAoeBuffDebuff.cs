using System;
using System.Collections.Generic;
using UnityEngine;

public class SparkAoeBuffDebuff : Ability
{
	[Header("-- Targeting")]
	public SparkAoeBuffDebuff.TargetingType m_TargetingType;

	public bool m_penetrateLos;

	[Header("-- Shape")]
	public AbilityAreaShape m_shape = AbilityAreaShape.Five_x_Five_NoCorners;

	[Header("-- Radius")]
	public float m_radius = 6f;

	[Header("-- Damage and Healing")]
	public int m_damageAmount;

	public int m_allyHealAmount = 0xA;

	[Header("-- Self Heal Per Hit")]
	public int m_baseSelfHeal;

	public int m_selfHealAmountPerHit;

	public bool m_selfHealCountEnemyHit = true;

	public bool m_selfHealCountAllyHit = true;

	[Header("-- Normal Hit Effects")]
	public StandardEffectInfo m_selfHitEffect;

	public StandardEffectInfo m_allyHitEffect;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_sequenceOnEnemies;

	public GameObject m_sequenceOnAllies;

	private AbilityMod_SparkAoeBuffDebuff m_abilityMod;

	private StandardEffectInfo m_cachedSelfHitEffect;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.Start()).MethodHandle;
			}
			this.m_abilityName = "Spark Aoe Buff Debuff";
		}
		this.SetupTargeter();
	}

	public float GetTargetingRadius()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.GetTargetingRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_radiusMod.GetModifiedValue(this.m_radius);
		}
		else
		{
			result = this.m_radius;
		}
		return result;
	}

	public AbilityAreaShape GetHitShape()
	{
		return this.m_shape;
	}

	public bool ShouldIgnoreLos()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.ShouldIgnoreLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_ignoreLosMod.GetModifiedValue(this.m_penetrateLos);
		}
		else
		{
			result = this.m_penetrateLos;
		}
		return result;
	}

	public int GetAllyHeal(AbilityMod_SparkAoeBuffDebuff mod)
	{
		return (!mod) ? this.m_allyHealAmount : mod.m_allyHealMod.GetModifiedValue(this.m_allyHealAmount);
	}

	public int GetBaseSelfHeal(AbilityMod_SparkAoeBuffDebuff mod)
	{
		int result;
		if (mod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.GetBaseSelfHeal(AbilityMod_SparkAoeBuffDebuff)).MethodHandle;
			}
			result = mod.m_baseSelfHealMod.GetModifiedValue(this.m_baseSelfHeal);
		}
		else
		{
			result = this.m_baseSelfHeal;
		}
		return result;
	}

	public int GetSelfHealPerHit(AbilityMod_SparkAoeBuffDebuff mod)
	{
		return (!mod) ? this.m_selfHealAmountPerHit : mod.m_selfHealPerHitMod.GetModifiedValue(this.m_selfHealAmountPerHit);
	}

	public bool SelfHealCountAllyHit()
	{
		return (!this.m_abilityMod) ? this.m_selfHealCountAllyHit : this.m_abilityMod.m_selfHealHitCountAlly.GetModifiedValue(this.m_selfHealCountAllyHit);
	}

	public bool SelfHealCountEnemyHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.SelfHealCountEnemyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfHealHitCountEnemy.GetModifiedValue(this.m_selfHealCountEnemyHit);
		}
		else
		{
			result = this.m_selfHealCountEnemyHit;
		}
		return result;
	}

	public int GetShieldOnSelfPerAllyHit()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.GetShieldOnSelfPerAllyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_shieldOnSelfPerAllyHitMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetShieldOnSelfDuration()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.GetShieldOnSelfDuration()).MethodHandle;
			}
			result = this.m_abilityMod.m_shieldOnSelfDuration;
		}
		else
		{
			result = 1;
		}
		return result;
	}

	public bool IncludeCaster()
	{
		if (!this.GetSelfHitEffect().m_applyEffect)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.IncludeCaster()).MethodHandle;
			}
			if (this.GetSelfHealPerHit(this.m_abilityMod) <= 0)
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
				if (this.GetBaseSelfHeal(this.m_abilityMod) <= 0)
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
					return this.GetShieldOnSelfPerAllyHit() > 0;
				}
			}
		}
		return true;
	}

	public bool IncludeAllies()
	{
		bool result;
		if (!this.GetAllyHitEffect().m_applyEffect)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.IncludeAllies()).MethodHandle;
			}
			result = (this.GetAllyHeal(this.m_abilityMod) > 0);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IncludeEnemies()
	{
		bool result;
		if (!this.GetEnemyHitEffect().m_applyEffect)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.IncludeEnemies()).MethodHandle;
			}
			result = (this.m_damageAmount > 0);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedSelfHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.GetSelfHitEffect()).MethodHandle;
			}
			result = this.m_cachedSelfHitEffect;
		}
		else
		{
			result = this.m_selfHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return (this.m_cachedAllyHitEffect == null) ? this.m_allyHitEffect : this.m_cachedAllyHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.GetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	private void SetCachedFields()
	{
		this.m_cachedSelfHitEffect = ((!this.m_abilityMod) ? this.m_selfHitEffect : this.m_abilityMod.m_selfHitEffectMod.GetModifiedValue(this.m_selfHitEffect));
		StandardEffectInfo cachedAllyHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.SetCachedFields()).MethodHandle;
			}
			cachedAllyHitEffect = this.m_abilityMod.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = this.m_allyHitEffect;
		}
		this.m_cachedAllyHitEffect = cachedAllyHitEffect;
		StandardEffectInfo cachedEnemyHitEffect;
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
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		if (this.m_TargetingType == SparkAoeBuffDebuff.TargetingType.UseShape)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.SetupTargeter()).MethodHandle;
			}
			AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
			if (!this.IncludeCaster())
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
				affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
			}
			base.Targeter = new AbilityUtil_Targeter_Shape(this, this.GetHitShape(), this.ShouldIgnoreLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, this.IncludeEnemies(), this.IncludeAllies(), affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, this.GetTargetingRadius(), this.ShouldIgnoreLos(), this.IncludeEnemies(), this.IncludeAllies(), -1);
		}
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SparkAoeBuffDebuff abilityMod_SparkAoeBuffDebuff = modAsBase as AbilityMod_SparkAoeBuffDebuff;
		base.AddTokenInt(tokens, "Heal_OnAlly", "heal on ally", this.GetAllyHeal(abilityMod_SparkAoeBuffDebuff), false);
		base.AddTokenInt(tokens, "Heal_OnSelfBase", "heal on self, base amount", this.GetBaseSelfHeal(abilityMod_SparkAoeBuffDebuff), false);
		base.AddTokenInt(tokens, "Heal_OnSelfPerHit", "heal on self, per hit", this.GetSelfHealPerHit(abilityMod_SparkAoeBuffDebuff), false);
		StandardEffectInfo effectInfo;
		if (abilityMod_SparkAoeBuffDebuff)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			effectInfo = abilityMod_SparkAoeBuffDebuff.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			effectInfo = this.m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnAlly", this.m_allyHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_SparkAoeBuffDebuff) ? this.m_selfHitEffect : abilityMod_SparkAoeBuffDebuff.m_selfHitEffectMod.GetModifiedValue(this.m_selfHitEffect), "EffectOnSelf", this.m_selfHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_SparkAoeBuffDebuff) ? this.m_enemyHitEffect : abilityMod_SparkAoeBuffDebuff.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect), "EffectOnEnemy", this.m_enemyHitEffect, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetSelfHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		this.GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetBaseSelfHeal(this.m_abilityMod) + this.GetSelfHealPerHit(this.m_abilityMod));
		AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Self, this.GetShieldOnSelfPerAllyHit());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetAllyHeal(this.m_abilityMod));
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.m_damageAmount);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (this.GetSelfHealPerHit(this.m_abilityMod) <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			if (this.GetBaseSelfHeal(this.m_abilityMod) <= 0 && this.GetShieldOnSelfPerAllyHit() <= 0)
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
				return null;
			}
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
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
				List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
				{
					if (visibleActorsInRangeByTooltipSubject[i].GetTeam() != targetActor.GetTeam())
					{
						num++;
					}
					else if (visibleActorsInRangeByTooltipSubject[i] != targetActor)
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
						num2++;
					}
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
				int value = this.CalcSelfHealAmountFromHits(num2, num);
				dictionary[AbilityTooltipSymbol.Healing] = value;
				if (this.GetShieldOnSelfPerAllyHit() > 0)
				{
					int num3 = 0;
					StandardEffectInfo selfHitEffect = this.GetSelfHitEffect();
					if (selfHitEffect.m_applyEffect && selfHitEffect.m_effectData.m_absorbAmount > 0)
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
						num3 = selfHitEffect.m_effectData.m_absorbAmount;
					}
					dictionary[AbilityTooltipSymbol.Absorb] = num3 + num2 * this.GetShieldOnSelfPerAllyHit();
				}
			}
		}
		return dictionary;
	}

	private int CalcSelfHealAmountFromHits(int allyHits, int enemyHits)
	{
		int result = 0;
		if (this.GetSelfHealPerHit(this.m_abilityMod) <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.CalcSelfHealAmountFromHits(int, int)).MethodHandle;
			}
			if (this.GetBaseSelfHeal(this.m_abilityMod) <= 0)
			{
				return result;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		int num = 0;
		if (this.SelfHealCountAllyHit())
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
			num += allyHits;
		}
		if (this.SelfHealCountEnemyHit())
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
			num += enemyHits;
		}
		result = this.GetBaseSelfHeal(this.m_abilityMod) + num * this.GetSelfHealPerHit(this.m_abilityMod);
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SparkAoeBuffDebuff))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkAoeBuffDebuff.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_SparkAoeBuffDebuff);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public enum TargetingType
	{
		UseShape,
		UseRadius
	}
}
