using System;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreAoeBuffDebuff : Ability
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
	public StandardEffectInfo m_selfHitEffect;

	public StandardEffectInfo m_allyHitEffect;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Energy Gain/Loss for hit actors")]
	public int m_allyEnergyGain;

	public int m_enemyEnergyLoss;

	public bool m_energyChangeOnlyIfHasAdjacent;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ClaymoreAoeBuffDebuff m_abilityMod;

	private StandardEffectInfo m_cachedSelfHitEffect;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.Start()).MethodHandle;
			}
			this.m_abilityName = "Thundering Roar";
		}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.SetCachedFields()).MethodHandle;
			}
			cachedSelfHitEffect = this.m_abilityMod.m_selfHitEffectMod.GetModifiedValue(this.m_selfHitEffect);
		}
		else
		{
			cachedSelfHitEffect = this.m_selfHitEffect;
		}
		this.m_cachedSelfHitEffect = cachedSelfHitEffect;
		StandardEffectInfo cachedAllyHitEffect;
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
				switch (3)
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

	public AbilityAreaShape GetShape()
	{
		return (!this.m_abilityMod) ? this.m_shape : this.m_abilityMod.m_shapeMod.GetModifiedValue(this.m_shape);
	}

	public bool GetPenetrateLos()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.GetPenetrateLos()).MethodHandle;
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
		return (!this.m_abilityMod) ? this.m_baseSelfHeal : this.m_abilityMod.m_baseSelfHealMod.GetModifiedValue(this.m_baseSelfHeal);
	}

	public int GetSelfHealAmountPerHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.GetSelfHealAmountPerHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfHealAmountPerHitMod.GetModifiedValue(this.m_selfHealAmountPerHit);
		}
		else
		{
			result = this.m_selfHealAmountPerHit;
		}
		return result;
	}

	public bool GetSelfHealCountEnemyHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.GetSelfHealCountEnemyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfHealCountEnemyHitMod.GetModifiedValue(this.m_selfHealCountEnemyHit);
		}
		else
		{
			result = this.m_selfHealCountEnemyHit;
		}
		return result;
	}

	public bool GetSelfHealCountAllyHit()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.GetSelfHealCountAllyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfHealCountAllyHitMod.GetModifiedValue(this.m_selfHealCountAllyHit);
		}
		else
		{
			result = this.m_selfHealCountAllyHit;
		}
		return result;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		return (this.m_cachedSelfHitEffect == null) ? this.m_selfHitEffect : this.m_cachedSelfHitEffect;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.GetAllyHitEffect()).MethodHandle;
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
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.GetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public int GetAllyEnergyGain()
	{
		return (!this.m_abilityMod) ? this.m_allyEnergyGain : this.m_abilityMod.m_allyEnergyGainMod.GetModifiedValue(this.m_allyEnergyGain);
	}

	public int GetEnemyEnergyLoss()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.GetEnemyEnergyLoss()).MethodHandle;
			}
			result = this.m_abilityMod.m_enemyEnergyLossMod.GetModifiedValue(this.m_enemyEnergyLoss);
		}
		else
		{
			result = this.m_enemyEnergyLoss;
		}
		return result;
	}

	public bool GetEnergyChangeOnlyIfHasAdjacent()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.GetEnergyChangeOnlyIfHasAdjacent()).MethodHandle;
			}
			result = this.m_abilityMod.m_energyChangeOnlyIfHasAdjacentMod.GetModifiedValue(this.m_energyChangeOnlyIfHasAdjacent);
		}
		else
		{
			result = this.m_energyChangeOnlyIfHasAdjacent;
		}
		return result;
	}

	public bool IncludeCaster()
	{
		bool result;
		if (!this.GetSelfHitEffect().m_applyEffect && this.GetBaseSelfHeal() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.IncludeCaster()).MethodHandle;
			}
			result = (this.GetSelfHealAmountPerHit() > 0);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IncludeAllies()
	{
		bool result;
		if (!this.GetAllyHitEffect().m_applyEffect)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.IncludeAllies()).MethodHandle;
			}
			result = (this.GetAllyEnergyGain() > 0);
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
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.IncludeEnemies()).MethodHandle;
			}
			result = (this.GetEnemyEnergyLoss() > 0);
		}
		else
		{
			result = true;
		}
		return result;
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
		if (!this.IncludeCaster())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.SetupTargeter()).MethodHandle;
			}
			affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
		}
		base.Targeter = new AbilityUtil_Targeter_Shape(this, this.GetShape(), this.GetPenetrateLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, this.IncludeEnemies(), this.IncludeAllies(), affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreAoeBuffDebuff abilityMod_ClaymoreAoeBuffDebuff = modAsBase as AbilityMod_ClaymoreAoeBuffDebuff;
		string name = "BaseSelfHeal";
		string empty = string.Empty;
		int val;
		if (abilityMod_ClaymoreAoeBuffDebuff)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_ClaymoreAoeBuffDebuff.m_baseSelfHealMod.GetModifiedValue(this.m_baseSelfHeal);
		}
		else
		{
			val = this.m_baseSelfHeal;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "SelfHealAmountPerHit", string.Empty, (!abilityMod_ClaymoreAoeBuffDebuff) ? this.m_selfHealAmountPerHit : abilityMod_ClaymoreAoeBuffDebuff.m_selfHealAmountPerHitMod.GetModifiedValue(this.m_selfHealAmountPerHit), false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_ClaymoreAoeBuffDebuff) ? this.m_selfHitEffect : abilityMod_ClaymoreAoeBuffDebuff.m_selfHitEffectMod.GetModifiedValue(this.m_selfHitEffect), "SelfHitEffect", this.m_selfHitEffect, true);
		StandardEffectInfo effectInfo;
		if (abilityMod_ClaymoreAoeBuffDebuff)
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
			effectInfo = abilityMod_ClaymoreAoeBuffDebuff.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			effectInfo = this.m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "AllyHitEffect", this.m_allyHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_ClaymoreAoeBuffDebuff) ? this.m_enemyHitEffect : abilityMod_ClaymoreAoeBuffDebuff.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect), "EnemyHitEffect", this.m_enemyHitEffect, true);
		base.AddTokenInt(tokens, "AllyEnergyGain", string.Empty, (!abilityMod_ClaymoreAoeBuffDebuff) ? this.m_allyEnergyGain : abilityMod_ClaymoreAoeBuffDebuff.m_allyEnergyGainMod.GetModifiedValue(this.m_allyEnergyGain), false);
		string name2 = "EnemyEnergyLoss";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ClaymoreAoeBuffDebuff)
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
			val2 = abilityMod_ClaymoreAoeBuffDebuff.m_enemyEnergyLossMod.GetModifiedValue(this.m_enemyEnergyLoss);
		}
		else
		{
			val2 = this.m_enemyEnergyLoss;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		this.GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetBaseSelfHeal() + this.GetSelfHealAmountPerHit());
		AbilityTooltipHelper.ReportEnergy(ref result, AbilityTooltipSubject.Ally, this.GetAllyEnergyGain());
		AbilityTooltipHelper.ReportEnergy(ref result, AbilityTooltipSubject.Enemy, -1 * this.GetEnemyEnergyLoss());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (this.GetSelfHealAmountPerHit() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			if (this.GetBaseSelfHeal() <= 0)
			{
				return null;
			}
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
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
				List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
				{
					if (visibleActorsInRangeByTooltipSubject[i].\u000E() != targetActor.\u000E())
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
						num++;
					}
					else if (visibleActorsInRangeByTooltipSubject[i] != targetActor)
					{
						num2++;
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
				int value = this.CalcSelfHealAmountFromHits(num2, num);
				dictionary[AbilityTooltipSymbol.Healing] = value;
			}
			else if (this.GetEnergyChangeOnlyIfHasAdjacent())
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
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
					if (this.GetEnemyEnergyLoss() > 0)
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
						Dictionary<AbilityTooltipSymbol, int> dictionary2 = dictionary;
						AbilityTooltipSymbol key = AbilityTooltipSymbol.Energy;
						int value2;
						if (AreaEffectUtils.HasAdjacentActorOfTeam(targetActor, targetActor.\u0012()))
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
							value2 = -1 * this.GetEnemyEnergyLoss();
						}
						else
						{
							value2 = 0;
						}
						dictionary2[key] = value2;
						return dictionary;
					}
				}
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
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
					if (this.GetAllyEnergyGain() > 0)
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
						dictionary[AbilityTooltipSymbol.Energy] = ((!AreaEffectUtils.HasAdjacentActorOfTeam(targetActor, targetActor.\u0012())) ? 0 : this.GetAllyEnergyGain());
					}
				}
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
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.CalcSelfHealAmountFromHits(int, int)).MethodHandle;
			}
			if (this.GetBaseSelfHeal() <= 0)
			{
				return result;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		int num = 0;
		if (this.GetSelfHealCountAllyHit())
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
			num += allyHits;
		}
		if (this.GetSelfHealCountEnemyHit())
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
			num += enemyHits;
		}
		result = this.GetBaseSelfHeal() + num * this.GetSelfHealAmountPerHit();
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClaymoreAoeBuffDebuff))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreAoeBuffDebuff.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ClaymoreAoeBuffDebuff);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
