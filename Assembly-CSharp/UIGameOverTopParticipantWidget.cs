using System;
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
					LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
					if (lobbyPlayerInfo.PlayerId == topPlayerInfo.PlayerId)
					{
						UIManager.SetGameObjectActive(this.m_AllyTeamIndicator, lobbyPlayerInfo.TeamId == playerInfo.TeamId, null);
						UIManager.SetGameObjectActive(this.m_EnemyTeamIndicator, lobbyPlayerInfo.TeamId != playerInfo.TeamId, null);
						GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(lobbyPlayerInfo.BannerID);
						string path;
						if (banner != null)
						{
							path = banner.m_resourceString;
						}
						else
						{
							path = "Banners/Background/02_blue";
						}
						this.m_BannerImage.sprite = Resources.Load<Sprite>(path);
						bool doActive = !lobbyPlayerInfo.IsAIControlled;
						GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(lobbyPlayerInfo.EmblemID);
						string path2;
						if (banner2 != null)
						{
							path2 = banner2.m_resourceString;
						}
						else
						{
							path2 = "Banners/Emblems/Chest01";
						}
						this.m_EmblemImage.sprite = Resources.Load<Sprite>(path2);
						UIManager.SetGameObjectActive(this.m_EmblemImage, doActive, null);
						this.m_PlayerName.text = lobbyPlayerInfo.Handle;
						this.m_PlayerTitle.text = GameBalanceVars.Get().GetTitle(lobbyPlayerInfo.TitleID, string.Empty, lobbyPlayerInfo.TitleLevel);
						UIManager.SetGameObjectActive(this.m_PlayerLevel, false, null);
						this.m_CharacterIcon.sprite = GameWideData.Get().GetCharacterResourceLink(topPlayerInfo.FreelancerPlayed).GetCharacterSelectIcon();
						this.m_TopParticipantType.text = StringUtil.TR(slotType.ToString(), "GameOver");
						if (topPlayerInfo.BadgesEarned != null)
						{
							topPlayerInfo.BadgesEarned.Sort(delegate(BadgeInfo x, BadgeInfo y)
							{
								if (x == null)
								{
									if (y == null)
									{
										return 0;
									}
								}
								if (x == null)
								{
									return 1;
								}
								if (y == null)
								{
									return -1;
								}
								GameBalanceVars.GameResultBadge badgeInfo2 = GameResultBadgeData.Get().GetBadgeInfo(x.BadgeId);
								GameBalanceVars.GameResultBadge badgeInfo3 = GameResultBadgeData.Get().GetBadgeInfo(y.BadgeId);
								if (badgeInfo2 == null)
								{
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
									return -1;
								}
								if (badgeInfo2.Quality == badgeInfo3.Quality)
								{
									return 0;
								}
								if (badgeInfo2.Quality > badgeInfo3.Quality)
								{
									return -1;
								}
								if (badgeInfo2.Quality < badgeInfo3.Quality)
								{
									return 1;
								}
								return 0;
							});
							int num = 0;
							for (int i = 0; i < topPlayerInfo.BadgesEarned.Count; i++)
							{
								GameBalanceVars.GameResultBadge badgeInfo = GameResultBadgeData.Get().GetBadgeInfo(topPlayerInfo.BadgesEarned[i].BadgeId);
								if (badgeInfo == null)
								{
								}
								else
								{
									bool flag = true;
									if (slotType == TopParticipantSlot.Deadliest)
									{
										if (badgeInfo.Role != GameBalanceVars.GameResultBadge.BadgeRole.Firepower)
										{
											flag = false;
										}
									}
									if (slotType == TopParticipantSlot.Supportiest && badgeInfo.Role != GameBalanceVars.GameResultBadge.BadgeRole.Support)
									{
										flag = false;
									}
									if (slotType == TopParticipantSlot.Tankiest)
									{
										if (badgeInfo.Role != GameBalanceVars.GameResultBadge.BadgeRole.Frontliner)
										{
											flag = false;
										}
									}
									if (flag)
									{
										UIGameOverBadgeWidget uigameOverBadgeWidget = UnityEngine.Object.Instantiate<UIGameOverBadgeWidget>(this.m_BadgePrefab);
										uigameOverBadgeWidget.Setup(topPlayerInfo.BadgesEarned[i], lobbyPlayerInfo.CharacterType, topPlayerInfo.GlobalPercentiles);
										UIManager.ReparentTransform(uigameOverBadgeWidget.transform, this.m_BadgesContainer.transform);
										num++;
									}
								}
							}
							if (num > 5)
							{
								if (this.m_BadgesContainer as GridLayoutGroup != null)
								{
									GridLayoutGroup gridLayoutGroup = this.m_BadgesContainer as GridLayoutGroup;
									gridLayoutGroup.cellSize = new Vector2(50f, 50f);
									gridLayoutGroup.spacing = new Vector2(-15f, -15f);
									gridLayoutGroup.childAlignment = TextAnchor.UpperCenter;
								}
							}
						}
						return;
					}
				}
			}
		}
	}
}
