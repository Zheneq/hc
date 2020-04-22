using TMPro;
using UnityEngine;

public class UITimerPanel : MonoBehaviour, IGameEventListener
{
	public TextMeshProUGUI m_timeLabel;

	public TextMeshProUGUI m_turnLabel;

	public static UITimerPanel s_instance;

	private int m_turn;

	private int m_minutes;

	private int m_seconds;

	private float m_matchStartTime;

	private int m_matchStartTurn;

	public static UITimerPanel Get()
	{
		return s_instance;
	}

	public void Awake()
	{
		s_instance = this;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.GameCameraCreated)
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
			m_matchStartTime = Time.realtimeSinceStartup;
			return;
		}
	}

	public void SetMatchTime(float timeSinceMatchStart)
	{
		m_matchStartTime = Time.realtimeSinceStartup - timeSinceMatchStart;
	}

	public void SetMatchTurn(int matchTurn)
	{
		m_matchStartTurn = m_turn - matchTurn;
		m_turnLabel.text = $"-{GetTurn()}-";
	}

	public int GetMinutes()
	{
		return m_minutes;
	}

	public int GetSeconds()
	{
		return m_seconds;
	}

	public int GetTurn()
	{
		return m_turn - m_matchStartTurn;
	}

	private void Start()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameCameraCreated);
		m_matchStartTime = Time.realtimeSinceStartup;
		m_matchStartTurn = 0;
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameCameraCreated);
			s_instance = null;
		}
	}

	private void Update()
	{
		if (GameFlowData.Get() == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (m_turn != GameFlowData.Get().CurrentTurn)
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
			m_turn = GameFlowData.Get().CurrentTurn;
			m_turnLabel.text = $"-{GetTurn()}-";
		}
		float num = 0f;
		if (!AppState.GetCurrent() != (bool)AppState_InGameDeployment.Get())
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
			num = Time.realtimeSinceStartup - m_matchStartTime;
		}
		int num2 = (int)(num / 60f);
		int num3 = (int)num % 60;
		if (num2 == m_minutes)
		{
			if (num3 == m_seconds)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (num3 < 10)
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
			m_timeLabel.text = $"{num2}:0{num3}";
		}
		else
		{
			m_timeLabel.text = $"{num2}:{num3}";
		}
		m_minutes = num2;
		m_seconds = num3;
	}

	private void OnEnable()
	{
		Update();
	}
}
