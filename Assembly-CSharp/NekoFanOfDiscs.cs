using System.Collections.Generic;
using UnityEngine;

public class NekoFanOfDiscs : Ability
{
	[Separator("Targeting", true)]
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

	[Separator("Hit On Throw", true)]
	public int m_directDamage = 25;

	public int m_directSubsequentHitDamage = 15;

	public StandardEffectInfo m_directEnemyHitEffect;

	[Separator("Return Trip", true)]
	public int m_returnTripDamage = 10;

	public int m_returnTripSubsequentHitDamage = 5;

	public bool m_returnTripIgnoreCover = true;

	public int m_returnTripEnergyOnCasterPerDiscMiss;

	[Separator("Effect on Self for misses", true)]
	public StandardEffectInfo m_effectOnSelfIfMissOnCast;

	public StandardEffectInfo m_effectOnSelfIfMissOnDiscReturn;

	[Separator("Zero Energy cost after N consecutive use", true)]
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
		base.Targeter = new AbilityUtil_Targeter_NekoDiscsFan(this, GetMinAngleForLaserFan(), GetMaxAngleForLaserFan(), GetAngleInterpMinDist(), GetAngleInterpMaxDist(), GetLaserRange(), GetLaserWidth(), GetAoeRadius(), GetMaxTargetsPerLaser(), GetNumDiscs(), false, m_interpStepInSquares, 0f);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_directDamage));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, m_returnTripDamage));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeter, targetActor, currentTargeterIndex, GetDirectDamage(), GetDirectSubsequentHitDamage());
		return symbolToValue;
	}

	public override int GetModdedCost()
	{
		if (m_syncComp != null)
		{
			if (GetZeroEnergyRequiredTurns() > 0)
			{
				if (m_syncComp.m_numUltConsecUsedTurns >= GetZeroEnergyRequiredTurns())
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return 0;
						}
					}
				}
			}
		}
		return base.GetModdedCost();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedDirectEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedDirectEnemyHitEffect = m_abilityMod.m_directEnemyHitEffectMod.GetModifiedValue(m_directEnemyHitEffect);
		}
		else
		{
			cachedDirectEnemyHitEffect = m_directEnemyHitEffect;
		}
		m_cachedDirectEnemyHitEffect = cachedDirectEnemyHitEffect;
		StandardEffectInfo cachedEffectOnSelfIfMissOnCast;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnSelfIfMissOnCast = m_abilityMod.m_effectOnSelfIfMissOnCastMod.GetModifiedValue(m_effectOnSelfIfMissOnCast);
		}
		else
		{
			cachedEffectOnSelfIfMissOnCast = m_effectOnSelfIfMissOnCast;
		}
		m_cachedEffectOnSelfIfMissOnCast = cachedEffectOnSelfIfMissOnCast;
		StandardEffectInfo cachedEffectOnSelfIfMissOnDiscReturn;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnSelfIfMissOnDiscReturn = m_abilityMod.m_effectOnSelfIfMissOnDiscReturnMod.GetModifiedValue(m_effectOnSelfIfMissOnDiscReturn);
		}
		else
		{
			cachedEffectOnSelfIfMissOnDiscReturn = m_effectOnSelfIfMissOnDiscReturn;
		}
		m_cachedEffectOnSelfIfMissOnDiscReturn = cachedEffectOnSelfIfMissOnDiscReturn;
	}

	public int GetNumDiscs()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_numDiscsMod.GetModifiedValue(m_numDiscs);
		}
		else
		{
			result = m_numDiscs;
		}
		return result;
	}

	public float GetMinAngleForLaserFan()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_minAngleForLaserFanMod.GetModifiedValue(m_minAngleForLaserFan);
		}
		else
		{
			result = m_minAngleForLaserFan;
		}
		return result;
	}

	public float GetMaxAngleForLaserFan()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_totalAngleForLaserFanMod.GetModifiedValue(m_totalAngleForLaserFan);
		}
		else
		{
			result = m_totalAngleForLaserFan;
		}
		return result;
	}

	public float GetAngleInterpMinDist()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_angleInterpMinDistMod.GetModifiedValue(m_angleInterpMinDist);
		}
		else
		{
			result = m_angleInterpMinDist;
		}
		return result;
	}

	public float GetAngleInterpMaxDist()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_angleInterpMaxDistMod.GetModifiedValue(m_angleInterpMaxDist);
		}
		else
		{
			result = m_angleInterpMaxDist;
		}
		return result;
	}

	public float GetLaserRange()
	{
		return (!m_abilityMod) ? m_laserRange : m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
	}

	public float GetLaserWidth()
	{
		return (!m_abilityMod) ? m_laserWidth : m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
	}

	public float GetAoeRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_aoeRadiusAtEndMod.GetModifiedValue(m_aoeRadiusAtEnd);
		}
		else
		{
			result = m_aoeRadiusAtEnd;
		}
		return result;
	}

	public int GetMaxTargetsPerLaser()
	{
		return (!m_abilityMod) ? m_maxTargetsPerLaser : m_abilityMod.m_maxTargetsPerLaserMod.GetModifiedValue(m_maxTargetsPerLaser);
	}

	public float GetInterpStepInSquares()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_interpStepInSquaresMod.GetModifiedValue(m_interpStepInSquares);
		}
		else
		{
			result = m_interpStepInSquares;
		}
		return result;
	}

	public float GetDiscReturnEndRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_discReturnEndRadiusMod.GetModifiedValue(m_discReturnEndRadius);
		}
		else
		{
			result = m_discReturnEndRadius;
		}
		return result;
	}

	public int GetDirectDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_directDamageMod.GetModifiedValue(m_directDamage);
		}
		else
		{
			result = m_directDamage;
		}
		return result;
	}

	public int GetDirectSubsequentHitDamage()
	{
		return (!m_abilityMod) ? m_directSubsequentHitDamage : m_abilityMod.m_directSubsequentHitDamageMod.GetModifiedValue(m_directSubsequentHitDamage);
	}

	public StandardEffectInfo GetDirectEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedDirectEnemyHitEffect != null)
		{
			result = m_cachedDirectEnemyHitEffect;
		}
		else
		{
			result = m_directEnemyHitEffect;
		}
		return result;
	}

	public int GetReturnTripDamage()
	{
		return (!m_abilityMod) ? m_returnTripDamage : m_abilityMod.m_returnTripDamageMod.GetModifiedValue(m_returnTripDamage);
	}

	public int GetReturnTripSubsequentHitDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_returnTripSubsequentHitDamageMod.GetModifiedValue(m_returnTripSubsequentHitDamage);
		}
		else
		{
			result = m_returnTripSubsequentHitDamage;
		}
		return result;
	}

	public bool ReturnTripIgnoreCover()
	{
		return (!m_abilityMod) ? m_returnTripIgnoreCover : m_abilityMod.m_returnTripIgnoreCoverMod.GetModifiedValue(m_returnTripIgnoreCover);
	}

	public int GetReturnTripEnergyOnCasterPerDiscMiss()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_returnTripEnergyOnCasterPerDiscMissMod.GetModifiedValue(m_returnTripEnergyOnCasterPerDiscMiss);
		}
		else
		{
			result = m_returnTripEnergyOnCasterPerDiscMiss;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelfIfMissOnCast()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnSelfIfMissOnCast != null)
		{
			result = m_cachedEffectOnSelfIfMissOnCast;
		}
		else
		{
			result = m_effectOnSelfIfMissOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelfIfMissOnDiscReturn()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnSelfIfMissOnDiscReturn != null)
		{
			result = m_cachedEffectOnSelfIfMissOnDiscReturn;
		}
		else
		{
			result = m_effectOnSelfIfMissOnDiscReturn;
		}
		return result;
	}

	public int GetZeroEnergyRequiredTurns()
	{
		return (!m_abilityMod) ? m_zeroEnergyRequiredTurns : m_abilityMod.m_zeroEnergyRequiredTurnsMod.GetModifiedValue(m_zeroEnergyRequiredTurns);
	}

	private LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo laserTargetingInfo = new LaserTargetingInfo();
		laserTargetingInfo.affectsEnemies = true;
		laserTargetingInfo.range = GetLaserRange();
		laserTargetingInfo.width = GetLaserWidth();
		laserTargetingInfo.maxTargets = GetMaxTargetsPerLaser();
		return laserTargetingInfo;
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
		if (abilityMod.GetType() != typeof(AbilityMod_NekoFanOfDiscs))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_NekoFanOfDiscs);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private Dictionary<ActorData, int> GetHitActorsAndHitCount(List<AbilityTarget> targets, ActorData caster, out List<List<ActorData>> actorsForSequence, out List<BoardSquare> targetSquares, out int numLasersWithHits, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		targetSquares = new List<BoardSquare>();
		List<Vector3> targetPosForSequences;
		Dictionary<ActorData, int> hitActorsAndHitCount = AbilityCommon_FanLaser.GetHitActorsAndHitCount(targets, caster, GetLaserInfo(), GetNumDiscs(), GetMaxAngleForLaserFan() / (float)GetNumDiscs(), true, GetMinAngleForLaserFan(), GetMaxAngleForLaserFan(), GetAngleInterpMinDist(), GetAngleInterpMaxDist(), out actorsForSequence, out targetPosForSequences, out numLasersWithHits, nonActorTargetInfo, false, m_interpStepInSquares);
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
		for (int i = 0; i < targetPosForSequences.Count; i++)
		{
			Vector3 vector = targetPosForSequences[i];
			Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(travelBoardSquareWorldPositionForLos, vector);
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(vector, GetAoeRadius(), false, caster, caster.GetEnemyTeam(), nonActorTargetInfo, true, coneLosCheckPos);
			using (List<ActorData>.Enumerator enumerator = actorsInRadius.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
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
		}
		while (true)
		{
			targetSquares = GetDiscSquaresFromEndPositions(targetPosForSequences, caster.GetTravelBoardSquareWorldPositionForLos());
			return hitActorsAndHitCount;
		}
	}

	public static List<BoardSquare> GetDiscSquaresFromEndPositions(List<Vector3> endPositions, Vector3 startPos)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		using (List<Vector3>.Enumerator enumerator = endPositions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Vector3 current = enumerator.Current;
				BoardSquare boardSquare = NekoBoomerangDisc.GetDiscEndSquare(startPos, current);
				if (list.Contains(boardSquare))
				{
					Vector3 pos = current;
					bool flag = false;
					for (int i = 1; i < 3; i++)
					{
						if (flag)
						{
							break;
						}
						List<BoardSquare> squares = AreaEffectUtils.GetSquaresInBorderLayer(boardSquare, i, true);
						AreaEffectUtils.SortSquaresByDistanceToPos(ref squares, pos);
						using (List<BoardSquare>.Enumerator enumerator2 = squares.GetEnumerator())
						{
							while (true)
							{
								if (!enumerator2.MoveNext())
								{
									break;
								}
								BoardSquare current2 = enumerator2.Current;
								if (current2.IsValidForGameplay())
								{
									if (!list.Contains(current2))
									{
										boardSquare = current2;
										flag = true;
										break;
									}
								}
							}
						}
					}
				}
				list.Add(boardSquare);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
	}
}
