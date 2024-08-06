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
		if (GameManager.Get() == null)
		{
			return;
		}
		LobbyPlayerInfo playerInfo = GameManager.Get().PlayerInfo;
		foreach (LobbyPlayerInfo current in GameManager.Get().TeamInfo.TeamPlayerInfo)
		{
			if (current.PlayerId != topPlayerInfo.PlayerId)
			{
				continue;
			}
			UIManager.SetGameObjectActive(m_AllyTeamIndicator, current.TeamId == playerInfo.TeamId);
			UIManager.SetGameObjectActive(m_EnemyTeamIndicator, current.TeamId != playerInfo.TeamId);
			
			GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(current.BannerID);
			string bannerPath = banner != null ? banner.m_resourceString : "Banners/Background/02_blue";
			m_BannerImage.sprite = Resources.Load<Sprite>(bannerPath);
			
			bool doActive = !current.IsAIControlled;
			
			GameBalanceVars.PlayerBanner emblem = GameWideData.Get().m_gameBalanceVars.GetBanner(current.EmblemID);
			string emblemPath = emblem != null ? emblem.m_resourceString : "Banners/Emblems/Chest01";
			m_EmblemImage.sprite = Resources.Load<Sprite>(emblemPath);
			
			UIManager.SetGameObjectActive(m_EmblemImage, doActive);
			m_PlayerName.text = current.Handle;
#if !VANILLA && !SERVER
            // Custom titles
            m_PlayerTitle.text = GameBalanceVars.Get().GetTitle(current.TitleID, current.Handle, string.Empty, current.TitleLevel);
#else
			m_PlayerTitle.text = GameBalanceVars.Get().GetTitle(current.TitleID, string.Empty, current.TitleLevel);
#endif
			UIManager.SetGameObjectActive(m_PlayerLevel, false);
			m_CharacterIcon.sprite = GameWideData.Get().GetCharacterResourceLink(topPlayerInfo.FreelancerPlayed).GetCharacterSelectIcon();
			m_TopParticipantType.text = StringUtil.TR(slotType.ToString(), "GameOver");
			
			if (topPlayerInfo.BadgesEarned == null)
			{
				return;
			}
			
			topPlayerInfo.BadgesEarned.Sort(BadgeComparison);
			int badgeNum = 0;
			foreach (BadgeInfo badge in topPlayerInfo.BadgesEarned)
			{
				GameBalanceVars.GameResultBadge badgeInfo = GameResultBadgeData.Get().GetBadgeInfo(badge.BadgeId);
				if (badgeInfo == null)
				{
					continue;
				}

				if ((slotType != TopParticipantSlot.Deadliest || badgeInfo.Role == GameBalanceVars.GameResultBadge.BadgeRole.Firepower)
				    && (slotType != TopParticipantSlot.Supportiest || badgeInfo.Role == GameBalanceVars.GameResultBadge.BadgeRole.Support)
				    && (slotType != TopParticipantSlot.Tankiest || badgeInfo.Role == GameBalanceVars.GameResultBadge.BadgeRole.Frontliner))
				{
					UIGameOverBadgeWidget uIGameOverBadgeWidget = Instantiate(m_BadgePrefab);
					uIGameOverBadgeWidget.Setup(badge, current.CharacterType, topPlayerInfo.GlobalPercentiles);
					UIManager.ReparentTransform(uIGameOverBadgeWidget.transform, m_BadgesContainer.transform);
					badgeNum++;
				}
			}
			if (badgeNum > 5 && m_BadgesContainer is GridLayoutGroup gridLayoutGroup)
			{
				gridLayoutGroup.cellSize = new Vector2(50f, 50f);
				gridLayoutGroup.spacing = new Vector2(-15f, -15f);
				gridLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			}
			return;
		}
	}

	private static int BadgeComparison(BadgeInfo x, BadgeInfo y)
	{
		if (x == null && y == null)
		{
			return 0;
		}

		if (x == null)
		{
			return 1;
		}

		if (y == null)
		{
			return -1;
		}

		GameBalanceVars.GameResultBadge badgeInfoX = GameResultBadgeData.Get().GetBadgeInfo(x.BadgeId);
		GameBalanceVars.GameResultBadge badgeInfoY = GameResultBadgeData.Get().GetBadgeInfo(y.BadgeId);
		if (badgeInfoX == null && badgeInfoY == null)
		{
			return 0;
		}

		if (badgeInfoX == null)
		{
			return 1;
		}

		if (badgeInfoY == null)
		{
			return -1;
		}

		if (badgeInfoX.Quality == badgeInfoY.Quality)
		{
			return 0;
		}

		if (badgeInfoX.Quality > badgeInfoY.Quality)
		{
			return -1;
		}

		if (badgeInfoX.Quality < badgeInfoY.Quality)
		{
			return 1;
		}

		return 0;
	}
}
