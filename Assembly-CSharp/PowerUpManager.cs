using System;
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

	public static void AddListenerStatic(PowerUp.IPowerUpListener listener)
	{
		if (PowerUpManager.Get() == null)
		{
			if (PowerUpManager.s_powerUpListenersTemp == null)
			{
				PowerUpManager.s_powerUpListenersTemp = new List<PowerUp.IPowerUpListener>();
			}
			PowerUpManager.s_powerUpListenersTemp.Add(listener);
		}
		else
		{
			PowerUpManager.Get().AddListener(listener);
		}
	}

	public List<PowerUp.IPowerUpListener> powerUpListeners
	{
		get
		{
			return this.m_powerUpListeners;
		}
	}

	public GameObject GetSpawnedPowerupsRoot()
	{
		if (this.m_spawnedPowerupsRoot == null)
		{
			this.m_spawnedPowerupsRoot = new GameObject("PowerupRoot_SpawnedPowerups");
		}
		return this.m_spawnedPowerupsRoot;
	}

	public GameObject GetSpawnerRoot()
	{
		if (this.m_powerupSpawnerRoot == null)
		{
			this.m_powerupSpawnerRoot = new GameObject("PowerupRoot_Spawners");
		}
		return this.m_powerupSpawnerRoot;
	}

	public GameObject GetSpawnedPersistentSequencesRoot()
	{
		if (this.m_powerupSequencesRoot == null)
		{
			this.m_powerupSequencesRoot = new GameObject("PowerupRoot_PersistentSequences");
		}
		return this.m_powerupSequencesRoot;
	}

	internal PowerUp GetPowerUpOfGuid(int guid)
	{
		if (guid < 0)
		{
			return null;
		}
		if (this.m_guidToPowerupDictionary == null)
		{
			return null;
		}
		if (!this.m_guidToPowerupDictionary.ContainsKey(guid))
		{
			return null;
		}
		return this.m_guidToPowerupDictionary[guid];
	}

	internal void SetPowerUpGuid(PowerUp pup, int guid)
	{
		if (!this.m_guidToPowerupDictionary.ContainsKey(guid))
		{
			this.m_guidToPowerupDictionary.Add(guid, pup);
		}
		else
		{
			Log.Error("Trying to add powerup guid more than once", new object[0]);
		}
	}

	internal void OnPowerUpDestroy(PowerUp pup)
	{
		if (this.m_guidToPowerupDictionary.ContainsKey(pup.Guid))
		{
			this.m_guidToPowerupDictionary.Remove(pup.Guid);
		}
	}

	private void Awake()
	{
		PowerUpManager.s_instance = this;
		this.m_powerUpListeners = new List<PowerUp.IPowerUpListener>();
		if (PowerUpManager.s_powerUpListenersTemp != null)
		{
			using (List<PowerUp.IPowerUpListener>.Enumerator enumerator = PowerUpManager.s_powerUpListenersTemp.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PowerUp.IPowerUpListener listener = enumerator.Current;
					this.AddListener(listener);
				}
			}
			PowerUpManager.s_powerUpListenersTemp.Clear();
			PowerUpManager.s_powerUpListenersTemp = null;
		}
		this.m_powerupSpawnerRoot = new GameObject("PowerupRoot_Spawners");
		this.m_spawnedPowerupsRoot = new GameObject("PowerupRoot_SpawnedPowerups");
		this.m_powerupSequencesRoot = new GameObject("PowerupRoot_PersistentSequences");
	}

	private void OnDestroy()
	{
		if (PowerUpManager.s_powerUpListenersTemp != null)
		{
			PowerUpManager.s_powerUpListenersTemp.Clear();
		}
		PowerUpManager.s_instance = null;
	}

	public static PowerUpManager Get()
	{
		return PowerUpManager.s_instance;
	}

	public void AddListener(PowerUp.IPowerUpListener listener)
	{
		this.m_powerUpListeners.Add(listener);
	}

	public void RemoveListener(PowerUp.IPowerUpListener listener)
	{
		this.m_powerUpListeners.Remove(listener);
	}

	public PowerUp GetPowerUpInPos(GridPos gridPos)
	{
		PowerUp result = null;
		using (List<PowerUp.IPowerUpListener>.Enumerator enumerator = this.powerUpListeners.GetEnumerator())
		{
			IL_DC:
			while (enumerator.MoveNext())
			{
				PowerUp.IPowerUpListener powerUpListener = enumerator.Current;
				if (powerUpListener != null)
				{
					PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
					for (int i = 0; i < activePowerUps.Length; i++)
					{
						if (activePowerUps[i] != null)
						{
							if (activePowerUps[i].boardSquare != null)
							{
								GridPos gridPos2 = activePowerUps[i].boardSquare.GetGridPos();
								if (gridPos2.x == gridPos.x)
								{
									if (gridPos2.y == gridPos.y)
									{
										result = activePowerUps[i];
										goto IL_DC;
									}
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	public List<PowerUp> GetServerPowerUpsOnSquare(BoardSquare square)
	{
		List<PowerUp> list = new List<PowerUp>();
		foreach (PowerUp.IPowerUpListener powerUpListener in this.powerUpListeners)
		{
			if (powerUpListener != null)
			{
				PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
				for (int i = 0; i < activePowerUps.Length; i++)
				{
					if (activePowerUps[i] != null)
					{
						if (activePowerUps[i].boardSquare != null)
						{
							if (activePowerUps[i].boardSquare == square)
							{
								list.Add(activePowerUps[i]);
							}
						}
					}
				}
			}
		}
		return list;
	}

	internal bool IsPowerUpSpawnPoint(BoardSquare square)
	{
		bool result = false;
		using (List<PowerUp.IPowerUpListener>.Enumerator enumerator = this.powerUpListeners.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PowerUp.IPowerUpListener powerUpListener = enumerator.Current;
				if (powerUpListener != null)
				{
					if (powerUpListener.IsPowerUpSpawnPoint(square))
					{
						return true;
					}
				}
			}
		}
		return result;
	}

	public void TrackClientPowerUp(PowerUp powerUp)
	{
		this.m_clientPowerUps.Add(powerUp);
	}

	public void UntrackClientPowerUp(PowerUp powerUp)
	{
		this.m_clientPowerUps.Remove(powerUp);
	}

	public List<PowerUp> GetClientPowerUpsOnSquare(BoardSquare square)
	{
		List<PowerUp> list = new List<PowerUp>();
		foreach (PowerUp powerUp in this.m_clientPowerUps)
		{
			if (powerUp.boardSquare == square)
			{
				list.Add(powerUp);
			}
		}
		return list;
	}

	public void SetSpawningEnabled(bool enabled)
	{
		using (List<PowerUp.IPowerUpListener>.Enumerator enumerator = this.powerUpListeners.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PowerUp.IPowerUpListener powerUpListener = enumerator.Current;
				if (powerUpListener != null)
				{
					powerUpListener.SetSpawningEnabled(enabled);
				}
			}
		}
	}
}
