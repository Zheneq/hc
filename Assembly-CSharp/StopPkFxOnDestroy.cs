using System;
using UnityEngine;

public class StopPkFxOnDestroy : MonoBehaviour
{
	public bool m_restartOnEnable;

	public bool m_killEffect;

	private bool s_quitting;

	private bool m_hasStarted;

	private void OnApplicationQuit()
	{
		this.s_quitting = true;
	}

	private void OnDisable()
	{
		if (!this.s_quitting)
		{
			PKFxFX[] componentsInChildren = base.GetComponentsInChildren<PKFxFX>(true);
			foreach (PKFxFX pkfxFX in componentsInChildren)
			{
				if (pkfxFX != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(StopPkFxOnDestroy.OnDisable()).MethodHandle;
					}
					if (this.m_killEffect)
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
						pkfxFX.KillEffect();
					}
					else
					{
						pkfxFX.TerminateEffect();
					}
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private void Start()
	{
		this.m_hasStarted = true;
	}

	private void StartEffects()
	{
		PKFxFX[] componentsInChildren = base.GetComponentsInChildren<PKFxFX>(true);
		foreach (PKFxFX pkfxFX in componentsInChildren)
		{
			if (pkfxFX != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(StopPkFxOnDestroy.StartEffects()).MethodHandle;
				}
				pkfxFX.StartEffect();
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void OnEnable()
	{
		if (this.m_restartOnEnable)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(StopPkFxOnDestroy.OnEnable()).MethodHandle;
			}
			if (this.m_hasStarted)
			{
				this.StartEffects();
			}
		}
	}
}
