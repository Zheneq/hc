using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalRoar : Ability
{
	public enum TargetingMode
	{
		Shape,
		Radius
	}

	public TargetingMode m_targetingMode;

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
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		bool flag = AffectEnemies();
		bool flag2 = AffectAllies();
		if (m_targetingMode == TargetingMode.Shape)
		{
			while (true)
			{
				AbilityUtil_Targeter.AffectsActor affectsCaster;
				switch (1)
				{
				case 0:
					break;
				default:
					{
						if (!m_selfEffect.m_applyEffect)
						{
							if (!HasSelfEffectFromBaseMod())
							{
								if (m_allyEffect_includingMe.m_applyEffect)
								{
									affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
								}
								else
								{
									affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
								}
								goto IL_0070;
							}
						}
						affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
						goto IL_0070;
					}
					IL_0070:
					if (UseInnerShape())
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
							{
								List<AbilityAreaShape> list = new List<AbilityAreaShape>();
								list.Add(GetInnerShape());
								list.Add(GetTargetingShape());
								List<AbilityTooltipSubject> list2 = new List<AbilityTooltipSubject>();
								list2.Add(AbilityTooltipSubject.Secondary);
								list2.Add(AbilityTooltipSubject.Primary);
								List<AbilityTooltipSubject> subjects = list2;
								base.Targeter = new AbilityUtil_Targeter_MultipleShapes(this, list, subjects, GetPenetrateLos(), flag, flag2, m_selfEffect.m_applyEffect);
								return;
							}
							}
						}
					}
					base.Targeter = new AbilityUtil_Targeter_Shape(this, GetTargetingShape(), GetPenetrateLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, flag, flag2, affectsCaster);
					(base.Targeter as AbilityUtil_Targeter_Shape).SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Ally);
					return;
				}
			}
		}
		base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, GetTargetingRadius(), GetPenetrateLos(), flag, flag2);
		if (!m_selfEffect.m_applyEffect)
		{
			if (!HasSelfEffectFromBaseMod())
			{
				return;
			}
		}
		base.Targeter.SetAffectedGroups(flag, flag2, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (GetDamageAmount() != 0)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		}
		if (UseInnerShape())
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetInnerShapeDamage());
		}
		m_enemyEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		m_allyEffect_includingMe.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		m_allyEffect_excludingMe.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		int moddedHealingForAllies = GetModdedHealingForAllies();
		if (moddedHealingForAllies != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, moddedHealingForAllies));
		}
		int moddedTechPointGainForAllies = GetModdedTechPointGainForAllies();
		if (moddedTechPointGainForAllies != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Ally, moddedTechPointGainForAllies));
		}
		m_selfEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		AppendTooltipNumbersFromBaseModEffects(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (m_targetingMode == TargetingMode.Shape)
		{
			if (UseInnerShape())
			{
				List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
				if (tooltipSubjectTypes != null)
				{
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					bool flag = tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary);
					if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
					{
						Dictionary<AbilityTooltipSymbol, int> dictionary2 = dictionary;
						int value;
						if (flag)
						{
							value = GetDamageAmount();
						}
						else
						{
							value = GetInnerShapeDamage();
						}
						dictionary2[AbilityTooltipSymbol.Damage] = value;
					}
					else
					{
						dictionary[AbilityTooltipSymbol.Damage] = 0;
					}
				}
			}
		}
		else if (GetInnerRadius() > 0f)
		{
			List<AbilityTooltipSubject> tooltipSubjectTypes2 = base.Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
			ActorData actorData = base.ActorData;
			if (actorData != null)
			{
				if (targetActor.GetCurrentBoardSquare() != null)
				{
					if (tooltipSubjectTypes2 != null && tooltipSubjectTypes2.Contains(AbilityTooltipSubject.Enemy))
					{
						dictionary = new Dictionary<AbilityTooltipSymbol, int>();
						bool flag2 = AreaEffectUtils.IsSquareInConeByActorRadius(targetActor.GetCurrentBoardSquare(), actorData.GetTravelBoardSquareWorldPosition(), 0f, 360f, GetInnerRadius(), 0f, GetPenetrateLos(), actorData);
						Dictionary<AbilityTooltipSymbol, int> dictionary3 = dictionary;
						int value2;
						if (flag2)
						{
							value2 = GetInnerShapeDamage();
						}
						else
						{
							value2 = GetDamageAmount();
						}
						dictionary3[AbilityTooltipSymbol.Damage] = value2;
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RobotAnimalRoar abilityMod_RobotAnimalRoar = modAsBase as AbilityMod_RobotAnimalRoar;
		AbilityMod.AddToken_EffectInfo(tokens, m_allyEffect_includingMe, "AllyEffect_includingMe", m_allyEffect_includingMe);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyEffect_excludingMe, "AllyEffect_excludingMe", m_allyEffect_excludingMe);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyEffect, "EnemyEffect", m_enemyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_selfEffect, "SelfEffect", m_selfEffect);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_RobotAnimalRoar)
		{
			val = abilityMod_RobotAnimalRoar.m_damageMod.GetModifiedValue(m_damage);
		}
		else
		{
			val = m_damage;
		}
		AddTokenInt(tokens, "Damage", empty, val);
		int input = (m_innerShapeDamage >= 0) ? m_innerShapeDamage : m_damage;
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_RobotAnimalRoar)
		{
			val2 = abilityMod_RobotAnimalRoar.m_innerShapeDamageMod.GetModifiedValue(input);
		}
		else
		{
			val2 = m_innerShapeDamage;
		}
		AddTokenInt(tokens, "InnerShapeDamage", empty2, val2);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RobotAnimalRoar))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_RobotAnimalRoar);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public AbilityAreaShape GetTargetingShape()
	{
		AbilityAreaShape result;
		if (m_abilityMod == null)
		{
			result = m_aoeShape;
		}
		else
		{
			result = m_abilityMod.m_shapeMod.GetModifiedValue(m_aoeShape);
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffectInfo()
	{
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_enemyHitEffectOverride.m_applyEffect)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return m_abilityMod.m_enemyHitEffectOverride;
					}
				}
			}
		}
		return m_enemyEffect;
	}

	public bool GetPenetrateLos()
	{
		bool result;
		if (m_abilityMod == null)
		{
			result = m_penetrateLineOfSight;
		}
		else
		{
			result = m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLineOfSight);
		}
		return result;
	}

	public int GetTechPointDamage()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_techPointDamageMod.GetModifiedValue(0) : 0;
	}

	public int GetDamageAmount()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_damage;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damage);
		}
		return result;
	}

	public int GetModdedHealingForAllies()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_healAmountToTargetAllyOnHit;
		}
		return result;
	}

	public int GetModdedTechPointGainForAllies()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_techPointGainToTargetAllyOnHit : 0;
	}

	public bool UseInnerShape()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_useInnerShapeMod.GetModifiedValue(m_useInnerShape);
		}
		else
		{
			result = m_useInnerShape;
		}
		return result;
	}

	public AbilityAreaShape GetInnerShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_innerShapeMod.GetModifiedValue(m_innerShape);
		}
		else
		{
			result = m_innerShape;
		}
		return result;
	}

	public int GetInnerShapeDamage()
	{
		int num;
		if (m_innerShapeDamage < 0)
		{
			num = m_damage;
		}
		else
		{
			num = m_innerShapeDamage;
		}
		int num2 = num;
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_innerShapeDamageMod.GetModifiedValue(num2);
		}
		else
		{
			result = num2;
		}
		return result;
	}

	public float GetTargetingRadius()
	{
		return (!m_abilityMod) ? m_targetingRadius : m_abilityMod.m_targetingRadiusMod.GetModifiedValue(m_targetingRadius);
	}

	public float GetInnerRadius()
	{
		return (!m_abilityMod) ? m_innerRadius : m_abilityMod.m_innerRadiusMod.GetModifiedValue(m_innerRadius);
	}

	public bool AffectAllies()
	{
		StandardEffectInfo moddedEffectForAllies = GetModdedEffectForAllies();
		int result;
		if (!m_allyEffect_excludingMe.m_applyEffect)
		{
			if (!m_allyEffect_includingMe.m_applyEffect)
			{
				if ((moddedEffectForAllies == null || !moddedEffectForAllies.m_applyEffect) && GetModdedHealingForAllies() == 0)
				{
					result = ((GetModdedTechPointGainForAllies() != 0) ? 1 : 0);
					goto IL_006e;
				}
			}
		}
		result = 1;
		goto IL_006e;
		IL_006e:
		return (byte)result != 0;
	}

	public bool AffectEnemies()
	{
		int result;
		if (GetDamageAmount() <= 0)
		{
			if (GetTechPointDamage() <= 0)
			{
				result = (GetEnemyHitEffectInfo().m_applyEffect ? 1 : 0);
				goto IL_0041;
			}
		}
		result = 1;
		goto IL_0041;
		IL_0041:
		return (byte)result != 0;
	}
}
