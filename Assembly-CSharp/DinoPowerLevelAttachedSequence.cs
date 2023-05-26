using System.Collections.Generic;
using UnityEngine;

public class DinoPowerLevelAttachedSequence : SimpleAttachedVFXSequence
{
	[Separator("FX Prefabs for each power level, from low to high")]
	public List<GameObject> m_fxPrefabForPowerLevels = new List<GameObject>();

	private Dino_SyncComponent m_syncComp;

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (Caster != null)
		{
			m_syncComp = Caster.GetComponent<Dino_SyncComponent>();
		}
	}

	protected override GameObject GetFxPrefab()
	{
		if (m_syncComp == null || m_fxPrefabForPowerLevels.Count <= 0)
		{
			return base.GetFxPrefab();
		}
		int layerConePowerLevel = Mathf.Min(m_syncComp.m_layerConePowerLevel, m_fxPrefabForPowerLevels.Count - 1);
		return m_fxPrefabForPowerLevels[layerConePowerLevel];
	}
}
