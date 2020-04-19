using System;
using UnityEngine;

public class Passive_Archer : Passive
{
	[HideInInspector]
	public int m_turnShieldGenEffectExpires = -1;

	private Archer_SyncComponent m_syncComp;

	private ArcherDashAndShoot m_dashAbility;

	private AbilityData.ActionType m_dashAbilityAction;
}
