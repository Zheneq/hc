// ROGUES
// SERVER
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class PowerUp : NetworkBehaviour
{
	public enum PowerUpCategory
	{
		NoCategory,
		Healing,
		Might,
		Movement,
		Energy
	}

	public interface IPowerUpListener
	{
		void OnPowerUpDestroyed(PowerUp powerUp);
		PowerUp[] GetActivePowerUps();
		void SetSpawningEnabled(bool enabled);
		void OnTurnTick();
		bool IsPowerUpSpawnPoint(BoardSquare square);
		void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor);
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public int m_pickupTeamAsInt;
		public bool m_ignoreSpawnSpline;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			sbyte value = (sbyte)m_pickupTeamAsInt;
			stream.Serialize(ref value);
			stream.Serialize(ref m_ignoreSpawnSpline);
		}

		// rogues
		//public override void XSP_SerializeToStream(NetworkWriter writer)
		//{
		//	sbyte b = (sbyte)m_pickupTeamAsInt;
		//	writer.Write(b);
		//	writer.Write(m_ignoreSpawnSpline);
		//}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte value = (sbyte)m_pickupTeamAsInt;
			stream.Serialize(ref value);
			m_pickupTeamAsInt = value;
			stream.Serialize(ref m_ignoreSpawnSpline);
		}

		// rogues
		//public override void XSP_DeserializeFromStream(NetworkReader reader)
		//{
		//	m_pickupTeamAsInt = (int)reader.ReadSByte();
		//	m_ignoreSpawnSpline = reader.ReadBoolean();
		//}
	}

	// server-only
#if SERVER
	private enum PowerupFate
	{
		Unclaimed,
		EvadedOver,
		StolenInCombat,
		KnockedBackOver,
		MovedOver,
		SpawnedUnderActor,
		BecameClaimableUnderActor,
		ActorBecameEligibleOverPowerup
	}
#endif

	private static int s_nextPowerupGuid;
	public Ability m_ability;
	public string m_powerUpName;
	public string m_powerUpToolTip;
	[AudioEvent(false)]
	public string m_audioEventPickUp = "ui_pickup_health";
	public GameObject m_sequencePrefab;
	public bool m_restrictPickupByTeam;
	public PowerUpCategory m_chatterCategory;

	[Tooltip("Treats the target as 'ally' for power up ability")]
	[SyncVar]
	private Team m_pickupTeam = Team.Objects;  // public in rogues
	[SyncVar(hook = "HookSetGuid")]
	private int m_guid = -1;
	[SyncVar]
	private uint m_sequenceSourceId;

	private List<int> m_clientSequenceIds = new List<int>();
	private BoardSquare m_boardSquare;

	[SyncVar]
	public bool m_isSpoil;
	[SyncVar]
	public bool m_ignoreSpawnSplineForSequence;

	public int m_duration;
	private int m_age;
	private bool m_pickedUp;
	private bool m_stolen;
	private ActorTag m_tags;
	private bool m_markedForRemoval;
	private SequenceSource _sequenceSource;

	// removed in rogues
	private static int kRpcRpcOnPickedUp = -430057904;
	// removed in rogues
	private static int kRpcRpcOnSteal = 1919536730;

#if SERVER
	// added in rogues
	private ActorData m_creator;
	// added in rogues
	private Ability m_creatorAbility;
	// added in rogues
	protected List<MovementResults> m_evadeResults = new List<MovementResults>();
	// added in rogues
	protected List<MovementResults> m_knockbackResults = new List<MovementResults>();
	// added in rogues
	protected List<MovementResults> m_normalMovementResults = new List<MovementResults>();
	// added in rogues
	private PowerUp.PowerupFate m_fate;
