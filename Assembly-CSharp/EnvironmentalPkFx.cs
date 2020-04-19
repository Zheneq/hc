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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(EnvironmentalPkFx.Awake()).MethodHandle;
			}
			this.m_randEmitIntervalMax = this.m_randEmitIntervalMin;
		}
	}

	private void OnEnable()
	{
		if (GameFlowData.Get() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(EnvironmentalPkFx.OnEnable()).MethodHandle;
			}
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(EnvironmentalPkFx.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
				}
				if (!VisualsLoader.Get().LevelLoaded())
				{
					return;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (this.m_fx == null)
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
				this.m_fx = base.GetComponent<PKFxFX>();
			}
			if (this.m_fx != null && this.m_fx.enabled)
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
				if (this.m_randEmitIntervalMin <= 0f)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(EnvironmentalPkFx.Update()).MethodHandle;
			}
			if (this.m_randEmitIntervalMax > 0f)
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
				if (this.m_fx != null)
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
					if (this.m_fx.enabled)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.m_timeTillNextEmit < 0f)
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
							this.m_timeTillNextEmit = UnityEngine.Random.Range(this.m_randEmitIntervalMin, this.m_randEmitIntervalMax);
						}
						else if (this.m_timeTillNextEmit > 0f)
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
							this.m_timeTillNextEmit -= Time.deltaTime;
							if (this.m_timeTillNextEmit <= 0f)
							{
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
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
