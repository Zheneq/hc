using System;

[Serializable]
public class EloDatum : ICloneable
{
	public EloDatum()
	{
		this.Elo = 1500f;
		this.Count = 0;
	}

	public float Elo { get; set; }

	public int Count { get; set; }

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
