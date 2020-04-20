using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class ContextCalcData
	{
		public ContextVars symbol_001D = new ContextVars();

		public Dictionary<ActorData, ActorHitContext> symbol_000E = new Dictionary<ActorData, ActorHitContext>();

		public void symbol_0012()
		{
			this.symbol_000E.Clear();
			this.symbol_001D.Clear();
		}

		public void symbol_0012(ActorData symbol_001D, Vector3 symbol_000E, bool symbol_0012 = false)
		{
			if (symbol_001D == null)
			{
				Log.Error("Trying to add null actor", new object[0]);
			}
			if (!this.symbol_000E.ContainsKey(symbol_001D))
			{
				this.symbol_000E.Add(symbol_001D, new ActorHitContext());
				this.symbol_000E[symbol_001D].symbol_001D = symbol_000E;
				this.symbol_000E[symbol_001D].symbol_000E = symbol_0012;
			}
			else if (Application.isEditor)
			{
				Log.Warning("TargetSelect context: trying to add actor more than once", new object[0]);
			}
		}

		public void symbol_0012(ActorData symbol_001D, int symbol_000E, int symbol_0012)
		{
			if (this.symbol_000E.ContainsKey(symbol_001D))
			{
				this.symbol_000E[symbol_001D].symbol_0015.SetInt(symbol_000E, symbol_0012);
			}
			else if (Application.isEditor)
			{
				Log.Warning("Setting context for actor we didn't track", new object[0]);
			}
		}

		public void symbol_0012(ActorData symbol_001D, int symbol_000E, float symbol_0012)
		{
			if (this.symbol_000E.ContainsKey(symbol_001D))
			{
				this.symbol_000E[symbol_001D].symbol_0015.SetFloat(symbol_000E, symbol_0012);
			}
			else if (Application.isEditor)
			{
				Log.Warning("Setting context for actor we didn't track", new object[0]);
			}
		}

		public void symbol_0012(ActorData symbol_001D, int symbol_000E, Vector3 symbol_0012)
		{
			if (this.symbol_000E.ContainsKey(symbol_001D))
			{
				this.symbol_000E[symbol_001D].symbol_0015.SetVector(symbol_000E, symbol_0012);
			}
			else if (Application.isEditor)
			{
				Log.Warning("Setting context for actor we didn't track", new object[0]);
			}
		}
	}
}
