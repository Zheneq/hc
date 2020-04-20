using System;
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
		if (this.m_randEmitIntervalMax < this.m_randEmitIntervalMin)
		{
			this.m_randEmitIntervalMax = this.m_randEmitIntervalMin;
		}
	}

	private void OnEnable()
	{
		if (GameFlowData.Get() == null)
		{
			if (this.m_fx == null)
			{
				this.m_fx = base.GetComponent<PKFxFX>();
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
		if (eventType == GameEventManager.EventType.GameCameraCreated)
		{
			if (!(VisualsLoader.Get() == null))
			{
				if (!VisualsLoader.Get().LevelLoaded())
				{
					return;
				}
			}
			if (this.m_fx == null)
			{
				this.m_fx = base.GetComponent<PKFxFX>();
			}
			if (this.m_fx != null && this.m_fx.enabled)
			{
				if (this.m_randEmitIntervalMin <= 0f)
				{
					this.m_fx.StartEffect();
				}
				else
				{
					this.m_timeTillNextEmit = UnityEngine.Random.Range(this.m_randEmitIntervalMin, this.m_randEmitIntervalMax);
				}
			}
		}
	}

	public void Update()
	{
		if (this.m_randEmitIntervalMin > 0f)
		{
			if (this.m_randEmitIntervalMax > 0f)
			{
				if (this.m_fx != null)
				{
					if (this.m_fx.enabled)
					{
						if (this.m_timeTillNextEmit < 0f)
						{
							this.m_timeTillNextEmit = UnityEngine.Random.Range(this.m_randEmitIntervalMin, this.m_randEmitIntervalMax);
						}
						else if (this.m_timeTillNextEmit > 0f)
						{
							this.m_timeTillNextEmit -= Time.deltaTime;
							if (this.m_timeTillNextEmit <= 0f)
							{
								this.m_fx.TerminateEffect();
								this.m_fx.StartEffect();
								this.m_timeTillNextEmit = UnityEngine.Random.Range(this.m_randEmitIntervalMin, this.m_randEmitIntervalMax);
							}
						}
					}
				}
			}
		}
	}
}
