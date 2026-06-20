using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINavPanelPartyMember : MonoBehaviour
{
    public UITooltipClickObject m_groupMenuClickListener;
    public _ButtonSwapSprite m_hitbox;
    public Animator m_animController;
    public Animator m_PartyLeaderIconAnimator;
    public Animator m_ReadyIconAnimator;
    public Animator m_IsInGameAnimator;
    public Image m_bannerImage;
    public Image m_emblemImage;
    public Image m_ribbonImage;
    public TextMeshProUGUI m_playerLevel;
    public RectTransform m_playerContainer;
    public RectTransform m_invitationSentContainer;

    private UpdateGroupMemberData m_memberInfo;
    private bool m_isHidden;

    public UpdateGroupMemberData GetMemberInfo()
    {
        return m_memberInfo;
    }

    private void Start()
    {
        if (m_hitbox != null)
        {
            m_groupMenuClickListener.Setup(TooltipType.PlayerGroupMenu, OpenGroupMenu);
            m_hitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, SetupTooltip);
        }

        if (m_memberInfo != null)
        {
            if (m_memberInfo.IsLeader)
            {
                UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, true);
                m_PartyLeaderIconAnimator.Play("leaderIconIN");
            }
            else
            {
                UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, false);
            }

            if (m_memberInfo.IsReady)
            {
                UIManager.SetGameObjectActive(m_ReadyIconAnimator, true);
                m_ReadyIconAnimator.Play("readyIconIN");
            }
            else
            {
                UIManager.SetGameObjectActive(m_ReadyIconAnimator, false);
            }
        }
        else
        {
            UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, false);
            UIManager.SetGameObjectActive(m_ReadyIconAnimator, false);
        }

        if (m_invitationSentContainer != null)
        {
            UIManager.SetGameObjectActive(m_invitationSentContainer, false);
        }

        if (m_playerLevel != null)
        {
            m_playerLevel.text = string.Empty;
        }

        if (m_ribbonImage != null)
        {
            UIManager.SetGameObjectActive(m_ribbonImage, false);
        }
    }

    private bool SetupTooltip(UITooltipBase tooltip)
    {
        if (m_memberInfo == null || UITooltipManager.Get().IsVisible(TooltipType.PlayerGroupMenu))
        {
            return false;
        }

        string tooltipText = StringUtil.TR("UnknownCharacter", "Global");
        if (m_memberInfo.MemberDisplayCharacter != CharacterType.None)
        {
            CharacterResourceLink characterResourceLink = GameWideData.Get()
                .GetCharacterResourceLink(m_memberInfo.MemberDisplayCharacter);
            if (characterResourceLink != null)
            {
                tooltipText = characterResourceLink.GetDisplayName();
            }
        }

        (tooltip as UITitledTooltip).Setup(m_memberInfo.MemberDisplayName, tooltipText);
        return true;
    }

    private bool OpenGroupMenu(UITooltipBase tooltip)
    {
        if (m_memberInfo == null)
        {
            FriendListPanel.Get().SetVisible(true, true);
            return false;
        }

        (tooltip as UIPlayerPanelGroupMenu).Setup(m_memberInfo);
        UITooltipManager.Get().HideDisplayTooltip();
        return true;
    }

    public void SetAsLeader(bool isLeader)
    {
        if (isLeader)
        {
            if (!m_PartyLeaderIconAnimator.gameObject.activeSelf)
            {
                UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, true);
                m_PartyLeaderIconAnimator.Play("leaderIconIN");
            }
        }
        else
        {
            UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, false);
        }
    }

    public void SetIsInGame(bool isInGame)
    {
        UIManager.SetGameObjectActive(m_IsInGameAnimator, true);
        if (isInGame)
        {
            m_IsInGameAnimator.Play("readyIconIN");
        }
        else
        {
            AnimatorClipInfo[] clipInfo = m_IsInGameAnimator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo != null
                && clipInfo.Length > 0
                && clipInfo[0].clip.name != "readyIconOUT"
                && clipInfo[0].clip.name != "readyIconOFF")
            {
                m_IsInGameAnimator.Play("readyIconOUT");
            }
        }
    }

    public void UpdateReadyState(bool IsReady)
    {
        UIManager.SetGameObjectActive(m_ReadyIconAnimator, true);
        if (IsReady)
        {
            AnimatorClipInfo[] clipInfo = m_ReadyIconAnimator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo != null
                && clipInfo.Length > 0
                && clipInfo[0].clip.name != "readyIconIN"
                && clipInfo[0].clip.name != "readyIconON")
            {
                m_ReadyIconAnimator.Play("readyIconIN");
            }
        }
        else
        {
            AnimatorClipInfo[] clipInfo = m_ReadyIconAnimator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo != null
                && clipInfo.Length > 0
                && clipInfo[0].clip.name != "readyIconOUT"
                && clipInfo[0].clip.name != "readyIconOFF")
            {
                m_ReadyIconAnimator.Play("readyIconOUT");
            }
        }
    }

    public void Setup(UpdateGroupMemberData info)
    {
        if (info == null)
        {
            return;
        }

#if EVOS
        BannerManager.GetInstance()?.UpdateBanner(
            info.BackgroundBannerID,
            info.ForegroundBannerID,
            info.MemberHandle,
            m_bannerImage,
            m_emblemImage);
#else
        Sprite sprite;
        if (m_bannerImage != null)
        {
            GameBalanceVars.PlayerBanner banner =
                GameWideData.Get().m_gameBalanceVars.GetBanner(info.BackgroundBannerID);
            if (banner == null)
            {
                sprite = (Sprite)Resources.Load(UIPlayerBanner.standardResourceString, typeof(Sprite));
            }
            else
            {
                sprite = (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite));
            }

            m_bannerImage.sprite = sprite;
        }

        if (m_emblemImage != null)
        {
            GameBalanceVars.PlayerBanner emblem =
                GameWideData.Get().m_gameBalanceVars.GetBanner(info.ForegroundBannerID);
            if (emblem == null)
            {
                sprite = (Sprite)Resources.Load(UIPlayerBanner.standardResourceString, typeof(Sprite));
            }
            else
            {
                sprite = (Sprite)Resources.Load(emblem.m_resourceString, typeof(Sprite));
            }

            m_emblemImage.sprite = sprite;
        }
