using System;
using System.Collections.Generic;
using UnityEngine;

public class RampartAoeBuffDebuff : Ability
{
	[Header("-- Targeting")]
	public AbilityAreaShape m_shape = AbilityAreaShape.Five_x_Five_NoCorners;

	public bool m_penetrateLos;

	[Header("-- Self Heal Per Hit")]
	public int m_baseSelfHeal;

	public int m_selfHealAmountPerHit;

	public bool m_selfHealCountEnemyHit = true;

	public bool m_selfHealCountAllyHit = true;

	[Header("-- Normal Hit Effects")]
	public bool m_includeCaster = true;

	public StandardEffectInfo m_selfHitEffect;

	public StandardEffectInfo m_allyHitEffect;

	public StandardEffectInfo m_enemyHitEffect;

	public int m_damageToEnemies;

	public int m_healingToAllies;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_RampartAoeBuffDebuff m_abilityMod;

	private StandardEffectInfo m_cachedSelfHitEffect;

	private StandardEffectInfo m_cachedAllyHitEffect;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.Start()).MethodHandle;
			}
			this.m_abilityName = "Robotic Roar";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
		if (!this.IncludeCaster())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.SetupTargeter()).MethodHandle;
			}
			affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
		}
		base.Targeter = new AbilityUtil_Targeter_Shape(this, this.GetShape(), this.PenetrateLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, this.IncludeEnemies(), this.IncludeAllies(), affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartAoeBuffDebuff))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_RampartAoeBuffDebuff);
		}
		this.SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedSelfHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.SetCachedFields()).MethodHandle;
			}
			cachedSelfHitEffect = this.m_abilityMod.m_selfHitEffectMod.GetModifiedValue(this.m_selfHitEffect);
		}
		else
		{
			cachedSelfHitEffect = this.m_selfHitEffect;
		}
		this.m_cachedSelfHitEffect = cachedSelfHitEffect;
		this.m_cachedAllyHitEffect = ((!this.m_abilityMod) ? this.m_allyHitEffect : this.m_abilityMod.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect));
		StandardEffectInfo cachedEnemyHitEffect;
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
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartAoeBuffDebuff abilityMod_RampartAoeBuffDebuff = modAsBase as AbilityMod_RampartAoeBuffDebuff;
		string name = "BaseSelfHeal";
		string empty = string.Empty;
		int val;
		if (abilityMod_RampartAoeBuffDebuff)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_RampartAoeBuffDebuff.m_baseSelfHealMod.GetModifiedValue(this.m_baseSelfHeal);
		}
		else
		{
			val = this.m_baseSelfHeal;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "SelfHealAmountPerHit", string.Empty, (!abilityMod_RampartAoeBuffDebuff) ? this.m_selfHealAmountPerHit : abilityMod_RampartAoeBuffDebuff.m_selfHealAmountPerHitMod.GetModifiedValue(this.m_selfHealAmountPerHit), false);
		StandardEffectInfo effectInfo;
		if (abilityMod_RampartAoeBuffDebuff)
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
			effectInfo = abilityMod_RampartAoeBuffDebuff.m_selfHitEffectMod.GetModifiedValue(this.m_selfHitEffect);
		}
		else
		{
			effectInfo = this.m_selfHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "SelfHitEffect", this.m_selfHitEffect, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_RampartAoeBuffDebuff)
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
			effectInfo2 = abilityMod_RampartAoeBuffDebuff.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			effectInfo2 = this.m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "AllyHitEffect", this.m_allyHitEffect, true);
		StandardEffectInfo effectInfo3;
		if (abilityMod_RampartAoeBuffDebuff)
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
			effectInfo3 = abilityMod_RampartAoeBuffDebuff.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo3 = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "EnemyHitEffect", this.m_enemyHitEffect, true);
		string name2 = "DamageToEnemies";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_RampartAoeBuffDebuff)
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
			val2 = abilityMod_RampartAoeBuffDebuff.m_damageToEnemiesMod.GetModifiedValue(this.m_damageToEnemies);
		}
		else
		{
			val2 = this.m_damageToEnemies;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		base.AddTokenInt(tokens, "HealingToAllies", string.Empty, (!abilityMod_RampartAoeBuffDebuff) ? this.m_healingToAllies : abilityMod_RampartAoeBuffDebuff.m_healingToAlliesMod.GetModifiedValue(this.m_healingToAllies), false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		this.GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetBaseSelfHeal() + this.GetSelfHealAmountPerHit());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetHealingToAllies());
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamageToEnemies());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (this.GetSelfHealAmountPerHit() <= 0 && this.GetBaseSelfHeal() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
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
				List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
				{
					if (visibleActorsInRangeByTooltipSubject[i].GetTeam() != targetActor.GetTeam())
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
						num++;
					}
					else if (visibleActorsInRangeByTooltipSubject[i] != targetActor)
					{
						num2++;
					}
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				int value = this.CalcSelfHealAmountFromHits(num2, num);
				dictionary[AbilityTooltipSymbol.Healing] = value;
			}
		}
		return dictionary;
	}

	private int CalcSelfHealAmountFromHits(int allyHits, int enemyHits)
	{
		int result = 0;
		if (this.GetSelfHealAmountPerHit() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.CalcSelfHealAmountFromHits(int, int)).MethodHandle;
			}
			if (this.GetBaseSelfHeal() <= 0)
			{
				return result;
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
		}
		int num = 0;
		if (this.SelfHealCountAllyHit())
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
			num += allyHits;
		}
		if (this.SelfHealCountEnemyHit())
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
			num += enemyHits;
		}
		result = this.GetBaseSelfHeal() + num * this.GetSelfHealAmountPerHit();
		return result;
	}

	public bool IncludeCaster()
	{
		if (!this.ModdedIncludeCaster())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.IncludeCaster()).MethodHandle;
			}
			if (this.GetSelfHealAmountPerHit() <= 0)
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
				return this.GetBaseSelfHeal() > 0;
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
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.IncludeAllies()).MethodHandle;
			}
			result = (this.GetHealingToAllies() > 0);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IncludeEnemies()
	{
		return this.GetEnemyHitEffect().m_applyEffect || this.GetDamageToEnemies() > 0;
	}

	public AbilityAreaShape GetShape()
	{
		AbilityAreaShape result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.GetShape()).MethodHandle;
			}
			result = this.m_abilityMod.m_shapeMod.GetModifiedValue(this.m_shape);
		}
		else
		{
			result = this.m_shape;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.PenetrateLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
		}
		else
		{
			result = this.m_penetrateLos;
		}
		return result;
	}

	public int GetBaseSelfHeal()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.GetBaseSelfHeal()).MethodHandle;
			}
			result = this.m_abilityMod.m_baseSelfHealMod.GetModifiedValue(this.m_baseSelfHeal);
		}
		else
		{
			result = this.m_baseSelfHeal;
		}
		return result;
	}

	public int GetSelfHealAmountPerHit()
	{
		return (!this.m_abilityMod) ? this.m_selfHealAmountPerHit : this.m_abilityMod.m_selfHealAmountPerHitMod.GetModifiedValue(this.m_selfHealAmountPerHit);
	}

	public bool SelfHealCountEnemyHit()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.SelfHealCountEnemyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfHealCountEnemyHitMod.GetModifiedValue(this.m_selfHealCountEnemyHit);
		}
		else
		{
			result = this.m_selfHealCountEnemyHit;
		}
		return result;
	}

	public bool SelfHealCountAllyHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.SelfHealCountAllyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfHealCountAllyHitMod.GetModifiedValue(this.m_selfHealCountAllyHit);
		}
		else
		{
			result = this.m_selfHealCountAllyHit;
		}
		return result;
	}

	public bool ModdedIncludeCaster()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.ModdedIncludeCaster()).MethodHandle;
			}
			result = this.m_abilityMod.m_includeCasterMod.GetModifiedValue(this.m_includeCaster);
		}
		else
		{
			result = this.m_includeCaster;
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
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.GetSelfHitEffect()).MethodHandle;
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
		StandardEffectInfo result;
		if (this.m_cachedAllyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.GetAllyHitEffect()).MethodHandle;
			}
			result = this.m_cachedAllyHitEffect;
		}
		else
		{
			result = this.m_allyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return (this.m_cachedEnemyHitEffect == null) ? this.m_enemyHitEffect : this.m_cachedEnemyHitEffect;
	}

	public int GetDamageToEnemies()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.GetDamageToEnemies()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageToEnemiesMod.GetModifiedValue(this.m_damageToEnemies);
		}
		else
		{
			result = this.m_damageToEnemies;
		}
		return result;
	}

	public int GetHealingToAllies()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAoeBuffDebuff.GetHealingToAllies()).MethodHandle;
			}
			result = this.m_abilityMod.m_healingToAlliesMod.GetModifiedValue(this.m_healingToAllies);
		}
		else
		{
			result = this.m_healingToAllies;
		}
		return result;
	}
}
