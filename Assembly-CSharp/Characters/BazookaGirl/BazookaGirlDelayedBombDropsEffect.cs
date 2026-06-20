// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class BazookaGirlDelayedBombDropsEffect : Effect
{
	public List<ActorData> m_targetActors;
	public BazookaGirlDroppedBombInfo m_bombInfo;
	
	private int m_maxNumAreasForExtraDamage;
	private int m_extraDamagePerFewerArea;
	private Vector3 m_casterPosOnCast;
	private int m_delayDuration;
	private AbilityPriority m_bombDropPhase;
	private int m_bombDropAnimIndexInEffect;
	private GameObject m_bombDropSequencePrefab;
	private GameObject m_warningSequencePrefab;
	private List<BoardSquare> m_bombDropSquares = new List<BoardSquare>();
	private bool m_markersPlaced;
	private bool m_bombDropped;
	private int m_useCinematics = -1;

	public BazookaGirlDelayedBombDropsEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData caster,
		List<ActorData> targetActors,
		BazookaGirlDroppedBombInfo bombInfo,
		int maxNumAreasForExtraDamage,
		int extraDamagePerFewerArea,
		Vector3 casterPosOnCast,
		int delayDuration,
		AbilityPriority bombDropPhase,
		int bombDropAnimIndexInEffect,
		GameObject warningSequencePrefab,
		GameObject bombDropSequencePrefab,
		int useCinematics)
		: base(parent, targetSquare, null, caster)
	{
		m_effectName = "Zuki- Ult - Delayed Bomb Drops";
		m_targetActors = targetActors;
		m_bombInfo = bombInfo;
		m_maxNumAreasForExtraDamage = maxNumAreasForExtraDamage;
		m_extraDamagePerFewerArea = extraDamagePerFewerArea;
		m_casterPosOnCast = casterPosOnCast;
		m_delayDuration = Mathf.Max(0, delayDuration);
		m_bombDropPhase = bombDropPhase;
		HitPhase = bombDropPhase;
		m_bombDropAnimIndexInEffect = bombDropAnimIndexInEffect;
		m_bombDropSequencePrefab = bombDropSequencePrefab;
		m_warningSequencePrefab = warningSequencePrefab;
		m_useCinematics = useCinematics;
		m_time.duration = m_delayDuration + 1;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> result = new List<ServerClientUtils.SequenceStartData>();
		AddWarningSequenceStartDataToList(ref result);
		return result;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_time.age >= m_delayDuration && !Caster.IsDead())
		{
			GetBombHitActorToDamage(out _, out var sequenceActors, out var sequenceHitPositions, null);
			for (int i = 0; i < sequenceHitPositions.Count; i++)
			{
				ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(m_bombDropSequencePrefab, sequenceHitPositions[i], sequenceActors[i].ToArray(), Caster, SequenceSource);
				list.Add(item);
			}
		}
		return list;
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		if (m_time.age >= m_delayDuration
		    // rogues
		    // && GameFlowData.Get().ActingTeam == Caster.GetTeam()
		    && !Caster.IsDead())
		{
			SetupBombDropSquares();
		}
	}

	public override void OnStart()
	{
		if (m_delayDuration <= 0)
		{
			SetupBombDropSquares();
		}
	}

	public override bool HitsCanBeReactedTo()
	{
		return true;
	}

	// rogues
	// public override bool CanExecuteForTeam_FCFS(Team team)
	// {
	// 	return team == Caster.GetTeam();
	// }

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_time.age < m_delayDuration
		    || !m_markersPlaced
		    || Caster.IsDead())
		{
			return;
		}
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Dictionary<ActorData, int> bombHitActorToDamage = GetBombHitActorToDamage(out var actorToDamageOrigin, out _, out var sequenceHitPositions, nonActorTargetInfo);
		foreach (ActorData actorData in bombHitActorToDamage.Keys)
		{
			ActorHitParameters hitParams = new ActorHitParameters(actorData, actorToDamageOrigin[actorData]);
			ActorHitResults actorHitResults = new ActorHitResults(bombHitActorToDamage[actorData], HitActionType.Damage, hitParams);
			actorHitResults.AddStandardEffectInfo(m_bombInfo.m_enemyHitEffect);
			effectResults.StoreActorHit(actorHitResults);
		}
		foreach (Vector3 pos in sequenceHitPositions)
		{
			PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(pos));
			positionHitResults.AddEffectSequenceToEnd(m_warningSequencePrefab, m_guid);
			effectResults.StorePositionHit(positionHitResults);
		}
		effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		if (phase == m_bombDropPhase && m_time.age >= m_delayDuration)
		{
			if (!Caster.IsDead())
			{
				DropBombs();
			}
			else
			{
				Caster.SetTechPoints(Caster.GetMaxTechPoints());
			}
		}
	}

	private void SetupBombDropSquares()
	{
		if (Caster.IsDead())
		{
			return;
		}
		if (m_markersPlaced)
		{
			Debug.LogError("Trying to place markers after they are already placed");
			return;
		}
		
		foreach (ActorData actorData in m_targetActors)
		{
			if (actorData != null && actorData.GetCurrentBoardSquare() != null && actorData.transform.position.y >= Board.Get().BaselineHeight - 1f)
			{
				m_bombDropSquares.Add(actorData.GetCurrentBoardSquare());
			}
		}

		m_markersPlaced = true;
	}

	private void AddWarningSequenceStartDataToList(ref List<ServerClientUtils.SequenceStartData> startDataList)
	{
		foreach (ActorData actorData in m_targetActors)
		{
			if (actorData != null)
			{
				ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(m_warningSequencePrefab, actorData.GetFreePos(), actorData.AsArray(), Caster, SequenceSource);
				startDataList.Add(item);
			}
		}
	}

	private void DropBombs()
	{
		if (!m_bombDropped)
		{
			m_bombDropped = true;
		}
	}

	public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
	{
		return ShouldPlayAnim(phaseIndex)
			? m_bombDropAnimIndexInEffect
			: 0;
	}

	public override int GetCinematicRequested(AbilityPriority phaseIndex)
	{
		return ShouldPlayAnim(phaseIndex)
			? m_useCinematics
			: -1;
	}

	private bool ShouldPlayAnim(AbilityPriority phaseIndex)
	{
		return m_time.age >= m_delayDuration
		       && m_bombDropPhase == phaseIndex
		       && m_targetActors.Count != 0
		       && !Caster.IsDead();
	}

	private Dictionary<ActorData, int> GetBombHitActorToDamage(out Dictionary<ActorData, Vector3> actorToDamageOrigin, out List<List<ActorData>> sequenceActors, out List<Vector3> sequenceHitPositions, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		actorToDamageOrigin = new Dictionary<ActorData, Vector3>();
		sequenceActors = new List<List<ActorData>>();
		sequenceHitPositions = new List<Vector3>();
		int extraDamage = 0;
		if (m_maxNumAreasForExtraDamage > 0 && m_extraDamagePerFewerArea > 0)
		{
			int num = m_maxNumAreasForExtraDamage - m_bombDropSquares.Count;
			if (num > 0)
			{
				extraDamage = m_extraDamagePerFewerArea * num;
			}
		}
		foreach (BoardSquare boardSquare in m_bombDropSquares)
		{
			List<ActorData> list = new List<ActorData>();
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_bombInfo.m_shape, boardSquare.ToVector3(), boardSquare);
			foreach (ActorData actorData in AreaEffectUtils.GetActorsInShape(m_bombInfo.m_shape, centerOfShape, boardSquare, m_bombInfo.m_penetrateLos, Caster, Caster.GetOtherTeams(), nonActorTargetInfo))
			{
				if (dictionary.ContainsKey(actorData))
				{
					dictionary[actorData] += m_bombInfo.m_subsequentDamageAmount;
				}
				else
				{
					dictionary[actorData] = m_bombInfo.m_damageAmount + extraDamage;
					actorToDamageOrigin[actorData] = centerOfShape;
					list.Add(actorData);
				}
			}
			sequenceActors.Add(list);
			sequenceHitPositions.Add(centerOfShape);
		}
		return dictionary;
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam() && m_delayDuration > 0)
		{
			foreach (BoardSquare boardSquare in m_bombDropSquares)
			{
				Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_bombInfo.m_shape, boardSquare.ToVector3(), boardSquare);
				List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(m_bombInfo.m_shape, centerOfShape, boardSquare, true, Caster);
				squaresToAvoid.UnionWith(squaresInShape);
			}
		}
	}
}
#endif
