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
			if (!gameplayOverrides.EnableConversations)
			{
				return;
			}
		}
		if (!(GameFlowData.Get() == null))
		{
			if (!(GameFlowData.Get().activeOwnedActorData == null))
			{
				if (GameFlowData.Get().activeOwnedActorData.GetTeam() != Team.TeamA)
				{
					if (GameFlowData.Get().activeOwnedActorData.GetTeam() != Team.TeamB)
					{
						return;
					}
				}
				AudioWideData audioWideData = AudioWideData.Get();
				if (audioWideData == null)
				{
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
							hashSet.Add(actorData.m_characterType);
						}
					}
				}
				List<ConversationTemplate> list = new List<ConversationTemplate>();
				foreach (ConversationTemplate conversationTemplate in audioWideData.m_conversations)
				{
					if (hashSet.Contains(conversationTemplate.m_initiator))
					{
						if (hashSet.Contains(conversationTemplate.m_responder))
						{
							list.Add(conversationTemplate);
						}
					}
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
