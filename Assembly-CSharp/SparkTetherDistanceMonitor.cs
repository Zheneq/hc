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
			if (this.m_targetActor != null)
			{
				if (this.m_syncComp.IsActorOutOfRangeForEvade(this.m_targetActor))
				{
					if (this.m_lineFromCasterSequence != null)
					{
						this.m_lineFromCasterSequence.ForceHideLine();
					}
					if (this.m_lineFromTargetSequence != null)
					{
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
			if (this.m_lineFromCasterSequence.Caster != null)
			{
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
