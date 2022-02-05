// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only, missing in reactor
#if SERVER
public class SpoilSpawnDataForAbilityHit
{
	public SpawnSquareSource m_spawnSquareSourceType;
	public ActorData m_targetActor;
	public BoardSquare m_boardSquare;
	public Team m_team;
	public List<GameObject> m_powerupPrefabs;
	public int m_numToSpawn = 1;
	public bool m_canSpawnOnEnemyOccupiedSquare;
	public bool m_canSpawnOnAllyOccupiedSquare;
	public int m_duration;
	public bool m_ignoreSpawnSplineForSequence;
	public StandardPowerUpAbilityModData m_spoilMod;

	public SpoilSpawnDataForAbilityHit(ActorData targetActor, Team team, SpoilsSpawnData spawnData)
	{
		InitSpoilSpawnData(SpawnSquareSource.FromTarget, targetActor, null, team, spawnData);
	}

	public SpoilSpawnDataForAbilityHit(BoardSquare square, Team team, SpoilsSpawnData spawnData)
	{
		InitSpoilSpawnData(SpawnSquareSource.FromBoardSquare, null, square, team, spawnData);
	}

	public SpoilSpawnDataForAbilityHit(ActorData targetActor, Team team, List<GameObject> prefabs)
	{
		InitSpoilSpawnData(SpawnSquareSource.FromTarget, targetActor, null, team, prefabs);
	}

	public SpoilSpawnDataForAbilityHit(BoardSquare square, Team team, List<GameObject> prefabs)
	{
		InitSpoilSpawnData(SpawnSquareSource.FromBoardSquare, null, square, team, prefabs);
	}

	private void InitSpoilSpawnData(SpawnSquareSource spawnSquareSourceType, ActorData targetActor, BoardSquare square, Team team, SpoilsSpawnData spawnData)
	{
		m_spawnSquareSourceType = spawnSquareSourceType;
		m_targetActor = targetActor;
		m_boardSquare = square;
		m_team = team;
		m_powerupPrefabs = spawnData.m_powerupPrefabs;
		m_numToSpawn = spawnData.m_numToSpawn;
		m_canSpawnOnEnemyOccupiedSquare = spawnData.m_canSpawnOnEnemyOccupiedSquare;
		m_canSpawnOnAllyOccupiedSquare = spawnData.m_canSpawnOnAllyOccupiedSquare;
		m_duration = spawnData.m_duration;
	}

	private void InitSpoilSpawnData(SpawnSquareSource spawnSquareSourceType, ActorData targetActor, BoardSquare square, Team team, List<GameObject> prefabs)
	{
		m_spawnSquareSourceType = spawnSquareSourceType;
		m_targetActor = targetActor;
		m_boardSquare = square;
		m_team = team;
		m_powerupPrefabs = prefabs;
	}

	public BoardSquare GetDesiredSpawnSquare()
	{
		BoardSquare result = null;
		if (m_spawnSquareSourceType == SpawnSquareSource.FromTarget && m_targetActor != null)
		{
			result = m_targetActor.GetCurrentBoardSquare();
		}
		else if (m_spawnSquareSourceType == SpawnSquareSource.FromBoardSquare)
		{
			result = m_boardSquare;
		}
		return result;
	}

	public bool CanSpawnSpoils()
	{
		return m_numToSpawn > 0 && m_powerupPrefabs.Count > 0;
	}

	public void SpawnSpoils(ActorData creator, Ability creatorAbility)
	{
		BoardSquare desiredSpawnSquare = GetDesiredSpawnSquare();
		if (desiredSpawnSquare != null)
		{
			foreach (PowerUp powerUp in SpoilsManager.Get().SpawnSpoilsAroundSquare(desiredSpawnSquare, m_team, m_numToSpawn, m_powerupPrefabs, m_canSpawnOnEnemyOccupiedSquare, m_canSpawnOnAllyOccupiedSquare, m_spoilMod, m_ignoreSpawnSplineForSequence, 4))
			{
				powerUp.SetDuration(m_duration);
				powerUp.SetCreator(creator, creatorAbility);
			}
		}
	}

	public PowerUp ChooseRandomPowerupComponent()
	{
		if (m_powerupPrefabs != null && m_powerupPrefabs.Count > 0)
		{
			int index = GameplayRandom.Range(0, m_powerupPrefabs.Count);
			if (m_powerupPrefabs[index] != null)
			{
				PowerUp component = m_powerupPrefabs[index].GetComponent<PowerUp>();
				if (component != null)
				{
					return component;
				}
			}
		}
		return null;
	}

	public SpoilSpawnDataForAbilityHit GetShallowCopy()
	{
		return (SpoilSpawnDataForAbilityHit)MemberwiseClone();
	}

	public enum SpawnSquareSource
	{
		FromTarget,
		FromBoardSquare
	}
}
#endif
