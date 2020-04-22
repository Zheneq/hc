using UnityEngine;

public class SniperOverwatchSatelliteActionSequence : Sequence
{
	public enum SatelliteAction
	{
		Attack
	}

	public SatelliteAction m_action;

	private bool m_processedAction;

	public override void FinishSetup()
	{
		ProcessAction();
	}

	private bool Finished()
	{
		return m_processedAction;
	}

	private void ProcessAction()
	{
		m_processedAction = true;
		GameObject gameObject = SequenceManager.Get().FindTempSatellite(base.Source);
		if (!(gameObject != null))
		{
			return;
		}
		while (true)
		{
			SniperOverwatchSatellite component = gameObject.GetComponent<SniperOverwatchSatellite>();
			if (!(component != null))
			{
				return;
			}
			while (true)
			{
				if (m_action == SatelliteAction.Attack && base.Target != null)
				{
					while (true)
					{
						component.TriggerAttack(base.Target.gameObject);
						return;
					}
				}
				return;
			}
		}
	}
}
