using System;
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
		this.Initialize();
	}

	private void Initialize()
	{
		this.m_actorModelData = base.GetComponent<ActorModelData>();
		this.m_spawnedVfxList = new List<AttachedActorVFXInfo>();
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoVfxController.Initialize()).MethodHandle;
			}
			if (this.m_actorModelData.m_parentActorData != null)
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
				this.m_syncComp = this.m_actorModelData.m_parentActorData.GetComponent<Dino_SyncComponent>();
				this.m_owner = this.m_actorModelData.m_parentActorData;
			}
		}
		bool flag = this.m_syncComp == null;
		for (int i = 0; i < this.m_indicatorVfxPrefabs.Count; i++)
		{
			GameObject vfxPrefab = this.m_indicatorVfxPrefabs[i];
			if (this.m_frontEndCharSelectVfxPrefab != null)
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
				if (flag)
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
					vfxPrefab = this.m_frontEndCharSelectVfxPrefab;
				}
			}
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(vfxPrefab, base.gameObject, this.m_fxJoint, false, "DinoPowerVfx", AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
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
				this.m_spawnedVfxList.Add(attachedActorVFXInfo);
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
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void LateUpdate()
	{
		ActorData owner = this.m_owner;
		bool flag = true;
		if (owner != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoVfxController.LateUpdate()).MethodHandle;
			}
			flag = (owner.\u0018() && (owner.\u000E() == null || owner.\u000E().IsVisibleToClient()));
		}
		bool flag2 = owner != null && owner.\u0012();
		int num = 0;
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
			num = (int)this.m_syncComp.m_layerConePowerLevel;
			num = Mathf.Min(num, this.m_spawnedVfxList.Count - 1);
		}
		else if (this.m_spawnedVfxList.Count > 0)
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
			num = this.m_spawnedVfxList.Count - 1;
		}
		for (int i = 0; i < this.m_spawnedVfxList.Count; i++)
		{
			bool actorVisible = false;
			if (num == i)
			{
				bool flag3;
				if (flag)
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
					flag3 = !flag2;
				}
				else
				{
					flag3 = false;
				}
				actorVisible = flag3;
			}
			this.m_spawnedVfxList[i].UpdateVisibility(actorVisible, true);
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

	private void OnDestroy()
	{
		if (this.m_spawnedVfxList != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DinoVfxController.OnDestroy()).MethodHandle;
			}
			for (int i = 0; i < this.m_spawnedVfxList.Count; i++)
			{
				this.m_spawnedVfxList[i].DestroyVfx();
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
			this.m_spawnedVfxList.Clear();
		}
	}
}
