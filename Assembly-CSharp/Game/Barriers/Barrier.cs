// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;
using UnityEngine.Networking;

public class Barrier
{
	private string m_name;

	private Vector3 m_center;
	private Vector3 m_endpoint1;
	private Vector3 m_endpoint2;
	private Vector3 m_facingDir;

	private bool m_bidirectional;
	private bool m_makeClientGeo;

	private GameObject m_generatedClientGeometry;

	private Team m_team;
	private ActorData m_owner;

#if SERVER
	// server-only
	private GameplayResponseForActor m_onEnemyMovedThrough;
	// server-only
	private GameplayResponseForActor m_onAllyMovedThrough;
	// server-only
	private Ability m_sourceAbility;
	// server-only
	private BarrierResponseOnShot m_responseOnShotBlock;
	// server-only
	private BarrierSet m_barrierSetHandler;
	
	
	// custom 
	public GameplayResponseForActor OnEnemyMovedThrough => m_onEnemyMovedThrough;
#endif

	public SpoilsSpawnData m_spoilsSpawnOnEnemyMovedThrough;
	public SpoilsSpawnData m_spoilsSpawnOnAllyMovedThrough;

	public bool m_removeAtTurnEndIfEnemyMovedThrough;
	public bool m_removeAtTurnEndIfAllyMovedThrough;
	public bool m_removeAtPhaseEndIfEnemyMovedThrough;
	public bool m_removeAtPhaseEndIfAllyMovedThrough;

	public AbilityPriority m_customEndPhase = AbilityPriority.INVALID;

	public bool m_removeAtPhaseEndIfCasterKnockedBack;

	// server-only
#if SERVER
	private bool m_casterWasKnockedBack;
#endif

	private int m_maxHits;

#if SERVER
	// custom
	public int MaxHits => m_maxHits;
	
	// server-only
	private List<Barrier> m_linkedBarriers;
	// server-only
	private LinkedBarrierData m_linkedData;
#endif

	public EffectDuration m_time;
	public int m_guid;
	public List<Sequence> m_barrierSequences;
	private List<GameObject> m_barrierSequencePrefabs;
	private bool m_playSequences;

#if SERVER
	// server-only
	private List<OnHitAuthoredData> m_onHitData = new List<OnHitAuthoredData>();
	// server-only
	private bool m_endOnCasterDeath;
	// server-only
	private bool m_scriptedToEnd;
#endif

	private bool m_considerAsCover;

#if SERVER
	// server-only
	protected List<MovementResults> m_evadeResults;
	// server-only
	protected List<MovementResults> m_knockbackResults;
	// server-only
	protected List<MovementResults> m_normalMovementResults; // TODO BARRIERS never populated
#endif

	public string Name
	{
		get
		{
			return m_name;
		}
		private set
		{
			m_name = value;
		}
	}

	// server-only
#if SERVER
	public bool makeClientGeo
	{
		set => m_makeClientGeo = value;
	}
#endif

	public ActorData Caster
	{
		get
		{
			return m_owner;
		}
		private set
		{
			m_owner = value;
		}
	}

	// server-only
#if SERVER
	public void SetEnemyMovedThroughResponse(GameplayResponseForActor enemyMovedThroughResponse)
	{
		m_onEnemyMovedThrough = enemyMovedThroughResponse;
	}
#endif

	// server-only
#if SERVER
	public void SetAllyMovedThroughResponse(GameplayResponseForActor allyMovedThroughResponse)
	{
		m_onAllyMovedThrough = allyMovedThroughResponse;
	}
#endif

	// server-only
#if SERVER
	public void SetSourceAbility(Ability source)
	{
		m_sourceAbility = source;
	}
#endif

	// server-only
#if SERVER
	public Ability GetSourceAbility()
	{
		return m_sourceAbility;
	}
#endif

	// server-only
#if SERVER
	public BarrierResponseOnShot GetResponseOnShotBlock()
	{
		return m_responseOnShotBlock;
	}
#endif

	// server-only
#if SERVER
	public void SetBarrierSetHandler(BarrierSet handler)
	{
		m_barrierSetHandler = handler;
	}
#endif

	// server-only
#if SERVER
	private bool CanStillHit()
	{
		return UnlimitedHits() || m_linkedData == null || m_linkedData.GetNumHits() < m_maxHits;
	}
#endif

	public SequenceSource BarrierSequenceSource { get; protected set; }

	// server-only
#if SERVER
	public bool scriptedToEnd
	{
		set => m_scriptedToEnd = value;
	}
#endif

	public BlockingRules BlocksVision { get; private set; }
	public BlockingRules BlocksAbilities { get; private set; }
	public BlockingRules BlocksMovement { get; private set; }
	public BlockingRules BlocksMovementOnCrossover { get; private set; }
	public BlockingRules BlocksPositionTargeting { get; private set; }

	public bool ConsiderAsCover
	{
		get
		{
			return m_considerAsCover;
		}
		set
		{
			m_considerAsCover = value;
		}
	}

	public Barrier(
		int guid,
		string name,
		Vector3 center,
		Vector3 facingDir,
		float width,
		bool bidirectional,
		BlockingRules blocksVision,
		BlockingRules blocksAbilities,
		BlockingRules blocksMovement,
		BlockingRules blocksPositionTargeting,
		bool considerAsCover,
		int maxDuration,
		ActorData owner,
		List<GameObject> barrierSequencePrefabs = null,
		bool playSequences = true,
		GameplayResponseForActor onEnemyMovedThrough = null,
		GameplayResponseForActor onAllyMovedThrough = null,
		int maxHits = -1,
		bool endOnCasterDeath = false,
		SequenceSource parentSequenceSource = null,
		Team barrierTeam = Team.Invalid)
	//, List<OnHitAuthoredData> onHitData = null in rogues
	{
		InitBarrier(guid, name, center, facingDir, width, bidirectional,
			blocksVision, blocksAbilities, blocksMovement, BlockingRules.ForNobody, blocksPositionTargeting,
			considerAsCover, maxDuration, owner, barrierSequencePrefabs, playSequences,
			onEnemyMovedThrough, onAllyMovedThrough, maxHits, endOnCasterDeath, parentSequenceSource, barrierTeam);
		//, onHitData);
	}

