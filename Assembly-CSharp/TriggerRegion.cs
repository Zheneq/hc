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
		List<ActorData> occupantActors = GetOccupantActors();
		using (List<ActorData>.Enumerator enumerator = occupantActors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (m_triggerLimit == TriggerAmount.OnceEverPerActor)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (m_actorsTriggeredOnThisGame.Contains(current))
					{
						while (true)
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
				if (m_triggerLimit == TriggerAmount.OnceEverTotal)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_actorsTriggeredOnThisGame.Count > 0)
					{
						while (true)
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
				if (!ActorCanTrigger(current, m_triggerActor))
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
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
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
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
