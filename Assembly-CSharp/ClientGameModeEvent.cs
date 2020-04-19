using System;

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
		this.m_eventType = eventType;
		this.m_objectGuid = objectGuid;
		this.m_square = square;
		this.m_primaryActor = primaryActor;
		this.m_secondaryActor = secondaryActor;
		this.m_eventGuid = eventGuid;
	}

	public void ExecuteClientGameModeEvent()
	{
		if (GameModeUtils.IsCtfGameModeEvent(this))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientGameModeEvent.ExecuteClientGameModeEvent()).MethodHandle;
			}
			if (CaptureTheFlag.Get() != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				CaptureTheFlag.Get().ExecuteClientGameModeEvent(this);
			}
		}
		else if (GameModeUtils.IsCtcGameModeEvent(this))
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (CollectTheCoins.Get() != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				CollectTheCoins.Get().ExecuteClientGameModeEvent(this);
			}
		}
	}
}
