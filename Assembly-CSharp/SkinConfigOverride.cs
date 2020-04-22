using System;

[Serializable]
public class SkinConfigOverride
{
	public int SkinIndex;

	public int PatternIndex;

	public int ColorIndex;

	public bool Allowed;

	public SkinConfigOverride Clone()
	{
		return (SkinConfigOverride)MemberwiseClone();
	}
}