	public Vector3 GetCenterPos()
	{
		return m_center;
	}

	public Vector3 GetEndPos1()
	{
		return m_endpoint1;
	}

	public Vector3 GetEndPos2()
	{
		return m_endpoint2;
	}

	public Team GetBarrierTeam()
	{
		return m_team;
	}

	private bool UnlimitedHits()
	{
		return m_maxHits < 0;
	}

	private void InitBarrier(
		int guid,
		string name,
		Vector3 center,
		Vector3 facingDir,
		float width,
		bool bidirectional,
		BlockingRules blocksVision,
		BlockingRules blocksAbilities,
		BlockingRules blocksMovement,
		BlockingRules blocksMovementOnCrossover,
		BlockingRules blocksPositionTargeting,
		bool considerAsCover,
		int maxDuration,
		ActorData owner,
		List<GameObject> barrierSequencePrefabs,
		bool playSequences,
		GameplayResponseForActor onEnemyMovedThrough,
		GameplayResponseForActor onAllyMovedThrough,
		int maxHits,
		bool endOnCasterDeath,
		SequenceSource parentSequenceSource,
		Team barrierTeam)
	// ,List<OnHitAuthoredData> onHitData) in rogues
	{
		m_guid = guid;
		m_name = name;
		m_center = center;
		m_facingDir = facingDir;
		m_bidirectional = bidirectional;

		// rogues
		//m_onHitData = onHitData != null ? onHitData : new List<OnHitAuthoredData>(); // rogues

		Vector3 a = Vector3.Cross(facingDir, Vector3.up);
		a.Normalize();
		float d = width * Board.Get().squareSize;
		m_endpoint1 = center + a * d / 2f;
		m_endpoint2 = center - a * d / 2f;
		BlocksVision = blocksVision;
		BlocksAbilities = blocksAbilities;
		BlocksMovement = blocksMovement;
		BlocksMovementOnCrossover = blocksMovementOnCrossover;
		BlocksPositionTargeting = blocksPositionTargeting;
		m_considerAsCover = considerAsCover;
		m_owner = owner;
		m_team = m_owner?.GetTeam() ?? barrierTeam;
		m_time = new EffectDuration();
		m_time.duration = maxDuration;

		//added in rogues
#if SERVER
		m_linkedBarriers = new List<Barrier>();
#endif

		m_barrierSequencePrefabs = barrierSequencePrefabs;
		m_playSequences = playSequences && m_barrierSequencePrefabs != null;
		m_barrierSequences = new List<Sequence>();
		if (m_playSequences)
		{
			BarrierSequenceSource = new SequenceSource(null, null, false, parentSequenceSource);
		}
		m_maxHits = maxHits;

		// server-only
#if SERVER
		m_endOnCasterDeath = endOnCasterDeath;
		m_onEnemyMovedThrough = onEnemyMovedThrough;
		m_onAllyMovedThrough = onAllyMovedThrough;
		m_evadeResults = new List<MovementResults>();
		m_knockbackResults = new List<MovementResults>();
		m_normalMovementResults = new List<MovementResults>();
#endif
	}

	// server-only
#if SERVER
	public Barrier(
		string name,
		Vector3 center,
		Vector3 facingDir,
		ActorData owner,
		StandardBarrierData data,
		bool playSequences = true,
		SequenceSource parentSequenceSource = null,
		Team barrierTeam = Team.Invalid,
		List<OnHitAuthoredData> onHitData = null)
	{
		int guid = BarrierManager.s_nextBarrierGuid++;
		InitBarrier(
			guid,
			name,
			center,
			facingDir,
			data.m_width,
			data.m_bidirectional,
			data.m_blocksVision,
			data.m_blocksAbilities,
			data.m_blocksMovement,
			data.m_blocksMovementOnCrossover,
			data.m_blocksPositionTargeting,
			data.m_considerAsCover,
			data.m_maxDuration,
			owner,
			data.m_barrierSequencePrefabs,
			playSequences,
			data.m_onEnemyMovedThrough,
			data.m_onAllyMovedThrough,
			data.m_maxHits,
			data.m_endOnCasterDeath,
			parentSequenceSource,
			barrierTeam); // , data.onHitData);
		m_removeAtTurnEndIfAllyMovedThrough = data.m_removeAtTurnEndIfAllyMovedThrough;
		m_removeAtTurnEndIfEnemyMovedThrough = data.m_removeAtTurnEndIfEnemyMovedThrough;
		m_removeAtPhaseEndIfAllyMovedThrough = data.m_removeAtPhaseEndIfAllyMovedThrough;
		m_removeAtPhaseEndIfEnemyMovedThrough = data.m_removeAtPhaseEndIfEnemyMovedThrough;
		m_responseOnShotBlock = data.m_responseOnShotBlock;
	}
#endif

	// server-only
#if SERVER
	public Barrier(
		string name,
		Vector3 center,
		Vector3 facingDir,
		float width,
		bool bidirectional,
		BlockingRules blocksVision,
		BlockingRules blocksAbilities,
		BlockingRules blocksMovement,
		BlockingRules blocksPositionTargeting,
		int maxDuration,
		ActorData owner,
		List<GameObject> barrierSequencePrefabs = null,
		bool playSequences = true,
		GameplayResponseForActor onEnemyMovedThrough = null,
		GameplayResponseForActor onAllyMovedThrough = null,
		int maxHits = -1,
		bool endOnCasterDeath = false,
		SequenceSource parentSequenceSource = null,
		Team barrierTeam = Team.Invalid)
	// List<OnHitAuthoredData> onHitData = null)
	{
		int guid = BarrierManager.s_nextBarrierGuid++;
		InitBarrier(
			guid,
			name,
			center,
			facingDir,
			width,
			bidirectional,
			blocksVision,
			blocksAbilities,
			blocksMovement,
			BlockingRules.ForNobody,
			blocksPositionTargeting,
			false,
			maxDuration,
			owner,
			barrierSequencePrefabs,
			playSequences,
			onEnemyMovedThrough,
			onAllyMovedThrough,
			maxHits,
			endOnCasterDeath,
			parentSequenceSource,
			barrierTeam); // , onHitData);
	}
#endif

	public bool CanBeSeenThroughBy(ActorData viewer)
	{
		return !IsBlocked(viewer, BlocksVision);
	}

