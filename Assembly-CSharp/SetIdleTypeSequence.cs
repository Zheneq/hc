using System;
using UnityEngine;

public class SetIdleTypeSequence : Sequence
{
	public string m_animParameter = "IdleType";

	public string m_triggerAnimParameter;

	public ActorMovement.IdleType m_idleType;

	public bool m_useAltIntIdleType;

	public int m_altIntIdleType;

	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_endEvent;

	public bool m_restoreOnDisable = true;

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SetIdleTypeSequence.FinishSetup()).MethodHandle;
			}
			this.SetIdleType();
		}
	}

	private void SetIdleType()
	{
		Animator animator = base.Caster.\u000E();
		if (animator != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SetIdleTypeSequence.SetIdleType()).MethodHandle;
			}
			int num;
			if (this.m_useAltIntIdleType)
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
				num = this.m_altIntIdleType;
			}
			else
			{
				num = (int)this.m_idleType;
			}
			int value = num;
			if (!this.m_animParameter.IsNullOrEmpty())
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
				animator.SetInteger(this.m_animParameter, value);
			}
			if (!this.m_triggerAnimParameter.IsNullOrEmpty())
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
				animator.SetTrigger(this.m_triggerAnimParameter);
			}
		}
	}

	private void RestoreIdleType()
	{
		Animator animator = base.Caster.\u000E();
		if (animator != null && !this.m_animParameter.IsNullOrEmpty())
		{
			animator.SetInteger(this.m_animParameter, 0);
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SetIdleTypeSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.SetIdleType();
		}
		else if (this.m_endEvent == parameter)
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
			this.RestoreIdleType();
		}
	}

	private void OnDisable()
	{
		if (this.m_initialized)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SetIdleTypeSequence.OnDisable()).MethodHandle;
			}
			if (this.m_restoreOnDisable)
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
				this.RestoreIdleType();
			}
		}
	}
}
