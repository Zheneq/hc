using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class PowerUpSpawner : NetworkBehaviour, PowerUp.IPowerUpListener, IGameEventListener
{
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

	private BoardSquare m_boardSquare;

	[Separator("Default Prefabs")]
	public PowerUp m_powerUpPrefab;
	public GameObject m_baseSequencePrefab;
	public GameObject m_spawnSequencePrefab;

	[Separator("Additional Prefabs for mixing up powerup to spawn")]
	public ExtraPowerupSelectMode m_extraPowerupSelectMode;
	public List<PowerupSpawnInfo> m_extraPowerupsForMixedSpawn;
	public bool m_useSameFirstPowerupIfRandom = true;

	[Separator("Timing of spawns")]
	public int m_spawnInterval;
	public int m_initialSpawnDelay;
	public Team m_teamRestriction;
	public string[] m_tagsToApplyToPowerup;

	private List<PowerupSpawnInfo> m_finalizedPowerupSpawnInfoList;
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

	public BoardSquare boardSquare => m_boardSquare;

	public bool IsEnabled => m_spawningEnabled && m_isReady;

	public uint Networkm_sequenceSourceId
	{
		get => m_sequenceSourceId;
		[param: In]
		set => SetSyncVar(value, ref m_sequenceSourceId, 1u);
	}

	public int Networkm_nextPowerupPrefabIndex
	{
		get => m_nextPowerupPrefabIndex;
		[param: In]
		set => SetSyncVar(value, ref m_nextPowerupPrefabIndex, 2u);
	}

	public int Networkm_nextSpawnTurn
	{
		get => m_nextSpawnTurn;
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookNextSpawnTurn(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_nextSpawnTurn, 4u);
		}
	}

	public bool Networkm_spawningEnabled
	{
		get => m_spawningEnabled;
		[param: In]
		set => SetSyncVar(value, ref m_spawningEnabled, 8u);
	}

	private int ChooseNextPrefabSpawnIndex(bool isForFirstSpawn = false)
	{
		if (m_extraPowerupsForMixedSpawn == null)
		{
			return 0;
		}
		if (m_extraPowerupSelectMode == ExtraPowerupSelectMode.InOrder)
		{
			if (m_lastServerSpawnPrefabIndex >= 0)
			{
				return (m_lastServerSpawnPrefabIndex + 1) % m_finalizedPowerupSpawnInfoList.Count;
			}
		}
		else if (m_extraPowerupSelectMode == ExtraPowerupSelectMode.Random)
		{
			if (!isForFirstSpawn || !m_useSameFirstPowerupIfRandom)
			{
				return Random.Range(0, m_finalizedPowerupSpawnInfoList.Count);
			}
		}
		return 0;
	}

	private void HookNextSpawnTurn(int nextSpawnTurn)
	{
		bool isChange = m_nextSpawnTurn != nextSpawnTurn;
		Networkm_nextSpawnTurn = nextSpawnTurn;
		if (isChange)
		{
			UpdateTimerController();
		}
	}

	public override void OnStartServer()
	{
		SequenceSource sequenceSource = new SequenceSource(null, null, false);
		Networkm_sequenceSourceId = sequenceSource.RootID;
		Networkm_nextSpawnTurn = GameFlowData.Get().CurrentTurn + m_initialSpawnDelay + 1;
	}

	public void Awake()
	{
		m_finalizedPowerupSpawnInfoList = new List<PowerupSpawnInfo>();
		PowerupSpawnInfo powerupSpawnInfo = new PowerupSpawnInfo
		{
			m_powerupObjectPrefab = m_powerUpPrefab,
			m_baseSeqPrefab = m_baseSequencePrefab,
			m_spawnSeqPrefab = m_spawnSequencePrefab
		};
		m_finalizedPowerupSpawnInfoList.Add(powerupSpawnInfo);
		if (m_extraPowerupsForMixedSpawn != null && m_extraPowerupsForMixedSpawn.Count > 0)
		{
			foreach (PowerupSpawnInfo extraPowerupSpawnInfo in m_extraPowerupsForMixedSpawn)
			{
				if (extraPowerupSpawnInfo.m_powerupObjectPrefab != null
					&& extraPowerupSpawnInfo.m_baseSeqPrefab != null)
				{
					m_finalizedPowerupSpawnInfoList.Add(extraPowerupSpawnInfo);
				}
			}
		}
		if (NetworkServer.active)
		{
			Networkm_nextPowerupPrefabIndex = ChooseNextPrefabSpawnIndex(true);
		}
	}

	public void Start()
	{
		if (NetworkClient.active)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReplayRestart);
		}
	}

	private void Initialize()
	{
		if (!m_initialized)
		{
			m_initialized = true;
			PowerUpManager.AddListenerStatic(this);
			m_boardSquare = Board.Get().GetSquareFromPos(transform.position.x, transform.position.z);
			PlayBaseSequence();
			transform.parent = PowerUpManager.Get().GetSpawnerRoot().transform;
		}
	}

	public void OnDestroy()
	{
		if (PowerUpManager.Get() != null)
		{
			PowerUpManager.Get().RemoveListener(this);
		}
		if (NetworkClient.active)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReplayRestart);
		}
	}

	private void PlayBaseSequence()
	{
		GameObject baseSeqPrefab = m_finalizedPowerupSpawnInfoList[0].m_baseSeqPrefab;
		if (m_nextPowerupPrefabIndex >= 0 && m_nextPowerupPrefabIndex < m_finalizedPowerupSpawnInfoList.Count)
		{
			baseSeqPrefab = m_finalizedPowerupSpawnInfoList[m_nextPowerupPrefabIndex].m_baseSeqPrefab;
		}
		if (baseSeqPrefab != null)
		{
			SequenceSource source = new SequenceSource(null, null, m_sequenceSourceId, false);
			m_baseSequences = SequenceManager.Get().CreateClientSequences(baseSeqPrefab, m_boardSquare, null, null, source, null);
			SetSequencesRoot(m_baseSequences);
			UpdateTimerController();
		}
	}

	private void PlaySpawnSequence()
	{
		GameObject spawnSeqPrefab = m_finalizedPowerupSpawnInfoList[0].m_spawnSeqPrefab;
		if (m_nextPowerupPrefabIndex < m_finalizedPowerupSpawnInfoList.Count)
		{
			spawnSeqPrefab = m_finalizedPowerupSpawnInfoList[m_nextPowerupPrefabIndex].m_spawnSeqPrefab;
		}
		if (spawnSeqPrefab != null)
		{
			SequenceSource source = new SequenceSource(null, null, m_sequenceSourceId, false);
			m_spawnSequences = SequenceManager.Get().CreateClientSequences(spawnSeqPrefab, m_boardSquare, null, null, source, null);
		}
	}

	private void SetSequencesRoot(Sequence[] sequences)
	{
		if (sequences != null && PowerUpManager.Get() != null)
		{
			foreach (Sequence sequence in sequences)
			{
				if (sequence != null)
				{
					sequence.transform.parent = PowerUpManager.Get().GetSpawnedPersistentSequencesRoot().transform;
					return;
				}
			}
		}
	}

	private void UpdateTimerController()
	{
		if (m_baseSequences != null)
		{
			foreach (Sequence sequence in m_baseSequences)
			{
				if (IsEnabled)
				{
					sequence.SetTimerController(m_nextSpawnTurn - GameFlowData.Get().CurrentTurn);
				}
				else
				{
					sequence.SetTimerController(5);
				}
			}
		}
	}

	private void Update()
	{
		UpdateTimerController();
		if (!m_initialized && (VisualsLoader.Get() == null || VisualsLoader.Get().LevelLoaded()))
		{
			Initialize();
		}
		if (m_initialized
			&& NetworkClient.active
			&& m_nextPowerupPrefabIndex >= 0
			&& m_currentBasePrefabIndex != m_nextPowerupPrefabIndex)
		{
			ClearPreviousBaseSpawnSequences();
			PlayBaseSequence();
			m_currentBasePrefabIndex = m_nextPowerupPrefabIndex;
		}
	}

	private void OnDrawGizmos()
	{
		if (CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			Gizmos.DrawIcon(transform.position, "icon_PowerUp.png");
		}
	}

	void PowerUp.IPowerUpListener.OnPowerUpDestroyed(PowerUp destroyedPowerUp)
	{
		Networkm_nextSpawnTurn = GameFlowData.Get().CurrentTurn + m_spawnInterval;
		if (GameplayMutators.Get() != null)
		{
			Networkm_nextSpawnTurn = Mathf.Max(
				m_nextSpawnTurn - GameplayMutators.GetPowerupRefreshSpeedAdjustment(), 
				GameFlowData.Get().CurrentTurn + 1);
		}
		Networkm_nextPowerupPrefabIndex = ChooseNextPrefabSpawnIndex();
		UpdateTimerController();
		m_powerUpInstance = null;
	}

	PowerUp[] PowerUp.IPowerUpListener.GetActivePowerUps()
	{
		return new[]
		{
			m_powerUpInstance
		};
	}

	void PowerUp.IPowerUpListener.SetSpawningEnabled(bool enabled)
	{
		Networkm_spawningEnabled = enabled;
	}

	public void SetReadyState(bool value)
	{
		m_isReady = value;
	}

	private void ClearPreviousSpawnSequences()
	{
		if (m_spawnSequences != null)
		{
			Sequence[] spawnSequences = m_spawnSequences;
			foreach (Sequence sequence in spawnSequences)
			{
				sequence.MarkForRemoval();
			}
			m_spawnSequences = null;
		}
	}

	private void ClearPreviousBaseSpawnSequences()
	{
		if (m_baseSequences != null)
		{
			Sequence[] baseSequences = m_baseSequences;
			foreach (Sequence sequence in baseSequences)
			{
				sequence.MarkForRemoval();
			}
			m_baseSequences = null;
		}
	}

	void PowerUp.IPowerUpListener.OnTurnTick()
	{
		UpdateTimerController();
		ClearPreviousSpawnSequences();
		if (m_nextSpawnTurn == GameFlowData.Get().CurrentTurn && IsEnabled)
		{
			PlaySpawnSequence();
		}
		if (NetworkServer.active)
		{
			if (m_powerUpInstance == null)
			{
				if (m_nextSpawnTurn <= GameFlowData.Get().CurrentTurn && IsEnabled)
				{
					SpawnPowerUp();
				}
			}
			else
			{
				m_powerUpInstance.OnTurnTick();
			}
		}
		if (m_nextPowerupPrefabIndex >= 0 && m_currentBasePrefabIndex != m_nextPowerupPrefabIndex)
		{
			ClearPreviousBaseSpawnSequences();
			PlayBaseSequence();
			m_currentBasePrefabIndex = m_nextPowerupPrefabIndex;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.ReplayRestart)
		{
			PlayBaseSequence();
			m_spawnSequences = null;
		}
	}

	bool PowerUp.IPowerUpListener.IsPowerUpSpawnPoint(BoardSquare square)
	{
		return square == m_boardSquare;
	}

	[Server]
	private void SpawnPowerUp()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PowerUpSpawner::SpawnPowerUp()' called on client");
			return;
		}
		PowerUp powerupObjectPrefab = m_finalizedPowerupSpawnInfoList[0].m_powerupObjectPrefab;
		m_lastServerSpawnPrefabIndex = 0;
		if (m_nextPowerupPrefabIndex < m_finalizedPowerupSpawnInfoList.Count)
		{
			powerupObjectPrefab = m_finalizedPowerupSpawnInfoList[m_nextPowerupPrefabIndex].m_powerupObjectPrefab;
			m_lastServerSpawnPrefabIndex = m_nextPowerupPrefabIndex;
		}
		else
		{
			Debug.LogError("Powerup Spawn Index is larger than number of prefabs to choose from");
		}
		if (powerupObjectPrefab)
		{
			m_powerUpInstance = Instantiate(powerupObjectPrefab, boardSquare.ToVector3(), Quaternion.identity);
			m_powerUpInstance.SetPickupTeam(m_teamRestriction);
			m_powerUpInstance.powerUpListener = this;
			NetworkServer.Spawn(m_powerUpInstance.gameObject);
			m_powerUpInstance.CalculateBoardSquare();
			m_powerUpInstance.CheckForPickupOnSpawn();
			if (m_tagsToApplyToPowerup != null)
			{
				foreach (string powerupTag in m_tagsToApplyToPowerup)
				{
					m_powerUpInstance.AddTag(powerupTag);
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

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32(m_sequenceSourceId);
			writer.WritePackedUInt32((uint)m_nextPowerupPrefabIndex);
			writer.WritePackedUInt32((uint)m_nextSpawnTurn);
			writer.Write(m_spawningEnabled);
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
			writer.WritePackedUInt32(m_sequenceSourceId);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_nextPowerupPrefabIndex);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_nextSpawnTurn);
		}
		if ((syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_spawningEnabled);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			m_sequenceSourceId = reader.ReadPackedUInt32();
			m_nextPowerupPrefabIndex = (int)reader.ReadPackedUInt32();
			m_nextSpawnTurn = (int)reader.ReadPackedUInt32();
			m_spawningEnabled = reader.ReadBoolean();
			LogJson();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_sequenceSourceId = reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			m_nextPowerupPrefabIndex = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			HookNextSpawnTurn((int)reader.ReadPackedUInt32());
		}
		if ((num & 8) != 0)
		{
			m_spawningEnabled = reader.ReadBoolean();
		}
		LogJson(num);
	}

	private void LogJson(int mask = Int32.MaxValue)
	{
		var jsonLog = new List<string>();
		if ((mask & 1) != 0)
		{
			jsonLog.Add($"\"msequenceSourceId\":{DefaultJsonSerializer.Serialize(m_sequenceSourceId)}");
		}
		if ((mask & 2) != 0)
		{
			jsonLog.Add($"\"nextPowerupPrefabIndex\":{DefaultJsonSerializer.Serialize(m_nextPowerupPrefabIndex)}");
		}
		if ((mask & 4) != 0)
		{
			jsonLog.Add($"\"nextSpawnTurn\":{DefaultJsonSerializer.Serialize(Networkm_nextSpawnTurn)}");
		}
		if ((mask & 8) != 0)
		{
			jsonLog.Add($"\"spawningEnabled\":{DefaultJsonSerializer.Serialize(m_spawningEnabled)}");
		}

		Log.Info($"[JSON] {{\"powerUpSpawner\":{{{String.Join(",", jsonLog.ToArray())}}}}}");
	}
}
