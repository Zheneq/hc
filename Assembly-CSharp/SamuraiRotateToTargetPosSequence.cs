using System;
using UnityEngine;

public class SamuraiRotateToTargetPosSequence : Sequence
{
	[AnimEventPicker]
	[Separator("Anim Event to start rotating", true)]
	public UnityEngine.Object m_rotateSignalAnimEvent;

	[Separator("Rotation duration - if less than or equal to 0 it's instant", true)]
	public float m_rotateDuration;

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (parameter == this.m_rotateSignalAnimEvent && base.Caster != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiRotateToTargetPosSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			if (this.m_rotateDuration <= 0f)
			{
				base.Caster.TurnToPositionInstant(base.TargetPos);
			}
			else
			{
				base.Caster.TurnToPosition(base.TargetPos, this.m_rotateDuration);
			}
		}
	}
}
