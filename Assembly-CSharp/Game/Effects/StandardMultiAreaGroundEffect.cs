// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class StandardMultiAreaGroundEffect : Effect
{
	protected List<GroundAreaInfo> m_areaInfoList;
	public GroundEffectField m_fieldInfo; // protected in rogues
	protected HashSet<ActorData> m_actorsHitThisTurn;
	protected HashSet<ActorData> m_actorsHitThisTurn_fake;

	public StandardMultiAreaGroundEffect(
		EffectSource parent,
		List<GroundAreaInfo> areaInfoList,
		ActorData caster,
		GroundEffectField fieldInfo)
		: base(parent, null, null, caster)
	{
		m_time.duration = fieldInfo.duration;
		m_fieldInfo = fieldInfo;
		HitPhase = AbilityPriority.Combat_Damage;
		m_actorsHitThisTurn = new HashSet<ActorData>();
		m_actorsHitThisTurn_fake = new HashSet<ActorData>();
		m_areaInfoList = new List<GroundAreaInfo>();
		foreach (GroundAreaInfo groundAreaInfoPrefab in areaInfoList)
		{
			GroundAreaInfo groundAreaInfo = new GroundAreaInfo(groundAreaInfoPrefab.m_targetSquare, groundAreaInfoPrefab.m_shapeFreePos, m_fieldInfo.shape);
			foreach (ServerClientUtils.SequenceStartData sequenceStartData in groundAreaInfoPrefab.GetAdditionalPersistentSeqStartData())
			{
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(sequenceStartData.GetCasterActorIndex());
				if (actorData != null && sequenceStartData.GetServerOnlyPrefabReference() != null)
				{
					groundAreaInfo.AddSequenceStartDataToPersist(new ServerClientUtils.SequenceStartData(
						sequenceStartData.GetServerOnlyPrefabReference(),
						sequenceStartData.GetTargetPos(),
						null,
						actorData,
						SequenceSource,
						sequenceStartData.GetExtraParams()));
				}
			}
			m_areaInfoList.Add(groundAreaInfo);
		}
	}

	public override void OnStart()
	{
		foreach (GroundAreaInfo groundAreaInfo in m_areaInfoList)
		{
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(
				m_fieldInfo.shape,
				groundAreaInfo.m_shapeFreePos,
				groundAreaInfo.m_targetSquare,
				m_fieldInfo.penetrateLos,
				Caster);
			groundAreaInfo.InitAffectedSquares(squaresInShape);
		}
	}

	// custom
	public HashSet<ActorData> GetActorsInShape()
	{
		var result = new HashSet<ActorData>();
		bool includeEnemies = m_fieldInfo.IncludeEnemies();
		bool includeAllies = m_fieldInfo.IncludeAllies();
		foreach (GroundAreaInfo groundAreaInfo in m_areaInfoList)
		{
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
				m_fieldInfo.shape,
				groundAreaInfo.m_shapeFreePos,
				groundAreaInfo.m_targetSquare,
				m_fieldInfo.penetrateLos,
				Caster,
				TargeterUtils.GetRelevantTeams(Caster, includeAllies, includeEnemies),
				null);
			result.UnionWith(actorsInShape);
		}
		
		return result;
	}

	// custom
	public HashSet<BoardSquare> GetSquaresInShape()
	{
		var result = new HashSet<BoardSquare>();
		foreach (GroundAreaInfo groundAreaInfo in m_areaInfoList)
		{
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(
				m_fieldInfo.shape,
				groundAreaInfo.m_shapeFreePos,
				groundAreaInfo.m_targetSquare,
				m_fieldInfo.penetrateLos,
				Caster);
			result.UnionWith(squaresInShape);
		}

		return result;
	}

	public override void OnEnd()
	{
	}

	public override void OnTurnStart()
	{
		m_actorsHitThisTurn.Clear();
		m_actorsHitThisTurn_fake.Clear();
	}

	public override void OnTurnEnd()
	{
		MarkInactiveAreas();
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		base.OnAbilityPhaseStart(phase);
		if (phase == AbilityPriority.Prep_Defense)
		{
			m_actorsHitThisTurn.Clear();
			m_actorsHitThisTurn_fake.Clear();
			foreach (GroundAreaInfo groundAreaInfo in m_areaInfoList)
			{
				groundAreaInfo.ClearActorsHit();
			}
		}
	}

	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		base.OnAbilityPhaseEnd(phase);
		if (phase == HitPhase && m_fieldInfo.endIfHasDoneHits)
		{
			MarkInactiveAreas();
		}
	}

	public void AddToActorsHitThisTurn(List<ActorData> newActorsToExcludeThisTurn)
	{
		foreach (ActorData actorData in newActorsToExcludeThisTurn)
		{
			foreach (GroundAreaInfo groundAreaInfo in m_areaInfoList)
			{
				if (groundAreaInfo.IsSquareInArea(actorData.GetCurrentBoardSquare(), m_fieldInfo.shape, m_fieldInfo.penetrateLos, Caster))
				{
					groundAreaInfo.AddActorHit(actorData);
				}
			}
			m_actorsHitThisTurn.Add(actorData);
			m_actorsHitThisTurn_fake.Add(actorData);
		}
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		foreach (GroundAreaInfo groundAreaInfo in m_areaInfoList)
		{
			if (m_fieldInfo.endIfHasDoneHits && groundAreaInfo.GetActorsHit().Count != 0)
			{
				continue;
			}
			if (AreaEffectUtils.IsShapeOddByOdd(groundAreaInfo.m_shape))
			{
				list.Add(new ServerClientUtils.SequenceStartData(
					m_fieldInfo.persistentSequencePrefab,
					groundAreaInfo.m_targetSquare,
					null,
					Caster,
					SequenceSource));
			}
			else
			{
				list.Add(new ServerClientUtils.SequenceStartData(
					m_fieldInfo.persistentSequencePrefab,
					groundAreaInfo.GetShapeCenter(),
					null,
					Caster,
					SequenceSource));
			}
			list.AddRange(groundAreaInfo.GetAdditionalPersistentSeqStartData());
		}
		return list;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (!ShouldHitThisTurn())
		{
			return list;
		}
		SequenceSource shallowCopy = SequenceSource.GetShallowCopy();
		if (AddActorAnimEntryIfHasHits(HitPhase))
		{
			shallowCopy.SetWaitForClientEnable(true);
		}
		ActorData[] hitActorsArray = m_effectResults.HitActorsArray();
		foreach (GroundAreaInfo groundAreaInfo in m_areaInfoList)
		{
			if (groundAreaInfo.IsActive())
			{
				List<ActorData> hitActors = new List<ActorData>();
				List<ActorData> hitEnemies = new List<ActorData>();
				List<ActorData> hitAllies = new List<ActorData>();
				foreach (ActorData actorData in hitActorsArray)
				{
					if (groundAreaInfo.IsSquareInArea(actorData.GetCurrentBoardSquare()))
					{
						hitActors.Add(actorData);
						if (actorData.GetTeam() == Caster.GetTeam())
						{
							hitAllies.Add(actorData);
						}
						else
						{
							hitEnemies.Add(actorData);
						}
					}
				}
				if (m_fieldInfo.allyHitSequencePrefab != null && hitAllies.Count > 0)
				{
					list.Add(new ServerClientUtils.SequenceStartData(
						m_fieldInfo.allyHitSequencePrefab,
						groundAreaInfo.m_targetSquare,
						hitAllies.ToArray(),
						Caster,
						shallowCopy));
				}
				if (m_fieldInfo.enemyHitSequencePrefab != null && hitEnemies.Count > 0)
				{
					list.Add(new ServerClientUtils.SequenceStartData(
						m_fieldInfo.enemyHitSequencePrefab,
						groundAreaInfo.m_targetSquare,
						hitEnemies.ToArray(),
						Caster,
						shallowCopy));
				}
				GameObject hitPulseSequencePrefab = m_fieldInfo.hitPulseSequencePrefab;
				if (hitPulseSequencePrefab == null
				    && m_fieldInfo.allyHitSequencePrefab == null
				    && m_fieldInfo.enemyHitSequencePrefab == null)
				{
					hitPulseSequencePrefab = SequenceLookup.Get().GetSimpleHitSequencePrefab();
				}
				if (hitPulseSequencePrefab != null && hitActors.Count > 0)
				{
					list.Add(new ServerClientUtils.SequenceStartData(
						hitPulseSequencePrefab,
						groundAreaInfo.GetShapeCenter(),
						hitActors.ToArray(),
						Caster,
						shallowCopy,
						new SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam
						{
							ignoreStartEvent = true
						}.ToArray()));
				}
			}
		}
		return list;
	}

	public bool ShouldHitThisTurn()
	{
		return m_fieldInfo.hitDelayTurns <= 0 || m_time.age >= m_fieldInfo.hitDelayTurns;
	}

	public static Dictionary<ActorData, GroundFieldActorHitInfo> GetHitToResultMaps(
		GroundEffectField fieldInfo,
		ActorData caster,
		List<GroundAreaInfo> areaInfoList,
		out List<List<ActorData>> actorsInAreaList,
		out int numAreasWithHits,
		List<NonActorTargetInfo> nonActorTargetInfo,
		bool useShapeOverride = false,
		AbilityAreaShape shapeOverride = AbilityAreaShape.SingleSquare)
	{
		actorsInAreaList = new List<List<ActorData>>();
		numAreasWithHits = 0;
		bool includeEnemies = fieldInfo.IncludeEnemies();
		bool includeAllies = fieldInfo.IncludeAllies();
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, includeAllies, includeEnemies);
		AbilityAreaShape shape = fieldInfo.shape;
		if (useShapeOverride)
		{
			shape = shapeOverride;
		}
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		foreach (GroundAreaInfo groundAreaInfo in areaInfoList)
		{
			List<ActorData> list = new List<ActorData>();
			if (groundAreaInfo.IsActive())
			{
				foreach (ActorData actorData in AreaEffectUtils.GetActorsInShape(shape, groundAreaInfo.m_shapeFreePos, groundAreaInfo.m_targetSquare, fieldInfo.penetrateLos, caster, relevantTeams, nonActorTargetInfo))
				{
					if (fieldInfo.CanBeAffected(actorData, caster))
					{
						list.Add(actorData);
						if (dictionary.ContainsKey(actorData))
						{
							dictionary[actorData]++;
						}
						else
						{
							dictionary[actorData] = 1;
						}
					}
				}
			}
			actorsInAreaList.Add(list);
			if (list.Count > 0)
			{
				numAreasWithHits++;
			}
		}
		Dictionary<ActorData, GroundFieldActorHitInfo> dictionary3 = new Dictionary<ActorData, GroundFieldActorHitInfo>();
		foreach (ActorData actorData2 in dictionary.Keys)
		{
			GroundFieldActorHitInfo groundFieldActorHitInfo = new GroundFieldActorHitInfo(actorData2);
			int num = dictionary[actorData2];
			groundFieldActorHitInfo.m_hitCount = num;
			if (actorData2.GetTeam() != caster.GetTeam())
			{
				int damageAmount = fieldInfo.damageAmount;
				int num2 = Mathf.Max(0, (num - 1) * fieldInfo.subsequentDamageAmount);
				groundFieldActorHitInfo.m_damage = damageAmount + num2;
				groundFieldActorHitInfo.m_effectInfo = fieldInfo.effectOnEnemies;
			}
			else
			{
				int healAmount = fieldInfo.healAmount;
				int num3 = Mathf.Max(0, (num - 1) * fieldInfo.subsequentHealAmount);
				groundFieldActorHitInfo.m_healing = healAmount + num3;
				int energyGain = fieldInfo.energyGain;
				int num4 = Mathf.Max(0, (num - 1) * fieldInfo.subsequentEnergyGain);
				groundFieldActorHitInfo.m_energyGain = energyGain + num4;
				groundFieldActorHitInfo.m_effectInfo = fieldInfo.effectOnAllies;
			}
			dictionary3.Add(actorData2, groundFieldActorHitInfo);
		}
		return dictionary3;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (!ShouldHitThisTurn())
		{
			return;
		}

		Dictionary<ActorData, GroundFieldActorHitInfo> hitToResultMaps = GetHitToResultMaps(m_fieldInfo, Caster, m_areaInfoList, out List<List<ActorData>> list, out _, null);
		bool flag = false;
		foreach (ActorData actorData in hitToResultMaps.Keys)
		{
			if (!GetActorsHitThisTurn(isReal).Contains(actorData))
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetFreePos()));
				if (actorData.GetTeam() != Caster.GetTeam())
				{
					actorHitResults.SetBaseDamage(hitToResultMaps[actorData].m_damage);
				}
				else
				{
					actorHitResults.SetBaseHealing(hitToResultMaps[actorData].m_healing);
					actorHitResults.SetTechPointGain(hitToResultMaps[actorData].m_energyGain);
				}
				if (hitToResultMaps[actorData].m_effectInfo != null)
				{
					actorHitResults.AddStandardEffectInfo(hitToResultMaps[actorData].m_effectInfo);
				}
				if (m_fieldInfo.endIfHasDoneHits && !flag)
				{
					for (int i = 0; i < m_areaInfoList.Count; i++)
					{
						GroundAreaInfo groundAreaInfo = m_areaInfoList[i];
						if (list[i].Count > 0)
						{
							actorHitResults.AddEffectSequenceToEnd(m_fieldInfo.persistentSequencePrefab, m_guid, groundAreaInfo.GetShapeCenter());
							groundAreaInfo.AddSequencesToEndForHitResults(actorHitResults, m_guid);
						}
					}
					flag = true;
				}
				ProcessActorHit(actorHitResults, isReal); // custom
				effectResults.StoreActorHit(actorHitResults);
				AddActorHitThisTurn(actorData, isReal);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			GroundAreaInfo groundAreaInfo2 = m_areaInfoList[j];
			foreach (ActorData actor in list[j])
			{
				groundAreaInfo2.AddActorHit(actor);
			}
		}
	}

	// custom
	protected virtual void ProcessActorHit(ActorHitResults actorHitResults, bool isReal)
	{
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return true;
	}

	public override ActorData GetActorAnimationActor()
	{
		foreach (ActorData actorData in m_effectResults.HitActorsArray())
		{
			if (actorData != null && !actorData.IsDead())
			{
				return actorData;
			}
		}
		return Caster;
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || !HasActiveArea();
	}

	private bool HasActiveArea()
	{
		foreach (GroundAreaInfo areaInfo in m_areaInfoList)
		{
			if (areaInfo.IsActive())
			{
				return true;
			}
		}
		return false;
	}

	private void MarkInactiveAreas(bool doHitToRemoveSequence = false)
	{
		if (!m_fieldInfo.endIfHasDoneHits)
		{
			return;
		}
		foreach (GroundAreaInfo groundAreaInfo in m_areaInfoList)
		{
			if (groundAreaInfo.IsActive() && groundAreaInfo.GetActorsHit().Count > 0)
			{
				groundAreaInfo.MarkAsInactive();
			}
		}
	}

	public bool IsSquareInAnyShape(BoardSquare testSquare)
	{
		foreach (GroundAreaInfo groundAreaInfo in m_areaInfoList)
		{
			if (groundAreaInfo.IsActive() && groundAreaInfo.IsSquareInArea(testSquare))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsSquareInAnyShape(BoardSquare testSquare, out BoardSquare shapeCenterSquare)
	{
		shapeCenterSquare = null;
		foreach (GroundAreaInfo groundAreaInfo in m_areaInfoList)
		{
			if (groundAreaInfo.IsActive() && groundAreaInfo.IsSquareInArea(testSquare))
			{
				shapeCenterSquare = groundAreaInfo.m_targetSquare;
				return true;
			}
		}
		return false;
	}

	public override void GatherMovementResults(MovementCollection movement, ref List<MovementResults> movementResultsList)
	{
		if (m_fieldInfo.ignoreMovementHits || !ShouldHitThisTurn())
		{
			return;
		}
		bool includeEnemies = m_fieldInfo.IncludeEnemies();
		bool includeAllies = m_fieldInfo.IncludeAllies();
		List<ServerAbilityUtils.TriggeringPathInfo> triggeringPaths = new List<ServerAbilityUtils.TriggeringPathInfo>();
		List<BoardSquare> hitCenterSquares = new List<BoardSquare>();
		foreach (MovementInstance movementInstance in movement.m_movementInstances)
		{
			ActorData mover = movementInstance.m_mover;
			bool isEnemy = mover.GetTeam() != Caster.GetTeam();
			bool isAlly = !isEnemy;
			bool canBeAffected = m_fieldInfo.CanBeAffected(mover, Caster);
			bool isOutside = !IsSquareInAnyShape(mover.GetCurrentBoardSquare());
			if (canBeAffected && isOutside)
			{
				for (BoardSquarePathInfo boardSquarePathInfo = movementInstance.m_path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
				{
					BoardSquare square = boardSquarePathInfo.square;
					if ((movementInstance.m_groundBased || boardSquarePathInfo.IsPathEndpoint())
					    && !boardSquarePathInfo.IsPathStartPoint()
					    && ((isEnemy && includeEnemies) || (isAlly && includeAllies))
					    && IsSquareInAnyShape(square, out BoardSquare hitCenterSquare)
					    && !m_actorsHitThisTurn.Contains(mover))
					{
						triggeringPaths.Add(new ServerAbilityUtils.TriggeringPathInfo(mover, boardSquarePathInfo));
						hitCenterSquares.Add(hitCenterSquare);
						m_actorsHitThisTurn.Add(mover); // TODO knockbacks are not included in fake results?
					}
				}
			}
		}
		for (int i = 0; i < triggeringPaths.Count; i++)
		{
			SetUpMovementHit(triggeringPaths[i], hitCenterSquares[i], movement.m_movementStage, ref movementResultsList);
		}
	}

	private void SetUpMovementHit(
		ServerAbilityUtils.TriggeringPathInfo triggeringPath,
		BoardSquare hitCenterSquare,
		MovementStage movementStage,
		ref List<MovementResults> movementResultsList)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(triggeringPath));
		m_fieldInfo.SetupActorHitResult(ref actorHitResults, Caster, triggeringPath.m_triggeringPathSegment.square);
		List<GameObject> list = new List<GameObject>();
		if (m_fieldInfo.hitPulseSequencePrefab != null)
		{
			list.Add(m_fieldInfo.hitPulseSequencePrefab);
		}
		if (triggeringPath.m_mover.GetTeam() != Caster.GetTeam())
		{
			if (m_fieldInfo.enemyHitSequencePrefab != null)
			{
				list.Add(m_fieldInfo.enemyHitSequencePrefab);
			}
		}
		else if (m_fieldInfo.allyHitSequencePrefab != null)
		{
			list.Add(m_fieldInfo.allyHitSequencePrefab);
		}
		foreach (GroundAreaInfo groundAreaInfo in m_areaInfoList)
		{
			if (groundAreaInfo.IsActive() && groundAreaInfo.IsSquareInArea(triggeringPath.m_triggeringPathSegment.square))
			{
				groundAreaInfo.AddActorHit(triggeringPath.m_mover);
				if (m_fieldInfo.endIfHasDoneHits)
				{
					actorHitResults.AddEffectSequenceToEnd(m_fieldInfo.persistentSequencePrefab, m_guid, groundAreaInfo.GetShapeCenter());
					groundAreaInfo.AddSequencesToEndForHitResults(actorHitResults, m_guid);
				}
			}
		}
		
		ProcessActorHit(actorHitResults, true); // custom

		MovementResults movementResults = new MovementResults(movementStage);
		movementResults.SetupTriggerData(triggeringPath);
		movementResults.SetupGameplayData(this, actorHitResults);
		movementResults.SetupSequenceDataList(
			list,
			hitCenterSquare,
			SequenceSource,
			new SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam
			{
				ignoreStartEvent = true
			}.ToArray());
		movementResultsList.Add(movementResults);
	}

	public override void GatherMovementResultsFromSegment(ActorData mover, MovementInstance movementInstance, MovementStage movementStage, BoardSquarePathInfo sourcePath, BoardSquarePathInfo destPath, ref List<MovementResults> movementResultsList)
	{
		if (!m_fieldInfo.ignoreMovementHits
		    && ShouldHitThisTurn()
		    && !m_actorsHitThisTurn.Contains(mover)
		    && !IsSquareInAnyShape(sourcePath.square)
		    && IsSquareInAnyShape(destPath.square, out BoardSquare hitCenterSquare)
		    && m_fieldInfo.CanBeAffected(mover, Caster)
		    && (movementInstance.m_groundBased || destPath.IsPathEndpoint()))
		{
			ServerAbilityUtils.TriggeringPathInfo triggeringPath = new ServerAbilityUtils.TriggeringPathInfo(mover, destPath);
			SetUpMovementHit(triggeringPath, hitCenterSquare, movementStage, ref movementResultsList);
			m_actorsHitThisTurn.Add(mover);
		}
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam())
		{
			foreach (GroundAreaInfo areaInfo in m_areaInfoList)
			{
				areaInfo.AddToSquaresToAvoidForRespawn(squaresToAvoid);
			}
		}
	}

	protected HashSet<ActorData> GetActorsHitThisTurn(bool isReal) // private in rogues
	{
		return isReal ? m_actorsHitThisTurn : m_actorsHitThisTurn_fake;
	}

	protected void AddActorHitThisTurn(ActorData actor, bool isReal) // private in rogues
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

	public class GroundAreaInfo
	{
		public AbilityAreaShape m_shape;
		public BoardSquare m_targetSquare;
		public Vector3 m_shapeFreePos;
		private Vector3 m_shapeCenter;
		private HashSet<BoardSquare> m_affectedSquares;
		private List<ActorData> m_actorsHitInTurn;
		private bool m_shouldEnd;
		private List<ServerClientUtils.SequenceStartData> m_additionalPersistentSeqStartData;

		public GroundAreaInfo(BoardSquare targetSquare, Vector3 freePos, AbilityAreaShape shape)
		{
			m_targetSquare = targetSquare;
			m_shapeFreePos = freePos;
			m_shape = shape;
			m_shapeCenter = AreaEffectUtils.GetCenterOfShape(shape, m_shapeFreePos, m_targetSquare);
			m_shapeCenter.y = Board.Get().BaselineHeight;
			m_affectedSquares = new HashSet<BoardSquare>();
			m_actorsHitInTurn = new List<ActorData>();
			m_shouldEnd = false;
			m_additionalPersistentSeqStartData = new List<ServerClientUtils.SequenceStartData>();
		}

		public void InitAffectedSquares(List<BoardSquare> affectedSquares)
		{
			m_affectedSquares.Clear();
			foreach (BoardSquare square in affectedSquares)
			{
				m_affectedSquares.Add(square);
			}
		}

		public bool IsSquareInArea(BoardSquare testSquare)
		{
			return m_affectedSquares.Contains(testSquare);
		}

		public bool IsSquareInArea(BoardSquare testSquare, AbilityAreaShape shape, bool penetrateLos, ActorData caster)
		{
			return AreaEffectUtils.IsSquareInShape(testSquare, shape, m_shapeFreePos, m_targetSquare, penetrateLos, caster);
		}

		public void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid)
		{
			if (IsActive())
			{
				squaresToAvoid.UnionWith(m_affectedSquares);
			}
		}

		public Vector3 GetShapeCenter()
		{
			return m_shapeCenter;
		}

		public bool IsActive()
		{
			return !m_shouldEnd;
		}

		public void MarkAsInactive()
		{
			m_shouldEnd = true;
		}

		public void ClearActorsHit()
		{
			m_actorsHitInTurn.Clear();
		}

		public void AddActorHit(ActorData actor)
		{
			if (!m_actorsHitInTurn.Contains(actor))
			{
				m_actorsHitInTurn.Add(actor);
			}
		}

		public List<ActorData> GetActorsHit()
		{
			return m_actorsHitInTurn;
		}

		public void AddSequenceStartDataToPersist(ServerClientUtils.SequenceStartData seqStartData)
		{
			if (seqStartData != null)
			{
				m_additionalPersistentSeqStartData.Add(seqStartData);
			}
		}

		public List<ServerClientUtils.SequenceStartData> GetAdditionalPersistentSeqStartData()
		{
			return m_additionalPersistentSeqStartData;
		}

		public void AddSequencesToEndForHitResults(ActorHitResults hitResults, int effectGuid)
		{
			foreach (ServerClientUtils.SequenceStartData sequenceStartData in GetAdditionalPersistentSeqStartData())
			{
				if (sequenceStartData.GetServerOnlyPrefabReference() != null)
				{
					hitResults.AddEffectSequenceToEnd(sequenceStartData.GetServerOnlyPrefabReference(), effectGuid, sequenceStartData.GetTargetPos());
				}
			}
		}
	}
}
#endif
