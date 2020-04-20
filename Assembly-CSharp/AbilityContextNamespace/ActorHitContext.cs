using System;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class ActorHitContext
	{
		public Vector3 \u001D;

		public bool \u000E;

		public bool \u0012;

		public ContextVars \u0015 = new ContextVars();

		public void \u0016()
		{
			this.\u001D = Vector3.zero;
			this.\u0015.Clear();
			this.\u0012 = false;
			this.\u000E = false;
		}
	}
}
