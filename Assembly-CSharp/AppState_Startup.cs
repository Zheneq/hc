public class AppState_Startup : AppState
{
	private static AppState_Startup s_instance;

	public static AppState_Startup Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_Startup>();
	}

	private void Awake()
	{
		s_instance = this;
	}

	protected override void OnEnter()
	{
		AppState_FrontendLoadingScreen loadingScreen = AppState_FrontendLoadingScreen.Get();
#if SERVER
		if (loadingScreen != null) {  // TODO LOW NULL custom check bc it's null on the server. Remove it altogether?
			loadingScreen.Enter(null);
		}
#else
		loadingScreen.Enter(null);
#endif
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
