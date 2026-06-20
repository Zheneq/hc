using UnityEngine;

public class TempSatelliteActionSequence : Sequence
{
	public enum SatelliteAction
	{
		Attack
	}

	public SatelliteAction m_action;

	public bool m_setRotationOnInit;

	[Header("-- Timing --")]
	public float m_startDelayTime = -1f;

	[AnimEventPicker]
	public Object m_startActionEvent;

	public bool m_ignoreStartEventIfActorRagdoll;

	private bool m_processedAction;

	private float m_timeToProcessAction = -1f;

	public override void FinishSetup()
	{
		if (m_setRotationOnInit)
		{
			GameObject gameObject = SequenceManager.Get().FindTempSatellite(base.Source);
			if (gameObject != null)
			{
				TempSatellite component = gameObject.GetComponent<TempSatellite>();
				if (component != null)
				{
					component.SetRotation(base.TargetRotation);
				}
			}
		}
		bool flag = false;
		int num;
		if (m_ignoreStartEventIfActorRagdoll)
		{
			num = ((base.Caster.IsInRagdoll() || base.Caster.IsDead()) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		flag = ((byte)num != 0);
		if (!(m_startActionEvent == null))
		{
			if (!flag)
			{
				return;
			}
		}
		if (m_startDelayTime <= 0f)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					ProcessAction();
					return;
				}
			}
		}
		m_timeToProcessAction = GameTime.time + m_startDelayTime;
	}

	private bool Finished()
	{
		return m_processedAction;
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(parameter == m_startActionEvent))
		{
			return;
		}
		while (true)
		{
			ProcessAction();
			return;
		}
	}

	private void ProcessAction()
	{
		if (m_processedAction)
		{
			return;
		}
		while (true)
		{
			m_processedAction = true;
			GameObject gameObject = SequenceManager.Get().FindTempSatellite(base.Source);
			if (!(gameObject != null))
			{
				return;
			}
			TempSatellite component = gameObject.GetComponent<TempSatellite>();
			if (!(component != null))
			{
				return;
			}
			while (true)
			{
				if (m_action != 0)
				{
					return;
				}
				while (true)
				{
					if (base.Target != null)
					{
						component.TriggerAttack(base.Target.gameObject);
					}
					return;
				}
			}
		}
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			if (!(m_timeToProcessAction > 0f))
			{
				return;
			}
			while (true)
			{
				if (!m_processedAction && GameTime.time > m_timeToProcessAction)
				{
					while (true)
					{
						m_timeToProcessAction = -1f;
						ProcessAction();
						return;
					}
				}
				return;
			}
		}
	}
}
