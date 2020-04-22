using System;
using UnityEngine;

[Serializable]
public class QuadBez
{
	public Vector3 st;

	public Vector3 en;

	public Vector3 ctrl;

	public QuadBez(Vector3 st, Vector3 en, Vector3 ctrl)
	{
		this.st = st;
		this.en = en;
		this.ctrl = ctrl;
	}

	public Vector3 Interp(float t)
	{
		float num = 1f - t;
		return num * num * st + 2f * num * t * ctrl + t * t * en;
	}

	public Vector3 Velocity(float t)
	{
		return (2f * st - 4f * ctrl + 2f * en) * t + 2f * ctrl - 2f * st;
	}

	public void GizmoDraw(float t)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(st, ctrl);
		Gizmos.DrawLine(ctrl, en);
		Gizmos.color = Color.white;
		Vector3 to = st;
		for (int i = 1; i <= 20; i++)
		{
			float t2 = (float)i / 20f;
			Vector3 vector = Interp(t2);
			Gizmos.DrawLine(vector, to);
			to = vector;
		}
		Gizmos.color = Color.blue;
		Vector3 vector2 = Interp(t);
		Gizmos.DrawLine(vector2, vector2 + Velocity(t));
	}
}
