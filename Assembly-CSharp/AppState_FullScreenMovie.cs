using System;
using System.Collections.Generic;
using UnityEngine;

public class AppState_FullScreenMovie : AppState
{
	private static AppState_FullScreenMovie s_instance;

	private AppState_FullScreenMovie.AppStates m_nextAppState;

	private AppState m_lastAppState;

	private Queue<string> m_movieNames;

	private PlayRawImageMovieTexture m_moviePlayer;

	public static AppState_FullScreenMovie Get()
	{
		return AppState_FullScreenMovie.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_FullScreenMovie>();
	}

	private void Awake()
	{
		AppState_FullScreenMovie.s_instance = this;
	}

	public void Enter(Queue<string> movieNames, AppState_FullScreenMovie.AppStates nextAppState)
	{
		this.m_movieNames = new Queue<string>(movieNames);
		this.m_nextAppState = nextAppState;
		this.m_lastAppState = AppState.GetCurrent();
		if (FullScreenMovie.Get())
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_FullScreenMovie.Enter(Queue<string>, AppState_FullScreenMovie.AppStates)).MethodHandle;
			}
			this.m_moviePlayer = FullScreenMovie.Get().GetMovieTexture();
			FullScreenMovie.Get().SetVisible(true);
		}
		if (this.m_moviePlayer)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_movieNames.Count > 0)
			{
				string movieAssetName = this.m_movieNames.Dequeue();
				this.m_moviePlayer.Play(movieAssetName, false, false, true);
			}
		}
		base.Enter();
	}

	protected override void OnLeave()
	{
		if (this.m_moviePlayer)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_FullScreenMovie.OnLeave()).MethodHandle;
			}
			this.m_moviePlayer = null;
		}
		if (UIScreenManager.Get() != null && FullScreenMovie.Get())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			FullScreenMovie.Get().SetVisible(false);
		}
	}

	private void Update()
	{
		if (this.m_moviePlayer)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_FullScreenMovie.Update()).MethodHandle;
			}
			if (this.m_moviePlayer.MovieState != PlayRawImageMovieTexture.MovieStates.Invalid && this.m_moviePlayer.MovieState != PlayRawImageMovieTexture.MovieStates.Done)
			{
				if (!Input.GetKeyUp(KeyCode.Escape))
				{
					goto IL_8F;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (this.m_movieNames.Count > 0)
			{
				string movieAssetName = this.m_movieNames.Dequeue();
				this.m_moviePlayer.Play(movieAssetName, false, false, true);
			}
			else
			{
				this.Finish();
			}
			IL_8F:;
		}
		else
		{
			this.Finish();
		}
	}

	private void Finish()
	{
		if (this.m_nextAppState == AppState_FullScreenMovie.AppStates.LandingPage)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_FullScreenMovie.Finish()).MethodHandle;
			}
			AppState_LandingPage.Get().Enter();
		}
		else if (this.m_nextAppState == AppState_FullScreenMovie.AppStates.PostGameLoadingScreen)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			AppState_FrontendLoadingScreen.Get().Enter(null, AppState_FrontendLoadingScreen.NextState.GoToLandingPage);
		}
		else
		{
			this.m_lastAppState.Enter();
		}
	}

	public enum AppStates
	{
		LandingPage,
		PostGameLoadingScreen,
		None
	}
}
