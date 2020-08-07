using System.Collections.Generic;
using UnityEngine;

public class SpoilsData : MonoBehaviour
{
	public SpoilsManager.SpoilsType m_spoilsType;

	public PowerUp m_overrideSpoils;

	private PowerUp m_spawnedPowerUp;

	public void SpawnSpoilOnDeath(BoardSquare desiredSquare)
	{
		if (!(SpoilsManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (!(desiredSquare != null))
			{
				return;
			}
			while (true)
			{
				ActorData component = GetComponent<ActorData>();
				if (!GameplayUtils.IsPlayerControlled(component))
				{
					return;
				}
				List<BoardSquare> list = SpoilsManager.Get().FindSquaresToSpawnSpoil(desiredSquare, component.GetEnemyTeam(), 1, true, true, 3);
				if (list.Count > 0)
				{
					if (m_overrideSpoils != null)
					{
						m_spawnedPowerUp = SpoilsManager.Get().SpawnSpoils(list[0], m_overrideSpoils, component.GetEnemyTeam(), false);
					}
					else
					{
						m_spawnedPowerUp = SpoilsManager.Get().SpawnSpoils(list[0], m_spoilsType, component.GetEnemyTeam());
					}
				}
				if (!m_spawnedPowerUp)
				{
					return;
				}
				while (true)
				{
					m_spawnedPowerUp.CalculateBoardSquare();
					return;
				}
			}
		}
	}
}
