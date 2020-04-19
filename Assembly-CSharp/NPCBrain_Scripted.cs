using System;
using UnityEngine;

public class NPCBrain_Scripted : NPCBrain
{
	public string m_name;

	public ActionsForTurn[] m_actions;

	public NPCBrain m_brainAfterActions;

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_Scripted npcbrain_Scripted = bot.gameObject.AddComponent<NPCBrain_Scripted>();
		npcbrain_Scripted.m_actions = this.m_actions;
		npcbrain_Scripted.m_brainAfterActions = this.m_brainAfterActions;
		return npcbrain_Scripted;
	}
}
