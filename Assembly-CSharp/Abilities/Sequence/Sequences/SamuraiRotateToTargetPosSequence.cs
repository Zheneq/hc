// ROGUES
// SERVER
using UnityEngine;

// identical in reactor and rogues
public class SamuraiRotateToTargetPosSequence : Sequence
{
	[AnimEventPicker]
	[Separator("Anim Event to start rotating")]
	public Object m_rotateSignalAnimEvent;
	[Separator("Rotation duration - if less than or equal to 0 it's instant")]
	public float m_rotateDuration;

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (parameter == m_rotateSignalAnimEvent && Caster != null)
		{
			if (m_rotateDuration <= 0f)
			{
				Caster.TurnToPositionInstant(TargetPos);
			}
			else
			{
				Caster.TurnToPosition(TargetPos, m_rotateDuration);
			}
		}
	}
}
