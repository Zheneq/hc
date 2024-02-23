using System.Text;
using UnityEngine;

public class AttachedActorVFXInfo
{
	public enum FriendOrFoeVisibility
	{
		Both,
		FriendlyOnly,
		EnemyOnly
	}

	protected GameObject m_vfxInstance;

	protected FriendlyEnemyVFXSelector m_fofSelector;

	protected GameObject m_attachedToObject;

	protected ActorData m_actor;

	protected JointPopupProperty m_joint;

	protected bool m_alignToRootOrientation;

	protected GameObject m_attachParentObject;

	protected string m_parentObjectName = "VfxAttach";

	protected Team m_casterTeam = Team.Invalid;

	protected FriendOrFoeVisibility m_friendOrFoeVisibility;

	public AttachedActorVFXInfo(GameObject vfxPrefab, ActorData actor, JointPopupProperty vfxJoint, bool alignToRootOrientation, string parentObjectName, FriendOrFoeVisibility fofVisibility)
	{
		m_actor = actor;
		Initialize(vfxPrefab, (!(actor != null)) ? null : actor.gameObject, vfxJoint, alignToRootOrientation, parentObjectName, fofVisibility);
	}

	public AttachedActorVFXInfo(GameObject vfxPrefab, GameObject attachedObject, JointPopupProperty vfxJoint, bool alignToRootOrientation, string parentObjectName, FriendOrFoeVisibility fofVisibility)
	{
		Initialize(vfxPrefab, attachedObject, vfxJoint, alignToRootOrientation, parentObjectName, fofVisibility);
	}

	private void Initialize(GameObject vfxPrefab, GameObject attachedToObject, JointPopupProperty vfxJoint, bool alignToRootOrientation, string parentObjectName, FriendOrFoeVisibility fofVisibility)
	{
		m_vfxInstance = null;
		m_attachedToObject = attachedToObject;
		m_joint = new JointPopupProperty();
		m_joint.m_joint = vfxJoint.m_joint;
		m_joint.m_jointCharacter = vfxJoint.m_jointCharacter;
		m_alignToRootOrientation = alignToRootOrientation;
		m_parentObjectName = parentObjectName;
		m_friendOrFoeVisibility = fofVisibility;
		if (!(attachedToObject != null))
		{
			return;
		}
		while (true)
		{
			if (!(vfxPrefab != null))
			{
				return;
			}
			while (true)
			{
				m_joint.Initialize(attachedToObject);
				if (m_joint.m_jointObject != null)
				{
					m_vfxInstance = Object.Instantiate(vfxPrefab);
					if (m_vfxInstance != null)
					{
						m_vfxInstance.SetActive(false);
						GameObject gameObject = new GameObject(m_parentObjectName);
						gameObject.transform.parent = m_joint.m_jointObject.transform;
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localScale = Vector3.one;
						gameObject.transform.localRotation = Quaternion.identity;
						m_attachParentObject = gameObject;
						m_vfxInstance.transform.parent = gameObject.transform;
						m_vfxInstance.transform.localPosition = Vector3.zero;
						m_vfxInstance.transform.localScale = Vector3.one;
						m_vfxInstance.transform.localRotation = Quaternion.identity;
						m_fofSelector = m_vfxInstance.GetComponent<FriendlyEnemyVFXSelector>();
					}
					else
					{
						Debug.LogWarning("Failed to spawn Vfx prefab");
					}
				}
				else
				{
					Log.Warning(new StringBuilder().Append("Did not find joint for vfx, on actor ").Append(attachedToObject.name).ToString());
				}
				return;
			}
		}
	}

	public virtual void DestroyVfx()
	{
		if (m_vfxInstance != null)
		{
			m_fofSelector = null;
			Object.Destroy(m_vfxInstance);
			m_vfxInstance = null;
		}
		if (m_attachParentObject != null)
		{
			Object.Destroy(m_attachParentObject);
			m_attachParentObject = null;
		}
	}

	public virtual void UpdateVisibility(bool actorVisible, bool sameTeamAsClientActor)
	{
		if (!(m_vfxInstance != null))
		{
			return;
		}
		while (true)
		{
			if (!(m_attachParentObject != null))
			{
				return;
			}
			while (true)
			{
				int num;
				if (m_friendOrFoeVisibility != 0)
				{
					if (m_friendOrFoeVisibility != FriendOrFoeVisibility.FriendlyOnly || !sameTeamAsClientActor)
					{
						if (m_friendOrFoeVisibility == FriendOrFoeVisibility.EnemyOnly)
						{
							num = ((!sameTeamAsClientActor) ? 1 : 0);
						}
						else
						{
							num = 0;
						}
						goto IL_007c;
					}
				}
				num = 1;
				goto IL_007c;
				IL_007c:
				bool flag = (byte)num != 0;
				bool flag2 = actorVisible && flag;
				if (m_vfxInstance.activeSelf != flag2)
				{
					m_vfxInstance.SetActive(flag2);
				}
				if (flag2 && m_alignToRootOrientation)
				{
					m_vfxInstance.transform.rotation = m_attachedToObject.transform.rotation;
				}
				if (m_fofSelector != null && flag2 && m_casterTeam != Team.Invalid)
				{
					m_fofSelector.Setup(m_casterTeam);
				}
				return;
			}
		}
	}

	public void SetCasterTeam(Team team)
	{
		m_casterTeam = team;
	}

	public bool HasVfxInstance()
	{
		return m_vfxInstance != null;
	}

	public void SetInstanceScale(Vector3 scale)
	{
		if (!(m_vfxInstance != null))
		{
			return;
		}
		while (true)
		{
			m_vfxInstance.transform.localScale = scale;
			return;
		}
	}

	public void SetInstanceLocalPosition(Vector3 localPosition)
	{
		if (!(m_vfxInstance != null))
		{
			return;
		}
		while (true)
		{
			m_vfxInstance.transform.localPosition = localPosition;
			return;
		}
	}

	public Vector3 GetInstancePosition()
	{
		if (m_vfxInstance != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_vfxInstance.transform.position;
				}
			}
		}
		return Vector3.zero;
	}

	public void SetInstanceLayer(int layer)
	{
		if (!(m_vfxInstance != null))
		{
			return;
		}
		while (true)
		{
			m_vfxInstance.transform.gameObject.SetLayerRecursively(layer);
			return;
		}
	}

	public void RestartEffects()
	{
		if (m_vfxInstance != null)
		{
			PKFxFX[] componentsInChildren = m_vfxInstance.GetComponentsInChildren<PKFxFX>();
			foreach (PKFxFX pKFxFX in componentsInChildren)
			{
				pKFxFX.TerminateEffect();
				pKFxFX.StartEffect();
			}
		}
	}
}
