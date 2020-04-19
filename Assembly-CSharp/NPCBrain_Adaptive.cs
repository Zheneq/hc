using System;
using UnityEngine;

public class NPCBrain_Adaptive : NPCBrain
{
	public string m_name;

	public BotDifficulty m_botDifficulty = BotDifficulty.Hard;

	public bool m_canTaunt;

	public bool m_inactiveUntilPlayerEncountered;

	public float m_evasionScoreTweak = 1f;

	public float m_damageScoreTweak = 1f;

	public float m_healingScoreTweak = 1f;

	public float m_shieldingScoreTweak = 1f;

	public float m_multipleEnemyTweak = 1f;

	public float m_multipleAllyTweak = 1f;

	public int[] m_allowedAbilities;

	public bool m_logReasoning;

	public bool m_sendReasoningToTeamChat;

	[HideInInspector]
	public bool m_playerEncountered;

	private bool m_isReplacingHuman;

	private static bool s_gatherRealResults = true;

	public bool isReplacingHuman
	{
		get
		{
			return this.m_isReplacingHuman;
		}
		set
		{
			this.m_isReplacingHuman = value;
		}
	}

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_Adaptive npcbrain_Adaptive = bot.gameObject.AddComponent<NPCBrain_Adaptive>();
		npcbrain_Adaptive.m_botDifficulty = this.m_botDifficulty;
		npcbrain_Adaptive.m_inactiveUntilPlayerEncountered = this.m_inactiveUntilPlayerEncountered;
		npcbrain_Adaptive.m_playerEncountered = false;
		npcbrain_Adaptive.m_evasionScoreTweak = this.m_evasionScoreTweak;
		npcbrain_Adaptive.m_damageScoreTweak = this.m_damageScoreTweak;
		npcbrain_Adaptive.m_healingScoreTweak = this.m_healingScoreTweak;
		npcbrain_Adaptive.m_shieldingScoreTweak = this.m_shieldingScoreTweak;
		npcbrain_Adaptive.m_multipleEnemyTweak = this.m_multipleEnemyTweak;
		npcbrain_Adaptive.m_multipleAllyTweak = this.m_multipleAllyTweak;
		npcbrain_Adaptive.m_allowedAbilities = this.m_allowedAbilities;
		this.MakeFSM(npcbrain_Adaptive);
		if (GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.EnableTeamAIOutput))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NPCBrain_Adaptive.Create(BotController, Transform)).MethodHandle;
			}
			npcbrain_Adaptive.m_sendReasoningToTeamChat = true;
		}
		return npcbrain_Adaptive;
	}

	public static NPCBrain Create(BotController bot, Transform destination, BotDifficulty botDifficulty, bool canTaunt)
	{
		NPCBrain_Adaptive npcbrain_Adaptive = bot.gameObject.AddComponent<NPCBrain_Adaptive>();
		npcbrain_Adaptive.m_botDifficulty = botDifficulty;
		npcbrain_Adaptive.m_canTaunt = canTaunt;
		npcbrain_Adaptive.m_inactiveUntilPlayerEncountered = false;
		npcbrain_Adaptive.m_playerEncountered = false;
		npcbrain_Adaptive.MakeFSM(npcbrain_Adaptive);
		if (GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.EnableTeamAIOutput))
		{
			npcbrain_Adaptive.m_sendReasoningToTeamChat = true;
		}
		return npcbrain_Adaptive;
	}

	public void SendDecisionToTeamChat(bool val)
	{
		this.m_sendReasoningToTeamChat = val;
	}
}
