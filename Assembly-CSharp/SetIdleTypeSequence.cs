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
			this.SetIdleType();
		}
	}

	private void SetIdleType()
	{
		Animator modelAnimator = base.Caster.GetModelAnimator();
		if (modelAnimator != null)
		{
			int num;
			if (this.m_useAltIntIdleType)
			{
				num = this.m_altIntIdleType;
			}
			else
			{
				num = (int)this.m_idleType;
			}
			int value = num;
			if (!this.m_animParameter.IsNullOrEmpty())
			{
				modelAnimator.SetInteger(this.m_animParameter, value);
			}
			if (!this.m_triggerAnimParameter.IsNullOrEmpty())
			{
				modelAnimator.SetTrigger(this.m_triggerAnimParameter);
			}
		}
	}

	private void RestoreIdleType()
	{
		Animator modelAnimator = base.Caster.GetModelAnimator();
		if (modelAnimator != null && !this.m_animParameter.IsNullOrEmpty())
		{
			modelAnimator.SetInteger(this.m_animParameter, 0);
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.SetIdleType();
		}
		else if (this.m_endEvent == parameter)
		{
			this.RestoreIdleType();
		}
	}

	private void OnDisable()
	{
		if (this.m_initialized)
		{
			if (this.m_restoreOnDisable)
			{
				this.RestoreIdleType();
			}
		}
	}
}
