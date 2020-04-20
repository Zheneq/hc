using System;
using UnityEngine;

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
		return SinglePlayerCoordinator.s_instance;
	}

	private void Awake()
	{
		SinglePlayerCoordinator.s_instance = this;
		for (int i = 0; i < this.m_script.Length; i++)
		{
			this.m_script[i].m_stateIndex = i;
		}
		GameFlowData.s_onGameStateChanged += this.OnGameStateChanged;
	}

	private void OnDestroy()
	{
		if (ChatterManager.Get() != null)
		{
			ChatterManager.Get().EnableChatter = true;
		}
		GameFlowData.s_onGameStateChanged -= this.OnGameStateChanged;
		SinglePlayerCoordinator.s_instance = null;
	}

	private void Start()
	{
		this.m_forbiddenSquares.Initialize();
		foreach (SinglePlayerState singlePlayerState in this.m_script)
		{
			singlePlayerState.Initialize();
		}
	}

	private void OnGameStateChanged(GameState newState)
	{
		if (newState == GameState.EndingGame)
		{
			UITutorialPanel.Get().ClearAll();
			if (this.m_chatTextAtEndOfMatch != null)
			{
				if (this.m_chatTextAtEndOfMatch.Length > 0)
				{
					foreach (SinglePlayerScriptedChat singlePlayerScriptedChat in this.m_chatTextAtEndOfMatch)
					{
						UITutorialPanel.Get().QueueDialogue(singlePlayerScriptedChat.m_text, singlePlayerScriptedChat.m_audioEvent, singlePlayerScriptedChat.m_displaySeconds, singlePlayerScriptedChat.m_sender);
					}
				}
			}
		}
	}
}
