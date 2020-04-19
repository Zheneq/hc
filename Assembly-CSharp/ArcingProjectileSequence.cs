using System;
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
		if (this.m_maxHeight != 0f)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcingProjectileSequence.FinishSetup()).MethodHandle;
			}
			this.m_doHitsAsProjectileTravels = false;
		}
	}

	internal override Vector3 GetSequencePos()
	{
		if (this.m_fx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcingProjectileSequence.GetSequencePos()).MethodHandle;
			}
			return this.m_fx.transform.position;
		}
		return Vector3.zero;
	}

	internal virtual Vector3 GetStartPos()
	{
		return this.m_fxJoint.m_jointObject.transform.position;
	}

	internal override Vector3[] GetSplinePath(int curIndex, int maxIndex)
	{
		Vector3 vector = this.GetStartPos();
		if (this.m_useOverrideStartPos)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcingProjectileSequence.GetSplinePath(int, int)).MethodHandle;
			}
			vector = this.m_overrideStartPos;
		}
		vector.y += this.m_startPosYOffset;
		Vector3[] array = new Vector3[5];
		if (this.m_maxHeight == 0f)
		{
			Vector3 vector2;
			if (this.m_useTargetHitPos)
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
				int targetIndex = 0;
				if (this.m_useLastTargetHitPos)
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
					targetIndex = Mathf.Max(0, base.Targets.Length - 1);
				}
				vector2 = base.GetTargetHitPosition(targetIndex, this.m_hitPosJoint);
			}
			else
			{
				vector2 = base.TargetPos;
				vector2.y += this.m_yOffset;
			}
			if (this.m_destPosUseSameHeightAsStart)
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
				vector2.y = vector.y;
			}
			if (this.m_destPosAlwaysUseGroundHeight)
			{
				vector2.y = (float)Board.\u000E().BaselineHeight + this.m_yOffset;
			}
			else
			{
				if (base.Targets != null)
				{
					if (base.Targets.Length != 0)
					{
						goto IL_168;
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (this.m_destPosUseSameHeightIfNoTargets)
				{
					vector2.y = vector.y;
				}
				else if (this.m_destPosUseGroundHeightIfNoTargets)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					vector2.y = (float)Board.\u000E().BaselineHeight + this.m_yOffset;
				}
			}
			IL_168:
			Vector3 b = vector2 - vector;
			array[0] = vector - b;
			array[1] = vector;
			array[2] = (vector + vector2) * 0.5f;
			array[3] = vector2;
			array[4] = vector2 + b;
		}
		else
		{
			Vector3 vector3;
			if (this.m_useTargetHitPos)
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
				vector3 = base.GetTargetHitPosition(0, this.m_hitPosJoint);
			}
			else
			{
				vector3 = base.TargetPos;
			}
			if (this.m_destPosUseSameHeightAsStart)
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
				vector3.y = vector.y;
			}
			array[0] = vector + Vector3.down * this.m_maxHeight;
			array[1] = vector;
			array[2] = (vector + vector3) * 0.5f + Vector3.up * this.m_maxHeight;
			array[3] = vector3;
			array[4] = vector3 + Vector3.down * this.m_maxHeight;
		}
		if (this.m_reverseDirection)
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
