using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpoilsManager : MonoBehaviour, PowerUp.IPowerUpListener
{
	public enum SpoilsType
	{
		None,
		Hero,
		Minion
	}

	public PowerUp[] m_heroSpoils;
	public PowerUp[] m_minionSpoils;

	private List<PowerUp> m_activePowerUps;
	private static SpoilsManager s_instance;

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Start()
	{
		m_activePowerUps = new List<PowerUp>();
		PowerUpManager.AddListenerStatic(this);
	}

	public static SpoilsManager Get()
	{
		return s_instance;
	}

	private PowerUp PickRandomPowerUpPrefab(SpoilsType spoilsType)
	{
		switch (spoilsType)
		{
			case SpoilsType.Hero:
				if (m_heroSpoils != null && m_heroSpoils.Length > 0)
				{
					return m_heroSpoils[GameplayRandom.Range(0, m_heroSpoils.Length)];
				}
				return null;
			case SpoilsType.Minion  :
				if (m_heroSpoils != null && m_heroSpoils.Length > 0)
				{
					return m_minionSpoils[GameplayRandom.Range(0, m_heroSpoils.Length)];
				}
				return null;
			default:
				return null;
		}
	}

	internal PowerUp SpawnSpoils(BoardSquare square, SpoilsType spoilsType, Team pickupTeam)
	{
		PowerUp powerUp = null;
		if (NetworkServer.active)
		{
			PowerUp prefab = PickRandomPowerUpPrefab(spoilsType);
			if (prefab != null)
			{
				Vector3 position = square.ToVector3();
				GameObject powerUpGameObject = Instantiate(prefab.gameObject, position, Quaternion.identity);
				powerUp = powerUpGameObject.GetComponent<PowerUp>();
				powerUp.PickupTeam = pickupTeam;
				powerUp.powerUpListener = this;
				m_activePowerUps.Add(powerUp);
				powerUp.transform.parent = transform;
				powerUp.Networkm_isSpoil = true;
				NetworkServer.Spawn(powerUpGameObject);
				powerUp.CalculateBoardSquare();
				powerUp.CheckForPickupOnSpawn();
			}
		}
		return powerUp;
	}

	internal PowerUp SpawnSpoils(BoardSquare square, PowerUp spoilsPrefab, Team pickupTeam, bool ignoreSpawnSplineForSequence)
	{
		PowerUp powerUp = null;
		if (NetworkServer.active && spoilsPrefab != null)
		{
			Vector3 position = square.ToVector3();
			GameObject powerUpGameObject = Instantiate(spoilsPrefab.gameObject, position, Quaternion.identity);
			powerUp = powerUpGameObject.GetComponent<PowerUp>();
			powerUp.PickupTeam = pickupTeam;
			powerUp.powerUpListener = this;
			m_activePowerUps.Add(powerUp);
			powerUp.transform.parent = transform;
			powerUp.Networkm_isSpoil = true;
			powerUp.Networkm_ignoreSpawnSplineForSequence = ignoreSpawnSplineForSequence;
			NetworkServer.Spawn(powerUpGameObject);
			powerUp.CalculateBoardSquare();
			powerUp.CheckForPickupOnSpawn();
		}
		return powerUp;
	}

	internal List<PowerUp> SpawnSpoilsAroundSquare(
		BoardSquare centerSquare,
		Team forTeam,
		int numToSpawn,
		List<GameObject> powerUpPrefabsToChooseFrom,
		bool canSpawnOnEnemyOccupiedSquare,
		bool canSpawnOnAllyOccupiedSquare,
		StandardPowerUpAbilityModData standardSpoilModData,
		bool ignoreSpawnSplineForSequence,
		int maxBorderSearchLayers = 4)
	{
		List<PowerUp> list = new List<PowerUp>();
		if (numToSpawn < 1
		    || powerUpPrefabsToChooseFrom.Count == 0
		    || centerSquare == null)
		{
			return list;
		}
		List<BoardSquare> squaresToSpawn = Get().FindSquaresToSpawnSpoil(
			centerSquare,
			forTeam,
			numToSpawn,
			canSpawnOnEnemyOccupiedSquare,
			canSpawnOnAllyOccupiedSquare,
			maxBorderSearchLayers);
					
		foreach (BoardSquare square in squaresToSpawn)
		{
			int index = GameplayRandom.Range(0, powerUpPrefabsToChooseFrom.Count);
			if (powerUpPrefabsToChooseFrom[index] == null)
			{
				continue;
			}
			
			PowerUp prefab = powerUpPrefabsToChooseFrom[index].GetComponent<PowerUp>();
			if (prefab == null)
			{
				continue;
			}
			
			PowerUp powerUp = Get().SpawnSpoils(square, prefab, forTeam, ignoreSpawnSplineForSequence);
			if (powerUp == null)
			{
				continue;
			}
			
			if (standardSpoilModData != null)
			{
				PowerUp_Standard_Ability ability = powerUp.GetComponent<PowerUp_Standard_Ability>();
				if (ability != null)
				{
					ability.SetHealAmount(standardSpoilModData.m_healMod.GetModifiedValue(ability.m_healAmount));
					ability.SetTechPointAmount(standardSpoilModData.m_techPointMod.GetModifiedValue(ability.m_techPointsAmount));
				}
			}
			powerUp.CalculateBoardSquare();
			list.Add(powerUp);
		}
		return list;
	}

	internal List<BoardSquare> FindSquaresToSpawnSpoil(
		BoardSquare desiredCenterSquare,
		Team forTeam,
		int numToSpawn,
		bool canSpawnOnEnemyOccupiedSquare,
		bool canSpawnOnAllyOccupiedSquare,
		int maxBorderSearchLayers,
		List<BoardSquare> squaresToExclude = null)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		if (desiredCenterSquare == null)
		{
			return list;
		}
		int num = 0;
		for (int i = 0; i < maxBorderSearchLayers; i++)
		{
			if (num >= numToSpawn)
			{
				break;
			}
			List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(desiredCenterSquare, i, true);
			foreach (BoardSquare square in squaresInBorderLayer)
			{
				if (num >= numToSpawn)
				{
					break;
				}

				if (!square.IsValidForGameplay()
				    || Get().GetPowerUpInPos(square) != null
				    || (squaresToExclude != null && squaresToExclude.Contains(square)))
				{
					continue;
				}
				
				bool isValidSquare = true;
				if (square.occupant != null)
				{
					ActorData actorData = square.occupant.GetComponent<ActorData>();
					isValidSquare = actorData == null
					                || actorData.IgnoreForAbilityHits
					                || canSpawnOnEnemyOccupiedSquare && actorData.GetTeam() != forTeam
					                || canSpawnOnAllyOccupiedSquare && actorData.GetTeam() == forTeam;
				}

				if (isValidSquare)
				{
					list.Add(square);
					num++;
				}
			}
		}
		return list;
	}

	void PowerUp.IPowerUpListener.OnPowerUpDestroyed(PowerUp destroyedPowerUp)
	{
		m_activePowerUps.Remove(destroyedPowerUp);
	}

	PowerUp[] PowerUp.IPowerUpListener.GetActivePowerUps()
	{
		return m_activePowerUps.ToArray();
	}

	void PowerUp.IPowerUpListener.SetSpawningEnabled(bool enabled)
	{
	}

	public PowerUp GetPowerUpInPos(BoardSquare square)
	{
		PowerUp result = null;
		foreach (PowerUp activePowerUp in m_activePowerUps)
		{
			if (activePowerUp.boardSquare == square)
			{
				result = activePowerUp;
			}
		}
		return result;
	}

	void PowerUp.IPowerUpListener.OnTurnTick()
	{
		if (NetworkServer.active)
		{
			foreach (PowerUp powerUp in m_activePowerUps.ToArray())
			{
				powerUp.OnTurnTick();
			}
		}
	}

	bool PowerUp.IPowerUpListener.IsPowerUpSpawnPoint(BoardSquare square)
	{
		return false;
	}

	public void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
	}
}
