using System;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public abstract class TMP_InputValidator : ScriptableObject
	{
		public abstract char Validate(ref string text, ref int pos, char ch);
	}
}