#endif

        if (m_ribbonImage != null)
        {
            GameBalanceVars.PlayerRibbon ribbon = GameWideData.Get().m_gameBalanceVars.GetRibbon(info.RibbonID);
            if (ribbon == null)
            {
                UIManager.SetGameObjectActive(m_ribbonImage, false);
            }
            else
            {
                m_ribbonImage.sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
                UIManager.SetGameObjectActive(m_ribbonImage, m_ribbonImage.sprite != null);
            }
        }

        if (m_memberInfo == null || m_memberInfo.AccountID != info.AccountID)
        {
            m_isHidden = false;
            if (m_playerContainer != null)
            {
                UIManager.SetGameObjectActive(m_playerContainer, true);
            }

            UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, info.IsLeader);
            if (info.IsLeader)
            {
                m_PartyLeaderIconAnimator.Play("leaderIconIN");
            }

            UpdateReadyState(info.IsReady);
            m_memberInfo = info;
            if (m_animController != null)
            {
                m_animController.Play("UIBannerInviteIN", 1, 0f);
            }

            if (m_groupMenuClickListener != null)
            {
                m_groupMenuClickListener.Refresh();
            }
        }
    }

    private void DoHidden()
    {
        m_isHidden = true;
        if (m_playerContainer != null)
        {
            UIManager.SetGameObjectActive(m_playerContainer, false);
        }

        if (m_ribbonImage != null)
        {
            UIManager.SetGameObjectActive(m_ribbonImage, false);
        }

        if (m_animController != null)
        {
            m_animController.Play("UIBannerDisabledIN", 1, 0f);
        }

        UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, false);
        UIManager.SetGameObjectActive(m_ReadyIconAnimator, false);
        UIManager.SetGameObjectActive(m_IsInGameAnimator, false);
        m_memberInfo = null;
    }

    public void SetToHidden()
    {
        if (!m_isHidden)
        {
            DoHidden();
        }
    }
}