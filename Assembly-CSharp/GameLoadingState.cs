using System;

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
		return $"IsLoaded={IsLoaded} LoadingProgress {TotalLoadingProgress}% = Level {LevelLoadingProgress}% + Character {CharacterLoadingProgress}% + VfxPreload {VfxLoadingProgress}% + Spawning {SpawningProgress}% (updateCount {LoadingProgressUpdateCount})";
	}
}
