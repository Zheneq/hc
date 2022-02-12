using UnityEngine;

public class SparkTetherDistanceMonitor : MonoBehaviour, IGameEventListener
{
	private ProjectileWithTetherSequence m_lineFromCasterSequence;
	private LineSequence m_lineFromTargetSequence;
	private SparkBeamTrackerComponent m_syncComp;
	private ActorData m_targetActor;

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.TheatricsEvasionMoveStart
			&& m_syncComp != null
			&& m_targetActor != null
			&& m_syncComp.IsActorOutOfRangeForEvade(m_targetActor))
		{
			if (m_lineFromCasterSequence != null)
			{
				m_lineFromCasterSequence.ForceHideLine();
			}
			if (m_lineFromTargetSequence != null)
			{
				m_lineFromTargetSequence.ForceHideLine();
			}
		}
	}

	private void Start()
	{
		m_lineFromCasterSequence = GetComponent<ProjectileWithTetherSequence>();
		m_lineFromTargetSequence = GetComponent<LineSequence>();
		if (m_lineFromCasterSequence != null && m_lineFromCasterSequence.Caster != null)
		{
			m_syncComp = m_lineFromCasterSequence.Caster.GetComponent<SparkBeamTrackerComponent>();
			m_targetActor = m_lineFromCasterSequence.Target;
		}
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TheatricsEvasionMoveStart);
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TheatricsEvasionMoveStart);
	}
}
