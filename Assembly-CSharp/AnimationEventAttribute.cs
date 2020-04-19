using System;
using UnityEngine;

public class AnimationEventAttribute : Attribute
{
	public AnimationEventAttribute()
	{
		this.R = 1f;
		this.G = 1f;
		this.B = 1f;
		this.IsAudio = false;
	}

	public float R { get; set; }

	public float G { get; set; }

	public float B { get; set; }

	public Color DisplayColor
	{
		get
		{
			return new Color(this.R, this.G, this.B);
		}
	}

	public bool IsAudio { get; set; }
}
