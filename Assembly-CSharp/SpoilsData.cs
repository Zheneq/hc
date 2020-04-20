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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpoilsData.SpawnSpoilOnDeath(BoardSquare)).MethodHandle;
			}
			if (desiredSquare != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				ActorData component = base.GetComponent<ActorData>();
				if (!GameplayUtils.IsPlayerControlled(component))
				{
					return;
				}
				List<BoardSquare> list = SpoilsManager.Get().FindSquaresToSpawnSpoil(desiredSquare, component.GetOpposingTeam(), 1, true, true, 3, null);
				if (list.Count > 0)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
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
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_spawnedPowerUp.CalculateBoardSquare();
				}
			}
		}
	}
}
