using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpoilsManager : MonoBehaviour, PowerUp.IPowerUpListener
{
	public PowerUp[] m_heroSpoils;

	public PowerUp[] m_minionSpoils;

	private List<PowerUp> m_activePowerUps;

	private static SpoilsManager s_instance;

	private void Awake()
	{
		SpoilsManager.s_instance = this;
	}

	private void OnDestroy()
	{
		SpoilsManager.s_instance = null;
	}

	private void Start()
	{
		this.m_activePowerUps = new List<PowerUp>();
		PowerUpManager.AddListenerStatic(this);
	}

	public static SpoilsManager Get()
	{
		return SpoilsManager.s_instance;
	}

	private PowerUp PickRandomPowerUpPrefab(SpoilsManager.SpoilsType spoilsType)
	{
		PowerUp result = null;
		if (spoilsType != SpoilsManager.SpoilsType.Hero)
		{
			if (spoilsType != SpoilsManager.SpoilsType.Minion)
			{
			}
			else if (this.m_heroSpoils != null)
			{
				if (this.m_heroSpoils.Length > 0)
				{
					result = this.m_minionSpoils[GameplayRandom.Range(0, this.m_heroSpoils.Length)];
				}
			}
		}
		else if (this.m_heroSpoils != null)
		{
			if (this.m_heroSpoils.Length > 0)
			{
				result = this.m_heroSpoils[GameplayRandom.Range(0, this.m_heroSpoils.Length)];
			}
		}
		return result;
	}

	internal PowerUp SpawnSpoils(BoardSquare square, SpoilsManager.SpoilsType spoilsType, Team pickupTeam)
	{
		PowerUp powerUp = null;
		if (NetworkServer.active)
		{
			PowerUp powerUp2 = this.PickRandomPowerUpPrefab(spoilsType);
			if (powerUp2)
			{
				Vector3 position = square.ToVector3();
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(powerUp2.gameObject, position, Quaternion.identity);
				powerUp = gameObject.GetComponent<PowerUp>();
				powerUp.PickupTeam = pickupTeam;
				powerUp.powerUpListener = this;
				this.m_activePowerUps.Add(powerUp);
				powerUp.transform.parent = base.transform;
				powerUp.Networkm_isSpoil = true;
				NetworkServer.Spawn(gameObject);
				powerUp.CalculateBoardSquare();
				powerUp.CheckForPickupOnSpawn();
			}
		}
		return powerUp;
	}

	internal PowerUp SpawnSpoils(BoardSquare square, PowerUp spoilsPrefab, Team pickupTeam, bool ignoreSpawnSplineForSequence)
	{
		PowerUp powerUp = null;
		if (NetworkServer.active && spoilsPrefab)
		{
			Vector3 position = square.ToVector3();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(spoilsPrefab.gameObject, position, Quaternion.identity);
			powerUp = gameObject.GetComponent<PowerUp>();
			powerUp.PickupTeam = pickupTeam;
			powerUp.powerUpListener = this;
			this.m_activePowerUps.Add(powerUp);
			powerUp.transform.parent = base.transform;
			powerUp.Networkm_isSpoil = true;
			powerUp.Networkm_ignoreSpawnSplineForSequence = ignoreSpawnSplineForSequence;
			NetworkServer.Spawn(gameObject);
			powerUp.CalculateBoardSquare();
			powerUp.CheckForPickupOnSpawn();
		}
		return powerUp;
	}

	internal List<PowerUp> SpawnSpoilsAroundSquare(BoardSquare centerSquare, Team forTeam, int numToSpawn, List<GameObject> powerUpPrefabsToChooseFrom, bool canSpawnOnEnemyOccupiedSquare, bool canSpawnOnAllyOccupiedSquare, StandardPowerUpAbilityModData standardSpoilModData, bool ignoreSpawnSplineForSequence, int maxBorderSearchLayers = 4)
	{
		List<PowerUp> list = new List<PowerUp>();
		if (numToSpawn >= 1)
		{
			if (powerUpPrefabsToChooseFrom.Count != 0)
			{
				if (!(centerSquare == null))
				{
					List<BoardSquare> list2 = SpoilsManager.Get().FindSquaresToSpawnSpoil(centerSquare, forTeam, numToSpawn, canSpawnOnEnemyOccupiedSquare, canSpawnOnAllyOccupiedSquare, maxBorderSearchLayers, null);
					using (List<BoardSquare>.Enumerator enumerator = list2.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BoardSquare square = enumerator.Current;
							int index = GameplayRandom.Range(0, powerUpPrefabsToChooseFrom.Count);
							if (powerUpPrefabsToChooseFrom[index] != null)
							{
								PowerUp component = powerUpPrefabsToChooseFrom[index].GetComponent<PowerUp>();
								if (component != null)
								{
									PowerUp powerUp = SpoilsManager.Get().SpawnSpoils(square, component, forTeam, ignoreSpawnSplineForSequence);
									if (powerUp != null)
									{
										if (standardSpoilModData != null)
										{
											PowerUp_Standard_Ability component2 = powerUp.GetComponent<PowerUp_Standard_Ability>();
											if (component2 != null)
											{
												component2.SetHealAmount(standardSpoilModData.m_healMod.GetModifiedValue(component2.m_healAmount));
												component2.SetTechPointAmount(standardSpoilModData.m_techPointMod.GetModifiedValue(component2.m_techPointsAmount));
											}
										}
										powerUp.CalculateBoardSquare();
										list.Add(powerUp);
									}
								}
							}
						}
					}
					return list;
				}
			}
		}
		return list;
	}

	internal List<BoardSquare> FindSquaresToSpawnSpoil(BoardSquare desiredCenterSquare, Team forTeam, int numToSpawn, bool canSpawnOnEnemyOccupiedSquare, bool canSpawnOnAllyOccupiedSquare, int maxBorderSearchLayers, List<BoardSquare> squaresToExclude = null)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		if (desiredCenterSquare == null)
		{
			return list;
		}
		int num = 0;
		int i = 0;
		while (i < maxBorderSearchLayers)
		{
			if (num >= numToSpawn)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					return list;
				}
			}
			else
			{
				List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(desiredCenterSquare, i, true);
				int j = 0;
				while (j < squaresInBorderLayer.Count)
				{
					if (num >= numToSpawn)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							goto IL_183;
						}
					}
					else
					{
						BoardSquare boardSquare = squaresInBorderLayer[j];
						if (boardSquare.IsBaselineHeight())
						{
							if (SpoilsManager.Get().GetPowerUpInPos(boardSquare) == null)
							{
								if (squaresToExclude != null)
								{
									if (squaresToExclude.Contains(boardSquare))
									{
										goto IL_160;
									}
								}
								bool flag2;
								if (boardSquare.occupant != null)
								{
									ActorData component = boardSquare.occupant.GetComponent<ActorData>();
									if (component == null)
									{
										goto IL_13E;
									}
									if (component.IgnoreForAbilityHits)
									{
										goto IL_13E;
									}
									if (canSpawnOnEnemyOccupiedSquare)
									{
										if (component.GetTeam() != forTeam)
										{
											goto IL_13E;
										}
									}
									bool flag;
									if (canSpawnOnAllyOccupiedSquare)
									{
										flag = (component.GetTeam() == forTeam);
									}
									else
									{
										flag = false;
									}
									IL_13F:
									flag2 = flag;
									goto IL_146;
									IL_13E:
									flag = true;
									goto IL_13F;
								}
								flag2 = true;
								IL_146:
								if (flag2)
								{
									list.Add(boardSquare);
									num++;
								}
							}
						}
						IL_160:
						j++;
					}
				}
				IL_183:
				i++;
			}
		}
		return list;
	}

	void PowerUp.IPowerUpListener.OnPowerUpDestroyed(PowerUp destroyedPowerUp)
	{
		this.m_activePowerUps.Remove(destroyedPowerUp);
	}

	PowerUp[] PowerUp.IPowerUpListener.GetActivePowerUps()
	{
		return this.m_activePowerUps.ToArray();
	}

	void PowerUp.IPowerUpListener.SetSpawningEnabled(bool enabled)
	{
	}

	public PowerUp GetPowerUpInPos(BoardSquare square)
	{
		PowerUp result = null;
		foreach (PowerUp powerUp in this.m_activePowerUps)
		{
			if (powerUp.boardSquare == square)
			{
				result = powerUp;
			}
		}
		return result;
	}

	void PowerUp.IPowerUpListener.OnTurnTick()
	{
		if (NetworkServer.active)
		{
			PowerUp[] array = this.m_activePowerUps.ToArray();
			foreach (PowerUp powerUp in array)
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

	public enum SpoilsType
	{
		None,
		Hero,
		Minion
	}
}
