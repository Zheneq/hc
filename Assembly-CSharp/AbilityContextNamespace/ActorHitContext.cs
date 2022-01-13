using UnityEngine;

namespace AbilityContextNamespace
{
	public class ActorHitContext
	{
		public Vector3 m_hitOrigin;
		public bool m_ignoreMinCoverDist;
		public bool m_inRangeForTargeter;
		public ContextVars m_contextVars = new ContextVars();

		public void Reset()
		{
			m_hitOrigin = Vector3.zero;
			m_contextVars.ClearData();
			m_inRangeForTargeter = false;
			m_ignoreMinCoverDist = false;
		}
	}
}
