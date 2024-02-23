using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class PowerUpCoordinator : NetworkBehaviour, PowerUp.IPowerUpListener
{
	public PowerUpLocation[] m_powerUpLocations;
	public PowerUp[] m_powerUpPrefabs;
	public int m_spawnIntervalMin;
	public int m_spawnIntervalMax;
	public int m_initialSpawnDelay;
	
	[SyncVar]
	private int m_nextSpawnTurn;
	private PowerUp m_currentPowerUp;
	private PowerUpLocation m_nextPowerUpLocation;
	private GameObject m_nextPowerUpPrefab;
	[SyncVar]
	private int m_nextPowerUpIndex;
	private bool m_spawningEnabled = true;

	public PowerUp currentPowerUp
	{
		get { return m_currentPowerUp; }
	}

	public int Networkm_nextSpawnTurn
	{
		get { return m_nextSpawnTurn; }
		[param: In]
		set { SetSyncVar(value, ref m_nextSpawnTurn, 1u); }
	}

	public int Networkm_nextPowerUpIndex
	{
		get { return m_nextPowerUpIndex; }
		[param: In]
		set { SetSyncVar(value, ref m_nextPowerUpIndex, 2u); }
	}

	private void Start()
	{
		Networkm_nextPowerUpIndex = -1;
		Networkm_nextSpawnTurn = m_initialSpawnDelay;
		PowerUpManager.AddListenerStatic(this);
		foreach (PowerUpLocation powerUpLocation in m_powerUpLocations)
		{
			if (powerUpLocation != null)
			{
				powerUpLocation.Initialize();
			}
		}
	}

	private void Update()
	{
		if (m_nextPowerUpIndex == -1)
		{
			SetupNextPowerupData();
		}
	}

	void PowerUp.IPowerUpListener.OnTurnTick()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (m_currentPowerUp == null)
		{
			if (m_nextSpawnTurn <= GameFlowData.Get().CurrentTurn && IsSpawningEnabled())
			{
				SpawnPowerUp();
			}
		}
		else
		{
			m_currentPowerUp.OnTurnTick();
		}
	}

	void PowerUp.IPowerUpListener.OnPowerUpDestroyed(PowerUp destroyedPowerUp)
	{
		Networkm_nextSpawnTurn = GameFlowData.Get().CurrentTurn + GameplayRandom.Range(m_spawnIntervalMin, m_spawnIntervalMax);
		if (GameplayMutators.Get() != null)
		{
			Networkm_nextSpawnTurn = Mathf.Max(
				m_nextSpawnTurn - GameplayMutators.GetPowerupRefreshSpeedAdjustment(), 
				GameFlowData.Get().CurrentTurn + 1);
		}
		SetupNextPowerupData();
		m_currentPowerUp = null;
	}

	PowerUp[] PowerUp.IPowerUpListener.GetActivePowerUps()
	{
		return new[]
		{
			m_currentPowerUp
		};
	}

	bool PowerUp.IPowerUpListener.IsPowerUpSpawnPoint(BoardSquare square)
	{
		foreach (PowerUpLocation powerUpLocation in m_powerUpLocations)
		{
			if (powerUpLocation != null && powerUpLocation.boardSquare == square)
			{
				return true;
			}
		}
		return false;
	}

	void PowerUp.IPowerUpListener.SetSpawningEnabled(bool enabled)
	{
		m_spawningEnabled = enabled;
	}

	private bool IsSpawningEnabled()
	{
		return m_spawningEnabled
		       && (DebugParameters.Get() == null || !DebugParameters.Get().GetParameterAsBool("DisablePowerUps"));
	}

	private void SetupNextPowerupData()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (m_powerUpPrefabs == null || m_powerUpLocations == null)
		{
			return;
		}
		Networkm_nextPowerUpIndex = GameplayRandom.Range(0, m_powerUpPrefabs.Length);
		PowerUpLocation powerUpLocation = m_powerUpLocations[GameplayRandom.Range(0, m_powerUpLocations.Length)];
		if (powerUpLocation != null && powerUpLocation.boardSquare != null)
		{
			m_nextPowerUpLocation = powerUpLocation;
		}
		m_nextPowerUpPrefab = m_powerUpPrefabs[m_nextPowerUpIndex] != null
			? m_powerUpPrefabs[m_nextPowerUpIndex].gameObject
			: null;
	}

	[Server]
	private void SpawnPowerUp()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PowerUpCoordinator::SpawnPowerUp()' called on client");
			return;
		}
		if (!m_nextPowerUpPrefab
		    || !m_nextPowerUpLocation
		    || !m_nextPowerUpLocation.boardSquare)
		{
			return;
		}
		Vector3 position = m_nextPowerUpLocation.boardSquare.ToVector3();
		GameObject gameObject = Instantiate(m_nextPowerUpPrefab, position, Quaternion.identity);
		m_currentPowerUp = gameObject.GetComponent<PowerUp>();
		m_currentPowerUp.powerUpListener = this;
		m_nextPowerUpPrefab = null;
		m_nextPowerUpLocation = null;
		Networkm_nextPowerUpIndex = -1;
		NetworkServer.Spawn(gameObject);
		m_currentPowerUp.CalculateBoardSquare();
		m_currentPowerUp.CheckForPickupOnSpawn();
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
			writer.WritePackedUInt32((uint)m_nextSpawnTurn);
			writer.WritePackedUInt32((uint)m_nextPowerUpIndex);
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
			writer.WritePackedUInt32((uint)m_nextSpawnTurn);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_nextPowerUpIndex);
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
			m_nextSpawnTurn = (int)reader.ReadPackedUInt32();
			m_nextPowerUpIndex = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_nextSpawnTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			m_nextPowerUpIndex = (int)reader.ReadPackedUInt32();
		}
	}
}
