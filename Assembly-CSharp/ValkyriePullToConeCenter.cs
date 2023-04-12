using System.Collections.Generic;
using UnityEngine;

public class ValkyriePullToConeCenter : Ability
{
	[Header("-- Targeting")]
	public float m_coneAngleWidth = 60f;
	public float m_coneLengthInSquares = 5.5f;
	public float m_coneBackwardOffset;
	public bool m_penetratesLoS;
	[Header("-- Damage & effects")]
	public int m_damage = 40;
	public StandardEffectInfo m_effectToEnemies;
	[Header("-- Knockback on Cast")]
	public float m_maxKnockbackDist = 3f;
	public KnockbackType m_knockbackType = KnockbackType.PerpendicularPullToAimDir;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Valkyrie Pull Cone";
		}
		Setup();
	}

	private void Setup()
	{
		AbilityUtil_Targeter_StretchCone targeter = new AbilityUtil_Targeter_StretchCone(
			this,
			GetConeLength(),
			GetConeLength(),
			GetConeWidth(),
			GetConeWidth(),
			AreaEffectUtils.StretchConeStyle.Linear,
			m_coneBackwardOffset,
			GetPenetrateLoS());
		targeter.InitKnockbackData(GetKnockbackDistance(), m_knockbackType, 0f, m_knockbackType);
		Targeter = targeter;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamage());
		return numbers;
	}

	private int GetDamage()
	{
		return m_damage;
	}

	private StandardEffectInfo GetEffectOnEnemy()
	{
		return m_effectToEnemies;
	}

	private float GetConeWidth()
	{
		return m_coneAngleWidth;
	}

	private float GetConeLength()
	{
		return m_coneLengthInSquares;
	}

	private bool GetPenetrateLoS()
	{
		return m_penetratesLoS;
	}

	private float GetKnockbackDistance()
	{
		return m_maxKnockbackDist;
	}

#if SERVER
	//Added in rouges
	private List<ActorData> FindHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
		return AreaEffectUtils.GetActorsInCone(caster.GetLoSCheckPos(), coneCenterAngleDegrees, GetConeWidth(), GetConeLength(), m_coneBackwardOffset, GetPenetrateLoS(), caster, caster.GetOtherTeams(), nonActorTargetInfo);
	}

	//Added in rouges
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ActorData> list = FindHitActors(targets, caster, null);
		return new ServerClientUtils.SequenceStartData(m_castSequencePrefab, caster.GetCurrentBoardSquare(), list.ToArray(), caster, additionalData.m_sequenceSource, null);
	}

	//Added in rouges
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> list = FindHitActors(targets, caster, nonActorTargetInfo);
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		foreach (ActorData target in list)
		{
			ActorHitParameters hitParams = new ActorHitParameters(target, loSCheckPos);
			ActorHitResults actorHitResults = new ActorHitResults(GetDamage(), HitActionType.Damage, GetEffectOnEnemy(), hitParams);
			if (GetKnockbackDistance() != 0f)
			{
				KnockbackHitData knockbackData = new KnockbackHitData(target, caster, m_knockbackType, targets[0].AimDirection, loSCheckPos, GetKnockbackDistance());
				actorHitResults.AddKnockbackData(knockbackData);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif
}
