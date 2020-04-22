using UnityEngine;

public class PowerUpSequence : Sequence
{
	public Transform m_powerUpPrefab;

	private Transform m_powerUpVFX;

	private bool m_created;

	private void Update()
	{
		if (!m_powerUpPrefab)
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
			if (!m_initialized)
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
				if (!m_created)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						m_created = true;
						m_powerUpVFX = Object.Instantiate(m_powerUpPrefab, base.TargetSquare.ToVector3(), Quaternion.identity);
						return;
					}
				}
				return;
			}
		}
	}

	private void OnDisable()
	{
		if (!m_powerUpVFX)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Object.Destroy(m_powerUpVFX.gameObject);
			return;
		}
	}
}
