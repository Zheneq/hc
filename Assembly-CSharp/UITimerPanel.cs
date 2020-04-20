using System;
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
		return UITimerPanel.s_instance;
	}

	public void Awake()
	{
		UITimerPanel.s_instance = this;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GameCameraCreated)
		{
			this.m_matchStartTime = Time.realtimeSinceStartup;
		}
	}

	public void SetMatchTime(float timeSinceMatchStart)
	{
		this.m_matchStartTime = Time.realtimeSinceStartup - timeSinceMatchStart;
	}

	public void SetMatchTurn(int matchTurn)
	{
		this.m_matchStartTurn = this.m_turn - matchTurn;
		this.m_turnLabel.text = string.Format("-{0}-", this.GetTurn());
	}

	public int GetMinutes()
	{
		return this.m_minutes;
	}

	public int GetSeconds()
	{
		return this.m_seconds;
	}

	public int GetTurn()
	{
		return this.m_turn - this.m_matchStartTurn;
	}

	private void Start()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameCameraCreated);
		this.m_matchStartTime = Time.realtimeSinceStartup;
		this.m_matchStartTurn = 0;
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameCameraCreated);
			UITimerPanel.s_instance = null;
		}
	}

	private void Update()
	{
		if (GameFlowData.Get() == null)
		{
			return;
		}
		if (this.m_turn != GameFlowData.Get().CurrentTurn)
		{
			this.m_turn = GameFlowData.Get().CurrentTurn;
			this.m_turnLabel.text = string.Format("-{0}-", this.GetTurn());
		}
		float num = 0f;
		if (!AppState.GetCurrent() != AppState_InGameDeployment.Get())
		{
			num = Time.realtimeSinceStartup - this.m_matchStartTime;
		}
		int num2 = (int)(num / 60f);
		int num3 = (int)num % 0x3C;
		if (num2 == this.m_minutes)
		{
			if (num3 == this.m_seconds)
			{
				return;
			}
		}
		if (num3 < 0xA)
		{
			this.m_timeLabel.text = string.Format("{0}:0{1}", num2, num3);
		}
		else
		{
			this.m_timeLabel.text = string.Format("{0}:{1}", num2, num3);
		}
		this.m_minutes = num2;
		this.m_seconds = num3;
	}

	private void OnEnable()
	{
		this.Update();
	}
}
