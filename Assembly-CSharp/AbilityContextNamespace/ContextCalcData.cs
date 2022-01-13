using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class ContextCalcData
	{
		public ContextVars m_nonActorSpecificContext = new ContextVars();
		public Dictionary<ActorData, ActorHitContext> m_actorToHitContext = new Dictionary<ActorData, ActorHitContext>();

		public void ResetContextData()
		{
			m_actorToHitContext.Clear();
			m_nonActorSpecificContext.ClearData();
		}

		public void AddHitActor(ActorData actor, Vector3 source, bool flag = false)
		{
			if (actor == null)
			{
				Log.Error("Trying to add null actor");
			}
			if (!m_actorToHitContext.ContainsKey(actor))
			{
				m_actorToHitContext.Add(actor, new ActorHitContext());
				m_actorToHitContext[actor].m_hitOrigin = source;
				m_actorToHitContext[actor].m_ignoreMinCoverDist = flag;
			}
			else
			{
				if (!Application.isEditor)
				{
					return;
				}
				Log.Warning("TargetSelect context: trying to add actor more than once");
			}
		}

		public void SetActorContext(ActorData actor, int index, int value)
		{
			if (m_actorToHitContext.ContainsKey(actor))
			{
				m_actorToHitContext[actor].m_contextVars.SetValue(index, value);
				return;
			}
			if (Application.isEditor)
			{
				Log.Warning("Setting context for actor we didn't track");
			}
		}

		public void SetActorContext(ActorData actor, int index, float value)
		{
			if (m_actorToHitContext.ContainsKey(actor))
			{
				m_actorToHitContext[actor].m_contextVars.SetValue(index, value);
				return;
			}
			if (Application.isEditor)
			{
				Log.Warning("Setting context for actor we didn't track");
			}
		}

		public void SetActorContext(ActorData actor, int index, Vector3 value)
		{
			if (m_actorToHitContext.ContainsKey(actor))
			{
				m_actorToHitContext[actor].m_contextVars.SetValue(index, value);
				return;
			}
			if (Application.isEditor)
			{
				Log.Warning("Setting context for actor we didn't track");
			}
		}
	}
}
