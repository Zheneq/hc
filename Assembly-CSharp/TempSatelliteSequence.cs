using System;
using UnityEngine;

public abstract class TempSatelliteSequence : Sequence
{
	public GameObject m_tempSatellitePrefab;

	protected GameObject m_tempSatelliteInstance;

	public GameObject GetTempSatellite()
	{
		return this.m_tempSatelliteInstance;
	}
}
