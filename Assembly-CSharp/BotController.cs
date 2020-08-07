using System;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
	public float m_combatRange = 7f;

	public float m_idealRange = 5f;

	public float m_retreatFromRange = 15f;

	[HideInInspector]
	public Stack<NPCBrain> previousBrainStack;

	private int m_aiStartedForTurn;

	private void Start()
	{
		ActorData component = base.GetComponent<ActorData>();
		BotDifficulty botDifficulty = BotDifficulty.Expert;
		bool flag = false;
		foreach (LobbyPlayerInfo lobbyPlayerInfo in GameManager.Get().TeamInfo.TeamPlayerInfo)
		{
			if (!(component.PlayerData == null))
			{
				if (component.PlayerData.LookupDetails() == null)
				{
				}
				else if (lobbyPlayerInfo.PlayerId == component.PlayerData.LookupDetails().m_lobbyPlayerInfoId)
				{
					botDifficulty = lobbyPlayerInfo.Difficulty;
					flag = lobbyPlayerInfo.BotCanTaunt;
					break;
				}
			}
		}
		this.previousBrainStack = new Stack<NPCBrain>();
		if (base.GetComponent<NPCBrain>() == null)
		{
			if (!(component.GetObjectName() == "Sniper") && !(component.GetObjectName() == "RageBeast") && !(component.GetObjectName() == "Scoundrel"))
			{
				if (!(component.GetObjectName() == "RobotAnimal"))
				{
					if (!(component.GetObjectName() == "NanoSmith"))
					{
						if (!(component.GetObjectName() == "Thief"))
						{
							if (!(component.GetObjectName() == "BattleMonk") && !(component.GetObjectName() == "BazookaGirl"))
							{
								if (!(component.GetObjectName() == "SpaceMarine"))
								{
									if (!(component.GetObjectName() == "Gremlins"))
									{
										if (!(component.GetObjectName() == "Tracker"))
										{
											if (!(component.GetObjectName() == "DigitalSorceress"))
											{
												if (!(component.GetObjectName() == "Spark") && !(component.GetObjectName() == "Claymore") && !(component.GetObjectName() == "Rampart"))
												{
													if (!(component.GetObjectName() == "Trickster"))
													{
														if (!(component.GetObjectName() == "Blaster"))
														{
															if (!(component.GetObjectName() == "FishMan"))
															{
																if (!(component.GetObjectName() == "Thief"))
																{
																	if (!(component.GetObjectName() == "Soldier"))
																	{
																		if (!(component.GetObjectName() == "Exo"))
																		{
																			if (!(component.GetObjectName() == "Martyr") && !(component.GetObjectName() == "Sensei"))
																			{
																				if (!(component.GetObjectName() == "TeleportingNinja") && !(component.GetObjectName() == "Manta"))
																				{
																					if (!(component.GetObjectName() == "Valkyrie"))
																					{
																						if (!(component.GetObjectName() == "Archer"))
																						{
																							if (!(component.GetObjectName() == "Samurai"))
																							{
																								if (!(component.GetObjectName() == "Cleric"))
																								{
																									if (!(component.GetObjectName() == "Neko"))
																									{
																										if (!(component.GetObjectName() == "Scamp"))
																										{
																											if (!(component.GetObjectName() == "Dino"))
																											{
																												if (!(component.GetObjectName() == "Iceborg"))
																												{
																													if (!(component.GetObjectName() == "Fireborg"))
																													{
																														Log.Info("Using Generic AI for {0}", new object[]
																														{
																															component.GetObjectName()
																														});
																														return;
																													}
																												}
																											}
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			NPCBrain_Adaptive.Create(this, component.transform, botDifficulty, flag);
			Log.Info("Making Adaptive AI for {0} at difficulty {1}, can taunt: {2}", new object[]
			{
				component.GetObjectName(),
				botDifficulty.ToString(),
				flag
			});
			if (this.IAmTheOnlyBotOnATwoPlayerTeam(component))
			{
				component.GetComponent<NPCBrain_Adaptive>().SendDecisionToTeamChat(true);
			}
		}
	}

	public unsafe BoardSquare GetClosestEnemyPlayerSquare(bool includeInvisibles, out int numEnemiesInRange)
	{
		numEnemiesInRange = 0;
		ActorData component = base.GetComponent<ActorData>();
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(component.GetEnemyTeam());
		BoardSquare currentBoardSquare = component.GetCurrentBoardSquare();
		BoardSquare boardSquare = null;
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				BoardSquare currentBoardSquare2 = actorData.GetCurrentBoardSquare();
				if (!actorData.IsDead())
				{
					if (currentBoardSquare2 == null)
					{
					}
					else
					{
						float num = currentBoardSquare.HorizontalDistanceOnBoardTo(currentBoardSquare2);
						if (num <= this.m_combatRange)
						{
							numEnemiesInRange++;
						}
						if (!includeInvisibles)
						{
							if (!component.GetFogOfWar().IsVisible(currentBoardSquare2))
							{
								continue;
							}
						}
						if (boardSquare == null)
						{
							boardSquare = currentBoardSquare2;
						}
						else
						{
							float num2 = currentBoardSquare.HorizontalDistanceOnBoardTo(boardSquare);
							if (num < num2)
							{
								boardSquare = currentBoardSquare2;
							}
						}
					}
				}
			}
		}
		return boardSquare;
	}

	public BoardSquare GetRetreatSquare()
	{
		ActorData component = base.GetComponent<ActorData>();
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(component.GetEnemyTeam());
		BoardSquare currentBoardSquare = component.GetCurrentBoardSquare();
		Vector3 a = new Vector3(0f, 0f, 0f);
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				BoardSquare currentBoardSquare2 = actorData.GetCurrentBoardSquare();
				if (!actorData.IsDead())
				{
					if (currentBoardSquare2 == null)
					{
					}
					else
					{
						float num = currentBoardSquare.HorizontalDistanceOnBoardTo(currentBoardSquare2);
						if (num <= this.m_retreatFromRange)
						{
							Vector3 b = currentBoardSquare2.ToVector3() - currentBoardSquare.ToVector3();
							b.Normalize();
							a += b;
						}
					}
				}
			}
		}
		Vector3 a2 = -a;
		a2.Normalize();
		Vector3 vector = currentBoardSquare.ToVector3() + a2 * this.m_retreatFromRange;
		return Board.Get()._0013(vector.x, vector.z);
	}

	public BoardSquare GetAdvanceSquare()
	{
		int num;
		BoardSquare closestEnemyPlayerSquare = this.GetClosestEnemyPlayerSquare(true, out num);
		if (closestEnemyPlayerSquare == null)
		{
			return null;
		}
		Vector3 a = closestEnemyPlayerSquare.ToVector3();
		ActorData component = base.GetComponent<ActorData>();
		BoardSquare currentBoardSquare = component.GetCurrentBoardSquare();
		Vector3 vector = currentBoardSquare.ToVector3();
		Vector3 vector2 = a - vector;
		if (num > 1)
		{
			float magnitude = vector2.magnitude;
			if (magnitude > this.m_idealRange)
			{
				vector2.Normalize();
				vector2 *= this.m_idealRange;
			}
		}
		Vector3 vector3 = Vector3.zero;
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(component.GetTeam());
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (!actorData.IsDead() || actorData == component)
				{
					BoardSquare currentBoardSquare2 = actorData.GetCurrentBoardSquare();
					if (currentBoardSquare2 != null && currentBoardSquare.HorizontalDistanceOnBoardTo(currentBoardSquare2) < this.m_idealRange)
					{
						Vector3 a2 = currentBoardSquare2.ToVector3() - vector;
						a2.Normalize();
						vector3 -= a2 * 1.5f;
					}
				}
			}
		}
		Vector3 vector4 = vector + vector2 + vector3;
		BoardSquare boardSquareUnsafe = Board.Get().GetClosestSquareToPosition(vector4.x, vector4.z);
		return Board.Get()._0018(boardSquareUnsafe, null);
	}

	public void SelectBotAbilityMods()
	{
		NPCBrain component = base.GetComponent<NPCBrain>();
		if (component != null)
		{
			component.SelectBotAbilityMods();
		}
		else
		{
			this.SelectBotAbilityMods_Brainless();
		}
	}

	public void SelectBotCards()
	{
		NPCBrain component = base.GetComponent<NPCBrain>();
		if (component != null)
		{
			component.SelectBotCards();
		}
		else
		{
			this.SelectBotCards_Brainless();
		}
	}

	public void SelectBotAbilityMods_Brainless()
	{
		ActorData component = base.GetComponent<ActorData>();
		AbilityData abilityData = component.GetAbilityData();
		List<Ability> abilitiesAsList = abilityData.GetAbilitiesAsList();
		CharacterModInfo selectedMods = default(CharacterModInfo);
		int num = 0;
		using (List<Ability>.Enumerator enumerator = abilitiesAsList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Ability ability = enumerator.Current;
				AbilityMod defaultModForAbility = AbilityModManager.Get().GetDefaultModForAbility(ability);
				int num2;
				if (defaultModForAbility != null)
				{
					num2 = defaultModForAbility.m_abilityScopeId;
				}
				else
				{
					num2 = -1;
				}
				int mod = num2;
				selectedMods.SetModForAbility(num, mod);
				num++;
			}
		}
		component.m_selectedMods = selectedMods;
	}

	public void SelectBotCards_Brainless()
	{
		ActorData component = base.GetComponent<ActorData>();
		CharacterCardInfo cardInfo = default(CharacterCardInfo);
		cardInfo.PrepCard = CardManagerData.Get().GetDefaultPrepCardType();
		cardInfo.CombatCard = CardManagerData.Get().GetDefaultCombatCardType();
		cardInfo.DashCard = CardManagerData.Get().GetDefaultDashCardType();
		CardManager.Get().SetDeckAndGiveCards(component, cardInfo, false);
	}

	public bool IAmTheOnlyBotOnATwoPlayerTeam(ActorData actorData)
	{
		PlayerDetails playerDetails = actorData.PlayerData.LookupDetails();
		if (playerDetails == null)
		{
			return false;
		}
		bool flag = false;
		using (List<LobbyPlayerInfo>.Enumerator enumerator = GameManager.Get().TeamInfo.TeamPlayerInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
				if (lobbyPlayerInfo.TeamId != actorData.GetTeam())
				{
				}
				else if (lobbyPlayerInfo.PlayerId == playerDetails.m_lobbyPlayerInfoId)
				{
				}
				else
				{
					if (flag)
					{
						return false;
					}
					if (lobbyPlayerInfo.IsAIControlled)
					{
						return false;
					}
					flag = true;
				}
			}
		}
		return true;
	}
}
