using System;
using System.Collections.Generic;
using UnityEngine;

public class SenseiAmmoLaser : Ability
{
	[Header("-- Targeting --")]
	public LaserTargetingInfo m_laserTargetingInfo;

	public int m_maxOrbsPerCast = 3;

	[Header("-- On Hit --")]
	public int m_damage = 0xA;

	public StandardEffectInfo m_enemyHitEffect;

	public int m_healOnAlly = 0xA;

	public StandardEffectInfo m_allyHitEffect;

	public int m_healOnSelfPerHit = 5;

	[Header("-- Sequences: assuming projectile sequence, one per hit target/ammo")]
	public GameObject m_castSequencePrefab;

	public float m_delayBetweenOrbSequence = 0.25f;

	private Sensei_SyncComponent m_syncComp;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "SenseiAmmoLaser";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_syncComp = base.GetComponent<Sensei_SyncComponent>();
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(this, this.m_laserTargetingInfo);
		abilityUtil_Targeter_Laser.SetAffectedGroups(this.m_laserTargetingInfo.affectsEnemies, this.m_laserTargetingInfo.affectsAllies, this.m_healOnSelfPerHit > 0);
		abilityUtil_Targeter_Laser.m_customMaxTargetsDelegate = new AbilityUtil_Targeter_Laser.MaxTargetsDelegate(this.GetMaxTargetsForTargeter);
		abilityUtil_Targeter_Laser.m_affectCasterDelegate = new AbilityUtil_Targeter_Laser.IsAffectingCasterDelegate(this.TargeterIncludeCaster);
		base.Targeter = abilityUtil_Targeter_Laser;
	}

	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar)
	{
		bool result;
		if (this.m_healOnSelfPerHit > 0)
		{
			result = (actorsSoFar.Count > 0);
		}
		else
		{
			result = false;
		}
		return result;
	}

	private int GetMaxTargetsForTargeter(ActorData caster)
	{
		return this.GetCurrentMaxTargets();
	}

	public int GetCurrentMaxTargets()
	{
		int num = this.m_laserTargetingInfo.maxTargets;
		if (this.m_syncComp != null)
		{
			num = (int)this.m_syncComp.m_syncCurrentNumOrbs;
			if (this.m_maxOrbsPerCast > 0)
			{
				if (num > this.m_maxOrbsPerCast)
				{
					num = this.m_maxOrbsPerCast;
				}
			}
		}
		return num;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return this.GetCurrentMaxTargets() > 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.m_damage);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.m_healOnAlly);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.m_healOnSelfPerHit);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (this.m_healOnSelfPerHit > 0)
		{
			if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
			{
				int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
				int visibleActorsCountByTooltipSubject2 = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				int healing = this.m_healOnSelfPerHit * (visibleActorsCountByTooltipSubject + visibleActorsCountByTooltipSubject2);
				results.m_healing = healing;
			}
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}
}
