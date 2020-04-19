using System;
using System.Collections.Generic;

namespace AbilityContextNamespace
{
	[Serializable]
	public class EffectFieldListModData
	{
		public List<OnHitEffecField> m_prependEffectFields;

		public List<EffectFieldOverride> m_overrides;

		public List<OnHitEffecField> \u001D(List<OnHitEffecField> \u001D)
		{
			List<OnHitEffecField> list = new List<OnHitEffecField>();
			for (int i = 0; i < this.m_prependEffectFields.Count; i++)
			{
				list.Add(this.m_prependEffectFields[i].GetCopy());
			}
			for (int j = 0; j < \u001D.Count; j++)
			{
				string identifier = \u001D[j].GetIdentifier();
				OnHitEffecField onHitEffecField = this.\u001D(identifier);
				if (onHitEffecField != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(EffectFieldListModData.\u001D(List<OnHitEffecField>)).MethodHandle;
					}
					list.Add(onHitEffecField.GetCopy());
				}
				else
				{
					list.Add(\u001D[j].GetCopy());
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			return list;
		}

		private OnHitEffecField \u001D(string \u001D)
		{
			if (string.IsNullOrEmpty(\u001D))
			{
				return null;
			}
			for (int i = 0; i < this.m_overrides.Count; i++)
			{
				string text = this.m_overrides[i].\u001D();
				if (text.Equals(\u001D, StringComparison.OrdinalIgnoreCase))
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(EffectFieldListModData.\u001D(string)).MethodHandle;
					}
					return this.m_overrides[i].m_effectOverride;
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			return null;
		}
	}
}
