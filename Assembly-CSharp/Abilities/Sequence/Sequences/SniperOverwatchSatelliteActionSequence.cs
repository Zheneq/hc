// ROGUES
// SERVER
using UnityEngine;

// same in reactor & rogues
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
		GameObject satelliteObject = SequenceManager.Get().FindTempSatellite(Source);
		if (satelliteObject != null)
		{
			SniperOverwatchSatellite satelliteComp = satelliteObject.GetComponent<SniperOverwatchSatellite>();
			if (satelliteComp != null
			    && m_action == SatelliteAction.Attack
			    && Target != null)
			{
				satelliteComp.TriggerAttack(Target.gameObject);
			}
		}
	}
}
