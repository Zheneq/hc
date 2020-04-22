using UnityEngine;

public class NPCBrain_Scripted : NPCBrain
{
	public string m_name;

	public ActionsForTurn[] m_actions;

	public NPCBrain m_brainAfterActions;

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_Scripted nPCBrain_Scripted = bot.gameObject.AddComponent<NPCBrain_Scripted>();
		nPCBrain_Scripted.m_actions = m_actions;
		nPCBrain_Scripted.m_brainAfterActions = m_brainAfterActions;
		return nPCBrain_Scripted;
	}
}
