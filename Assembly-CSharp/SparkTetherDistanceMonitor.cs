using System;
using UnityEngine;

public class SparkTetherDistanceMonitor : MonoBehaviour, IGameEventListener
{
	private ProjectileWithTetherSequence m_lineFromCasterSequence;

	private LineSequence m_lineFromTargetSequence;

	private SparkBeamTrackerComponent m_syncComp;

	private ActorData m_targetActor;

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.TheatricsEvasionMoveStart && this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkTetherDistanceMonitor.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
			if (this.m_targetActor != null)
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
				if (this.m_syncComp.IsActorOutOfRangeForEvade(this.m_targetActor))
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
					if (this.m_lineFromCasterSequence != null)
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
						this.m_lineFromCasterSequence.ForceHideLine();
					}
					if (this.m_lineFromTargetSequence != null)
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
						this.m_lineFromTargetSequence.ForceHideLine();
					}
				}
			}
		}
	}

	private void Start()
	{
		this.m_lineFromCasterSequence = base.GetComponent<ProjectileWithTetherSequence>();
		this.m_lineFromTargetSequence = base.GetComponent<LineSequence>();
		if (this.m_lineFromCasterSequence != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SparkTetherDistanceMonitor.Start()).MethodHandle;
			}
			if (this.m_lineFromCasterSequence.Caster != null)
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
				this.m_syncComp = this.m_lineFromCasterSequence.Caster.GetComponent<SparkBeamTrackerComponent>();
				this.m_targetActor = this.m_lineFromCasterSequence.Target;
			}
		}
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TheatricsEvasionMoveStart);
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TheatricsEvasionMoveStart);
	}
}
