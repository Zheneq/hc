using System.Collections.Generic;
using UnityEngine;

public class AppState_FullScreenMovie : AppState
{
	public enum AppStates
	{
		LandingPage,
		PostGameLoadingScreen,
		None
	}

	private static AppState_FullScreenMovie s_instance;

	private AppStates m_nextAppState;

	private AppState m_lastAppState;

	private Queue<string> m_movieNames;

	private PlayRawImageMovieTexture m_moviePlayer;

	public static AppState_FullScreenMovie Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_FullScreenMovie>();
	}

	private void Awake()
	{
		s_instance = this;
	}

	public void Enter(Queue<string> movieNames, AppStates nextAppState)
	{
		m_movieNames = new Queue<string>(movieNames);
		m_nextAppState = nextAppState;
		m_lastAppState = AppState.GetCurrent();
		if ((bool)FullScreenMovie.Get())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_moviePlayer = FullScreenMovie.Get().GetMovieTexture();
			FullScreenMovie.Get().SetVisible(true);
		}
		if ((bool)m_moviePlayer)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_movieNames.Count > 0)
			{
				string movieAssetName = m_movieNames.Dequeue();
				m_moviePlayer.Play(movieAssetName, false);
			}
		}
		base.Enter();
	}

	protected override void OnLeave()
	{
		if ((bool)m_moviePlayer)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_moviePlayer = null;
		}
		if (!(UIScreenManager.Get() != null) || !FullScreenMovie.Get())
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			FullScreenMovie.Get().SetVisible(false);
			return;
		}
	}

	private void Update()
	{
		if ((bool)m_moviePlayer)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (m_moviePlayer.MovieState != 0 && m_moviePlayer.MovieState != PlayRawImageMovieTexture.MovieStates.Done)
					{
						if (!Input.GetKeyUp(KeyCode.Escape))
						{
							return;
						}
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (m_movieNames.Count > 0)
					{
						string movieAssetName = m_movieNames.Dequeue();
						m_moviePlayer.Play(movieAssetName, false);
					}
					else
					{
						Finish();
					}
					return;
				}
			}
		}
		Finish();
	}

	private void Finish()
	{
		if (m_nextAppState == AppStates.LandingPage)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					AppState_LandingPage.Get().Enter();
					return;
				}
			}
		}
		if (m_nextAppState == AppStates.PostGameLoadingScreen)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					AppState_FrontendLoadingScreen.Get().Enter(null);
					return;
				}
			}
		}
		m_lastAppState.Enter();
	}
}
