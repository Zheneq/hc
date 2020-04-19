using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalBite : Ability
{
	public bool m_penetrateLineOfSight;

	public int m_damageAmount = 0x14;

	public float m_width = 1f;

	public float m_distance = 2f;

	public int m_maxTargets = 1;

	public float m_lifeOnFirstHit;

	public float m_lifePerHit;

	private AbilityMod_RobotAnimalBite m_abilityMod;

	private RobotAnimal_SyncComponent m_syncComp;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalBite.Start()).MethodHandle;
			}
			this.m_abilityName = "Megabyte";
		}
		this.m_syncComp = base.GetComponent<RobotAnimal_SyncComponent>();
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(this, this.m_width, this.m_distance, this.m_penetrateLineOfSight, this.m_maxTargets, false, true);
		abilityUtil_Targeter_Laser.m_affectCasterDelegate = ((ActorData caster, List<ActorData> actorsSoFar) => actorsSoFar.Count > 0);
		base.Targeter = abilityUtil_Targeter_Laser;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.m_distance;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.m_damageAmount),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, Mathf.RoundToInt(this.m_lifeOnFirstHit)),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, Mathf.RoundToInt(this.m_lifePerHit))
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalBite.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
				}
				List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
				int lifeGainAmount = this.GetLifeGainAmount(visibleActorsInRangeByTooltipSubject.Count);
				dictionary[AbilityTooltipSymbol.Healing] = Mathf.RoundToInt((float)lifeGainAmount);
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
				int num = 0;
				dictionary[AbilityTooltipSymbol.Damage] = this.ModdedDamage(false, ref num);
			}
			return dictionary;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RobotAnimalBite abilityMod_RobotAnimalBite = modAsBase as AbilityMod_RobotAnimalBite;
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_RobotAnimalBite) ? this.m_damageAmount : abilityMod_RobotAnimalBite.m_damageMod.GetModifiedValue(this.m_damageAmount), false);
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "LifeFirstHit", string.Empty, Mathf.RoundToInt(this.m_lifeOnFirstHit), false);
		base.AddTokenInt(tokens, "LifePerHit", string.Empty, Mathf.RoundToInt(this.m_lifePerHit), false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RobotAnimalBite))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalBite.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_RobotAnimalBite);
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
	}

	public float ModdedLifeOnFirstHit()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_lifeOnFirstHitMod.GetModifiedValue(this.m_lifeOnFirstHit) : this.m_lifeOnFirstHit;
	}

	public float ModdedLifePerHit()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_lifePerHitMod.GetModifiedValue(this.m_lifePerHit) : this.m_lifePerHit;
	}

	public unsafe int ModdedDamage(bool includeVariance, ref int damageToSelf)
	{
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalBite.ModdedDamage(bool, int*)).MethodHandle;
			}
			int num = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damageAmount);
			if (this.m_syncComp != null)
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
				if (this.m_abilityMod.m_extraDamageOnConsecutiveCast > 0)
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
					if (this.m_syncComp.m_biteLastCastTurn > 0 && GameFlowData.Get().CurrentTurn - this.m_syncComp.m_biteLastCastTurn == 1)
					{
						num += this.m_abilityMod.m_extraDamageOnConsecutiveCast;
					}
				}
				if (this.m_abilityMod.m_extraDamageOnConsecutiveHit > 0)
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
					if (this.m_syncComp.m_biteLastHitTurn > 0 && GameFlowData.Get().CurrentTurn - this.m_syncComp.m_biteLastHitTurn == 1)
					{
						num += this.m_abilityMod.m_extraDamageOnConsecutiveHit;
					}
				}
			}
			if (includeVariance)
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
				if (this.m_abilityMod.m_varianceExtraDamageMin >= 0)
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
					if (this.m_abilityMod.m_varianceExtraDamageMax - this.m_abilityMod.m_varianceExtraDamageMin > 0)
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
						int num2 = GameplayRandom.Range(this.m_abilityMod.m_varianceExtraDamageMin, this.m_abilityMod.m_varianceExtraDamageMax);
						num += num2;
						damageToSelf = Mathf.RoundToInt((float)num2 * this.m_abilityMod.m_varianceExtraDamageToSelf);
					}
				}
			}
			return num;
		}
		return this.m_damageAmount;
	}

	public bool HasEffectOnNextTurnStart()
	{
		return !(this.m_abilityMod == null) && this.m_abilityMod.m_perAdjacentEnemyEffectOnSelfNextTurn.m_applyEffect;
	}

	public StandardEffectInfo EffectInfoOnNextTurnStart()
	{
		StandardEffectInfo result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalBite.EffectInfoOnNextTurnStart()).MethodHandle;
			}
			result = new StandardEffectInfo();
		}
		else
		{
			result = this.m_abilityMod.m_perAdjacentEnemyEffectOnSelfNextTurn;
		}
		return result;
	}

	public int GetLifeGainAmount(int hitCount)
	{
		float num = 0f;
		if (hitCount > 0 && this.ModdedLifeOnFirstHit() != 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalBite.GetLifeGainAmount(int)).MethodHandle;
			}
			num += this.ModdedLifeOnFirstHit();
		}
		if (this.ModdedLifePerHit() != 0f)
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
			num += this.ModdedLifePerHit() * (float)hitCount;
		}
		return Mathf.RoundToInt(num);
	}
}
