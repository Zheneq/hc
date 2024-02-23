using System;
using System.Text;

[Serializable]
public class GameLoadingState
{
	public bool IsReady;

	public bool IsLoaded;

	public byte TotalLoadingProgress;

	public byte LevelLoadingProgress;

	public byte CharacterLoadingProgress;

	public byte VfxLoadingProgress;

	public byte SpawningProgress;

	public ushort LoadingProgressUpdateCount;

	public override string ToString()
	{
		return new StringBuilder().Append("IsLoaded=").Append(IsLoaded).Append(" LoadingProgress ").Append(TotalLoadingProgress).Append("% = Level ").Append(LevelLoadingProgress).Append("% + Character ").Append(CharacterLoadingProgress).Append("% + VfxPreload ").Append(VfxLoadingProgress).Append("% + Spawning ").Append(SpawningProgress).Append("% (updateCount ").Append(LoadingProgressUpdateCount).Append(")").ToString();
	}
}
