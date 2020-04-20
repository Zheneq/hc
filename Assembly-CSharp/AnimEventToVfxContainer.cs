using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventToVfxContainer
{
	public UnityEngine.Object m_persistentVfxStartEvent;

	public UnityEngine.Object m_persistentVfxStopEvent;

	public List<AttachedActorVFXInfo> m_vfxInstances;

	public bool m_shouldShowPersistentVfx;

	public bool m_turnOffOnTurnStart = true;

	public AnimEventToVfxContainer(UnityEngine.Object startEvent, UnityEngine.Object stopEvent, List<AdditionalAttachedActorVfx.JointToVfx> vfxPrefabList, GameObject attachToObj)
	{
		this.m_persistentVfxStartEvent = startEvent;
		this.m_persistentVfxStopEvent = stopEvent;
		this.m_vfxInstances = new List<AttachedActorVFXInfo>();
		for (int i = 0; i < vfxPrefabList.Count; i++)
		{
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(vfxPrefabList[i].m_vfxCommonPrefab, attachToObj, vfxPrefabList[i].m_joint, vfxPrefabList[i].m_alignToRootOrientation, "AttachedVfx_" + vfxPrefabList[i].m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
			{
				this.m_vfxInstances.Add(attachedActorVFXInfo);
			}
		}
	}

	public void UpdateVisibilityForSpawnedVfx(bool actorVisible, bool sameTeam)
	{
		bool flag;
		if (actorVisible)
		{
			flag = this.m_shouldShowPersistentVfx;
		}
		else
		{
			flag = false;
		}
		bool actorVisible2 = flag;
		for (int i = 0; i < this.m_vfxInstances.Count; i++)
		{
			this.m_vfxInstances[i].UpdateVisibility(actorVisible2, sameTeam);
		}
	}

	public void HandleAnimEvent(UnityEngine.Object eventObj, GameObject sourceObj)
	{
		if (this.m_persistentVfxStartEvent != null)
		{
			if (eventObj == this.m_persistentVfxStartEvent)
			{
				if (GameFlowData.Get() != null)
				{
					if (GameFlowData.Get().gameState != GameState.BothTeams_Decision)
					{
						this.m_shouldShowPersistentVfx = true;
					}
				}
				return;
			}
		}
		if (this.m_persistentVfxStopEvent != null)
		{
			if (eventObj == this.m_persistentVfxStopEvent)
			{
				this.m_shouldShowPersistentVfx = false;
			}
		}
	}
}
