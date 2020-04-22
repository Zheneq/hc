using UnityEngine;

public class EnvironmentalPkFx : MonoBehaviour, IGameEventListener
{
	[Separator("For Randomized emit intervals, used if both min and max are positive", true)]
	public float m_randEmitIntervalMin;

	public float m_randEmitIntervalMax;

	private PKFxFX m_fx;

	private float m_timeTillNextEmit = -1f;

	private void Awake()
	{
		if (!(m_randEmitIntervalMax < m_randEmitIntervalMin))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_randEmitIntervalMax = m_randEmitIntervalMin;
			return;
		}
	}

	private void OnEnable()
	{
		if (GameFlowData.Get() == null)
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
			if (m_fx == null)
			{
				m_fx = GetComponent<PKFxFX>();
			}
		}
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameCameraCreated);
	}

	private void OnDisable()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameCameraCreated);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.GameCameraCreated)
		{
			return;
		}
		if (!(VisualsLoader.Get() == null))
		{
			while (true)
			{
				switch (7)
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
			if (!VisualsLoader.Get().LevelLoaded())
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (m_fx == null)
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
			m_fx = GetComponent<PKFxFX>();
		}
		if (!(m_fx != null) || !m_fx.enabled)
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
			if (m_randEmitIntervalMin <= 0f)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						m_fx.StartEffect();
						return;
					}
				}
			}
			m_timeTillNextEmit = Random.Range(m_randEmitIntervalMin, m_randEmitIntervalMax);
			return;
		}
	}

	public void Update()
	{
		if (!(m_randEmitIntervalMin > 0f))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(m_randEmitIntervalMax > 0f))
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (!(m_fx != null))
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
					if (!m_fx.enabled)
					{
						return;
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						if (m_timeTillNextEmit < 0f)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									m_timeTillNextEmit = Random.Range(m_randEmitIntervalMin, m_randEmitIntervalMax);
									return;
								}
							}
						}
						if (!(m_timeTillNextEmit > 0f))
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
							m_timeTillNextEmit -= Time.deltaTime;
							if (m_timeTillNextEmit <= 0f)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									m_fx.TerminateEffect();
									m_fx.StartEffect();
									m_timeTillNextEmit = Random.Range(m_randEmitIntervalMin, m_randEmitIntervalMax);
									return;
								}
							}
							return;
						}
					}
				}
			}
		}
	}
}
