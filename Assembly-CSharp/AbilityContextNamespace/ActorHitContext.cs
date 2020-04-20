using System;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class ActorHitContext
	{
		public Vector3 symbol_001D;

		public bool symbol_000E;

		public bool symbol_0012;

		public ContextVars symbol_0015 = new ContextVars();

		public void symbol_0016()
		{
			this.symbol_001D = Vector3.zero;
			this.symbol_0015.Clear();
			this.symbol_0012 = false;
			this.symbol_000E = false;
		}
	}
}
