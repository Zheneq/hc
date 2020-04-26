using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

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

	[Separator("Default Prefabs", true)]
	public PowerUp m_powerUpPrefab;

	public GameObject m_baseSequencePrefab;

	public GameObject m_spawnSequencePrefab;

	[Separator("Additional Prefabs for mixing up powerup to spawn", true)]
	public ExtraPowerupSelectMode m_extraPowerupSelectMode;

	public List<PowerupSpawnInfo> m_extraPowerupsForMixedSpawn;

	public bool m_useSameFirstPowerupIfRandom = true;

	[Separator("Timing of spawns", true)]
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

	public bool IsEnabled
	{
		get
		{
			int result;
			if (m_spawningEnabled)
			{
				result = (m_isReady ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
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
			SetSyncVar(value, ref m_sequenceSourceId, 1u);
		}
	}

	public int Networkm_nextPowerupPrefabIndex
	{
		get
		{
			return m_nextPowerupPrefabIndex;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_nextPowerupPrefabIndex, 2u);
		}
	}

	public int Networkm_nextSpawnTurn
	{
		get
		{
			return m_nextSpawnTurn;
		}
		[param: In]
		set
		{
			ref int nextSpawnTurn = ref m_nextSpawnTurn;
			if (NetworkServer.localClientActive)
			{
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookNextSpawnTurn(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref nextSpawnTurn, 4u);
		}
	}

	public bool Networkm_spawningEnabled
	{
		get
		{
			return m_spawningEnabled;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_spawningEnabled, 8u);
		}
	}

	private int ChooseNextPrefabSpawnIndex(bool isForFirstSpawn = false)
	{
		int result = 0;
		if (m_extraPowerupsForMixedSpawn != null)
		{
			int count = m_finalizedPowerupSpawnInfoList.Count;
			if (m_extraPowerupSelectMode == ExtraPowerupSelectMode.InOrder)
			{
				if (m_lastServerSpawnPrefabIndex >= 0)
				{
					result = (m_lastServerSpawnPrefabIndex + 1) % count;
				}
				else
				{
					result = 0;
				}
			}
			else if (m_extraPowerupSelectMode == ExtraPowerupSelectMode.Random)
			{
				if (isForFirstSpawn)
				{
					if (m_useSameFirstPowerupIfRandom)
					{
						result = 0;
						goto IL_0096;
					}
				}
				result = UnityEngine.Random.Range(0, count);
			}
		}
		goto IL_0096;
		IL_0096:
		return result;
	}

	private void HookNextSpawnTurn(int nextSpawnTurn)
	{
		bool flag = m_nextSpawnTurn != nextSpawnTurn;
		Networkm_nextSpawnTurn = nextSpawnTurn;
		if (!flag)
		{
			return;
		}
		while (true)
		{
			UpdateTimerController();
			return;
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
		PowerupSpawnInfo powerupSpawnInfo = new PowerupSpawnInfo();
		powerupSpawnInfo.m_powerupObjectPrefab = m_powerUpPrefab;
		powerupSpawnInfo.m_baseSeqPrefab = m_baseSequencePrefab;
		powerupSpawnInfo.m_spawnSeqPrefab = m_spawnSequencePrefab;
		m_finalizedPowerupSpawnInfoList.Add(powerupSpawnInfo);
		if (m_extraPowerupsForMixedSpawn != null)
		{
			if (m_extraPowerupsForMixedSpawn.Count > 0)
			{
				for (int i = 0; i < m_extraPowerupsForMixedSpawn.Count; i++)
				{
					PowerupSpawnInfo powerupSpawnInfo2 = m_extraPowerupsForMixedSpawn[i];
					if (!(powerupSpawnInfo2.m_powerupObjectPrefab != null))
					{
						continue;
					}
					if (powerupSpawnInfo2.m_baseSeqPrefab != null)
					{
						m_finalizedPowerupSpawnInfoList.Add(powerupSpawnInfo2);
					}
				}
			}
		}
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			Networkm_nextPowerupPrefabIndex = ChooseNextPrefabSpawnIndex(true);
			return;
		}
	}

	public void Start()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReplayRestart);
			return;
		}
	}

	private void Initialize()
	{
		if (m_initialized)
		{
			return;
		}
		while (true)
		{
			m_initialized = true;
			PowerUpManager.AddListenerStatic(this);
			Board board = Board.Get();
			Vector3 position = base.transform.position;
			float x = position.x;
			Vector3 position2 = base.transform.position;
			m_boardSquare = board.GetBoardSquareSafe(x, position2.z);
			PlayBaseSequence();
			base.transform.parent = PowerUpManager.Get().GetSpawnerRoot().transform;
			return;
		}
	}

	public void OnDestroy()
	{
		if (PowerUpManager.Get() != null)
		{
			PowerUpManager.Get().RemoveListener(this);
		}
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReplayRestart);
			return;
		}
	}

	private void PlayBaseSequence()
	{
		GameObject baseSeqPrefab = m_finalizedPowerupSpawnInfoList[0].m_baseSeqPrefab;
		if (m_nextPowerupPrefabIndex >= 0 && m_nextPowerupPrefabIndex < m_finalizedPowerupSpawnInfoList.Count)
		{
			baseSeqPrefab = m_finalizedPowerupSpawnInfoList[m_nextPowerupPrefabIndex].m_baseSeqPrefab;
		}
		if (!(baseSeqPrefab != null))
		{
			return;
		}
		while (true)
		{
			SequenceSource source = new SequenceSource(null, null, m_sequenceSourceId, false);
			m_baseSequences = SequenceManager.Get().CreateClientSequences(baseSeqPrefab, m_boardSquare, null, null, source, null);
			SetSequencesRoot(m_baseSequences);
			UpdateTimerController();
			return;
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
		if (sequences == null)
		{
			return;
		}
		while (true)
		{
			if (!(PowerUpManager.Get() != null))
			{
				return;
			}
			while (true)
			{
				foreach (Sequence sequence in sequences)
				{
					if (!(sequence != null))
					{
						continue;
					}
					while (true)
					{
						sequence.transform.parent = PowerUpManager.Get().GetSpawnedPersistentSequencesRoot().transform;
						return;
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
	}

	private void UpdateTimerController()
	{
		if (m_baseSequences == null)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_baseSequences.Length; i++)
			{
				if (IsEnabled)
				{
					m_baseSequences[i].SetTimerController(m_nextSpawnTurn - GameFlowData.Get().CurrentTurn);
				}
				else
				{
					m_baseSequences[i].SetTimerController(5);
				}
			}
			return;
		}
	}

	private void Update()
	{
		UpdateTimerController();
		if (!m_initialized)
		{
			if (!(VisualsLoader.Get() == null))
			{
				if (!VisualsLoader.Get().LevelLoaded())
				{
					goto IL_0050;
				}
			}
			Initialize();
		}
		goto IL_0050;
		IL_0050:
		if (!m_initialized || !NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			if (m_nextPowerupPrefabIndex >= 0 && m_currentBasePrefabIndex != m_nextPowerupPrefabIndex)
			{
				while (true)
				{
					ClearPreviousBaseSpawnSequences();
					PlayBaseSequence();
					m_currentBasePrefabIndex = m_nextPowerupPrefabIndex;
					return;
				}
			}
			return;
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		Gizmos.DrawIcon(base.transform.position, "icon_PowerUp.png");
	}

	void PowerUp.IPowerUpListener.OnPowerUpDestroyed(PowerUp destroyedPowerUp)
	{
		Networkm_nextSpawnTurn = GameFlowData.Get().CurrentTurn + m_spawnInterval;
		if (GameplayMutators.Get() != null)
		{
			Networkm_nextSpawnTurn = Mathf.Max(m_nextSpawnTurn - GameplayMutators.GetPowerupRefreshSpeedAdjustment(), GameFlowData.Get().CurrentTurn + 1);
		}
		Networkm_nextPowerupPrefabIndex = ChooseNextPrefabSpawnIndex();
		UpdateTimerController();
		m_powerUpInstance = null;
	}

	PowerUp[] PowerUp.IPowerUpListener.GetActivePowerUps()
	{
		return new PowerUp[1]
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
		if (m_spawnSequences == null)
		{
			return;
		}
		while (true)
		{
			Sequence[] spawnSequences = m_spawnSequences;
			foreach (Sequence sequence in spawnSequences)
			{
				sequence.MarkForRemoval();
			}
			while (true)
			{
				m_spawnSequences = null;
				return;
			}
		}
	}

	private void ClearPreviousBaseSpawnSequences()
	{
		if (m_baseSequences == null)
		{
			return;
		}
		while (true)
		{
			Sequence[] baseSequences = m_baseSequences;
			foreach (Sequence sequence in baseSequences)
			{
				sequence.MarkForRemoval();
			}
			m_baseSequences = null;
			return;
		}
	}

	void PowerUp.IPowerUpListener.OnTurnTick()
	{
		UpdateTimerController();
		ClearPreviousSpawnSequences();
		if (m_nextSpawnTurn == GameFlowData.Get().CurrentTurn)
		{
			if (IsEnabled)
			{
				PlaySpawnSequence();
			}
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
		if (m_nextPowerupPrefabIndex < 0 || m_currentBasePrefabIndex == m_nextPowerupPrefabIndex)
		{
			return;
		}
		while (true)
		{
			ClearPreviousBaseSpawnSequences();
			PlayBaseSequence();
			m_currentBasePrefabIndex = m_nextPowerupPrefabIndex;
			return;
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void PowerUpSpawner::SpawnPowerUp()' called on client");
					return;
				}
			}
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
		if (!powerupObjectPrefab)
		{
			return;
		}
		while (true)
		{
			Vector3 position = boardSquare.ToVector3();
			m_powerUpInstance = UnityEngine.Object.Instantiate(powerupObjectPrefab, position, Quaternion.identity);
			m_powerUpInstance.SetPickupTeam(m_teamRestriction);
			GameObject gameObject = m_powerUpInstance.gameObject;
			m_powerUpInstance.powerUpListener = this;
			NetworkServer.Spawn(gameObject);
			m_powerUpInstance.CalculateBoardSquare();
			m_powerUpInstance.CheckForPickupOnSpawn();
			if (m_tagsToApplyToPowerup == null)
			{
				return;
			}
			while (true)
			{
				string[] tagsToApplyToPowerup = m_tagsToApplyToPowerup;
				foreach (string powerupTag in tagsToApplyToPowerup)
				{
					m_powerUpInstance.AddTag(powerupTag);
				}
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
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32(m_sequenceSourceId);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_nextPowerupPrefabIndex);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_nextSpawnTurn);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_spawningEnabled);
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
				switch (5)
				{
				case 0:
					break;
				default:
					m_sequenceSourceId = reader.ReadPackedUInt32();
					m_nextPowerupPrefabIndex = (int)reader.ReadPackedUInt32();
					m_nextSpawnTurn = (int)reader.ReadPackedUInt32();
					m_spawningEnabled = reader.ReadBoolean();
					return;
				}
			}
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
		if ((num & 8) == 0)
		{
			return;
		}
		while (true)
		{
			m_spawningEnabled = reader.ReadBoolean();
			return;
		}
	}
}
