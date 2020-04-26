using System.Collections.Generic;
using UnityEngine;

public class SniperOverwatch : Ability
{
	public GameplayResponseForActor m_onEnemyMoveThrough;

	public bool m_penetrateLos;

	public float m_range = 10f;

	public int m_duration = 1;

	public int m_maxHits = 1;

	public bool m_removeOnTurnEndIfEnemyMovedThrough = true;

	public List<GameObject> m_barrierSequencePrefabs;

	private AbilityMod_SniperOverwatch m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Overwatch";
		}
		base.Targeter = new AbilityUtil_Targeter_Line(this, m_range, m_penetrateLos);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return m_range;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_onEnemyMoveThrough.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Duration", string.Empty, m_duration);
		m_onEnemyMoveThrough.AddTooltipTokens(tokens, "DroneBarrier", false, null);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SniperOverwatch))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_SniperOverwatch);
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

	public int GetBarrierDuration()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_duration;
		}
		else
		{
			result = m_abilityMod.m_durationMod.GetModifiedValue(m_duration);
		}
		return result;
	}

	public int GetEnemyMaxHits()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_enemyMaxHitsMod.GetModifiedValue(m_maxHits) : m_maxHits;
	}

	public StandardEffectInfo GetOnMovedThroughEffectInfo()
	{
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_useEnemyHitEffectOverride)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return m_abilityMod.m_enemyHitEffectOverride;
					}
				}
			}
		}
		return m_onEnemyMoveThrough.m_effect;
	}

	public GameplayResponseForActor GetOnEnemyMovedThroughResponse()
	{
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					GameplayResponseForActor shallowCopy = m_onEnemyMoveThrough.GetShallowCopy();
					shallowCopy.m_effect = GetOnMovedThroughEffectInfo();
					shallowCopy.m_damage = m_abilityMod.m_damageMod.GetModifiedValue(m_onEnemyMoveThrough.m_damage);
					return shallowCopy;
				}
				}
			}
		}
		return m_onEnemyMoveThrough;
	}
}
