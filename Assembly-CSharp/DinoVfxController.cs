using System.Collections.Generic;
using UnityEngine;

public class DinoVfxController : CopyableVfxControllerComponent
{
	[Separator("Vfx prefabs for power levels, from low to high", true)]
	public List<GameObject> m_indicatorVfxPrefabs;
	[Separator("Vfx prefab for front end character select, etc", true)]
	public GameObject m_frontEndCharSelectVfxPrefab;
	[JointPopup("Fx Attach Joint")]
	public JointPopupProperty m_fxJoint;

	private ActorData m_owner;
	private ActorModelData m_actorModelData;
	private List<AttachedActorVFXInfo> m_spawnedVfxList;
	private Dino_SyncComponent m_syncComp;

	private void Start()
	{
		Initialize();
	}

	private void Initialize()
	{
		m_actorModelData = GetComponent<ActorModelData>();
		m_spawnedVfxList = new List<AttachedActorVFXInfo>();
		if (m_actorModelData != null && m_actorModelData.m_parentActorData != null)
		{
			m_syncComp = m_actorModelData.m_parentActorData.GetComponent<Dino_SyncComponent>();
			m_owner = m_actorModelData.m_parentActorData;
		}
		bool noSyncComp = m_syncComp == null;
		foreach (GameObject prefab in m_indicatorVfxPrefabs)
		{
			GameObject vfxPrefab = m_frontEndCharSelectVfxPrefab != null && noSyncComp
				? m_frontEndCharSelectVfxPrefab
				: prefab;
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(
				vfxPrefab,
				base.gameObject,
				m_fxJoint,
				false,
				"DinoPowerVfx",
				AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
			{
				m_spawnedVfxList.Add(attachedActorVFXInfo);
			}
			else if (Application.isEditor)
			{
				Debug.LogWarning("Failed to spawn vfx on joint in " + GetType());
			}
		}
	}

	private void LateUpdate()
	{
		bool ownerIsVisible = m_owner == null
		            || m_owner.IsActorVisibleToClient()
		            && (m_owner.GetActorModelData() == null
		                || m_owner.GetActorModelData().IsVisibleToClient());
		bool ownerIsDead = m_owner != null && m_owner.IsInRagdoll();
		int powerLevel = 0;
		if (m_syncComp != null)
		{
			powerLevel = m_syncComp.m_layerConePowerLevel;
			powerLevel = Mathf.Min(powerLevel, m_spawnedVfxList.Count - 1);
		}
		else if (m_spawnedVfxList.Count > 0)
		{
			powerLevel = m_spawnedVfxList.Count - 1;
		}
		for (int i = 0; i < m_spawnedVfxList.Count; i++)
		{
			bool actorVisible = false;
			if (powerLevel == i)
			{
				actorVisible = ownerIsVisible && !ownerIsDead;
			}
			m_spawnedVfxList[i].UpdateVisibility(actorVisible, true);
		}
	}

	private void OnDestroy()
	{
		if (m_spawnedVfxList == null)
		{
			return;
		}
		foreach (AttachedActorVFXInfo fx in m_spawnedVfxList)
		{
			fx.DestroyVfx();
		}
		m_spawnedVfxList.Clear();
	}
}
