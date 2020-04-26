using UnityEngine;

public class NPCBrain_StateMachine : NPCBrain
{
	public override NPCBrain Create(BotController bot, Transform destination)
	{
		return bot.gameObject.AddComponent<NPCBrain_StateMachine>();
	}
}
