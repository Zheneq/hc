using System.Collections.Generic;
using UnityEngine;

public class ScampErraticCore : Ability
{
	[Separator("Targeting")]
	public float m_radius = 6f;
	public bool m_ignoreLos;
	[Separator("On Hit")]
	public int m_damage = 10;
	public StandardEffectInfo m_enemyHitEffect;
	[Separator("Sequences")]
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
		Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, m_radius, m_ignoreLos, IncludeEnemies());
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	public bool IncludeEnemies()
	{
		return m_damage > 0 || m_enemyHitEffect.m_applyEffect;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damage);
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart;
	}
}
