// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ThiefPickPocket : Ability
{
	[Header("-- Targeter")]
	public bool m_targeterMultiTarget = true;
	public float m_targeterMinAngle;
	public float m_targeterMaxAngle = 180f;
	public float m_targeterMinInterpDistance = 0.75f;
	public float m_targeterMaxInterpDistance = 3f;
	[Header("-- Laser Properties")]
	public float m_laserRange = 5f;
	public float m_laserWidth = 0.5f;
	public int m_laserMaxTargets = 1;
	public int m_laserCount = 2;
	public bool m_laserPenetrateLos;
	[Header("-- Self Hit")]
	public bool m_includeSelf = true;
	public int m_selfHealAmount = 12;
	public StandardEffectInfo m_selfHitEffect;
	[Header("-- Ally Hit")]
	public bool m_includeAllies = true;
	public int m_laserHealAmount = 3;
	public int m_laserSubsequentHealAmount = 3;
	public StandardEffectInfo m_allyHitEffect;
	[Header("-- Enemy Hit")]
	public bool m_includeEnemies;
	public int m_laserDamageAmount = 3;
	public int m_laserSubsequentDamageAmount = 3;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- How much stock is needed for casting (-1 => \"Consumed Amount On Cast\")")]
	public int m_castableStockThreshold = -1;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_selfHitSequencePrefab;

	private AbilityData.ActionType m_actionType = AbilityData.ActionType.INVALID_ACTION;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Pick Pocket";
		}
		m_sequencePrefab = m_castSequencePrefab;
		m_actionType = GetComponent<ActorData>().GetAbilityData().GetActionTypeOfAbility(this);
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_targeterMultiTarget)
		{
			ClearTargeters();
			for (int i = 0; i < m_laserCount; i++)
			{
				AbilityUtil_Targeter_ThiefFanLaser targeter = new AbilityUtil_Targeter_ThiefFanLaser(
					this,
					m_targeterMinAngle,
					m_targeterMaxAngle,
					m_targeterMinInterpDistance,
					m_targeterMaxInterpDistance,
					m_laserRange,
					m_laserWidth,
					m_laserMaxTargets,
					m_laserCount,
					m_laserPenetrateLos,
					false,
					false,
					false,
					false,
					0);
				targeter.SetIncludeTeams(m_includeAllies, m_includeEnemies, m_includeSelf);
				Targeters.Add(targeter);
				Targeters[i].SetUseMultiTargetUpdate(true);
			}
		}
		else
		{
			AbilityUtil_Targeter_ThiefFanLaser targeter = new AbilityUtil_Targeter_ThiefFanLaser(
				this,
				m_targeterMinAngle,
				m_targeterMaxAngle,
				m_targeterMinInterpDistance,
				m_targeterMaxInterpDistance,
				m_laserRange,
				m_laserWidth,
				m_laserMaxTargets,
				m_laserCount,
				m_laserPenetrateLos,
				false,
				false,
				false,
				false,
				0);
			targeter.SetIncludeTeams(m_includeAllies, m_includeEnemies, m_includeSelf);
			Targeter = targeter;
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_targeterMultiTarget
			? m_laserCount
			: 1;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_laserDamageAmount);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, m_laserHealAmount);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, m_selfHealAmount);
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		int stocksRemaining = caster.GetAbilityData().GetStocksRemaining(m_actionType);
		return m_castableStockThreshold < 0
			? stocksRemaining >= m_stockConsumedOnCast
			: stocksRemaining >= m_castableStockThreshold;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			AccumulateHealthChangesFromTargeter(targetActor, Targeter, dictionary);
		}
		else
		{
			for (int i = 0; i <= currentTargeterIndex; i++)
			{
				AccumulateHealthChangesFromTargeter(targetActor, Targeters[i], dictionary);
			}
		}
		return dictionary;
	}

	private void AccumulateHealthChangesFromTargeter(
		ActorData targetActor,
		AbilityUtil_Targeter targeter,
		Dictionary<AbilityTooltipSymbol, int> symbolToValue)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return;
		}
		foreach (AbilityTooltipSubject current in tooltipSubjectTypes)
		{
			if (current == AbilityTooltipSubject.Primary && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				if (symbolToValue.ContainsKey(AbilityTooltipSymbol.Damage))
				{
					symbolToValue[AbilityTooltipSymbol.Damage] += m_laserSubsequentDamageAmount;
				}
				else
				{
					symbolToValue[AbilityTooltipSymbol.Damage] = m_laserDamageAmount;
				}
			}
			else if (current == AbilityTooltipSubject.Primary && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
			{
				if (symbolToValue.ContainsKey(AbilityTooltipSymbol.Healing))
				{
					symbolToValue[AbilityTooltipSymbol.Healing] += m_laserSubsequentHealAmount;
				}
				else
				{
					symbolToValue[AbilityTooltipSymbol.Healing] = m_laserHealAmount;
				}
			}
		}
	}
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_includeSelf)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_selfHitSequencePrefab,
				caster.GetFreePos(),
				caster.AsArray(),
				caster,
				additionalData.m_sequenceSource));
		}
		GetSequencePositionAndTargets(
			targets,
			caster,
			out List<Vector3> sequenceTargetPositions,
			out List<List<ActorData>> sequenceTargets);
		for (int i = 0; i < sequenceTargetPositions.Count; i++)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				sequenceTargetPositions[i],
				sequenceTargets[i].ToArray(),
				caster,
				additionalData.m_sequenceSource));
		}
		return list;
	}

	// added in rogues
	private void GetSequencePositionAndTargets(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<Vector3> sequenceTargetPositions,
		out List<List<ActorData>> sequenceTargets)
	{
		sequenceTargetPositions = new List<Vector3>();
		sequenceTargets = new List<List<ActorData>>();
		HashSet<PowerUp> powerupsHitPreviously = new HashSet<PowerUp>();
		List<Vector3> laserDirections = GetLaserDirections(targets, caster);
		foreach (Vector3 direction in laserDirections)
		{
			List<ActorData> hitActorsInDirection = GetHitActorsInDirection(
				direction,
				caster,
				powerupsHitPreviously,
				out VectorUtils.LaserCoords laserCoords,
				null);
			sequenceTargets.Add(hitActorsInDirection);
			sequenceTargetPositions.Add(laserCoords.end);
		}
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Dictionary<ActorData, int> hitActorToHealthDelta = new Dictionary<ActorData, int>();
		HashSet<PowerUp> powerupsHitSoFar = new HashSet<PowerUp>();
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		if (m_includeSelf)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			actorHitResults.SetBaseHealing(m_selfHealAmount);
			actorHitResults.AddStandardEffectInfo(m_selfHitEffect);
			abilityResults.StoreActorHit(actorHitResults);
		}
		List<Vector3> laserDirections = GetLaserDirections(targets, caster);
		foreach (Vector3 direction in laserDirections)
		{
			List<ActorData> hitActorsInDirection = GetHitActorsInDirection(direction, caster, powerupsHitSoFar, out _, nonActorTargetInfo);
			// List<ActorData> list = new List<ActorData>();
			foreach (ActorData hitActor in hitActorsInDirection)
			{
				if (hitActor.GetTeam() == caster.GetTeam())
				{
					if (!hitActorToHealthDelta.ContainsKey(hitActor))
					{
						hitActorToHealthDelta[hitActor] = m_laserHealAmount;
						// list.Add(actorData);
					}
					else
					{
						hitActorToHealthDelta[hitActor] += m_laserSubsequentHealAmount;
					}
				}
				else
				{
					if (!hitActorToHealthDelta.ContainsKey(hitActor))
					{
						hitActorToHealthDelta[hitActor] = m_laserDamageAmount;
						// list.Add(actorData);
					}
					else
					{
						hitActorToHealthDelta[hitActor] += m_laserSubsequentDamageAmount;
					}
				}
			}
		}
		ThiefCreateSpoilsMarkerEffect thiefCreateSpoilsMarkerEffect = ServerEffectManager.Get()
			.GetEffect(caster, typeof(ThiefCreateSpoilsMarkerEffect)) as ThiefCreateSpoilsMarkerEffect;
		bool removedEffect = false;
		foreach (ActorData hitActor in hitActorToHealthDelta.Keys)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, caster.GetFreePos()));
			if (hitActor.GetTeam() == caster.GetTeam())
			{
				actorHitResults.SetBaseHealing(hitActorToHealthDelta[hitActor]);
				actorHitResults.AddStandardEffectInfo(m_allyHitEffect);
			}
			else
			{
				actorHitResults.SetBaseDamage(hitActorToHealthDelta[hitActor]);
				actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
				if (thiefCreateSpoilsMarkerEffect != null && !removedEffect)
				{
					actorHitResults.AddEffectForRemoval(thiefCreateSpoilsMarkerEffect, ServerEffectManager.Get().GetActorEffects(caster));
					removedEffect = true;
				}
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private float CalculateFanAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float distInSquares = (currentTarget.FreePos - targetingActor.GetFreePos()).magnitude / Board.Get().squareSize;
		float share = Mathf.Clamp(distInSquares, m_targeterMinInterpDistance, m_targeterMaxInterpDistance) - m_targeterMinInterpDistance;
		return m_targeterMaxAngle * (1f - share / (m_targeterMaxInterpDistance - m_targeterMinInterpDistance));
	}

	// added in rogues
	public float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees)
	{
		return AbilityCommon_FanLaser.CalculateDistanceFromFanAngleDegrees(
			fanAngleDegrees,
			m_targeterMaxAngle,
			m_targeterMinInterpDistance,
			m_targeterMaxInterpDistance);
	}

	// added in rogues
	private List<Vector3> GetLaserDirections(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		if (m_targeterMultiTarget)
		{
			for (int i = 0; i < m_laserCount && i < targets.Count; i++)
			{
				Vector3 aimDir = targets[i].AimDirection;
				if (i > 0 && m_targeterMaxAngle > 0f)
				{
					Vector3 prevAimDir = targets[i - 1].AimDirection;
					float angle = Vector3.Angle(aimDir, prevAimDir);
					if (angle > m_targeterMaxAngle)
					{
						aimDir = Vector3.RotateTowards(
							aimDir, 
							prevAimDir,
							0.0174532924f * (angle - m_targeterMaxAngle), 0f);
					}
				}
				list.Add(aimDir);
			}
		}
		else
		{
			float angleFull = m_laserCount > 1 ? CalculateFanAngleDegrees(targets[0], caster) : 0f;
			float angleStep = m_laserCount > 1 ? angleFull / (m_laserCount - 1) : 0f;
			float angleStart = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection) - 0.5f * angleFull;
			for (int i = 0; i < m_laserCount; i++)
			{
				list.Add(VectorUtils.AngleDegreesToVector(angleStart + i * angleStep));
			}
		}
		return list;
	}

	// added in rogues
	private List<ActorData> GetHitActorsInDirection(
		Vector3 direction,
		ActorData caster,
		HashSet<PowerUp> powerupsHitPreviously,
		out VectorUtils.LaserCoords endPoints,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return ThiefBasicAttack.GetHitActorsInDirectionStatic(
			caster.GetLoSCheckPos(),
			direction,
			caster,
			m_laserRange,
			m_laserWidth,
			m_laserPenetrateLos,
			m_laserMaxTargets,
			m_includeAllies,
			m_includeEnemies,
			true,
			0,
			false,
			false,
			false,
			false,
			powerupsHitPreviously,
			out endPoints,
			out _,
			nonActorTargetInfo,
			false);
	}
#endif
}
