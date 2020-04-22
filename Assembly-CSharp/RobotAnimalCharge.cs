using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalCharge : Ability
{
	public int m_damageAmount = 20;

	public float m_lifeOnFirstHit;

	public float m_lifePerHit;

	public int m_maxTargetsHit = 1;

	public AbilityAreaShape m_targetShape;

	public bool m_targetShapePenetratesLoS;

	public bool m_chaseTarget = true;

	public StandardEffectInfo m_chaserEffect;

	public StandardEffectInfo m_enemyTargetEffect;

	public StandardEffectInfo m_allyTargetEffect;

	public float m_recoveryTime = 1f;

	[Header("-- Targeting: Whether require dashing at target actor")]
	public bool m_requireTargetActor = true;

	public bool m_canIncludeEnemy = true;

	public bool m_canIncludeAlly = true;

	[Header("-- Cooldown reduction on hitting target")]
	public int m_cdrOnHittingAlly;

	public int m_cdrOnHittingEnemy;

	private AbilityMod_RobotAnimalCharge m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Death Snuggle";
		}
		Setup();
	}

	private void Setup()
	{
		AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, m_targetShape, m_targetShapePenetratesLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, CanIncludeEnemy(), CanIncludeAlly());
		abilityUtil_Targeter_Charge.m_forceChase = true;
		if (!(ModdedLifeOnFirstHit() > 0f))
		{
			if (!(ModdedLifePerHit() > 0f))
			{
				goto IL_0073;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
		}
		abilityUtil_Targeter_Charge.m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
		abilityUtil_Targeter_Charge.m_affectCasterDelegate = TargeterIncludeCaster;
		goto IL_0073;
		IL_0073:
		base.Targeter = abilityUtil_Targeter_Charge;
	}

	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
	{
		int enemyCount = AbilityUtils.GetEnemyCount(actorsSoFar, caster);
		return enemyCount > 0;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	public int ModdedDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_damageAmount;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount);
		}
		return result;
	}

	public float ModdedLifeOnFirstHit()
	{
		float result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_lifeOnFirstHit;
		}
		else
		{
			result = m_abilityMod.m_lifeOnFirstHitMod.GetModifiedValue(m_lifeOnFirstHit);
		}
		return result;
	}

	public float ModdedLifePerHit()
	{
		float result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_lifePerHit;
		}
		else
		{
			result = m_abilityMod.m_lifePerHitMod.GetModifiedValue(m_lifePerHit);
		}
		return result;
	}

	public int ModdedHealOnNextTurnStartIfKilledTarget()
	{
		int result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_healOnNextTurnStartIfKilledTarget;
		}
		return result;
	}

	public StandardEffectInfo ModdedEffectForSelfPerAdjacentAlly()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = new StandardEffectInfo();
		}
		else
		{
			result = m_abilityMod.m_effectToSelfPerAdjacentAlly;
		}
		return result;
	}

	public int ModdedTechPointGainPerAdjacentAlly()
	{
		int result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_techPointsPerAdjacentAlly;
		}
		return result;
	}

	public bool RequireTargetActor()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_requireTargetActorMod.GetModifiedValue(m_requireTargetActor);
		}
		else
		{
			result = m_requireTargetActor;
		}
		return result;
	}

	public bool CanIncludeEnemy()
	{
		return (!m_abilityMod) ? m_canIncludeEnemy : m_abilityMod.m_canIncludeEnemyMod.GetModifiedValue(m_canIncludeEnemy);
	}

	public bool CanIncludeAlly()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_canIncludeAllyMod.GetModifiedValue(m_canIncludeAlly);
		}
		else
		{
			result = m_canIncludeAlly;
		}
		return result;
	}

	public int GetCdrOnHittingAlly()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_cdrOnHittingAllyMod.GetModifiedValue(m_cdrOnHittingAlly);
		}
		else
		{
			result = m_cdrOnHittingAlly;
		}
		return result;
	}

	public int GetCdrOnHittingEnemy()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_cdrOnHittingEnemyMod.GetModifiedValue(m_cdrOnHittingEnemy);
		}
		else
		{
			result = m_cdrOnHittingEnemy;
		}
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (RequireTargetActor())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return HasTargetableActorsInDecision(caster, CanIncludeEnemy(), CanIncludeAlly(), false, ValidateCheckPath.CanBuildPath, true, false);
				}
			}
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool result = true;
		if (RequireTargetActor())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					result = false;
					List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, CanIncludeAlly(), CanIncludeEnemy());
					List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(m_targetShape, target, m_targetShapePenetratesLoS, caster, relevantTeams, null);
					using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData current = enumerator.Current;
							if (CanTargetActorInDecision(caster, current, CanIncludeEnemy(), CanIncludeAlly(), false, ValidateCheckPath.CanBuildPath, true, false))
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										return true;
									}
								}
							}
						}
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								return result;
							}
						}
					}
				}
				}
			}
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damageAmount);
		int amount = Mathf.RoundToInt(m_lifeOnFirstHit);
		int amount2 = Mathf.RoundToInt(m_lifePerHit);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Tertiary, amount);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Quaternary, amount2);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, ModdedDamage());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, Mathf.RoundToInt(Mathf.Max(ModdedLifeOnFirstHit(), ModdedLifePerHit())));
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
						int num = dictionary[AbilityTooltipSymbol.Healing] = GetLifeGainAmount(visibleActorsInRangeByTooltipSubject.Count);
					}
					else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
					{
						dictionary[AbilityTooltipSymbol.Damage] = ModdedDamage();
					}
					return dictionary;
				}
				}
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RobotAnimalCharge abilityMod_RobotAnimalCharge = modAsBase as AbilityMod_RobotAnimalCharge;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_RobotAnimalCharge)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			val = abilityMod_RobotAnimalCharge.m_damageMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			val = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty, val);
		AddTokenInt(tokens, "MaxTargetsHit", string.Empty, m_maxTargetsHit);
		AbilityMod.AddToken_EffectInfo(tokens, m_chaserEffect, "ChaserEffect", m_chaserEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyTargetEffect, "EnemyTargetEffect", m_enemyTargetEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyTargetEffect, "AllyTargetEffect", m_allyTargetEffect);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RobotAnimalCharge))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityMod = (abilityMod as AbilityMod_RobotAnimalCharge);
					Setup();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	public int GetLifeGainAmount(int hitCount)
	{
		float num = 0f;
		if (hitCount > 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (ModdedLifeOnFirstHit() != 0f)
			{
				num += ModdedLifeOnFirstHit();
			}
		}
		if (ModdedLifePerHit() != 0f)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			num += ModdedLifePerHit() * (float)hitCount;
		}
		return Mathf.RoundToInt(num);
	}
}
