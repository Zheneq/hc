using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAlertDisplay : MonoBehaviour
{
    private class AlertMessage
    {
        public float startTime;
        public float duration;
        public TextMeshProUGUI label;
        public bool showBackground;
        public int alertLabelIndex;
    }

    public enum LowTimePulseType
    {
        Standard,
        TurnEndWarning,
        UsingTimeBank
    }

    public TextMeshProUGUI[] m_label;
    public Image[] m_optionalLabelBackground;
    public TextMeshProUGUI m_deathLabel;
    public Animator m_DeathAnimController;
    public GameObject m_lowTimePulse;
    public GameObject m_timebankTimePulse;

    [Space(10f)]
    [Header("Takedown")]
    public TextMeshProUGUI m_enemyTakeDownTitle;
    public TextMeshProUGUI m_enemyTakeDownSubTitle;
    public TextMeshProUGUI m_allyTakeDownTitle;
    public TextMeshProUGUI m_allyTakeDownSubTitle;
    public Animator m_takendownAlerts;
    public RectTransform m_enemyTakendownContainer;
    public RectTransform m_allyTakendownContainer;
    public RectTransform m_enemyAceContainer;
    public RectTransform m_allyAceContainer;
    public RectTransform[] m_enemyTakedownContainer;
    public RectTransform[] m_allyTakedownContainer;
    public Image[] m_enemyTakedowns;
    public Image[] m_allyTakedowns;

    private List<AlertMessage> m_currentMessages = new List<AlertMessage>();
    private List<AlertMessage> m_messagesToRemove = new List<AlertMessage>();

    private float m_originalLabelBgAlpha = 1f;

    public void Start()
    {
        if (m_lowTimePulse != null)
        {
            UIManager.SetGameObjectActive(m_lowTimePulse, false);
        }

        if (m_timebankTimePulse != null)
        {
            UIManager.SetGameObjectActive(m_timebankTimePulse, false);
        }

        for (int i = 0; i < m_label.Length; i++)
        {
            m_label[i].raycastTarget = false;
            UIManager.SetGameObjectActive(m_label[i], false);
            UIManager.SetGameObjectActive(m_optionalLabelBackground[i], false);
        }

        m_deathLabel.raycastTarget = false;
        if (m_optionalLabelBackground != null)
        {
            Color color = m_optionalLabelBackground[0].color;
            m_originalLabelBgAlpha = color.a;
        }
    }

    public void DisplayAlert(
        string message,
        Color color,
        float messageTimeSeconds,
        bool showBackground = false,
        int alertToUse = 0)
    {
        AlertMessage alertMessage = null;
        foreach (AlertMessage currentMessage in m_currentMessages)
        {
            if (ReferenceEquals(message, currentMessage.label.text) || message == currentMessage.label.text)
            {
                alertMessage = currentMessage;
                break;
            }
        }

        if (alertMessage == null)
        {
            alertMessage = new AlertMessage();
            alertMessage.alertLabelIndex = alertToUse;
            alertMessage.label = m_label[alertMessage.alertLabelIndex];
            m_currentMessages.Add(alertMessage);
        }

        alertMessage.showBackground = showBackground;
        if (m_optionalLabelBackground != null && showBackground)
        {
            for (int i = 0; i < m_optionalLabelBackground.Length; i++)
            {
                UIManager.SetGameObjectActive(m_optionalLabelBackground[i], i == alertMessage.alertLabelIndex);
            }

            Color bgColor = m_optionalLabelBackground[alertMessage.alertLabelIndex].color;
            bgColor.a = m_originalLabelBgAlpha;
            m_optionalLabelBackground[alertMessage.alertLabelIndex].color = bgColor;
        }

        alertMessage.label.text = message;
        if (color != alertMessage.label.color)
        {
            alertMessage.label.color = color;
        }

        alertMessage.startTime = Time.time;
        alertMessage.duration = messageTimeSeconds > 0f ? messageTimeSeconds : 2f;
    }

    public void CancelAlert(string message)
    {
        foreach (AlertMessage alertMessage in m_currentMessages)
        {
            if (alertMessage.label.text == message)
            {
                alertMessage.startTime = 0f;
            }
        }
    }

    public void TriggerLowTimePulse(LowTimePulseType type)
    {
        TurnStateEnum currentState = GameFlowData.Get().activeOwnedActorData.GetActorTurnSM().CurrentState;
        if (currentState == TurnStateEnum.CONFIRMED)
        {
            return;
        }

        if (GameFlowData.Get().activeOwnedActorData.IsDead() && currentState != TurnStateEnum.PICKING_RESPAWN)
        {
            return;
        }

        if (m_lowTimePulse != null && type == LowTimePulseType.Standard)
        {
            UIManager.SetGameObjectActive(m_lowTimePulse, false);
            UIManager.SetGameObjectActive(m_lowTimePulse, true);
        }

        if (m_timebankTimePulse != null && type != LowTimePulseType.Standard)
        {
            UIManager.SetGameObjectActive(m_timebankTimePulse, false);
            UIManager.SetGameObjectActive(m_timebankTimePulse, true);
        }

        switch (type)
        {
            case LowTimePulseType.TurnEndWarning:
                UISounds.GetUISounds().Play("ui/countdown/one_second_left");
                break;
            case LowTimePulseType.UsingTimeBank:
                UISounds.GetUISounds().Play("ui/countdown/timebank");
                break;
            default:
                UISounds.GetUISounds().Play("ui/countdown/tick");
                break;
        }
    }

    private void Update()
    {
        if (GameFlowData.Get() == null)
        {
            return;
        }

        ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
        if (activeOwnedActorData != null
            && activeOwnedActorData.IsDead()
            && activeOwnedActorData.IsInRagdoll()
            && GameFlowData.Get().gameState > GameState.StartingGame)
        {
            UIManager.SetGameObjectActive(m_DeathAnimController, true);
            string text;
            if (activeOwnedActorData.NextRespawnTurn != -1)
            {
                int turns = activeOwnedActorData.NextRespawnTurn - GameFlowData.Get().CurrentTurn - 1;
                if (turns > 0)
                {
                    text = string.Format(StringUtil.TR("RespawnInTurns", "Global"), turns + 1);
                }
                else
                {
                    text = StringUtil.TR("RespawnNextTurn", "Global");
                }
            }
            else
            {
                text = StringUtil.TR("YouAreDead", "Global");
            }

            if (!ReferenceEquals(m_deathLabel.text, text) || m_deathLabel.text != text)
            {
                m_deathLabel.text = text;
            }
        }
        else if (m_DeathAnimController.gameObject.activeInHierarchy)
        {
            m_DeathAnimController.Play("DeathTimePulseOUT");
        }

        int idx = 0;
        using (List<AlertMessage>.Enumerator enumerator = m_currentMessages.GetEnumerator())
        {
            foreach (AlertMessage alertMessage in m_currentMessages)
            {
                if (Time.time - alertMessage.startTime > alertMessage.duration)
                {
                    if (m_label[alertMessage.alertLabelIndex] != null)
                    {
                        UIManager.SetGameObjectActive(m_label[alertMessage.alertLabelIndex], false);
                    }

                    if (m_optionalLabelBackground != null
                        && alertMessage.showBackground
                        && m_optionalLabelBackground[alertMessage.alertLabelIndex] != null)
                    {
                        UIManager.SetGameObjectActive(
                            m_optionalLabelBackground[alertMessage.alertLabelIndex],
                            false);
                    }

                    m_messagesToRemove.Add(alertMessage);
                }
                else
                {
                    float fadeOutTime = alertMessage.duration * 0.75f;
                    float elapsedTime = Time.time - alertMessage.startTime;
                    float fadeOutAlpha = 1f - (elapsedTime - fadeOutTime) / (alertMessage.duration - fadeOutTime);
                    Color color = alertMessage.label.color;

                    Color fadeOutColor = m_optionalLabelBackground[alertMessage.alertLabelIndex] != null
                        ? m_optionalLabelBackground[alertMessage.alertLabelIndex].color
                        : default(Color);
                    color.a = idx > 0 ? 0f : fadeOutAlpha;

                    alertMessage.label.color = color;
                    fadeOutColor.a = fadeOutAlpha;
                    if (alertMessage.showBackground
                        && m_optionalLabelBackground[alertMessage.alertLabelIndex] != null
                        && fadeOutAlpha < m_originalLabelBgAlpha)
                    {
                        m_optionalLabelBackground[alertMessage.alertLabelIndex].color = fadeOutColor;
                    }

                    if (!alertMessage.label.gameObject.activeSelf)
                    {
                        UIManager.SetGameObjectActive(alertMessage.label, true);
                        if (m_optionalLabelBackground != null && alertMessage.showBackground)
                        {
                            for (int i = 0; i < m_optionalLabelBackground.Length; i++)
                            {
                                if (m_optionalLabelBackground[i] != null)
                                {
                                    UIManager.SetGameObjectActive(
                                        m_optionalLabelBackground[i],
                                        i == alertMessage.alertLabelIndex);
                                }
                            }

                            fadeOutColor.a = m_originalLabelBgAlpha;
                            m_optionalLabelBackground[alertMessage.alertLabelIndex].color = fadeOutColor;
                        }
                    }

                    idx++;
                }
            }
        }

        foreach (AlertMessage alertMessage in m_messagesToRemove)
        {
            m_currentMessages.Remove(alertMessage);
        }

        m_messagesToRemove.Clear();
    }

    private void OnEnable()
    {
        Update();
    }
}