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
			m_powerupSpawnerRoot = new GameObject("PowerupRoot_Spawners");
		}
		return m_powerupSpawnerRoot;
	}

	public GameObject GetSpawnedPersistentSequencesRoot()
	{
		if (m_powerupSequencesRoot == null)
		{
			m_powerupSequencesRoot = new GameObject("PowerupRoot_PersistentSequences");
		}
		return m_powerupSequencesRoot;
	}

	internal PowerUp GetPowerUpOfGuid(int guid)
	{
		if (guid >= 0 && m_guidToPowerupDictionary != null && m_guidToPowerupDictionary.ContainsKey(guid))
		{
			return m_guidToPowerupDictionary[guid];
		}
		return null;
	}

	internal void SetPowerUpGuid(PowerUp pup, int guid)
	{
		if (m_guidToPowerupDictionary.ContainsKey(guid))
		{
			Log.Error("Trying to add powerup guid more than once");
		}
		m_guidToPowerupDictionary.Add(guid, pup);
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
			foreach (PowerUp.IPowerUpListener current in s_powerUpListenersTemp)
			{
				AddListener(current);
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
		foreach (PowerUp.IPowerUpListener listener in powerUpListeners)
		{
			if (listener != null)
			{
				PowerUp[] activePowerUps = listener.GetActivePowerUps();
				foreach (PowerUp pup in activePowerUps)
				{
					if (pup != null && pup.boardSquare != null)
					{
						GridPos pupGridPos = pup.boardSquare.GetGridPos();
						if (pupGridPos.x == gridPos.x && pupGridPos.y == gridPos.y)
						{
							return pup;
						}
					}
				}
			}
		}
		return null;
	}

	public List<PowerUp> GetServerPowerUpsOnSquare(BoardSquare square)
	{
		List<PowerUp> list = new List<PowerUp>();
		foreach (PowerUp.IPowerUpListener listener in powerUpListeners)
		{
			if (listener != null)
			{
				PowerUp[] activePowerUps = listener.GetActivePowerUps();
				foreach (PowerUp pup in activePowerUps)
				{
					if (pup != null
						&& pup.boardSquare != null
						&& pup.boardSquare == square)
					{
						list.Add(pup);
					}
				}
			}
		}
		return list;
	}

	internal bool IsPowerUpSpawnPoint(BoardSquare square)
	{
		foreach (PowerUp.IPowerUpListener listener in powerUpListeners)
		{
			if (listener != null && listener.IsPowerUpSpawnPoint(square))
			{
				return true;
			}
		}
		return false;
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
				list.Add(clientPowerUp);
			}
		}
		return list;
	}

	public void SetSpawningEnabled(bool enabled)
	{
		foreach (PowerUp.IPowerUpListener listener in powerUpListeners)
		{
			if (listener != null)
			{
				listener.SetSpawningEnabled(enabled);
			}
		}
	}
}
