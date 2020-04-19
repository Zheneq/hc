using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventDrivenAttachedVfxController : CopyableVfxControllerComponent
{
	[Header("-- Persistent vfx that can be started by anim event")]
	public List<AnimEventToAttachedVfxData> m_animEventToVfxDataList;

	private ActorData m_owner;

	private ActorModelData m_actorModelData;

	private List<AnimEventToVfxContainer> m_animDrivenVfxList = new List<AnimEventToVfxContainer>();

	private void Start()
	{
		this.m_actorModelData = base.GetComponent<ActorModelData>();
		if (this.m_actorModelData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnimEventDrivenAttachedVfxController.Start()).MethodHandle;
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
				this.m_owner = this.m_actorModelData.m_parentActorData;
				this.m_owner.OnAnimationEventDelegates += this.HandleAnimEvent;
				this.m_owner.OnTurnStartDelegates += this.HandleTurnTick;
				for (int i = 0; i < this.m_animEventToVfxDataList.Count; i++)
				{
					AnimEventToAttachedVfxData animEventToAttachedVfxData = this.m_animEventToVfxDataList[i];
					this.m_animDrivenVfxList.Add(new AnimEventToVfxContainer(animEventToAttachedVfxData.m_persistentVfxStartEvent, animEventToAttachedVfxData.m_persistentVfxStopEvent, animEventToAttachedVfxData.m_persistentVfxList, base.gameObject));
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
	}

	private void OnDestroy()
	{
		if (this.m_owner != null)
		{
			this.m_owner.OnAnimationEventDelegates -= this.HandleAnimEvent;
			this.m_owner.OnTurnStartDelegates -= this.HandleTurnTick;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnimEventDrivenAttachedVfxController.Update()).MethodHandle;
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
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
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = actorData2.\u000E().enabled;
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
			if (GameFlowData.Get() != null)
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
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					flag5 = (GameFlowData.Get().activeOwnedActorData.\u000E() == actorData2.\u000E());
					goto IL_10D;
				}
			}
			flag5 = false;
			IL_10D:;
		}
		else
		{
			flag5 = true;
		}
		bool sameTeam = flag5;
		for (int i = 0; i < this.m_animDrivenVfxList.Count; i++)
		{
			AnimEventToVfxContainer animEventToVfxContainer = this.m_animDrivenVfxList[i];
			bool actorVisible;
			if (flag2)
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
				actorVisible = !flag4;
			}
			else
			{
				actorVisible = false;
			}
			animEventToVfxContainer.UpdateVisibilityForSpawnedVfx(actorVisible, sameTeam);
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
	}

	private void HandleTurnTick()
	{
		for (int i = 0; i < this.m_animDrivenVfxList.Count; i++)
		{
			if (this.m_animDrivenVfxList[i].m_turnOffOnTurnStart)
			{
				this.m_animDrivenVfxList[i].m_shouldShowPersistentVfx = false;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(AnimEventDrivenAttachedVfxController.HandleTurnTick()).MethodHandle;
		}
	}

	private void HandleAnimEvent(UnityEngine.Object eventObj, GameObject sourceObject)
	{
		if (this.m_owner != null)
		{
			for (int i = 0; i < this.m_animDrivenVfxList.Count; i++)
			{
				this.m_animDrivenVfxList[i].HandleAnimEvent(eventObj, sourceObject);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AnimEventDrivenAttachedVfxController.HandleAnimEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
		}
	}
}
