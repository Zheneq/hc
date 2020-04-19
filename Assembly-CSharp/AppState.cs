using System;
using UnityEngine;

public class AppState : MonoBehaviour
{
	protected static GameObject s_appStateObject;

	protected static AppState s_currentAppState;

	protected static AppState s_previousAppState;

	protected static AppState s_nextAppState;

	protected static bool s_ready = true;

	protected float m_timeStart;

	public AppState()
	{
		this.m_timeStart = 0f;
	}

	public static AppState GetCurrent()
	{
		return AppState.s_currentAppState;
	}

	public static AppState GetPrevious()
	{
		return AppState.s_previousAppState;
	}

	public static AppState GetNext()
	{
		return AppState.s_previousAppState;
	}

	public static string GetCurrentName()
	{
		string result;
		if (AppState.GetCurrent() == null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState.GetCurrentName()).MethodHandle;
			}
			result = "NULL";
		}
		else
		{
			result = AppState.GetCurrent().GetType().Name;
		}
		return result;
	}

	public static bool IsInGame()
	{
		if (!(AppState.GetCurrent() == AppState_InGameDecision.Get()))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState.IsInGame()).MethodHandle;
			}
			if (!(AppState.GetCurrent() == AppState_InGameStarting.Get()) && !(AppState.GetCurrent() == AppState_InGameDeployment.Get()))
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
				if (!(AppState.GetCurrent() == AppState_InGameResolve.Get()))
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
					return AppState.GetCurrent() == AppState_InGameEnding.Get();
				}
			}
		}
		return true;
	}

	public virtual void Enter()
	{
		if (base.enabled)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState.Enter()).MethodHandle;
			}
			base.enabled = false;
		}
		base.enabled = true;
		this.m_timeStart = Time.time;
	}

	public void Leave()
	{
		base.enabled = false;
		this.m_timeStart = 0f;
	}

	private void OnEnable()
	{
		if (AppState.s_ready)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState.OnEnable()).MethodHandle;
			}
			if (!(AppState.s_currentAppState == this))
			{
				AppState.s_previousAppState = AppState.s_currentAppState;
				AppState.s_nextAppState = this;
				if (AppState.s_currentAppState != null)
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
					AppState.s_currentAppState.Leave();
				}
				Log.Info("Entering {0}", new object[]
				{
					base.GetType().Name
				});
				AppState.s_currentAppState = this;
				this.OnEnter();
				AppState.s_nextAppState = null;
				GameEventManager.Get().FireEvent(GameEventManager.EventType.AppStateChanged, null);
				return;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private void OnDisable()
	{
		if (AppState.s_ready)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState.OnDisable()).MethodHandle;
			}
			if (!(AppState.s_currentAppState != this))
			{
				this.OnLeave();
				AppState.s_currentAppState = null;
				return;
			}
		}
	}

	private void OnApplicationQuit()
	{
		AppState.s_ready = false;
		AppState.s_currentAppState = null;
	}

	public float Elapsed
	{
		get
		{
			return (this.m_timeStart != 0f) ? (Time.time - this.m_timeStart) : 0f;
		}
	}

	protected virtual void OnEnter()
	{
	}

	protected virtual void OnLeave()
	{
	}

	protected static AppStateType Create<AppStateType>() where AppStateType : AppState, new()
	{
		if (!AppState.s_ready)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState.Create()).MethodHandle;
			}
			Log.Error("AppState.Create not ready! code error", new object[0]);
			return (AppStateType)((object)null);
		}
		if (AppState.s_appStateObject == null)
		{
			AppState.s_appStateObject = new GameObject("AppStates");
			UnityEngine.Object.DontDestroyOnLoad(AppState.s_appStateObject);
		}
		AppState.s_ready = false;
		AppStateType result = AppState.s_appStateObject.AddComponent<AppStateType>();
		result.enabled = false;
		AppState.s_ready = true;
		return result;
	}
}
