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
		m_actorModelData = GetComponent<ActorModelData>();
		if (!(m_actorModelData != null))
		{
			return;
		}
		while (true)
		{
			if (!(m_actorModelData.m_parentActorData != null))
			{
				return;
			}
			while (true)
			{
				m_owner = m_actorModelData.m_parentActorData;
				m_owner.OnAnimationEventDelegates += HandleAnimEvent;
				m_owner.OnTurnStartDelegates += HandleTurnTick;
				for (int i = 0; i < m_animEventToVfxDataList.Count; i++)
				{
					AnimEventToAttachedVfxData animEventToAttachedVfxData = m_animEventToVfxDataList[i];
					m_animDrivenVfxList.Add(new AnimEventToVfxContainer(animEventToAttachedVfxData.m_persistentVfxStartEvent, animEventToAttachedVfxData.m_persistentVfxStopEvent, animEventToAttachedVfxData.m_persistentVfxList, base.gameObject));
				}
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (m_owner != null)
		{
			m_owner.OnAnimationEventDelegates -= HandleAnimEvent;
			m_owner.OnTurnStartDelegates -= HandleTurnTick;
		}
	}

	private void Update()
	{
		object obj;
		if (m_actorModelData != null)
		{
			obj = m_actorModelData.m_parentActorData;
		}
		else
		{
			obj = null;
		}
		ActorData actorData = (ActorData)obj;
		int num;
		if (!(actorData == null))
		{
			if (actorData.IsActorVisibleToClient())
			{
				if (!(actorData.GetModelRenderer() == null))
				{
					num = (actorData.GetModelRenderer().enabled ? 1 : 0);
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				num = 0;
			}
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		int num2;
		if (actorData != null)
		{
			num2 = (actorData.IsInRagdoll() ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool flag2 = (byte)num2 != 0;
		int num3;
		if (!(actorData == null))
		{
			if (GameFlowData.Get() != null)
			{
				if (GameFlowData.Get().activeOwnedActorData != null)
				{
					num3 = ((GameFlowData.Get().activeOwnedActorData.GetTeam() == actorData.GetTeam()) ? 1 : 0);
					goto IL_0110;
				}
			}
			num3 = 0;
		}
		else
		{
			num3 = 1;
		}
		goto IL_0110;
		IL_0110:
		bool sameTeam = (byte)num3 != 0;
		for (int i = 0; i < m_animDrivenVfxList.Count; i++)
		{
			AnimEventToVfxContainer animEventToVfxContainer = m_animDrivenVfxList[i];
			int actorVisible;
			if (flag)
			{
				actorVisible = ((!flag2) ? 1 : 0);
			}
			else
			{
				actorVisible = 0;
			}
			animEventToVfxContainer.UpdateVisibilityForSpawnedVfx((byte)actorVisible != 0, sameTeam);
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void HandleTurnTick()
	{
		for (int i = 0; i < m_animDrivenVfxList.Count; i++)
		{
			if (m_animDrivenVfxList[i].m_turnOffOnTurnStart)
			{
				m_animDrivenVfxList[i].m_shouldShowPersistentVfx = false;
			}
		}
		while (true)
		{
			return;
		}
	}

	private void HandleAnimEvent(Object eventObj, GameObject sourceObject)
	{
		if (!(m_owner != null))
		{
			return;
		}
		for (int i = 0; i < m_animDrivenVfxList.Count; i++)
		{
			m_animDrivenVfxList[i].HandleAnimEvent(eventObj, sourceObject);
		}
		while (true)
		{
			return;
		}
	}
}
