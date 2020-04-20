using System;
using UnityEngine;

public class NinjaDeathMarkSequence : TempSatelliteSequence
{
	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_startEvent;

	private bool m_spawnedTempSatellite;

	private bool m_setFinishTrigger;

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDeathMarkSequence.FinishSetup()).MethodHandle;
			}
			this.SpawnTempSatellite();
		}
	}

	private void SpawnTempSatellite()
	{
		this.m_spawnedTempSatellite = true;
		this.m_tempSatelliteInstance = base.InstantiateFX(this.m_tempSatellitePrefab, base.TargetPos, Quaternion.identity, true, true);
		this.m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().Setup(this);
		this.m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().TriggerDeathMarkAttack();
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDeathMarkSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.SpawnTempSatellite();
		}
	}

	private void Update()
	{
		if (this.m_spawnedTempSatellite)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDeathMarkSequence.Update()).MethodHandle;
			}
			if (!this.m_setFinishTrigger)
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
				if (!(this.m_tempSatelliteInstance == null))
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
					if (!this.m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().IsDespawning())
					{
						return;
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				this.m_setFinishTrigger = true;
				base.Caster.GetActorModelData().GetModelAnimator().SetTrigger("FinishAttack");
			}
		}
	}

	private void OnDisable()
	{
		this.m_initialized = false;
	}
}