#endif

	public int Guid
	{
		get
		{
			return m_guid;
		}
		private set
		{
		}
	}

	public IPowerUpListener powerUpListener { get; set; }

	public BoardSquare boardSquare => m_boardSquare;

	public Team PickupTeam
	{
		get
		{
			return m_pickupTeam;
		}
		set
		{
#if SERVER
			if (NetworkServer.active && m_pickupTeam != value)
			{
				SetPickupTeam(value);
				if (m_boardSquare != null && m_boardSquare.occupant != null)
				{
					ActorData component = m_boardSquare.occupant.GetComponent<ActorData>();
					if (CanBePickedUpByActor(component))
					{
						SetFate(PowerupFate.BecameClaimableUnderActor);
						PickedUpOutsideResolution(component);
						GameplayMetricHelper.CollectPowerup(component);
					}
				}
			}
#endif
		}
	}

	internal SequenceSource SequenceSource
	{
		get
		{
			if (_sequenceSource == null)
			{
				_sequenceSource = new SequenceSource(null, null, false);
			}
			return _sequenceSource;
		}
	}

	public Team Networkm_pickupTeam
	{
		get
		{
			return m_pickupTeam;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_pickupTeam, 1u);
		}
	}

	public int Networkm_guid
	{
		get
		{
			return m_guid;
		}
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetGuid(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_guid, 2u);
		}
	}

	public uint Networkm_sequenceSourceId
	{
		get
		{
			return m_sequenceSourceId;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_sequenceSourceId, 4u);
		}
	}

	public bool Networkm_isSpoil
	{
		get
		{
			return m_isSpoil;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_isSpoil, 8u);
		}
	}

	public bool Networkm_ignoreSpawnSplineForSequence
	{
		get
		{
			return m_ignoreSpawnSplineForSequence;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_ignoreSpawnSplineForSequence, 16u);
		}
	}

	static PowerUp()
	{
		// reactor
		RegisterRpcDelegate(typeof(PowerUp), kRpcRpcOnPickedUp, InvokeRpcRpcOnPickedUp);
		RegisterRpcDelegate(typeof(PowerUp), kRpcRpcOnSteal, InvokeRpcRpcOnSteal);
		NetworkCRC.RegisterBehaviour("PowerUp", 0);
		// rogues
		//NetworkBehaviour.RegisterRpcDelegate(typeof(PowerUp), "RpcOnPickedUp", new NetworkBehaviour.CmdDelegate(PowerUp.InvokeRpcRpcOnPickedUp));
		//NetworkBehaviour.RegisterRpcDelegate(typeof(PowerUp), "RpcOnSteal", new NetworkBehaviour.CmdDelegate(PowerUp.InvokeRpcRpcOnSteal));
	}

	public void SetPickupTeam(Team value)
	{
		Networkm_pickupTeam = value;
	}

	public void AddTag(string powerupTag)
	{
		if (m_tags == null)
		{
			m_tags = gameObject.GetComponent<ActorTag>();
			if (m_tags == null)
			{
				m_tags = gameObject.AddComponent<ActorTag>();
			}
		}
		m_tags.AddTag(powerupTag);
	}

	public bool HasTag(string powerupTag)
	{
		return m_tags != null && m_tags.HasTag(powerupTag);
	}

	public void ClientSpawnSequences()
	{
		if (NetworkClient.active && m_clientSequenceIds.Count == 0)
		{
			BoardSquare squareFromPos = Board.Get().GetSquareFromPos(transform.position.x, transform.position.z);
			ExtraParams extraParams = new ExtraParams
			{
				m_pickupTeamAsInt = m_restrictPickupByTeam ? (int)m_pickupTeam : 2,
				m_ignoreSpawnSpline = m_ignoreSpawnSplineForSequence
			};
			Sequence.IExtraSequenceParams[] extraParams2 = new Sequence.IExtraSequenceParams[]
			{
				extraParams
			};
			if (_sequenceSource == null)
			{
				_sequenceSource = new SequenceSource(null, null, m_sequenceSourceId, false);
			}
			Sequence[] array = SequenceManager.Get().CreateClientSequences(m_sequencePrefab, squareFromPos, null, null, SequenceSource, extraParams2);
			bool flag = false;
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].RemoveAtTurnEnd = false;
					m_clientSequenceIds.Add(array[i].Id);
					if (!flag && PowerUpManager.Get() != null)
					{
						array[i].transform.parent = PowerUpManager.Get().GetSpawnedPersistentSequencesRoot().transform;
						flag = true;
					}
				}
			}
		}
	}

	public void CalculateBoardSquare()
	{
		m_boardSquare = Board.Get().GetSquareFromPos(transform.position.x, transform.position.z);
	}

	public void SetDuration(int duration)
	{
		m_duration = duration;
	}

	private void Awake()
	{
		if (NetworkServer.active)
		{
			Networkm_guid = s_nextPowerupGuid++;
			SequenceSource sequenceSource = SequenceSource;
			Networkm_sequenceSourceId = sequenceSource.RootID;
		}
	}

	private void HookSetGuid(int guid)
	{
		// reactor
		Networkm_guid = guid;
		PowerUpManager.Get().SetPowerUpGuid(this, guid);
		// rogues
		//if (guid != m_guid)
		//{
		//	PowerUpManager.Get().SetPowerUpGuid(this, guid);
		//}
	}

	private void Start()
	{
		if (m_ability == null)
		{
			Log.Error("PowerUp " + this + " needs a valid Ability assigned in the inspector for its prefab");
		}
		transform.parent = PowerUpManager.Get().GetSpawnedPowerupsRoot().transform;
		tag = "powerup";
		int layer = LayerMask.NameToLayer("PowerUp");
		foreach (Transform t in GetComponentsInChildren<Transform>())
		{
			t.gameObject.layer = layer;
		}
		if (m_boardSquare == null)
		{
			CalculateBoardSquare();
		}
		if (NetworkClient.active)
		{
			if (PowerUpManager.Get() != null)
			{
				PowerUpManager.Get().TrackClientPowerUp(this);
			}
			ClientSpawnSequences();
		}
	}

	[Server]
	public void CheckForPickupOnSpawn()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PowerUp::CheckForPickupOnSpawn()' called on client");
			return;
		}

