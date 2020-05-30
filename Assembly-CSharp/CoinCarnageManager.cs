using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CoinCarnageManager : NetworkBehaviour
{
	public enum Variant
	{
		PeriodicSingleTurnin,
		SeparateTurninsPerTeam
	}

	public Variant m_variant;

	public GameObject m_coinPrefab;

	public List<CoinSpawnInfo> m_coinSpawnPointGroups;

	public int m_coinAdditionalSpawnOnDeath = 1;

	public int m_teamTurninUnlockThreshold = 10;

	public int m_individualTurninUnlockThreshold = -1;

	public int m_turnInAreaAppearanceDelay = 5;

	public int m_turnInAreaPreviewTurns = 2;

	public int m_turnInAreaDuration = 2;

	public BoardRegion[] m_coinTurninLocations;

	public GameObject m_turninRegionPrefabFriendly;

	public GameObject m_turninRegionPrefabFriendlyPreview;

	public GameObject m_turninRegionPrefabEnemy;

	public GameObject m_turninRegionPrefabEnemyPreview;

	[Space(10f)]
	public bool m_showDebugCoinCountInNameplate = true;

	public bool m_showDebugUI;

	public Text m_debugTextLeft;

	public Text m_debugTextRight;

	public Color m_debugTurnInRegionColor = Color.magenta;

	private Dictionary<ActorData, int> m_actorToCoinCount;

	private List<CoinSpawnPointTrackingData> m_coinSpawnPointTracking;

	private List<CoinCarnageCoin> m_coins;

	private Dictionary<CoinCarnageCoin, CoinSpawnPointTrackingData> m_coinToSpawnPoint;

	[SyncVar(hook = "HookSetTurninRegionLocationA")]
	private int m_coinTurninIdxTeamA = -1;

	[SyncVar(hook = "HookSetTurninRegionLocationB")]
	private int m_coinTurninIdxTeamB = -1;

	private List<int> m_turninLocationChoices;

	[SyncVar(hook = "HookSetTurnsUntilTurnInSpawn")]
	private int m_turnsUntilTurnInSpawn;

	[SyncVar(hook = "HookSetTurnsUntilTurnInDeSpawn")]
	private int m_turnsUntilTurnInDeSpawn;

	private Dictionary<Team, GameObject> m_spawnedTurnInLocationInstances = new Dictionary<Team, GameObject>();

	private static CoinCarnageManager s_instance;

	private static int kRpcRpcActorPickedUpCoin;

	public int Networkm_coinTurninIdxTeamA
	{
		get
		{
			return m_coinTurninIdxTeamA;
		}
		[param: In]
		set
		{
			ref int coinTurninIdxTeamA = ref m_coinTurninIdxTeamA;
			if (NetworkServer.localClientActive && !base.syncVarHookGuard)
			{
				base.syncVarHookGuard = true;
				HookSetTurninRegionLocationA(value);
				base.syncVarHookGuard = false;
			}
			SetSyncVar(value, ref coinTurninIdxTeamA, 1u);
		}
	}

	public int Networkm_coinTurninIdxTeamB
	{
		get
		{
			return m_coinTurninIdxTeamB;
		}
		[param: In]
		set
		{
			ref int coinTurninIdxTeamB = ref m_coinTurninIdxTeamB;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetTurninRegionLocationB(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref coinTurninIdxTeamB, 2u);
		}
	}

	public int Networkm_turnsUntilTurnInSpawn
	{
		get
		{
			return m_turnsUntilTurnInSpawn;
		}
		[param: In]
		set
		{
			ref int turnsUntilTurnInSpawn = ref m_turnsUntilTurnInSpawn;
			if (NetworkServer.localClientActive && !base.syncVarHookGuard)
			{
				base.syncVarHookGuard = true;
				HookSetTurnsUntilTurnInSpawn(value);
				base.syncVarHookGuard = false;
			}
			SetSyncVar(value, ref turnsUntilTurnInSpawn, 4u);
		}
	}

	public int Networkm_turnsUntilTurnInDeSpawn
	{
		get
		{
			return m_turnsUntilTurnInDeSpawn;
		}
		[param: In]
		set
		{
			ref int turnsUntilTurnInDeSpawn = ref m_turnsUntilTurnInDeSpawn;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetTurnsUntilTurnInDeSpawn(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref turnsUntilTurnInDeSpawn, 8u);
		}
	}

	static CoinCarnageManager()
	{
		kRpcRpcActorPickedUpCoin = -867352238;
		NetworkBehaviour.RegisterRpcDelegate(typeof(CoinCarnageManager), kRpcRpcActorPickedUpCoin, InvokeRpcRpcActorPickedUpCoin);
		NetworkCRC.RegisterBehaviour("CoinCarnageManager", 0);
	}

	public static CoinCarnageManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		if (s_instance == null)
		{
			s_instance = this;
		}
		else
		{
			Log.Error("Multiple CoinCarnageManager components in this scene, remove extraneous ones.");
		}
		m_coins = new List<CoinCarnageCoin>();
		m_actorToCoinCount = new Dictionary<ActorData, int>();
		m_coinSpawnPointTracking = new List<CoinSpawnPointTrackingData>();
		m_coinToSpawnPoint = new Dictionary<CoinCarnageCoin, CoinSpawnPointTrackingData>();
		m_turninLocationChoices = new List<int>(m_coinTurninLocations.Length);
		for (int i = 0; i < m_coinTurninLocations.Length; i++)
		{
			m_coinTurninLocations.Initialize();
			m_turninLocationChoices.Add(i);
		}
		while (true)
		{
			Networkm_turnsUntilTurnInSpawn = m_turnInAreaAppearanceDelay;
			Networkm_turnsUntilTurnInDeSpawn = -1;
			if (!NetworkServer.active)
			{
				SpawnTurninRegionVisuals(m_coinTurninIdxTeamA, Team.TeamA);
				if (m_variant != 0)
				{
					SpawnTurninRegionVisuals(m_coinTurninIdxTeamB, Team.TeamB);
				}
			}
			if (m_debugTextLeft != null)
			{
				m_debugTextLeft.text = string.Empty;
			}
			if (m_debugTextRight != null)
			{
				m_debugTextRight.text = string.Empty;
			}
			return;
		}
	}

	private void Start()
	{
		using (List<CoinSpawnInfo>.Enumerator enumerator = m_coinSpawnPointGroups.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CoinSpawnInfo current = enumerator.Current;
				foreach (Transform spawnLocation in current.m_spawnLocations)
				{
					BoardSquare boardSquare = Board.Get().GetSquare(spawnLocation);
					if (boardSquare != null)
					{
						if (boardSquare.IsBaselineHeight())
						{
							m_coinSpawnPointTracking.Add(new CoinSpawnPointTrackingData(current.m_spawnRulePerSquare, boardSquare));
						}
					}
				}
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_coinTurninLocations == null)
		{
			return;
		}
		while (true)
		{
			BoardRegion[] coinTurninLocations = m_coinTurninLocations;
			foreach (BoardRegion boardRegion in coinTurninLocations)
			{
				boardRegion.Initialize();
				boardRegion.GizmosDrawRegion(m_debugTurnInRegionColor);
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	[Server]
	public void OnActorMoved(ActorData mover, BoardSquare movementSquare)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::OnActorMoved(ActorData,BoardSquare)' called on client");
					return;
				}
			}
		}
		using (List<CoinCarnageCoin>.Enumerator enumerator = m_coins.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CoinCarnageCoin current = enumerator.Current;
				if (!current.IsPickedUp())
				{
					if (current.GetSquare() == movementSquare)
					{
						current.PickUp(mover);
						AddCoinToActor(mover, 1);
						if (m_coinToSpawnPoint.ContainsKey(current))
						{
							m_coinToSpawnPoint[current].TurnOfLastPickup = GameFlowData.Get().CurrentTurn;
						}
						CheckTurninAvailability(mover.GetTeam());
					}
				}
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	[Server]
	public void OnActorDeath(ActorData actor)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::OnActorDeath(ActorData)' called on client");
					return;
				}
			}
		}
		if (!GameplayUtils.IsValidPlayer(actor))
		{
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		int num = GetCoinCountForActor(actor) + m_coinAdditionalSpawnOnDeath;
		int num2 = 0;
		ClearCoinsFromActor(actor);
		BoardSquare currentBoardSquare = actor.GetCurrentBoardSquare();
		if (currentBoardSquare == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogError("Actor dead, has no board square");
					return;
				}
			}
		}
		HashSet<BoardSquare> coinOccupiedSquares = GetCoinOccupiedSquares();
		for (int i = 0; i < 6; i++)
		{
			if (num2 < num)
			{
				List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(currentBoardSquare, i, true);
				for (int j = 0; j < squaresInBorderLayer.Count; j++)
				{
					if (num2 < num)
					{
						if (squaresInBorderLayer[j].IsBaselineHeight() && !coinOccupiedSquares.Contains(squaresInBorderLayer[j]))
						{
							SpawnCoinOnSquare(squaresInBorderLayer[j]);
							num2++;
						}
						continue;
					}
					break;
				}
				continue;
			}
			break;
		}
		if (num2 != num)
		{
			Debug.LogError("Did not spawn desired amount of coins!");
		}
	}

	[Server]
	public void OnTurnStart()
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::OnTurnStart()' called on client");
					return;
				}
			}
		}
		CleanupPickedUpCoins();
		SpawnCoinsForTurn();
		UpdateDebugUI();
		if (!m_showDebugCoinCountInNameplate)
		{
		}
	}

	[Server]
	public void OnTurnEnd()
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::OnTurnEnd()' called on client");
					return;
				}
			}
		}
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(Team.TeamA);
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				CheckTurninForActor(current);
			}
		}
		allTeamMembers = GameFlowData.Get().GetAllTeamMembers(Team.TeamB);
		using (List<ActorData>.Enumerator enumerator2 = allTeamMembers.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ActorData current2 = enumerator2.Current;
				CheckTurninForActor(current2);
			}
		}
		CleanupPickedUpCoins();
		if (m_variant == Variant.PeriodicSingleTurnin)
		{
			if (m_turnsUntilTurnInSpawn >= 0)
			{
				Networkm_turnsUntilTurnInSpawn = m_turnsUntilTurnInSpawn - 1;
			}
			if (m_turnsUntilTurnInDeSpawn >= 0)
			{
				Networkm_turnsUntilTurnInDeSpawn = m_turnsUntilTurnInDeSpawn - 1;
			}
			if (m_turnInAreaPreviewTurns >= 0)
			{
				if (m_turnsUntilTurnInSpawn == m_turnInAreaPreviewTurns)
				{
					ActivateTurnInArea(Team.Invalid);
				}
			}
			if (m_turnsUntilTurnInSpawn == 0)
			{
				Networkm_turnsUntilTurnInDeSpawn = m_turnInAreaDuration;
			}
			if (m_turnsUntilTurnInDeSpawn == 0)
			{
				DeactivateTurnInArea(Team.Invalid);
				Networkm_turnsUntilTurnInSpawn = m_turnInAreaAppearanceDelay;
				Networkm_turnsUntilTurnInDeSpawn = -1;
			}
		}
		UpdateDebugUI();
	}

	[Server]
	private void CheckTurninForActor(ActorData actor)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::CheckTurninForActor(ActorData)' called on client");
					return;
				}
			}
		}
		if (m_variant == Variant.PeriodicSingleTurnin)
		{
			if (m_turnsUntilTurnInSpawn > 0)
			{
				while (true)
				{
					switch (7)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		int num;
		if (actor.GetTeam() == Team.TeamA)
		{
			num = m_coinTurninIdxTeamA;
		}
		else
		{
			num = m_coinTurninIdxTeamB;
		}
		int num2 = num;
		if (num2 <= -1)
		{
			return;
		}
		while (true)
		{
			if (num2 >= m_coinTurninLocations.Length)
			{
				return;
			}
			int coinCountForActor = GetCoinCountForActor(actor);
			if (coinCountForActor <= 0)
			{
				return;
			}
			while (true)
			{
				BoardRegion boardRegion = m_coinTurninLocations[num2];
				if (boardRegion.IsActorInRegion(actor))
				{
					ObjectivePoints objectivePoints = ObjectivePoints.Get();
					if (objectivePoints != null)
					{
						objectivePoints.AdjustPoints(coinCountForActor, actor.GetTeam());
					}
					ClearCoinsFromActor(actor);
				}
				return;
			}
		}
	}

	[Client]
	private void HookSetTurninRegionLocationA(int newValue)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Client] function 'System.Void CoinCarnageManager::HookSetTurninRegionLocationA(System.Int32)' called on server");
					return;
				}
			}
		}
		Networkm_coinTurninIdxTeamA = newValue;
		bool preview = m_variant == Variant.PeriodicSingleTurnin;
		SpawnTurninRegionVisuals(m_coinTurninIdxTeamA, Team.TeamA, preview);
	}

	[Client]
	private void HookSetTurninRegionLocationB(int newValue)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void CoinCarnageManager::HookSetTurninRegionLocationB(System.Int32)' called on server");
			return;
		}
		Networkm_coinTurninIdxTeamB = newValue;
		if (m_variant == Variant.PeriodicSingleTurnin)
		{
			return;
		}
		while (true)
		{
			SpawnTurninRegionVisuals(m_coinTurninIdxTeamB, Team.TeamB);
			return;
		}
	}

	[Client]
	private void SpawnTurninRegionVisuals(int locationIndex, Team team, bool preview = false)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void CoinCarnageManager::SpawnTurninRegionVisuals(System.Int32,Team,System.Boolean)' called on server");
			return;
		}
		if (locationIndex == -1)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (m_spawnedTurnInLocationInstances.TryGetValue(team, out GameObject value))
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								UnityEngine.Object.Destroy(value);
								m_spawnedTurnInLocationInstances.Remove(team);
								return;
							}
						}
					}
					return;
				}
				}
			}
		}
		if (locationIndex < 0)
		{
			return;
		}
		while (true)
		{
			if (locationIndex >= m_coinTurninLocations.Length)
			{
				return;
			}
			BoardRegion boardRegion = m_coinTurninLocations[locationIndex];
			if (boardRegion == null)
			{
				return;
			}
			while (true)
			{
				if (GameFlowData.Get() != null)
				{
					if (GameFlowData.Get().activeOwnedActorData != null)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
							{
								bool flag = false;
								if (m_variant == Variant.PeriodicSingleTurnin)
								{
									flag = true;
								}
								else
								{
									flag = (team == GameFlowData.Get().activeOwnedActorData.GetTeam());
								}
								GameObject gameObject = null;
								if (flag)
								{
									GameObject gameObject2;
									if (preview)
									{
										gameObject2 = m_turninRegionPrefabFriendlyPreview;
									}
									else
									{
										gameObject2 = m_turninRegionPrefabFriendly;
									}
									gameObject = gameObject2;
								}
								else
								{
									GameObject gameObject3;
									if (preview)
									{
										gameObject3 = m_turninRegionPrefabEnemyPreview;
									}
									else
									{
										gameObject3 = m_turninRegionPrefabEnemy;
									}
									gameObject = gameObject3;
								}
								if (gameObject != null)
								{
									boardRegion.Initialize();
									GameObject gameObject4 = new GameObject();
									List<BoardSquare> squaresInRegion = boardRegion.GetSquaresInRegion();
									using (List<BoardSquare>.Enumerator enumerator = squaresInRegion.GetEnumerator())
									{
										while (enumerator.MoveNext())
										{
											BoardSquare current = enumerator.Current;
											if (current.WorldBounds.HasValue)
											{
												GameObject gameObject5 = UnityEngine.Object.Instantiate(gameObject);
												gameObject5.transform.position = current.WorldBounds.Value.center;
												gameObject5.transform.localScale = current.WorldBounds.Value.extents * 2f;
												gameObject5.transform.parent = gameObject4.transform;
											}
										}
									}
									if (m_spawnedTurnInLocationInstances.ContainsKey(team))
									{
										Log.Error("Turn in location already spawned for " + team);
										m_spawnedTurnInLocationInstances[team] = gameObject4;
									}
									else
									{
										m_spawnedTurnInLocationInstances.Add(team, gameObject4);
									}
								}
								if (m_variant == Variant.SeparateTurninsPerTeam)
								{
									ShowTurninAvailableMessage(flag);
								}
								return;
							}
							}
						}
					}
				}
				Log.Error("Could not find local player to spawn turn-in region.");
				return;
			}
		}
	}

	[Client]
	private void ShowTurninAvailableMessage(bool friendlyArea)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void CoinCarnageManager::ShowTurninAvailableMessage(System.Boolean)' called on server");
			return;
		}
		string text = "[TEMP] ";
		if (m_variant == Variant.SeparateTurninsPerTeam)
		{
			string str = text;
			object str2;
			if (friendlyArea)
			{
				str2 = "Your";
			}
			else
			{
				str2 = "Enemy";
			}
			text = str + (string)str2;
		}
		else
		{
			text += "The";
		}
		text += " coin turn-in area has been activated.";
		InterfaceManager.Get().DisplayAlert(text, (!friendlyArea) ? Color.red : Color.cyan, 7f);
	}

	[Client]
	private void HookSetTurnsUntilTurnInSpawn(int newValue)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Client] function 'System.Void CoinCarnageManager::HookSetTurnsUntilTurnInSpawn(System.Int32)' called on server");
					return;
				}
			}
		}
		Networkm_turnsUntilTurnInSpawn = newValue;
		if (m_turnsUntilTurnInSpawn > 0)
		{
			string alertText = $"[TEMP] {m_turnsUntilTurnInSpawn} turns until turn-in available!";
			InterfaceManager.Get().DisplayAlert(alertText, Color.cyan, 7f);
		}
		if (m_turnsUntilTurnInSpawn != 0)
		{
			return;
		}
		while (true)
		{
			ShowTurninAvailableMessage(true);
			SpawnTurninRegionVisuals(-1, Team.TeamA);
			SpawnTurninRegionVisuals(m_coinTurninIdxTeamA, Team.TeamA);
			return;
		}
	}

	[Client]
	private void HookSetTurnsUntilTurnInDeSpawn(int newValue)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Client] function 'System.Void CoinCarnageManager::HookSetTurnsUntilTurnInDeSpawn(System.Int32)' called on server");
					return;
				}
			}
		}
		Networkm_turnsUntilTurnInDeSpawn = newValue;
		if (m_turnsUntilTurnInDeSpawn > 0)
		{
			string alertText = $"[TEMP] {m_turnsUntilTurnInDeSpawn} turns until turn-in is no longer available!";
			InterfaceManager.Get().DisplayAlert(alertText, Color.cyan, 7f);
		}
	}

	[ClientRpc]
	public void RpcActorPickedUpCoin(int actorIndex)
	{
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
		if (actorData != null)
		{
			GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchObjectiveEvent, new GameEventManager.MatchObjectiveEventArgs
			{
				objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.CoinCollected,
				activatingActor = actorData
			});
		}
	}

	private int GetCoinCountForActor(ActorData actor)
	{
		if (m_actorToCoinCount.ContainsKey(actor))
		{
			return m_actorToCoinCount[actor];
		}
		return 0;
	}

	[Server]
	private void AddCoinToActor(ActorData actor, int amount)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::AddCoinToActor(ActorData,System.Int32)' called on client");
					return;
				}
			}
		}
		if (m_actorToCoinCount.ContainsKey(actor))
		{
			m_actorToCoinCount[actor] += amount;
		}
		else
		{
			m_actorToCoinCount[actor] = amount;
		}
		CallRpcActorPickedUpCoin(actor.ActorIndex);
		if (m_showDebugCoinCountInNameplate)
		{
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void ClearCoinsFromActor(ActorData actor)
	{
		m_actorToCoinCount[actor] = 0;
		if (!m_showDebugCoinCountInNameplate)
		{
		}
	}

	private void CheckTurninAvailability(Team forTeam)
	{
		int num = 0;
		if (forTeam != 0 && forTeam != Team.TeamB)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					throw new ArgumentException("Team must be Team A or Team B");
				}
			}
		}
		int num2 = (forTeam != 0) ? m_coinTurninIdxTeamB : m_coinTurninIdxTeamA;
		if (num2 != -1)
		{
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		bool flag = false;
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(forTeam);
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (true)
			{
				if (!enumerator.MoveNext())
				{
					break;
				}
				ActorData current = enumerator.Current;
				if (!m_actorToCoinCount.ContainsKey(current))
				{
					continue;
				}
				int num3 = m_actorToCoinCount[current];
				num += num3;
				if (m_individualTurninUnlockThreshold != -1)
				{
					if (num3 >= m_individualTurninUnlockThreshold)
					{
						goto IL_00f5;
					}
				}
				if (m_teamTurninUnlockThreshold == -1)
				{
					continue;
				}
				if (num < m_teamTurninUnlockThreshold)
				{
					continue;
				}
				goto IL_00f5;
				IL_00f5:
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		while (true)
		{
			if (m_turnInAreaAppearanceDelay > 0)
			{
				while (true)
				{
					switch (7)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			ActivateTurnInArea(forTeam);
			return;
		}
	}

	private void ActivateTurnInArea(Team forTeam)
	{
		if (m_turninLocationChoices.Count == 0)
		{
			if (m_variant == Variant.PeriodicSingleTurnin)
			{
				RefillRandomLocationChoices();
			}
			else if (m_variant == Variant.SeparateTurninsPerTeam)
			{
				if (forTeam == Team.TeamA)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							Networkm_coinTurninIdxTeamA = m_coinTurninIdxTeamB;
							return;
						}
					}
				}
				Networkm_coinTurninIdxTeamB = m_coinTurninIdxTeamA;
				return;
			}
		}
		if (m_turninLocationChoices.Count == 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Log.Error("No turn-in locations defined in manager!");
					return;
				}
			}
		}
		int index = GameplayRandom.Range(0, m_turninLocationChoices.Count);
		int num = m_turninLocationChoices[index];
		m_turninLocationChoices.RemoveAt(index);
		if (m_variant == Variant.SeparateTurninsPerTeam)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (forTeam == Team.TeamA)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								Networkm_coinTurninIdxTeamA = num;
								return;
							}
						}
					}
					Networkm_coinTurninIdxTeamB = num;
					return;
				}
			}
		}
		if (m_variant != 0)
		{
			return;
		}
		while (true)
		{
			Networkm_coinTurninIdxTeamA = num;
			Networkm_coinTurninIdxTeamB = num;
			return;
		}
	}

	private void DeactivateTurnInArea(Team forTeam)
	{
		Networkm_coinTurninIdxTeamA = -1;
		Networkm_coinTurninIdxTeamB = -1;
	}

	private void RefillRandomLocationChoices()
	{
		m_turninLocationChoices.Clear();
		for (int i = 0; i < m_coinTurninLocations.Length; i++)
		{
			m_turninLocationChoices.Add(i);
		}
		while (true)
		{
			return;
		}
	}

	private void SpawnCoinsForTurn()
	{
		for (int i = 0; i < m_coinSpawnPointTracking.Count; i++)
		{
			if (m_coinSpawnPointTracking[i].CanSpawnThisTurn(GameFlowData.Get().CurrentTurn))
			{
				CoinCarnageCoin coinCarnageCoin = SpawnCoinOnSquare(m_coinSpawnPointTracking[i].GetSpawnSquare());
				if (coinCarnageCoin != null)
				{
					m_coinSpawnPointTracking[i].NumSpawnedSoFar++;
					m_coinSpawnPointTracking[i].TurnOfLastSpawn = GameFlowData.Get().CurrentTurn;
					m_coinToSpawnPoint[coinCarnageCoin] = m_coinSpawnPointTracking[i];
				}
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private HashSet<BoardSquare> GetCoinOccupiedSquares()
	{
		HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
		using (List<CoinCarnageCoin>.Enumerator enumerator = m_coins.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CoinCarnageCoin current = enumerator.Current;
				if (!current.IsPickedUp())
				{
					hashSet.Add(current.GetSquare());
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return hashSet;
				}
			}
		}
	}

	private CoinCarnageCoin SpawnCoinOnSquare(BoardSquare square)
	{
		HashSet<BoardSquare> coinOccupiedSquares = GetCoinOccupiedSquares();
		if (!(square == null))
		{
			if (square.IsBaselineHeight())
			{
				if (!coinOccupiedSquares.Contains(square))
				{
					return CreateCoinObjectOnSquare(square);
				}
			}
		}
		return null;
	}

	private CoinCarnageCoin CreateCoinObjectOnSquare(BoardSquare square)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Log.Error("Attempted to call server function without server");
					return null;
				}
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(m_coinPrefab, square.ToVector3(), Quaternion.identity);
		CoinCarnageCoin component = gameObject.GetComponent<CoinCarnageCoin>();
		component.Initialize(square);
		NetworkServer.Spawn(gameObject);
		m_coins.Add(component);
		return component;
	}

	private void CleanupPickedUpCoins()
	{
		for (int num = m_coins.Count - 1; num >= 0; num--)
		{
			if (m_coins[num].IsPickedUp())
			{
				m_coinToSpawnPoint.Remove(m_coins[num]);
				m_coins[num].Destroy();
				m_coins.RemoveAt(num);
			}
			else if (m_coins[num] == null)
			{
				m_coins.RemoveAt(num);
			}
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private string GetCoinCountString(ActorData actor)
	{
		return "coin count: " + GetCoinCountForActor(actor);
	}

	private void UpdateDebugUI()
	{
		if (m_debugTextLeft == null)
		{
			return;
		}
		while (true)
		{
			if (!(m_debugTextRight == null) && m_showDebugUI)
			{
				string text = string.Empty;
				string text2 = string.Empty;
				int num = 0;
				using (List<CoinCarnageCoin>.Enumerator enumerator = m_coins.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CoinCarnageCoin current = enumerator.Current;
						if (!current.IsPickedUp())
						{
							num++;
						}
					}
				}
				m_debugTextLeft.text = "Coin count [active/total tracked] = " + num + " / " + m_coins.Count.ToString() + "\n";
				m_debugTextRight.text = "\n";
				using (Dictionary<ActorData, int>.Enumerator enumerator2 = m_actorToCoinCount.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<ActorData, int> current2 = enumerator2.Current;
						ActorData key = current2.Key;
						int value = current2.Value;
						if (key.GetTeam() == Team.TeamA)
						{
							string text3 = text;
							text = text3 + key.DisplayName + " -> " + value + "\n";
						}
						else if (key.GetTeam() == Team.TeamB)
						{
							string text3 = text2;
							text2 = text3 + key.DisplayName + " -> " + value + "\n";
						}
					}
				}
				m_debugTextLeft.text += text;
				m_debugTextRight.text += text2;
			}
			return;
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcActorPickedUpCoin(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcActorPickedUpCoin called on server.");
		}
		else
		{
			((CoinCarnageManager)obj).RpcActorPickedUpCoin((int)reader.ReadPackedUInt32());
		}
	}

	public void CallRpcActorPickedUpCoin(int actorIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcActorPickedUpCoin called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcActorPickedUpCoin);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndex);
		SendRPCInternal(networkWriter, 0, "RpcActorPickedUpCoin");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)m_coinTurninIdxTeamA);
			writer.WritePackedUInt32((uint)m_coinTurninIdxTeamB);
			writer.WritePackedUInt32((uint)m_turnsUntilTurnInSpawn);
			writer.WritePackedUInt32((uint)m_turnsUntilTurnInDeSpawn);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_coinTurninIdxTeamA);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_coinTurninIdxTeamB);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_turnsUntilTurnInSpawn);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_turnsUntilTurnInDeSpawn);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					m_coinTurninIdxTeamA = (int)reader.ReadPackedUInt32();
					m_coinTurninIdxTeamB = (int)reader.ReadPackedUInt32();
					m_turnsUntilTurnInSpawn = (int)reader.ReadPackedUInt32();
					m_turnsUntilTurnInDeSpawn = (int)reader.ReadPackedUInt32();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			HookSetTurninRegionLocationA((int)reader.ReadPackedUInt32());
		}
		if ((num & 2) != 0)
		{
			HookSetTurninRegionLocationB((int)reader.ReadPackedUInt32());
		}
		if ((num & 4) != 0)
		{
			HookSetTurnsUntilTurnInSpawn((int)reader.ReadPackedUInt32());
		}
		if ((num & 8) != 0)
		{
			HookSetTurnsUntilTurnInDeSpawn((int)reader.ReadPackedUInt32());
		}
	}
}
