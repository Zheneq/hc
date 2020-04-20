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
			if (this.m_actorModelData.m_parentActorData != null)
			{
				this.m_owner = this.m_actorModelData.m_parentActorData;
				this.m_owner.OnAnimationEventDelegates += this.HandleAnimEvent;
				this.m_owner.OnTurnStartDelegates += this.HandleTurnTick;
				for (int i = 0; i < this.m_animEventToVfxDataList.Count; i++)
				{
					AnimEventToAttachedVfxData animEventToAttachedVfxData = this.m_animEventToVfxDataList[i];
					this.m_animDrivenVfxList.Add(new AnimEventToVfxContainer(animEventToAttachedVfxData.m_persistentVfxStartEvent, animEventToAttachedVfxData.m_persistentVfxStopEvent, animEventToAttachedVfxData.m_persistentVfxList, base.gameObject));
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
			if (actorData2.IsVisibleToClient())
			{
				if (!(actorData2.GetActorModelDataRenderer() == null))
				{
					flag = actorData2.GetActorModelDataRenderer().enabled;
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
			flag3 = actorData2.IsModelAnimatorDisabled();
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
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					flag5 = (GameFlowData.Get().activeOwnedActorData.GetTeam() == actorData2.GetTeam());
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
				actorVisible = !flag4;
			}
			else
			{
				actorVisible = false;
			}
			animEventToVfxContainer.UpdateVisibilityForSpawnedVfx(actorVisible, sameTeam);
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
	}

	private void HandleAnimEvent(UnityEngine.Object eventObj, GameObject sourceObject)
	{
		if (this.m_owner != null)
		{
			for (int i = 0; i < this.m_animDrivenVfxList.Count; i++)
			{
				this.m_animDrivenVfxList[i].HandleAnimEvent(eventObj, sourceObject);
			}
		}
	}
}