#if SERVER
		if (NetworkServer.active && m_boardSquare != null && m_boardSquare.occupant != null)
		{
			ActorData component = m_boardSquare.occupant.GetComponent<ActorData>();
			if (CanBePickedUpByActor(component))
			{
				SetFate(PowerupFate.SpawnedUnderActor);
				PickedUpOutsideResolution(component);
				GameplayMetricHelper.CollectPowerup(component);
			}
		}
#endif
	}

	// removed in rogues, empty in reactor
	public void CheckForPickupOnTurnStart()
	{
		// TODO some server code might have been here
	}

	public void MarkSequencesForRemoval()
	{
		m_markedForRemoval = true;
		if (SequenceManager.Get() != null)
		{
			for (int i = 0; i < m_clientSequenceIds.Count; i++)
			{
				Sequence sequence = SequenceManager.Get().FindSequence(m_clientSequenceIds[i]);
				if (sequence != null)
				{
					sequence.MarkForRemoval();
				}
			}
		}
	}

	public bool WasMarkedForRemoval()
	{
		return m_markedForRemoval;
	}

	public void SetHideSequence(bool hide)
	{
		for (int i = 0; i < m_clientSequenceIds.Count; i++)
		{
			Sequence sequence = SequenceManager.Get().FindSequence(m_clientSequenceIds[i]);
			if (sequence != null)
			{
				sequence.gameObject.transform.localPosition = hide
					? new Vector3(0f, -100f, 0f)
					: Vector3.zero;
			}
		}
	}

	[ClientRpc]
	private void RpcOnPickedUp(int pickedUpByActorIndex)
	{
		Client_OnPickedUp(pickedUpByActorIndex);
	}

	public void Client_OnPickedUp(int pickedUpByActorIndex)
	{
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(pickedUpByActorIndex);
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null && clientFog.IsVisible(boardSquare))
		{
			AudioManager.PostEvent(m_audioEventPickUp, (actorData == null) ? gameObject : actorData.gameObject);
		}
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.PowerUpActivated, new GameEventManager.PowerUpActivatedArgs
		{
			byActor = actorData,
			powerUp = this
		});
		MarkSequencesForRemoval();
	}

	[ClientRpc]
	private void RpcOnSteal(int actorIndexFor3DAudio)
	{
		Client_OnSteal(actorIndexFor3DAudio);
	}

	public void Client_OnSteal(int actorIndexFor3DAudio)
	{
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null && clientFog.IsVisible(boardSquare))
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndexFor3DAudio);
			AudioManager.PostEvent(m_audioEventPickUp, (actorData == null) ? gameObject : actorData.gameObject);
		}
		MarkSequencesForRemoval();
	}

	[Server]
	internal void Destroy()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PowerUp::Destroy()' called on client");
			return;
		}
		if (powerUpListener != null)
		{
			powerUpListener.OnPowerUpDestroyed(this);
		}
		NetworkServer.Destroy(gameObject);
	}

	private void OnDestroy()
	{
		if (NetworkClient.active)
		{
			if (PowerUpManager.Get() != null)
			{
				PowerUpManager.Get().UntrackClientPowerUp(this);
			}
			MarkSequencesForRemoval();
			m_clientSequenceIds.Clear();
		}
		if (PowerUpManager.Get() != null)
		{
			PowerUpManager.Get().OnPowerUpDestroy(this);
		}
	}

	public bool TeamAllowedForPickUp(Team team)
	{
		return !m_restrictPickupByTeam || team == PickupTeam;
	}

	internal void OnTurnTick()
	{
#if SERVER
		if (NetworkServer.active)
		{
			m_age++;
			if (m_pickedUp || m_stolen)
			{
				Destroy();
				return;
			}
			if (m_duration > 0 && m_age >= m_duration)
			{
				Destroy();
				return;
			}
			if (m_fate != PowerupFate.Unclaimed)
			{
				Debug.LogError("Powerup ended a turn indicating it should have been claimed: " + m_fate.ToString() + "\nChanging it back to Unclaimed");
				SetFate(PowerupFate.Unclaimed);
			}
		}
#endif
	}

	public bool CanBeStolen()
	{
#if SERVER
		return !NetworkServer.active
			|| m_fate == PowerupFate.Unclaimed
			|| m_fate == PowerupFate.StolenInCombat;
#else
		return true;
#endif
	}

	// server-only