	public bool CanBeShotThroughBy(ActorData shooter)
	{
		return BarrierManager.Get().SuppressingAbilityBlocks() || !IsBlocked(shooter, BlocksAbilities);
	}

	public bool CanBeMovedThroughBy(ActorData mover)
	{
		return !IsBlocked(mover, BlocksMovement);
	}

	public bool CanMoveThroughAfterCrossoverBy(ActorData mover)
	{
		return !IsBlocked(mover, BlocksMovementOnCrossover);
	}

	public bool IsPositionTargetingBlockedFor(ActorData caster)
	{
		return IsBlocked(caster, BlocksPositionTargeting);
	}

	private bool IsBlocked(ActorData actor, BlockingRules rules)
	{
		switch (rules)
		{
			case BlockingRules.ForEverybody:
				return true;
			case BlockingRules.ForNobody:
				return false;
			case BlockingRules.ForEnemies:
				return actor == null || actor.GetTeam() != m_team;
			//case BlockingRules.ForAllies: // added in rogues
			//	return actor == null || actor.GetTeam() == m_team;
			default:
				return false;
		}
	}

	// server-only
#if SERVER
	public virtual void OnTurnStart()
	{
		// custom
		m_linkedData?.OnTurnStart();
	}
#endif

	// server-only
#if SERVER
	public virtual void OnTurnEnd()
	{
		m_linkedData?.OnTurnEnd();
	}
#endif

	public virtual void OnStart(bool delayVisionUpdate, out List<ActorData> visionUpdaters)
	{
		visionUpdaters = new List<ActorData>();
#if SERVER
		if (NetworkServer.active)  // server-only
		{
			UpdateActorsForVision(delayVisionUpdate, out visionUpdaters);
			UpdateActorsForMovement();
		}
#endif
		if (NetworkClient.active && m_makeClientGeo)
		{
			float squareSize = Board.Get().squareSize;
			Vector3 a = m_endpoint2 - m_endpoint1;
			bool flag = Mathf.Abs(a.z) > Mathf.Abs(a.x);
			Vector3 vector = m_endpoint1 + 0.5f * a;
			m_generatedClientGeometry = GameObject.CreatePrimitive(PrimitiveType.Cube);
			m_generatedClientGeometry.transform.position = new Vector3(vector.x, 1.5f * squareSize, vector.z);
			if (flag)
			{
				m_generatedClientGeometry.transform.localScale = new Vector3(0.25f, 2f * squareSize, a.magnitude);
			}
			else
			{
				m_generatedClientGeometry.transform.localScale = new Vector3(a.magnitude, 2f * squareSize, 0.25f);
			}
		}
	}

	public virtual void OnEnd()
	{
		if (NetworkServer.active)
		{
			foreach (Sequence current in m_barrierSequences)
			{
				if (current != null)
				{
					current.MarkForRemoval();
				}
			}

			// server-only
#if SERVER
			UnlinkFromBarriers();
			UpdateActorsForVision(false, out _);
			UpdateActorsForMovement();
			m_barrierSetHandler?.OnBarrierEnd(this);
			ServerEffectManager.GetSharedEffectBarrierManager().NotifyBarrierEnded(m_guid);
#endif
		}
		if (NetworkClient.active && m_makeClientGeo)
		{
			if (m_generatedClientGeometry != null)
			{
				UnityEngine.Object.DestroyObject(m_generatedClientGeometry);
			}
			m_generatedClientGeometry = null;
		}
	}

	public bool CanAffectVision()
	{
		return BlocksVision == BlockingRules.ForEnemies || BlocksVision == BlockingRules.ForEverybody;
		// rogues
		//return BlocksVision > BlockingRules.ForNobody;
	}

