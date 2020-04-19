using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class ContextCalcData
	{
		public ContextVars \u001D = new ContextVars();

		public Dictionary<ActorData, ActorHitContext> \u000E = new Dictionary<ActorData, ActorHitContext>();

		public void \u0012()
		{
			this.\u000E.Clear();
			this.\u001D.\u0015();
		}

		public void \u0012(ActorData \u001D, Vector3 \u000E, bool \u0012 = false)
		{
			if (\u001D == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ContextCalcData.\u0012(ActorData, Vector3, bool)).MethodHandle;
				}
				Log.Error("Trying to add null actor", new object[0]);
			}
			if (!this.\u000E.ContainsKey(\u001D))
			{
				this.\u000E.Add(\u001D, new ActorHitContext());
				this.\u000E[\u001D].\u001D = \u000E;
				this.\u000E[\u001D].\u000E = \u0012;
			}
			else if (Application.isEditor)
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
				Log.Warning("TargetSelect context: trying to add actor more than once", new object[0]);
			}
		}

		public void \u0012(ActorData \u001D, int \u000E, int \u0012)
		{
			if (this.\u000E.ContainsKey(\u001D))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ContextCalcData.\u0012(ActorData, int, int)).MethodHandle;
				}
				this.\u000E[\u001D].\u0015.\u0016(\u000E, \u0012);
			}
			else if (Application.isEditor)
			{
				Log.Warning("Setting context for actor we didn't track", new object[0]);
			}
		}

		public void \u0012(ActorData \u001D, int \u000E, float \u0012)
		{
			if (this.\u000E.ContainsKey(\u001D))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ContextCalcData.\u0012(ActorData, int, float)).MethodHandle;
				}
				this.\u000E[\u001D].\u0015.\u0015(\u000E, \u0012);
			}
			else if (Application.isEditor)
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
				Log.Warning("Setting context for actor we didn't track", new object[0]);
			}
		}

		public void \u0012(ActorData \u001D, int \u000E, Vector3 \u0012)
		{
			if (this.\u000E.ContainsKey(\u001D))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ContextCalcData.\u0012(ActorData, int, Vector3)).MethodHandle;
				}
				this.\u000E[\u001D].\u0015.\u0015(\u000E, \u0012);
			}
			else if (Application.isEditor)
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
				Log.Warning("Setting context for actor we didn't track", new object[0]);
			}
		}
	}
}
