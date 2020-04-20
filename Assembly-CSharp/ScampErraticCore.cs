using System;
using System.Collections.Generic;
using UnityEngine;

public class ScampErraticCore : Ability
{
	[Separator("Targeting", true)]
	public float m_radius = 6f;

	public bool m_ignoreLos;

	[Separator("On Hit", true)]
	public int m_damage = 0xA;

	public StandardEffectInfo m_enemyHitEffect;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private Scamp_SyncComponent m_syncComp;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "ScampErraticCore";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_syncComp = base.GetComponent<Scamp_SyncComponent>();
		base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, this.m_radius, this.m_ignoreLos, this.IncludeEnemies(), false, -1);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	public bool IncludeEnemies()
	{
		bool result;
		if (this.m_damage <= 0)
		{
			result = this.m_enemyHitEffect.m_applyEffect;
		}
		else
		{
			result = true;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_damage);
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool result;
		if (this.m_syncComp != null)
		{
			result = this.m_syncComp.m_suitWasActiveOnTurnStart;
		}
		else
		{
			result = false;
		}
		return result;
	}
}