	public bool CanAffectMovement()
	{
		return BlocksMovement == BlockingRules.ForEnemies || BlocksMovement == BlockingRules.ForEverybody;
		// rogues
		//return BlocksMovement > BlockingRules.ForNobody;
	}

#if SERVER
	// server-only
	public virtual void UpdateActorsForVision(bool delayVisionUpdate, out List<ActorData> visionUpdaters)
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		visionUpdaters = new List<ActorData>();
		if (BlocksVision == BlockingRules.ForEnemies)
		{
			List<Team> otherTeamsThan = GameplayUtils.GetOtherTeamsThan(m_team);
			visionUpdaters = new List<ActorData>();
			foreach (ActorData actorData in actors)
			{
				if (otherTeamsThan.Contains(actorData.GetTeam()))
				{
					visionUpdaters.Add(actorData);
				}
			}
			foreach (Team team in otherTeamsThan)
			{
				BarrierManager.Get().UpdateVisionStateForTeam(team);
			}
		}
		else
		{
			// rogues
			//if (BlocksVision == BlockingRules.ForAllies)
			//{
			//	visionUpdaters = new List<ActorData>();
			//	foreach (ActorData actorData2 in actors)
			//	{
			//		if (actorData2.GetTeam() == m_team)
			//		{
			//			visionUpdaters.Add(actorData2);
			//		}
			//	}
			//	BarrierManager.Get().UpdateVisionStateForTeam(m_team);
			//}
			//else
			if (BlocksVision == BlockingRules.ForEverybody)
			{
				visionUpdaters = actors;
				BarrierManager.Get().UpdateVisionStateForTeam(Team.TeamA);
				BarrierManager.Get().UpdateVisionStateForTeam(Team.TeamB);
				BarrierManager.Get().UpdateVisionStateForTeam(Team.Objects);
			}
		}
		if (!delayVisionUpdate && visionUpdaters != null && visionUpdaters.Count > 0)
		{
			foreach (ActorData actorData3 in visionUpdaters)
			{
				actorData3.GetFogOfWar().ImmediateUpdateVisibilityOfSquares();
			}
			foreach (ActorData actorData in GameFlowData.Get().GetActors())
			{
				if (actorData.IsActorVisibleToAnyEnemy())
				{
					Log.Info($"Requesting SynchronizeTeamSensitiveData for {actorData.DisplayName} for being visible to an enemy after a barrier update"); // custom
					actorData.SynchronizeTeamSensitiveData();
				}
			}
		}
	}

	// server-only
	public virtual void UpdateActorsForMovement()
	{
		List<ActorData> actors = GameFlowData.Get().GetActors();
		List<ActorData> list;
		if (BlocksMovement == BlockingRules.ForEnemies)
		{
			list = new List<ActorData>();
			foreach (ActorData actorData in actors)
			{
				if (actorData.GetTeam() != m_team)
				{
					list.Add(actorData);
				}
			}
			foreach (Team team in GameplayUtils.GetOtherTeamsThan(m_team))
			{
				BarrierManager.Get().UpdateMovementStateForTeam(team);
			}
		}
		else
		{
			// rogues
			//if (BlocksMovement == BlockingRules.ForAllies)
			//{
			//	list = new List<ActorData>();
			//	foreach (ActorData actorData2 in actors)
			//	{
			//		if (actorData2.GetTeam() == m_team)
			//		{
			//			list.Add(actorData2);
			//		}
			//	}
			//	BarrierManager.Get().UpdateMovementStateForTeam(m_team);
			//}
			//else
			if (BlocksMovement == BlockingRules.ForEverybody)
			{
				list = actors;
				BarrierManager.Get().UpdateMovementStateForTeam(Team.TeamA);
				BarrierManager.Get().UpdateMovementStateForTeam(Team.TeamB);
				BarrierManager.Get().UpdateMovementStateForTeam(Team.Objects);
			}
			else
			{
				list = null;
			}
		}
		if (list != null)
		{
			foreach (ActorData actorData3 in list)
			{
				actorData3.GetActorMovement().UpdateSquaresCanMoveTo();
			}
		}
	}

	// server-only
	public virtual void LinkWithBarrier(Barrier otherBarrier, LinkedBarrierData linkedData)
	{
		if (otherBarrier != this && !m_linkedBarriers.Contains(otherBarrier))
		{
			m_linkedBarriers.Add(otherBarrier);
			m_linkedData = linkedData;
			otherBarrier.m_linkedBarriers.Add(this);
			otherBarrier.m_linkedData = linkedData;
		}
	}

	// server-only
	public virtual void UnlinkFromBarriers()
	{
		foreach (Barrier barrier in m_linkedBarriers)
		{
			barrier.m_linkedBarriers.Remove(this);
		}
		m_linkedBarriers.Clear();
		m_linkedData = null;
	}

	// server-only
	public List<Barrier> GetLinkedBarriers()
	{
		if (m_linkedBarriers == null)
		{
			return null;
		}
		return new List<Barrier>(m_linkedBarriers);
	}

	// server-only
	public virtual void OnShootThrough()
	{
	}

	// server-only
	public bool HasResponseToAllyMoveThrough()
	{
		bool hasResponse = m_onAllyMovedThrough != null && m_onAllyMovedThrough.HasResponse();
		bool hasSpoils = m_spoilsSpawnOnAllyMovedThrough != null && m_spoilsSpawnOnAllyMovedThrough.m_numToSpawn > 0;
		bool result = hasResponse || hasSpoils;
		if (hasSpoils && !hasResponse)
		{
			Debug.LogError("Barrier " + Name + " has no allied gameplay on-move-through response, " +
			               "but does have a spoils response.  It needs a gameplay response, at least for sequences.");
		}
		return result;
	}

	// server-only
	public bool HasResponseToEnemyMoveThrough()
	{
		bool hasResponse = m_onEnemyMovedThrough != null && m_onEnemyMovedThrough.HasResponse();
		bool flag2 = m_spoilsSpawnOnEnemyMovedThrough != null && m_spoilsSpawnOnEnemyMovedThrough.m_numToSpawn > 0;
		bool hasSpoils = hasResponse || flag2;
		if (flag2 && !hasResponse)
		{
			Debug.LogError("Barrier " + Name + " has no enemy gameplay on-move-through response, " +
			               "but does have a spoils response.  It needs a gameplay response, at least for sequences.");
		}
		return hasSpoils;
	}

	// server-only
	public bool ShouldEnd()
	{
		return m_time.ReadyToEnd() || ShouldEndEarly() || ShouldEndFromMovedThrough() || ShouldEndFromMaxHits();
	}

	// server-only
	protected virtual bool ShouldEndEarly()
	{
		if (m_endOnCasterDeath)
		{
			return Caster == null || Caster.IsDead();
		}
		return m_scriptedToEnd;
	}

	// server-only
	protected virtual bool ShouldEndFromMovedThrough()
	{
		return m_linkedData != null
		       && ((m_removeAtTurnEndIfAllyMovedThrough && HasAllyMovedThrough())
		           || (m_removeAtTurnEndIfEnemyMovedThrough && HasEnemyMovedThrough()));
	}

	// server-only
	protected bool HasAllyMovedThrough()
	{
		if (m_linkedData != null)
		{
			foreach (ActorData actor in m_linkedData.m_actorsMovedThroughThisTurn)
			{
				if (actor.GetTeam() == m_owner.GetTeam())
				{
					return true;
				}
			}
		}
		return false;
	}

	// server-only
	protected bool HasEnemyMovedThrough()
	{
		if (m_linkedData != null)
		{
			foreach (ActorData actor in m_linkedData.m_actorsMovedThroughThisTurn)
			{
				if (actor.GetTeam() != m_owner.GetTeam())
				{
					return true;
				}
			}
		}
		return false;
	}

	// server-only
	internal bool ActorMovedThroughThisTurn(ActorData actor)
	{
		return m_linkedData != null
		       && actor != null
		       && m_linkedData.m_actorsMovedThroughThisTurn.Contains(actor);
	}

	// server-only
	public virtual bool ShouldEndAtEndOfPhase(AbilityPriority phase)
	{
		return ShouldEndAtEndOfPhaseFromMovedThrough(phase)
		       || (phase == AbilityPriority.Combat_Knockback && ShouldEndAtEndOfPhaseFromKnockback())
		       || (m_customEndPhase != AbilityPriority.INVALID
		           && m_customEndPhase == phase
		           && m_time.duration > 0
		           && m_time.age >= m_time.duration - 1);
	}

	// server-only
	public virtual bool ShouldEndAtEndOfPhaseFromMovedThrough(AbilityPriority phase)
	{
		return (m_removeAtPhaseEndIfAllyMovedThrough && HasAllyMovedThrough())
		       || (m_removeAtPhaseEndIfEnemyMovedThrough && HasEnemyMovedThrough());
	}

	// server-only
	public virtual bool ShouldEndAtEndOfPhaseFromKnockback()
	{
		return m_removeAtPhaseEndIfCasterKnockedBack && m_casterWasKnockedBack;
	}

	// server-only
	protected virtual bool ShouldEndFromMaxHits()
	{
		return !CanStillHit();
	}
