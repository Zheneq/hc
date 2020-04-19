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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AdditionalAttachedActorVfx.Start()).MethodHandle;
			}
			for (int i = 0; i < this.m_vfxToAttach.Length; i++)
			{
				AdditionalAttachedActorVfx.JointToVfx jointToVfx = this.m_vfxToAttach[i];
				if (jointToVfx.m_vfxCommonPrefab != null)
				{
					AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(jointToVfx.m_vfxCommonPrefab, this.m_actorData, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "AttachedVfx_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
					if (attachedActorVFXInfo.HasVfxInstance())
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						attachedActorVFXInfo.SetInstanceLocalPosition(jointToVfx.m_localOffset);
						this.m_spawnedVfxList.Add(attachedActorVFXInfo);
					}
				}
				if (jointToVfx.m_vfxFriendlyOnlyPrefab != null)
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
					AttachedActorVFXInfo attachedActorVFXInfo2 = new AttachedActorVFXInfo(jointToVfx.m_vfxFriendlyOnlyPrefab, this.m_actorData, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "AttachedVfxFriendly_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.FriendlyOnly);
					if (attachedActorVFXInfo2.HasVfxInstance())
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
						attachedActorVFXInfo2.SetInstanceLocalPosition(jointToVfx.m_localOffset);
						this.m_spawnedVfxList.Add(attachedActorVFXInfo2);
					}
				}
				if (jointToVfx.m_vfxEnemyOnlyPrefab != null)
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
					AttachedActorVFXInfo attachedActorVFXInfo3 = new AttachedActorVFXInfo(jointToVfx.m_vfxEnemyOnlyPrefab, this.m_actorData, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "AttachedVFXEnemy_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.EnemyOnly);
					if (attachedActorVFXInfo3.HasVfxInstance())
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
						attachedActorVFXInfo3.SetInstanceLocalPosition(jointToVfx.m_localOffset);
						this.m_spawnedVfxList.Add(attachedActorVFXInfo3);
					}
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AdditionalAttachedActorVfx.OnDestroy()).MethodHandle;
			}
			this.m_spawnedVfxList.Clear();
		}
	}

	private void Update()
	{
		if (this.m_actorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AdditionalAttachedActorVfx.Update()).MethodHandle;
			}
			bool flag;
			if (this.m_actorData.\u0018())
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
				flag = (this.m_actorData.\u000E() == null || this.m_actorData.\u000E().enabled);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			bool flag3 = this.m_actorData.\u0012();
			bool flag4;
			if (GameFlowData.Get() != null)
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
				if (GameFlowData.Get().activeOwnedActorData != null)
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
					flag4 = (GameFlowData.Get().activeOwnedActorData.\u000E() == this.m_actorData.\u000E());
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
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					actorVisible = !flag3;
				}
				else
				{
					actorVisible = false;
				}
				attachedActorVFXInfo.UpdateVisibility(actorVisible, sameTeamAsClientActor);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
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
