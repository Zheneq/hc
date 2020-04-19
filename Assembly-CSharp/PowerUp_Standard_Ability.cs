using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Standard_Ability : Ability
{
	public int m_healAmount = 0x1E;

	public int m_techPointsAmount;

	[Tooltip("Credits to give to the actor who ran over the powerup.")]
	public int m_personalCredits;

	[Tooltip("Credits to give to each actor on the team of the actor who ran over the powerup (including the picker-upper).")]
	public int m_teamCredits;

	public int m_objectivePointAdjust_casterTeam;

	public int m_objectivePointAdjust_enemyTeam;

	public bool m_applyEffect;

	public StandardActorEffectData m_effect;

	public AbilityStatMod[] m_permanentStatMods;

	public StatusType[] m_permanentStatusChanges;

	public bool m_awardCoins;

	[Separator("Extra Effects, for one-off powerups", true)]
	public List<PowerUp_Standard_Ability.ExtraEffectApplyData> m_extraEffectsToApply;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Standard PowerUp";
		}
	}

	public void SetHealAmount(int amount)
	{
		this.m_healAmount = amount;
	}

	public void SetTechPointAmount(int amount)
	{
		this.m_techPointsAmount = amount;
	}

	public enum ExtraEffectApplyCondition
	{
		Ignore,
		EnemyLowestHealthPct
	}

	[Serializable]
	public class ExtraEffectApplyData
	{
		public PowerUp_Standard_Ability.ExtraEffectApplyCondition m_condition;

		public StandardEffectInfo m_extraEffect;
	}
}
