using System;

[Serializable]
public class PassiveEventData
{
	public PassiveEventType m_eventType;

	public Ability[] m_causalAbilities;

	public PassiveEventResponse m_responseOnSelf;

	public PassiveEventResponse m_responseOnOther;
}