#endif

	public bool CrossingBarrier(Vector3 src, Vector3 dest)
	{
		bool srcInside = VectorUtils.IsPointInLaser(src, m_endpoint1, m_endpoint2, 0.001f);
		bool destInside = VectorUtils.IsPointInLaser(dest, m_endpoint1, m_endpoint2, 0.001f);
		if (srcInside)
		{
			return false;
		}
		if (!destInside && VectorUtils.OnSameSideOfLine(src, dest, m_endpoint1, m_endpoint2))
		{
			return false;
		}
		if (!destInside && VectorUtils.OnSameSideOfLine(m_endpoint1, m_endpoint2, src, dest))
		{
			return false;
		}
		if (m_bidirectional)
		{
			return true;
		}
		Vector3 lhs = src - m_center;
		float num = Vector3.Dot(lhs, m_facingDir);
		return num > 0f;
	}

	public bool CrossingBarrierForVision(Vector3 src, Vector3 dest)
	{
		return SegmentsIntersectForVision(src, dest, m_endpoint1, m_endpoint2);
	}

	private bool SegmentsIntersectForVision(Vector3 startA, Vector3 endA, Vector3 startB, Vector3 endB)
	{
		return PointsAreCounterClockwise(startA, startB, endB) != PointsAreCounterClockwise(endA, startB, endB) && PointsAreCounterClockwise(startA, endA, startB) != PointsAreCounterClockwise(startA, endA, endB);
	}

	private static bool PointsAreCounterClockwise(Vector3 a, Vector3 b, Vector3 c)
	{
		return (c.z - a.z) * (b.x - a.x) > (b.z - a.z) * (c.x - a.x);
	}

	public Vector3 GetIntersectionPoint(Vector3 src, Vector3 dest)
	{
		Vector3 vector = dest - src;
		Vector3 directionOfSecond = m_endpoint2 - m_endpoint1;
		Vector3 lineLineIntersection = VectorUtils.GetLineLineIntersection(src, vector, m_endpoint1, directionOfSecond, out bool intersecting);
		if (intersecting)
		{
			lineLineIntersection.y = src.y;
			Vector3 normalized = (-vector).normalized;
			lineLineIntersection += normalized * 0.05f;
		}
		return lineLineIntersection;
	}

	public Vector3 GetCollisionNormal(Vector3 incomingDir)
	{
		if (m_bidirectional && Vector3.Dot(incomingDir, m_facingDir) > 0f)
		{
			return -m_facingDir;
		}
		return m_facingDir;
	}

	public Vector3 GetFacingDir()
	{
		return m_facingDir;
	}

