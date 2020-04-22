public interface IChatterData
{
	ChatterData GetCommonData();

	GameEventManager.EventType GetActivateOnEvent();

	bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component);
}
