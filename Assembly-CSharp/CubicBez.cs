using System;
using UnityEngine;

[Serializable]
public class CubicBez
{
	public Vector3 st;

	public Vector3 en;

	public Vector3 ctrl1;

	public Vector3 ctrl2;

	public CubicBez(Vector3 st, Vector3 en, Vector3 ctrl1, Vector3 ctrl2)
	{
		this.st = st;
		this.en = en;
		this.ctrl1 = ctrl1;
		this.ctrl2 = ctrl2;
	}

	public Vector3 Interp(float t)
	{
		float num = 1f - t;
		return num * num * num * st + 3f * num * num * t * ctrl1 + 3f * num * t * t * ctrl2 + t * t * t * en;
	}

	public Vector3 Velocity(float t)
	{
		return (-3f * st + 9f * ctrl1 - 9f * ctrl2 + 3f * en) * t * t + (6f * st - 12f * ctrl1 + 6f * ctrl2) * t - 3f * st + 3f * ctrl1;
	}

	public void GizmoDraw(float t)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(st, ctrl1);
		Gizmos.DrawLine(ctrl2, en);
		Gizmos.color = Color.white;
		Vector3 to = st;
		for (int i = 1; i <= 20; i++)
		{
			float t2 = (float)i / 20f;
			Vector3 vector = Interp(t2);
			Gizmos.DrawLine(vector, to);
			to = vector;
		}
		while (true)
		{
			Gizmos.color = Color.blue;
			Vector3 vector2 = Interp(t);
			Gizmos.DrawLine(vector2, vector2 + Velocity(t));
			return;
		}
	}
}
