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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(BotController.Start()).MethodHandle;
				}
				if (component.PlayerData.LookupDetails() == null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				else if (lobbyPlayerInfo.PlayerId == component.PlayerData.LookupDetails().m_lobbyPlayerInfoId)
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
					botDifficulty = lobbyPlayerInfo.Difficulty;
					flag = lobbyPlayerInfo.BotCanTaunt;
					break;
				}
			}
		}
		this.previousBrainStack = new Stack<NPCBrain>();
		if (base.GetComponent<NPCBrain>() == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!(component.\u0012() == "Sniper") && !(component.\u0012() == "RageBeast") && !(component.\u0012() == "Scoundrel"))
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(component.\u0012() == "RobotAnimal"))
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(component.\u0012() == "NanoSmith"))
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!(component.\u0012() == "Thief"))
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!(component.\u0012() == "BattleMonk") && !(component.\u0012() == "BazookaGirl"))
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!(component.\u0012() == "SpaceMarine"))
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
									if (!(component.\u0012() == "Gremlins"))
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
										if (!(component.\u0012() == "Tracker"))
										{
											for (;;)
											{
												switch (3)
												{
												case 0:
													continue;
												}
												break;
											}
											if (!(component.\u0012() == "DigitalSorceress"))
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
												if (!(component.\u0012() == "Spark") && !(component.\u0012() == "Claymore") && !(component.\u0012() == "Rampart"))
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
													if (!(component.\u0012() == "Trickster"))
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
														if (!(component.\u0012() == "Blaster"))
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
															if (!(component.\u0012() == "FishMan"))
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
																if (!(component.\u0012() == "Thief"))
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
																	if (!(component.\u0012() == "Soldier"))
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
																		if (!(component.\u0012() == "Exo"))
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
																			if (!(component.\u0012() == "Martyr") && !(component.\u0012() == "Sensei"))
																			{
																				for (;;)
																				{
																					switch (5)
																					{
																					case 0:
																						continue;
																					}
																					break;
																				}
																				if (!(component.\u0012() == "TeleportingNinja") && !(component.\u0012() == "Manta"))
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
																					if (!(component.\u0012() == "Valkyrie"))
																					{
																						for (;;)
																						{
																							switch (5)
																							{
																							case 0:
																								continue;
																							}
																							break;
																						}
																						if (!(component.\u0012() == "Archer"))
																						{
																							for (;;)
																							{
																								switch (3)
																								{
																								case 0:
																									continue;
																								}
																								break;
																							}
																							if (!(component.\u0012() == "Samurai"))
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
																								if (!(component.\u0012() == "Cleric"))
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
																									if (!(component.\u0012() == "Neko"))
																									{
																										for (;;)
																										{
																											switch (5)
																											{
																											case 0:
																												continue;
																											}
																											break;
																										}
																										if (!(component.\u0012() == "Scamp"))
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
																											if (!(component.\u0012() == "Dino"))
																											{
																												for (;;)
																												{
																													switch (7)
																													{
																													case 0:
																														continue;
																													}
																													break;
																												}
																												if (!(component.\u0012() == "Iceborg"))
																												{
																													for (;;)
																													{
																														switch (7)
																														{
																														case 0:
																															continue;
																														}
																														break;
																													}
																													if (!(component.\u0012() == "Fireborg"))
																													{
																														Log.Info("Using Generic AI for {0}", new object[]
																														{
																															component.\u0012()
																														});
																														return;
																													}
																													for (;;)
																													{
																														switch (6)
																														{
																														case 0:
																															continue;
																														}
																														break;
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
				component.\u0012(),
				botDifficulty.ToString(),
				flag
			});
			if (this.IAmTheOnlyBotOnATwoPlayerTeam(component))
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
				component.GetComponent<NPCBrain_Adaptive>().SendDecisionToTeamChat(true);
			}
		}
	}

	public unsafe BoardSquare GetClosestEnemyPlayerSquare(bool includeInvisibles, out int numEnemiesInRange)
	{
		numEnemiesInRange = 0;
		ActorData component = base.GetComponent<ActorData>();
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(component.\u0012());
		BoardSquare boardSquare = component.\u0012();
		BoardSquare boardSquare2 = null;
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				BoardSquare boardSquare3 = actorData.\u0012();
				if (!actorData.\u000E())
				{
					if (boardSquare3 == null)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(BotController.GetClosestEnemyPlayerSquare(bool, int*)).MethodHandle;
						}
					}
					else
					{
						float num = boardSquare.HorizontalDistanceOnBoardTo(boardSquare3);
						if (num <= this.m_combatRange)
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
							numEnemiesInRange++;
						}
						if (!includeInvisibles)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!component.\u000E().IsVisible(boardSquare3))
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
								continue;
							}
						}
						if (boardSquare2 == null)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							boardSquare2 = boardSquare3;
						}
						else
						{
							float num2 = boardSquare.HorizontalDistanceOnBoardTo(boardSquare2);
							if (num < num2)
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
								boardSquare2 = boardSquare3;
							}
						}
					}
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return boardSquare2;
	}

	public BoardSquare GetRetreatSquare()
	{
		ActorData component = base.GetComponent<ActorData>();
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(component.\u0012());
		BoardSquare boardSquare = component.\u0012();
		Vector3 a = new Vector3(0f, 0f, 0f);
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				BoardSquare boardSquare2 = actorData.\u0012();
				if (!actorData.\u000E())
				{
					if (boardSquare2 == null)
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
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(BotController.GetRetreatSquare()).MethodHandle;
						}
					}
					else
					{
						float num = boardSquare.HorizontalDistanceOnBoardTo(boardSquare2);
						if (num <= this.m_retreatFromRange)
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
							Vector3 b = boardSquare2.ToVector3() - boardSquare.ToVector3();
							b.Normalize();
							a += b;
						}
					}
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		Vector3 a2 = -a;
		a2.Normalize();
		Vector3 vector = boardSquare.ToVector3() + a2 * this.m_retreatFromRange;
		return Board.\u000E().\u0013(vector.x, vector.z);
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
		BoardSquare boardSquare = component.\u0012();
		Vector3 vector = boardSquare.ToVector3();
		Vector3 vector2 = a - vector;
		if (num > 1)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BotController.GetAdvanceSquare()).MethodHandle;
			}
			float magnitude = vector2.magnitude;
			if (magnitude > this.m_idealRange)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				vector2.Normalize();
				vector2 *= this.m_idealRange;
			}
		}
		Vector3 vector3 = Vector3.zero;
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(component.\u000E());
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (!actorData.\u000E() || actorData == component)
				{
					BoardSquare boardSquare2 = actorData.\u0012();
					if (boardSquare2 != null && boardSquare.HorizontalDistanceOnBoardTo(boardSquare2) < this.m_idealRange)
					{
						Vector3 a2 = boardSquare2.ToVector3() - vector;
						a2.Normalize();
						vector3 -= a2 * 1.5f;
					}
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		Vector3 vector4 = vector + vector2 + vector3;
		BoardSquare u001D = Board.\u000E().\u0015(vector4.x, vector4.z);
		return Board.\u000E().\u0018(u001D, null);
	}

	public void SelectBotAbilityMods()
	{
		NPCBrain component = base.GetComponent<NPCBrain>();
		if (component != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BotController.SelectBotAbilityMods()).MethodHandle;
			}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BotController.SelectBotCards()).MethodHandle;
			}
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
		AbilityData abilityData = component.\u000E();
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
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(BotController.SelectBotAbilityMods_Brainless()).MethodHandle;
					}
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BotController.IAmTheOnlyBotOnATwoPlayerTeam(ActorData)).MethodHandle;
			}
			return false;
		}
		bool flag = false;
		using (List<LobbyPlayerInfo>.Enumerator enumerator = GameManager.Get().TeamInfo.TeamPlayerInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
				if (lobbyPlayerInfo.TeamId != actorData.\u000E())
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				else if (lobbyPlayerInfo.PlayerId == playerDetails.m_lobbyPlayerInfoId)
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
				}
				else
				{
					if (flag)
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
						return false;
					}
					if (lobbyPlayerInfo.IsAIControlled)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						return false;
					}
					flag = true;
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return true;
	}
}
