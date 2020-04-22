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

	public PowerUp currentPowerUp => m_currentPowerUp;

	public int Networkm_nextSpawnTurn
	{
		get
		{
			return m_nextSpawnTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_nextSpawnTurn, 1u);
		}
	}

	public int Networkm_nextPowerUpIndex
	{
		get
		{
			return m_nextPowerUpIndex;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_nextPowerUpIndex, 2u);
		}
	}

	private void Start()
	{
		Networkm_nextPowerUpIndex = -1;
		Networkm_nextSpawnTurn = m_initialSpawnDelay;
		PowerUpManager.AddListenerStatic(this);
		for (int i = 0; i < m_powerUpLocations.Length; i++)
		{
			if (m_powerUpLocations[i] != null)
			{
				m_powerUpLocations[i].Initialize();
			}
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	private void Update()
	{
		if (m_nextPowerUpIndex != -1)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SetupNextPowerupData();
			return;
		}
	}

	void PowerUp.IPowerUpListener.OnTurnTick()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_currentPowerUp == null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (m_nextSpawnTurn <= GameFlowData.Get().CurrentTurn)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									if (IsSpawningEnabled())
									{
										while (true)
										{
											switch (2)
											{
											case 0:
												break;
											default:
												SpawnPowerUp();
												return;
											}
										}
									}
									return;
								}
							}
						}
						return;
					}
				}
			}
			m_currentPowerUp.OnTurnTick();
			return;
		}
	}

	void PowerUp.IPowerUpListener.OnPowerUpDestroyed(PowerUp destroyedPowerUp)
	{
		Networkm_nextSpawnTurn = GameFlowData.Get().CurrentTurn + GameplayRandom.Range(m_spawnIntervalMin, m_spawnIntervalMax);
		if (GameplayMutators.Get() != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Networkm_nextSpawnTurn = Mathf.Max(m_nextSpawnTurn - GameplayMutators.GetPowerupRefreshSpeedAdjustment(), GameFlowData.Get().CurrentTurn + 1);
		}
		SetupNextPowerupData();
		m_currentPowerUp = null;
	}

	PowerUp[] PowerUp.IPowerUpListener.GetActivePowerUps()
	{
		return new PowerUp[1]
		{
			m_currentPowerUp
		};
	}

	bool PowerUp.IPowerUpListener.IsPowerUpSpawnPoint(BoardSquare square)
	{
		bool result = false;
		PowerUpLocation[] powerUpLocations = m_powerUpLocations;
		int num = 0;
		while (true)
		{
			if (num < powerUpLocations.Length)
			{
				PowerUpLocation powerUpLocation = powerUpLocations[num];
				if (powerUpLocation != null && powerUpLocation.boardSquare == square)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					result = true;
					break;
				}
				num++;
				continue;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
		}
		return result;
	}

	void PowerUp.IPowerUpListener.SetSpawningEnabled(bool enabled)
	{
		m_spawningEnabled = enabled;
	}

	private bool IsSpawningEnabled()
	{
		int result;
		if (m_spawningEnabled)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (DebugParameters.Get() != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				result = ((!DebugParameters.Get().GetParameterAsBool("DisablePowerUps")) ? 1 : 0);
			}
			else
			{
				result = 1;
			}
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private void SetupNextPowerupData()
	{
		if (!NetworkServer.active || m_powerUpPrefabs == null)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_powerUpLocations == null)
			{
				return;
			}
			Networkm_nextPowerUpIndex = GameplayRandom.Range(0, m_powerUpPrefabs.Length);
			PowerUpLocation powerUpLocation = m_powerUpLocations[GameplayRandom.Range(0, m_powerUpLocations.Length)];
			if (powerUpLocation != null && powerUpLocation.boardSquare != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				m_nextPowerUpLocation = powerUpLocation;
			}
			object nextPowerUpPrefab;
			if (m_powerUpPrefabs[m_nextPowerUpIndex] == null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				nextPowerUpPrefab = null;
			}
			else
			{
				nextPowerUpPrefab = m_powerUpPrefabs[m_nextPowerUpIndex].gameObject;
			}
			m_nextPowerUpPrefab = (GameObject)nextPowerUpPrefab;
			return;
		}
	}

	[Server]
	private void SpawnPowerUp()
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogWarning("[Server] function 'System.Void PowerUpCoordinator::SpawnPowerUp()' called on client");
					return;
				}
			}
		}
		if (!m_nextPowerUpPrefab)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (!m_nextPowerUpLocation)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if ((bool)m_nextPowerUpLocation.boardSquare)
				{
					Vector3 position = m_nextPowerUpLocation.boardSquare.ToVector3();
					GameObject gameObject = Object.Instantiate(m_nextPowerUpPrefab, position, Quaternion.identity);
					m_currentPowerUp = gameObject.GetComponent<PowerUp>();
					m_currentPowerUp.powerUpListener = this;
					m_nextPowerUpPrefab = null;
					m_nextPowerUpLocation = null;
					Networkm_nextPowerUpIndex = -1;
					NetworkServer.Spawn(gameObject);
					m_currentPowerUp.CalculateBoardSquare();
					m_currentPowerUp.CheckForPickupOnSpawn();
				}
				return;
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					writer.WritePackedUInt32((uint)m_nextSpawnTurn);
					writer.WritePackedUInt32((uint)m_nextPowerUpIndex);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_nextSpawnTurn);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			while (true)
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
				while (true)
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
			writer.WritePackedUInt32((uint)m_nextPowerUpIndex);
		}
		if (!flag)
		{
			while (true)
			{
				switch (6)
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
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_nextSpawnTurn = (int)reader.ReadPackedUInt32();
					m_nextPowerUpIndex = (int)reader.ReadPackedUInt32();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_nextSpawnTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) == 0)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			m_nextPowerUpIndex = (int)reader.ReadPackedUInt32();
			return;
		}
	}
}
