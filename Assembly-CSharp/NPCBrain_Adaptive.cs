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
			return m_isReplacingHuman;
		}
		set
		{
			m_isReplacingHuman = value;
		}
	}

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_Adaptive nPCBrain_Adaptive = bot.gameObject.AddComponent<NPCBrain_Adaptive>();
		nPCBrain_Adaptive.m_botDifficulty = m_botDifficulty;
		nPCBrain_Adaptive.m_inactiveUntilPlayerEncountered = m_inactiveUntilPlayerEncountered;
		nPCBrain_Adaptive.m_playerEncountered = false;
		nPCBrain_Adaptive.m_evasionScoreTweak = m_evasionScoreTweak;
		nPCBrain_Adaptive.m_damageScoreTweak = m_damageScoreTweak;
		nPCBrain_Adaptive.m_healingScoreTweak = m_healingScoreTweak;
		nPCBrain_Adaptive.m_shieldingScoreTweak = m_shieldingScoreTweak;
		nPCBrain_Adaptive.m_multipleEnemyTweak = m_multipleEnemyTweak;
		nPCBrain_Adaptive.m_multipleAllyTweak = m_multipleAllyTweak;
		nPCBrain_Adaptive.m_allowedAbilities = m_allowedAbilities;
		MakeFSM(nPCBrain_Adaptive);
		if (GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.EnableTeamAIOutput))
		{
			nPCBrain_Adaptive.m_sendReasoningToTeamChat = true;
		}
		return nPCBrain_Adaptive;
	}

	public static NPCBrain Create(BotController bot, Transform destination, BotDifficulty botDifficulty, bool canTaunt)
	{
		NPCBrain_Adaptive nPCBrain_Adaptive = bot.gameObject.AddComponent<NPCBrain_Adaptive>();
		nPCBrain_Adaptive.m_botDifficulty = botDifficulty;
		nPCBrain_Adaptive.m_canTaunt = canTaunt;
		nPCBrain_Adaptive.m_inactiveUntilPlayerEncountered = false;
		nPCBrain_Adaptive.m_playerEncountered = false;
		nPCBrain_Adaptive.MakeFSM(nPCBrain_Adaptive);
		if (GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.EnableTeamAIOutput))
		{
			nPCBrain_Adaptive.m_sendReasoningToTeamChat = true;
		}
		return nPCBrain_Adaptive;
	}

	public void SendDecisionToTeamChat(bool val)
	{
		m_sendReasoningToTeamChat = val;
	}
}
