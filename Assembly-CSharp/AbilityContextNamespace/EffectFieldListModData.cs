using System;
using System.Collections.Generic;

namespace AbilityContextNamespace
{
	[Serializable]
	public class EffectFieldListModData
	{
		public List<OnHitEffecField> m_prependEffectFields;

		public List<EffectFieldOverride> m_overrides;

		public List<OnHitEffecField> _001D(List<OnHitEffecField> _001D)
		{
			List<OnHitEffecField> list = new List<OnHitEffecField>();
			for (int i = 0; i < m_prependEffectFields.Count; i++)
			{
				list.Add(m_prependEffectFields[i].GetCopy());
			}
			for (int j = 0; j < _001D.Count; j++)
			{
				string identifier = _001D[j].GetIdentifier();
				OnHitEffecField onHitEffecField = this._001D(identifier);
				if (onHitEffecField != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					list.Add(onHitEffecField.GetCopy());
				}
				else
				{
					list.Add(_001D[j].GetCopy());
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				return list;
			}
		}

		private OnHitEffecField _001D(string _001D)
		{
			if (string.IsNullOrEmpty(_001D))
			{
				return null;
			}
			for (int i = 0; i < m_overrides.Count; i++)
			{
				string text = m_overrides[i]._001D();
				if (!text.Equals(_001D, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_overrides[i].m_effectOverride;
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				return null;
			}
		}
	}
}
