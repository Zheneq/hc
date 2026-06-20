// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class RageBeastPlasmaEffect : Effect
{
	private int m_plasmaDamage;
	private int m_plasmaHealing;
	private int m_plasmaTechPointGain;
	private int m_plasmaDuration;
	private int m_spillingStateDuration;
	private AbilityAreaShape m_initialSpillShape;
	private AbilityAreaShape m_walkingSpillShape;
	private bool m_spillsPenetrateLoS;
	private bool m_clearCooldownsAtFirstTurnEnd;
	private GameObject m_groundSequencePrefab;
	private GameObject m_hitSequencePrefab;
	private GameObject m_buffSequencePrefab;
	private List<ActorData> m_actorsHitThisTurn;
	private List<ActorData> m_actorsHitThisTurn_fake;
	public Dictionary<BoardSquare, PlasmaSquareEntry> m_plasmaSquares;

	public RageBeastPlasmaEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData caster,
		int plasmaDamage,
		int plasmaDuration,
		int spillingStateDuration,
		AbilityAreaShape initialSpillShape,
		AbilityAreaShape walkingSpillShape,
		bool spillsPenetrateLoS,
		bool clearCooldownsAtFirstTurnEnd,
		List<ActorData> actorsHitThisTurn,
		GameObject groundSequencePrefab,
		GameObject hitSequencePrefab,
		GameObject buffSequencePrefab,
		int plasmaHealing,
		int plasmaTechPointGain)
		: base(parent, targetSquare, null, caster)
	{
		m_plasmaDamage = plasmaDamage;
		m_plasmaDuration = plasmaDuration;
		m_spillingStateDuration = spillingStateDuration;
		m_initialSpillShape = initialSpillShape;
		m_walkingSpillShape = walkingSpillShape;
		m_groundSequencePrefab = groundSequencePrefab;
		m_hitSequencePrefab = hitSequencePrefab;
		m_buffSequencePrefab = buffSequencePrefab;
		m_spillsPenetrateLoS = spillsPenetrateLoS;
		m_clearCooldownsAtFirstTurnEnd = clearCooldownsAtFirstTurnEnd;
		m_plasmaHealing = plasmaHealing;
		m_plasmaTechPointGain = plasmaTechPointGain;
		HitPhase = AbilityPriority.Combat_Damage;
		m_actorsHitThisTurn = actorsHitThisTurn;
		m_actorsHitThisTurn_fake = new List<ActorData>(actorsHitThisTurn);
		m_plasmaSquares = new Dictionary<BoardSquare, PlasmaSquareEntry>();
		m_time.duration = Mathf.Max(1, m_plasmaDuration + m_spillingStateDuration - 1);
	}

	public void AddPlasmaSquares(List<BoardSquare> squares)
	{
		foreach (BoardSquare addSquare in squares)
		{
			AddPlasmaSquare(addSquare);
		}
	}

	public void AddPlasmaSquare(BoardSquare addSquare)
	{
		if (CanSpill())
		{
			if (!m_plasmaSquares.ContainsKey(addSquare))
			{
				m_plasmaSquares.Add(addSquare, new PlasmaSquareEntry(m_plasmaDuration));
			}
			else if (m_plasmaSquares[addSquare].timeLeft < m_plasmaDuration)
			{
				m_plasmaSquares[addSquare].timeLeft = m_plasmaDuration;
			}
		}
	}

	public void RemovePlasmaSquare(BoardSquare removeSquare)
	{
		m_plasmaSquares.Remove(removeSquare);
	}

	private bool IsAffectedByPlasma(ActorData possibleTarget)
	{
		if (possibleTarget == null || possibleTarget.IgnoreForAbilityHits)
		{
			return false;
		}
		if (possibleTarget.GetTeam() != Caster.GetTeam())
		{
			return m_plasmaDamage > 0;
		}
		return m_plasmaHealing > 0 || m_plasmaTechPointGain > 0;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> effectStartSeqDataList = base.GetEffectStartSeqDataList();
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(
			m_buffSequencePrefab,
			Caster.GetSquareAtPhaseStart(),
			Caster.AsArray(),
			Caster,
			SequenceSource);
		effectStartSeqDataList.Add(item);
		if (CanSpill() && m_groundSequencePrefab != null)
		{
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(
				m_initialSpillShape, Caster.GetFreePos(), Caster.GetSquareAtPhaseStart(), m_spillsPenetrateLoS, Caster);
			foreach (BoardSquare targetSquare in squaresInShape)
			{
				effectStartSeqDataList.Add(new ServerClientUtils.SequenceStartData(
					m_groundSequencePrefab, targetSquare, null, Caster, SequenceSource));
			}
		}
		return effectStartSeqDataList;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> effectHitSeqDataList = base.GetEffectHitSeqDataList();
		if (CanSpill() && m_groundSequencePrefab != null)
		{
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(
				m_walkingSpillShape, Caster.GetFreePos(), Caster.GetSquareAtPhaseStart(), m_spillsPenetrateLoS, Caster);
			foreach (BoardSquare targetSquare in squaresInShape)
			{
				effectHitSeqDataList.Add(new ServerClientUtils.SequenceStartData(
					m_groundSequencePrefab, targetSquare, null, Caster, SequenceSource));
			}
		}
		ActorData[] hitActors = m_effectResults.HitActorsArray();
		if (hitActors.Length != 0 && m_hitSequencePrefab != null)
		{
			effectHitSeqDataList.Add(new ServerClientUtils.SequenceStartData(
				m_hitSequencePrefab, Caster.GetSquareAtPhaseStart(), hitActors, Caster, SequenceSource));
		}
		return effectHitSeqDataList;
	}

	public override void OnStart()
	{
		base.OnStart();
		if (CanSpill())
		{
			AddPlasmaSquares(AreaEffectUtils.GetSquaresInShape(
				m_initialSpillShape, Caster.GetFreePos(), Caster.GetSquareAtPhaseStart(), m_spillsPenetrateLoS, Caster));
		}
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		m_actorsHitThisTurn.Clear();
		m_actorsHitThisTurn_fake.Clear();
	}

	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		if (m_time.age == 1 && m_clearCooldownsAtFirstTurnEnd)
		{
			Caster.GetAbilityData().ClearCharacterAbilityCooldowns();
		}
		List<BoardSquare> list = new List<BoardSquare>();
		foreach (KeyValuePair<BoardSquare, PlasmaSquareEntry> keyValuePair in m_plasmaSquares)
		{
			keyValuePair.Value.timeLeft--;
			if (keyValuePair.Value.timeLeft == 0)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (BoardSquare removeSquare in list)
		{
			RemovePlasmaSquare(removeSquare);
		}
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		base.OnAbilityPhaseStart(phase);
		if (phase == AbilityPriority.Combat_Damage && CanSpill())
		{
			AddPlasmaSquares(AreaEffectUtils.GetSquaresInShape(
				m_walkingSpillShape, Caster.GetFreePos(), Caster.GetSquareAtPhaseStart(), m_spillsPenetrateLoS, Caster));
		}
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		base.GatherEffectResults(ref effectResults, isReal);
		List<ActorData> list = new List<ActorData>();
		foreach (BoardSquare boardSquare in m_plasmaSquares.Keys)
		{
			if (boardSquare.occupant != null)
			{
				ActorData actorData = boardSquare.occupant.GetComponent<ActorData>();
				if (IsAffectedByPlasma(actorData))
				{
					list.Add(actorData);
				}
			}
		}
		if (CanSpill())
		{
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(
				m_walkingSpillShape, Caster.GetFreePos(), Caster.GetSquareAtPhaseStart(), m_spillsPenetrateLoS, Caster);
			foreach (BoardSquare square in squaresInShape)
			{
				if (square.occupant != null)
				{
					ActorData actorData = square.occupant.GetComponent<ActorData>();
					if (IsAffectedByPlasma(actorData))
					{
						list.Add(actorData);
					}
				}
			}
		}
		foreach (ActorData actorData in list)
		{
			if (!GetActorsHitThisTurn(isReal).Contains(actorData))
			{
				ActorHitParameters hitParams = new ActorHitParameters(actorData, actorData.GetFreePos());
				if (actorData.GetTeam() != Caster.GetTeam())
				{
					effectResults.StoreActorHit(new ActorHitResults(m_plasmaDamage, HitActionType.Damage, hitParams));
				}
				else
				{
					ActorHitResults actorHitResults = new ActorHitResults(hitParams);
					actorHitResults.AddBaseHealing(m_plasmaHealing);
					actorHitResults.AddTechPointGain(m_plasmaTechPointGain);
					effectResults.StoreActorHit(actorHitResults);
				}
				AddActorHitThisTurn(actorData, isReal);
			}
		}
	}

	private bool CanSpill()
	{
		return m_time.age < m_spillingStateDuration;
	}

	public override void GatherMovementResults(MovementCollection movement, ref List<MovementResults> movementResultsList)
	{
		List<ServerAbilityUtils.TriggeringPathInfo> list = new List<ServerAbilityUtils.TriggeringPathInfo>();
		foreach (MovementInstance movementInstance in movement.m_movementInstances)
		{
			ActorData mover = movementInstance.m_mover;
			bool flag = m_plasmaSquares.ContainsKey(movementInstance.m_path.square);
			if (IsAffectedByPlasma(mover) && !m_actorsHitThisTurn.Contains(mover) && !flag)
			{
				for (BoardSquarePathInfo step = movementInstance.m_path; step != null; step = step.next)
				{
					BoardSquare square = step.square;
					if ((movementInstance.m_groundBased
					     || step.IsPathEndpoint()) && !step.IsPathStartPoint() && m_plasmaSquares.ContainsKey(square))
					{
						ServerAbilityUtils.TriggeringPathInfo item = new ServerAbilityUtils.TriggeringPathInfo(mover, step);
						list.Add(item);
						break;
					}
				}
			}
		}
		foreach (ServerAbilityUtils.TriggeringPathInfo triggeringPathInfo in list)
		{
			ActorHitParameters hitParams = new ActorHitParameters(triggeringPathInfo);
			ActorHitResults actorHitResults;
			if (triggeringPathInfo.m_mover.GetTeam() != Caster.GetTeam())
			{
				actorHitResults = new ActorHitResults(m_plasmaDamage, HitActionType.Damage, hitParams);
			}
			else
			{
				actorHitResults = new ActorHitResults(hitParams);
				actorHitResults.AddBaseHealing(m_plasmaHealing);
				actorHitResults.AddTechPointGain(m_plasmaTechPointGain);
			}
			MovementResults movementResults = new MovementResults(movement.m_movementStage);
			movementResults.SetupTriggerData(triggeringPathInfo);
			movementResults.SetupGameplayData(this, actorHitResults);
			movementResults.SetupSequenceData(m_hitSequencePrefab, TargetSquare, SequenceSource);
			movementResultsList.Add(movementResults);
			m_actorsHitThisTurn.Add(triggeringPathInfo.m_mover);
		}
	}

	public override void GatherMovementResultsFromSegment(
		ActorData mover,
		MovementInstance movementInstance,
		MovementStage movementStage,
		BoardSquarePathInfo sourcePath,
		BoardSquarePathInfo destPath,
		ref List<MovementResults> movementResultsList)
	{
		if (m_actorsHitThisTurn.Contains(mover)
		    || !IsAffectedByPlasma(mover)
		    || m_plasmaSquares.ContainsKey(sourcePath.square)
		    || !m_plasmaSquares.ContainsKey(destPath.square)
		    ||!movementInstance.m_groundBased && !destPath.IsPathEndpoint())
		{
			return;
		}
		ServerAbilityUtils.TriggeringPathInfo triggeringPathInfo = new ServerAbilityUtils.TriggeringPathInfo(mover, destPath);
		ActorHitParameters hitParams = new ActorHitParameters(triggeringPathInfo);
		ActorHitResults actorHitResults;
		if (triggeringPathInfo.m_mover.GetTeam() != Caster.GetTeam())
		{
			actorHitResults = new ActorHitResults(m_plasmaDamage, HitActionType.Damage, hitParams);
		}
		else
		{
			actorHitResults = new ActorHitResults(hitParams);
			actorHitResults.AddBaseHealing(m_plasmaHealing);
			actorHitResults.AddTechPointGain(m_plasmaTechPointGain);
		}
		MovementResults movementResults = new MovementResults(movementStage);
		movementResults.SetupTriggerData(triggeringPathInfo);
		movementResults.SetupGameplayData(this, actorHitResults);
		movementResults.SetupSequenceData(m_hitSequencePrefab, TargetSquare, SequenceSource);
		movementResultsList.Add(movementResults);
		m_actorsHitThisTurn.Add(triggeringPathInfo.m_mover);
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam() && m_plasmaSquares != null)
		{
			foreach (BoardSquare item in m_plasmaSquares.Keys)
			{
				squaresToAvoid.Add(item);
			}
		}
	}

	private List<ActorData> GetActorsHitThisTurn(bool isReal)
	{
		return isReal
			? m_actorsHitThisTurn
			: m_actorsHitThisTurn_fake;
	}

	private void AddActorHitThisTurn(ActorData actor, bool isReal)
	{
		if (isReal)
		{
			m_actorsHitThisTurn.Add(actor);
		}
		else
		{
			m_actorsHitThisTurn_fake.Add(actor);
		}
	}
}
#endif
