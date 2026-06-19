using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NavigationBar : UIScene
{
    public class NavigationBarSceneStateParameters : SceneStateParameters
    {
    }

    public TextMeshProUGUI m_searchQueueText;
    public _SelectableBtn m_cancelBtn;
    public Button m_cancelHitbox;
    public Animator m_cancelBtnAnimator;
    public _SelectableBtn m_gameSettingsBtn;
    public TextMeshProUGUI[] m_timeInQueueLabel;

    private static NavigationBar s_instance;

    private string m_queueStatusDisplayString = string.Empty;

    private static NavigationBarSceneStateParameters m_currentState = new NavigationBarSceneStateParameters();

    public static NavigationBar Get()
    {
        return s_instance;
    }

    public override SceneStateParameters GetCurrentState()
    {
        return m_currentState;
    }

    public static NavigationBarSceneStateParameters GetCurrentSpecificState()
    {
        return m_currentState;
    }

    private void OnShowGameSettingsClicked(BaseEventData data)
    {
        UICharacterSelectScreen.Get().OnShowGameSettingsClicked(data);
    }

    private void CancelButtonClickCallback(BaseEventData data)
    {
        UICharacterSelectScreenController.Get().CancelButtonClickCallback(data);
    }

    public override void Awake()
    {
        s_instance = this;
        m_gameSettingsBtn.spriteController.callback = OnShowGameSettingsClicked;
        UIManager.SetGameObjectActive(m_cancelBtn, false);
        m_cancelBtn.spriteController.callback = CancelButtonClickCallback;
        m_searchQueueText.raycastTarget = false;
        UIManager.SetGameObjectActive(m_gameSettingsBtn, false);
        m_cancelHitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.SearchQueue, ShowTooltip);
        base.Awake();
    }

    public void UpdateTimeInQueueLabel(string newText)
    {
        if (m_timeInQueueLabel == null)
        {
            return;
        }

        foreach (TextMeshProUGUI textMesh in m_timeInQueueLabel)
        {
            textMesh.text = newText;
        }
    }

    private bool ShowTooltip(UITooltipBase tooltip)
    {
        LobbyMatchmakingQueueInfo queueInfo = GameManager.Get().QueueInfo;
        if (queueInfo == null)
        {
            return false;
        }

        ((UISearchQueueTooltip)tooltip).Setup();
        return true;
    }

    public void UpdateSearchQueueTooltipLabels()
    {
        if (m_cancelBtn.gameObject.activeSelf)
        {
            m_cancelHitbox.GetComponent<UITooltipHoverObject>().Refresh();
        }
    }

    public void SearchQueueTextExit()
    {
        UITooltipManager.Get().HideDisplayTooltip(TooltipType.SearchQueue);
    }

    public void NotifyStatusQueueAnimDone()
    {
        m_searchQueueText.text = m_queueStatusDisplayString;
    }

    public void UpdateStatusMessage()
    {
        bool isWaitingForGroup = SceneStateParameters.IsWaitingForGroup;
        bool isInCustomGame = SceneStateParameters.IsInCustomGame;
        string newText = string.Empty;
        if (!isWaitingForGroup && !isInCustomGame)
        {
            newText = string.Format(
                StringUtil.TR("SecondsTimerShort", "Global"),
                (int)SceneStateParameters.TimeInQueue.TotalSeconds);
        }

        UpdateTimeInQueueLabel(newText);
        if (m_searchQueueText == null)
        {
            return;
        }

        m_queueStatusDisplayString = ClientGameManager.Get().GenerateQueueLabel();
        if (!m_searchQueueText.text.IsNullOrEmpty() && !m_queueStatusDisplayString.IsNullOrEmpty())
        {
            bool flag = m_queueStatusDisplayString != m_searchQueueText.text;
            string value = StringUtil.TR("Searching", "Frontend");
            if (m_queueStatusDisplayString.Contains(value) && m_searchQueueText.text.Contains(value))
            {
                flag = false;
            }

            if (flag)
            {
                try
                {
                    AnimatorStateInfo currentAnimatorStateInfo = m_cancelBtnAnimator.GetCurrentAnimatorStateInfo(0);
                    AnimatorClipInfo animatorClipInfo = m_cancelBtnAnimator.GetCurrentAnimatorClipInfo(0)[0];
                    if (animatorClipInfo.clip.name != "CancelBtnStatusChange"
                        || currentAnimatorStateInfo.normalizedTime >= currentAnimatorStateInfo.length)
                    {
                        m_cancelBtnAnimator.Play("CancelBtnStatusChange", 0, 0f);
                    }
                }
                catch
                {
                    m_searchQueueText.text = m_queueStatusDisplayString;
                }

                return;
            }

            m_searchQueueText.text = m_queueStatusDisplayString;
            return;
        }

        if (m_searchQueueText.text.IsNullOrEmpty() || m_queueStatusDisplayString.IsNullOrEmpty())
        {
            m_searchQueueText.text = m_queueStatusDisplayString;
        }
    }

    public void Update()
    {
        if (AppState_CharacterSelect.Get() != AppState.GetCurrent()
            && AppState_GroupCharacterSelect.Get() != AppState.GetCurrent()
            && AppState_LandingPage.Get() != AppState.GetCurrent())
        {
            return;
        }

        if (UIGameSettingsPanel.Get().m_lastVisible)
        {
            return;
        }

        UpdateStatusMessage();
        UpdateSearchQueueTooltipLabels();
    }

    public override SceneType GetSceneType()
    {
        return SceneType.FrontEndNavPanel;
    }
}
