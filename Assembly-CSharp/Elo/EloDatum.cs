using System;

[Serializable]
public class EloDatum : ICloneable
{
    public float Elo { get; set; }
    public int Count { get; set; }

    public EloDatum()
    {
        Elo = 1500f;
        Count = 0;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}