#if SERVER
	public bool CanBePickedUpByActor(ActorData taker)
	{
		return !(taker == null) && !taker.IsDead() && (!(SpawnPointManager.Get() != null) || SpawnPointManager.Get().m_respawnActorsCanCollectPowerUps || !taker.IgnoreForAbilityHits) && m_fate == PowerupFate.Unclaimed && TeamAllowedForPickUp(taker.GetTeam()) && !taker.GetActorStatus().HasStatus(StatusType.CantCollectPowerups, true);
	}
#endif

	// server-only
#if SERVER
	public void SetCreator(ActorData creator, Ability ability)
	{
		m_creator = creator;
		m_creatorAbility = ability;
	}
#endif

	// server-only
#if SERVER
	public ActorData GetCreator()
	{
		return m_creator;
	}
#endif

	// server-only
#if SERVER
	public Ability GetCreatorAbility()
	{
		return m_creatorAbility;
	}
#endif

	// server-only
#if SERVER
	private void SetFate(PowerUp.PowerupFate fate)
	{
		m_fate = fate;
	}

	// server-only
	public void GatherResultsInResponseToEvades(MovementCollection collection)
	{
		m_evadeResults.Clear();
		GatherMovementResults(collection, ref m_evadeResults);
		foreach (MovementResults movementResults in m_evadeResults)
		{
			movementResults.m_triggeringPath.m_moverHasGameplayHitHere = true;
			movementResults.m_triggeringPath.m_updateLastKnownPos = movementResults.ShouldMovementHitUpdateTargetLastKnownPos(movementResults.m_triggeringMover);
			Log.Info($"UpdateLastKnownPos {movementResults.m_triggeringMover?.DisplayName} " +
			         $"{movementResults.m_triggeringPath.square?.GetGridPos()} " +
			         $"{(movementResults.m_triggeringPath.m_updateLastKnownPos ? "" : "NOT ")} updating for evade movement powerup hit"); // custom debug
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
			m_knockbackResults[i].m_triggeringPath.m_updateLastKnownPos = m_knockbackResults[i].ShouldMovementHitUpdateTargetLastKnownPos(m_knockbackResults[i].m_triggeringMover);
			TheatricsManager.Get().OnKnockbackMovementHitGathered(m_knockbackResults[i].GetTriggeringActor());
			Log.Info($"UpdateLastKnownPos {m_knockbackResults[i].m_triggeringMover?.DisplayName} " +
			         $"{m_knockbackResults[i].m_triggeringPath.square?.GetGridPos()} " +
			         $"{(m_knockbackResults[i].m_triggeringPath.m_updateLastKnownPos ? "" : "NOT ")} updating for knockback movement powerup hit"); // custom debug
		}
	}

	// server-only
	public void ExecuteUnexecutedMovementResults_PowerUp(MovementStage movementStage, bool failsafe)
	{
		if (movementStage == MovementStage.Evasion)
		{
			MovementResults.ExecuteUnexecutedHits(m_evadeResults, failsafe);
			return;
		}
		if (movementStage == MovementStage.Knockback)
		{
			MovementResults.ExecuteUnexecutedHits(m_knockbackResults, failsafe);
			return;
		}
		if (movementStage == MovementStage.Normal)
		{
			MovementResults.ExecuteUnexecutedHits(m_normalMovementResults, failsafe);
		}
	}

	// server-only
	public void ExecuteUnexecutedMovementResultsForDistance_PowerUp(float distance, MovementStage movementStage, bool failsafe, out bool stillHasUnexecutedHits, out float nextUnexecutedHitDistance)
	{
		stillHasUnexecutedHits = false;
		nextUnexecutedHitDistance = -1f;
		if (movementStage == MovementStage.Evasion)
		{
			MovementResults.ExecuteUnexecutedHitsForDistance(m_evadeResults, distance, failsafe, out stillHasUnexecutedHits, out nextUnexecutedHitDistance);
			return;
		}
		if (movementStage == MovementStage.Knockback)
		{
			MovementResults.ExecuteUnexecutedHitsForDistance(m_knockbackResults, distance, failsafe, out stillHasUnexecutedHits, out nextUnexecutedHitDistance);
			return;
		}
		if (movementStage == MovementStage.Normal)
		{
			MovementResults.ExecuteUnexecutedHitsForDistance(m_normalMovementResults, distance, failsafe, out stillHasUnexecutedHits, out nextUnexecutedHitDistance);
		}
	}

	// server-only
	public List<MovementResults> GetMovementResultsForMovementStage(MovementStage movementStage)
	{
		if (movementStage == MovementStage.Evasion)
		{
			return m_evadeResults;
		}
		if (movementStage == MovementStage.Knockback)
		{
			return m_knockbackResults;
		}
		if (movementStage == MovementStage.Normal)
		{
			return m_normalMovementResults;
		}
		return null;
	}

	// server-only
	public virtual void GatherMovementResults(MovementCollection movement, ref List<MovementResults> movementResultsList)
	{
		if (m_fate != PowerupFate.Unclaimed)
		{
			return;
		}
		MovementInstance movementInstance = null;
		float currentShortestMoveCost = 0f;
		BoardSquarePathInfo triggeringPathSegment = null;
		foreach (MovementInstance movementInstance2 in movement.m_movementInstances)
		{
			if (CanBePickedUpByActor(movementInstance2.m_mover))
			{
				for (BoardSquarePathInfo boardSquarePathInfo = movementInstance2.m_path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
				{
					if ((movementInstance2.m_groundBased || boardSquarePathInfo.IsPathEndpoint()) && !boardSquarePathInfo.IsPathStartPoint() && !boardSquarePathInfo.m_moverClashesHere && boardSquarePathInfo.square == m_boardSquare && MovementUtils.IsBetterMovementPathForGameplayThan(movementInstance2, boardSquarePathInfo.moveCost, movementInstance, currentShortestMoveCost))
					{
						movementInstance = movementInstance2;
						currentShortestMoveCost = boardSquarePathInfo.moveCost;
						triggeringPathSegment = boardSquarePathInfo;
						break;
					}
				}
			}
		}
		if (movementInstance != null)
		{
			ActorData mover = movementInstance.m_mover;
			if (m_ability is PowerUp_Standard_Ability)
			{
				MovementResults item = BuildPickUpMovementResults(mover, triggeringPathSegment, movement.m_movementStage);
				movementResultsList.Add(item);
				if (movement.m_movementStage == MovementStage.Evasion)
				{
					SetFate(PowerupFate.EvadedOver);
					GameplayMetricHelper.CollectPowerup(mover);
					return;
				}
				if (movement.m_movementStage == MovementStage.Knockback)
				{
					SetFate(PowerupFate.KnockedBackOver);
					GameplayMetricHelper.CollectPowerup(mover);
					return;
				}
				if (movement.m_movementStage == MovementStage.Normal)
				{
					SetFate(PowerupFate.MovedOver);
					GameplayMetricHelper.CollectPowerup(mover);
					return;
				}
			}
			else
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Powerup ",
					name,
					" ",
					m_powerUpName,
					" does not have a PowerUp_Standard_Ability.  All powerups need one, now."
				}));
			}
		}
	}

	// server-only
	public virtual void GatherMovementResultsFromSegment(ActorData mover, MovementInstance movementInstance, MovementStage movementStage, BoardSquarePathInfo sourcePath, BoardSquarePathInfo destPath, ref List<MovementResults> movementResultsList)
	{
		if (m_fate != PowerupFate.Unclaimed)
		{
			return;
		}
		if (!CanBePickedUpByActor(mover))
		{
			return;
		}
		if (destPath.square != m_boardSquare)
		{
			return;
		}
		if (!movementInstance.m_groundBased && !destPath.IsPathEndpoint())
		{
			return;
		}
		if (m_ability is PowerUp_Standard_Ability)
		{
			MovementResults item = BuildPickUpMovementResults(mover, destPath, movementStage);
			movementResultsList.Add(item);
			if (movementStage == MovementStage.Evasion)
			{
				SetFate(PowerupFate.EvadedOver);
				GameplayMetricHelper.CollectPowerup(mover);
				return;
			}
			if (movementStage == MovementStage.Knockback)
			{
				SetFate(PowerupFate.KnockedBackOver);
				GameplayMetricHelper.CollectPowerup(mover);
				return;
			}
			if (movementStage == MovementStage.Normal)
			{
				SetFate(PowerupFate.MovedOver);
				GameplayMetricHelper.CollectPowerup(mover);
			}
		}
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
	public void GatherGrossDamageResults_PowerUp_Evasion(ref Dictionary<ActorData, int> actorToGrossDamage_real, ref Dictionary<ActorData, ServerGameplayUtils.DamageDodgedStats> stats)
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
	public MovementResults BuildPickUpMovementResults(ActorData mover, BoardSquarePathInfo triggeringPathSegment, MovementStage movementStage)
	{
		ActorHitResults actorHitResults = (m_ability as PowerUp_Standard_Ability).CreateActorHitResults(this, mover, m_boardSquare.ToVector3(), null, null, false);
		actorHitResults.CanBeReactedTo = false;
		actorHitResults.AddPowerupForRemoval(this);
		MovementResults movementResults = new MovementResults(movementStage);
		movementResults.SetupTriggerData(mover, triggeringPathSegment);
		movementResults.SetupGameplayData(this, actorHitResults);
		ServerClientUtils.SequenceStartData startData = new ServerClientUtils.SequenceStartData(m_ability.m_sequencePrefab, m_boardSquare, null, mover, SequenceSource, null);
		movementResults.AddSequenceStartOverride(startData, SequenceSource, true);
		return movementResults;
	}

	// server-only
	public void OnPickedUp(ActorData user)
	{
		m_pickedUp = true;
		CallRpcOnPickedUp(user.ActorIndex);
		EventLogMessage eventLogMessage = new EventLogMessage("match", "ActorPickup");
		GameManager gameManager = GameManager.Get();
		eventLogMessage.AddData("ProcessCode", (HydrogenConfig.Get() != null) ? HydrogenConfig.Get().ProcessCode : "?");
		eventLogMessage.AddData("BuildVersion", BuildVersion.FullVersionString);
		eventLogMessage.AddData("Map", gameManager ? gameManager.GameConfig.Map : "?");  // GameMission instead of GameConfig in rogues
																						 //eventLogMessage.AddData("Encounter", gameManager ? gameManager.GameMission.Encounter : "?");  // rogues
		eventLogMessage.AddData("Turn", GameFlowData.Get().CurrentTurn);
		GenerateEventData(eventLogMessage, false);
		user.GenerateEventData(eventLogMessage, false);
		eventLogMessage.Write();
	}

	// server-only
	public void GenerateEventData(EventLogMessage eventLogMessage, bool condensed = true)
	{
		eventLogMessage.AddData("PowerUpName", m_powerUpName);
		eventLogMessage.AddData("PowerUpAbilityType", m_ability.m_abilityName);
		if (!condensed && m_boardSquare != null)
		{
			eventLogMessage.AddData("PowerUpLocationX", m_boardSquare.GetGridPos().x);
			eventLogMessage.AddData("PowerUpLocationY", m_boardSquare.GetGridPos().y);
		}
	}

	// server-only
	public void PickedUpOutsideResolution(ActorData user)
	{
		MovementResults movementResults = BuildPickUpMovementResults(user, null, MovementStage.INVALID);
		movementResults.ExecuteUnexecutedMovementHits(false);
		if (ServerResolutionManager.Get() != null)
		{
			ServerResolutionManager.Get().SendNonResolutionActionToClients(movementResults);
		}
		OnPickedUp(user);
		Destroy();
	}

	// server-only
	public void ActorBecameAbleToCollectPowerups(ActorData user)
	{
		if (CanBePickedUpByActor(user))
		{
			SetFate(PowerupFate.ActorBecameEligibleOverPowerup);
			PickedUpOutsideResolution(user);
			GameplayMetricHelper.CollectPowerup(user);
		}
	}

	// server-only
	public void OnWillBeStolen(ActorData stealer)
	{
		if (m_fate == PowerupFate.Unclaimed || m_fate == PowerupFate.StolenInCombat)
		{
			SetFate(PowerupFate.StolenInCombat);
			GameplayMetricHelper.CollectPowerup(stealer);
			return;
		}
		Debug.LogError(string.Concat(new object[]
		{
			"Powerup ",
			name,
			" ",
			m_powerUpName,
			" is going to be stolen, but its fate is already decided to be ",
			(int)m_fate,
			" ('",
			m_fate.ToString(),
			"')."
		}));
	}

	// server-only
	public void OnStealingHit(ActorData thief)
	{
		if (NetworkServer.active && !m_pickedUp)
		{
			m_stolen = true;
			CallRpcOnSteal(thief.ActorIndex);
		}
	}

	// server-only
	public bool HasBeenPickedUp()
	{
		return m_pickedUp;
	}

	// server-only
	public ActorHitResults BuildPowerupHitResults(ActorData pickupActor, BoardSquare squareForHitOrigin, StandardPowerUpAbilityModData powerupMod, EffectSource effectSourceOverride, bool isDirectActorHit)
	{
		PowerUp_Standard_Ability powerUp_Standard_Ability = m_ability as PowerUp_Standard_Ability;
		if (powerUp_Standard_Ability != null)
		{
			ActorHitResults actorHitResults = powerUp_Standard_Ability.CreateActorHitResults(this, pickupActor, squareForHitOrigin.ToVector3(), powerupMod, effectSourceOverride, isDirectActorHit);
			actorHitResults.CanBeReactedTo = false;
			return actorHitResults;
		}
		return null;
	}

	// server-only
	public AbilityResults_Powerup BuildPowerupResultsForAbilityHit(ActorData pickupActor, StandardPowerUpAbilityModData powerupMod)
	{
		ActorHitResults powerupHitResults = BuildPowerupHitResults(pickupActor, m_boardSquare, powerupMod, null, false);
		return new AbilityResults_Powerup(this, powerupHitResults, m_ability.m_sequencePrefab, m_boardSquare, SequenceSource, null);
	}

	// server-only
	public MovementResults BuildDirectPowerupHitResults(ActorData pickupActor, BoardSquare squareForHitOrigin, Ability sourceAbility, ActorData powerupCreator, StandardPowerUpAbilityModData powerupMod)
	{
		if (m_ability != null)
		{
			SequenceSource parentSequenceSource = new SequenceSource(null, null, true, null, null);
			MovementResults movementResults = new MovementResults(MovementStage.INVALID);
			ActorHitResults reactionHitResults = BuildPowerupHitResults(pickupActor, squareForHitOrigin, powerupMod, (sourceAbility != null) ? sourceAbility.AsEffectSource() : null, true);
			ActorData caster = pickupActor;
			if (powerupCreator != null)
			{
				caster = powerupCreator;
			}
			movementResults.SetupTriggerData(pickupActor, null);
			movementResults.SetupGameplayDataForAbility(sourceAbility, caster);
			movementResults.AddActorHitResultsForReaction(reactionHitResults);
			movementResults.SetupSequenceData(m_ability.m_sequencePrefab, pickupActor.GetCurrentBoardSquare(), parentSequenceSource, null, true);
			return movementResults;
		}
		return null;
	}

	// added in rogues
	//private void MirrorProcessed()
	//{
	//}

#endif

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcOnPickedUp(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcOnPickedUp called on server.");
			return;
		}
		((PowerUp)obj).RpcOnPickedUp((int)reader.ReadPackedUInt32()); // ReadPackedInt32 in rogues

	}

	protected static void InvokeRpcRpcOnSteal(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcOnSteal called on server.");
			return;
		}
		((PowerUp)obj).RpcOnSteal((int)reader.ReadPackedUInt32()); // ReadPackedInt32 in rogues
	}

	public void CallRpcOnPickedUp(int pickedUpByActorIndex)
	{
		// reactor
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcOnPickedUp called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcOnPickedUp);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)pickedUpByActorIndex);
		SendRPCInternal(networkWriter, 0, "RpcOnPickedUp");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.WritePackedInt32(pickedUpByActorIndex);
		//SendRPCInternal(typeof(PowerUp), "RpcOnPickedUp", networkWriter, 0);
	}

	public void CallRpcOnSteal(int actorIndexFor3DAudio)
	{
		// reactor
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcOnSteal called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcOnSteal);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndexFor3DAudio);
		SendRPCInternal(networkWriter, 0, "RpcOnSteal");
		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//networkWriter.WritePackedInt32(actorIndexFor3DAudio);
		//SendRPCInternal(typeof(PowerUp), "RpcOnSteal", networkWriter, 0);
	}

	// reactor
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write((int)m_pickupTeam);
			writer.WritePackedUInt32((uint)m_guid);
			writer.WritePackedUInt32(m_sequenceSourceId);
			writer.Write(m_isSpoil);
			writer.Write(m_ignoreSpawnSplineForSequence);
			return true;
		}
		bool flag = false;
		if ((syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_pickupTeam);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_guid);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32(m_sequenceSourceId);
		}
		if ((syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_isSpoil);
		}
		if ((syncVarDirtyBits & 0x10) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_ignoreSpawnSplineForSequence);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	// rogues
	//public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	//{
	//	bool result = base.OnSerialize(writer, forceAll);
	//	if (forceAll)
	//	{
	//		writer.WritePackedInt32((int)m_pickupTeam);
	//		writer.WritePackedInt32(m_guid);
	//		writer.WritePackedUInt32(m_sequenceSourceId);
	//		writer.Write(m_isSpoil);
	//		writer.Write(m_ignoreSpawnSplineForSequence);
	//		return true;
	//	}
	//	writer.WritePackedUInt64(base.syncVarDirtyBits);
	//	if ((base.syncVarDirtyBits & 1UL) != 0UL)
	//	{
	//		writer.WritePackedInt32((int)m_pickupTeam);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 2UL) != 0UL)
	//	{
	//		writer.WritePackedInt32(m_guid);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 4UL) != 0UL)
	//	{
	//		writer.WritePackedUInt32(m_sequenceSourceId);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 8UL) != 0UL)
	//	{
	//		writer.Write(m_isSpoil);
	//		result = true;
	//	}
	//	if ((base.syncVarDirtyBits & 16UL) != 0UL)
	//	{
	//		writer.Write(m_ignoreSpawnSplineForSequence);
	//		result = true;
	//	}
	//	return result;
	//}

	// reactor
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			m_pickupTeam = (Team)reader.ReadInt32();
			m_guid = (int)reader.ReadPackedUInt32();
			m_sequenceSourceId = reader.ReadPackedUInt32();
			m_isSpoil = reader.ReadBoolean();
			m_ignoreSpawnSplineForSequence = reader.ReadBoolean();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_pickupTeam = (Team)reader.ReadInt32();
		}
		if ((num & 2) != 0)
		{
			HookSetGuid((int)reader.ReadPackedUInt32());
		}
		if ((num & 4) != 0)
		{
			m_sequenceSourceId = reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			m_isSpoil = reader.ReadBoolean();
		}
		if ((num & 0x10) != 0)
		{
			m_ignoreSpawnSplineForSequence = reader.ReadBoolean();
		}
	}

	// rogues
	//public override void OnDeserialize(NetworkReader reader, bool initialState)
	//{
	//	base.OnDeserialize(reader, initialState);
	//	if (initialState)
	//	{
	//		Team networkm_pickupTeam = (Team)reader.ReadPackedInt32();
	//		Networkm_pickupTeam = networkm_pickupTeam;
	//		int num = reader.ReadPackedInt32();
	//		HookSetGuid(num);
	//		Networkm_guid = num;
	//		uint networkm_sequenceSourceId = reader.ReadPackedUInt32();
	//		Networkm_sequenceSourceId = networkm_sequenceSourceId;
	//		bool networkm_isSpoil = reader.ReadBoolean();
	//		Networkm_isSpoil = networkm_isSpoil;
	//		bool networkm_ignoreSpawnSplineForSequence = reader.ReadBoolean();
	//		Networkm_ignoreSpawnSplineForSequence = networkm_ignoreSpawnSplineForSequence;
	//		return;
	//	}
	//	long num2 = (long)reader.ReadPackedUInt64();
	//	if ((num2 & 1L) != 0L)
	//	{
	//		Team networkm_pickupTeam2 = (Team)reader.ReadPackedInt32();
	//		Networkm_pickupTeam = networkm_pickupTeam2;
	//	}
	//	if ((num2 & 2L) != 0L)
	//	{
	//		int num3 = reader.ReadPackedInt32();
	//		HookSetGuid(num3);
	//		Networkm_guid = num3;
	//	}
	//	if ((num2 & 4L) != 0L)
	//	{
	//		uint networkm_sequenceSourceId2 = reader.ReadPackedUInt32();
	//		Networkm_sequenceSourceId = networkm_sequenceSourceId2;
	//	}
	//	if ((num2 & 8L) != 0L)
	//	{
	//		bool networkm_isSpoil2 = reader.ReadBoolean();
	//		Networkm_isSpoil = networkm_isSpoil2;
	//	}
	//	if ((num2 & 16L) != 0L)
	//	{
	//		bool networkm_ignoreSpawnSplineForSequence2 = reader.ReadBoolean();
	//		Networkm_ignoreSpawnSplineForSequence = networkm_ignoreSpawnSplineForSequence2;
	//	}
	//}
}
