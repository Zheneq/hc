using UnityEngine;
using UnityEngine.Networking;

public class SinglePlayerCoordinator : MonoBehaviour
{
    public SinglePlayerState[] m_script;
    public BoardRegion m_forbiddenSquares;
    public ActivatableObject[] m_activationsOnForbiddenPath;
    public ActivatableObject[] m_activationsOnFailedToShootAndMove;
    public ActivatableObject[] m_activationsOnFailedToUseAllAbilities;
    public GameObject m_initialCameraRotationTarget;
    public SinglePlayerScriptedChat[] m_chatTextOnLowHealth;
    public SinglePlayerScriptedChat[] m_chatTextAtEndOfMatch;

    private static SinglePlayerCoordinator s_instance;

    public static SinglePlayerCoordinator Get()
    {
        return s_instance;
    }

    private void Awake()
    {
        s_instance = this;

#if SERVER
        if (NetworkServer.active && m_script.Length == 0)
        {
            Log.Warning("SinglePlayerCoordinator has no states in its script-- why even have the coordinator?");
            m_script = new SinglePlayerState[1];
        }
#endif

        for (int i = 0; i < m_script.Length; i++)
        {
            m_script[i].m_stateIndex = i;
        }

        GameFlowData.s_onGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
#if !SERVER
        if (ChatterManager.Get() != null)
        {
            ChatterManager.Get().EnableChatter = true;
        }
#endif
        GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
        s_instance = null;
    }

    private void Start()
    {
        m_forbiddenSquares.Initialize(); // Initialize(base.gameObject.scene) in rogues
        SinglePlayerState[] script = m_script;
        foreach (SinglePlayerState singlePlayerState in script)
        {
            singlePlayerState.Initialize(); // Initialize(base.gameObject.scene) in rogues
        }
    }

    private void OnGameStateChanged(GameState newState)
    {
#if !SERVER
        if (newState != GameState.EndingGame)
        {
            return;
        }
        
        UITutorialPanel.Get().ClearAll();
        
        if (m_chatTextAtEndOfMatch == null || m_chatTextAtEndOfMatch.Length <= 0)
        {
            return;
        }
        
        foreach (SinglePlayerScriptedChat singlePlayerScriptedChat in m_chatTextAtEndOfMatch)
        {
            UITutorialPanel.Get().QueueDialogue(
                singlePlayerScriptedChat.m_text,
                singlePlayerScriptedChat.m_audioEvent,
                singlePlayerScriptedChat.m_displaySeconds,
                singlePlayerScriptedChat.m_sender);
        }
#endif
    }
}