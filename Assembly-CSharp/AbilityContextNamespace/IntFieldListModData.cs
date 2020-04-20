using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	[Serializable]
	public class IntFieldListModData
	{
		[Header("-- Evaluated before original's int fields (first match would be used)")]
		public List<OnHitIntField> m_prependIntFields;

		[Header("-- Overrides to existing int fields")]
		public List<IntFieldOverride> m_overrides;

		public List<OnHitIntField> symbol_001D(List<OnHitIntField> symbol_001D)
		{
			List<OnHitIntField> list = new List<OnHitIntField>();
			for (int i = 0; i < this.m_prependIntFields.Count; i++)
			{
				list.Add(this.m_prependIntFields[i].GetCopy());
			}
			for (int j = 0; j < symbol_001D.Count; j++)
			{
				OnHitIntField onHitIntField = symbol_001D[j];
				string identifier = onHitIntField.GetIdentifier();
				SingleOnHitIntFieldMod singleOnHitIntFieldMod = this.symbol_001D(identifier);
				if (singleOnHitIntFieldMod != null)
				{
					OnHitIntField item = singleOnHitIntFieldMod.symbol_001D(onHitIntField);
					list.Add(item);
				}
				else
				{
					list.Add(onHitIntField.GetCopy());
				}
			}
			return list;
		}

		private SingleOnHitIntFieldMod symbol_001D(string symbol_001D)
		{
			if (string.IsNullOrEmpty(symbol_001D))
			{
				return null;
			}
			for (int i = 0; i < this.m_overrides.Count; i++)
			{
				string text = this.m_overrides[i].symbol_001D();
				if (text.Equals(symbol_001D, StringComparison.OrdinalIgnoreCase))
				{
					return this.m_overrides[i].m_fieldOverride;
				}
			}
			return null;
		}
	}
}
