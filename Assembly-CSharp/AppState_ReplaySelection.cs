public class AppState_ReplaySelection : AppState
{
	private static AppState_ReplaySelection s_instance;

	public static AppState_ReplaySelection Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_ReplaySelection>();
	}

	private void Awake()
	{
		s_instance = this;
	}

	protected override void OnEnter()
	{
	}

	protected override void OnLeave()
	{
	}

	public void OnCancelClicked()
	{
	}

	public void OnStartClicked(string replayFile)
	{
	}
}
