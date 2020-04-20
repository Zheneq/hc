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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TempSatelliteActionSequence.FinishSetup()).MethodHandle;
			}
			GameObject gameObject = SequenceManager.Get().FindTempSatellite(base.Source);
			if (gameObject != null)
			{
				TempSatellite component = gameObject.GetComponent<TempSatellite>();
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
					component.SetRotation(base.TargetRotation);
				}
			}
		}
		bool flag;
		if (this.m_ignoreStartEventIfActorRagdoll)
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
			flag = (base.Caster.IsModelAnimatorDisabled() || base.Caster.IsDead());
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (!(this.m_startActionEvent == null))
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
			if (!flag2)
			{
				return;
			}
		}
		if (this.m_startDelayTime <= 0f)
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TempSatelliteActionSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.ProcessAction();
		}
	}

	private void ProcessAction()
	{
		if (!this.m_processedAction)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TempSatelliteActionSequence.ProcessAction()).MethodHandle;
			}
			this.m_processedAction = true;
			GameObject gameObject = SequenceManager.Get().FindTempSatellite(base.Source);
			if (gameObject != null)
			{
				TempSatellite component = gameObject.GetComponent<TempSatellite>();
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
					if (this.m_action == TempSatelliteActionSequence.SatelliteAction.Attack)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TempSatelliteActionSequence.Update()).MethodHandle;
			}
			if (this.m_timeToProcessAction > 0f)
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
				if (!this.m_processedAction && GameTime.time > this.m_timeToProcessAction)
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
