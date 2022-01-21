using System;
using UnityEngine;

namespace AbilityContextNamespace
{
	[Serializable]
	public class EffectFieldOverride
	{
		[Header("-- Identifier string to match in base ability")]
		public string m_targetIdentifier = string.Empty;

		public OnHitEffecField m_effectOverride;

		public string GetIdentifier()
		{
			return m_targetIdentifier.Trim();
		}
	}
}
