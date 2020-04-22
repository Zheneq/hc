using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
	private static ConversationManager s_instance;

	public static ConversationManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void Start()
	{
		GameFlowData.s_onGameStateChanged += HandleOnGameStateChanged;
	}

	private void OnDestroy()
	{
		s_instance = null;
		GameFlowData.s_onGameStateChanged -= HandleOnGameStateChanged;
	}

	private void HandleOnGameStateChanged(GameState newGameState)
	{
		if (newGameState != GameState.Deployment)
		{
			return;
		}
		LobbyGameplayOverrides gameplayOverrides = GameManager.Get().GameplayOverrides;
		if (gameplayOverrides != null)
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
			if (!gameplayOverrides.EnableConversations)
			{
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
		if (GameFlowData.Get() == null)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (GameFlowData.Get().activeOwnedActorData == null)
			{
				return;
			}
			if (GameFlowData.Get().activeOwnedActorData.GetTeam() != 0)
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
				if (GameFlowData.Get().activeOwnedActorData.GetTeam() != Team.TeamB)
				{
					while (true)
					{
						switch (6)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
			}
			AudioWideData audioWideData = AudioWideData.Get();
			if (audioWideData == null)
			{
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			HashSet<CharacterType> hashSet = new HashSet<CharacterType>();
			using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().GetActors().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (current.GetTeam() == GameFlowData.Get().activeOwnedActorData.GetTeam())
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
						hashSet.Add(current.m_characterType);
					}
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			List<ConversationTemplate> list = new List<ConversationTemplate>();
			ConversationTemplate[] conversations = audioWideData.m_conversations;
			foreach (ConversationTemplate conversationTemplate in conversations)
			{
				if (!hashSet.Contains(conversationTemplate.m_initiator))
				{
					continue;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (hashSet.Contains(conversationTemplate.m_responder))
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
					list.Add(conversationTemplate);
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (list.Count > 0)
				{
					ConversationTemplate conversation = list[Random.Range(0, list.Count)];
					ChatterManager.Get().SubmitConversation(conversation);
				}
				return;
			}
		}
	}
}
