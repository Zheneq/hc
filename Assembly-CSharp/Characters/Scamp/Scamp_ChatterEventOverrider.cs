public class Scamp_ChatterEventOverrider : ChatterEventOverrider
{
	private Scamp_SyncComponent m_syncComp;

	public Scamp_ChatterEventOverrider(Scamp_SyncComponent syncComp)
	{
		m_syncComp = syncComp;
	}

	public override void OnSubmitChatter(IChatterData chatter, GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (GameFlowData.Get() == null
		    || m_syncComp == null
		    || string.IsNullOrEmpty(m_syncComp.m_noSuitChatterEventOverride))
		{
			return;
		}
		
		if (!m_syncComp.IsSuitModelActive())
		{
			chatter.GetCommonData().SetAudioEventOverride(m_syncComp.m_noSuitChatterEventOverride);
		}
		else
		{
			string audioEventOverride = chatter.GetCommonData().GetAudioEventOverride();
			if (audioEventOverride != null && audioEventOverride.Equals(m_syncComp.m_noSuitChatterEventOverride))
			{
				chatter.GetCommonData().ClearAudioEventOverride();
			}
		}
	}
}
