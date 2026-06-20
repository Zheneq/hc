// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SorceressHealingCrossBeam : Ability
{
	[Header("-- Enemy Hit Damage and Effect")]
	public int m_damageAmount = 10;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Ally Hit Heal and Effect")]
	public int m_healAmount = 5;
	public StandardEffectInfo m_allyHitEffect;
	[Header("-- Targeting")]
	public float m_width = 1f;
	public float m_distance = 15f;
	public int m_numLasers = 4;
	public bool m_alsoHealSelf = true;
	public bool m_penetrateLineOfSight;
	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_beamSequencePrefab;
	public GameObject m_centerSequencePrefab;
	public GameObject m_healSequencePrefab;

	private AbilityUtil_Targeter_CrossBeam m_customTargeter;
	private AbilityMod_SorceressHealingCrossBeam m_abilityMod;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_customTargeter = new AbilityUtil_Targeter_CrossBeam(
			this,
			GetNumLasers(),
			GetLaserRange(),
			GetLaserWidth(), 
			m_penetrateLineOfSight,
			true, 
			m_alsoHealSelf);
		m_customTargeter.SetKnockbackParams(GetKnockbackDistance(), GetKnockbackType(), GetKnockbackThresholdDistance());
		Targeter = m_customTargeter;
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
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_healAmount));
		if (m_alsoHealSelf)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, m_healAmount));
		}
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityUtil_Targeter_CrossBeam.HitActorContext> hitActorContext = m_customTargeter.GetHitActorContext();
		int numTargetsInLaser = 0;
		foreach (AbilityUtil_Targeter_CrossBeam.HitActorContext hitActor in hitActorContext)
		{
			if (hitActor.actor == targetActor)
			{
				numTargetsInLaser = hitActor.totalTargetsInLaser;
				break;
			}
		}
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
		{
			dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmount(numTargetsInLaser);
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
		{
			dictionary[AbilityTooltipSymbol.Healing] = GetHealAmount(numTargetsInLaser);
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			dictionary[AbilityTooltipSymbol.Healing] = GetHealAmount(numTargetsInLaser);
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressHealingCrossBeam abilityMod_SorceressHealingCrossBeam = modAsBase as AbilityMod_SorceressHealingCrossBeam;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_SorceressHealingCrossBeam != null
			? abilityMod_SorceressHealingCrossBeam.m_normalDamageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SorceressHealingCrossBeam != null
			? abilityMod_SorceressHealingCrossBeam.m_enemyEffectOverride.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "HealAmount", string.Empty, abilityMod_SorceressHealingCrossBeam != null
			? abilityMod_SorceressHealingCrossBeam.m_normalHealingMod.GetModifiedValue(m_healAmount)
			: m_healAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SorceressHealingCrossBeam != null
			? abilityMod_SorceressHealingCrossBeam.m_allyEffectOverride.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AddTokenInt(tokens, "NumLasers", string.Empty, abilityMod_SorceressHealingCrossBeam != null
			? abilityMod_SorceressHealingCrossBeam.m_laserNumberMod.GetModifiedValue(m_numLasers)
			: m_numLasers);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SorceressHealingCrossBeam))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_SorceressHealingCrossBeam;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private int GetNumLasers()
	{
		return m_abilityMod != null
			? Mathf.Max(1, m_abilityMod.m_laserNumberMod.GetModifiedValue(m_numLasers))
			: m_numLasers;
	}

	private int GetDamageAmount(int numTargetsInLaser)
	{
		return m_abilityMod != null
			? numTargetsInLaser == 1 && m_abilityMod.m_useSingleTargetHitMods
				? m_abilityMod.m_singleTargetDamageMod.GetModifiedValue(m_damageAmount)
				: m_abilityMod.m_normalDamageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	private int GetHealAmount(int numTargetsInLaser)
	{
		return m_abilityMod != null
			? numTargetsInLaser == 1 && m_abilityMod.m_useSingleTargetHitMods
				? m_abilityMod.m_singleTargetHealingMod.GetModifiedValue(m_healAmount)
				: m_abilityMod.m_normalHealingMod.GetModifiedValue(m_healAmount)
			: m_healAmount;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyEffectOverride.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
	}

	private StandardEffectInfo GetAllyHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyEffectOverride.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
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

	private float GetKnockbackDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistance
			: 0f;
	}

	private KnockbackType GetKnockbackType()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackType
			: KnockbackType.AwayFromSource;
	}

	private float GetKnockbackThresholdDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackThresholdDistance
			: -1f;
	}

