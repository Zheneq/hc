using System;
using UnityEngine;

[Serializable]
public class CRSpline
{
	public Vector3[] pts;

	public CRSpline(params Vector3[] pts)
	{
		this.pts = new Vector3[pts.Length];
		Array.Copy(pts, this.pts, pts.Length);
	}

	public int Section(float t)
	{
		int num = pts.Length - 3;
		return Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
	}

	public Vector3 Interp(float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 a = pts[num2];
		Vector3 a2 = pts[num2 + 1];
		Vector3 vector = pts[num2 + 2];
		Vector3 b = pts[num2 + 3];
		return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num3 * num3 * num3) + (2f * a - 5f * a2 + 4f * vector - b) * (num3 * num3) + (-a + vector) * num3 + 2f * a2);
	}

	public Vector3 Velocity(float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 a = pts[num2];
		Vector3 a2 = pts[num2 + 1];
		Vector3 a3 = pts[num2 + 2];
		Vector3 b = pts[num2 + 3];
		return 1.5f * (-a + 3f * a2 - 3f * a3 + b) * (num3 * num3) + (2f * a - 5f * a2 + 4f * a3 - b) * num3 + 0.5f * a3 - 0.5f * a;
	}

	public void GizmoDraw(float t)
	{
		Gizmos.color = Color.white;
		Vector3 to = Interp(0f);
		for (int i = 1; i <= 20; i++)
		{
			float t2 = (float)i / 20f;
			Vector3 vector = Interp(t2);
			Gizmos.DrawLine(vector, to);
			to = vector;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Gizmos.color = Color.blue;
			Vector3 vector2 = Interp(t);
			Gizmos.DrawLine(vector2, vector2 + Velocity(t));
			return;
		}
	}
}
