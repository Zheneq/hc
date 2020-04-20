using System;
using System.Collections.Generic;

namespace AbilityContextNamespace
{
	[Serializable]
	public class EffectFieldListModData
	{
		public List<OnHitEffecField> m_prependEffectFields;

		public List<EffectFieldOverride> m_overrides;

		public List<OnHitEffecField> symbol_001D(List<OnHitEffecField> symbol_001D)
		{
			List<OnHitEffecField> list = new List<OnHitEffecField>();
			for (int i = 0; i < this.m_prependEffectFields.Count; i++)
			{
				list.Add(this.m_prependEffectFields[i].GetCopy());
			}
			for (int j = 0; j < symbol_001D.Count; j++)
			{
				string identifier = symbol_001D[j].GetIdentifier();
				OnHitEffecField onHitEffecField = this.symbol_001D(identifier);
				if (onHitEffecField != null)
				{
					list.Add(onHitEffecField.GetCopy());
				}
				else
				{
					list.Add(symbol_001D[j].GetCopy());
				}
			}
			return list;
		}

		private OnHitEffecField symbol_001D(string symbol_001D)
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
					return this.m_overrides[i].m_effectOverride;
				}
			}
			return null;
		}
	}
}
