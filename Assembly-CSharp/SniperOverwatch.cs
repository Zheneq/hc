using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Overwatch";
		}
		base.Targeter = new AbilityUtil_Targeter_Line(this, this.m_range, this.m_penetrateLos);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.m_range;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.m_onEnemyMoveThrough.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "Duration", string.Empty, this.m_duration, false);
		this.m_onEnemyMoveThrough.AddTooltipTokens(tokens, "DroneBarrier", false, null);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SniperOverwatch))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SniperOverwatch);
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

	public int GetBarrierDuration()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_duration;
		}
		else
		{
			result = this.m_abilityMod.m_durationMod.GetModifiedValue(this.m_duration);
		}
		return result;
	}

	public int GetEnemyMaxHits()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_enemyMaxHitsMod.GetModifiedValue(this.m_maxHits) : this.m_maxHits;
	}

	public StandardEffectInfo GetOnMovedThroughEffectInfo()
	{
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_useEnemyHitEffectOverride)
			{
				return this.m_abilityMod.m_enemyHitEffectOverride;
			}
		}
		return this.m_onEnemyMoveThrough.m_effect;
	}

	public GameplayResponseForActor GetOnEnemyMovedThroughResponse()
	{
		if (this.m_abilityMod != null)
		{
			GameplayResponseForActor shallowCopy = this.m_onEnemyMoveThrough.GetShallowCopy();
			shallowCopy.m_effect = this.GetOnMovedThroughEffectInfo();
			shallowCopy.m_damage = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_onEnemyMoveThrough.m_damage);
			return shallowCopy;
		}
		return this.m_onEnemyMoveThrough;
	}
}
