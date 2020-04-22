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
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SniperOverwatchSatellite component = gameObject.GetComponent<SniperOverwatchSatellite>();
			if (!(component != null))
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (m_action == SatelliteAction.Attack && base.Target != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						component.TriggerAttack(base.Target.gameObject);
						return;
					}
				}
				return;
			}
		}
	}
}
