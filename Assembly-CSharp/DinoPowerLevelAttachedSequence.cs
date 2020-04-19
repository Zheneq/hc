using System;
using System.Collections.Generic;
using UnityEngine;

public class DinoPowerLevelAttachedSequence : SimpleAttachedVFXSequence
{
	[Separator("FX Prefabs for each power level, from low to high", true)]
	public List<GameObject> m_fxPrefabForPowerLevels = new List<GameObject>();

	private Dino_SyncComponent m_syncComp;

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (base.Caster != null)
		{
			this.m_syncComp = base.Caster.GetComponent<Dino_SyncComponent>();
		}
	}

	protected override GameObject GetFxPrefab()
	{
		GameObject result = base.GetFxPrefab();
		if (this.m_syncComp != null && this.m_fxPrefabForPowerLevels.Count > 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoPowerLevelAttachedSequence.GetFxPrefab()).MethodHandle;
			}
			int num = (int)this.m_syncComp.m_layerConePowerLevel;
			num = (int)this.m_syncComp.m_layerConePowerLevel;
			num = Mathf.Min(num, this.m_fxPrefabForPowerLevels.Count - 1);
			result = this.m_fxPrefabForPowerLevels[num];
		}
		return result;
	}
}
