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
		base.Targeter = abilityUtil_Targeter_Laser;
	}

	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar)
	{
		int result;
		if (m_healOnSelfPerHit > 0)
		{
			result = ((actorsSoFar.Count > 0) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private int GetMaxTargetsForTargeter(ActorData caster)
	{
		return GetCurrentMaxTargets();
	}

	public int GetCurrentMaxTargets()
	{
		int num = m_laserTargetingInfo.maxTargets;
		if (m_syncComp != null)
		{
			num = m_syncComp.m_syncCurrentNumOrbs;
			if (m_maxOrbsPerCast > 0)
			{
				if (num > m_maxOrbsPerCast)
				{
					num = m_maxOrbsPerCast;
				}
			}
		}
		return num;
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
		if (m_healOnSelfPerHit > 0)
		{
			if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
			{
				int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
				int visibleActorsCountByTooltipSubject2 = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				int num = results.m_healing = m_healOnSelfPerHit * (visibleActorsCountByTooltipSubject + visibleActorsCountByTooltipSubject2);
			}
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}
}