#if SERVER
	// server-only
	public void GatherResultsInResponseToEvades(MovementCollection collection)
	{
		m_evadeResults.Clear();
		GatherMovementResults(collection, ref m_evadeResults);
		foreach (MovementResults movementResults in m_evadeResults)
		{
			movementResults.m_triggeringPath.m_moverHasGameplayHitHere = true;
			movementResults.m_triggeringPath.m_updateLastKnownPos = 
				movementResults.ShouldMovementHitUpdateTargetLastKnownPos(movementResults.m_triggeringMover);
			Log.Info($"UpdateLastKnownPos {movementResults.m_triggeringMover?.DisplayName} " +
			         $"{movementResults.m_triggeringPath.square?.GetGridPos()} " +
			         $"{(movementResults.m_triggeringPath.m_updateLastKnownPos ? "" : "NOT ")} updating for evade movement hit"); // custom debug
		}
	}

	// server-only
	public void GatherResultsInResponseToKnockbacks(MovementCollection collection)
	{
		m_knockbackResults.Clear();
		GatherMovementResults(collection, ref m_knockbackResults);
		for (int i = 0; i < m_knockbackResults.Count; i++)
		{
			m_knockbackResults[i].m_triggeringPath.m_moverHasGameplayHitHere = true;
			m_knockbackResults[i].m_triggeringPath.m_updateLastKnownPos = 
				m_knockbackResults[i].ShouldMovementHitUpdateTargetLastKnownPos(m_knockbackResults[i].m_triggeringMover);
			TheatricsManager.Get().OnKnockbackMovementHitGathered(m_knockbackResults[i].GetTriggeringActor());
			Log.Info($"UpdateLastKnownPos {m_knockbackResults[i].m_triggeringMover?.DisplayName} " +
			         $"{m_knockbackResults[i].m_triggeringPath.square?.GetGridPos()} " +
			         $"{(m_knockbackResults[i].m_triggeringPath.m_updateLastKnownPos ? "" : "NOT ")} updating for knockback movement hit"); // custom debug
		}
	}

	// server-only
	public void GatherBarrierResultsInResponseToMovementSegment(
		ServerGameplayUtils.MovementGameplayData gameplayData,
		MovementStage movementStage,
		ref List<MovementResults> moveResultsForSegment)
	{
		ActorData actor = gameplayData.Actor;
		bool hasResponseToAlly = HasResponseToAllyMoveThrough();
		bool hasResponseToEnemy = HasResponseToEnemyMoveThrough();
		if (m_linkedData == null)
		{
			if (m_linkedBarriers.Count > 0)
			{
				Log.Error($"Barrier {Name} of {m_owner.DisplayName} has more-than-zero linked barriers but its linked data is null.");
			}
			m_linkedData = new LinkedBarrierData();
		}

		if ((!hasResponseToAlly && actor.GetTeam() == m_team)
		    || (!hasResponseToEnemy && actor.GetTeam() != m_team)
		    || !CanStillHit()
		    || m_linkedData.m_actorsMovedThroughThisTurn.Contains(actor)
		    || (m_sourceAbility != null && !m_sourceAbility.ShouldBarrierHitThisMover(actor))
		    || actor.IgnoreForAbilityHits)
		{
			return;
		}
		
		BoardSquare prevSquare = gameplayData.m_currentlyConsideredPath.prev.square;
		BoardSquare curSquare = gameplayData.m_currentlyConsideredPath.square;
		if (CrossingBarrier(curSquare.ToVector3(), prevSquare.ToVector3()))
		{
			Vector3 hitOrigin = (prevSquare.ToVector3() + curSquare.ToVector3()) / 2f;
			MovementResults item = CreateMoveResultsForMover(
				actor,
				gameplayData.m_currentlyConsideredPath,
				hitOrigin,
				movementStage);
			moveResultsForSegment.Add(item);
			m_casterWasKnockedBack = false;
		}
	}

	// server-only
	public void ClearNormalMovementResults()
	{
		m_normalMovementResults.Clear();
	}

	// server-only
	public void IntegrateDamageResultsForEvasion(ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		IntegrateDamageResultsForMovement(m_evadeResults, ref actorToDeltaHP);
	}

	// server-only
	public void IntegrateDamageResultsForKnockback(ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		IntegrateDamageResultsForMovement(m_knockbackResults, ref actorToDeltaHP);
	}

	// server-only
	private void IntegrateDamageResultsForMovement(List<MovementResults> results, ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		for (int i = 0; i < results.Count; i++)
		{
			ServerGameplayUtils.IntegrateHpDeltas(results[i].GetMovementDamageResults(), ref actorToDeltaHP);
		}
	}

	// server-only
	public void GatherGrossDamageResults_Barrier_Evasion(
		ref Dictionary<ActorData, int> actorToGrossDamage_real,
		ref Dictionary<ActorData, ServerGameplayUtils.DamageDodgedStats> stats)
	{
		Dictionary<ActorData, int> fakeDamageTaken = new Dictionary<ActorData, int>();
		foreach (MovementResults movementResults in GetMovementResultsForMovementStage(MovementStage.Evasion))
		{
			Dictionary<ActorData, int> movementDamageResults_Gross = movementResults.GetMovementDamageResults_Gross();
			ServerGameplayUtils.CalcDamageDodgedAndIntercepted(movementDamageResults_Gross, fakeDamageTaken, ref stats);
			ServerGameplayUtils.IntegrateHpDeltas(movementDamageResults_Gross, ref actorToGrossDamage_real);
		}
	}

	// server-only
	public List<SortablePathEntry> FindCrossingPathsFromCollection(MovementCollection movement)
	{
		List<SortablePathEntry> list = new List<SortablePathEntry>();
		foreach (MovementInstance movementInstance in movement.m_movementInstances)
		{
			if (CanReactToMovementInstance(movementInstance))
			{
				BoardSquarePathInfo step = movementInstance.m_path;
				while (step.next != null)
				{
					step = step.next;
					BoardSquare curSquare = step.square;
					BoardSquare prevSquare = step.prev.square;
					if (CrossingBarrier(curSquare.ToVector3(), prevSquare.ToVector3()))
					{
						list.Add(new SortablePathEntry(movementInstance, step, step.moveCost));
					}
				}
			}
		}
		return list;
	}

	// server-only
	public void RecordHitOnMover(ActorData mover)
	{
		if (mover.GetTeam() != m_team)
		{
			m_linkedData.m_hitsOnEnemies++;
		}
		else
		{
			m_linkedData.m_hitsOnAllies++;
		}
		if (!m_linkedData.m_actorsMovedThroughThisTurn.Contains(mover))
		{
			m_linkedData.m_actorsMovedThroughThisTurn.Add(mover);
		}
		if (!m_linkedData.m_actorsMovedThrough.Contains(mover))
		{
			m_linkedData.m_actorsMovedThrough.Add(mover);
		}
	}

	// server-only
	public virtual void GatherMovementResults(MovementCollection movement, ref List<MovementResults> movementResultsList)
	{
		if (!HasResponseToAllyMoveThrough() && !HasResponseToEnemyMoveThrough())
		{
			return;
		}
		if (m_linkedData == null)
		{
			if (m_linkedBarriers.Count > 0)
			{
				Log.Error($"Barrier {Name} of {m_owner.DisplayName} has more-than-zero linked barriers but its linked data is null.");
			}
			m_linkedData = new LinkedBarrierData();
		}
		List<SortablePathEntry> list = FindCrossingPathsFromCollection(movement);
		if (!UnlimitedHits())
		{
			list.Sort();
		}
		
		foreach (SortablePathEntry pathEntry in list)
		{
			if (!CanStillHit())
			{
				break;
			}
			ActorData mover = pathEntry.m_movementInstance.m_mover;
			if (!m_linkedData.m_actorsMovedThroughThisTurn.Contains(mover)
			    && (m_sourceAbility == null || m_sourceAbility.ShouldBarrierHitThisMover(mover))
			    && !mover.IgnoreForAbilityHits)
			{
				Vector3 hitOrigin = (pathEntry.m_triggeringPathSegment.prev.square.ToVector3() + pathEntry.m_triggeringPathSegment.square.ToVector3()) / 2f;
				movementResultsList.Add(
					CreateMoveResultsForMover(mover, pathEntry.m_triggeringPathSegment, hitOrigin, movement.m_movementStage));
			}
		}
		m_casterWasKnockedBack = false;
		if (movement.m_movementStage == MovementStage.Knockback)
		{
			foreach (MovementInstance m in movement.m_movementInstances)
			{
				if (m.m_mover == Caster)
				{
					m_casterWasKnockedBack = true;
					break;
				}
			}
		}
	}

	// NOTE ROGUES
	// server-only
	private void SetupActorHitResults(ref ActorHitResults hitRes, ActorData caster, int numHits = 1)
	{
		ActorData target = hitRes.m_hitParameters.Target;
		if (numHits <= 0)
		{
			return;
		}
		bool isAlly = target.GetTeam() == caster.GetTeam();
		ActorHitContext actorContext = new ActorHitContext();
		ContextVars abilityContext = new ContextVars();
		NumericHitResultScratch numericHitResultScratch = new NumericHitResultScratch();
		foreach (OnHitAuthoredData onHitAuthoredData in m_onHitData)
		{
			if (isAlly)
			{
				GenericAbility_Container.CalcIntFieldValues(target, caster, actorContext, abilityContext, onHitAuthoredData.m_allyHitIntFields, numericHitResultScratch);
				GenericAbility_Container.SetNumericFieldsOnHitResults(hitRes, numericHitResultScratch);
				//GenericAbility_Container.SetKnockbackFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_allyHitKnockbackFields);
				//GenericAbility_Container.SetCooldownReductionFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_allyHitCooldownReductionFields, numHits);
				GenericAbility_Container.SetEffectFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_allyHitEffectFields);
				// rogues?
				//GenericAbility_Container.SetEffectTemplateFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_allyHitEffectTemplateFields);
			}
			else
			{
				GenericAbility_Container.CalcIntFieldValues(target, caster, actorContext, abilityContext, onHitAuthoredData.m_enemyHitIntFields, numericHitResultScratch);
				GenericAbility_Container.SetNumericFieldsOnHitResults(hitRes, numericHitResultScratch);
				//GenericAbility_Container.SetKnockbackFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_enemyHitKnockbackFields);
				//GenericAbility_Container.SetCooldownReductionFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_enemyHitCooldownReductionFields, numHits);
				GenericAbility_Container.SetEffectFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_enemyHitEffectFields);
				// rogues?
				//GenericAbility_Container.SetEffectTemplateFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_enemyHitEffectTemplateFields);
			}
		}
	}

	// server-only
	private MovementResults CreateMoveResultsForMover(
		ActorData mover,
		BoardSquarePathInfo triggeringPath,
		Vector3 hitOrigin,
		MovementStage movementStage)
	{
		BoardSquare square = triggeringPath.square;
		GameplayResponseForActor matchingMoveThroughResponseFor = GetMatchingMoveThroughResponseFor(mover);
		ActorHitResults actorHitResults;
		if (m_barrierSetHandler == null || m_barrierSetHandler.ShouldAddGameplayHit(this, mover))
		{
			actorHitResults = matchingMoveThroughResponseFor.ConvertToActorHitResults(mover, hitOrigin, this, m_sourceAbility);
			SetupActorHitResults(ref actorHitResults, Caster, 1);
			SpoilsSpawnData matchingMoveThroughSpoilsFor = GetMatchingMoveThroughSpoilsFor(mover);
			if (matchingMoveThroughSpoilsFor != null && matchingMoveThroughSpoilsFor.HasResponse())
			{
				Team forTeam = Caster != null ? Caster.GetTeam() : m_team;
				matchingMoveThroughSpoilsFor.AddSpoilsToHitResults(square, forTeam, ref actorHitResults);
			}
		}
		else
		{
			actorHitResults = new ActorHitResults(new ActorHitParameters(mover, hitOrigin));
			SetupActorHitResults(ref actorHitResults, Caster);
		}
		MovementResults movementResults = new MovementResults(movementStage);
		movementResults.SetupTriggerData(mover, triggeringPath);
		movementResults.SetupGameplayData(this, actorHitResults);
		movementResults.SetupSequenceData(matchingMoveThroughResponseFor.m_sequenceToPlay, square, BarrierSequenceSource);
		RecordHitOnMover(mover);
		if (!CanStillHit())
		{
			actorHitResults.AddBarrierForRemoval(this, true);
		}
		return movementResults;
	}

	// server-only
	private bool CanReactToMovementInstance(MovementInstance movementInstance)
	{
		return movementInstance.m_groundBased
		       && movementInstance.m_canCrossBarriers
		       && (movementInstance.m_mover.GetTeam() != m_team || HasResponseToAllyMoveThrough())
		       && (movementInstance.m_mover.GetTeam() == m_team || HasResponseToEnemyMoveThrough())
		       && movementInstance.m_path != null
		       && movementInstance.m_path.next != null
		       && movementInstance.m_path.square != null
		       && movementInstance.m_path.next.square != null
		       && !m_linkedData.m_actorsMovedThroughThisTurn.Contains(movementInstance.m_mover)
		       && (m_sourceAbility == null || m_sourceAbility.ShouldBarrierHitThisMover(movementInstance.m_mover))
		       && !movementInstance.m_mover.IgnoreForAbilityHits;
	}

	// server-only
	private GameplayResponseForActor GetMatchingMoveThroughResponseFor(ActorData actor)
	{
		return actor.GetTeam() == m_team
			? m_onAllyMovedThrough
			: m_onEnemyMovedThrough;
	}

	// server-only
	private SpoilsSpawnData GetMatchingMoveThroughSpoilsFor(ActorData actor)
	{
		return actor.GetTeam() == m_team
			? m_spoilsSpawnOnAllyMovedThrough
			: m_spoilsSpawnOnEnemyMovedThrough;
	}

	// server-only
	public void ExecuteUnexecutedMovementResults_Barrier(MovementStage movementStage, bool failsafe)
	{
		switch (movementStage)
		{
			case MovementStage.Evasion:
				MovementResults.ExecuteUnexecutedHits(m_evadeResults, failsafe);
				return;
			case MovementStage.Knockback:
				MovementResults.ExecuteUnexecutedHits(m_knockbackResults, failsafe);
				return;
			case MovementStage.Normal:
				MovementResults.ExecuteUnexecutedHits(m_normalMovementResults, failsafe);
				break;
		}
	}

	// server-only
	public void ExecuteUnexecutedMovementResultsForDistance_Barrier(
		float distance,
		MovementStage movementStage,
		bool failsafe,
		out bool stillHasUnexecutedHits,
		out float nextUnexecutedHitDistance)
	{
		stillHasUnexecutedHits = false;
		nextUnexecutedHitDistance = -1f;
		switch (movementStage)
		{
			case MovementStage.Evasion:
				MovementResults.ExecuteUnexecutedHitsForDistance(
					m_evadeResults, distance, failsafe, out stillHasUnexecutedHits, out nextUnexecutedHitDistance);
				return;
			case MovementStage.Knockback:
				MovementResults.ExecuteUnexecutedHitsForDistance(
					m_knockbackResults, distance, failsafe, out stillHasUnexecutedHits, out nextUnexecutedHitDistance);
				return;
			case MovementStage.Normal:
				MovementResults.ExecuteUnexecutedHitsForDistance(
					m_normalMovementResults, distance, failsafe, out stillHasUnexecutedHits, out nextUnexecutedHitDistance);
				break;
		}
	}

	// server-only
	public List<MovementResults> GetMovementResultsForMovementStage(MovementStage movementStage)
	{
		switch (movementStage)
		{
			case MovementStage.Evasion:
				return m_evadeResults;
			case MovementStage.Knockback:
				return m_knockbackResults;
			case MovementStage.Normal:
				return m_normalMovementResults;
			default:
				return null;
		}
	}
