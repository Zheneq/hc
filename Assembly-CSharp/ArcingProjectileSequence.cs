using UnityEngine;

public class ArcingProjectileSequence : SplineProjectileSequence
{
	[Separator("Arc's highest point", true)]
	public float m_maxHeight;

	[Separator("Use hit target's position as destination? (vs target pos passed from ability)", true)]
	public bool m_useTargetHitPos;

	public bool m_useLastTargetHitPos;

	[Space(10f)]
	public bool m_reverseDirection;

	[Separator("Start Height Offset", true)]
	public float m_startPosYOffset;

	[Separator("Destination Height", true)]
	public bool m_destPosUseSameHeightAsStart;

	public float m_yOffset;

	[Header("-- For destination height in case of no targets (yOffset still appiles after adjustment) --")]
	public bool m_destPosUseSameHeightIfNoTargets;

	public bool m_destPosUseGroundHeightIfNoTargets;

	public bool m_destPosAlwaysUseGroundHeight;

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (m_maxHeight == 0f)
		{
			return;
		}
		while (true)
		{
			m_doHitsAsProjectileTravels = false;
			return;
		}
	}

	internal override Vector3 GetSequencePos()
	{
		if (m_fx != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_fx.transform.position;
				}
			}
		}
		return Vector3.zero;
	}

	internal virtual Vector3 GetStartPos()
	{
		return m_fxJoint.m_jointObject.transform.position;
	}

	internal override Vector3[] GetSplinePath(int curIndex, int maxIndex)
	{
		Vector3 vector = GetStartPos();
		if (m_useOverrideStartPos)
		{
			vector = m_overrideStartPos;
		}
		vector.y += m_startPosYOffset;
		Vector3[] array = new Vector3[5];
		Vector3 vector2;
		if (m_maxHeight == 0f)
		{
			if (m_useTargetHitPos)
			{
				int targetIndex = 0;
				if (m_useLastTargetHitPos)
				{
					targetIndex = Mathf.Max(0, base.Targets.Length - 1);
				}
				vector2 = GetTargetHitPosition(targetIndex, m_hitPosJoint);
			}
			else
			{
				vector2 = base.TargetPos;
				vector2.y += m_yOffset;
			}
			if (m_destPosUseSameHeightAsStart)
			{
				vector2.y = vector.y;
			}
			if (m_destPosAlwaysUseGroundHeight)
			{
				vector2.y = (float)Board.Get().BaselineHeight + m_yOffset;
			}
			else
			{
				if (base.Targets != null)
				{
					if (base.Targets.Length != 0)
					{
						goto IL_0168;
					}
				}
				if (m_destPosUseSameHeightIfNoTargets)
				{
					vector2.y = vector.y;
				}
				else if (m_destPosUseGroundHeightIfNoTargets)
				{
					vector2.y = (float)Board.Get().BaselineHeight + m_yOffset;
				}
			}
			goto IL_0168;
		}
		Vector3 vector3;
		if (m_useTargetHitPos)
		{
			vector3 = GetTargetHitPosition(0, m_hitPosJoint);
		}
		else
		{
			vector3 = base.TargetPos;
		}
		if (m_destPosUseSameHeightAsStart)
		{
			vector3.y = vector.y;
		}
		array[0] = vector + Vector3.down * m_maxHeight;
		array[1] = vector;
		array[2] = (vector + vector3) * 0.5f + Vector3.up * m_maxHeight;
		array[3] = vector3;
		array[4] = vector3 + Vector3.down * m_maxHeight;
		goto IL_02cb;
		IL_0168:
		Vector3 b = vector2 - vector;
		array[0] = vector - b;
		array[1] = vector;
		array[2] = (vector + vector2) * 0.5f;
		array[3] = vector2;
		array[4] = vector2 + b;
		goto IL_02cb;
		IL_02cb:
		if (m_reverseDirection)
		{
			Vector3 vector4 = array[0];
			array[0] = array[4];
			array[4] = vector4;
			vector4 = array[1];
			array[1] = array[3];
			array[3] = vector4;
		}
		return array;
	}
}
