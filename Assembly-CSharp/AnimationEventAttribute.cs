using System;
using UnityEngine;

public class AnimationEventAttribute : Attribute
{
	public float R
	{
		get;
		set;
	}

	public float G
	{
		get;
		set;
	}

	public float B
	{
		get;
		set;
	}

	public Color DisplayColor => new Color(R, G, B);

	public bool IsAudio
	{
		get;
		set;
	}

	public AnimationEventAttribute()
	{
		R = 1f;
		G = 1f;
		B = 1f;
		IsAudio = false;
	}
}
