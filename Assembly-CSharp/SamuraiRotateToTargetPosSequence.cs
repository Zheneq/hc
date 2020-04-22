using UnityEngine;

public class SamuraiRotateToTargetPosSequence : Sequence
{
	[AnimEventPicker]
	[Separator("Anim Event to start rotating", true)]
	public Object m_rotateSignalAnimEvent;

	[Separator("Rotation duration - if less than or equal to 0 it's instant", true)]
	public float m_rotateDuration;

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(parameter == m_rotateSignalAnimEvent) || !(base.Caster != null))
		{
			return;
		}
		while (true)
		{
			if (m_rotateDuration <= 0f)
			{
				base.Caster.TurnToPositionInstant(base.TargetPos);
			}
			else
			{
				base.Caster.TurnToPosition(base.TargetPos, m_rotateDuration);
			}
			return;
		}
	}
}
