﻿using System.Collections.Generic;
using UnityEngine;

public class SpaceMarineMissileBarrage : Ability
{
	[Space(10f)]
	public int m_missiles = 3;
	public int m_damage = 3;
	public float m_radius = 5f;
	public bool m_penetrateLineOfSight;
	public AbilityPriority m_damageRunPriority;
	public StandardEffectInfo m_effectOnTargets;
	[Separator("Whether damage can be reacted to")]
	public bool m_considerDamageAsDirect;
	[Separator("Sequences and Anim")]
	public GameObject m_buffSequence;
	public GameObject m_missileSequence;
	public int m_missileLaunchAnimIndex = 11;

	private AbilityMod_SpaceMarineMissileBarrage m_abilityMod;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		Targeter = new AbilityUtil_Targeter_AoE_Smooth(
			this,
			m_radius,
			m_penetrateLineOfSight,
			true, 
			false,
			m_missiles);
		bool affectsCaster = GetModdedEffectForSelf() != null && GetModdedEffectForSelf().m_applyEffect;
		Targeter.SetAffectedGroups(true, false, affectsCaster);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, ModdedDamage()));
		ModdedEffectOnTargets().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		AppendTooltipNumbersFromBaseModEffects(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		return new Dictionary<AbilityTooltipSymbol, int>
		{
			{
				AbilityTooltipSymbol.Damage,
				ModdedDamage() + ModdedExtraDamagePerTarget() * (Targeter.GetNumActorsInRange() - 1)
			}
		};
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SpaceMarineMissileBarrage abilityMod_SpaceMarineMissileBarrage = modAsBase as AbilityMod_SpaceMarineMissileBarrage;
		AddTokenInt(tokens, "DelayTurns", string.Empty, 1);
		AddTokenInt(tokens, "Missiles", string.Empty, m_missiles);
		AddTokenInt(tokens, "Damage", string.Empty, abilityMod_SpaceMarineMissileBarrage != null
			? abilityMod_SpaceMarineMissileBarrage.m_damageMod.GetModifiedValue(m_damage)
			: m_damage);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnTargets, "EffectOnTargets");
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return animIndex == m_missileLaunchAnimIndex || base.CanTriggerAnimAtIndexForTaunt(animIndex);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SpaceMarineMissileBarrage))
		{
			m_abilityMod = abilityMod as AbilityMod_SpaceMarineMissileBarrage;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	public int ModdedDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	public int ModdedMissileActiveDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_activeDurationMod.GetModifiedValue(1)
			: 1;
	}

	public StandardEffectInfo ModdedEffectOnTargets()
	{
		return m_abilityMod != null && m_abilityMod.m_missileHitEffectOverride.m_applyEffect
			? m_abilityMod.m_missileHitEffectOverride
			: m_effectOnTargets;
	}

	public int ModdedExtraDamagePerTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamagePerTarget.GetModifiedValue(0)
			: 0;
	}
}
