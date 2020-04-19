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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scamp_ChatterEventOverrider.OnSubmitChatter(IChatterData, GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
			if (this.m_syncComp != null)
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
				if (!string.IsNullOrEmpty(this.m_syncComp.m_noSuitChatterEventOverride))
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
					if (!this.m_syncComp.IsSuitModelActive())
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						chatter.GetCommonData().SetAudioEventOverride(this.m_syncComp.m_noSuitChatterEventOverride);
					}
					else
					{
						string audioEventOverride = chatter.GetCommonData().GetAudioEventOverride();
						if (audioEventOverride != null)
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
							if (audioEventOverride.Equals(this.m_syncComp.m_noSuitChatterEventOverride))
							{
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								chatter.GetCommonData().ClearAudioEventOverride();
							}
						}
					}
				}
			}
		}
	}
}
