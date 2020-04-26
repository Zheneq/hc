using System;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalAttachedActorVfx : MonoBehaviour
{
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

	public JointToVfx[] m_vfxToAttach;

	private ActorData m_actorData;

	private List<AttachedActorVFXInfo> m_spawnedVfxList;

	private void Start()
	{
		m_actorData = GetComponent<ActorData>();
		m_spawnedVfxList = new List<AttachedActorVFXInfo>();
		if (!(m_actorData != null) || m_vfxToAttach == null)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_vfxToAttach.Length; i++)
			{
				JointToVfx jointToVfx = m_vfxToAttach[i];
				if (jointToVfx.m_vfxCommonPrefab != null)
				{
					AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(jointToVfx.m_vfxCommonPrefab, m_actorData, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "AttachedVfx_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
					if (attachedActorVFXInfo.HasVfxInstance())
					{
						attachedActorVFXInfo.SetInstanceLocalPosition(jointToVfx.m_localOffset);
						m_spawnedVfxList.Add(attachedActorVFXInfo);
					}
				}
				if (jointToVfx.m_vfxFriendlyOnlyPrefab != null)
				{
					AttachedActorVFXInfo attachedActorVFXInfo2 = new AttachedActorVFXInfo(jointToVfx.m_vfxFriendlyOnlyPrefab, m_actorData, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "AttachedVfxFriendly_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.FriendlyOnly);
					if (attachedActorVFXInfo2.HasVfxInstance())
					{
						attachedActorVFXInfo2.SetInstanceLocalPosition(jointToVfx.m_localOffset);
						m_spawnedVfxList.Add(attachedActorVFXInfo2);
					}
				}
				if (!(jointToVfx.m_vfxEnemyOnlyPrefab != null))
				{
					continue;
				}
				AttachedActorVFXInfo attachedActorVFXInfo3 = new AttachedActorVFXInfo(jointToVfx.m_vfxEnemyOnlyPrefab, m_actorData, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "AttachedVFXEnemy_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.EnemyOnly);
				if (attachedActorVFXInfo3.HasVfxInstance())
				{
					attachedActorVFXInfo3.SetInstanceLocalPosition(jointToVfx.m_localOffset);
					m_spawnedVfxList.Add(attachedActorVFXInfo3);
				}
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (m_spawnedVfxList == null)
		{
			return;
		}
		for (int i = 0; i < m_spawnedVfxList.Count; i++)
		{
			m_spawnedVfxList[i].DestroyVfx();
		}
		while (true)
		{
			m_spawnedVfxList.Clear();
			return;
		}
	}

	private void Update()
	{
		if (!(m_actorData != null))
		{
			return;
		}
		while (true)
		{
			int num;
			if (m_actorData.IsVisibleToClient())
			{
				num = ((m_actorData.GetActorModelDataRenderer() == null || m_actorData.GetActorModelDataRenderer().enabled) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag = (byte)num != 0;
			bool flag2 = m_actorData.IsModelAnimatorDisabled();
			int num2;
			if (GameFlowData.Get() != null)
			{
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					num2 = ((GameFlowData.Get().activeOwnedActorData.GetTeam() == m_actorData.GetTeam()) ? 1 : 0);
					goto IL_00e1;
				}
			}
			num2 = 0;
			goto IL_00e1;
			IL_00e1:
			bool sameTeamAsClientActor = (byte)num2 != 0;
			for (int i = 0; i < m_spawnedVfxList.Count; i++)
			{
				AttachedActorVFXInfo attachedActorVFXInfo = m_spawnedVfxList[i];
				int actorVisible;
				if (flag)
				{
					actorVisible = ((!flag2) ? 1 : 0);
				}
				else
				{
					actorVisible = 0;
				}
				attachedActorVFXInfo.UpdateVisibility((byte)actorVisible != 0, sameTeamAsClientActor);
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}
}
