// ROGUES
// SERVER
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
			Log.Error($"Trying to add powerup guid more than once {guid}, {pup.name}"); // no params in reactor
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

#if SERVER
	// added in rogues
	// TODO POWERUPS call it
	public void ExecuteUnexecutedMovementHitsForAllPowerups(MovementStage movementStage, bool asFailsafe)
	{
		foreach (PowerUp.IPowerUpListener powerUpListener in powerUpListeners)
		{
			if (powerUpListener != null)
			{
				PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
				for (int i = 0; i < activePowerUps.Length; i++)
				{
					if (activePowerUps[i] != null && activePowerUps[i].boardSquare != null)
					{
						activePowerUps[i].ExecuteUnexecutedMovementResults_PowerUp(movementStage, asFailsafe);
					}
				}
			}
		}
	}

	// added in rogues
	public void ExecuteUnexecutedMovementHitsForAllPowerupsForDistance(float distance, MovementStage movementStage, bool asFailsafe, out bool stillHasUnexecutedHits, out float nextUnexecutedHitDistance)
	{
		stillHasUnexecutedHits = false;
		nextUnexecutedHitDistance = float.MaxValue;
		foreach (PowerUp.IPowerUpListener powerUpListener in powerUpListeners)
		{
			if (powerUpListener != null)
			{
				PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
				for (int i = 0; i < activePowerUps.Length; i++)
				{
					if (activePowerUps[i] != null && activePowerUps[i].boardSquare != null)
					{
						bool flag;
						float num;
						activePowerUps[i].ExecuteUnexecutedMovementResultsForDistance_PowerUp(distance, movementStage, asFailsafe, out flag, out num);
						if (flag)
						{
							stillHasUnexecutedHits |= true;
							if (num < nextUnexecutedHitDistance)
							{
								nextUnexecutedHitDistance = num;
							}
						}
					}
				}
			}
		}
	}

	// added in rogues
	public void GatherAllPowerupResultsInResponseToEvades(MovementCollection evadeMovementCollection)
	{
		foreach (PowerUp.IPowerUpListener powerUpListener in powerUpListeners)
		{
			if (powerUpListener != null)
			{
				PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
				for (int i = 0; i < activePowerUps.Length; i++)
				{
					if (activePowerUps[i] != null && activePowerUps[i].boardSquare != null)
					{
						activePowerUps[i].GatherResultsInResponseToEvades(evadeMovementCollection);
					}
				}
			}
		}
	}

	// added in rogues
	public void GatherAllPowerupResultsInResponseToKnockbacks(MovementCollection knockbackMovementCollection)
	{
		foreach (PowerUp.IPowerUpListener powerUpListener in powerUpListeners)
		{
			if (powerUpListener != null)
			{
				PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
				for (int i = 0; i < activePowerUps.Length; i++)
				{
					if (activePowerUps[i] != null && activePowerUps[i].boardSquare != null)
					{
						activePowerUps[i].GatherResultsInResponseToKnockbacks(knockbackMovementCollection);
					}
				}
			}
		}
	}

	// added in rogues
	public void ClearAllPowerupResultsForNormalMovement()
	{
		foreach (PowerUp.IPowerUpListener powerUpListener in powerUpListeners)
		{
			if (powerUpListener != null)
			{
				PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
				for (int i = 0; i < activePowerUps.Length; i++)
				{
					if (activePowerUps[i] != null && activePowerUps[i].boardSquare != null)
					{
						activePowerUps[i].GetMovementResultsForMovementStage(MovementStage.Normal).Clear();
					}
				}
			}
		}
	}

	// added in rogues
	public void GatherAllPowerupResultsInResponseToMovementSegment(ServerGameplayUtils.MovementGameplayData gameplayData, MovementStage movementStage, ref List<MovementResults> moveResultsForSegment)
	{
		if (gameplayData.m_currentlyConsideredPath.m_moverClashesHere)
		{
			return;
		}
		foreach (PowerUp.IPowerUpListener powerUpListener in powerUpListeners)
		{
			if (powerUpListener != null)
			{
				PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
				for (int i = 0; i < activePowerUps.Length; i++)
				{
					if (activePowerUps[i] != null && activePowerUps[i].boardSquare != null)
					{
						List<MovementResults> list = new List<MovementResults>();
						activePowerUps[i].GatherMovementResultsFromSegment(gameplayData.Actor, gameplayData.m_movementInstance, movementStage, gameplayData.m_currentlyConsideredPath.prev, gameplayData.m_currentlyConsideredPath, ref list);
						List<MovementResults> movementResultsForMovementStage = activePowerUps[i].GetMovementResultsForMovementStage(movementStage);
						for (int j = 0; j < list.Count; j++)
						{
							if (list[j].ShouldMovementHitUpdateTargetLastKnownPos(gameplayData.Actor))
							{
								gameplayData.m_currentlyConsideredPath.m_visibleToEnemies = true;
								gameplayData.m_currentlyConsideredPath.m_updateLastKnownPos = true;
							}
							gameplayData.m_currentlyConsideredPath.m_moverHasGameplayHitHere = true;
							movementResultsForMovementStage.Add(list[j]);
							moveResultsForSegment.Add(list[j]);
						}
					}
				}
			}
		}
	}

	// added in rogues
	public void IntegrateMovementDamageResults_Evasion(ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		foreach (PowerUp.IPowerUpListener powerUpListener in powerUpListeners)
		{
			if (powerUpListener != null)
			{
				PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
				for (int i = 0; i < activePowerUps.Length; i++)
				{
					if (activePowerUps[i] != null && activePowerUps[i].boardSquare != null)
					{
						activePowerUps[i].IntegrateDamageResultsForEvasion(ref actorToDeltaHP);
					}
				}
			}
		}
	}

	// added in rogues
	public void IntegrateMovementDamageResults_Knockback(ref Dictionary<ActorData, int> actorToDeltaHP)
	{
		foreach (PowerUp.IPowerUpListener powerUpListener in powerUpListeners)
		{
			if (powerUpListener != null)
			{
				PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
				for (int i = 0; i < activePowerUps.Length; i++)
				{
					if (activePowerUps[i] != null && activePowerUps[i].boardSquare != null)
					{
						activePowerUps[i].IntegrateDamageResultsForKnockback(ref actorToDeltaHP);
					}
				}
			}
		}
	}

	// added in rogues
	public void GatherGrossDamageResults_PowerUps_Evasion(ref Dictionary<ActorData, int> actorToGrossDamage_real, ref Dictionary<ActorData, ServerGameplayUtils.DamageDodgedStats> stats)
	{
		foreach (PowerUp.IPowerUpListener powerUpListener in powerUpListeners)
		{
			if (powerUpListener != null)
			{
				PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
				for (int i = 0; i < activePowerUps.Length; i++)
				{
					if (activePowerUps[i] != null && activePowerUps[i].boardSquare != null)
					{
						activePowerUps[i].GatherGrossDamageResults_PowerUp_Evasion(ref actorToGrossDamage_real, ref stats);
					}
				}
			}
		}
	}

	// added in rogues
	public bool HasPowerup(PowerUp powerup)
	{
		bool result = false;
		if (powerup == null)
		{
			result = false;
		}
		else
		{
			foreach (PowerUp.IPowerUpListener powerUpListener in powerUpListeners)
			{
				if (powerUpListener != null)
				{
					PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
					if (Array.Exists<PowerUp>(activePowerUps, (PowerUp p) => p == powerup))
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	// added in rogues
	public void UsePowerup(PowerUp powerup, ActorData user)
	{
		powerup.OnPickedUp(user);
	}

	// added in rogues
	public void ActorBecameAbleToCollectPowerups(ActorData actor)
	{
		PowerUp powerUpInPos = GetPowerUpInPos(actor.GetGridPos());
		if (powerUpInPos != null && powerUpInPos.CanBePickedUpByActor(actor))
		{
			powerUpInPos.ActorBecameAbleToCollectPowerups(actor);
		}
	}

	// added in rogues
	public void CollectSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		foreach (PowerUp.IPowerUpListener powerUpListener in powerUpListeners)
		{
			if (powerUpListener != null)
			{
				powerUpListener.AddToSquaresToAvoidForRespawn(squaresToAvoid, forActor);
			}
		}
	}
#endif

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
