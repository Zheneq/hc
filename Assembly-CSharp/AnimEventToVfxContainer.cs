using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AnimEventToVfxContainer
{
	public Object m_persistentVfxStartEvent;

	public Object m_persistentVfxStopEvent;

	public List<AttachedActorVFXInfo> m_vfxInstances;

	public bool m_shouldShowPersistentVfx;

	public bool m_turnOffOnTurnStart = true;

	public AnimEventToVfxContainer(Object startEvent, Object stopEvent, List<AdditionalAttachedActorVfx.JointToVfx> vfxPrefabList, GameObject attachToObj)
	{
		m_persistentVfxStartEvent = startEvent;
		m_persistentVfxStopEvent = stopEvent;
		m_vfxInstances = new List<AttachedActorVFXInfo>();
		for (int i = 0; i < vfxPrefabList.Count; i++)
		{
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(vfxPrefabList[i].m_vfxCommonPrefab, attachToObj, vfxPrefabList[i].m_joint, vfxPrefabList[i].m_alignToRootOrientation, new StringBuilder().Append("AttachedVfx_").Append(vfxPrefabList[i].m_name).ToString(), AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
			{
				m_vfxInstances.Add(attachedActorVFXInfo);
			}
		}
	}

	public void UpdateVisibilityForSpawnedVfx(bool actorVisible, bool sameTeam)
	{
		int num;
		if (actorVisible)
		{
			num = (m_shouldShowPersistentVfx ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool actorVisible2 = (byte)num != 0;
		for (int i = 0; i < m_vfxInstances.Count; i++)
		{
			m_vfxInstances[i].UpdateVisibility(actorVisible2, sameTeam);
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void HandleAnimEvent(Object eventObj, GameObject sourceObj)
	{
		if (m_persistentVfxStartEvent != null)
		{
			if (eventObj == m_persistentVfxStartEvent)
			{
				if (!(GameFlowData.Get() != null))
				{
					return;
				}
				while (true)
				{
					if (GameFlowData.Get().gameState != GameState.BothTeams_Decision)
					{
						while (true)
						{
							m_shouldShowPersistentVfx = true;
							return;
						}
					}
					return;
				}
			}
		}
		if (!(m_persistentVfxStopEvent != null))
		{
			return;
		}
		while (true)
		{
			if (eventObj == m_persistentVfxStopEvent)
			{
				while (true)
				{
					m_shouldShowPersistentVfx = false;
					return;
				}
			}
			return;
		}
	}
}
