using UnityEngine;

public class TriggerCoordinator : MonoBehaviour
{
	private static TriggerCoordinator s_instance;

	public TriggerRegion[] m_regions;

	public static TriggerCoordinator Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Start()
	{
		TriggerRegion[] regions = m_regions;
		foreach (TriggerRegion triggerRegion in regions)
		{
			triggerRegion.Initialize();
		}
		while (true)
		{
			return;
		}
	}

	public void OnTurnTick()
	{
		TriggerRegion[] regions = m_regions;
		foreach (TriggerRegion triggerRegion in regions)
		{
			triggerRegion.OnTurnTick();
		}
		while (true)
		{
			return;
		}
	}
}
