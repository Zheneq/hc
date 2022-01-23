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
		if (m_actorModelData != null)
		{
			if (m_actorModelData.m_parentActorData != null)
			{
				m_syncComp = m_actorModelData.m_parentActorData.GetComponent<Sensei_SyncComponent>();
				m_owner = m_actorModelData.m_parentActorData;
			}
		}
		for (int i = 0; i < m_jointToVfxList.Count; i++)
		{
			AdditionalAttachedActorVfx.JointToVfx jointToVfx = m_jointToVfxList[i];
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(jointToVfx.m_vfxCommonPrefab, base.gameObject, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "MartyrCrystalVfx_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
			{
				attachedActorVFXInfo.SetInstanceLocalPosition(jointToVfx.m_localOffset);
				m_spawnedCrystalVfxList.Add(attachedActorVFXInfo);
			}
			else if (Application.isEditor)
			{
				Debug.LogWarning("Failed to spawn vfx on joint in " + GetType().ToString());
			}
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void Update()
	{
		object obj;
		if (m_actorModelData != null)
		{
			obj = m_actorModelData.m_parentActorData;
		}
		else
		{
			obj = null;
		}
		ActorData actorData = (ActorData)obj;
		int num;
		if (!(actorData == null))
		{
			if (actorData.IsActorVisibleToClient())
			{
				if (!(actorData.GetActorModelData() == null))
				{
					num = (actorData.GetActorModelData().IsVisibleToClient() ? 1 : 0);
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				num = 0;
			}
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		int num2;
		if (actorData != null)
		{
			num2 = (actorData.IsInRagdoll() ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool flag2 = (byte)num2 != 0;
		int num3;
		if (!(actorData == null))
		{
			if (GameFlowData.Get() != null)
			{
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					num3 = ((GameFlowData.Get().activeOwnedActorData.GetTeam() == actorData.GetTeam()) ? 1 : 0);
					goto IL_010e;
				}
			}
			num3 = 0;
		}
		else
		{
			num3 = 1;
		}
		goto IL_010e;
		IL_010e:
		bool sameTeamAsClientActor = (byte)num3 != 0;
		int num4 = 0;
		if (m_syncComp == null)
		{
			int num5;
			if (m_showOrbsInFrontEnd)
			{
				num5 = m_spawnedCrystalVfxList.Count;
			}
			else
			{
				num5 = 0;
			}
			num4 = num5;
		}
		else
		{
			num4 = GetNumOrbs();
		}
		AttachedActorVFXInfo attachedActorVFXInfo;
		int actorVisible;
		for (int i = 0; i < m_spawnedCrystalVfxList.Count; attachedActorVFXInfo.UpdateVisibility((byte)actorVisible != 0, sameTeamAsClientActor), i++)
		{
			attachedActorVFXInfo = m_spawnedCrystalVfxList[i];
			if (flag)
			{
				if (!flag2)
				{
					actorVisible = ((i < num4) ? 1 : 0);
					continue;
				}
			}
			actorVisible = 0;
		}
	}

	private void OnDestroy()
	{
		if (m_spawnedCrystalVfxList == null)
		{
			return;
		}
		for (int i = 0; i < m_spawnedCrystalVfxList.Count; i++)
		{
			m_spawnedCrystalVfxList[i].DestroyVfx();
		}
		while (true)
		{
			m_spawnedCrystalVfxList.Clear();
			return;
		}
	}

	private int GetNumOrbs()
	{
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return Mathf.Max(0, m_syncComp.m_syncCurrentNumOrbs + m_syncComp.m_clientOrbNumAdjust);
				}
			}
		}
		return 0;
	}

	public Vector3 GetSpawnPosAndAdvanceCounter()
	{
		if (m_syncComp != null)
		{
			if (m_spawnedCrystalVfxList.Count > 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						int value = GetNumOrbs() - 1;
						value = Mathf.Clamp(value, 0, m_spawnedCrystalVfxList.Count - 1);
						Vector3 instancePosition = m_spawnedCrystalVfxList[value].GetInstancePosition();
						m_syncComp.m_clientOrbNumAdjust--;
						return instancePosition;
					}
					}
				}
			}
		}
		return m_owner.GetLoSCheckPos();
	}
}
