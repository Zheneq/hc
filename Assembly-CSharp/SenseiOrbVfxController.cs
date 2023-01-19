using System.Collections.Generic;
using UnityEngine;

public class SenseiOrbVfxController : CopyableVfxControllerComponent
{
	public bool m_showOrbsInFrontEnd;
	public List<AdditionalAttachedActorVfx.JointToVfx> m_jointToVfxList;

	private ActorData m_owner;
	private ActorModelData m_actorModelData;
	private List<AttachedActorVFXInfo> m_spawnedCrystalVfxList;
	private Sensei_SyncComponent m_syncComp;

	private void Start()
	{
		Initialize();
	}

	private void Initialize()
	{
		m_actorModelData = GetComponent<ActorModelData>();
		m_spawnedCrystalVfxList = new List<AttachedActorVFXInfo>();
		if (m_actorModelData != null && m_actorModelData.m_parentActorData != null)
		{
			m_syncComp = m_actorModelData.m_parentActorData.GetComponent<Sensei_SyncComponent>();
			m_owner = m_actorModelData.m_parentActorData;
		}
		foreach (AdditionalAttachedActorVfx.JointToVfx jointToVfx in m_jointToVfxList)
		{
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(
				jointToVfx.m_vfxCommonPrefab,
				gameObject,
				jointToVfx.m_joint,
				jointToVfx.m_alignToRootOrientation,
				"MartyrCrystalVfx_" + jointToVfx.m_name,
				AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
			{
				attachedActorVFXInfo.SetInstanceLocalPosition(jointToVfx.m_localOffset);
				m_spawnedCrystalVfxList.Add(attachedActorVFXInfo);
			}
			else if (Application.isEditor)
			{
				Debug.LogWarning("Failed to spawn vfx on joint in " + GetType());
			}
		}
	}

	private void Update()
	{
		ActorData actorData = m_actorModelData != null ? m_actorModelData.m_parentActorData : null;
		bool shouldShowOrbs = actorData == null
		            || actorData.IsActorVisibleToClient()
						&& (actorData.GetActorModelData() == null || actorData.GetActorModelData().IsVisibleToClient());
		bool isDead = actorData != null && actorData.IsInRagdoll();
		bool sameTeamAsClientActor = actorData == null
		                             || GameFlowData.Get() != null
		                             && GameFlowData.Get().activeOwnedActorData != null
		                             && GameFlowData.Get().activeOwnedActorData.GetTeam() == actorData.GetTeam();
		int numOrbs = m_syncComp == null
			? m_showOrbsInFrontEnd
				? m_spawnedCrystalVfxList.Count
				: 0
			: GetNumOrbs();
		for (int i = 0; i < m_spawnedCrystalVfxList.Count; i++)
		{
			m_spawnedCrystalVfxList[i].UpdateVisibility(shouldShowOrbs && !isDead && i < numOrbs, sameTeamAsClientActor);
		}
	}

	private void OnDestroy()
	{
		if (m_spawnedCrystalVfxList == null)
		{
			return;
		}
		foreach (AttachedActorVFXInfo fx in m_spawnedCrystalVfxList)
		{
			fx.DestroyVfx();
		}
		m_spawnedCrystalVfxList.Clear();
	}

	private int GetNumOrbs()
	{
		if (m_syncComp != null)
		{
			return Mathf.Max(0, m_syncComp.m_syncCurrentNumOrbs + m_syncComp.m_clientOrbNumAdjust);
		}
		return 0;
	}

	public Vector3 GetSpawnPosAndAdvanceCounter()
	{
		if (m_syncComp != null && m_spawnedCrystalVfxList.Count > 0)
		{
			int value = GetNumOrbs() - 1;
			value = Mathf.Clamp(value, 0, m_spawnedCrystalVfxList.Count - 1);
			Vector3 instancePosition = m_spawnedCrystalVfxList[value].GetInstancePosition();
			m_syncComp.m_clientOrbNumAdjust--;
			return instancePosition;
		}
		return m_owner.GetLoSCheckPos();
	}
}
