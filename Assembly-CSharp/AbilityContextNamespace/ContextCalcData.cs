using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class ContextCalcData
	{
		public ContextVars m_contextVars = new ContextVars();

		public Dictionary<ActorData, ActorHitContext> m_actorHitContext = new Dictionary<ActorData, ActorHitContext>();

		public void Clear()
		{
			m_actorHitContext.Clear();
			m_contextVars.Clear();
		}

		public void Add(ActorData actor, Vector3 source, bool flag = false)
		{
			if (actor == null)
			{
				Log.Error("Trying to add null actor");
			}
			if (!this.m_actorHitContext.ContainsKey(actor))
			{
				this.m_actorHitContext.Add(actor, new ActorHitContext());
				this.m_actorHitContext[actor].source = source;
				this.m_actorHitContext[actor].flag = flag;
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

		public void Set(ActorData actor, int index, int value)
		{
			if (m_actorHitContext.ContainsKey(actor))
			{
				m_actorHitContext[actor].context.SetInt(index, value);
				return;
			}
			if (Application.isEditor)
			{
				Log.Warning("Setting context for actor we didn't track");
			}
		}

		public void Set(ActorData actor, int index, float value)
		{
			if (m_actorHitContext.ContainsKey(actor))
			{
				m_actorHitContext[actor].context.SetFloat(index, value);
				return;
			}
			if (Application.isEditor)
			{
				Log.Warning("Setting context for actor we didn't track");
			}
		}

		public void Set(ActorData actor, int index, Vector3 value)
		{
			if (m_actorHitContext.ContainsKey(actor))
			{
				m_actorHitContext[actor].context.SetVector(index, value);
				return;
			}
			if (Application.isEditor)
			{
				Log.Warning("Setting context for actor we didn't track");
			}
		}
	}
}
