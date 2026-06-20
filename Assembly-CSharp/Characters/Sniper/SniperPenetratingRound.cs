// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SniperPenetratingRound : Ability
{
	[Header("-- Targeting --")]
	public LaserTargetingInfo m_laserInfo;
	[Header("-- On Hit Stuff --")]
	public int m_laserDamageAmount = 5;
	public StandardEffectInfo m_laserHitEffect;
	[Header("-- Bonus Damage from Target Health Threshold (0 to 1) --")]
	public int m_additionalDamageOnLowHealthTarget;
	public float m_lowHealthThreshold;

	private AbilityMod_SniperPenetratingRound m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Penetrating Round";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (CanKnockbackOnHitActors())
		{
			Targeter = new AbilityUtil_Targeter_SniperPenetratingRound(
				this,
				GetLaserWidth(),
				GetLaserRange(),
				m_laserInfo.penetrateLos,
				m_laserInfo.maxTargets,
				true,
				GetKnockbackThresholdDistance(),
				m_abilityMod.m_knockbackType,
				m_abilityMod.m_knockbackDistance);
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_SniperPenetratingRound(
				this,
				GetLaserWidth(),
				GetLaserRange(),
				m_laserInfo.penetrateLos,
				m_laserInfo.maxTargets);
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	public int GetModdedDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamage.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public bool CanKnockbackOnHitActors()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_knockbackHitEnemy
		       && m_abilityMod.m_knockbackDistance > 0f;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserInfo.width)
			: m_laserInfo.width;
	}

	public float GetLaserRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserInfo.range)
			: m_laserInfo.range;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_abilityMod != null && m_abilityMod.m_useEnemyHitEffectOverride
			? m_abilityMod.m_enemyHitEffectOverride
			: m_laserHitEffect;
	}

	public float GetKnockbackThresholdDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackThresholdDistance
			: -1f;
	}

	public int GetAdditionalDamageOnLowHealthTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_additionalDamageOnLowHealthTargetMod.GetModifiedValue(m_additionalDamageOnLowHealthTarget)
			: m_additionalDamageOnLowHealthTarget;
	}

	public float GetLowHealthThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lowHealthThresholdMod.GetModifiedValue(m_lowHealthThreshold)
			: m_lowHealthThreshold;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (GetModdedDamage() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetModdedDamage());
		}
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (GetLowHealthThreshold() > 0f && GetAdditionalDamageOnLowHealthTarget() > 0)
		{
			List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
			if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				dictionary = new Dictionary<AbilityTooltipSymbol, int>();
				int additionalDamage = targetActor.GetHitPointPercent() < GetLowHealthThreshold()
					? GetAdditionalDamageOnLowHealthTarget()
					: 0;
				dictionary[AbilityTooltipSymbol.Damage] = GetModdedDamage() + additionalDamage;
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SniperPenetratingRound abilityMod_SniperPenetratingRound = modAsBase as AbilityMod_SniperPenetratingRound;
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, abilityMod_SniperPenetratingRound != null
			? abilityMod_SniperPenetratingRound.m_laserDamage.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffect, "LaserHitEffect", null, false);
		AddTokenInt(tokens, "AdditionalDamageOnLowHealthTarget", string.Empty, abilityMod_SniperPenetratingRound != null
			? abilityMod_SniperPenetratingRound.m_additionalDamageOnLowHealthTargetMod.GetModifiedValue(m_additionalDamageOnLowHealthTarget)
			: m_additionalDamageOnLowHealthTarget);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SniperPenetratingRound))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}

		m_abilityMod = abilityMod as AbilityMod_SniperPenetratingRound;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		GetHitActors(targets, caster, out var endPoints, null);
		if (m_laserInfo.maxTargets <= 0)
		{
			float maxDistanceInWorld = GetLaserRange() * Board.Get().squareSize;
			float widthInWorld = GetLaserWidth() * Board.Get().squareSize;
			endPoints = VectorUtils.GetLaserCoordinates(
				caster.GetLoSCheckPos(),
				targets[0].AimDirection,
				maxDistanceInWorld,
				widthInWorld,
				m_laserInfo.penetrateLos,
				caster);
		}
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			null,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource,
			new Sequence.IExtraSequenceParams[]
			{
				new HealLaserSequence.ExtraParams
				{
					endPos = endPoints.end
				}
			});
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out _, nonActorTargetInfo);
		bool flag = GetLowHealthThreshold() > 0f && GetAdditionalDamageOnLowHealthTarget() > 0;
		foreach (ActorData actorData in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			int num = 0;
			if (flag && actorData.GetHitPointPercent() < GetLowHealthThreshold())
			{
				num = GetAdditionalDamageOnLowHealthTarget();
			}
			actorHitResults.SetBaseDamage(GetModdedDamage() + num);
			actorHitResults.AddStandardEffectInfo(GetEnemyHitEffect());
			if (CanKnockbackOnHitActors() && ActorMeetKnockbackConditions(actorData, caster))
			{
				KnockbackHitData knockbackData = new KnockbackHitData(
					actorData,
					caster,
					m_abilityMod.m_knockbackType,
					targets[0].AimDirection,
					caster.GetFreePos(),
					m_abilityMod.m_knockbackDistance);
				actorHitResults.AddKnockbackData(knockbackData);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> GetHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out VectorUtils.LaserCoords endPoints,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, m_laserInfo.affectsAllies, m_laserInfo.affectsEnemies);
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targets[0].AimDirection,
			GetLaserRange(),
			GetLaserWidth(),
			caster,
			relevantTeams,
			m_laserInfo.penetrateLos,
			m_laserInfo.maxTargets,
			false,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		endPoints = laserCoords;
		return actorsInLaser;
	}

	// added in rogues
	private bool ActorMeetKnockbackConditions(ActorData target, ActorData caster)
	{
		return CanKnockbackOnHitActors()
		       && (GetKnockbackThresholdDistance() <= 0f || VectorUtils.HorizontalPlaneDistInSquares(target.GetFreePos(), caster.GetFreePos()) < GetKnockbackThresholdDistance());
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SniperStats.DamageDoneByUlt, results.FinalDamage);
		}
	}
#endif
}
