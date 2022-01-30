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
		m_actorsTriggeredOnThisGame = new List<ActorData>();
	}

	public void OnTurnTick()
	{
		List<ActorData> occupantActors = GetActorsInRegion();
		using (List<ActorData>.Enumerator enumerator = occupantActors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (m_triggerLimit == TriggerAmount.OnceEverPerActor)
				{
					if (m_actorsTriggeredOnThisGame.Contains(current))
					{
						continue;
					}
				}
				if (m_triggerLimit == TriggerAmount.OnceEverTotal)
				{
					if (m_actorsTriggeredOnThisGame.Count > 0)
					{
						continue;
					}
				}
				if (!ActorCanTrigger(current, m_triggerActor))
				{
				}
				else if (m_triggerLimit == TriggerAmount.OncePerTurnTotal)
				{
					return;
				}
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private static bool ActorCanTrigger(ActorData actor, TriggerActor triggerActor)
	{
		switch (triggerActor)
		{
		case TriggerActor.ClientPlayer:
			return GameFlowData.Get().m_ownedActorDatas.Contains(actor);
		case TriggerActor.HumanPlayer:
		{
			int result;
			if (GameplayUtils.IsPlayerControlled(actor))
			{
				result = (GameplayUtils.IsHumanControlled(actor) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
		case TriggerActor.AnyPlayer:
			return GameplayUtils.IsPlayerControlled(actor);
		case TriggerActor.NonPlayerActor:
			return !GameplayUtils.IsPlayerControlled(actor);
		case TriggerActor.AnyActor:
			return true;
		default:
			return true;
		}
	}
}
