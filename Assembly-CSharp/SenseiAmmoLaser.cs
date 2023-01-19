using System.Collections.Generic;
using UnityEngine;

public class SenseiAmmoLaser : Ability
{
	[Header("-- Targeting --")]
	public LaserTargetingInfo m_laserTargetingInfo;
	public int m_maxOrbsPerCast = 3;
	[Header("-- On Hit --")]
	public int m_damage = 10;
	public StandardEffectInfo m_enemyHitEffect;
	public int m_healOnAlly = 10;
	public StandardEffectInfo m_allyHitEffect;
	public int m_healOnSelfPerHit = 5;
	[Header("-- Sequences: assuming projectile sequence, one per hit target/ammo")]
	public GameObject m_castSequencePrefab;
	public float m_delayBetweenOrbSequence = 0.25f;

	private Sensei_SyncComponent m_syncComp;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "SenseiAmmoLaser";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Sensei_SyncComponent>();
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(this, m_laserTargetingInfo);
		abilityUtil_Targeter_Laser.SetAffectedGroups(m_laserTargetingInfo.affectsEnemies, m_laserTargetingInfo.affectsAllies, m_healOnSelfPerHit > 0);
		abilityUtil_Targeter_Laser.m_customMaxTargetsDelegate = GetMaxTargetsForTargeter;
		abilityUtil_Targeter_Laser.m_affectCasterDelegate = TargeterIncludeCaster;
		Targeter = abilityUtil_Targeter_Laser;
	}

	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar)
	{
		return m_healOnSelfPerHit > 0 && actorsSoFar.Count > 0;
	}

	private int GetMaxTargetsForTargeter(ActorData caster)
	{
		return GetCurrentMaxTargets();
	}

	public int GetCurrentMaxTargets()
	{
		int targets = m_laserTargetingInfo.maxTargets;
		if (m_syncComp != null)
		{
			targets = m_syncComp.m_syncCurrentNumOrbs;
			if (m_maxOrbsPerCast > 0 && targets > m_maxOrbsPerCast)
			{
				targets = m_maxOrbsPerCast;
			}
		}
		return targets;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return GetCurrentMaxTargets() > 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damage);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, m_healOnAlly);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, m_healOnSelfPerHit);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (m_healOnSelfPerHit > 0 && Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
		{
			int allyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			int enemyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			results.m_healing = m_healOnSelfPerHit * (allyNum + enemyNum);
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}
}
