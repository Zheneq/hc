﻿using System;
using UnityEngine;

namespace AbilityContextNamespace
{
	[Serializable]
	public class IntFieldOverride
	{
		[Header("-- Identifier string to match in base ability")]
		public string m_targetIdentifier = string.Empty;

		public SingleOnHitIntFieldMod m_fieldOverride;

		public string symbol_001D()
		{
			return this.m_targetIdentifier.Trim();
		}
	}
}
