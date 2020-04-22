using UnityEngine;

public class NinjaDeathMarkSequence : TempSatelliteSequence
{
	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public Object m_startEvent;

	private bool m_spawnedTempSatellite;

	private bool m_setFinishTrigger;

	public override void FinishSetup()
	{
		if (!(m_startEvent == null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SpawnTempSatellite();
			return;
		}
	}

	private void SpawnTempSatellite()
	{
		m_spawnedTempSatellite = true;
		m_tempSatelliteInstance = InstantiateFX(m_tempSatellitePrefab, base.TargetPos, Quaternion.identity);
		m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().Setup(this);
		m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().TriggerDeathMarkAttack();
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(m_startEvent == parameter))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SpawnTempSatellite();
			return;
		}
	}

	private void Update()
	{
		if (!m_spawnedTempSatellite)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_setFinishTrigger)
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
				if (!(m_tempSatelliteInstance == null))
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
					if (!m_tempSatelliteInstance.GetComponent<NinjaCloneSatellite>().IsDespawning())
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
						break;
					}
				}
				m_setFinishTrigger = true;
				base.Caster.GetActorModelData().GetModelAnimator().SetTrigger("FinishAttack");
				return;
			}
		}
	}

	private void OnDisable()
	{
		m_initialized = false;
	}
}
