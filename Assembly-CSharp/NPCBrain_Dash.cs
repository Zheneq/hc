using System;
using UnityEngine;

public class NPCBrain_Dash : NPCBrain
{
	public GameObject[] m_dashLocations;

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_Dash npcbrain_Dash = bot.gameObject.AddComponent<NPCBrain_Dash>();
		npcbrain_Dash.m_dashLocations = this.m_dashLocations;
		return npcbrain_Dash;
	}
}
