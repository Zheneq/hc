using System.Collections.Generic;
using UnityEngine;

public class DinoVfxController : CopyableVfxControllerComponent
{
	[Separator("Vfx prefabs for power levels, from low to high", true)]
	public List<GameObject> m_indicatorVfxPrefabs;

	[Separator("Vfx prefab for front end character select, etc", true)]
	public GameObject m_frontEndCharSelectVfxPrefab;

	[JointPopup("Fx Attach Joint")]
	public JointPopupProperty m_fxJoint;

	private ActorData m_owner;

	private ActorModelData m_actorModelData;

	private List<AttachedActorVFXInfo> m_spawnedVfxList;

	private Dino_SyncComponent m_syncComp;

	private void Start()
	{
		Initialize();
	}

	private void Initialize()
	{
		m_actorModelData = GetComponent<ActorModelData>();
		m_spawnedVfxList = new List<AttachedActorVFXInfo>();
		if (m_actorModelData != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_actorModelData.m_parentActorData != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				m_syncComp = m_actorModelData.m_parentActorData.GetComponent<Dino_SyncComponent>();
				m_owner = m_actorModelData.m_parentActorData;
			}
		}
		bool flag = m_syncComp == null;
		for (int i = 0; i < m_indicatorVfxPrefabs.Count; i++)
		{
			GameObject vfxPrefab = m_indicatorVfxPrefabs[i];
			if (m_frontEndCharSelectVfxPrefab != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (flag)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					vfxPrefab = m_frontEndCharSelectVfxPrefab;
				}
			}
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(vfxPrefab, base.gameObject, m_fxJoint, false, "DinoPowerVfx", AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				m_spawnedVfxList.Add(attachedActorVFXInfo);
			}
			else if (Application.isEditor)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				Debug.LogWarning("Failed to spawn vfx on joint in " + GetType().ToString());
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void LateUpdate()
	{
		ActorData owner = m_owner;
		bool flag = true;
		if (owner != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			flag = (owner.IsVisibleToClient() && (owner.GetActorModelData() == null || owner.GetActorModelData().IsVisibleToClient()));
		}
		bool flag2 = owner != null && owner.IsModelAnimatorDisabled();
		int num = 0;
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			num = m_syncComp.m_layerConePowerLevel;
			num = Mathf.Min(num, m_spawnedVfxList.Count - 1);
		}
		else if (m_spawnedVfxList.Count > 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			num = m_spawnedVfxList.Count - 1;
		}
		for (int i = 0; i < m_spawnedVfxList.Count; i++)
		{
			bool actorVisible = false;
			if (num == i)
			{
				int num2;
				if (flag)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					num2 = ((!flag2) ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
				actorVisible = ((byte)num2 != 0);
			}
			m_spawnedVfxList[i].UpdateVisibility(actorVisible, true);
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

	private void OnDestroy()
	{
		if (m_spawnedVfxList == null)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < m_spawnedVfxList.Count; i++)
			{
				m_spawnedVfxList[i].DestroyVfx();
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				m_spawnedVfxList.Clear();
				return;
			}
		}
	}
}
