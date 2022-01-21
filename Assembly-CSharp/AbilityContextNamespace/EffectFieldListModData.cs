using System;
using System.Collections.Generic;

namespace AbilityContextNamespace
{
	[Serializable]
	public class EffectFieldListModData
	{
		public List<OnHitEffecField> m_prependEffectFields;
		public List<EffectFieldOverride> m_overrides;

		public List<OnHitEffecField> GetModdedEffectFieldList(List<OnHitEffecField> input)
		{
			List<OnHitEffecField> list = new List<OnHitEffecField>();
			for (int i = 0; i < m_prependEffectFields.Count; i++)
			{
				list.Add(m_prependEffectFields[i].GetCopy());
			}
			for (int j = 0; j < input.Count; j++)
			{
				string identifier = input[j].GetIdentifier();
				OnHitEffecField onHitEffecField = this.GetOverrideEntry(identifier);
				if (onHitEffecField != null)
				{
					list.Add(onHitEffecField.GetCopy());
				}
				else
				{
					list.Add(input[j].GetCopy());
				}
			}
			return list;
		}

		private OnHitEffecField GetOverrideEntry(string identifier)
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
					return m_overrides[i].m_effectOverride;
				}
			}
			return null;
		}
	}
}
