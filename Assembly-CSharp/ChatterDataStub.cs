public class ChatterDataStub : IChatterData
{
	private ChatterData m_data;

	public ChatterDataStub(string eventName)
	{
		m_data = new ChatterData
		{
			m_audioEvent = eventName
		};
	}

	public ChatterData GetCommonData()
	{
		return m_data;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.AppStateChanged;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		return true;
	}
}
