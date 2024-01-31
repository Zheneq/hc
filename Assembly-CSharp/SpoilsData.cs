using System.Collections.Generic;
using UnityEngine;

public class SpoilsData : MonoBehaviour
{
	public SpoilsManager.SpoilsType m_spoilsType;
	public PowerUp m_overrideSpoils;

	private PowerUp m_spawnedPowerUp;

	public void SpawnSpoilOnDeath(BoardSquare desiredSquare)
	{
		if (SpoilsManager.Get() == null || desiredSquare == null)
		{
			return;
		}
		ActorData component = GetComponent<ActorData>();
		if (!GameplayUtils.IsPlayerControlled(component))
		{
			return;
		}
		List<BoardSquare> list = SpoilsManager.Get().FindSquaresToSpawnSpoil(
			desiredSquare,
			component.GetEnemyTeam(),
			1,
			true,
			true,
			3);
		if (list.Count > 0)
		{
			m_spawnedPowerUp = m_overrideSpoils != null
				? SpoilsManager.Get().SpawnSpoils(list[0], m_overrideSpoils, component.GetEnemyTeam(), false)
				: SpoilsManager.Get().SpawnSpoils(list[0], m_spoilsType, component.GetEnemyTeam());
		}

		if (m_spawnedPowerUp)
		{
			m_spawnedPowerUp.CalculateBoardSquare();
		}
	}
}
