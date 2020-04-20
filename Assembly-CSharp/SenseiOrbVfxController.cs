using System;
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
		this.Initialize();
	}

	private void Initialize()
	{
		this.m_actorModelData = base.GetComponent<ActorModelData>();
		this.m_spawnedCrystalVfxList = new List<AttachedActorVFXInfo>();
		if (this.m_actorModelData != null)
		{
			if (this.m_actorModelData.m_parentActorData != null)
			{
				this.m_syncComp = this.m_actorModelData.m_parentActorData.GetComponent<Sensei_SyncComponent>();
				this.m_owner = this.m_actorModelData.m_parentActorData;
			}
		}
		for (int i = 0; i < this.m_jointToVfxList.Count; i++)
		{
			AdditionalAttachedActorVfx.JointToVfx jointToVfx = this.m_jointToVfxList[i];
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(jointToVfx.m_vfxCommonPrefab, base.gameObject, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "MartyrCrystalVfx_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
			{
				attachedActorVFXInfo.SetInstanceLocalPosition(jointToVfx.m_localOffset);
				this.m_spawnedCrystalVfxList.Add(attachedActorVFXInfo);
			}
			else if (Application.isEditor)
			{
				Debug.LogWarning("Failed to spawn vfx on joint in " + base.GetType().ToString());
			}
		}
	}

	private void Update()
	{
		ActorData actorData;
		if (this.m_actorModelData != null)
		{
			actorData = this.m_actorModelData.m_parentActorData;
		}
		else
		{
			actorData = null;
		}
		ActorData actorData2 = actorData;
		bool flag;
		if (!(actorData2 == null))
		{
			if (actorData2.IsVisibleToClient())
			{
				if (!(actorData2.GetActorModelData() == null))
				{
					flag = actorData2.GetActorModelData().IsVisibleToClient();
				}
				else
				{
					flag = true;
				}
			}
			else
			{
				flag = false;
			}
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		bool flag3;
		if (actorData2 != null)
		{
			flag3 = actorData2.IsModelAnimatorDisabled();
		}
		else
		{
			flag3 = false;
		}
		bool flag4 = flag3;
		bool flag5;
		if (!(actorData2 == null))
		{
			if (GameFlowData.Get() != null)
			{
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					flag5 = (GameFlowData.Get().activeOwnedActorData.GetTeam() == actorData2.GetTeam());
					goto IL_10B;
				}
			}
			flag5 = false;
			IL_10B:;
		}
		else
		{
			flag5 = true;
		}
		bool sameTeamAsClientActor = flag5;
		int num2;
		if (this.m_syncComp == null)
		{
			int num;
			if (this.m_showOrbsInFrontEnd)
			{
				num = this.m_spawnedCrystalVfxList.Count;
			}
			else
			{
				num = 0;
			}
			num2 = num;
		}
		else
		{
			num2 = this.GetNumOrbs();
		}
		int i = 0;
		while (i < this.m_spawnedCrystalVfxList.Count)
		{
			AttachedActorVFXInfo attachedActorVFXInfo = this.m_spawnedCrystalVfxList[i];
			if (!flag2)
			{
				goto IL_188;
			}
			if (flag4)
			{
				goto IL_188;
			}
			bool actorVisible = i < num2;
			IL_189:
			attachedActorVFXInfo.UpdateVisibility(actorVisible, sameTeamAsClientActor);
			i++;
			continue;
			IL_188:
			actorVisible = false;
			goto IL_189;
		}
	}

	private void OnDestroy()
	{
		if (this.m_spawnedCrystalVfxList != null)
		{
			for (int i = 0; i < this.m_spawnedCrystalVfxList.Count; i++)
			{
				this.m_spawnedCrystalVfxList[i].DestroyVfx();
			}
			this.m_spawnedCrystalVfxList.Clear();
		}
	}

	private int GetNumOrbs()
	{
		if (this.m_syncComp != null)
		{
			return Mathf.Max(0, (int)this.m_syncComp.m_syncCurrentNumOrbs + this.m_syncComp.m_clientOrbNumAdjust);
		}
		return 0;
	}

	public Vector3 GetSpawnPosAndAdvanceCounter()
	{
		if (this.m_syncComp != null)
		{
			if (this.m_spawnedCrystalVfxList.Count > 0)
			{
				int num = this.GetNumOrbs() - 1;
				num = Mathf.Clamp(num, 0, this.m_spawnedCrystalVfxList.Count - 1);
				Vector3 instancePosition = this.m_spawnedCrystalVfxList[num].GetInstancePosition();
				this.m_syncComp.m_clientOrbNumAdjust--;
				return instancePosition;
			}
		}
		return this.m_owner.GetTravelBoardSquareWorldPositionForLos();
	}
}
