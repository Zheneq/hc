using System;
using UnityEngine;

public class AttachedActorVFXInfo
{
	protected GameObject m_vfxInstance;

	protected FriendlyEnemyVFXSelector m_fofSelector;

	protected GameObject m_attachedToObject;

	protected ActorData m_actor;

	protected JointPopupProperty m_joint;

	protected bool m_alignToRootOrientation;

	protected GameObject m_attachParentObject;

	protected string m_parentObjectName = "VfxAttach";

	protected Team m_casterTeam = Team.Invalid;

	protected AttachedActorVFXInfo.FriendOrFoeVisibility m_friendOrFoeVisibility;

	public AttachedActorVFXInfo(GameObject vfxPrefab, ActorData actor, JointPopupProperty vfxJoint, bool alignToRootOrientation, string parentObjectName, AttachedActorVFXInfo.FriendOrFoeVisibility fofVisibility)
	{
		this.m_actor = actor;
		this.Initialize(vfxPrefab, (!(actor != null)) ? null : actor.gameObject, vfxJoint, alignToRootOrientation, parentObjectName, fofVisibility);
	}

	public AttachedActorVFXInfo(GameObject vfxPrefab, GameObject attachedObject, JointPopupProperty vfxJoint, bool alignToRootOrientation, string parentObjectName, AttachedActorVFXInfo.FriendOrFoeVisibility fofVisibility)
	{
		this.Initialize(vfxPrefab, attachedObject, vfxJoint, alignToRootOrientation, parentObjectName, fofVisibility);
	}

	private void Initialize(GameObject vfxPrefab, GameObject attachedToObject, JointPopupProperty vfxJoint, bool alignToRootOrientation, string parentObjectName, AttachedActorVFXInfo.FriendOrFoeVisibility fofVisibility)
	{
		this.m_vfxInstance = null;
		this.m_attachedToObject = attachedToObject;
		this.m_joint = new JointPopupProperty();
		this.m_joint.m_joint = vfxJoint.m_joint;
		this.m_joint.m_jointCharacter = vfxJoint.m_jointCharacter;
		this.m_alignToRootOrientation = alignToRootOrientation;
		this.m_parentObjectName = parentObjectName;
		this.m_friendOrFoeVisibility = fofVisibility;
		if (attachedToObject != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AttachedActorVFXInfo.Initialize(GameObject, GameObject, JointPopupProperty, bool, string, AttachedActorVFXInfo.FriendOrFoeVisibility)).MethodHandle;
			}
			if (vfxPrefab != null)
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
				this.m_joint.Initialize(attachedToObject);
				if (this.m_joint.m_jointObject != null)
				{
					this.m_vfxInstance = UnityEngine.Object.Instantiate<GameObject>(vfxPrefab);
					if (this.m_vfxInstance != null)
					{
						this.m_vfxInstance.SetActive(false);
						GameObject gameObject = new GameObject(this.m_parentObjectName);
						gameObject.transform.parent = this.m_joint.m_jointObject.transform;
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localScale = Vector3.one;
						gameObject.transform.localRotation = Quaternion.identity;
						this.m_attachParentObject = gameObject;
						this.m_vfxInstance.transform.parent = gameObject.transform;
						this.m_vfxInstance.transform.localPosition = Vector3.zero;
						this.m_vfxInstance.transform.localScale = Vector3.one;
						this.m_vfxInstance.transform.localRotation = Quaternion.identity;
						this.m_fofSelector = this.m_vfxInstance.GetComponent<FriendlyEnemyVFXSelector>();
					}
					else
					{
						Debug.LogWarning("Failed to spawn Vfx prefab");
					}
				}
				else
				{
					Log.Warning("Did not find joint for vfx, on actor " + attachedToObject.name, new object[0]);
				}
			}
		}
	}

	public virtual void DestroyVfx()
	{
		if (this.m_vfxInstance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AttachedActorVFXInfo.DestroyVfx()).MethodHandle;
			}
			this.m_fofSelector = null;
			UnityEngine.Object.Destroy(this.m_vfxInstance);
			this.m_vfxInstance = null;
		}
		if (this.m_attachParentObject != null)
		{
			UnityEngine.Object.Destroy(this.m_attachParentObject);
			this.m_attachParentObject = null;
		}
	}

	public virtual void UpdateVisibility(bool actorVisible, bool sameTeamAsClientActor)
	{
		if (this.m_vfxInstance != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AttachedActorVFXInfo.UpdateVisibility(bool, bool)).MethodHandle;
			}
			if (this.m_attachParentObject != null)
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
				bool flag;
				if (this.m_friendOrFoeVisibility != AttachedActorVFXInfo.FriendOrFoeVisibility.Both)
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
					if (this.m_friendOrFoeVisibility != AttachedActorVFXInfo.FriendOrFoeVisibility.FriendlyOnly || !sameTeamAsClientActor)
					{
						if (this.m_friendOrFoeVisibility == AttachedActorVFXInfo.FriendOrFoeVisibility.EnemyOnly)
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
							flag = !sameTeamAsClientActor;
						}
						else
						{
							flag = false;
						}
						goto IL_7C;
					}
				}
				flag = true;
				IL_7C:
				bool flag2 = flag;
				bool flag3 = actorVisible && flag2;
				if (this.m_vfxInstance.activeSelf != flag3)
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
					this.m_vfxInstance.SetActive(flag3);
				}
				if (flag3 && this.m_alignToRootOrientation)
				{
					this.m_vfxInstance.transform.rotation = this.m_attachedToObject.transform.rotation;
				}
				if (this.m_fofSelector != null && flag3 && this.m_casterTeam != Team.Invalid)
				{
					this.m_fofSelector.Setup(this.m_casterTeam);
				}
			}
		}
	}

	public void SetCasterTeam(Team team)
	{
		this.m_casterTeam = team;
	}

	public bool HasVfxInstance()
	{
		return this.m_vfxInstance != null;
	}

	public void SetInstanceScale(Vector3 scale)
	{
		if (this.m_vfxInstance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AttachedActorVFXInfo.SetInstanceScale(Vector3)).MethodHandle;
			}
			this.m_vfxInstance.transform.localScale = scale;
		}
	}

	public void SetInstanceLocalPosition(Vector3 localPosition)
	{
		if (this.m_vfxInstance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AttachedActorVFXInfo.SetInstanceLocalPosition(Vector3)).MethodHandle;
			}
			this.m_vfxInstance.transform.localPosition = localPosition;
		}
	}

	public Vector3 GetInstancePosition()
	{
		if (this.m_vfxInstance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AttachedActorVFXInfo.GetInstancePosition()).MethodHandle;
			}
			return this.m_vfxInstance.transform.position;
		}
		return Vector3.zero;
	}

	public void SetInstanceLayer(int layer)
	{
		if (this.m_vfxInstance != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AttachedActorVFXInfo.SetInstanceLayer(int)).MethodHandle;
			}
			this.m_vfxInstance.transform.gameObject.SetLayerRecursively(layer);
		}
	}

	public void RestartEffects()
	{
		if (this.m_vfxInstance != null)
		{
			foreach (PKFxFX pkfxFX in this.m_vfxInstance.GetComponentsInChildren<PKFxFX>())
			{
				pkfxFX.TerminateEffect();
				pkfxFX.StartEffect();
			}
		}
	}

	public enum FriendOrFoeVisibility
	{
		Both,
		FriendlyOnly,
		EnemyOnly
	}
}
