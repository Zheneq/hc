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
		return string.Format("IsLoaded={0} LoadingProgress {1}% = Level {2}% + Character {3}% + VfxPreload {4}% + Spawning {5}% (updateCount {6})", new object[]
		{
			this.IsLoaded,
			this.TotalLoadingProgress,
			this.LevelLoadingProgress,
			this.CharacterLoadingProgress,
			this.VfxLoadingProgress,
			this.SpawningProgress,
			this.LoadingProgressUpdateCount
		});
	}
}
