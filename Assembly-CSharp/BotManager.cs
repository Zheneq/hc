// TODO BOTS
using System.Collections.Generic;
using UnityEngine;

// server-only -- empty in reactor
// added minimal required stuff -- not actually updated with rouges code
public class BotManager : MonoBehaviour
{
#if SERVER
	private static BotManager s_instance;

	private string[] m_botNames;
	private List<Player> m_botAgents;

	public static BotManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		m_botAgents = new List<Player>();
		if (m_botNames == null || m_botNames.Length == 0)
		{
			m_botNames = new string[10];
			m_botNames[0] = "RockMan";
			m_botNames[1] = "ProtoMan";
			m_botNames[2] = "MetalMan";
			m_botNames[3] = "FlashMan";
			m_botNames[4] = "QuickMan";
			m_botNames[5] = "HeatMan";
			m_botNames[6] = "WoodMan";
			m_botNames[7] = "AirMan";
			m_botNames[8] = "CrashMan";
			m_botNames[9] = "ElecMan";
		}
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public void AddExistingBot(Player newBot)
	{
		m_botAgents.Add(newBot);
	}
#endif
}
