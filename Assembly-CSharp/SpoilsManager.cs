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
		PowerUp result = null;
		if (spoilsType != SpoilsType.Hero)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (spoilsType != SpoilsType.Minion)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			else if (m_heroSpoils != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_heroSpoils.Length > 0)
				{
					result = m_minionSpoils[GameplayRandom.Range(0, m_heroSpoils.Length)];
				}
			}
		}
		else if (m_heroSpoils != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_heroSpoils.Length > 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				result = m_heroSpoils[GameplayRandom.Range(0, m_heroSpoils.Length)];
			}
		}
		return result;
	}

	internal PowerUp SpawnSpoils(BoardSquare square, SpoilsType spoilsType, Team pickupTeam)
	{
		PowerUp powerUp = null;
		if (NetworkServer.active)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			PowerUp powerUp2 = PickRandomPowerUpPrefab(spoilsType);
			if ((bool)powerUp2)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 position = square.ToVector3();
				GameObject gameObject = Object.Instantiate(powerUp2.gameObject, position, Quaternion.identity);
				powerUp = gameObject.GetComponent<PowerUp>();
				powerUp.PickupTeam = pickupTeam;
				powerUp.powerUpListener = this;
				m_activePowerUps.Add(powerUp);
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
		if (NetworkServer.active && (bool)spoilsPrefab)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Vector3 position = square.ToVector3();
			GameObject gameObject = Object.Instantiate(spoilsPrefab.gameObject, position, Quaternion.identity);
			powerUp = gameObject.GetComponent<PowerUp>();
			powerUp.PickupTeam = pickupTeam;
			powerUp.powerUpListener = this;
			m_activePowerUps.Add(powerUp);
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
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (powerUpPrefabsToChooseFrom.Count != 0)
			{
				if (!(centerSquare == null))
				{
					List<BoardSquare> list2 = Get().FindSquaresToSpawnSpoil(centerSquare, forTeam, numToSpawn, canSpawnOnEnemyOccupiedSquare, canSpawnOnAllyOccupiedSquare, maxBorderSearchLayers);
					using (List<BoardSquare>.Enumerator enumerator = list2.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BoardSquare current = enumerator.Current;
							int index = GameplayRandom.Range(0, powerUpPrefabsToChooseFrom.Count);
							if (powerUpPrefabsToChooseFrom[index] != null)
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
								PowerUp component = powerUpPrefabsToChooseFrom[index].GetComponent<PowerUp>();
								if (component != null)
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									PowerUp powerUp = Get().SpawnSpoils(current, component, forTeam, ignoreSpawnSplineForSequence);
									if (powerUp != null)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										if (standardSpoilModData != null)
										{
											while (true)
											{
												switch (3)
												{
												case 0:
													continue;
												}
												break;
											}
											PowerUp_Standard_Ability component2 = powerUp.GetComponent<PowerUp_Standard_Ability>();
											if (component2 != null)
											{
												while (true)
												{
													switch (1)
													{
													case 0:
														continue;
													}
													break;
												}
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
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return list;
							}
						}
					}
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return list;
				}
			}
		}
		int num = 0;
		for (int i = 0; i < maxBorderSearchLayers; i++)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (num < numToSpawn)
			{
				List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(desiredCenterSquare, i, true);
				for (int j = 0; j < squaresInBorderLayer.Count; j++)
				{
					BoardSquare boardSquare;
					int num2;
					bool flag;
					if (num < numToSpawn)
					{
						boardSquare = squaresInBorderLayer[j];
						if (!boardSquare.IsBaselineHeight())
						{
							continue;
						}
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!(Get().GetPowerUpInPos(boardSquare) == null))
						{
							continue;
						}
						if (squaresToExclude != null)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (squaresToExclude.Contains(boardSquare))
							{
								continue;
							}
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						flag = false;
						if (boardSquare.occupant != null)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							ActorData component = boardSquare.occupant.GetComponent<ActorData>();
							if (!(component == null))
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!component.IgnoreForAbilityHits)
								{
									if (canSpawnOnEnemyOccupiedSquare)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										if (component.GetTeam() != forTeam)
										{
											goto IL_013e;
										}
										while (true)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									if (canSpawnOnAllyOccupiedSquare)
									{
										while (true)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										num2 = ((component.GetTeam() == forTeam) ? 1 : 0);
									}
									else
									{
										num2 = 0;
									}
									goto IL_013f;
								}
							}
							goto IL_013e;
						}
						flag = true;
						goto IL_0146;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					break;
					IL_0146:
					if (flag)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						list.Add(boardSquare);
						num++;
					}
					continue;
					IL_013e:
					num2 = 1;
					goto IL_013f;
					IL_013f:
					flag = ((byte)num2 != 0);
					goto IL_0146;
				}
				continue;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
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
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				result = activePowerUp;
			}
		}
		return result;
	}

	void PowerUp.IPowerUpListener.OnTurnTick()
	{
		if (NetworkServer.active)
		{
			PowerUp[] array = m_activePowerUps.ToArray();
			PowerUp[] array2 = array;
			foreach (PowerUp powerUp in array2)
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
