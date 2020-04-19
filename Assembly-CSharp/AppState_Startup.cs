using System;

public class AppState_Startup : AppState
{
	private static AppState_Startup s_instance;

	public static AppState_Startup Get()
	{
		return AppState_Startup.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_Startup>();
	}

	private void Awake()
	{
		AppState_Startup.s_instance = this;
	}

	protected override void OnEnter()
	{
		AppState_FrontendLoadingScreen.Get().Enter(null, AppState_FrontendLoadingScreen.NextState.GoToLandingPage);
	}

	protected override void OnLeave()
	{
	}

	private void HandleGameLaunched(GameType gameType)
	{
	}

	private void Update()
	{
	}
}
