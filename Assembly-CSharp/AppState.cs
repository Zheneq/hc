using UnityEngine;

public class AppState : MonoBehaviour
{
	protected static GameObject s_appStateObject;

	protected static AppState s_currentAppState;

	protected static AppState s_previousAppState;

	protected static AppState s_nextAppState;

	protected static bool s_ready;

	protected float m_timeStart;

	public float Elapsed => (m_timeStart != 0f) ? (Time.time - m_timeStart) : 0f;

	static AppState()
	{
		s_ready = true;
	}

	public AppState()
	{
		m_timeStart = 0f;
	}

	public static AppState GetCurrent()
	{
		return s_currentAppState;
	}

	public static AppState GetPrevious()
	{
		return s_previousAppState;
	}

	public static AppState GetNext()
	{
		return s_previousAppState;
	}

	public static string GetCurrentName()
	{
		object result;
		if (GetCurrent() == null)
		{
			while (true)
			{
				switch (4)
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
			result = "NULL";
		}
		else
		{
			result = GetCurrent().GetType().Name;
		}
		return (string)result;
	}

	public static bool IsInGame()
	{
		int result;
		if (!(GetCurrent() == AppState_InGameDecision.Get()))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(GetCurrent() == AppState_InGameStarting.Get()) && !(GetCurrent() == AppState_InGameDeployment.Get()))
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
				if (!(GetCurrent() == AppState_InGameResolve.Get()))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					result = ((GetCurrent() == AppState_InGameEnding.Get()) ? 1 : 0);
					goto IL_0096;
				}
			}
		}
		result = 1;
		goto IL_0096;
		IL_0096:
		return (byte)result != 0;
	}

	public virtual void Enter()
	{
		if (base.enabled)
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
			base.enabled = false;
		}
		base.enabled = true;
		m_timeStart = Time.time;
	}

	public void Leave()
	{
		base.enabled = false;
		m_timeStart = 0f;
	}

	private void OnEnable()
	{
		if (!s_ready)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (s_currentAppState == this)
			{
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			s_previousAppState = s_currentAppState;
			s_nextAppState = this;
			if (s_currentAppState != null)
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
				s_currentAppState.Leave();
			}
			Log.Info("Entering {0}", GetType().Name);
			s_currentAppState = this;
			OnEnter();
			s_nextAppState = null;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.AppStateChanged, null);
			return;
		}
	}

	private void OnDisable()
	{
		if (!s_ready)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(s_currentAppState != this))
			{
				OnLeave();
				s_currentAppState = null;
			}
			return;
		}
	}

	private void OnApplicationQuit()
	{
		s_ready = false;
		s_currentAppState = null;
	}

	protected virtual void OnEnter()
	{
	}

	protected virtual void OnLeave()
	{
	}

	protected static AppStateType Create<AppStateType>() where AppStateType : AppState, new()
	{
		if (!s_ready)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Log.Error("AppState.Create not ready! code error");
					return (AppStateType)null;
				}
			}
		}
		if (s_appStateObject == null)
		{
			s_appStateObject = new GameObject("AppStates");
			Object.DontDestroyOnLoad(s_appStateObject);
		}
		s_ready = false;
		AppStateType result = s_appStateObject.AddComponent<AppStateType>();
		result.enabled = false;
		s_ready = true;
		return result;
	}
}
