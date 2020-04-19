using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CoinCarnageManager : NetworkBehaviour
{
	public CoinCarnageManager.Variant m_variant;

	public GameObject m_coinPrefab;

	public List<CoinSpawnInfo> m_coinSpawnPointGroups;

	public int m_coinAdditionalSpawnOnDeath = 1;

	public int m_teamTurninUnlockThreshold = 0xA;

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

	private static int kRpcRpcActorPickedUpCoin = -0x33B2BEAE;

	static CoinCarnageManager()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(CoinCarnageManager), CoinCarnageManager.kRpcRpcActorPickedUpCoin, new NetworkBehaviour.CmdDelegate(CoinCarnageManager.InvokeRpcRpcActorPickedUpCoin));
		NetworkCRC.RegisterBehaviour("CoinCarnageManager", 0);
	}

	public static CoinCarnageManager Get()
	{
		return CoinCarnageManager.s_instance;
	}

	private void Awake()
	{
		if (CoinCarnageManager.s_instance == null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.Awake()).MethodHandle;
			}
			CoinCarnageManager.s_instance = this;
		}
		else
		{
			Log.Error("Multiple CoinCarnageManager components in this scene, remove extraneous ones.", new object[0]);
		}
		this.m_coins = new List<CoinCarnageCoin>();
		this.m_actorToCoinCount = new Dictionary<ActorData, int>();
		this.m_coinSpawnPointTracking = new List<CoinSpawnPointTrackingData>();
		this.m_coinToSpawnPoint = new Dictionary<CoinCarnageCoin, CoinSpawnPointTrackingData>();
		this.m_turninLocationChoices = new List<int>(this.m_coinTurninLocations.Length);
		for (int i = 0; i < this.m_coinTurninLocations.Length; i++)
		{
			this.m_coinTurninLocations.Initialize();
			this.m_turninLocationChoices.Add(i);
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		this.Networkm_turnsUntilTurnInSpawn = this.m_turnInAreaAppearanceDelay;
		this.Networkm_turnsUntilTurnInDeSpawn = -1;
		if (!NetworkServer.active)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			this.SpawnTurninRegionVisuals(this.m_coinTurninIdxTeamA, Team.TeamA, false);
			if (this.m_variant != CoinCarnageManager.Variant.PeriodicSingleTurnin)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				this.SpawnTurninRegionVisuals(this.m_coinTurninIdxTeamB, Team.TeamB, false);
			}
		}
		if (this.m_debugTextLeft != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_debugTextLeft.text = string.Empty;
		}
		if (this.m_debugTextRight != null)
		{
			this.m_debugTextRight.text = string.Empty;
		}
	}

	private void Start()
	{
		using (List<CoinSpawnInfo>.Enumerator enumerator = this.m_coinSpawnPointGroups.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CoinSpawnInfo coinSpawnInfo = enumerator.Current;
				foreach (Transform u001D in coinSpawnInfo.m_spawnLocations)
				{
					BoardSquare boardSquare = Board.\u000E().\u000E(u001D);
					if (boardSquare != null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.Start()).MethodHandle;
						}
						if (boardSquare.\u0016())
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_coinSpawnPointTracking.Add(new CoinSpawnPointTrackingData(coinSpawnInfo.m_spawnRulePerSquare, boardSquare));
						}
					}
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private void OnDestroy()
	{
		CoinCarnageManager.s_instance = null;
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.OnDrawGizmos()).MethodHandle;
			}
			return;
		}
		if (this.m_coinTurninLocations != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			foreach (BoardRegion boardRegion in this.m_coinTurninLocations)
			{
				boardRegion.Initialize();
				boardRegion.GizmosDrawRegion(this.m_debugTurnInRegionColor);
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	[Server]
	public void OnActorMoved(ActorData mover, BoardSquare movementSquare)
	{
		if (!NetworkServer.active)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.OnActorMoved(ActorData, BoardSquare)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::OnActorMoved(ActorData,BoardSquare)' called on client");
			return;
		}
		using (List<CoinCarnageCoin>.Enumerator enumerator = this.m_coins.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CoinCarnageCoin coinCarnageCoin = enumerator.Current;
				if (!coinCarnageCoin.IsPickedUp())
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (coinCarnageCoin.GetSquare() == movementSquare)
					{
						coinCarnageCoin.PickUp(mover);
						this.AddCoinToActor(mover, 1);
						if (this.m_coinToSpawnPoint.ContainsKey(coinCarnageCoin))
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_coinToSpawnPoint[coinCarnageCoin].TurnOfLastPickup = GameFlowData.Get().CurrentTurn;
						}
						this.CheckTurninAvailability(mover.\u000E());
					}
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	[Server]
	public void OnActorDeath(ActorData actor)
	{
		if (!NetworkServer.active)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.OnActorDeath(ActorData)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::OnActorDeath(ActorData)' called on client");
			return;
		}
		if (!GameplayUtils.IsValidPlayer(actor))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			return;
		}
		int num = this.GetCoinCountForActor(actor) + this.m_coinAdditionalSpawnOnDeath;
		int num2 = 0;
		this.ClearCoinsFromActor(actor);
		BoardSquare boardSquare = actor.\u0012();
		if (boardSquare == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			Debug.LogError("Actor dead, has no board square");
			return;
		}
		HashSet<BoardSquare> coinOccupiedSquares = this.GetCoinOccupiedSquares();
		int i = 0;
		while (i < 6)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (num2 >= num)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					goto IL_130;
				}
			}
			else
			{
				List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(boardSquare, i, true);
				int j = 0;
				while (j < squaresInBorderLayer.Count)
				{
					if (num2 >= num)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							goto IL_10A;
						}
					}
					else
					{
						if (squaresInBorderLayer[j].\u0016() && !coinOccupiedSquares.Contains(squaresInBorderLayer[j]))
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							this.SpawnCoinOnSquare(squaresInBorderLayer[j]);
							num2++;
						}
						j++;
					}
				}
				IL_10A:
				i++;
			}
		}
		IL_130:
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.OnTurnStart()).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::OnTurnStart()' called on client");
			return;
		}
		this.CleanupPickedUpCoins();
		this.SpawnCoinsForTurn();
		this.UpdateDebugUI();
		if (this.m_showDebugCoinCountInNameplate)
		{
		}
	}

	[Server]
	public void OnTurnEnd()
	{
		if (!NetworkServer.active)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.OnTurnEnd()).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::OnTurnEnd()' called on client");
			return;
		}
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(Team.TeamA);
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actor = enumerator.Current;
				this.CheckTurninForActor(actor);
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		allTeamMembers = GameFlowData.Get().GetAllTeamMembers(Team.TeamB);
		using (List<ActorData>.Enumerator enumerator2 = allTeamMembers.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ActorData actor2 = enumerator2.Current;
				this.CheckTurninForActor(actor2);
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.CleanupPickedUpCoins();
		if (this.m_variant == CoinCarnageManager.Variant.PeriodicSingleTurnin)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_turnsUntilTurnInSpawn >= 0)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				this.Networkm_turnsUntilTurnInSpawn = this.m_turnsUntilTurnInSpawn - 1;
			}
			if (this.m_turnsUntilTurnInDeSpawn >= 0)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.Networkm_turnsUntilTurnInDeSpawn = this.m_turnsUntilTurnInDeSpawn - 1;
			}
			if (this.m_turnInAreaPreviewTurns >= 0)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_turnsUntilTurnInSpawn == this.m_turnInAreaPreviewTurns)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.ActivateTurnInArea(Team.Invalid);
				}
			}
			if (this.m_turnsUntilTurnInSpawn == 0)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				this.Networkm_turnsUntilTurnInDeSpawn = this.m_turnInAreaDuration;
			}
			if (this.m_turnsUntilTurnInDeSpawn == 0)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				this.DeactivateTurnInArea(Team.Invalid);
				this.Networkm_turnsUntilTurnInSpawn = this.m_turnInAreaAppearanceDelay;
				this.Networkm_turnsUntilTurnInDeSpawn = -1;
			}
		}
		this.UpdateDebugUI();
	}

	[Server]
	private void CheckTurninForActor(ActorData actor)
	{
		if (!NetworkServer.active)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.CheckTurninForActor(ActorData)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::CheckTurninForActor(ActorData)' called on client");
			return;
		}
		if (this.m_variant == CoinCarnageManager.Variant.PeriodicSingleTurnin)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_turnsUntilTurnInSpawn > 0)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				return;
			}
		}
		int num;
		if (actor.\u000E() == Team.TeamA)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			num = this.m_coinTurninIdxTeamA;
		}
		else
		{
			num = this.m_coinTurninIdxTeamB;
		}
		int num2 = num;
		if (num2 > -1)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (num2 < this.m_coinTurninLocations.Length)
			{
				int coinCountForActor = this.GetCoinCountForActor(actor);
				if (coinCountForActor > 0)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					BoardRegion boardRegion = this.m_coinTurninLocations[num2];
					if (boardRegion.\u001D(actor))
					{
						ObjectivePoints objectivePoints = ObjectivePoints.Get();
						if (objectivePoints != null)
						{
							objectivePoints.AdjustPoints(coinCountForActor, actor.\u000E());
						}
						this.ClearCoinsFromActor(actor);
					}
				}
			}
		}
	}

	[Client]
	private void HookSetTurninRegionLocationA(int newValue)
	{
		if (!NetworkClient.active)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.HookSetTurninRegionLocationA(int)).MethodHandle;
			}
			Debug.LogWarning("[Client] function 'System.Void CoinCarnageManager::HookSetTurninRegionLocationA(System.Int32)' called on server");
			return;
		}
		this.Networkm_coinTurninIdxTeamA = newValue;
		bool preview = this.m_variant == CoinCarnageManager.Variant.PeriodicSingleTurnin;
		this.SpawnTurninRegionVisuals(this.m_coinTurninIdxTeamA, Team.TeamA, preview);
	}

	[Client]
	private void HookSetTurninRegionLocationB(int newValue)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void CoinCarnageManager::HookSetTurninRegionLocationB(System.Int32)' called on server");
			return;
		}
		this.Networkm_coinTurninIdxTeamB = newValue;
		if (this.m_variant != CoinCarnageManager.Variant.PeriodicSingleTurnin)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.HookSetTurninRegionLocationB(int)).MethodHandle;
			}
			this.SpawnTurninRegionVisuals(this.m_coinTurninIdxTeamB, Team.TeamB, false);
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.SpawnTurninRegionVisuals(int, Team, bool)).MethodHandle;
			}
			GameObject obj;
			if (this.m_spawnedTurnInLocationInstances.TryGetValue(team, out obj))
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				UnityEngine.Object.Destroy(obj);
				this.m_spawnedTurnInLocationInstances.Remove(team);
			}
			return;
		}
		if (locationIndex >= 0)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (locationIndex < this.m_coinTurninLocations.Length)
			{
				BoardRegion boardRegion = this.m_coinTurninLocations[locationIndex];
				if (boardRegion != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (GameFlowData.Get() != null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (GameFlowData.Get().activeOwnedActorData != null)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							bool flag = false;
							if (this.m_variant == CoinCarnageManager.Variant.PeriodicSingleTurnin)
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								flag = true;
							}
							else
							{
								flag = (team == GameFlowData.Get().activeOwnedActorData.\u000E());
							}
							GameObject gameObject2;
							if (flag)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								GameObject gameObject;
								if (preview)
								{
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									gameObject = this.m_turninRegionPrefabFriendlyPreview;
								}
								else
								{
									gameObject = this.m_turninRegionPrefabFriendly;
								}
								gameObject2 = gameObject;
							}
							else
							{
								GameObject gameObject3;
								if (preview)
								{
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									gameObject3 = this.m_turninRegionPrefabEnemyPreview;
								}
								else
								{
									gameObject3 = this.m_turninRegionPrefabEnemy;
								}
								gameObject2 = gameObject3;
							}
							if (gameObject2 != null)
							{
								boardRegion.Initialize();
								GameObject gameObject4 = new GameObject();
								List<BoardSquare> list = boardRegion.\u001D();
								using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										BoardSquare boardSquare = enumerator.Current;
										if (boardSquare.WorldBounds != null)
										{
											GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
											gameObject5.transform.position = boardSquare.WorldBounds.Value.center;
											gameObject5.transform.localScale = boardSquare.WorldBounds.Value.extents * 2f;
											gameObject5.transform.parent = gameObject4.transform;
										}
									}
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								if (this.m_spawnedTurnInLocationInstances.ContainsKey(team))
								{
									Log.Error("Turn in location already spawned for " + team, new object[0]);
									this.m_spawnedTurnInLocationInstances[team] = gameObject4;
								}
								else
								{
									this.m_spawnedTurnInLocationInstances.Add(team, gameObject4);
								}
							}
							if (this.m_variant == CoinCarnageManager.Variant.SeparateTurninsPerTeam)
							{
								this.ShowTurninAvailableMessage(flag);
							}
							return;
						}
					}
					Log.Error("Could not find local player to spawn turn-in region.", new object[0]);
				}
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
		if (this.m_variant == CoinCarnageManager.Variant.SeparateTurninsPerTeam)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.ShowTurninAvailableMessage(bool)).MethodHandle;
			}
			string str = text;
			string str2;
			if (friendlyArea)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				str2 = "Your";
			}
			else
			{
				str2 = "Enemy";
			}
			text = str + str2;
		}
		else
		{
			text += "The";
		}
		text += " coin turn-in area has been activated.";
		InterfaceManager.Get().DisplayAlert(text, (!friendlyArea) ? Color.red : Color.cyan, 7f, false, 0);
	}

	[Client]
	private void HookSetTurnsUntilTurnInSpawn(int newValue)
	{
		if (!NetworkClient.active)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.HookSetTurnsUntilTurnInSpawn(int)).MethodHandle;
			}
			Debug.LogWarning("[Client] function 'System.Void CoinCarnageManager::HookSetTurnsUntilTurnInSpawn(System.Int32)' called on server");
			return;
		}
		this.Networkm_turnsUntilTurnInSpawn = newValue;
		if (this.m_turnsUntilTurnInSpawn > 0)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			string alertText = string.Format("[TEMP] {0} turns until turn-in available!", this.m_turnsUntilTurnInSpawn);
			InterfaceManager.Get().DisplayAlert(alertText, Color.cyan, 7f, false, 0);
		}
		if (this.m_turnsUntilTurnInSpawn == 0)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			this.ShowTurninAvailableMessage(true);
			this.SpawnTurninRegionVisuals(-1, Team.TeamA, false);
			this.SpawnTurninRegionVisuals(this.m_coinTurninIdxTeamA, Team.TeamA, false);
		}
	}

	[Client]
	private void HookSetTurnsUntilTurnInDeSpawn(int newValue)
	{
		if (!NetworkClient.active)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.HookSetTurnsUntilTurnInDeSpawn(int)).MethodHandle;
			}
			Debug.LogWarning("[Client] function 'System.Void CoinCarnageManager::HookSetTurnsUntilTurnInDeSpawn(System.Int32)' called on server");
			return;
		}
		this.Networkm_turnsUntilTurnInDeSpawn = newValue;
		if (this.m_turnsUntilTurnInDeSpawn > 0)
		{
			string alertText = string.Format("[TEMP] {0} turns until turn-in is no longer available!", this.m_turnsUntilTurnInDeSpawn);
			InterfaceManager.Get().DisplayAlert(alertText, Color.cyan, 7f, false, 0);
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
		if (this.m_actorToCoinCount.ContainsKey(actor))
		{
			return this.m_actorToCoinCount[actor];
		}
		return 0;
	}

	[Server]
	private void AddCoinToActor(ActorData actor, int amount)
	{
		if (!NetworkServer.active)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.AddCoinToActor(ActorData, int)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::AddCoinToActor(ActorData,System.Int32)' called on client");
			return;
		}
		if (this.m_actorToCoinCount.ContainsKey(actor))
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			Dictionary<ActorData, int> actorToCoinCount;
			(actorToCoinCount = this.m_actorToCoinCount)[actor] = actorToCoinCount[actor] + amount;
		}
		else
		{
			this.m_actorToCoinCount[actor] = amount;
		}
		this.CallRpcActorPickedUpCoin(actor.ActorIndex);
		if (this.m_showDebugCoinCountInNameplate)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private void ClearCoinsFromActor(ActorData actor)
	{
		this.m_actorToCoinCount[actor] = 0;
		if (this.m_showDebugCoinCountInNameplate)
		{
		}
	}

	private void CheckTurninAvailability(Team forTeam)
	{
		int num = 0;
		if (forTeam != Team.TeamA && forTeam != Team.TeamB)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.CheckTurninAvailability(Team)).MethodHandle;
			}
			throw new ArgumentException("Team must be Team A or Team B");
		}
		int num2 = (forTeam != Team.TeamA) ? this.m_coinTurninIdxTeamB : this.m_coinTurninIdxTeamA;
		if (num2 != -1)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			return;
		}
		bool flag = false;
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(forTeam);
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData key = enumerator.Current;
				if (this.m_actorToCoinCount.ContainsKey(key))
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					int num3 = this.m_actorToCoinCount[key];
					num += num3;
					if (this.m_individualTurninUnlockThreshold != -1)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (num3 >= this.m_individualTurninUnlockThreshold)
						{
							goto IL_F5;
						}
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (this.m_teamTurninUnlockThreshold == -1)
					{
						continue;
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (num < this.m_teamTurninUnlockThreshold)
					{
						continue;
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					IL_F5:
					flag = true;
					goto IL_121;
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		IL_121:
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_turnInAreaAppearanceDelay > 0)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			else
			{
				this.ActivateTurnInArea(forTeam);
			}
		}
	}

	private void ActivateTurnInArea(Team forTeam)
	{
		if (this.m_turninLocationChoices.Count == 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.ActivateTurnInArea(Team)).MethodHandle;
			}
			if (this.m_variant == CoinCarnageManager.Variant.PeriodicSingleTurnin)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				this.RefillRandomLocationChoices();
			}
			else if (this.m_variant == CoinCarnageManager.Variant.SeparateTurninsPerTeam)
			{
				if (forTeam == Team.TeamA)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					this.Networkm_coinTurninIdxTeamA = this.m_coinTurninIdxTeamB;
				}
				else
				{
					this.Networkm_coinTurninIdxTeamB = this.m_coinTurninIdxTeamA;
				}
				return;
			}
		}
		if (this.m_turninLocationChoices.Count == 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			Log.Error("No turn-in locations defined in manager!", new object[0]);
			return;
		}
		int index = GameplayRandom.Range(0, this.m_turninLocationChoices.Count);
		int num = this.m_turninLocationChoices[index];
		this.m_turninLocationChoices.RemoveAt(index);
		if (this.m_variant == CoinCarnageManager.Variant.SeparateTurninsPerTeam)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (forTeam == Team.TeamA)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.Networkm_coinTurninIdxTeamA = num;
			}
			else
			{
				this.Networkm_coinTurninIdxTeamB = num;
			}
		}
		else if (this.m_variant == CoinCarnageManager.Variant.PeriodicSingleTurnin)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.Networkm_coinTurninIdxTeamA = num;
			this.Networkm_coinTurninIdxTeamB = num;
		}
	}

	private void DeactivateTurnInArea(Team forTeam)
	{
		this.Networkm_coinTurninIdxTeamA = -1;
		this.Networkm_coinTurninIdxTeamB = -1;
	}

	private void RefillRandomLocationChoices()
	{
		this.m_turninLocationChoices.Clear();
		for (int i = 0; i < this.m_coinTurninLocations.Length; i++)
		{
			this.m_turninLocationChoices.Add(i);
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.RefillRandomLocationChoices()).MethodHandle;
		}
	}

	private void SpawnCoinsForTurn()
	{
		for (int i = 0; i < this.m_coinSpawnPointTracking.Count; i++)
		{
			if (this.m_coinSpawnPointTracking[i].CanSpawnThisTurn(GameFlowData.Get().CurrentTurn))
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.SpawnCoinsForTurn()).MethodHandle;
				}
				CoinCarnageCoin coinCarnageCoin = this.SpawnCoinOnSquare(this.m_coinSpawnPointTracking[i].GetSpawnSquare());
				if (coinCarnageCoin != null)
				{
					this.m_coinSpawnPointTracking[i].NumSpawnedSoFar++;
					this.m_coinSpawnPointTracking[i].TurnOfLastSpawn = GameFlowData.Get().CurrentTurn;
					this.m_coinToSpawnPoint[coinCarnageCoin] = this.m_coinSpawnPointTracking[i];
				}
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private HashSet<BoardSquare> GetCoinOccupiedSquares()
	{
		HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
		using (List<CoinCarnageCoin>.Enumerator enumerator = this.m_coins.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CoinCarnageCoin coinCarnageCoin = enumerator.Current;
				if (!coinCarnageCoin.IsPickedUp())
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.GetCoinOccupiedSquares()).MethodHandle;
					}
					hashSet.Add(coinCarnageCoin.GetSquare());
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return hashSet;
	}

	private CoinCarnageCoin SpawnCoinOnSquare(BoardSquare square)
	{
		HashSet<BoardSquare> coinOccupiedSquares = this.GetCoinOccupiedSquares();
		if (!(square == null))
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.SpawnCoinOnSquare(BoardSquare)).MethodHandle;
			}
			if (square.\u0016())
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!coinOccupiedSquares.Contains(square))
				{
					return this.CreateCoinObjectOnSquare(square);
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return null;
	}

	private CoinCarnageCoin CreateCoinObjectOnSquare(BoardSquare square)
	{
		if (!NetworkServer.active)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.CreateCoinObjectOnSquare(BoardSquare)).MethodHandle;
			}
			Log.Error("Attempted to call server function without server", new object[0]);
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_coinPrefab, square.ToVector3(), Quaternion.identity);
		CoinCarnageCoin component = gameObject.GetComponent<CoinCarnageCoin>();
		component.Initialize(square);
		NetworkServer.Spawn(gameObject);
		this.m_coins.Add(component);
		return component;
	}

	private void CleanupPickedUpCoins()
	{
		for (int i = this.m_coins.Count - 1; i >= 0; i--)
		{
			if (this.m_coins[i].IsPickedUp())
			{
				this.m_coinToSpawnPoint.Remove(this.m_coins[i]);
				this.m_coins[i].Destroy();
				this.m_coins.RemoveAt(i);
			}
			else if (this.m_coins[i] == null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.CleanupPickedUpCoins()).MethodHandle;
				}
				this.m_coins.RemoveAt(i);
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private string GetCoinCountString(ActorData actor)
	{
		return "coin count: " + this.GetCoinCountForActor(actor).ToString();
	}

	private void UpdateDebugUI()
	{
		if (!(this.m_debugTextLeft == null))
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.UpdateDebugUI()).MethodHandle;
			}
			if (!(this.m_debugTextRight == null) && this.m_showDebugUI)
			{
				string text = string.Empty;
				string text2 = string.Empty;
				int num = 0;
				using (List<CoinCarnageCoin>.Enumerator enumerator = this.m_coins.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CoinCarnageCoin coinCarnageCoin = enumerator.Current;
						if (!coinCarnageCoin.IsPickedUp())
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							num++;
						}
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				this.m_debugTextLeft.text = string.Concat(new object[]
				{
					"Coin count [active/total tracked] = ",
					num,
					" / ",
					this.m_coins.Count.ToString(),
					"\n"
				});
				this.m_debugTextRight.text = "\n";
				using (Dictionary<ActorData, int>.Enumerator enumerator2 = this.m_actorToCoinCount.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<ActorData, int> keyValuePair = enumerator2.Current;
						ActorData key = keyValuePair.Key;
						int value = keyValuePair.Value;
						if (key.\u000E() == Team.TeamA)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							string text3 = text;
							text = string.Concat(new string[]
							{
								text3,
								key.DisplayName,
								" -> ",
								value.ToString(),
								"\n"
							});
						}
						else if (key.\u000E() == Team.TeamB)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							string text3 = text2;
							text2 = string.Concat(new string[]
							{
								text3,
								key.DisplayName,
								" -> ",
								value.ToString(),
								"\n"
							});
						}
					}
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				Text debugTextLeft = this.m_debugTextLeft;
				debugTextLeft.text += text;
				Text debugTextRight = this.m_debugTextRight;
				debugTextRight.text += text2;
				return;
			}
		}
	}

	private void UNetVersion()
	{
	}

	public int Networkm_coinTurninIdxTeamA
	{
		get
		{
			return this.m_coinTurninIdxTeamA;
		}
		[param: In]
		set
		{
			uint dirtyBit = 1U;
			if (NetworkServer.localClientActive && !base.syncVarHookGuard)
			{
				base.syncVarHookGuard = true;
				this.HookSetTurninRegionLocationA(value);
				base.syncVarHookGuard = false;
			}
			base.SetSyncVar<int>(value, ref this.m_coinTurninIdxTeamA, dirtyBit);
		}
	}

	public int Networkm_coinTurninIdxTeamB
	{
		get
		{
			return this.m_coinTurninIdxTeamB;
		}
		[param: In]
		set
		{
			uint dirtyBit = 2U;
			if (NetworkServer.localClientActive)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.set_Networkm_coinTurninIdxTeamB(int)).MethodHandle;
				}
				if (!base.syncVarHookGuard)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					base.syncVarHookGuard = true;
					this.HookSetTurninRegionLocationB(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.m_coinTurninIdxTeamB, dirtyBit);
		}
	}

	public int Networkm_turnsUntilTurnInSpawn
	{
		get
		{
			return this.m_turnsUntilTurnInSpawn;
		}
		[param: In]
		set
		{
			uint dirtyBit = 4U;
			if (NetworkServer.localClientActive && !base.syncVarHookGuard)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.set_Networkm_turnsUntilTurnInSpawn(int)).MethodHandle;
				}
				base.syncVarHookGuard = true;
				this.HookSetTurnsUntilTurnInSpawn(value);
				base.syncVarHookGuard = false;
			}
			base.SetSyncVar<int>(value, ref this.m_turnsUntilTurnInSpawn, dirtyBit);
		}
	}

	public int Networkm_turnsUntilTurnInDeSpawn
	{
		get
		{
			return this.m_turnsUntilTurnInDeSpawn;
		}
		[param: In]
		set
		{
			uint dirtyBit = 8U;
			if (NetworkServer.localClientActive)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.set_Networkm_turnsUntilTurnInDeSpawn(int)).MethodHandle;
				}
				if (!base.syncVarHookGuard)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					base.syncVarHookGuard = true;
					this.HookSetTurnsUntilTurnInDeSpawn(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.m_turnsUntilTurnInDeSpawn, dirtyBit);
		}
	}

	protected static void InvokeRpcRpcActorPickedUpCoin(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcActorPickedUpCoin called on server.");
			return;
		}
		((CoinCarnageManager)obj).RpcActorPickedUpCoin((int)reader.ReadPackedUInt32());
	}

	public void CallRpcActorPickedUpCoin(int actorIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcActorPickedUpCoin called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)CoinCarnageManager.kRpcRpcActorPickedUpCoin);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndex);
		this.SendRPCInternal(networkWriter, 0, "RpcActorPickedUpCoin");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)this.m_coinTurninIdxTeamA);
			writer.WritePackedUInt32((uint)this.m_coinTurninIdxTeamB);
			writer.WritePackedUInt32((uint)this.m_turnsUntilTurnInSpawn);
			writer.WritePackedUInt32((uint)this.m_turnsUntilTurnInDeSpawn);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			if (!flag)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_coinTurninIdxTeamA);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_coinTurninIdxTeamB);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_turnsUntilTurnInSpawn);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_turnsUntilTurnInDeSpawn);
		}
		if (!flag)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageManager.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_coinTurninIdxTeamA = (int)reader.ReadPackedUInt32();
			this.m_coinTurninIdxTeamB = (int)reader.ReadPackedUInt32();
			this.m_turnsUntilTurnInSpawn = (int)reader.ReadPackedUInt32();
			this.m_turnsUntilTurnInDeSpawn = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.HookSetTurninRegionLocationA((int)reader.ReadPackedUInt32());
		}
		if ((num & 2) != 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			this.HookSetTurninRegionLocationB((int)reader.ReadPackedUInt32());
		}
		if ((num & 4) != 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.HookSetTurnsUntilTurnInSpawn((int)reader.ReadPackedUInt32());
		}
		if ((num & 8) != 0)
		{
			this.HookSetTurnsUntilTurnInDeSpawn((int)reader.ReadPackedUInt32());
		}
	}

	public enum Variant
	{
		PeriodicSingleTurnin,
		SeparateTurninsPerTeam
	}
}
