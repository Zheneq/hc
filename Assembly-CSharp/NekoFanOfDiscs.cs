using System.Collections.Generic;
using UnityEngine;

public class NekoFanOfDiscs : Ability
{
	[Separator("Targeting")]
	public int m_numDiscs = 5;
	public float m_minAngleForLaserFan = 90f;
	public float m_totalAngleForLaserFan = 288f;
	public float m_angleInterpMinDist = 1f;
	public float m_angleInterpMaxDist = 6f;
	[Space(10f)]
	public float m_laserRange = 6f;
	public float m_laserWidth = 1f;
	public float m_aoeRadiusAtEnd = 1f;
	public int m_maxTargetsPerLaser;
	public float m_interpStepInSquares = 1f;
	[Header("-- Disc return end radius")]
	public float m_discReturnEndRadius;
	[Separator("Hit On Throw")]
	public int m_directDamage = 25;
	public int m_directSubsequentHitDamage = 15;
	public StandardEffectInfo m_directEnemyHitEffect;
	[Separator("Return Trip")]
	public int m_returnTripDamage = 10;
	public int m_returnTripSubsequentHitDamage = 5;
	public bool m_returnTripIgnoreCover = true;
	public int m_returnTripEnergyOnCasterPerDiscMiss;
	[Separator("Effect on Self for misses")]
	public StandardEffectInfo m_effectOnSelfIfMissOnCast;
	public StandardEffectInfo m_effectOnSelfIfMissOnDiscReturn;
	[Separator("Zero Energy cost after N consecutive use")]
	public int m_zeroEnergyRequiredTurns;
	[Header("Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_returnTripSequencePrefab;
	public GameObject m_persistentDiscSequencePrefab;

	private AbilityMod_NekoFanOfDiscs m_abilityMod;
	private Neko_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedDirectEnemyHitEffect;
	private StandardEffectInfo m_cachedEffectOnSelfIfMissOnCast;
	private StandardEffectInfo m_cachedEffectOnSelfIfMissOnDiscReturn;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Fan of Discs";
		}
		m_syncComp = GetComponent<Neko_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_NekoDiscsFan(
			this,
			GetMinAngleForLaserFan(),
			GetMaxAngleForLaserFan(),
			GetAngleInterpMinDist(),
			GetAngleInterpMaxDist(),
			GetLaserRange(),
			GetLaserWidth(),
			GetAoeRadius(),
			GetMaxTargetsPerLaser(),
			GetNumDiscs(),
			false,
			m_interpStepInSquares,
			0f);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_directDamage),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, m_returnTripDamage)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		AddNameplateValueForOverlap(ref symbolToValue, Targeter, targetActor, currentTargeterIndex, GetDirectDamage(), GetDirectSubsequentHitDamage());
		return symbolToValue;
	}

	public override int GetModdedCost()
	{
		return m_syncComp != null
		       && GetZeroEnergyRequiredTurns() > 0
		       && m_syncComp.m_numUltConsecUsedTurns >= GetZeroEnergyRequiredTurns()
			? 0
			: base.GetModdedCost();
	}

	private void SetCachedFields()
	{
		m_cachedDirectEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_directEnemyHitEffectMod.GetModifiedValue(m_directEnemyHitEffect)
			: m_directEnemyHitEffect;
		m_cachedEffectOnSelfIfMissOnCast = m_abilityMod != null
			? m_abilityMod.m_effectOnSelfIfMissOnCastMod.GetModifiedValue(m_effectOnSelfIfMissOnCast)
			: m_effectOnSelfIfMissOnCast;
		m_cachedEffectOnSelfIfMissOnDiscReturn = m_abilityMod != null
			? m_abilityMod.m_effectOnSelfIfMissOnDiscReturnMod.GetModifiedValue(m_effectOnSelfIfMissOnDiscReturn)
			: m_effectOnSelfIfMissOnDiscReturn;
	}

	public int GetNumDiscs()
	{
		return m_abilityMod != null
			? m_abilityMod.m_numDiscsMod.GetModifiedValue(m_numDiscs)
			: m_numDiscs;
	}

	public float GetMinAngleForLaserFan()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minAngleForLaserFanMod.GetModifiedValue(m_minAngleForLaserFan)
			: m_minAngleForLaserFan;
	}

	public float GetMaxAngleForLaserFan()
	{
		return m_abilityMod != null
			? m_abilityMod.m_totalAngleForLaserFanMod.GetModifiedValue(m_totalAngleForLaserFan)
			: m_totalAngleForLaserFan;
	}

	public float GetAngleInterpMinDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_angleInterpMinDistMod.GetModifiedValue(m_angleInterpMinDist)
			: m_angleInterpMinDist;
	}

	public float GetAngleInterpMaxDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_angleInterpMaxDistMod.GetModifiedValue(m_angleInterpMaxDist)
			: m_angleInterpMaxDist;
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

	public float GetAoeRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeRadiusAtEndMod.GetModifiedValue(m_aoeRadiusAtEnd)
			: m_aoeRadiusAtEnd;
	}

	public int GetMaxTargetsPerLaser()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsPerLaserMod.GetModifiedValue(m_maxTargetsPerLaser)
			: m_maxTargetsPerLaser;
	}

	public float GetInterpStepInSquares()
	{
		return m_abilityMod != null
			? m_abilityMod.m_interpStepInSquaresMod.GetModifiedValue(m_interpStepInSquares)
			: m_interpStepInSquares;
	}

	public float GetDiscReturnEndRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_discReturnEndRadiusMod.GetModifiedValue(m_discReturnEndRadius)
			: m_discReturnEndRadius;
	}

	public int GetDirectDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_directDamageMod.GetModifiedValue(m_directDamage)
			: m_directDamage;
	}

	public int GetDirectSubsequentHitDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_directSubsequentHitDamageMod.GetModifiedValue(m_directSubsequentHitDamage)
			: m_directSubsequentHitDamage;
	}

	public StandardEffectInfo GetDirectEnemyHitEffect()
	{
		return m_cachedDirectEnemyHitEffect ?? m_directEnemyHitEffect;
	}

	public int GetReturnTripDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_returnTripDamageMod.GetModifiedValue(m_returnTripDamage)
			: m_returnTripDamage;
	}

	public int GetReturnTripSubsequentHitDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_returnTripSubsequentHitDamageMod.GetModifiedValue(m_returnTripSubsequentHitDamage)
			: m_returnTripSubsequentHitDamage;
	}

	public bool ReturnTripIgnoreCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_returnTripIgnoreCoverMod.GetModifiedValue(m_returnTripIgnoreCover)
			: m_returnTripIgnoreCover;
	}

	public int GetReturnTripEnergyOnCasterPerDiscMiss()
	{
		return m_abilityMod != null
			? m_abilityMod.m_returnTripEnergyOnCasterPerDiscMissMod.GetModifiedValue(m_returnTripEnergyOnCasterPerDiscMiss)
			: m_returnTripEnergyOnCasterPerDiscMiss;
	}

	public StandardEffectInfo GetEffectOnSelfIfMissOnCast()
	{
		return m_cachedEffectOnSelfIfMissOnCast ?? m_effectOnSelfIfMissOnCast;
	}

	public StandardEffectInfo GetEffectOnSelfIfMissOnDiscReturn()
	{
		return m_cachedEffectOnSelfIfMissOnDiscReturn ?? m_effectOnSelfIfMissOnDiscReturn;
	}

	public int GetZeroEnergyRequiredTurns()
	{
		return m_abilityMod != null
			? m_abilityMod.m_zeroEnergyRequiredTurnsMod.GetModifiedValue(m_zeroEnergyRequiredTurns)
			: m_zeroEnergyRequiredTurns;
	}

	private LaserTargetingInfo GetLaserInfo()
	{
		return new LaserTargetingInfo
		{
			affectsEnemies = true,
			range = GetLaserRange(),
			width = GetLaserWidth(),
			maxTargets = GetMaxTargetsPerLaser()
		};
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "NumDiscs", string.Empty, m_numDiscs);
		AddTokenInt(tokens, "MaxTargetsPerLaser", string.Empty, m_maxTargetsPerLaser);
		AddTokenInt(tokens, "DirectDamage", string.Empty, m_directDamage);
		AddTokenInt(tokens, "DirectSubsequentHitDamage", string.Empty, m_directSubsequentHitDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_directEnemyHitEffect, "DirectEnemyHitEffect", m_directEnemyHitEffect);
		AddTokenInt(tokens, "ReturnTripDamage", string.Empty, m_returnTripDamage);
		AddTokenInt(tokens, "ReturnTripSubsequentHitDamage", string.Empty, m_returnTripSubsequentHitDamage);
		AddTokenInt(tokens, "ReturnTripEnergyOnCasterPerDiscMiss", string.Empty, m_returnTripEnergyOnCasterPerDiscMiss);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnSelfIfMissOnCast, "EffectOnSelfIfMissOnCast", m_effectOnSelfIfMissOnCast);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnSelfIfMissOnDiscReturn, "EffectOnSelfIfMissOnDiscReturn", m_effectOnSelfIfMissOnDiscReturn);
		AddTokenInt(tokens, "ZeroEnergyRequiredTurns", string.Empty, m_zeroEnergyRequiredTurns);
	}

	public override int GetTheatricsSortPriority(AbilityData.ActionType actionType)
	{
		return 3;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange() + GetAoeRadius();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NekoFanOfDiscs))
		{
			m_abilityMod = abilityMod as AbilityMod_NekoFanOfDiscs;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private Dictionary<ActorData, int> GetHitActorsAndHitCount(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<List<ActorData>> actorsForSequence,
		out List<BoardSquare> targetSquares,
		out int numLasersWithHits,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		targetSquares = new List<BoardSquare>();
		List<Vector3> targetPosForSequences;
		Dictionary<ActorData, int> hitActorsAndHitCount = AbilityCommon_FanLaser.GetHitActorsAndHitCount(
			targets,
			caster,
			GetLaserInfo(),
			GetNumDiscs(),
			GetMaxAngleForLaserFan() / GetNumDiscs(),
			true,
			GetMinAngleForLaserFan(),
			GetMaxAngleForLaserFan(),
			GetAngleInterpMinDist(),
			GetAngleInterpMaxDist(),
			out actorsForSequence,
			out targetPosForSequences,
			out numLasersWithHits,
			nonActorTargetInfo,
			false,
			m_interpStepInSquares);
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetLoSCheckPos();
		for (int i = 0; i < targetPosForSequences.Count; i++)
		{
			Vector3 centerPos = targetPosForSequences[i];
			Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(travelBoardSquareWorldPositionForLos, centerPos);
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
				centerPos,
				GetAoeRadius(),
				false,
				caster,
				caster.GetEnemyTeam(),
				nonActorTargetInfo,
				true,
				coneLosCheckPos);
			foreach (ActorData current in actorsInRadius)
			{
				if (!actorsForSequence[i].Contains(current))
				{
					if (!hitActorsAndHitCount.ContainsKey(current))
					{
						hitActorsAndHitCount.Add(current, 1);
					}
					else
					{
						hitActorsAndHitCount[current]++;
					}
					if (!actorsForSequence[i].Contains(current))
					{
						actorsForSequence[i].Add(current);
					}
				}
			}
		}
		targetSquares = GetDiscSquaresFromEndPositions(targetPosForSequences, caster.GetLoSCheckPos());
		return hitActorsAndHitCount;
	}

	public static List<BoardSquare> GetDiscSquaresFromEndPositions(List<Vector3> endPositions, Vector3 startPos)
	{
		List<BoardSquare> result = new List<BoardSquare>();
		foreach (Vector3 endPos in endPositions)
		{
			BoardSquare square = NekoBoomerangDisc.GetDiscEndSquare(startPos, endPos);
			if (result.Contains(square))
			{
				bool found = false;
				for (int i = 1; i < 3; i++)
				{
					if (found)
					{
						break;
					}
					List<BoardSquare> borderSquares = AreaEffectUtils.GetSquaresInBorderLayer(square, i, true);
					AreaEffectUtils.SortSquaresByDistanceToPos(ref borderSquares, endPos);
					foreach (BoardSquare borderSquare in borderSquares)
					{
						if (borderSquare.IsValidForGameplay() && !result.Contains(borderSquare))
						{
							square = borderSquare;
							found = true;
							break;
						}
					}
				}
			}
			result.Add(square);
		}
		return result;
	}
}
