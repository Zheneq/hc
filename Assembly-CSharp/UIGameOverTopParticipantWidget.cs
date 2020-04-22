using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverTopParticipantWidget : MonoBehaviour
{
	public Image m_AllyTeamIndicator;

	public Image m_EnemyTeamIndicator;

	public Image m_BannerImage;

	public Image m_EmblemImage;

	public TextMeshProUGUI m_PlayerName;

	public TextMeshProUGUI m_PlayerTitle;

	public TextMeshProUGUI m_PlayerLevel;

	public Image m_CharacterIcon;

	public TextMeshProUGUI m_TopParticipantType;

	public LayoutGroup m_BadgesContainer;

	public UIGameOverBadgeWidget m_BadgePrefab;

	public void Setup(TopParticipantSlot slotType, BadgeAndParticipantInfo topPlayerInfo)
	{
		if (GameManager.Get() != null)
		{
			LobbyPlayerInfo playerInfo = GameManager.Get().PlayerInfo;
			using (List<LobbyPlayerInfo>.Enumerator enumerator = GameManager.Get().TeamInfo.TeamPlayerInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LobbyPlayerInfo current = enumerator.Current;
					if (current.PlayerId == topPlayerInfo.PlayerId)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
							{
								if (1 == 0)
								{
									/*OpCode not supported: LdMemberToken*/;
								}
								UIManager.SetGameObjectActive(m_AllyTeamIndicator, current.TeamId == playerInfo.TeamId);
								UIManager.SetGameObjectActive(m_EnemyTeamIndicator, current.TeamId != playerInfo.TeamId);
								GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(current.BannerID);
								string path;
								if (banner != null)
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
									path = banner.m_resourceString;
								}
								else
								{
									path = "Banners/Background/02_blue";
								}
								m_BannerImage.sprite = Resources.Load<Sprite>(path);
								bool doActive = !current.IsAIControlled;
								GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(current.EmblemID);
								string path2;
								if (banner2 != null)
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
									path2 = banner2.m_resourceString;
								}
								else
								{
									path2 = "Banners/Emblems/Chest01";
								}
								m_EmblemImage.sprite = Resources.Load<Sprite>(path2);
								UIManager.SetGameObjectActive(m_EmblemImage, doActive);
								m_PlayerName.text = current.Handle;
								m_PlayerTitle.text = GameBalanceVars.Get().GetTitle(current.TitleID, string.Empty, current.TitleLevel);
								UIManager.SetGameObjectActive(m_PlayerLevel, false);
								m_CharacterIcon.sprite = GameWideData.Get().GetCharacterResourceLink(topPlayerInfo.FreelancerPlayed).GetCharacterSelectIcon();
								m_TopParticipantType.text = StringUtil.TR(slotType.ToString(), "GameOver");
								if (topPlayerInfo.BadgesEarned != null)
								{
									topPlayerInfo.BadgesEarned.Sort(delegate(BadgeInfo x, BadgeInfo y)
									{
										if (x == null)
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
											if (y == null)
											{
												while (true)
												{
													switch (3)
													{
													case 0:
														break;
													default:
														return 0;
													}
												}
											}
										}
										if (x == null)
										{
											while (true)
											{
												switch (2)
												{
												case 0:
													break;
												default:
													return 1;
												}
											}
										}
										if (y == null)
										{
											return -1;
										}
										GameBalanceVars.GameResultBadge badgeInfo2 = GameResultBadgeData.Get().GetBadgeInfo(x.BadgeId);
										GameBalanceVars.GameResultBadge badgeInfo3 = GameResultBadgeData.Get().GetBadgeInfo(y.BadgeId);
										if (badgeInfo2 == null)
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
											if (badgeInfo3 == null)
											{
												return 0;
											}
										}
										if (badgeInfo2 == null)
										{
											return 1;
										}
										if (badgeInfo3 == null)
										{
											while (true)
											{
												switch (4)
												{
												case 0:
													break;
												default:
													return -1;
												}
											}
										}
										if (badgeInfo2.Quality == badgeInfo3.Quality)
										{
											while (true)
											{
												switch (3)
												{
												case 0:
													break;
												default:
													return 0;
												}
											}
										}
										if (badgeInfo2.Quality > badgeInfo3.Quality)
										{
											while (true)
											{
												switch (5)
												{
												case 0:
													break;
												default:
													return -1;
												}
											}
										}
										if (badgeInfo2.Quality < badgeInfo3.Quality)
										{
											while (true)
											{
												switch (7)
												{
												case 0:
													break;
												default:
													return 1;
												}
											}
										}
										return 0;
									});
									int num = 0;
									for (int i = 0; i < topPlayerInfo.BadgesEarned.Count; i++)
									{
										GameBalanceVars.GameResultBadge badgeInfo = GameResultBadgeData.Get().GetBadgeInfo(topPlayerInfo.BadgesEarned[i].BadgeId);
										if (badgeInfo == null)
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
										else
										{
											bool flag = true;
											if (slotType == TopParticipantSlot.Deadliest)
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
												if (badgeInfo.Role != GameBalanceVars.GameResultBadge.BadgeRole.Firepower)
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
													flag = false;
												}
											}
											if (slotType == TopParticipantSlot.Supportiest && badgeInfo.Role != GameBalanceVars.GameResultBadge.BadgeRole.Support)
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
												flag = false;
											}
											if (slotType == TopParticipantSlot.Tankiest)
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
												if (badgeInfo.Role != GameBalanceVars.GameResultBadge.BadgeRole.Frontliner)
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
													flag = false;
												}
											}
											if (flag)
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
												UIGameOverBadgeWidget uIGameOverBadgeWidget = Object.Instantiate(m_BadgePrefab);
												uIGameOverBadgeWidget.Setup(topPlayerInfo.BadgesEarned[i], current.CharacterType, topPlayerInfo.GlobalPercentiles);
												UIManager.ReparentTransform(uIGameOverBadgeWidget.transform, m_BadgesContainer.transform);
												num++;
											}
										}
									}
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											if (num > 5)
											{
												while (true)
												{
													switch (7)
													{
													case 0:
														break;
													default:
														if (m_BadgesContainer as GridLayoutGroup != null)
														{
															while (true)
															{
																switch (6)
																{
																case 0:
																	break;
																default:
																{
																	GridLayoutGroup gridLayoutGroup = m_BadgesContainer as GridLayoutGroup;
																	gridLayoutGroup.cellSize = new Vector2(50f, 50f);
																	gridLayoutGroup.spacing = new Vector2(-15f, -15f);
																	gridLayoutGroup.childAlignment = TextAnchor.UpperCenter;
																	return;
																}
																}
															}
														}
														return;
													}
												}
											}
											return;
										}
									}
								}
								return;
							}
							}
						}
					}
				}
				while (true)
				{
					switch (7)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}
}
