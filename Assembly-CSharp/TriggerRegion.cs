using System;
using System.Collections.Generic;

[Serializable]
public class TriggerRegion : BoardRegion
{
	public string m_regionName = "Default Region";

	public TriggerAmount m_triggerLimit;

	public TriggerActor m_triggerActor;

	private List<ActorData> m_actorsTriggeredOnThisGame;

	public override void Initialize()
	{
		base.Initialize();
		this.m_actorsTriggeredOnThisGame = new List<ActorData>();
	}

	public void OnTurnTick()
	{
		List<ActorData> occupantActors = base.GetOccupantActors();
		using (List<ActorData>.Enumerator enumerator = occupantActors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (this.m_triggerLimit == TriggerAmount.OnceEverPerActor)
				{
					if (this.m_actorsTriggeredOnThisGame.Contains(actorData))
					{
						continue;
					}
				}
				if (this.m_triggerLimit == TriggerAmount.OnceEverTotal)
				{
					if (this.m_actorsTriggeredOnThisGame.Count > 0)
					{
						continue;
					}
				}
				if (!TriggerRegion.ActorCanTrigger(actorData, this.m_triggerActor))
				{
				}
				else if (this.m_triggerLimit == TriggerAmount.OncePerTurnTotal)
				{
					return;
				}
			}
		}
	}

	private static bool ActorCanTrigger(ActorData actor, TriggerActor triggerActor)
	{
		bool result;
		switch (triggerActor)
		{
		case TriggerActor.ClientPlayer:
			result = GameFlowData.Get().m_ownedActorDatas.Contains(actor);
			break;
		case TriggerActor.HumanPlayer:
		{
			bool flag;
			if (GameplayUtils.IsPlayerControlled(actor))
			{
				flag = GameplayUtils.IsHumanControlled(actor);
			}
			else
			{
				flag = false;
			}
			result = flag;
			break;
		}
		case TriggerActor.AnyPlayer:
			result = GameplayUtils.IsPlayerControlled(actor);
			break;
		case TriggerActor.AnyActor:
			result = true;
			break;
		case TriggerActor.NonPlayerActor:
			result = !GameplayUtils.IsPlayerControlled(actor);
			break;
		default:
			result = true;
			break;
		}
		return result;
	}
}
