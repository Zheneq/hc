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
					if (this.m_killEffect)
					{
						pkfxFX.KillEffect();
					}
					else
					{
						pkfxFX.TerminateEffect();
					}
				}
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
				pkfxFX.StartEffect();
			}
		}
	}

	private void OnEnable()
	{
		if (this.m_restartOnEnable)
		{
			if (this.m_hasStarted)
			{
				this.StartEffects();
			}
		}
	}
}
