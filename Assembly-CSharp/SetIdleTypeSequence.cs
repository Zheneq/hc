using UnityEngine;

public class SetIdleTypeSequence : Sequence
{
	public string m_animParameter = "IdleType";

	public string m_triggerAnimParameter;

	public ActorMovement.IdleType m_idleType;

	public bool m_useAltIntIdleType;

	public int m_altIntIdleType;

	[AnimEventPicker]
	public Object m_startEvent;

	[AnimEventPicker]
	public Object m_endEvent;

	public bool m_restoreOnDisable = true;

	public override void FinishSetup()
	{
		if (!(m_startEvent == null))
		{
			return;
		}
		while (true)
		{
			SetIdleType();
			return;
		}
	}

	private void SetIdleType()
	{
		Animator modelAnimator = base.Caster.GetModelAnimator();
		if (!(modelAnimator != null))
		{
			return;
		}
		while (true)
		{
			int num;
			if (m_useAltIntIdleType)
			{
				num = m_altIntIdleType;
			}
			else
			{
				num = (int)m_idleType;
			}
			int value = num;
			if (!m_animParameter.IsNullOrEmpty())
			{
				modelAnimator.SetInteger(m_animParameter, value);
			}
			if (!m_triggerAnimParameter.IsNullOrEmpty())
			{
				while (true)
				{
					modelAnimator.SetTrigger(m_triggerAnimParameter);
					return;
				}
			}
			return;
		}
	}

	private void RestoreIdleType()
	{
		Animator modelAnimator = base.Caster.GetModelAnimator();
		if (modelAnimator != null && !m_animParameter.IsNullOrEmpty())
		{
			modelAnimator.SetInteger(m_animParameter, 0);
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					SetIdleType();
					return;
				}
			}
		}
		if (!(m_endEvent == parameter))
		{
			return;
		}
		while (true)
		{
			RestoreIdleType();
			return;
		}
	}

	private void OnDisable()
	{
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			if (m_restoreOnDisable)
			{
				while (true)
				{
					RestoreIdleType();
					return;
				}
			}
			return;
		}
	}
}
