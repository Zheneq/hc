// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThiefBasicAttack : Ability
{
	[Header("-- Targeter")]
	public bool m_targeterMultiTarget = true;
	public float m_targeterMaxAngle = 120f;
	public float m_targeterMinInterpDistance = 1.5f;
	public float m_targeterMaxInterpDistance = 6f;
	[Header("-- Damage")]
	public int m_laserDamageAmount = 3;
	public int m_laserSubsequentDamageAmount = 3;
	public int m_extraDamageForSingleHit;
	public int m_extraDamageForHittingPowerup;
	[Header("-- Healing")]
	public int m_healOnSelfIfHitEnemyAndPowerup;
	[Header("-- Energy")]
	public int m_energyGainPerLaserHit;
	public int m_energyGainPerPowerupHit;
	[Header("-- Laser Properties")]
	public float m_laserRange = 5f;
	public float m_laserWidth = 0.5f;
	public int m_laserMaxTargets = 1;
	public int m_laserCount = 2;
	public bool m_laserPenetrateLos;
	[Header("-- PowerUp/Spoils Interaction")]
	public bool m_stopOnPowerupHit = true;
	public bool m_includeSpoilsPowerups = true;
	public bool m_ignorePickupTeamRestriction;
	[Header("-- Sequences --")]
	public GameObject m_onCastSequencePrefab;
	public GameObject m_powerupReturnPrefab;

	private AbilityMod_ThiefBasicAttack m_abilityMod;
	private int c_maxPowerupPerLaser = 1;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Strong Arms";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		float targeterMaxAngle = GetTargeterMaxAngle();
		bool stopOnPowerUp = StopOnPowerupHit();
		bool hasHealOnSelf = GetHealOnSelfIfHitEnemyAndPowerup() > 0;
		if (TargeterMultiTarget())
		{
			ClearTargeters();
			for (int i = 0; i < GetLaserCount(); i++)
			{
				AbilityUtil_Targeter_ThiefFanLaser targeter = new AbilityUtil_Targeter_ThiefFanLaser(
					this,
					0f,
					targeterMaxAngle,
					m_targeterMinInterpDistance,
					m_targeterMaxInterpDistance,
					GetLaserRange(),
					GetLaserWidth(),
					GetLaserMaxTargets(),
					GetLaserCount(),
					LaserPenetrateLos(),
					true,
					stopOnPowerUp,
					IncludeSpoilsPowerups(),
					IgnorePickupTeamRestriction(),
					c_maxPowerupPerLaser);
				targeter.SetUseMultiTargetUpdate(true);
				if (hasHealOnSelf)
				{
					targeter.m_affectCasterDelegate = (caster, hitEnemy, hitPowerup) => hitEnemy && hitPowerup;
				}
				Targeters.Add(targeter);
			}
		}
		else
		{
			AbilityUtil_Targeter_ThiefFanLaser targeter = new AbilityUtil_Targeter_ThiefFanLaser(
				this,
				0f,
				targeterMaxAngle,
				m_targeterMinInterpDistance,
				m_targeterMaxInterpDistance,
				GetLaserRange(),
				GetLaserWidth(),
				GetLaserMaxTargets(),
				GetLaserCount(),
				LaserPenetrateLos(),
				true,
				stopOnPowerUp,
				IncludeSpoilsPowerups(),
				IgnorePickupTeamRestriction(),
				c_maxPowerupPerLaser);
			if (hasHealOnSelf)
			{
				targeter.m_affectCasterDelegate = (caster, hitEnemy, hitPowerup) => hitEnemy && hitPowerup;
			}
			Targeter = targeter;
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return TargeterMultiTarget()
			? GetLaserCount()
			: 1;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	public bool TargeterMultiTarget()
	{
		return m_targeterMultiTarget;
	}

	public float GetTargeterMaxAngle()
	{
		float targeterMaxAngle = m_abilityMod != null
			? m_abilityMod.m_targeterMaxAngleMod.GetModifiedValue(m_targeterMaxAngle)
			: m_targeterMaxAngle;
		return Mathf.Max(1f, targeterMaxAngle);
	}

	public int GetLaserDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public int GetLaserSubsequentDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserSubsequentDamageAmountMod.GetModifiedValue(m_laserSubsequentDamageAmount)
			: m_laserSubsequentDamageAmount;
	}

	public int GetExtraDamageForSingleHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit)
			: m_extraDamageForSingleHit;
	}

	public int GetExtraDamageForHittingPowerup()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForHittingPowerupMod.GetModifiedValue(m_extraDamageForHittingPowerup)
			: m_extraDamageForHittingPowerup;
	}

	public int GetHealOnSelfIfHitEnemyAndPowerup()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healOnSelfIfHitEnemyAndPowerupMod.GetModifiedValue(m_healOnSelfIfHitEnemyAndPowerup)
			: m_healOnSelfIfHitEnemyAndPowerup;
	}

	public int GetEnergyGainPerLaserHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyGainPerLaserHitMod.GetModifiedValue(m_energyGainPerLaserHit)
			: m_energyGainPerLaserHit;
	}

	public int GetEnergyGainPerPowerupHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyGainPerPowerupHitMod.GetModifiedValue(m_energyGainPerPowerupHit)
			: m_energyGainPerPowerupHit;
	}

	public float GetLaserRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange)
			: m_laserRange;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public int GetLaserMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets)
			: m_laserMaxTargets;
	}

	public int GetLaserCount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserCountMod.GetModifiedValue(m_laserCount)
			: m_laserCount;
	}

	public bool LaserPenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserPenetrateLosMod.GetModifiedValue(m_laserPenetrateLos)
			: m_laserPenetrateLos;
	}

	public bool StopOnPowerupHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_stopOnPowerupHitMod.GetModifiedValue(m_stopOnPowerupHit)
			: m_stopOnPowerupHit;
	}

	public bool IncludeSpoilsPowerups()
	{
		return m_abilityMod != null
			? m_abilityMod.m_includeSpoilsPowerupsMod.GetModifiedValue(m_includeSpoilsPowerups)
			: m_includeSpoilsPowerups;
	}

	public bool IgnorePickupTeamRestriction()
	{
		return m_abilityMod != null
			? m_abilityMod.m_ignorePickupTeamRestrictionMod.GetModifiedValue(m_ignorePickupTeamRestriction)
			: m_ignorePickupTeamRestriction;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, 1);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, 1);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			AccumulateDamageFromTargeter(targetActor, Targeter, dictionary);
		}
		else
		{
			for (int i = 0; i <= currentTargeterIndex; i++)
			{
				AccumulateDamageFromTargeter(targetActor, Targeters[i], dictionary);
			}
		}
		return dictionary;
	}

	private void AccumulateDamageFromTargeter(ActorData targetActor, AbilityUtil_Targeter targeter, Dictionary<AbilityTooltipSymbol, int> symbolToDamage)
	{
		bool hitPowerups = targeter is AbilityUtil_Targeter_ThiefFanLaser targeterThiefFanLaser
		             && targeterThiefFanLaser.m_powerupsHitSoFar != null
		             && targeterThiefFanLaser.m_powerupsHitSoFar.Count > 0;
		List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return;
		}
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
		{
			int tooltipSubjectCountOnActor = targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary);
			if (tooltipSubjectCountOnActor > 0)
			{
				int damage = GetLaserDamageAmount() + (tooltipSubjectCountOnActor - 1) * GetLaserSubsequentDamageAmount();
				if (hitPowerups)
				{
					damage += GetExtraDamageForHittingPowerup();
				}
				if (tooltipSubjectCountOnActor == 1)
				{
					damage += GetExtraDamageForSingleHit();
				}
				symbolToDamage[AbilityTooltipSymbol.Damage] = damage;
			}
		}
		if (targetActor == ActorData && hitPowerups)
		{
			symbolToDamage[AbilityTooltipSymbol.Healing] = GetHealOnSelfIfHitEnemyAndPowerup();
		}
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int energyGain = 0;
		int energyGainPerLaserHit = GetEnergyGainPerLaserHit();
		int energyGainPerPowerupHit = GetEnergyGainPerPowerupHit();
		
		for (int i = 0; i < Targeters.Count && i <= currentTargeterIndex; i++)
		{
			if (!(Targeters[i] is AbilityUtil_Targeter_ThiefFanLaser targeter))
			{
				continue;
			}
			for (int j = 0;
			     j < targeter.m_hitPowerupInLaser.Count
			     && j < targeter.m_hitActorInLaser.Count;
			     j++)
			{
				// added in rogues
				// if (targeter.m_hitActorInLaser[j])
				// {
				// 	energyGain += energyGainPerLaserHit;
				// }
				// else
				// end added in rogues
				if (targeter.m_hitPowerupInLaser[j])
				{
					energyGain += energyGainPerPowerupHit;
				}
			}
			// removed in rogues
			if (energyGainPerLaserHit > 0)
			{
				int numHits = 0;
				foreach (KeyValuePair<ActorData, int> keyValuePair in targeter.m_actorToHitCount)
				{
					numHits += keyValuePair.Value;
				}
				energyGain += numHits * energyGainPerLaserHit;
			}
			// end removed in rogues
		}
		return energyGain;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefBasicAttack abilityMod_ThiefBasicAttack = modAsBase as AbilityMod_ThiefBasicAttack;
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, abilityMod_ThiefBasicAttack != null
			? abilityMod_ThiefBasicAttack.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount);
		AddTokenInt(tokens, "LaserSubsequentDamageAmount", string.Empty, abilityMod_ThiefBasicAttack != null
			? abilityMod_ThiefBasicAttack.m_laserSubsequentDamageAmountMod.GetModifiedValue(m_laserSubsequentDamageAmount)
			: m_laserSubsequentDamageAmount);
		AddTokenInt(tokens, "LaserDamageTotalCombined", string.Empty, m_laserDamageAmount + m_laserSubsequentDamageAmount);
		AddTokenInt(tokens, "ExtraDamageForSingleHit", string.Empty, abilityMod_ThiefBasicAttack != null
			? abilityMod_ThiefBasicAttack.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit)
			: m_extraDamageForSingleHit);
		AddTokenInt(tokens, "ExtraDamageForHittingPowerup", string.Empty, abilityMod_ThiefBasicAttack != null
			? abilityMod_ThiefBasicAttack.m_extraDamageForHittingPowerupMod.GetModifiedValue(m_extraDamageForHittingPowerup)
			: m_extraDamageForHittingPowerup);
		AddTokenInt(tokens, "HealOnSelfIfHitEnemyAndPowerup", string.Empty, abilityMod_ThiefBasicAttack != null
			? abilityMod_ThiefBasicAttack.m_healOnSelfIfHitEnemyAndPowerupMod.GetModifiedValue(m_healOnSelfIfHitEnemyAndPowerup)
			: m_healOnSelfIfHitEnemyAndPowerup);
		AddTokenInt(tokens, "EnergyGainPerLaserHit", string.Empty, abilityMod_ThiefBasicAttack != null
			? abilityMod_ThiefBasicAttack.m_energyGainPerLaserHitMod.GetModifiedValue(m_energyGainPerLaserHit)
			: m_energyGainPerLaserHit);
		AddTokenInt(tokens, "EnergyGainPerPowerupHit", string.Empty, abilityMod_ThiefBasicAttack != null
			? abilityMod_ThiefBasicAttack.m_energyGainPerPowerupHitMod.GetModifiedValue(m_energyGainPerPowerupHit)
			: m_energyGainPerPowerupHit);
		AddTokenInt(tokens, "LaserMaxTargets", string.Empty, abilityMod_ThiefBasicAttack != null
			? abilityMod_ThiefBasicAttack.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets)
			: m_laserMaxTargets);
		AddTokenInt(tokens, "LaserCount", string.Empty, abilityMod_ThiefBasicAttack != null
			? abilityMod_ThiefBasicAttack.m_laserCountMod.GetModifiedValue(m_laserCount)
			: m_laserCount);
	}

	public override bool HasRestrictedFreePosDistance(
		ActorData aimingActor,
		int targetIndex,
		List<AbilityTarget> targetsSoFar,
		out float min,
		out float max)
	{
		min = m_targeterMinInterpDistance * Board.Get().squareSize;
		max = m_targeterMaxInterpDistance * Board.Get().squareSize;
		return true;
	}

	private List<ActorData> GetHitActorsInDirection(
		Vector3 direction,
		ActorData caster,
		HashSet<PowerUp> powerupsHitPreviously,
		out VectorUtils.LaserCoords endPoints,
		out List<PowerUp> powerupsHit,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return GetHitActorsInDirectionStatic(
			caster.GetLoSCheckPos(),
			direction,
			caster,
			GetLaserRange(),
			GetLaserWidth(),
			LaserPenetrateLos(),
			GetLaserMaxTargets(),
			false,
			true,
			true,
			c_maxPowerupPerLaser,
			true,
			StopOnPowerupHit(),
			IncludeSpoilsPowerups(),
			IgnorePickupTeamRestriction(),
			powerupsHitPreviously,
			out endPoints,
			out powerupsHit,
			nonActorTargetInfo,
			false);
	}

	public static List<ActorData> GetHitActorsInDirectionStatic(
		Vector3 startLosCheckPos,
		Vector3 direction,
		ActorData caster,
		float distanceInSquares,
		float widthInSquares,
		bool penetrateLos,
		int maxActorTargets,
		bool includeAllies,
		bool includeEnemies,
		bool includeInvisibles,
		int maxPowerupsCount,
		bool shouldIncludePowerups,
		bool stopOnPowerupHit,
		bool includeSpoils,
		bool ignoreTeamRestriction,
		HashSet<PowerUp> powerupsHitSoFar,
		out VectorUtils.LaserCoords outEndPoints,
		out List<PowerUp> outPowerupsHit,
		List<NonActorTargetInfo> nonActorTargetInfo,
		bool forClient,
		bool stopEndPosOnHitActor = true)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, includeAllies, includeEnemies);
		List<PowerUp> hitPowerups = new List<PowerUp>();
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = startLosCheckPos;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			direction,
			distanceInSquares,
			widthInSquares,
			caster,
			relevantTeams,
			penetrateLos,
			maxActorTargets,
			false,
			includeInvisibles,
			out laserCoords.end,
			nonActorTargetInfo);
		List<ActorData> hitActors = actorsInLaser;
		Vector3 end = laserCoords.end; // removed in rogues
		if (maxActorTargets > 0
		    && actorsInLaser.Count > 0
		    && stopEndPosOnHitActor)
		{
			laserCoords.end = actorsInLaser[actorsInLaser.Count - 1].GetLoSCheckPos();
		}
		if (shouldIncludePowerups)
		{
			List<BoardSquare> squaresInBox = AreaEffectUtils.GetSquaresInBox(
				laserCoords.start,
				end, // laserCoords.end in rogues
				widthInSquares / 2f,
				true,
				caster);
			foreach (BoardSquare square in squaresInBox)
			{
				PowerUp powerupHit = null;
				List<PowerUp> powerups = forClient
					? PowerUpManager.Get().GetClientPowerUpsOnSquare(square)
					: PowerUpManager.Get().GetServerPowerUpsOnSquare(square);
				foreach (PowerUp powerup in powerups)
				{
					if (CanPowerupBeStolen(powerup, powerupsHitSoFar, ignoreTeamRestriction, caster))
					{
						powerupHit = powerup;
						break;
					}
				}
				if (CanPowerupBeStolen(powerupHit, powerupsHitSoFar, ignoreTeamRestriction, caster)
				    && !hitPowerups.Contains(powerupHit)
				    && (!powerupHit.m_isSpoil || includeSpoils))
				{
					hitPowerups.Add(powerupHit);
				}
			}
			if (hitPowerups.Count > 0)
			{
				TargeterUtils.SortPowerupsByDistanceToPos(ref hitPowerups, startLosCheckPos);
				if (maxPowerupsCount > 0 && hitPowerups.Count > maxPowerupsCount)
				{
					hitPowerups.RemoveRange(maxPowerupsCount, hitPowerups.Count - maxPowerupsCount);
				}
				if (stopOnPowerupHit)
				{
					PowerUp hitPowerup = hitPowerups[0];
					float distToPowerup = (hitPowerup.boardSquare.ToVector3() - startLosCheckPos).magnitude;
					if (hitActors.Count > 0)
					{
						float distToActor = (hitActors[0].GetLoSCheckPos() - startLosCheckPos).magnitude;
						if (distToPowerup < distToActor)
						{
							hitActors.Clear();
						}
					}
					laserCoords.end = hitPowerup.boardSquare.ToVector3();
				}
				powerupsHitSoFar.UnionWith(hitPowerups);
			}
		}
		outEndPoints = laserCoords;
		outPowerupsHit = hitPowerups;
		return hitActors;
	}

	private static bool CanPowerupBeStolen(
		PowerUp powerUp,
		HashSet<PowerUp> powerupsHitSoFar,
		bool ignoreTeamRestriction,
		ActorData thief)
	{
		return powerUp != null
		       && powerUp.boardSquare != null
		       && (ignoreTeamRestriction || powerUp.TeamAllowedForPickUp(thief.GetTeam()))
		       && !powerupsHitSoFar.Contains(powerUp)
		       && powerUp.CanBeStolen();
	}

	private float CalculateFanAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float distInSquares = (currentTarget.FreePos - targetingActor.GetFreePos()).magnitude / Board.Get().squareSize;
		float share = Mathf.Clamp(distInSquares, m_targeterMinInterpDistance, m_targeterMaxInterpDistance) - m_targeterMinInterpDistance;
		return GetTargeterMaxAngle() * (1f - share / (m_targeterMaxInterpDistance - m_targeterMinInterpDistance));
	}

	public float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees)
	{
		return AbilityCommon_FanLaser.CalculateDistanceFromFanAngleDegrees(
			fanAngleDegrees,
			GetTargeterMaxAngle(),
			m_targeterMinInterpDistance,
			m_targeterMaxInterpDistance);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ThiefBasicAttack))
		{
			m_abilityMod = abilityMod as AbilityMod_ThiefBasicAttack;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		GetSequencePositionAndTargets(
			targets,
			caster,
			out List<Vector3> sequenceTargetPositions,
			out List<List<ActorData>> sequenceTargets,
			out List<List<PowerUp>> powerupHitList);
		for (int i = 0; i < sequenceTargetPositions.Count; i++)
		{
			if (powerupHitList[i].Count > 0 && m_powerupReturnPrefab != null)
			{
				sequenceTargets[i].Remove(caster);
			}
			list.Add(new ServerClientUtils.SequenceStartData(
				m_onCastSequencePrefab,
				sequenceTargetPositions[i],
				sequenceTargets[i].ToArray(),
				caster,
				additionalData.m_sequenceSource));
			if (powerupHitList[i].Count > 0 && m_powerupReturnPrefab != null)
			{
				foreach (PowerUp powerUp in powerupHitList[i])
				{
					list.Add(new ServerClientUtils.SequenceStartData(m_powerupReturnPrefab,
						caster.GetFreePos(),
						caster.AsArray(),
						caster,
						additionalData.m_sequenceSource,
						new List<Sequence.IExtraSequenceParams>
						{
							new SplineProjectileSequence.DelayedProjectileExtraParams
							{
								useOverrideStartPos = true,
								overrideStartPos = powerUp.gameObject.transform.position
							},
							new ThiefPowerupReturnProjectileSequence.PowerupTypeExtraParams
							{
								powerupCategory = (int)powerUp.m_chatterCategory
							}
						}.ToArray()));
				}
			}
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Dictionary<ActorData, int> hitActorToDamage = new Dictionary<ActorData, int>();
		Dictionary<ActorData, int> hitActorToNumHits = new Dictionary<ActorData, int>();
		HashSet<PowerUp> powerupsHitSoFar = new HashSet<PowerUp>();
		List<List<NonActorTargetInfo>> nonActorTargetsList = new List<List<NonActorTargetInfo>>();
		List<Vector3> posForHitList = new List<Vector3>();
		int powerupHitNum = 0;
		bool hasHitPowerups = false;
		int healing = 0;
		foreach (Vector3 direction in GetLaserDirections(targets, caster))
		{
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			List<ActorData> hitActorsInDirection = GetHitActorsInDirection(
				direction,
				caster,
				powerupsHitSoFar,
				out VectorUtils.LaserCoords endPoints,
				out List<PowerUp> powerupsHit,
				nonActorTargetInfo);
			posForHitList.Add(endPoints.end);
			nonActorTargetsList.Add(nonActorTargetInfo);
			if (powerupsHit.Count > 0)
			{
				hasHitPowerups = true;
			}
			foreach (ActorData hitActor in hitActorsInDirection)
			{
				if (!hitActorToDamage.ContainsKey(hitActor))
				{
					hitActorToDamage[hitActor] = GetLaserDamageAmount();
					hitActorToNumHits[hitActor] = 1;
				}
				else
				{
					hitActorToDamage[hitActor] += GetLaserSubsequentDamageAmount();
					hitActorToNumHits[hitActor]++;
				}
			}
			if (powerupsHit.Count > 0)
			{
				hasHitPowerups = true;
				if (hitActorsInDirection.Count == 0)
				{
					powerupHitNum++;
				}
			}
		}
		foreach (ActorData hitActor in hitActorToDamage.Keys.ToList())
		{
			if (hasHitPowerups)
			{
				hitActorToDamage[hitActor] += GetExtraDamageForHittingPowerup();
			}
			if (hitActorToNumHits[hitActor] == 1)
			{
				hitActorToDamage[hitActor] += GetExtraDamageForSingleHit();
			}
		}
		if (hasHitPowerups && hitActorToDamage.Count > 0)
		{
			healing = GetHealOnSelfIfHitEnemyAndPowerup();
		}
		ThiefCreateSpoilsMarkerEffect thiefCreateSpoilsMarkerEffect = ServerEffectManager.Get()
			.GetEffect(caster, typeof(ThiefCreateSpoilsMarkerEffect)) as ThiefCreateSpoilsMarkerEffect;
		bool removedEffect = false;
		foreach (ActorData hitActor in hitActorToDamage.Keys)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, caster.GetFreePos()));
			actorHitResults.SetBaseDamage(hitActorToDamage[hitActor]);
			if (GetEnergyGainPerLaserHit() > 0)
			{
				actorHitResults.SetTechPointGainOnCaster(GetEnergyGainPerLaserHit() * hitActorToNumHits[hitActor]);
			}
			if (thiefCreateSpoilsMarkerEffect != null && !removedEffect)
			{
				actorHitResults.AddEffectForRemoval(thiefCreateSpoilsMarkerEffect, ServerEffectManager.Get().GetActorEffects(caster));
				removedEffect = true;
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (powerupsHitSoFar.Count > 0 || healing != 0)
		{
			ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			foreach (PowerUp powerup in powerupsHitSoFar)
			{
				casterHitResults.AddPowerUpForSteal(powerup);
			}
			if (GetEnergyGainPerPowerupHit() > 0 && powerupHitNum > 0)
			{
				casterHitResults.AddTechPointGainOnCaster(powerupHitNum * GetEnergyGainPerPowerupHit());
			}
			if (healing != 0)
			{
				casterHitResults.AddBaseHealing(healing);
			}
			abilityResults.StoreActorHit(casterHitResults);
		}
		ProcessAndStoreBarrierNonActorHits(nonActorTargetsList, posForHitList, caster, abilityResults);
		foreach (List<NonActorTargetInfo> nonActorTargetInfo in nonActorTargetsList)
		{
			abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
	}

	// added in rogues
	private void GetSequencePositionAndTargets(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<Vector3> sequenceTargetPositions,
		out List<List<ActorData>> sequenceTargets,
		out List<List<PowerUp>> powerupHitList)
	{
		sequenceTargetPositions = new List<Vector3>();
		sequenceTargets = new List<List<ActorData>>();
		powerupHitList = new List<List<PowerUp>>();
		HashSet<PowerUp> powerupsHitSoFar = new HashSet<PowerUp>();
		foreach (Vector3 direction in GetLaserDirections(targets, caster))
		{
			List<ActorData> hitActorsInDirection = GetHitActorsInDirection(
				direction,
				caster,
				powerupsHitSoFar,
				out VectorUtils.LaserCoords endPoints,
				out List<PowerUp> powerupsHit,
				null);
			if (powerupsHit.Count > 0 && !hitActorsInDirection.Contains(caster))
			{
				hitActorsInDirection.Add(caster);
			}
			sequenceTargets.Add(hitActorsInDirection);
			sequenceTargetPositions.Add(endPoints.end);
			powerupHitList.Add(new List<PowerUp>(powerupsHit));
		}
	}

	// added in rogues
	private List<Vector3> GetLaserDirections(List<AbilityTarget> targets, ActorData caster)
	{
		int laserCount = GetLaserCount();
		List<Vector3> list = new List<Vector3>();
		if (TargeterMultiTarget())
		{
			float targeterMaxAngle = GetTargeterMaxAngle();
			for (int i = 0; i < laserCount; i++)
			{
				if (i >= targets.Count)
				{
					break;
				}
				Vector3 aimDir = targets[i].AimDirection;
				if (i > 0 && targeterMaxAngle > 0f)
				{
					Vector3 prevAimDir = targets[i - 1].AimDirection;
					float angle = Vector3.Angle(aimDir, prevAimDir);
					if (angle > targeterMaxAngle)
					{
						aimDir = Vector3.RotateTowards(
							aimDir,
							prevAimDir, 
							0.0174532924f * (angle - targeterMaxAngle), 0f);
					}
				}
				list.Add(aimDir);
			}
		}
		else
		{
			float angleFull = laserCount > 1
				? CalculateFanAngleDegrees(targets[0], caster)
				: 0f;
			float angleStep = laserCount > 1
				? angleFull / (laserCount - 1)
				: 0f;
			float angleStart = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection) - 0.5f * angleFull;
			for (int j = 0; j < m_laserCount; j++)
			{
				list.Add(VectorUtils.AngleDegreesToVector(angleStart + j * angleStep));
			}
		}
		return list;
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster == target
		    && results.m_powerUpsToSteal != null
		    && results.m_powerUpsToSteal.Count > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(
				FreelancerStats.ThiefStats.PowerUpsStolen,
				results.m_powerUpsToSteal.Count);
		}
	}
	
	// custom
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		GetSequencePositionAndTargets(targets, caster, out List<Vector3> positions, out _, out _);
		return positions;
	}
#endif
}
