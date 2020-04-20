using System;
using UnityEngine;

public class TriggerCoordinator : MonoBehaviour
{
	private static TriggerCoordinator s_instance;

	public TriggerRegion[] m_regions;

	public static TriggerCoordinator Get()
	{
		return TriggerCoordinator.s_instance;
	}

	private void Awake()
	{
		TriggerCoordinator.s_instance = this;
	}

	private void OnDestroy()
	{
		TriggerCoordinator.s_instance = null;
	}

	private void Start()
	{
		foreach (TriggerRegion triggerRegion in this.m_regions)
		{
			triggerRegion.Initialize();
		}
	}

	public void OnTurnTick()
	{
		foreach (TriggerRegion triggerRegion in this.m_regions)
		{
			triggerRegion.OnTurnTick();
		}
	}
}
