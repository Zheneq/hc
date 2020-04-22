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

		public List<OnHitIntField> _001D(List<OnHitIntField> _001D)
		{
			List<OnHitIntField> list = new List<OnHitIntField>();
			for (int i = 0; i < m_prependIntFields.Count; i++)
			{
				list.Add(m_prependIntFields[i].GetCopy());
			}
			while (true)
			{
				for (int j = 0; j < _001D.Count; j++)
				{
					OnHitIntField onHitIntField = _001D[j];
					string identifier = onHitIntField.GetIdentifier();
					SingleOnHitIntFieldMod singleOnHitIntFieldMod = this._001D(identifier);
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
		}

		private SingleOnHitIntFieldMod _001D(string _001D)
		{
			if (string.IsNullOrEmpty(_001D))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return null;
					}
				}
			}
			for (int i = 0; i < m_overrides.Count; i++)
			{
				string text = m_overrides[i]._001D();
				if (text.Equals(_001D, StringComparison.OrdinalIgnoreCase))
				{
					return m_overrides[i].m_fieldOverride;
				}
			}
			while (true)
			{
				return null;
			}
		}
	}
}
