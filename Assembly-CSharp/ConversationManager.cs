using System;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
	private static ConversationManager s_instance;

	public static ConversationManager Get()
	{
		return ConversationManager.s_instance;
	}

	private void Awake()
	{
		ConversationManager.s_instance = this;
	}

	private void Start()
	{
		GameFlowData.s_onGameStateChanged += this.HandleOnGameStateChanged;
	}

	private void OnDestroy()
	{
		ConversationManager.s_instance = null;
		GameFlowData.s_onGameStateChanged -= this.HandleOnGameStateChanged;
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ConversationManager.HandleOnGameStateChanged(GameState)).MethodHandle;
			}
			if (!gameplayOverrides.EnableConversations)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				return;
			}
		}
		if (!(GameFlowData.Get() == null))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!(GameFlowData.Get().activeOwnedActorData == null))
			{
				if (GameFlowData.Get().activeOwnedActorData.GetTeam() != Team.TeamA)
				{
					for (;;)
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
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						return;
					}
				}
				AudioWideData audioWideData = AudioWideData.Get();
				if (audioWideData == null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					return;
				}
				HashSet<CharacterType> hashSet = new HashSet<CharacterType>();
				using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().GetActors().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						if (actorData.GetTeam() == GameFlowData.Get().activeOwnedActorData.GetTeam())
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							hashSet.Add(actorData.m_characterType);
						}
					}
					for (;;)
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
				foreach (ConversationTemplate conversationTemplate in audioWideData.m_conversations)
				{
					if (hashSet.Contains(conversationTemplate.m_initiator))
					{
						for (;;)
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
							for (;;)
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
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (list.Count > 0)
				{
					ConversationTemplate conversation = list[UnityEngine.Random.Range(0, list.Count)];
					ChatterManager.Get().SubmitConversation(conversation);
				}
				return;
			}
		}
	}
}
