using System;
using UnityEngine;

public class TempSatelliteActionSequence : Sequence
{
	public TempSatelliteActionSequence.SatelliteAction m_action;

	public bool m_setRotationOnInit;

	[Header("-- Timing --")]
	public float m_startDelayTime = -1f;

	[AnimEventPicker]
	public UnityEngine.Object m_startActionEvent;

	public bool m_ignoreStartEventIfActorRagdoll;

	private bool m_processedAction;

	private float m_timeToProcessAction = -1f;

	public override void FinishSetup()
	{
		if (this.m_setRotationOnInit)
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
		bool flag;
		if (this.m_ignoreStartEventIfActorRagdoll)
		{
			flag = (base.Caster.IsModelAnimatorDisabled() || base.Caster.IsDead());
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (!(this.m_startActionEvent == null))
		{
			if (!flag2)
			{
				return;
			}
		}
		if (this.m_startDelayTime <= 0f)
		{
			this.ProcessAction();
		}
		else
		{
			this.m_timeToProcessAction = GameTime.time + this.m_startDelayTime;
		}
	}

	private bool Finished()
	{
		return this.m_processedAction;
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (parameter == this.m_startActionEvent)
		{
			this.ProcessAction();
		}
	}

	private void ProcessAction()
	{
		if (!this.m_processedAction)
		{
			this.m_processedAction = true;
			GameObject gameObject = SequenceManager.Get().FindTempSatellite(base.Source);
			if (gameObject != null)
			{
				TempSatellite component = gameObject.GetComponent<TempSatellite>();
				if (component != null)
				{
					if (this.m_action == TempSatelliteActionSequence.SatelliteAction.Attack)
					{
						if (base.Target != null)
						{
							component.TriggerAttack(base.Target.gameObject);
						}
					}
				}
			}
		}
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			if (this.m_timeToProcessAction > 0f)
			{
				if (!this.m_processedAction && GameTime.time > this.m_timeToProcessAction)
				{
					this.m_timeToProcessAction = -1f;
					this.ProcessAction();
				}
			}
		}
	}

	public enum SatelliteAction
	{
		Attack
	}
}
