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
		return num * num * num * this.st + 3f * num * num * t * this.ctrl1 + 3f * num * t * t * this.ctrl2 + t * t * t * this.en;
	}

	public Vector3 Velocity(float t)
	{
		return (-3f * this.st + 9f * this.ctrl1 - 9f * this.ctrl2 + 3f * this.en) * t * t + (6f * this.st - 12f * this.ctrl1 + 6f * this.ctrl2) * t - 3f * this.st + 3f * this.ctrl1;
	}

	public void GizmoDraw(float t)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.st, this.ctrl1);
		Gizmos.DrawLine(this.ctrl2, this.en);
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
