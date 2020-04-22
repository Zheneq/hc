using UnityEngine;

public class BazookaGirlDelayedMissileEffectSequence : Sequence
{
	public GameObject m_fxPrefab;

	private const float FLOOR_OFFSET = 0.12f;

	private GameObject m_fx;

	[AudioEvent(false)]
	public string m_audioEventApply;

	[AudioEvent(false)]
	public string m_audioEventFire = "ablty/bazookagirl/above_fire";

	protected override void OnStopVfxOnClient()
	{
		if (!(m_fx != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_fx.SetActive(false);
			return;
		}
	}

	private void Update()
	{
		if (!m_fxPrefab)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_initialized || !(m_fx == null))
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
				if (!(base.Caster != null))
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
					m_fx = InstantiateFX(m_fxPrefab);
					if (m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
					{
						m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
					}
					m_fx.transform.position = base.TargetPos + Vector3.up * 0.12f;
					m_fx.transform.localRotation = Quaternion.identity;
					return;
				}
			}
		}
	}

	private void OnDisable()
	{
		if (!m_fx)
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
			Object.Destroy(m_fx);
			return;
		}
	}
}
