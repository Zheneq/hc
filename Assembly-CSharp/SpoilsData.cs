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
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(desiredSquare != null))
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				ActorData component = GetComponent<ActorData>();
				if (!GameplayUtils.IsPlayerControlled(component))
				{
					return;
				}
				List<BoardSquare> list = SpoilsManager.Get().FindSquaresToSpawnSpoil(desiredSquare, component.GetOpposingTeam(), 1, true, true, 3);
				if (list.Count > 0)
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
					if (m_overrideSpoils != null)
					{
						m_spawnedPowerUp = SpoilsManager.Get().SpawnSpoils(list[0], m_overrideSpoils, component.GetOpposingTeam(), false);
					}
					else
					{
						m_spawnedPowerUp = SpoilsManager.Get().SpawnSpoils(list[0], m_spoilsType, component.GetOpposingTeam());
					}
				}
				if (!m_spawnedPowerUp)
				{
					return;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					m_spawnedPowerUp.CalculateBoardSquare();
					return;
				}
			}
		}
	}
}
