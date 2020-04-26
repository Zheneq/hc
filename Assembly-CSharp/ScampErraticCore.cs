using System.Collections.Generic;
using UnityEngine;

public class ScampErraticCore : Ability
{
	[Separator("Targeting", true)]
	public float m_radius = 6f;

	public bool m_ignoreLos;

	[Separator("On Hit", true)]
	public int m_damage = 10;

	public StandardEffectInfo m_enemyHitEffect;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private Scamp_SyncComponent m_syncComp;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "ScampErraticCore";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, m_radius, m_ignoreLos, IncludeEnemies());
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	public bool IncludeEnemies()
	{
		int result;
		if (m_damage <= 0)
		{
			result = (m_enemyHitEffect.m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damage);
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		int result;
		if (m_syncComp != null)
		{
			result = (m_syncComp.m_suitWasActiveOnTurnStart ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}
}
