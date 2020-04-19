using System;
using UnityEngine;

public class SniperOverwatchSatelliteActionSequence : Sequence
{
	public SniperOverwatchSatelliteActionSequence.SatelliteAction m_action;

	private bool m_processedAction;

	public override void FinishSetup()
	{
		this.ProcessAction();
	}

	private bool Finished()
	{
		return this.m_processedAction;
	}

	private void ProcessAction()
	{
		this.m_processedAction = true;
		GameObject gameObject = SequenceManager.Get().FindTempSatellite(base.Source);
		if (gameObject != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperOverwatchSatelliteActionSequence.ProcessAction()).MethodHandle;
			}
			SniperOverwatchSatellite component = gameObject.GetComponent<SniperOverwatchSatellite>();
			if (component != null)
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
				if (this.m_action == SniperOverwatchSatelliteActionSequence.SatelliteAction.Attack && base.Target != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					component.TriggerAttack(base.Target.gameObject);
				}
			}
		}
	}

	public enum SatelliteAction
	{
		Attack
	}
}
