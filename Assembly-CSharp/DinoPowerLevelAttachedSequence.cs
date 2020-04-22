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
			m_syncComp = base.Caster.GetComponent<Dino_SyncComponent>();
		}
	}

	protected override GameObject GetFxPrefab()
	{
		GameObject result = base.GetFxPrefab();
		if (m_syncComp != null && m_fxPrefabForPowerLevels.Count > 0)
		{
			int layerConePowerLevel = m_syncComp.m_layerConePowerLevel;
			layerConePowerLevel = m_syncComp.m_layerConePowerLevel;
			layerConePowerLevel = Mathf.Min(layerConePowerLevel, m_fxPrefabForPowerLevels.Count - 1);
			result = m_fxPrefabForPowerLevels[layerConePowerLevel];
		}
		return result;
	}
}
