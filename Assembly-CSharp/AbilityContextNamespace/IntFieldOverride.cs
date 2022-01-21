using System;
using UnityEngine;

namespace AbilityContextNamespace
{
	[Serializable]
	public class IntFieldOverride
	{
		[Header("-- Identifier string to match in base ability")]
		public string m_targetIdentifier = "";

		public SingleOnHitIntFieldMod m_fieldOverride;

		public string GetIdentifier()
		{
			return m_targetIdentifier.Trim();
		}
	}
}