#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		List<Vector3> laserEndPoints = GetLaserEndPoints(targets, caster);
		List<ActorData> hitActors = additionalData.m_abilityResults.HitActorList();
		List<ActorData> hitAllies = new List<ActorData>();
		List<ActorData> hitEnemies = new List<ActorData>();
		foreach (ActorData actorData in hitActors)
		{
			if (actorData.GetTeam() == caster.GetTeam())
			{
				hitAllies.Add(actorData);
			}
			else
			{
				hitEnemies.Add(actorData);
			}
		}
		for (int i = 0; i < laserEndPoints.Count; i++)
		{
			ActorData[] targetActorArray = i == 0
				? hitEnemies.ToArray()
				: new ActorData[0];
			list.Add(new ServerClientUtils.SequenceStartData(
				m_beamSequencePrefab, laserEndPoints[i], targetActorArray, caster, additionalData.m_sequenceSource));
		}
		if (m_centerSequencePrefab != null)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_centerSequencePrefab, caster.GetFreePos(), null, caster, additionalData.m_sequenceSource));
		}
		list.Add(new ServerClientUtils.SequenceStartData(
			m_healSequencePrefab, null, hitAllies.ToArray(), caster, additionalData.m_sequenceSource));
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitTargets = GetHitTargets(
			targets,
			caster,
			out Dictionary<ActorData, int> actorToNumInSameLaser,
			nonActorTargetInfo);
		Dictionary<ActorData, ActorHitResults> hitResults = new Dictionary<ActorData, ActorHitResults>();
		foreach (ActorData actorData in hitTargets)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			if (actorData.GetTeam() == caster.GetTeam())
			{
				actorHitResults.SetBaseHealing(GetHealAmount(actorToNumInSameLaser[actorData]));
				actorHitResults.AddStandardEffectInfo(GetAllyHitEffect());
			}
			else
			{
				actorHitResults.SetBaseDamage(GetDamageAmount(actorToNumInSameLaser[actorData]));
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
			}
			hitResults.Add(actorData, actorHitResults);
		}
		if (m_abilityMod != null && m_abilityMod.m_groundEffectOnEnemyHit.m_applyGroundEffect)
		{
			foreach (ActorData actorData in hitTargets)
			{
				if (actorData.GetTeam() != caster.GetTeam())
				{
					ActorHitResults actorHitResults = hitResults[actorData];
					BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
					List<ActorData> affectableActorsInField = m_abilityMod.m_groundEffectOnEnemyHit.GetAffectableActorsInField(
						currentBoardSquare,
						currentBoardSquare.ToVector3(),
						caster,
						nonActorTargetInfo);
					foreach (ActorData affectableActor in affectableActorsInField)
					{
						ActorHitResults value;
						if (hitResults.ContainsKey(affectableActor))
						{
							value = hitResults[affectableActor];
						}
						else
						{
							value = new ActorHitResults(new ActorHitParameters(affectableActor, actorData.GetFreePos()));
							hitResults.Add(affectableActor, value);
						}
						m_abilityMod.m_groundEffectOnEnemyHit.SetupActorHitResult(ref value, caster, currentBoardSquare);
					}
					StandardGroundEffect standardGroundEffect = new StandardGroundEffect(
						AsEffectSource(),
						currentBoardSquare,
						actorData.GetFreePos(),
						null,
						caster,
						m_abilityMod.m_groundEffectOnEnemyHit.m_groundEffectData);
					standardGroundEffect.AddToActorsHitThisTurn(affectableActorsInField);
					actorHitResults.AddEffect(standardGroundEffect);
				}
			}
		}
		foreach (ActorData key in hitResults.Keys)
		{
			abilityResults.StoreActorHit(hitResults[key]);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> GetHitTargets(
		List<AbilityTarget> targets,
		ActorData caster,
		out Dictionary<ActorData, int> actorToNumInSameLaser,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		actorToNumInSameLaser = new Dictionary<ActorData, int>();
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		List<Vector3> laserEndPoints = GetLaserEndPoints(targets, caster);
		List<ActorData> hitTargets = new List<ActorData>();
		foreach (Vector3 endpoint in laserEndPoints)
		{
			Vector3 vector = endpoint - loSCheckPos;
			vector.y = 0f;
			vector.Normalize();
			List<ActorData> actorsInBoxByActorRadius = AreaEffectUtils.GetActorsInBoxByActorRadius(
				loSCheckPos + Board.Get().squareSize * vector,
				endpoint,
				GetLaserWidth(),
				m_penetrateLineOfSight,
				caster,
				null,
				null,
				nonActorTargetInfo);
			int numInTheSameLaser = actorsInBoxByActorRadius.Contains(caster)
				? actorsInBoxByActorRadius.Count - 1
				: actorsInBoxByActorRadius.Count;
			foreach (ActorData actorData in actorsInBoxByActorRadius)
			{
				if (actorData != caster && !hitTargets.Contains(actorData))
				{
					hitTargets.Add(actorData);
					actorToNumInSameLaser[actorData] = numInTheSameLaser;
				}
			}
		}
		if (m_alsoHealSelf)
		{
			hitTargets.Add(caster);
			actorToNumInSameLaser[caster] = 0;
		}
		return hitTargets;
	}

	// added in rogues
	private List<Vector3> GetLaserDirections(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		float startAngle = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
		int numLasers = GetNumLasers();
		float step = 360f / numLasers;
		for (int i = 0; i < numLasers; i++)
		{
			Vector3 item = VectorUtils.AngleDegreesToVector(startAngle + i * step);
			list.Add(item);
		}
		return list;
	}

	// added in rogues
	private List<Vector3> GetLaserEndPoints(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		float maxDistanceInWorld = GetLaserRange() * Board.Get().squareSize;
		foreach (Vector3 dir in GetLaserDirections(targets, caster))
		{
			Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(loSCheckPos, dir, maxDistanceInWorld, m_penetrateLineOfSight, caster);
			list.Add(laserEndPoint);
		}
		return list;
	}

	// added in rogues
	public bool CanKnockbackOnHitActors()
	{
		return GetKnockbackDistance() > 0f;
	}

	// added in rogues
	private bool ActorMeetKnockbackConditions(ActorData target, ActorData caster)
	{
		return CanKnockbackOnHitActors()
		       && target.GetTeam() != caster.GetTeam()
		       && (GetKnockbackThresholdDistance() <= 0f
		           || VectorUtils.HorizontalPlaneDistInSquares(target.GetFreePos(), caster.GetFreePos()) < GetKnockbackThresholdDistance());
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.DigitalSorceressStats.UltDamagePlusHealing, results.FinalDamage);
		}
		if (results.FinalHealing > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.DigitalSorceressStats.UltDamagePlusHealing, results.FinalHealing);
		}
	}
#endif
}
