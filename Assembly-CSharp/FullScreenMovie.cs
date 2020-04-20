using System;

public class FullScreenMovie : UIScene
{
	public PlayRawImageMovieTexture m_movieTexture;

	private static FullScreenMovie s_instance;

	public static FullScreenMovie Get()
	{
		return FullScreenMovie.s_instance;
	}

	public override void Awake()
	{
		FullScreenMovie.s_instance = this;
		this.SetVisible(false);
	}

	public void Start()
	{
		if (UIManager.Get() != null)
		{
			UIManager.Get().RegisterUIScene(this);
		}
	}

	public override SceneType GetSceneType()
	{
		return SceneType.FullScreenMovie;
	}

	public PlayRawImageMovieTexture GetMovieTexture()
	{
		return this.m_movieTexture;
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_movieTexture, visible, null);
	}
}
