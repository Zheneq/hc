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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiOrbVfxController.Initialize()).MethodHandle;
			}
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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				attachedActorVFXInfo.SetInstanceLocalPosition(jointToVfx.m_localOffset);
				this.m_spawnedCrystalVfxList.Add(attachedActorVFXInfo);
			}
			else if (Application.isEditor)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				Debug.LogWarning("Failed to spawn vfx on joint in " + base.GetType().ToString());
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void Update()
	{
		ActorData actorData;
		if (this.m_actorModelData != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiOrbVfxController.Update()).MethodHandle;
			}
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
			if (actorData2.\u0018())
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(actorData2.\u000E() == null))
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = actorData2.\u000E().IsVisibleToClient();
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			flag3 = actorData2.\u0012();
		}
		else
		{
			flag3 = false;
		}
		bool flag4 = flag3;
		bool flag5;
		if (!(actorData2 == null))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (GameFlowData.Get() != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					flag5 = (GameFlowData.Get().activeOwnedActorData.\u000E() == actorData2.\u000E());
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			int num;
			if (this.m_showOrbsInFrontEnd)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiOrbVfxController.OnDestroy()).MethodHandle;
			}
			this.m_spawnedCrystalVfxList.Clear();
		}
	}

	private int GetNumOrbs()
	{
		if (this.m_syncComp != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiOrbVfxController.GetNumOrbs()).MethodHandle;
			}
			return Mathf.Max(0, (int)this.m_syncComp.m_syncCurrentNumOrbs + this.m_syncComp.m_clientOrbNumAdjust);
		}
		return 0;
	}

	public Vector3 GetSpawnPosAndAdvanceCounter()
	{
		if (this.m_syncComp != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiOrbVfxController.GetSpawnPosAndAdvanceCounter()).MethodHandle;
			}
			if (this.m_spawnedCrystalVfxList.Count > 0)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				int num = this.GetNumOrbs() - 1;
				num = Mathf.Clamp(num, 0, this.m_spawnedCrystalVfxList.Count - 1);
				Vector3 instancePosition = this.m_spawnedCrystalVfxList[num].GetInstancePosition();
				this.m_syncComp.m_clientOrbNumAdjust--;
				return instancePosition;
			}
		}
		return this.m_owner.\u0015();
	}
}
