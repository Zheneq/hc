using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class PowerUpSpawner : NetworkBehaviour, PowerUp.IPowerUpListener, IGameEventListener
{
	private BoardSquare m_boardSquare;

	[Separator("Default Prefabs", true)]
	public PowerUp m_powerUpPrefab;

	public GameObject m_baseSequencePrefab;

	public GameObject m_spawnSequencePrefab;

	[Separator("Additional Prefabs for mixing up powerup to spawn", true)]
	public PowerUpSpawner.ExtraPowerupSelectMode m_extraPowerupSelectMode;

	public List<PowerUpSpawner.PowerupSpawnInfo> m_extraPowerupsForMixedSpawn;

	public bool m_useSameFirstPowerupIfRandom = true;

	[Separator("Timing of spawns", true)]
	public int m_spawnInterval;

	public int m_initialSpawnDelay;

	public Team m_teamRestriction;

	public string[] m_tagsToApplyToPowerup;

	private List<PowerUpSpawner.PowerupSpawnInfo> m_finalizedPowerupSpawnInfoList;

	[SyncVar]
	private uint m_sequenceSourceId;

	private Sequence[] m_baseSequences;

	private Sequence[] m_spawnSequences;

	private int m_lastServerSpawnPrefabIndex = -1;

	private int m_currentBasePrefabIndex = -1;

	[SyncVar]
	private int m_nextPowerupPrefabIndex;

	[SyncVar(hook = "HookNextSpawnTurn")]
	private int m_nextSpawnTurn;

	private PowerUp m_powerUpInstance;

	[SyncVar]
	private bool m_spawningEnabled = true;

	private bool m_initialized;

	private bool m_isReady = true;

	public BoardSquare boardSquare
	{
		get
		{
			return this.m_boardSquare;
		}
	}

	private int ChooseNextPrefabSpawnIndex(bool isForFirstSpawn = false)
	{
		int result = 0;
		if (this.m_extraPowerupsForMixedSpawn != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.ChooseNextPrefabSpawnIndex(bool)).MethodHandle;
			}
			int count = this.m_finalizedPowerupSpawnInfoList.Count;
			if (this.m_extraPowerupSelectMode == PowerUpSpawner.ExtraPowerupSelectMode.InOrder)
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
				if (this.m_lastServerSpawnPrefabIndex >= 0)
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
					result = (this.m_lastServerSpawnPrefabIndex + 1) % count;
				}
				else
				{
					result = 0;
				}
			}
			else if (this.m_extraPowerupSelectMode == PowerUpSpawner.ExtraPowerupSelectMode.Random)
			{
				if (isForFirstSpawn)
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
					if (this.m_useSameFirstPowerupIfRandom)
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
						return 0;
					}
				}
				result = UnityEngine.Random.Range(0, count);
			}
		}
		return result;
	}

	private void HookNextSpawnTurn(int nextSpawnTurn)
	{
		bool flag = this.m_nextSpawnTurn != nextSpawnTurn;
		this.Networkm_nextSpawnTurn = nextSpawnTurn;
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.HookNextSpawnTurn(int)).MethodHandle;
			}
			this.UpdateTimerController();
		}
	}

	public override void OnStartServer()
	{
		SequenceSource sequenceSource = new SequenceSource(null, null, false, null, null);
		this.Networkm_sequenceSourceId = sequenceSource.RootID;
		this.Networkm_nextSpawnTurn = GameFlowData.Get().CurrentTurn + this.m_initialSpawnDelay + 1;
	}

	public void Awake()
	{
		this.m_finalizedPowerupSpawnInfoList = new List<PowerUpSpawner.PowerupSpawnInfo>();
		PowerUpSpawner.PowerupSpawnInfo powerupSpawnInfo = new PowerUpSpawner.PowerupSpawnInfo();
		powerupSpawnInfo.m_powerupObjectPrefab = this.m_powerUpPrefab;
		powerupSpawnInfo.m_baseSeqPrefab = this.m_baseSequencePrefab;
		powerupSpawnInfo.m_spawnSeqPrefab = this.m_spawnSequencePrefab;
		this.m_finalizedPowerupSpawnInfoList.Add(powerupSpawnInfo);
		if (this.m_extraPowerupsForMixedSpawn != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.Awake()).MethodHandle;
			}
			if (this.m_extraPowerupsForMixedSpawn.Count > 0)
			{
				for (int i = 0; i < this.m_extraPowerupsForMixedSpawn.Count; i++)
				{
					PowerUpSpawner.PowerupSpawnInfo powerupSpawnInfo2 = this.m_extraPowerupsForMixedSpawn[i];
					if (powerupSpawnInfo2.m_powerupObjectPrefab != null)
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
						if (powerupSpawnInfo2.m_baseSeqPrefab != null)
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
							this.m_finalizedPowerupSpawnInfoList.Add(powerupSpawnInfo2);
						}
					}
				}
			}
		}
		if (NetworkServer.active)
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
			this.Networkm_nextPowerupPrefabIndex = this.ChooseNextPrefabSpawnIndex(true);
		}
	}

	public void Start()
	{
		if (NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.Start()).MethodHandle;
			}
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReplayRestart);
		}
	}

	private void Initialize()
	{
		if (!this.m_initialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.Initialize()).MethodHandle;
			}
			this.m_initialized = true;
			PowerUpManager.AddListenerStatic(this);
			this.m_boardSquare = Board.Get().GetBoardSquareSafe(base.transform.position.x, base.transform.position.z);
			this.PlayBaseSequence();
			base.transform.parent = PowerUpManager.Get().GetSpawnerRoot().transform;
		}
	}

	public void OnDestroy()
	{
		if (PowerUpManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.OnDestroy()).MethodHandle;
			}
			PowerUpManager.Get().RemoveListener(this);
		}
		if (NetworkClient.active)
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
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReplayRestart);
		}
	}

	private void PlayBaseSequence()
	{
		GameObject baseSeqPrefab = this.m_finalizedPowerupSpawnInfoList[0].m_baseSeqPrefab;
		if (this.m_nextPowerupPrefabIndex >= 0 && this.m_nextPowerupPrefabIndex < this.m_finalizedPowerupSpawnInfoList.Count)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.PlayBaseSequence()).MethodHandle;
			}
			baseSeqPrefab = this.m_finalizedPowerupSpawnInfoList[this.m_nextPowerupPrefabIndex].m_baseSeqPrefab;
		}
		if (baseSeqPrefab != null)
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
			SequenceSource source = new SequenceSource(null, null, this.m_sequenceSourceId, false);
			this.m_baseSequences = SequenceManager.Get().CreateClientSequences(baseSeqPrefab, this.m_boardSquare, null, null, source, null);
			this.SetSequencesRoot(this.m_baseSequences);
			this.UpdateTimerController();
		}
	}

	private void PlaySpawnSequence()
	{
		GameObject spawnSeqPrefab = this.m_finalizedPowerupSpawnInfoList[0].m_spawnSeqPrefab;
		if (this.m_nextPowerupPrefabIndex < this.m_finalizedPowerupSpawnInfoList.Count)
		{
			spawnSeqPrefab = this.m_finalizedPowerupSpawnInfoList[this.m_nextPowerupPrefabIndex].m_spawnSeqPrefab;
		}
		if (spawnSeqPrefab != null)
		{
			SequenceSource source = new SequenceSource(null, null, this.m_sequenceSourceId, false);
			this.m_spawnSequences = SequenceManager.Get().CreateClientSequences(spawnSeqPrefab, this.m_boardSquare, null, null, source, null);
		}
	}

	private void SetSequencesRoot(Sequence[] sequences)
	{
		if (sequences != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.SetSequencesRoot(Sequence[])).MethodHandle;
			}
			if (PowerUpManager.Get() != null)
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
				foreach (Sequence sequence in sequences)
				{
					if (sequence != null)
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
						sequence.transform.parent = PowerUpManager.Get().GetSpawnedPersistentSequencesRoot().transform;
						return;
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
	}

	private void UpdateTimerController()
	{
		if (this.m_baseSequences != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.UpdateTimerController()).MethodHandle;
			}
			for (int i = 0; i < this.m_baseSequences.Length; i++)
			{
				if (this.IsEnabled)
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
					this.m_baseSequences[i].SetTimerController(this.m_nextSpawnTurn - GameFlowData.Get().CurrentTurn);
				}
				else
				{
					this.m_baseSequences[i].SetTimerController(5);
				}
			}
		}
	}

	private void Update()
	{
		this.UpdateTimerController();
		if (!this.m_initialized)
		{
			if (!(VisualsLoader.Get() == null))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.Update()).MethodHandle;
				}
				if (!VisualsLoader.Get().LevelLoaded())
				{
					goto IL_50;
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
			}
			this.Initialize();
		}
		IL_50:
		if (this.m_initialized && NetworkClient.active)
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
			if (this.m_nextPowerupPrefabIndex >= 0 && this.m_currentBasePrefabIndex != this.m_nextPowerupPrefabIndex)
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
				this.ClearPreviousBaseSpawnSequences();
				this.PlayBaseSequence();
				this.m_currentBasePrefabIndex = this.m_nextPowerupPrefabIndex;
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.OnDrawGizmos()).MethodHandle;
			}
			return;
		}
		Gizmos.DrawIcon(base.transform.position, "icon_PowerUp.png");
	}

	void PowerUp.IPowerUpListener.OnPowerUpDestroyed(PowerUp destroyedPowerUp)
	{
		this.Networkm_nextSpawnTurn = GameFlowData.Get().CurrentTurn + this.m_spawnInterval;
		if (GameplayMutators.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.PowerUp.IPowerUpListener.OnPowerUpDestroyed(PowerUp)).MethodHandle;
			}
			this.Networkm_nextSpawnTurn = Mathf.Max(this.m_nextSpawnTurn - GameplayMutators.GetPowerupRefreshSpeedAdjustment(), GameFlowData.Get().CurrentTurn + 1);
		}
		this.Networkm_nextPowerupPrefabIndex = this.ChooseNextPrefabSpawnIndex(false);
		this.UpdateTimerController();
		this.m_powerUpInstance = null;
	}

	PowerUp[] PowerUp.IPowerUpListener.GetActivePowerUps()
	{
		return new PowerUp[]
		{
			this.m_powerUpInstance
		};
	}

	void PowerUp.IPowerUpListener.SetSpawningEnabled(bool enabled)
	{
		this.Networkm_spawningEnabled = enabled;
	}

	public bool IsEnabled
	{
		get
		{
			bool result;
			if (this.m_spawningEnabled)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.get_IsEnabled()).MethodHandle;
				}
				result = this.m_isReady;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	public void SetReadyState(bool value)
	{
		this.m_isReady = value;
	}

	private void ClearPreviousSpawnSequences()
	{
		if (this.m_spawnSequences != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.ClearPreviousSpawnSequences()).MethodHandle;
			}
			foreach (Sequence sequence in this.m_spawnSequences)
			{
				sequence.MarkForRemoval();
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
			this.m_spawnSequences = null;
		}
	}

	private void ClearPreviousBaseSpawnSequences()
	{
		if (this.m_baseSequences != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.ClearPreviousBaseSpawnSequences()).MethodHandle;
			}
			foreach (Sequence sequence in this.m_baseSequences)
			{
				sequence.MarkForRemoval();
			}
			this.m_baseSequences = null;
		}
	}

	void PowerUp.IPowerUpListener.OnTurnTick()
	{
		this.UpdateTimerController();
		this.ClearPreviousSpawnSequences();
		if (this.m_nextSpawnTurn == GameFlowData.Get().CurrentTurn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.PowerUp.IPowerUpListener.OnTurnTick()).MethodHandle;
			}
			if (this.IsEnabled)
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
				this.PlaySpawnSequence();
			}
		}
		if (NetworkServer.active)
		{
			if (this.m_powerUpInstance == null)
			{
				if (this.m_nextSpawnTurn <= GameFlowData.Get().CurrentTurn && this.IsEnabled)
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
					this.SpawnPowerUp();
				}
			}
			else
			{
				this.m_powerUpInstance.OnTurnTick();
			}
		}
		if (this.m_nextPowerupPrefabIndex >= 0 && this.m_currentBasePrefabIndex != this.m_nextPowerupPrefabIndex)
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
			this.ClearPreviousBaseSpawnSequences();
			this.PlayBaseSequence();
			this.m_currentBasePrefabIndex = this.m_nextPowerupPrefabIndex;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.ReplayRestart)
		{
			this.PlayBaseSequence();
			this.m_spawnSequences = null;
		}
	}

	bool PowerUp.IPowerUpListener.IsPowerUpSpawnPoint(BoardSquare square)
	{
		return square == this.m_boardSquare;
	}

	[Server]
	private void SpawnPowerUp()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.SpawnPowerUp()).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void PowerUpSpawner::SpawnPowerUp()' called on client");
			return;
		}
		PowerUp powerupObjectPrefab = this.m_finalizedPowerupSpawnInfoList[0].m_powerupObjectPrefab;
		this.m_lastServerSpawnPrefabIndex = 0;
		if (this.m_nextPowerupPrefabIndex < this.m_finalizedPowerupSpawnInfoList.Count)
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
			powerupObjectPrefab = this.m_finalizedPowerupSpawnInfoList[this.m_nextPowerupPrefabIndex].m_powerupObjectPrefab;
			this.m_lastServerSpawnPrefabIndex = this.m_nextPowerupPrefabIndex;
		}
		else
		{
			Debug.LogError("Powerup Spawn Index is larger than number of prefabs to choose from");
		}
		if (powerupObjectPrefab)
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
			Vector3 position = this.boardSquare.ToVector3();
			this.m_powerUpInstance = UnityEngine.Object.Instantiate<PowerUp>(powerupObjectPrefab, position, Quaternion.identity);
			this.m_powerUpInstance.SetPickupTeam(this.m_teamRestriction);
			GameObject gameObject = this.m_powerUpInstance.gameObject;
			this.m_powerUpInstance.powerUpListener = this;
			NetworkServer.Spawn(gameObject);
			this.m_powerUpInstance.CalculateBoardSquare();
			this.m_powerUpInstance.CheckForPickupOnSpawn();
			if (this.m_tagsToApplyToPowerup != null)
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
				foreach (string powerupTag in this.m_tagsToApplyToPowerup)
				{
					this.m_powerUpInstance.AddTag(powerupTag);
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
		}
	}

	public void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
	}

	private void UNetVersion()
	{
	}

	public uint Networkm_sequenceSourceId
	{
		get
		{
			return this.m_sequenceSourceId;
		}
		[param: In]
		set
		{
			base.SetSyncVar<uint>(value, ref this.m_sequenceSourceId, 1U);
		}
	}

	public int Networkm_nextPowerupPrefabIndex
	{
		get
		{
			return this.m_nextPowerupPrefabIndex;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_nextPowerupPrefabIndex, 2U);
		}
	}

	public int Networkm_nextSpawnTurn
	{
		get
		{
			return this.m_nextSpawnTurn;
		}
		[param: In]
		set
		{
			uint dirtyBit = 4U;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.set_Networkm_nextSpawnTurn(int)).MethodHandle;
				}
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookNextSpawnTurn(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.m_nextSpawnTurn, dirtyBit);
		}
	}

	public bool Networkm_spawningEnabled
	{
		get
		{
			return this.m_spawningEnabled;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_spawningEnabled, 8U);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32(this.m_sequenceSourceId);
			writer.WritePackedUInt32((uint)this.m_nextPowerupPrefabIndex);
			writer.WritePackedUInt32((uint)this.m_nextSpawnTurn);
			writer.Write(this.m_spawningEnabled);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.OnSerialize(NetworkWriter, bool)).MethodHandle;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32(this.m_sequenceSourceId);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_nextPowerupPrefabIndex);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
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
			writer.WritePackedUInt32((uint)this.m_nextSpawnTurn);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
		{
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
			writer.Write(this.m_spawningEnabled);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawner.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_sequenceSourceId = reader.ReadPackedUInt32();
			this.m_nextPowerupPrefabIndex = (int)reader.ReadPackedUInt32();
			this.m_nextSpawnTurn = (int)reader.ReadPackedUInt32();
			this.m_spawningEnabled = reader.ReadBoolean();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_sequenceSourceId = reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
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
			this.m_nextPowerupPrefabIndex = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
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
			this.HookNextSpawnTurn((int)reader.ReadPackedUInt32());
		}
		if ((num & 8) != 0)
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
			this.m_spawningEnabled = reader.ReadBoolean();
		}
	}

	[Serializable]
	public class PowerupSpawnInfo
	{
		public PowerUp m_powerupObjectPrefab;

		public GameObject m_baseSeqPrefab;

		public GameObject m_spawnSeqPrefab;
	}

	public enum ExtraPowerupSelectMode
	{
		InOrder,
		Random
	}
}
