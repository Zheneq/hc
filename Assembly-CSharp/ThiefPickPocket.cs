using System;
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

	public int m_selfHealAmount = 0xC;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Pick Pocket";
		}
		this.m_sequencePrefab = this.m_castSequencePrefab;
		this.m_actionType = base.GetComponent<ActorData>().GetAbilityData().GetActionTypeOfAbility(this);
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.m_targeterMultiTarget)
		{
			base.ClearTargeters();
			for (int i = 0; i < this.m_laserCount; i++)
			{
				AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = new AbilityUtil_Targeter_ThiefFanLaser(this, this.m_targeterMinAngle, this.m_targeterMaxAngle, this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance, this.m_laserRange, this.m_laserWidth, this.m_laserMaxTargets, this.m_laserCount, this.m_laserPenetrateLos, false, false, false, false, 0, 0f, 0f);
				abilityUtil_Targeter_ThiefFanLaser.SetIncludeTeams(this.m_includeAllies, this.m_includeEnemies, this.m_includeSelf);
				base.Targeters.Add(abilityUtil_Targeter_ThiefFanLaser);
				base.Targeters[i].SetUseMultiTargetUpdate(true);
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefPickPocket.SetupTargeter()).MethodHandle;
			}
		}
		else
		{
			AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser2 = new AbilityUtil_Targeter_ThiefFanLaser(this, this.m_targeterMinAngle, this.m_targeterMaxAngle, this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance, this.m_laserRange, this.m_laserWidth, this.m_laserMaxTargets, this.m_laserCount, this.m_laserPenetrateLos, false, false, false, false, 0, 0f, 0f);
			abilityUtil_Targeter_ThiefFanLaser2.SetIncludeTeams(this.m_includeAllies, this.m_includeEnemies, this.m_includeSelf);
			base.Targeter = abilityUtil_Targeter_ThiefFanLaser2;
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return (!this.m_targeterMultiTarget) ? 1 : this.m_laserCount;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.m_laserDamageAmount);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.m_laserHealAmount);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.m_selfHealAmount);
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		int stocksRemaining = caster.GetAbilityData().GetStocksRemaining(this.m_actionType);
		int stockConsumedOnCast = this.m_stockConsumedOnCast;
		bool result;
		if (this.m_castableStockThreshold < 0)
		{
			result = (stocksRemaining >= stockConsumedOnCast);
		}
		else
		{
			result = (stocksRemaining >= this.m_castableStockThreshold);
		}
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (this.GetExpectedNumberOfTargeters() < 2)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefPickPocket.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			this.AccumulateHealthChangesFromTargeter(targetActor, base.Targeter, dictionary);
		}
		else
		{
			for (int i = 0; i <= currentTargeterIndex; i++)
			{
				this.AccumulateHealthChangesFromTargeter(targetActor, base.Targeters[i], dictionary);
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
					AbilityTooltipSubject abilityTooltipSubject = enumerator.Current;
					if (abilityTooltipSubject == AbilityTooltipSubject.Primary)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefPickPocket.AccumulateHealthChangesFromTargeter(ActorData, AbilityUtil_Targeter, Dictionary<AbilityTooltipSymbol, int>)).MethodHandle;
						}
						if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
							if (!symbolToValue.ContainsKey(AbilityTooltipSymbol.Damage))
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
								symbolToValue[AbilityTooltipSymbol.Damage] = this.m_laserDamageAmount;
							}
							else
							{
								symbolToValue[AbilityTooltipSymbol.Damage] = symbolToValue[AbilityTooltipSymbol.Damage] + this.m_laserSubsequentDamageAmount;
							}
							continue;
						}
					}
					if (abilityTooltipSubject == AbilityTooltipSubject.Primary && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
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
						if (!symbolToValue.ContainsKey(AbilityTooltipSymbol.Healing))
						{
							symbolToValue[AbilityTooltipSymbol.Healing] = this.m_laserHealAmount;
						}
						else
						{
							symbolToValue[AbilityTooltipSymbol.Healing] = symbolToValue[AbilityTooltipSymbol.Healing] + this.m_laserSubsequentHealAmount;
						}
					}
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
		}
	}
}
