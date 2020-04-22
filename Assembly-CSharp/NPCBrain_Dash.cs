using UnityEngine;

public class NPCBrain_Dash : NPCBrain
{
	public GameObject[] m_dashLocations;

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_Dash nPCBrain_Dash = bot.gameObject.AddComponent<NPCBrain_Dash>();
		nPCBrain_Dash.m_dashLocations = m_dashLocations;
		return nPCBrain_Dash;
	}
}
