public class Scamp_ChatterEventOverrider : ChatterEventOverrider
{
	private Scamp_SyncComponent m_syncComp;

	public Scamp_ChatterEventOverrider(Scamp_SyncComponent syncComp)
	{
		m_syncComp = syncComp;
	}

	public override void OnSubmitChatter(IChatterData chatter, GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (!(m_syncComp != null))
			{
				return;
			}
			while (true)
			{
				if (string.IsNullOrEmpty(m_syncComp.m_noSuitChatterEventOverride))
				{
					return;
				}
				while (true)
				{
					if (!m_syncComp.IsSuitModelActive())
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								chatter.GetCommonData().SetAudioEventOverride(m_syncComp.m_noSuitChatterEventOverride);
								return;
							}
						}
					}
					string audioEventOverride = chatter.GetCommonData().GetAudioEventOverride();
					if (audioEventOverride == null)
					{
						return;
					}
					while (true)
					{
						if (audioEventOverride.Equals(m_syncComp.m_noSuitChatterEventOverride))
						{
							while (true)
							{
								chatter.GetCommonData().ClearAudioEventOverride();
								return;
							}
						}
						return;
					}
				}
			}
		}
	}
}
