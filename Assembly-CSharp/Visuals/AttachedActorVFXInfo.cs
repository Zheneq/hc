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

    public AttachedActorVFXInfo(
        GameObject vfxPrefab,
        ActorData actor,
        JointPopupProperty vfxJoint,
        bool alignToRootOrientation,
        string parentObjectName,
        FriendOrFoeVisibility fofVisibility)
    {
        m_actor = actor;
        Initialize(
            vfxPrefab,
            actor != null ? actor.gameObject : null,
            vfxJoint,
            alignToRootOrientation,
            parentObjectName,
            fofVisibility);
    }

    public AttachedActorVFXInfo(
        GameObject vfxPrefab,
        GameObject attachedObject,
        JointPopupProperty vfxJoint,
        bool alignToRootOrientation,
        string parentObjectName,
        FriendOrFoeVisibility fofVisibility)
    {
        Initialize(vfxPrefab, attachedObject, vfxJoint, alignToRootOrientation, parentObjectName, fofVisibility);
    }

    private void Initialize(
        GameObject vfxPrefab,
        GameObject attachedToObject,
        JointPopupProperty vfxJoint,
        bool alignToRootOrientation,
        string parentObjectName,
        FriendOrFoeVisibility fofVisibility)
    {
        m_vfxInstance = null;
        m_attachedToObject = attachedToObject;
        m_joint = new JointPopupProperty
        {
            m_joint = vfxJoint.m_joint,
            m_jointCharacter = vfxJoint.m_jointCharacter
        };
        m_alignToRootOrientation = alignToRootOrientation;
        m_parentObjectName = parentObjectName;
        m_friendOrFoeVisibility = fofVisibility;

        if (attachedToObject != null && vfxPrefab != null)
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
                Log.Warning("Did not find joint for vfx, on actor " + attachedToObject.name);
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
        if (m_vfxInstance == null || m_attachParentObject == null)
        {
            return;
        }

        bool effectVisible = m_friendOrFoeVisibility == FriendOrFoeVisibility.Both
                             || (m_friendOrFoeVisibility == FriendOrFoeVisibility.FriendlyOnly && sameTeamAsClientActor)
                             || m_friendOrFoeVisibility == FriendOrFoeVisibility.EnemyOnly && !sameTeamAsClientActor;
        bool isVisible = actorVisible && effectVisible;
        if (m_vfxInstance.activeSelf != isVisible)
        {
            m_vfxInstance.SetActive(isVisible);
        }

        if (isVisible && m_alignToRootOrientation)
        {
            m_vfxInstance.transform.rotation = m_attachedToObject.transform.rotation;
        }

        if (m_fofSelector != null && isVisible && m_casterTeam != Team.Invalid)
        {
            m_fofSelector.Setup(m_casterTeam);
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
        if (m_vfxInstance != null)
        {
            m_vfxInstance.transform.localScale = scale;
        }
    }

    public void SetInstanceLocalPosition(Vector3 localPosition)
    {
        if (m_vfxInstance != null)
        {
            m_vfxInstance.transform.localPosition = localPosition;
        }
    }

    public Vector3 GetInstancePosition()
    {
        return m_vfxInstance != null ? m_vfxInstance.transform.position : Vector3.zero;
    }

    public void SetInstanceLayer(int layer)
    {
        if (m_vfxInstance != null)
        {
            m_vfxInstance.transform.gameObject.SetLayerRecursively(layer);
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