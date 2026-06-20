using UnityEngine;

public class ArcingProjectileSequence : SplineProjectileSequence
{
	[Separator("Arc's highest point")]
	public float m_maxHeight;
	[Separator("Use hit target's position as destination? (vs target pos passed from ability)")]
	public bool m_useTargetHitPos;
	public bool m_useLastTargetHitPos;
	[Space(10f)]
	public bool m_reverseDirection;
	[Separator("Start Height Offset")]
	public float m_startPosYOffset;
	[Separator("Destination Height")]
	public bool m_destPosUseSameHeightAsStart;
	public float m_yOffset;
	[Header("-- For destination height in case of no targets (yOffset still appiles after adjustment) --")]
	public bool m_destPosUseSameHeightIfNoTargets;
	public bool m_destPosUseGroundHeightIfNoTargets;
	public bool m_destPosAlwaysUseGroundHeight;

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (m_maxHeight != 0f)
		{
			m_doHitsAsProjectileTravels = false;
		}
	}

	internal override Vector3 GetSequencePos()
	{
		return m_fx != null
			? m_fx.transform.position
			: Vector3.zero;
	}

	internal virtual Vector3 GetStartPos()
	{
		return m_fxJoint.m_jointObject.transform.position;
	}

	internal override Vector3[] GetSplinePath(int curIndex, int maxIndex)
	{
		Vector3 startPos = GetStartPos();
		if (m_useOverrideStartPos)
		{
			startPos = m_overrideStartPos;
		}
		startPos.y += m_startPosYOffset;
		Vector3[] array = new Vector3[5];
		if (m_maxHeight == 0f)
		{
			Vector3 endPos;
			if (m_useTargetHitPos)
			{
				int targetIndex = 0;
				if (m_useLastTargetHitPos)
				{
					targetIndex = Mathf.Max(0, Targets.Length - 1);
				}
				endPos = GetTargetHitPosition(targetIndex, m_hitPosJoint);
			}
			else
			{
				endPos = TargetPos;
				endPos.y += m_yOffset;
			}
			if (m_destPosUseSameHeightAsStart)
			{
				endPos.y = startPos.y;
			}
			if (m_destPosAlwaysUseGroundHeight)
			{
				endPos.y = Board.Get().BaselineHeight + m_yOffset;
			}
			else if (Targets == null || Targets.Length == 0)
			{
				if (m_destPosUseSameHeightIfNoTargets)
				{
					endPos.y = startPos.y;
				}
				else if (m_destPosUseGroundHeightIfNoTargets)
				{
					endPos.y = Board.Get().BaselineHeight + m_yOffset;
				}
			}
			Vector3 moveVector = endPos - startPos;
			array[0] = startPos - moveVector;
			array[1] = startPos;
			array[2] = (startPos + endPos) * 0.5f;
			array[3] = endPos;
			array[4] = endPos + moveVector;
		}
		else
		{
			Vector3 vector3 = m_useTargetHitPos
				? GetTargetHitPosition(0, m_hitPosJoint)
				: TargetPos;
			if (m_destPosUseSameHeightAsStart)
			{
				vector3.y = startPos.y;
			}
			array[0] = startPos + Vector3.down * m_maxHeight;
			array[1] = startPos;
			array[2] = (startPos + vector3) * 0.5f + Vector3.up * m_maxHeight;
			array[3] = vector3;
			array[4] = vector3 + Vector3.down * m_maxHeight;
		}
		if (m_reverseDirection)
		{
			Vector3 tmp = array[0];
			array[0] = array[4];
			array[4] = tmp;
			tmp = array[1];
			array[1] = array[3];
			array[3] = tmp;
		}
		return array;
	}
}
