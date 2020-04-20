using System;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro.Examples
{
	public class TMP_TextEventCheck : MonoBehaviour
	{
		public TMP_TextEventHandler symbol_001D;

		private void symbol_000E()
		{
			if (this.symbol_001D != null)
			{
				this.symbol_001D.onCharacterSelection.AddListener(new UnityAction<char, int>(this.symbol_000E));
				this.symbol_001D.onWordSelection.AddListener(new UnityAction<string, int, int>(this.symbol_000E));
				this.symbol_001D.onLineSelection.AddListener(new UnityAction<string, int, int>(this.symbol_0012));
				this.symbol_001D.onLinkSelection.AddListener(new UnityAction<string, string, int>(this.symbol_000E));
			}
		}

		private void symbol_0012()
		{
			if (this.symbol_001D != null)
			{
				this.symbol_001D.onCharacterSelection.RemoveListener(new UnityAction<char, int>(this.symbol_000E));
				this.symbol_001D.onWordSelection.RemoveListener(new UnityAction<string, int, int>(this.symbol_000E));
				this.symbol_001D.onLineSelection.RemoveListener(new UnityAction<string, int, int>(this.symbol_0012));
				this.symbol_001D.onLinkSelection.RemoveListener(new UnityAction<string, string, int>(this.symbol_000E));
			}
		}

		private void symbol_000E(char symbol_001D, int symbol_000E)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Character [",
				symbol_001D,
				"] at Index: ",
				symbol_000E,
				" has been selected."
			}));
		}

		private void symbol_000E(string symbol_001D, int symbol_000E, int symbol_0012)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Word [",
				symbol_001D,
				"] with first character index of ",
				symbol_000E,
				" and length of ",
				symbol_0012,
				" has been selected."
			}));
		}

		private void symbol_0012(string symbol_001D, int symbol_000E, int symbol_0012)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Line [",
				symbol_001D,
				"] with first character index of ",
				symbol_000E,
				" and length of ",
				symbol_0012,
				" has been selected."
			}));
		}

		private void symbol_000E(string symbol_001D, string symbol_000E, int symbol_0012)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Link Index: ",
				symbol_0012,
				" with ID [",
				symbol_001D,
				"] and Text \"",
				symbol_000E,
				"\" has been selected."
			}));
		}
	}
}