#endif

	internal static BarrierSerializeInfo BarrierToSerializeInfo(Barrier barrier)
	{
		BarrierSerializeInfo barrierSerializeInfo = new BarrierSerializeInfo();
		barrierSerializeInfo.m_guid = barrier.m_guid;
		barrierSerializeInfo.m_center = barrier.m_center;
		barrierSerializeInfo.m_widthInWorld = (barrier.m_endpoint1 - barrier.m_endpoint2).magnitude;
		barrierSerializeInfo.m_facingHorizontalAngle = VectorUtils.HorizontalAngle_Deg(barrier.m_facingDir);
		barrierSerializeInfo.m_bidirectional = barrier.m_bidirectional;
		barrierSerializeInfo.m_blocksVision = (sbyte)barrier.BlocksVision;
		barrierSerializeInfo.m_blocksAbilities = (sbyte)barrier.BlocksAbilities;
		barrierSerializeInfo.m_blocksMovement = (sbyte)barrier.BlocksMovement;
		barrierSerializeInfo.m_blocksMovementOnCrossover = (sbyte)barrier.BlocksMovementOnCrossover;
		barrierSerializeInfo.m_blocksPositionTargeting = (sbyte)barrier.BlocksPositionTargeting;
		barrierSerializeInfo.m_considerAsCover = barrier.m_considerAsCover;
		barrierSerializeInfo.m_team = (sbyte)barrier.m_team;
		int ownerIndex = ActorData.s_invalidActorIndex;
		if (barrier.m_owner != null)
		{
			ownerIndex = barrier.m_owner.ActorIndex;
		}
		barrierSerializeInfo.m_ownerIndex = ownerIndex;
		barrierSerializeInfo.m_makeClientGeo = barrier.m_makeClientGeo;
		return barrierSerializeInfo;
	}

	internal static Barrier CreateBarrierFromSerializeInfo(BarrierSerializeInfo info)
	{
		BlockingRules blocksVision = (BlockingRules)info.m_blocksVision;
		BlockingRules blocksAbilities = (BlockingRules)info.m_blocksAbilities;
		BlockingRules blocksMovement = (BlockingRules)info.m_blocksMovement;
		BlockingRules blocksMovementOnCrossover = (BlockingRules)info.m_blocksMovementOnCrossover;
		BlockingRules blocksPositionTargeting = (BlockingRules)info.m_blocksPositionTargeting;
		ActorData owner = null;
		if (info.m_ownerIndex != ActorData.s_invalidActorIndex)
		{
			owner = GameFlowData.Get().FindActorByActorIndex(info.m_ownerIndex);
		}
		Vector3 facingDir = VectorUtils.AngleDegreesToVector(info.m_facingHorizontalAngle);
		float width = info.m_widthInWorld / Board.Get().squareSize;
		return new Barrier(info.m_guid, "", info.m_center, facingDir, width, info.m_bidirectional, blocksVision, blocksAbilities, blocksMovement, blocksPositionTargeting, info.m_considerAsCover, -1, owner)
		{
			BlocksMovementOnCrossover = blocksMovementOnCrossover,
			m_makeClientGeo = info.m_makeClientGeo
		};
	}

	public virtual List<ServerClientUtils.SequenceStartData> GetSequenceStartDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_barrierSequencePrefabs != null && m_playSequences)
		{
			Quaternion targetRotation = Quaternion.LookRotation(m_facingDir);
			ActorData[] targetActorArray = new ActorData[0];
			foreach (GameObject prefab in m_barrierSequencePrefabs)
			{
				if (prefab != null)
				{
					bool requiresExtraParams = false;
					foreach (Sequence sequence in prefab.GetComponents<Sequence>())
					{
						if (sequence is OverwatchScanSequence || sequence is GroundLineSequence || sequence is ExoLaserHittingWallSequence)
						{
							requiresExtraParams = true;
							break;
						}
					}
					Sequence.IExtraSequenceParams[] extraParams = null;
					if (requiresExtraParams)
					{
						extraParams = new Sequence.IExtraSequenceParams[]
						{
							new GroundLineSequence.ExtraParams
							{
								startPos = m_endpoint2,
								endPos = m_endpoint1
							}
						};
					}
					ServerClientUtils.SequenceStartData seq = new ServerClientUtils.SequenceStartData(
						prefab, null, targetRotation, targetActorArray, m_owner, BarrierSequenceSource, extraParams);
					list.Add(seq);
				}
			}
		}
		return list;
	}

	public void DrawGizmos()
	{
		Vector3 b = new Vector3(0f, 0f, 0f);
		for (int i = 0; i < 3; i++)
		{
			b += new Vector3(0f, 0.3f, 0f);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(m_endpoint1 + b, m_endpoint2 + b);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(m_center + b, m_center + m_facingDir + b);
		}
	}

	// server-only
#if SERVER
	public class SortablePathEntry : IComparable
	{
		public MovementInstance m_movementInstance;
		public BoardSquarePathInfo m_triggeringPathSegment;

		private float m_cost;

		public SortablePathEntry(MovementInstance movementInstance, BoardSquarePathInfo triggeringPathSegment, float cost)
		{
			m_movementInstance = movementInstance;
			m_triggeringPathSegment = triggeringPathSegment;
			m_cost = cost;
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			SortablePathEntry sortablePathEntry = obj as SortablePathEntry;
			if (sortablePathEntry == null)
			{
				throw new ArgumentException("Object is not a SortablePathEntry");
			}
			if (m_movementInstance.m_wasChase == sortablePathEntry.m_movementInstance.m_wasChase)
			{
				return m_cost.CompareTo(sortablePathEntry.m_cost);
			}
			if (!m_movementInstance.m_wasChase)
			{
				return -1;
			}
			return 1;
		}
	}
#endif
}
