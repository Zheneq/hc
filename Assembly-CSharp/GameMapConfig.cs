using System;

[Serializable]
public class GameMapConfig
{
	public bool IsActive;

	public string Map;

	public GameMapConfig Clone()
	{
		return (GameMapConfig)MemberwiseClone();
	}
}
