using System;

public class Scamp_ChatterEventOverrider : ChatterEventOverrider
{
	private Scamp_SyncComponent m_syncComp;

	public Scamp_ChatterEventOverrider(Scamp_SyncComponent syncComp)
	{
		this.m_syncComp = syncComp;
	}

	public override void OnSubmitChatter(IChatterData chatter, GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (GameFlowData.Get() != null)
		{
			if (this.m_syncComp != null)
			{
				if (!string.IsNullOrEmpty(this.m_syncComp.m_noSuitChatterEventOverride))
				{
					if (!this.m_syncComp.IsSuitModelActive())
					{
						chatter.GetCommonData().SetAudioEventOverride(this.m_syncComp.m_noSuitChatterEventOverride);
					}
					else
					{
						string audioEventOverride = chatter.GetCommonData().GetAudioEventOverride();
						if (audioEventOverride != null)
						{
							if (audioEventOverride.Equals(this.m_syncComp.m_noSuitChatterEventOverride))
							{
								chatter.GetCommonData().ClearAudioEventOverride();
							}
						}
					}
				}
			}
		}
	}
}
