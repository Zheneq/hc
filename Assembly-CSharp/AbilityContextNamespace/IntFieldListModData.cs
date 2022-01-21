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

		public List<OnHitIntField> GetModdedIntFieldList(List<OnHitIntField> input)
		{
			List<OnHitIntField> list = new List<OnHitIntField>();
			for (int i = 0; i < m_prependIntFields.Count; i++)
			{
				list.Add(m_prependIntFields[i].GetCopy());
			}
			for (int j = 0; j < input.Count; j++)
			{
				OnHitIntField onHitIntField = input[j];
				string identifier = onHitIntField.GetIdentifier();
				SingleOnHitIntFieldMod singleOnHitIntFieldMod = this.GetOverrideEntry(identifier);
				if (singleOnHitIntFieldMod != null)
				{
					OnHitIntField item = singleOnHitIntFieldMod._001D(onHitIntField);
					list.Add(item);
				}
				else
				{
					list.Add(onHitIntField.GetCopy());
				}
			}
			return list;
		}

		private SingleOnHitIntFieldMod GetOverrideEntry(string identifier)
		{
			if (string.IsNullOrEmpty(identifier))
			{
				return null;
			}
			for (int i = 0; i < m_overrides.Count; i++)
			{
				string text = m_overrides[i].GetIdentifier();
				if (text.Equals(identifier, StringComparison.OrdinalIgnoreCase))
				{
					return m_overrides[i].m_fieldOverride;
				}
			}
			return null;
		}
	}
}
