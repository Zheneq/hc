using System.Collections.Generic;
using System.Text;
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
		m_actorModelData = GetComponent<ActorModelData>();
		m_suitJoint.Initialize(gameObject);
		if (m_suitJoint.m_jointObject == null)
		{
			return;
		}
		foreach (AdditionalAttachedActorVfx.JointToVfx jointToVfx in m_persistentVfxList)
		{
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(
				jointToVfx.m_vfxCommonPrefab,
				gameObject,
				jointToVfx.m_joint,
				jointToVfx.m_alignToRootOrientation,
				new StringBuilder().Append("MartyrCrystalVfx_").Append(jointToVfx.m_name).ToString(),
				AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
			{
				attachedActorVFXInfo.SetInstanceLocalPosition(jointToVfx.m_localOffset);
				m_vfxInstances.Add(attachedActorVFXInfo);
			}
			else if (Application.isEditor)
			{
				Debug.LogWarning(new StringBuilder().Append("Failed to spawn vfx on joint in ").Append(GetType()).ToString());
			}
		}
	}

	private void LateUpdate()
	{
		ActorData actorData = m_actorModelData != null
			? m_actorModelData.m_parentActorData
			: null;
		if (m_suitJoint.IsInitialized() && m_suitJoint.m_jointObject != null)
		{
			bool isSuitNotShown = !IsSuitVisuallyShown();
			bool isVisible = actorData == null
			                 || actorData.IsActorVisibleToClient()
			                 && (actorData.GetActorModelData() == null ||
			                     actorData.GetActorModelData().IsVisibleToClient());
			foreach (AttachedActorVFXInfo fx in m_vfxInstances)
			{
				fx.UpdateVisibility(isVisible && !isSuitNotShown, true);
			}
		}
	}

	public bool IsSuitVisuallyShown()
	{
		return !m_suitJoint.IsInitialized()
		       || m_suitJoint.m_jointObject == null
		       || m_suitJoint.m_jointObject.transform.localScale.x > 0.1f;
	}
}
