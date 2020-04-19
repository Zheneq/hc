using System;
using UnityEngine;

public class AppState_Shutdown : AppState
{
	private static AppState_Shutdown s_instance;

	public static AppState_Shutdown Get()
	{
		return AppState_Shutdown.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_Shutdown>();
	}

	private void Awake()
	{
		AppState_Shutdown.s_instance = this;
	}

	protected override void OnEnter()
	{
		Log.Info("Shutting down", new object[0]);
		Application.Quit();
	}

	protected override void OnLeave()
	{
	}
}
