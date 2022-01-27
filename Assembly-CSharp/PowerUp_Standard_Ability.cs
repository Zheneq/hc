using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Standard_Ability : Ability
{
	public enum ExtraEffectApplyCondition
	{
		Ignore,
		EnemyLowestHealthPct
	}

	[Serializable]
	public class ExtraEffectApplyData
	{
		public ExtraEffectApplyCondition m_condition;
		public StandardEffectInfo m_extraEffect;
	}

	public int m_healAmount = 30;
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
	public List<ExtraEffectApplyData> m_extraEffectsToApply;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Standard PowerUp";
		}
	}

	public void SetHealAmount(int amount)
	{
		m_healAmount = amount;
	}

	public void SetTechPointAmount(int amount)
	{
		m_techPointsAmount = amount;
	}
}
