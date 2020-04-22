using System.Collections.Generic;
using UnityEngine;

public class ThiefPickPocket : Ability
{
	[Header("-- Targeter")]
	public bool m_targeterMultiTarget = true;

	public float m_targeterMinAngle;

	public float m_targeterMaxAngle = 180f;

	public float m_targeterMinInterpDistance = 0.75f;

	public float m_targeterMaxInterpDistance = 3f;

	[Header("-- Laser Properties")]
	public float m_laserRange = 5f;

	public float m_laserWidth = 0.5f;

	public int m_laserMaxTargets = 1;

	public int m_laserCount = 2;

	public bool m_laserPenetrateLos;

	[Header("-- Self Hit")]
	public bool m_includeSelf = true;

	public int m_selfHealAmount = 12;

	public StandardEffectInfo m_selfHitEffect;

	[Header("-- Ally Hit")]
	public bool m_includeAllies = true;

	public int m_laserHealAmount = 3;

	public int m_laserSubsequentHealAmount = 3;

	public StandardEffectInfo m_allyHitEffect;

	[Header("-- Enemy Hit")]
	public bool m_includeEnemies;

	public int m_laserDamageAmount = 3;

	public int m_laserSubsequentDamageAmount = 3;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- How much stock is needed for casting (-1 => \"Consumed Amount On Cast\")")]
	public int m_castableStockThreshold = -1;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_selfHitSequencePrefab;

	private AbilityData.ActionType m_actionType = AbilityData.ActionType.INVALID_ACTION;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Pick Pocket";
		}
		m_sequencePrefab = m_castSequencePrefab;
		m_actionType = GetComponent<ActorData>().GetAbilityData().GetActionTypeOfAbility(this);
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_targeterMultiTarget)
		{
			ClearTargeters();
			for (int i = 0; i < m_laserCount; i++)
			{
				AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = new AbilityUtil_Targeter_ThiefFanLaser(this, m_targeterMinAngle, m_targeterMaxAngle, m_targeterMinInterpDistance, m_targeterMaxInterpDistance, m_laserRange, m_laserWidth, m_laserMaxTargets, m_laserCount, m_laserPenetrateLos, false, false, false, false, 0);
				abilityUtil_Targeter_ThiefFanLaser.SetIncludeTeams(m_includeAllies, m_includeEnemies, m_includeSelf);
				base.Targeters.Add(abilityUtil_Targeter_ThiefFanLaser);
				base.Targeters[i].SetUseMultiTargetUpdate(true);
			}
			while (true)
			{
				return;
			}
		}
		AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser2 = new AbilityUtil_Targeter_ThiefFanLaser(this, m_targeterMinAngle, m_targeterMaxAngle, m_targeterMinInterpDistance, m_targeterMaxInterpDistance, m_laserRange, m_laserWidth, m_laserMaxTargets, m_laserCount, m_laserPenetrateLos, false, false, false, false, 0);
		abilityUtil_Targeter_ThiefFanLaser2.SetIncludeTeams(m_includeAllies, m_includeEnemies, m_includeSelf);
		base.Targeter = abilityUtil_Targeter_ThiefFanLaser2;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return (!m_targeterMultiTarget) ? 1 : m_laserCount;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_laserDamageAmount);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, m_laserHealAmount);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, m_selfHealAmount);
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool flag = false;
		int stocksRemaining = caster.GetAbilityData().GetStocksRemaining(m_actionType);
		int stockConsumedOnCast = m_stockConsumedOnCast;
		if (m_castableStockThreshold < 0)
		{
			return stocksRemaining >= stockConsumedOnCast;
		}
		return stocksRemaining >= m_castableStockThreshold;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			AccumulateHealthChangesFromTargeter(targetActor, base.Targeter, dictionary);
		}
		else
		{
			for (int i = 0; i <= currentTargeterIndex; i++)
			{
				AccumulateHealthChangesFromTargeter(targetActor, base.Targeters[i], dictionary);
			}
		}
		return dictionary;
	}

	private void AccumulateHealthChangesFromTargeter(ActorData targetActor, AbilityUtil_Targeter targeter, Dictionary<AbilityTooltipSymbol, int> symbolToValue)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			using (List<AbilityTooltipSubject>.Enumerator enumerator = tooltipSubjectTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityTooltipSubject current = enumerator.Current;
					if (current == AbilityTooltipSubject.Primary)
					{
						if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
						{
							if (!symbolToValue.ContainsKey(AbilityTooltipSymbol.Damage))
							{
								symbolToValue[AbilityTooltipSymbol.Damage] = m_laserDamageAmount;
							}
							else
							{
								symbolToValue[AbilityTooltipSymbol.Damage] += m_laserSubsequentDamageAmount;
							}
							continue;
						}
					}
					if (current == AbilityTooltipSubject.Primary && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
					{
						if (!symbolToValue.ContainsKey(AbilityTooltipSymbol.Healing))
						{
							symbolToValue[AbilityTooltipSymbol.Healing] = m_laserHealAmount;
						}
						else
						{
							symbolToValue[AbilityTooltipSymbol.Healing] += m_laserSubsequentHealAmount;
						}
					}
				}
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}
}
