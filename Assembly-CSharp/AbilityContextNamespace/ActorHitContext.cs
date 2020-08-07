using UnityEngine;

namespace AbilityContextNamespace
{
	public class ActorHitContext
	{
		public Vector3 source;
		public bool flag;
		public bool inRange;
		public ContextVars context = new ContextVars();

		public void Clear()
		{
			source = Vector3.zero;
			context.Clear();
			inRange = false;
			flag = false;
		}
	}
}
