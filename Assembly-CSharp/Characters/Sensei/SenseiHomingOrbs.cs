// ROGUES
// SERVER

using System.Collections.Generic;
using UnityEngine;

public class SenseiHomingOrbs : Ability
{
	[Header("-- Orb Targeting --")]
	public int m_numHomingOrbs = 4;
	public int m_maxOrbsPerVolley = 999;
	public float m_homingRadius = 3f;
	public bool m_canHitAllies = true;
	public bool m_canHitEnemies = true;
	[Header("-- Orb Hit Stuff --")]
	public int m_selfHealPerHit;
	public int m_allyHealAmount = 25;
	public int m_enemyDamageAmount = 25;
	public StandardEffectInfo m_allyHitEffect;
	public StandardEffectInfo m_enemyHitEffect;
	public int m_orbDuration = 4;
	[Header("-- Animation --")]
	public int m_orbLaunchAnimIndex = 11;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;
	public GameObject m_persistentOnCasterSequencePrefab;
	public GameObject m_orbSequence;

	private StandardEffectInfo m_cachedAllyHitEffect;
	private StandardEffectInfo m_cachedEnemyHitEffect;

#if SERVER
	// added in rogues
	private Sensei_SyncComponent m_syncComp;
#endif
	
	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Sensei Homing Orbs";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		
#if SERVER
		// added in rogues
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Sensei_SyncComponent>();
		}
#endif
		
		AbilityUtil_Targeter_AoE_Smooth targeter = new AbilityUtil_Targeter_AoE_Smooth(
			this,
			GetHomingRadius(),
			false,
			CanHitEnemies(),
			CanHitAllies(),
			Mathf.Min(GetNumHomingOrbs(), GetMaxOrbsPerVolley()));
		targeter.SetAffectedGroups(CanHitEnemies(), CanHitAllies(), GetSelfHealPerHit() > 0);
		targeter.m_affectCasterDelegate = TargeterAddCasterDelegate;
		Targeter = targeter;
		Targeter.ShowArcToShape = false;
	}

	private bool TargeterAddCasterDelegate(ActorData caster, List<ActorData> addedSoFar)
	{
		return GetSelfHealPerHit() > 0 && addedSoFar.Count > 0;
	}

	private void SetCachedFields()
	{
		m_cachedAllyHitEffect = m_allyHitEffect;
		m_cachedEnemyHitEffect = m_enemyHitEffect;
	}

	public int GetNumHomingOrbs()
	{
		return m_numHomingOrbs;
	}

	public int GetMaxOrbsPerVolley()
	{
		return m_maxOrbsPerVolley;
	}

	public float GetHomingRadius()
	{
		return m_homingRadius;
	}

	public bool CanHitAllies()
	{
		return m_canHitAllies;
	}

	public bool CanHitEnemies()
	{
		return m_canHitEnemies;
	}

	public int GetSelfHealPerHit()
	{
		return m_selfHealPerHit;
	}

	public int GetAllyHealAmount()
	{
		return m_allyHealAmount;
	}

	public int GetEnemyDamageAmount()
	{
		return m_enemyDamageAmount;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public int GetOrbDuration()
	{
		return m_orbDuration;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetEnemyDamageAmount());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetAllyHealAmount());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetSelfHealPerHit());
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (GetSelfHealPerHit() > 0 && Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
		{
			int enemyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			int allyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			results.m_healing = (enemyNum + allyNum) * GetSelfHealPerHit();
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "NumHomingOrbs", string.Empty, m_numHomingOrbs);
		AddTokenInt(tokens, "SelfHealPerReactHit", string.Empty, m_selfHealPerHit);
		AddTokenInt(tokens, "AllyHealAmount", string.Empty, m_allyHealAmount);
		AddTokenInt(tokens, "EnemyDamageAmount", string.Empty, m_enemyDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return animIndex == m_orbLaunchAnimIndex || base.CanTriggerAnimAtIndexForTaunt(animIndex);
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			caster.GetFreePos(),
			caster.AsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(
		List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		actorHitResults.AddEffect(new SenseiHomingOrbsEffect(
			AsEffectSource(),
			caster.GetCurrentBoardSquare(),
			caster,
			m_abilityName,
			m_syncComp,
			GetHomingRadius(),
			false,
			false,
			CanHitEnemies(),
			CanHitAllies(),
			GetEnemyDamageAmount(),
			GetAllyHealAmount(),
			GetSelfHealPerHit(),
			GetAllyHitEffect(),
			GetEnemyHitEffect(),
			GetNumHomingOrbs(),
			GetMaxOrbsPerVolley(),
			GetOrbDuration(),
			m_persistentOnCasterSequencePrefab,
			m_orbSequence,
			m_orbLaunchAnimIndex,
			abilityResults.CinematicRequested));
		abilityResults.StoreActorHit(actorHitResults);
	}
#endif
}
