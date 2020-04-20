using System;
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
		get
		{
			return this.m_currentPowerUp;
		}
	}

	private void Start()
	{
		this.Networkm_nextPowerUpIndex = -1;
		this.Networkm_nextSpawnTurn = this.m_initialSpawnDelay;
		PowerUpManager.AddListenerStatic(this);
		for (int i = 0; i < this.m_powerUpLocations.Length; i++)
		{
			if (this.m_powerUpLocations[i] != null)
			{
				this.m_powerUpLocations[i].Initialize();
			}
		}
	}

	private void Update()
	{
		if (this.m_nextPowerUpIndex == -1)
		{
			this.SetupNextPowerupData();
		}
	}

	void PowerUp.IPowerUpListener.OnTurnTick()
	{
		if (NetworkServer.active)
		{
			if (this.m_currentPowerUp == null)
			{
				if (this.m_nextSpawnTurn <= GameFlowData.Get().CurrentTurn)
				{
					if (this.IsSpawningEnabled())
					{
						this.SpawnPowerUp();
					}
				}
			}
			else
			{
				this.m_currentPowerUp.OnTurnTick();
			}
		}
	}

	void PowerUp.IPowerUpListener.OnPowerUpDestroyed(PowerUp destroyedPowerUp)
	{
		this.Networkm_nextSpawnTurn = GameFlowData.Get().CurrentTurn + GameplayRandom.Range(this.m_spawnIntervalMin, this.m_spawnIntervalMax);
		if (GameplayMutators.Get() != null)
		{
			this.Networkm_nextSpawnTurn = Mathf.Max(this.m_nextSpawnTurn - GameplayMutators.GetPowerupRefreshSpeedAdjustment(), GameFlowData.Get().CurrentTurn + 1);
		}
		this.SetupNextPowerupData();
		this.m_currentPowerUp = null;
	}

	PowerUp[] PowerUp.IPowerUpListener.GetActivePowerUps()
	{
		return new PowerUp[]
		{
			this.m_currentPowerUp
		};
	}

	bool PowerUp.IPowerUpListener.IsPowerUpSpawnPoint(BoardSquare square)
	{
		bool result = false;
		foreach (PowerUpLocation powerUpLocation in this.m_powerUpLocations)
		{
			if (powerUpLocation != null && powerUpLocation.boardSquare == square)
			{
				result = true;
				return result;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			return result;
		}
	}

	void PowerUp.IPowerUpListener.SetSpawningEnabled(bool enabled)
	{
		this.m_spawningEnabled = enabled;
	}

	private bool IsSpawningEnabled()
	{
		bool result;
		if (this.m_spawningEnabled)
		{
			if (DebugParameters.Get() != null)
			{
				result = !DebugParameters.Get().GetParameterAsBool("DisablePowerUps");
			}
			else
			{
				result = true;
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	private void SetupNextPowerupData()
	{
		if (NetworkServer.active && this.m_powerUpPrefabs != null)
		{
			if (this.m_powerUpLocations != null)
			{
				this.Networkm_nextPowerUpIndex = GameplayRandom.Range(0, this.m_powerUpPrefabs.Length);
				PowerUpLocation powerUpLocation = this.m_powerUpLocations[GameplayRandom.Range(0, this.m_powerUpLocations.Length)];
				if (powerUpLocation != null && powerUpLocation.boardSquare != null)
				{
					this.m_nextPowerUpLocation = powerUpLocation;
				}
				GameObject nextPowerUpPrefab;
				if (this.m_powerUpPrefabs[this.m_nextPowerUpIndex] == null)
				{
					nextPowerUpPrefab = null;
				}
				else
				{
					nextPowerUpPrefab = this.m_powerUpPrefabs[this.m_nextPowerUpIndex].gameObject;
				}
				this.m_nextPowerUpPrefab = nextPowerUpPrefab;
			}
		}
	}

	[Server]
	private void SpawnPowerUp()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PowerUpCoordinator::SpawnPowerUp()' called on client");
			return;
		}
		if (this.m_nextPowerUpPrefab)
		{
			if (this.m_nextPowerUpLocation)
			{
				if (this.m_nextPowerUpLocation.boardSquare)
				{
					Vector3 position = this.m_nextPowerUpLocation.boardSquare.ToVector3();
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_nextPowerUpPrefab, position, Quaternion.identity);
					this.m_currentPowerUp = gameObject.GetComponent<PowerUp>();
					this.m_currentPowerUp.powerUpListener = this;
					this.m_nextPowerUpPrefab = null;
					this.m_nextPowerUpLocation = null;
					this.Networkm_nextPowerUpIndex = -1;
					NetworkServer.Spawn(gameObject);
					this.m_currentPowerUp.CalculateBoardSquare();
					this.m_currentPowerUp.CheckForPickupOnSpawn();
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

	public int Networkm_nextSpawnTurn
	{
		get
		{
			return this.m_nextSpawnTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_nextSpawnTurn, 1U);
		}
	}

	public int Networkm_nextPowerUpIndex
	{
		get
		{
			return this.m_nextPowerUpIndex;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_nextPowerUpIndex, 2U);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)this.m_nextSpawnTurn);
			writer.WritePackedUInt32((uint)this.m_nextPowerUpIndex);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_nextSpawnTurn);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_nextPowerUpIndex);
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
			this.m_nextSpawnTurn = (int)reader.ReadPackedUInt32();
			this.m_nextPowerUpIndex = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_nextSpawnTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			this.m_nextPowerUpIndex = (int)reader.ReadPackedUInt32();
		}
	}
}
