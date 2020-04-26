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
			if (!m_initialized)
			{
				return;
			}
			while (true)
			{
				if (!m_created)
				{
					while (true)
					{
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
			Object.Destroy(m_powerUpVFX.gameObject);
			return;
		}
	}
}
