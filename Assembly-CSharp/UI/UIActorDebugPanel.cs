using System.Collections.Generic;
using UnityEngine;

public class UIActorDebugPanel : MonoBehaviour
{
    public UIActorDebugLabel m_actorDebugLabelPrefab;

    private Dictionary<ActorData, UIActorDebugLabel> m_actorLabels = new Dictionary<ActorData, UIActorDebugLabel>();
    private Dictionary<ActorData, bool> m_shouldDisplay = new Dictionary<ActorData, bool>();

    private static UIActorDebugPanel s_instance;

    public static UIActorDebugPanel Get()
    {
        return s_instance;
    }

    private void Awake()
    {
        UIManager.SetGameObjectActive(this, false);
        s_instance = this;
    }

    public void OnActorDestroyed(ActorData actor)
    {
        if (m_actorLabels.ContainsKey(actor))
        {
            UIActorDebugLabel uIActorDebugLabel = m_actorLabels[actor];
            uIActorDebugLabel.m_label.text = string.Empty;
            m_actorLabels.Remove(actor);
            Destroy(uIActorDebugLabel);
        }

        if (m_shouldDisplay.ContainsKey(actor))
        {
            m_shouldDisplay.Remove(actor);
        }
    }

    public void Reset()
    {
        foreach (UIActorDebugLabel label in m_actorLabels.Values)
        {
            Destroy(label.gameObject);
        }

        m_actorLabels.Clear();
        m_shouldDisplay.Clear();
    }

    public void SetActorValue(ActorData actorData, string key, string displayStr)
    {
        m_shouldDisplay[actorData] = true;
        UIActorDebugLabel uIActorDebugLabel;

        if (!m_actorLabels.ContainsKey(actorData))
        {
            uIActorDebugLabel = Instantiate(m_actorDebugLabelPrefab);
            uIActorDebugLabel.Setup(actorData);
            uIActorDebugLabel.transform.SetParent(transform);
            m_actorLabels[actorData] = uIActorDebugLabel;
        }
        else
        {
            uIActorDebugLabel = m_actorLabels[actorData];
        }

        uIActorDebugLabel.SetEntry(key, displayStr);
        UIManager.SetGameObjectActive(this, true);
    }
}