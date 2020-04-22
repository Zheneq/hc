public class FullScreenMovie : UIScene
{
	public PlayRawImageMovieTexture m_movieTexture;

	private static FullScreenMovie s_instance;

	public static FullScreenMovie Get()
	{
		return s_instance;
	}

	public override void Awake()
	{
		s_instance = this;
		SetVisible(false);
	}

	public void Start()
	{
		if (!(UIManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			UIManager.Get().RegisterUIScene(this);
			return;
		}
	}

	public override SceneType GetSceneType()
	{
		return SceneType.FullScreenMovie;
	}

	public PlayRawImageMovieTexture GetMovieTexture()
	{
		return m_movieTexture;
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_movieTexture, visible);
	}
}
