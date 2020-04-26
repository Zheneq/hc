using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalBite : Ability
{
	public bool m_penetrateLineOfSight;

	public int m_damageAmount = 20;

	public float m_width = 1f;

	public float m_distance = 2f;

	public int m_maxTargets = 1;

	public float m_lifeOnFirstHit;

	public float m_lifePerHit;

	private AbilityMod_RobotAnimalBite m_abilityMod;

	private RobotAnimal_SyncComponent m_syncComp;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Megabyte";
		}
		m_syncComp = GetComponent<RobotAnimal_SyncComponent>();
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(this, m_width, m_distance, m_penetrateLineOfSight, m_maxTargets, false, true);
		abilityUtil_Targeter_Laser.m_affectCasterDelegate = ((ActorData caster, List<ActorData> actorsSoFar) => actorsSoFar.Count > 0);
		base.Targeter = abilityUtil_Targeter_Laser;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return m_distance;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damageAmount));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, Mathf.RoundToInt(m_lifeOnFirstHit)));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, Mathf.RoundToInt(m_lifePerHit)));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
				int lifeGainAmount = GetLifeGainAmount(visibleActorsInRangeByTooltipSubject.Count);
				dictionary[AbilityTooltipSymbol.Healing] = Mathf.RoundToInt(lifeGainAmount);
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				int damageToSelf = 0;
				dictionary[AbilityTooltipSymbol.Damage] = ModdedDamage(false, ref damageToSelf);
			}
			return dictionary;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RobotAnimalBite abilityMod_RobotAnimalBite = modAsBase as AbilityMod_RobotAnimalBite;
		AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_RobotAnimalBite) ? m_damageAmount : abilityMod_RobotAnimalBite.m_damageMod.GetModifiedValue(m_damageAmount));
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "LifeFirstHit", string.Empty, Mathf.RoundToInt(m_lifeOnFirstHit));
		AddTokenInt(tokens, "LifePerHit", string.Empty, Mathf.RoundToInt(m_lifePerHit));
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RobotAnimalBite))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_RobotAnimalBite);
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
	}

	public float ModdedLifeOnFirstHit()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_lifeOnFirstHitMod.GetModifiedValue(m_lifeOnFirstHit) : m_lifeOnFirstHit;
	}

	public float ModdedLifePerHit()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_lifePerHitMod.GetModifiedValue(m_lifePerHit) : m_lifePerHit;
	}

	public int ModdedDamage(bool includeVariance, ref int damageToSelf)
	{
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					int num = m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount);
					if (m_syncComp != null)
					{
						if (m_abilityMod.m_extraDamageOnConsecutiveCast > 0)
						{
							if (m_syncComp.m_biteLastCastTurn > 0 && GameFlowData.Get().CurrentTurn - m_syncComp.m_biteLastCastTurn == 1)
							{
								num += m_abilityMod.m_extraDamageOnConsecutiveCast;
							}
						}
						if (m_abilityMod.m_extraDamageOnConsecutiveHit > 0)
						{
							if (m_syncComp.m_biteLastHitTurn > 0 && GameFlowData.Get().CurrentTurn - m_syncComp.m_biteLastHitTurn == 1)
							{
								num += m_abilityMod.m_extraDamageOnConsecutiveHit;
							}
						}
					}
					if (includeVariance)
					{
						if (m_abilityMod.m_varianceExtraDamageMin >= 0)
						{
							if (m_abilityMod.m_varianceExtraDamageMax - m_abilityMod.m_varianceExtraDamageMin > 0)
							{
								int num2 = GameplayRandom.Range(m_abilityMod.m_varianceExtraDamageMin, m_abilityMod.m_varianceExtraDamageMax);
								num += num2;
								damageToSelf = Mathf.RoundToInt((float)num2 * m_abilityMod.m_varianceExtraDamageToSelf);
							}
						}
					}
					return num;
				}
				}
			}
		}
		return m_damageAmount;
	}

	public bool HasEffectOnNextTurnStart()
	{
		return !(m_abilityMod == null) && m_abilityMod.m_perAdjacentEnemyEffectOnSelfNextTurn.m_applyEffect;
	}

	public StandardEffectInfo EffectInfoOnNextTurnStart()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
		{
			result = new StandardEffectInfo();
		}
		else
		{
			result = m_abilityMod.m_perAdjacentEnemyEffectOnSelfNextTurn;
		}
		return result;
	}

	public int GetLifeGainAmount(int hitCount)
	{
		float num = 0f;
		if (hitCount > 0 && ModdedLifeOnFirstHit() != 0f)
		{
			num += ModdedLifeOnFirstHit();
		}
		if (ModdedLifePerHit() != 0f)
		{
			num += ModdedLifePerHit() * (float)hitCount;
		}
		return Mathf.RoundToInt(num);
	}
}
