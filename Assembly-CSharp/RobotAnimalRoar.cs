using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalRoar : Ability
{
	public RobotAnimalRoar.TargetingMode m_targetingMode;

	public bool m_penetrateLineOfSight;

	[Header("-- Targeting: Shape")]
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Seven_x_Seven;

	[Header("-- Inner shape for different damage --")]
	public bool m_useInnerShape;

	public AbilityAreaShape m_innerShape = AbilityAreaShape.Three_x_Three_NoCorners;

	[Header("-- Targeting: Radius")]
	public float m_targetingRadius = 4.49f;

	public float m_innerRadius = -1f;

	public StandardEffectInfo m_allyEffect_includingMe;

	public StandardEffectInfo m_allyEffect_excludingMe;

	public StandardEffectInfo m_enemyEffect;

	public StandardEffectInfo m_selfEffect;

	[Header(" Damage, Inner Shape Damage also used for Radius targeting")]
	public int m_damage;

	public int m_innerShapeDamage = -1;

	private AbilityMod_RobotAnimalRoar m_abilityMod;

	private void Start()
	{
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		bool flag = this.AffectEnemies();
		bool flag2 = this.AffectAllies();
		if (this.m_targetingMode == RobotAnimalRoar.TargetingMode.Shape)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.SetupTargeter()).MethodHandle;
			}
			AbilityUtil_Targeter.AffectsActor affectsCaster;
			if (!this.m_selfEffect.m_applyEffect)
			{
				if (base.HasSelfEffectFromBaseMod())
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
				}
				else
				{
					if (this.m_allyEffect_includingMe.m_applyEffect)
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
						affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
						goto IL_70;
					}
					affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
					goto IL_70;
				}
			}
			affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
			IL_70:
			if (this.UseInnerShape())
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
				List<AbilityAreaShape> list = new List<AbilityAreaShape>();
				list.Add(this.GetInnerShape());
				list.Add(this.GetTargetingShape());
				List<AbilityTooltipSubject> subjects = new List<AbilityTooltipSubject>
				{
					AbilityTooltipSubject.Secondary,
					AbilityTooltipSubject.Primary
				};
				base.Targeter = new AbilityUtil_Targeter_MultipleShapes(this, list, subjects, this.GetPenetrateLos(), flag, flag2, this.m_selfEffect.m_applyEffect);
			}
			else
			{
				base.Targeter = new AbilityUtil_Targeter_Shape(this, this.GetTargetingShape(), this.GetPenetrateLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, flag, flag2, affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
				(base.Targeter as AbilityUtil_Targeter_Shape).SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Ally, AbilityTooltipSubject.None);
			}
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, this.GetTargetingRadius(), this.GetPenetrateLos(), flag, flag2, -1);
			if (!this.m_selfEffect.m_applyEffect)
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
				if (!base.HasSelfEffectFromBaseMod())
				{
					return;
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
			base.Targeter.SetAffectedGroups(flag, flag2, true);
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		if (this.GetDamageAmount() != 0)
		{
			AbilityTooltipHelper.ReportDamage(ref list, AbilityTooltipSubject.Primary, this.GetDamageAmount());
		}
		if (this.UseInnerShape())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			AbilityTooltipHelper.ReportDamage(ref list, AbilityTooltipSubject.Secondary, this.GetInnerShapeDamage());
		}
		this.m_enemyEffect.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Enemy);
		this.m_allyEffect_includingMe.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Ally);
		this.m_allyEffect_excludingMe.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Ally);
		int moddedHealingForAllies = this.GetModdedHealingForAllies();
		if (moddedHealingForAllies != 0)
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
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, moddedHealingForAllies));
		}
		int moddedTechPointGainForAllies = this.GetModdedTechPointGainForAllies();
		if (moddedTechPointGainForAllies != 0)
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
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Ally, moddedTechPointGainForAllies));
		}
		this.m_selfEffect.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Self);
		base.AppendTooltipNumbersFromBaseModEffects(ref list, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (this.m_targetingMode == RobotAnimalRoar.TargetingMode.Shape)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			if (this.UseInnerShape())
			{
				List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
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
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					bool flag = tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary);
					if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
					{
						Dictionary<AbilityTooltipSymbol, int> dictionary2 = dictionary;
						AbilityTooltipSymbol key = AbilityTooltipSymbol.Damage;
						int value;
						if (flag)
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
							value = this.GetDamageAmount();
						}
						else
						{
							value = this.GetInnerShapeDamage();
						}
						dictionary2[key] = value;
					}
					else
					{
						dictionary[AbilityTooltipSymbol.Damage] = 0;
					}
				}
			}
		}
		else if (this.GetInnerRadius() > 0f)
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
			List<AbilityTooltipSubject> tooltipSubjectTypes2 = base.Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
			ActorData actorData = base.ActorData;
			if (actorData != null)
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
				if (targetActor.\u0012() != null)
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
					if (tooltipSubjectTypes2 != null && tooltipSubjectTypes2.Contains(AbilityTooltipSubject.Enemy))
					{
						dictionary = new Dictionary<AbilityTooltipSymbol, int>();
						bool flag2 = AreaEffectUtils.IsSquareInConeByActorRadius(targetActor.\u0012(), actorData.\u0016(), 0f, 360f, this.GetInnerRadius(), 0f, this.GetPenetrateLos(), actorData, false, default(Vector3));
						Dictionary<AbilityTooltipSymbol, int> dictionary3 = dictionary;
						AbilityTooltipSymbol key2 = AbilityTooltipSymbol.Damage;
						int value2;
						if (flag2)
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
							value2 = this.GetInnerShapeDamage();
						}
						else
						{
							value2 = this.GetDamageAmount();
						}
						dictionary3[key2] = value2;
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RobotAnimalRoar abilityMod_RobotAnimalRoar = modAsBase as AbilityMod_RobotAnimalRoar;
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyEffect_includingMe, "AllyEffect_includingMe", this.m_allyEffect_includingMe, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyEffect_excludingMe, "AllyEffect_excludingMe", this.m_allyEffect_excludingMe, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyEffect, "EnemyEffect", this.m_enemyEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_selfEffect, "SelfEffect", this.m_selfEffect, true);
		string name = "Damage";
		string empty = string.Empty;
		int val;
		if (abilityMod_RobotAnimalRoar)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_RobotAnimalRoar.m_damageMod.GetModifiedValue(this.m_damage);
		}
		else
		{
			val = this.m_damage;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		int input = (this.m_innerShapeDamage >= 0) ? this.m_innerShapeDamage : this.m_damage;
		string name2 = "InnerShapeDamage";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_RobotAnimalRoar)
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
			val2 = abilityMod_RobotAnimalRoar.m_innerShapeDamageMod.GetModifiedValue(input);
		}
		else
		{
			val2 = this.m_innerShapeDamage;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RobotAnimalRoar))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_RobotAnimalRoar);
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

	public AbilityAreaShape GetTargetingShape()
	{
		AbilityAreaShape result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.GetTargetingShape()).MethodHandle;
			}
			result = this.m_aoeShape;
		}
		else
		{
			result = this.m_abilityMod.m_shapeMod.GetModifiedValue(this.m_aoeShape);
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffectInfo()
	{
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.GetEnemyHitEffectInfo()).MethodHandle;
			}
			if (this.m_abilityMod.m_enemyHitEffectOverride.m_applyEffect)
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
				return this.m_abilityMod.m_enemyHitEffectOverride;
			}
		}
		return this.m_enemyEffect;
	}

	public bool GetPenetrateLos()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.GetPenetrateLos()).MethodHandle;
			}
			result = this.m_penetrateLineOfSight;
		}
		else
		{
			result = this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLineOfSight);
		}
		return result;
	}

	public int GetTechPointDamage()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_techPointDamageMod.GetModifiedValue(0) : 0;
	}

	public int GetDamageAmount()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.GetDamageAmount()).MethodHandle;
			}
			result = this.m_damage;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damage);
		}
		return result;
	}

	public int GetModdedHealingForAllies()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.GetModdedHealingForAllies()).MethodHandle;
			}
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_healAmountToTargetAllyOnHit;
		}
		return result;
	}

	public int GetModdedTechPointGainForAllies()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_techPointGainToTargetAllyOnHit : 0;
	}

	public bool UseInnerShape()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.UseInnerShape()).MethodHandle;
			}
			result = this.m_abilityMod.m_useInnerShapeMod.GetModifiedValue(this.m_useInnerShape);
		}
		else
		{
			result = this.m_useInnerShape;
		}
		return result;
	}

	public AbilityAreaShape GetInnerShape()
	{
		AbilityAreaShape result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.GetInnerShape()).MethodHandle;
			}
			result = this.m_abilityMod.m_innerShapeMod.GetModifiedValue(this.m_innerShape);
		}
		else
		{
			result = this.m_innerShape;
		}
		return result;
	}

	public int GetInnerShapeDamage()
	{
		int num;
		if (this.m_innerShapeDamage < 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.GetInnerShapeDamage()).MethodHandle;
			}
			num = this.m_damage;
		}
		else
		{
			num = this.m_innerShapeDamage;
		}
		int num2 = num;
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
			result = this.m_abilityMod.m_innerShapeDamageMod.GetModifiedValue(num2);
		}
		else
		{
			result = num2;
		}
		return result;
	}

	public float GetTargetingRadius()
	{
		return (!this.m_abilityMod) ? this.m_targetingRadius : this.m_abilityMod.m_targetingRadiusMod.GetModifiedValue(this.m_targetingRadius);
	}

	public float GetInnerRadius()
	{
		return (!this.m_abilityMod) ? this.m_innerRadius : this.m_abilityMod.m_innerRadiusMod.GetModifiedValue(this.m_innerRadius);
	}

	public bool AffectAllies()
	{
		StandardEffectInfo moddedEffectForAllies = base.GetModdedEffectForAllies();
		if (!this.m_allyEffect_excludingMe.m_applyEffect)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.AffectAllies()).MethodHandle;
			}
			if (!this.m_allyEffect_includingMe.m_applyEffect)
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
				if ((moddedEffectForAllies == null || !moddedEffectForAllies.m_applyEffect) && this.GetModdedHealingForAllies() == 0)
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
					return this.GetModdedTechPointGainForAllies() != 0;
				}
			}
		}
		return true;
	}

	public bool AffectEnemies()
	{
		if (this.GetDamageAmount() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalRoar.AffectEnemies()).MethodHandle;
			}
			if (this.GetTechPointDamage() <= 0)
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
				return this.GetEnemyHitEffectInfo().m_applyEffect;
			}
		}
		return true;
	}

	public enum TargetingMode
	{
		Shape,
		Radius
	}
}
