using System;
using System.Collections.Generic;
using UnityEngine;

public class ScampVfxController : CopyableVfxControllerComponent
{
	[JointPopup("Joint for VFX")]
	public JointPopupProperty m_suitJoint;

	public List<AdditionalAttachedActorVfx.JointToVfx> m_persistentVfxList;

	private List<AttachedActorVFXInfo> m_vfxInstances = new List<AttachedActorVFXInfo>();

	private ActorModelData m_actorModelData;

	private void Start()
	{
		this.m_actorModelData = base.GetComponent<ActorModelData>();
		this.m_suitJoint.Initialize(base.gameObject);
		if (this.m_suitJoint.m_jointObject != null)
		{
			for (int i = 0; i < this.m_persistentVfxList.Count; i++)
			{
				AdditionalAttachedActorVfx.JointToVfx jointToVfx = this.m_persistentVfxList[i];
				AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(jointToVfx.m_vfxCommonPrefab, base.gameObject, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "MartyrCrystalVfx_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
				if (attachedActorVFXInfo.HasVfxInstance())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ScampVfxController.Start()).MethodHandle;
					}
					attachedActorVFXInfo.SetInstanceLocalPosition(jointToVfx.m_localOffset);
					this.m_vfxInstances.Add(attachedActorVFXInfo);
				}
				else if (Application.isEditor)
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
					Debug.LogWarning("Failed to spawn vfx on joint in " + base.GetType().ToString());
				}
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

	private void LateUpdate()
	{
		ActorData actorData;
		if (this.m_actorModelData != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampVfxController.LateUpdate()).MethodHandle;
			}
			actorData = this.m_actorModelData.m_parentActorData;
		}
		else
		{
			actorData = null;
		}
		ActorData actorData2 = actorData;
		if (this.m_suitJoint.IsInitialized() && this.m_suitJoint.m_jointObject != null)
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
			bool flag = !this.IsSuitVisuallyShown();
			bool flag2;
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
				if (actorData2.IsVisibleToClient())
				{
					if (!(actorData2.GetActorModelData() == null))
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
						flag2 = actorData2.GetActorModelData().IsVisibleToClient();
					}
					else
					{
						flag2 = true;
					}
				}
				else
				{
					flag2 = false;
				}
			}
			else
			{
				flag2 = true;
			}
			bool flag3 = flag2;
			for (int i = 0; i < this.m_vfxInstances.Count; i++)
			{
				AttachedActorVFXInfo attachedActorVFXInfo = this.m_vfxInstances[i];
				bool actorVisible;
				if (flag3)
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
					actorVisible = !flag;
				}
				else
				{
					actorVisible = false;
				}
				attachedActorVFXInfo.UpdateVisibility(actorVisible, true);
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
		}
	}

	public bool IsSuitVisuallyShown()
	{
		return !this.m_suitJoint.IsInitialized() || !(this.m_suitJoint.m_jointObject != null) || this.m_suitJoint.m_jointObject.transform.localScale.x > 0.1f;
	}
}
