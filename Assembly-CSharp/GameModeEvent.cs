// ROGUES
// SERVER
using System.Linq;

// added in rogues
#if SERVER
public class GameModeEvent
{
	public static int s_nextEventGuid;

	public GameModeEventType m_eventType;
	public byte m_objectGuid;
	public BoardSquare m_square;
	public ActorData m_primaryActor;
	public ActorData m_secondaryActor;
	public int m_eventGuid;

	public GameModeEvent()
	{
		m_eventGuid = s_nextEventGuid++;
	}

	public void ExecuteGameModeEvent()
	{
		// TODO CTF CTC
		//if (GameModeUtils.IsCtfGameModeEvent(this))
		//{
		//	if (CaptureTheFlag.Get() != null)
		//	{
		//		CaptureTheFlag.Get().ExecuteServerGameModeEvent(this);
		//		return;
		//	}
		//}
		//else if (GameModeUtils.IsCtcGameModeEvent(this) && CollectTheCoins.Get() != null)
		//{
		//	CollectTheCoins.Get().ExecuteServerGameModeEvent(this);
		//}
	}

	public bool AppliesStatusToActor(StatusType status, ActorData actor)
	{
		if (actor == null)
		{
			return false;
		}
		if (status == StatusType.INVALID)
		{
			return false;
		}
		bool result = false;
		if (CaptureTheFlag.Get() != null)
		{
			if (m_eventType == GameModeEventType.Ctf_FlagPickedUp)
			{
				if (actor == m_primaryActor && CaptureTheFlag.Get().m_flagHolderEffect.m_applyEffect && CaptureTheFlag.Get().m_flagHolderEffect.m_effectData.m_statusChanges.Contains(status))
				{
					result = true;
				}
			}
			else if (m_eventType == GameModeEventType.Ctf_FlagDropped)
			{
				if (actor == m_primaryActor && CaptureTheFlag.Get().m_onDroppedFlagEffect.m_applyEffect && CaptureTheFlag.Get().m_onDroppedFlagEffect.m_effectData.m_statusChanges.Contains(status))
				{
					result = true;
				}
			}
			else if (m_eventType == GameModeEventType.Ctf_FlagSentToSpawn)
			{
				if (actor == m_primaryActor && CaptureTheFlag.Get().m_onReturnedFlagEffect.m_applyEffect && CaptureTheFlag.Get().m_onReturnedFlagEffect.m_effectData.m_statusChanges.Contains(status))
				{
					result = true;
				}
			}
			else if (m_eventType == GameModeEventType.Ctf_FlagTurnedIn && actor == m_primaryActor && CaptureTheFlag.Get().m_onTurnedInFlagEffect.m_applyEffect && CaptureTheFlag.Get().m_onTurnedInFlagEffect.m_effectData.m_statusChanges.Contains(status))
			{
				result = true;
			}
		}
		return result;
	}
}
#endif
