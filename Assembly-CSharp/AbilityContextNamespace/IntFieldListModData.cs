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

		public List<OnHitIntField> \u001D(List<OnHitIntField> \u001D)
		{
			List<OnHitIntField> list = new List<OnHitIntField>();
			for (int i = 0; i < this.m_prependIntFields.Count; i++)
			{
				list.Add(this.m_prependIntFields[i].GetCopy());
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IntFieldListModData.\u001D(List<OnHitIntField>)).MethodHandle;
			}
			for (int j = 0; j < \u001D.Count; j++)
			{
				OnHitIntField onHitIntField = \u001D[j];
				string identifier = onHitIntField.GetIdentifier();
				SingleOnHitIntFieldMod singleOnHitIntFieldMod = this.\u001D(identifier);
				if (singleOnHitIntFieldMod != null)
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
					OnHitIntField item = singleOnHitIntFieldMod.\u001D(onHitIntField);
					list.Add(item);
				}
				else
				{
					list.Add(onHitIntField.GetCopy());
				}
			}
			return list;
		}

		private SingleOnHitIntFieldMod \u001D(string \u001D)
		{
			if (string.IsNullOrEmpty(\u001D))
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(IntFieldListModData.\u001D(string)).MethodHandle;
				}
				return null;
			}
			for (int i = 0; i < this.m_overrides.Count; i++)
			{
				string text = this.m_overrides[i].\u001D();
				if (text.Equals(\u001D, StringComparison.OrdinalIgnoreCase))
				{
					return this.m_overrides[i].m_fieldOverride;
				}
			}
			for (;;)
			{
				switch (4)
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
