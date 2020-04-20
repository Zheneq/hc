using System;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalAttachedActorVfx : MonoBehaviour
{
	public AdditionalAttachedActorVfx.JointToVfx[] m_vfxToAttach;

	private ActorData m_actorData;

	private List<AttachedActorVFXInfo> m_spawnedVfxList;

	private void Start()
	{
		this.m_actorData = base.GetComponent<ActorData>();
		this.m_spawnedVfxList = new List<AttachedActorVFXInfo>();
		if (this.m_actorData != null && this.m_vfxToAttach != null)
		{
			for (int i = 0; i < this.m_vfxToAttach.Length; i++)
			{
				AdditionalAttachedActorVfx.JointToVfx jointToVfx = this.m_vfxToAttach[i];
				if (jointToVfx.m_vfxCommonPrefab != null)
				{
					AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(jointToVfx.m_vfxCommonPrefab, this.m_actorData, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "AttachedVfx_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
					if (attachedActorVFXInfo.HasVfxInstance())
					{
						attachedActorVFXInfo.SetInstanceLocalPosition(jointToVfx.m_localOffset);
						this.m_spawnedVfxList.Add(attachedActorVFXInfo);
					}
				}
				if (jointToVfx.m_vfxFriendlyOnlyPrefab != null)
				{
					AttachedActorVFXInfo attachedActorVFXInfo2 = new AttachedActorVFXInfo(jointToVfx.m_vfxFriendlyOnlyPrefab, this.m_actorData, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "AttachedVfxFriendly_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.FriendlyOnly);
					if (attachedActorVFXInfo2.HasVfxInstance())
					{
						attachedActorVFXInfo2.SetInstanceLocalPosition(jointToVfx.m_localOffset);
						this.m_spawnedVfxList.Add(attachedActorVFXInfo2);
					}
				}
				if (jointToVfx.m_vfxEnemyOnlyPrefab != null)
				{
					AttachedActorVFXInfo attachedActorVFXInfo3 = new AttachedActorVFXInfo(jointToVfx.m_vfxEnemyOnlyPrefab, this.m_actorData, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "AttachedVFXEnemy_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.EnemyOnly);
					if (attachedActorVFXInfo3.HasVfxInstance())
					{
						attachedActorVFXInfo3.SetInstanceLocalPosition(jointToVfx.m_localOffset);
						this.m_spawnedVfxList.Add(attachedActorVFXInfo3);
					}
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (this.m_spawnedVfxList != null)
		{
			for (int i = 0; i < this.m_spawnedVfxList.Count; i++)
			{
				this.m_spawnedVfxList[i].DestroyVfx();
			}
			this.m_spawnedVfxList.Clear();
		}
	}

	private void Update()
	{
		if (this.m_actorData != null)
		{
			bool flag;
			if (this.m_actorData.IsVisibleToClient())
			{
				flag = (this.m_actorData.GetActorModelDataRenderer() == null || this.m_actorData.GetActorModelDataRenderer().enabled);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			bool flag3 = this.m_actorData.IsModelAnimatorDisabled();
			bool flag4;
			if (GameFlowData.Get() != null)
			{
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					flag4 = (GameFlowData.Get().activeOwnedActorData.GetTeam() == this.m_actorData.GetTeam());
					goto IL_E1;
				}
			}
			flag4 = false;
			IL_E1:
			bool sameTeamAsClientActor = flag4;
			for (int i = 0; i < this.m_spawnedVfxList.Count; i++)
			{
				AttachedActorVFXInfo attachedActorVFXInfo = this.m_spawnedVfxList[i];
				bool actorVisible;
				if (flag2)
				{
					actorVisible = !flag3;
				}
				else
				{
					actorVisible = false;
				}
				attachedActorVFXInfo.UpdateVisibility(actorVisible, sameTeamAsClientActor);
			}
		}
	}

	[Serializable]
	public class JointToVfx
	{
		public string m_name;

		[JointPopup("Joint for VFX")]
		public JointPopupProperty m_joint;

		public bool m_alignToRootOrientation;

		public Vector3 m_localOffset = Vector3.zero;

		[Header("-- Vfx Prefabs, vfx prefab for ones that should appear to both friend and enemy")]
		public GameObject m_vfxCommonPrefab;

		public GameObject m_vfxFriendlyOnlyPrefab;

		public GameObject m_vfxEnemyOnlyPrefab;
	}
}
