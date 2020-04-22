using UnityEngine;

public class SparkTetherDistanceMonitor : MonoBehaviour, IGameEventListener
{
	private ProjectileWithTetherSequence m_lineFromCasterSequence;

	private LineSequence m_lineFromTargetSequence;

	private SparkBeamTrackerComponent m_syncComp;

	private ActorData m_targetActor;

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.TheatricsEvasionMoveStart || !(m_syncComp != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(m_targetActor != null))
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
				if (!m_syncComp.IsActorOutOfRangeForEvade(m_targetActor))
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
					if (m_lineFromCasterSequence != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						m_lineFromCasterSequence.ForceHideLine();
					}
					if (m_lineFromTargetSequence != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							m_lineFromTargetSequence.ForceHideLine();
							return;
						}
					}
					return;
				}
			}
		}
	}

	private void Start()
	{
		m_lineFromCasterSequence = GetComponent<ProjectileWithTetherSequence>();
		m_lineFromTargetSequence = GetComponent<LineSequence>();
		if (m_lineFromCasterSequence != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_lineFromCasterSequence.Caster != null)
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
				m_syncComp = m_lineFromCasterSequence.Caster.GetComponent<SparkBeamTrackerComponent>();
				m_targetActor = m_lineFromCasterSequence.Target;
			}
		}
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TheatricsEvasionMoveStart);
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TheatricsEvasionMoveStart);
	}
}
