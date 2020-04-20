using System;
using UnityEngine;

public class PowerUpSequence : Sequence
{
	public Transform m_powerUpPrefab;

	private Transform m_powerUpVFX;

	private bool m_created;

	private void Update()
	{
		if (this.m_powerUpPrefab)
		{
			if (this.m_initialized)
			{
				if (!this.m_created)
				{
					this.m_created = true;
					this.m_powerUpVFX = UnityEngine.Object.Instantiate<Transform>(this.m_powerUpPrefab, base.TargetSquare.ToVector3(), Quaternion.identity);
				}
			}
		}
	}

	private void OnDisable()
	{
		if (this.m_powerUpVFX)
		{
			UnityEngine.Object.Destroy(this.m_powerUpVFX.gameObject);
		}
	}
}
