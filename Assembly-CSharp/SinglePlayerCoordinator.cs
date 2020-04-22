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
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		for (int i = 0; i < m_script.Length; i++)
		{
			m_script[i].m_stateIndex = i;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GameFlowData.s_onGameStateChanged += OnGameStateChanged;
			return;
		}
	}

	private void OnDestroy()
	{
		if (ChatterManager.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ChatterManager.Get().EnableChatter = true;
		}
		GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
		s_instance = null;
	}

	private void Start()
	{
		m_forbiddenSquares.Initialize();
		SinglePlayerState[] script = m_script;
		foreach (SinglePlayerState singlePlayerState in script)
		{
			singlePlayerState.Initialize();
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	private void OnGameStateChanged(GameState newState)
	{
		if (newState != GameState.EndingGame)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UITutorialPanel.Get().ClearAll();
			if (m_chatTextAtEndOfMatch == null)
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (m_chatTextAtEndOfMatch.Length <= 0)
				{
					return;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					SinglePlayerScriptedChat[] chatTextAtEndOfMatch = m_chatTextAtEndOfMatch;
					foreach (SinglePlayerScriptedChat singlePlayerScriptedChat in chatTextAtEndOfMatch)
					{
						UITutorialPanel.Get().QueueDialogue(singlePlayerScriptedChat.m_text, singlePlayerScriptedChat.m_audioEvent, singlePlayerScriptedChat.m_displaySeconds, singlePlayerScriptedChat.m_sender);
					}
					while (true)
					{
						switch (2)
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
}
