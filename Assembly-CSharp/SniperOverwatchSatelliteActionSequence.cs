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
			SniperOverwatchSatellite component = gameObject.GetComponent<SniperOverwatchSatellite>();
			if (component != null)
			{
				if (this.m_action == SniperOverwatchSatelliteActionSequence.SatelliteAction.Attack && base.Target != null)
				{
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
