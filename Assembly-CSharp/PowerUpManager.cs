using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
	public static List<PowerUp.IPowerUpListener> s_powerUpListenersTemp;

	private static PowerUpManager s_instance;

	private List<PowerUp.IPowerUpListener> m_powerUpListeners;

	private List<PowerUp> m_clientPowerUps = new List<PowerUp>();

	private GameObject m_spawnedPowerupsRoot;

	private GameObject m_powerupSpawnerRoot;

	private GameObject m_powerupSequencesRoot;

	private Dictionary<int, PowerUp> m_guidToPowerupDictionary = new Dictionary<int, PowerUp>();

	public List<PowerUp.IPowerUpListener> powerUpListeners => m_powerUpListeners;

	public static void AddListenerStatic(PowerUp.IPowerUpListener listener)
	{
		if (Get() == null)
		{
			if (s_powerUpListenersTemp == null)
			{
				s_powerUpListenersTemp = new List<PowerUp.IPowerUpListener>();
			}
			s_powerUpListenersTemp.Add(listener);
		}
		else
		{
			Get().AddListener(listener);
		}
	}

	public GameObject GetSpawnedPowerupsRoot()
	{
		if (m_spawnedPowerupsRoot == null)
		{
			m_spawnedPowerupsRoot = new GameObject("PowerupRoot_SpawnedPowerups");
		}
		return m_spawnedPowerupsRoot;
	}

	public GameObject GetSpawnerRoot()
	{
		if (m_powerupSpawnerRoot == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_powerupSpawnerRoot = new GameObject("PowerupRoot_Spawners");
		}
		return m_powerupSpawnerRoot;
	}

	public GameObject GetSpawnedPersistentSequencesRoot()
	{
		if (m_powerupSequencesRoot == null)
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
			m_powerupSequencesRoot = new GameObject("PowerupRoot_PersistentSequences");
		}
		return m_powerupSequencesRoot;
	}

	internal PowerUp GetPowerUpOfGuid(int guid)
	{
		if (guid < 0)
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
					return null;
				}
			}
		}
		if (m_guidToPowerupDictionary == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		if (!m_guidToPowerupDictionary.ContainsKey(guid))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		return m_guidToPowerupDictionary[guid];
	}

	internal void SetPowerUpGuid(PowerUp pup, int guid)
	{
		if (!m_guidToPowerupDictionary.ContainsKey(guid))
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
					m_guidToPowerupDictionary.Add(guid, pup);
					return;
				}
			}
		}
		Log.Error("Trying to add powerup guid more than once");
	}

	internal void OnPowerUpDestroy(PowerUp pup)
	{
		if (m_guidToPowerupDictionary.ContainsKey(pup.Guid))
		{
			m_guidToPowerupDictionary.Remove(pup.Guid);
		}
	}

	private void Awake()
	{
		s_instance = this;
		m_powerUpListeners = new List<PowerUp.IPowerUpListener>();
		if (s_powerUpListenersTemp != null)
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
			using (List<PowerUp.IPowerUpListener>.Enumerator enumerator = s_powerUpListenersTemp.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PowerUp.IPowerUpListener current = enumerator.Current;
					AddListener(current);
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			s_powerUpListenersTemp.Clear();
			s_powerUpListenersTemp = null;
		}
		m_powerupSpawnerRoot = new GameObject("PowerupRoot_Spawners");
		m_spawnedPowerupsRoot = new GameObject("PowerupRoot_SpawnedPowerups");
		m_powerupSequencesRoot = new GameObject("PowerupRoot_PersistentSequences");
	}

	private void OnDestroy()
	{
		if (s_powerUpListenersTemp != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			s_powerUpListenersTemp.Clear();
		}
		s_instance = null;
	}

	public static PowerUpManager Get()
	{
		return s_instance;
	}

	public void AddListener(PowerUp.IPowerUpListener listener)
	{
		m_powerUpListeners.Add(listener);
	}

	public void RemoveListener(PowerUp.IPowerUpListener listener)
	{
		m_powerUpListeners.Remove(listener);
	}

	public PowerUp GetPowerUpInPos(GridPos gridPos)
	{
		PowerUp result = null;
		using (List<PowerUp.IPowerUpListener>.Enumerator enumerator = powerUpListeners.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PowerUp.IPowerUpListener current = enumerator.Current;
				if (current != null)
				{
					PowerUp[] activePowerUps = current.GetActivePowerUps();
					int num = 0;
					while (true)
					{
						if (num >= activePowerUps.Length)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							break;
						}
						if (activePowerUps[num] != null)
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
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							if (activePowerUps[num].boardSquare != null)
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
								GridPos gridPos2 = activePowerUps[num].boardSquare.GetGridPos();
								if (gridPos2.x == gridPos.x)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (gridPos2.y == gridPos.y)
									{
										result = activePowerUps[num];
										break;
									}
								}
							}
						}
						num++;
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	public List<PowerUp> GetServerPowerUpsOnSquare(BoardSquare square)
	{
		List<PowerUp> list = new List<PowerUp>();
		foreach (PowerUp.IPowerUpListener powerUpListener in powerUpListeners)
		{
			if (powerUpListener != null)
			{
				while (true)
				{
					switch (2)
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
				PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
				for (int i = 0; i < activePowerUps.Length; i++)
				{
					if (activePowerUps[i] != null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (activePowerUps[i].boardSquare != null)
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
							if (activePowerUps[i].boardSquare == square)
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
								list.Add(activePowerUps[i]);
							}
						}
					}
				}
				while (true)
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
		return list;
	}

	internal bool IsPowerUpSpawnPoint(BoardSquare square)
	{
		bool result = false;
		using (List<PowerUp.IPowerUpListener>.Enumerator enumerator = powerUpListeners.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PowerUp.IPowerUpListener current = enumerator.Current;
				if (current != null)
				{
					while (true)
					{
						switch (2)
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
					if (current.IsPowerUpSpawnPoint(square))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	public void TrackClientPowerUp(PowerUp powerUp)
	{
		m_clientPowerUps.Add(powerUp);
	}

	public void UntrackClientPowerUp(PowerUp powerUp)
	{
		m_clientPowerUps.Remove(powerUp);
	}

	public List<PowerUp> GetClientPowerUpsOnSquare(BoardSquare square)
	{
		List<PowerUp> list = new List<PowerUp>();
		foreach (PowerUp clientPowerUp in m_clientPowerUps)
		{
			if (clientPowerUp.boardSquare == square)
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
				list.Add(clientPowerUp);
			}
		}
		return list;
	}

	public void SetSpawningEnabled(bool enabled)
	{
		using (List<PowerUp.IPowerUpListener>.Enumerator enumerator = powerUpListeners.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				enumerator.Current?.SetSpawningEnabled(enabled);
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
	}
}
