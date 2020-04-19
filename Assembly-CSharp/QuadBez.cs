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
		return num * num * this.st + 2f * num * t * this.ctrl + t * t * this.en;
	}

	public Vector3 Velocity(float t)
	{
		return (2f * this.st - 4f * this.ctrl + 2f * this.en) * t + 2f * this.ctrl - 2f * this.st;
	}

	public void GizmoDraw(float t)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.st, this.ctrl);
		Gizmos.DrawLine(this.ctrl, this.en);
		Gizmos.color = Color.white;
		Vector3 to = this.st;
		for (int i = 1; i <= 0x14; i++)
		{
			float t2 = (float)i / 20f;
			Vector3 vector = this.Interp(t2);
			Gizmos.DrawLine(vector, to);
			to = vector;
		}
		Gizmos.color = Color.blue;
		Vector3 vector2 = this.Interp(t);
		Gizmos.DrawLine(vector2, vector2 + this.Velocity(t));
	}
}
