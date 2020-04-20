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
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(TriggerRegion.OnTurnTick()).MethodHandle;
					}
					if (this.m_actorsTriggeredOnThisGame.Contains(actorData))
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
						continue;
					}
				}
				if (this.m_triggerLimit == TriggerAmount.OnceEverTotal)
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
					if (this.m_actorsTriggeredOnThisGame.Count > 0)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						continue;
					}
				}
				if (!TriggerRegion.ActorCanTrigger(actorData, this.m_triggerActor))
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				else if (this.m_triggerLimit == TriggerAmount.OncePerTurnTotal)
				{
					return;
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TriggerRegion.ActorCanTrigger(ActorData, TriggerActor)).MethodHandle;
				}
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
