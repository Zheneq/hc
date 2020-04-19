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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSequence.Update()).MethodHandle;
			}
			if (this.m_initialized)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.m_created)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSequence.OnDisable()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_powerUpVFX.gameObject);
		}
	}
}
