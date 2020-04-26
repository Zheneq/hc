using UnityEngine;

namespace AbilityContextNamespace
{
	public class ActorHitContext
	{
		public Vector3 _001D;

		public bool _000E;

		public bool _0012;

		public ContextVars _0015 = new ContextVars();

		public void _0016()
		{
			_001D = Vector3.zero;
			_0015.Clear();
			_0012 = false;
			_000E = false;
		}
	}
}
