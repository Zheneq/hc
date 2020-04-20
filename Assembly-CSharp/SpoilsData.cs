using System;
using System.Collections.Generic;
using UnityEngine;

public class SpoilsData : MonoBehaviour
{
	public SpoilsManager.SpoilsType m_spoilsType;

	public PowerUp m_overrideSpoils;

	private PowerUp m_spawnedPowerUp;

	public void SpawnSpoilOnDeath(BoardSquare desiredSquare)
	{
		if (SpoilsManager.Get() != null)
		{
			if (desiredSquare != null)
			{
				ActorData component = base.GetComponent<ActorData>();
				if (!GameplayUtils.IsPlayerControlled(component))
				{
					return;
				}
				List<BoardSquare> list = SpoilsManager.Get().FindSquaresToSpawnSpoil(desiredSquare, component.GetOpposingTeam(), 1, true, true, 3, null);
				if (list.Count > 0)
				{
					if (this.m_overrideSpoils != null)
					{
						this.m_spawnedPowerUp = SpoilsManager.Get().SpawnSpoils(list[0], this.m_overrideSpoils, component.GetOpposingTeam(), false);
					}
					else
					{
						this.m_spawnedPowerUp = SpoilsManager.Get().SpawnSpoils(list[0], this.m_spoilsType, component.GetOpposingTeam());
					}
				}
				if (this.m_spawnedPowerUp)
				{
					this.m_spawnedPowerUp.CalculateBoardSquare();
				}
			}
		}
	}
}
