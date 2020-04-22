public class ClientGameModeEvent
{
	public GameModeEventType m_eventType;

	public byte m_objectGuid;

	public BoardSquare m_square;

	public ActorData m_primaryActor;

	public ActorData m_secondaryActor;

	public int m_eventGuid;

	public ClientGameModeEvent(GameModeEventType eventType, byte objectGuid, BoardSquare square, ActorData primaryActor, ActorData secondaryActor, int eventGuid)
	{
		m_eventType = eventType;
		m_objectGuid = objectGuid;
		m_square = square;
		m_primaryActor = primaryActor;
		m_secondaryActor = secondaryActor;
		m_eventGuid = eventGuid;
	}

	public void ExecuteClientGameModeEvent()
	{
		if (GameModeUtils.IsCtfGameModeEvent(this))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (CaptureTheFlag.Get() != null)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								CaptureTheFlag.Get().ExecuteClientGameModeEvent(this);
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (!GameModeUtils.IsCtcGameModeEvent(this))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (CollectTheCoins.Get() != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					CollectTheCoins.Get().ExecuteClientGameModeEvent(this);
					return;
				}
			}
			return;
		}
	}
}
