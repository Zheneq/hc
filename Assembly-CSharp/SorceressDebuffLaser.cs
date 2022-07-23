// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SorceressDebuffLaser : Ability
{
	public bool m_penetrateLineOfSight;
	public float m_width = 1f;
	public float m_distance = 15f;
	[Header("-- Hit Effects")]
	public StandardEffectInfo m_enemyHitEffect;
	public StandardEffectInfo m_allyHitEffect;
	public StandardEffectInfo m_casterHitEffect;
	private AbilityMod_SorceressDebuffLaser m_abilityMod;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_Laser(
			this,
			GetLaserWidth(),
			GetLaserRange(),
			m_penetrateLineOfSight,
			-1,
			GetAllyHitEffect().m_applyEffect,
			GetCasterHitEffect().m_applyEffect);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		m_allyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		m_casterHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressDebuffLaser abilityMod_SorceressDebuffLaser = modAsBase as AbilityMod_SorceressDebuffLaser;
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SorceressDebuffLaser != null
			? abilityMod_SorceressDebuffLaser.m_enemyHitEffectOverride.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SorceressDebuffLaser != null
			? abilityMod_SorceressDebuffLaser.m_allyHitEffectOverride.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SorceressDebuffLaser != null
			? abilityMod_SorceressDebuffLaser.m_casterHitEffectOverride.GetModifiedValue(m_casterHitEffect)
			: m_casterHitEffect, "CasterHitEffect", m_casterHitEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SorceressDebuffLaser))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_SorceressDebuffLaser;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_width)
			: m_width;
	}

	private float GetLaserRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_distance)
			: m_distance;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectOverride.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
	}

	private StandardEffectInfo GetAllyHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHitEffectOverride.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
	}

	private StandardEffectInfo GetCasterHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_casterHitEffectOverride.GetModifiedValue(m_casterHitEffect)
			: m_casterHitEffect;
	}

	private bool HasAdditionalEffectIfHit()
	{
		return m_abilityMod != null && m_abilityMod.m_additionalEffectOnSelfIfHit.m_applyEffect;
	}

	private int GetEnemyEffectDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyEffectDurationMod.GetModifiedValue(GetEnemyHitEffect().m_effectData.m_duration)
			: m_enemyHitEffect.m_effectData.m_duration;
	}

	private int GetAllyEffectDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyEffectDurationMod.GetModifiedValue(GetAllyHitEffect().m_effectData.m_duration)
			: m_allyHitEffect.m_effectData.m_duration;
	}

	private int GetCasterEffectDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_casterEffectDurationMod.GetModifiedValue(GetCasterHitEffect().m_effectData.m_duration)
			: m_casterHitEffect.m_effectData.m_duration;
	}

	private int GetCooldownReduction(int numHit)
	{
		return m_abilityMod != null
			? Mathf.Clamp(
				m_abilityMod.m_cooldownReductionOnNumHit.GetModifiedValue(numHit) + m_abilityMod.m_cooldownFlatReduction,
				0,
				m_abilityMod.m_maxCooldownReduction)
			: 0;
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> points = new List<Vector3>();
		float maxDistanceInWorld = GetLaserRange() * Board.Get().squareSize;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(caster.GetLoSCheckPos(), targets[0].AimDirection, maxDistanceInWorld, m_penetrateLineOfSight, caster);
		AreaEffectUtils.AddBoxExtremaToList(ref points, caster.GetLoSCheckPos(), laserEndPoint, GetLaserWidth());
		return points;
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		Vector3 aimDirection = targets[0].AimDirection;
		float maxDistanceInWorld = m_distance * Board.Get().squareSize;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(loSCheckPos, aimDirection, maxDistanceInWorld, m_penetrateLineOfSight, caster);
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			laserEndPoint,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource,
			new Sequence.IExtraSequenceParams[0]);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> actorsInRange = GetActorsInRange(targets, caster, nonActorTargetInfo);
		bool flag = false;
		int numHit = actorsInRange.Contains(caster) ? actorsInRange.Count - 1 : actorsInRange.Count;
		int cooldownReduction = GetCooldownReduction(numHit);
		foreach (ActorData actorData in actorsInRange)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			if (actorData == caster)
			{
				StandardEffectInfo casterHitEffect = GetCasterHitEffect();
				if (casterHitEffect.m_applyEffect)
				{
					StandardActorEffect effect = casterHitEffect.CreateEffect(AsEffectSource(), actorData, caster);
					effect.SetDurationBeforeStart(GetCasterEffectDuration());
					actorHitResults.AddEffect(effect);
				}
				if (HasAdditionalEffectIfHit() && actorsInRange.Count > 1)
				{
					StandardActorEffect effect = m_abilityMod.m_additionalEffectOnSelfIfHit.CreateEffect(AsEffectSource(), actorData, caster);
					effect.SetDurationBeforeStart(actorsInRange.Count - 1);
					actorHitResults.AddEffect(effect);
					if (!casterHitEffect.m_applyEffect)
					{
						actorHitResults.SetIgnoreTechpointInteractionForHit(true);
					}
				}
			}
			else if (actorData.GetTeam() != caster.GetTeam())
			{
				StandardEffectInfo enemyHitEffect = GetEnemyHitEffect();
				if (enemyHitEffect.m_applyEffect)
				{
					StandardActorEffect effect = enemyHitEffect.CreateEffect(AsEffectSource(), actorData, caster);
					effect.SetDurationBeforeStart(GetEnemyEffectDuration());
					actorHitResults.AddEffect(effect);
				}
			}
			else
			{
				StandardEffectInfo allyHitEffect = GetAllyHitEffect();
				if (allyHitEffect.m_applyEffect)
				{
					StandardActorEffect effect = allyHitEffect.CreateEffect(AsEffectSource(), actorData, caster);
					effect.SetDurationBeforeStart(GetAllyEffectDuration());
					actorHitResults.AddEffect(effect);
				}
			}
			if (actorData != caster
			    && !flag
			    && cooldownReduction > 0)
			{
				int overrideValue = Mathf.Max(0, GetModdedCooldown() - cooldownReduction);
				AbilityData.ActionType actionTypeOfAbility = caster.GetAbilityData().GetActionTypeOfAbility(this);
				actorHitResults.AddMiscHitEvent(new MiscHitEventData_OverrideCooldown(actionTypeOfAbility, overrideValue));
				flag = true;
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	public List<ActorData> GetActorsInRange(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, GetAllyHitEffect().m_applyEffect, GetEnemyHitEffect().m_applyEffect);
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targets[0].AimDirection,
			GetLaserRange(),
			GetLaserWidth(),
			caster,
			relevantTeams,
			m_penetrateLineOfSight,
			-1,
			false,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		actorsInLaser.Remove(caster);
		if (GetCasterHitEffect().m_applyEffect
		    || (HasAdditionalEffectIfHit() && actorsInLaser.Count > 0))
		{
			actorsInLaser.Add(caster);
		}
		return actorsInLaser;
	}

	// added in rogues
	public override void OnCalculatedDamageReducedFromWeakenedGrantedByMyEffect(ActorData effectCaster, ActorData weakenedActor, int damageReduced)
	{
		effectCaster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.DigitalSorceressStats.MitigationFromDebuffLaser, damageReduced);
	}
#endif
}
