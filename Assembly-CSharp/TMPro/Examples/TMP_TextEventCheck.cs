using System;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro.Examples
{
	public class TMP_TextEventCheck : MonoBehaviour
	{
		public TMP_TextEventHandler \u001D;

		private void \u000E()
		{
			if (this.\u001D != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextEventCheck.\u000E()).MethodHandle;
				}
				this.\u001D.onCharacterSelection.AddListener(new UnityAction<char, int>(this.\u000E));
				this.\u001D.onWordSelection.AddListener(new UnityAction<string, int, int>(this.\u000E));
				this.\u001D.onLineSelection.AddListener(new UnityAction<string, int, int>(this.\u0012));
				this.\u001D.onLinkSelection.AddListener(new UnityAction<string, string, int>(this.\u000E));
			}
		}

		private void \u0012()
		{
			if (this.\u001D != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextEventCheck.\u0012()).MethodHandle;
				}
				this.\u001D.onCharacterSelection.RemoveListener(new UnityAction<char, int>(this.\u000E));
				this.\u001D.onWordSelection.RemoveListener(new UnityAction<string, int, int>(this.\u000E));
				this.\u001D.onLineSelection.RemoveListener(new UnityAction<string, int, int>(this.\u0012));
				this.\u001D.onLinkSelection.RemoveListener(new UnityAction<string, string, int>(this.\u000E));
			}
		}

		private void \u000E(char \u001D, int \u000E)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Character [",
				\u001D,
				"] at Index: ",
				\u000E,
				" has been selected."
			}));
		}

		private void \u000E(string \u001D, int \u000E, int \u0012)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Word [",
				\u001D,
				"] with first character index of ",
				\u000E,
				" and length of ",
				\u0012,
				" has been selected."
			}));
		}

		private void \u0012(string \u001D, int \u000E, int \u0012)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Line [",
				\u001D,
				"] with first character index of ",
				\u000E,
				" and length of ",
				\u0012,
				" has been selected."
			}));
		}

		private void \u000E(string \u001D, string \u000E, int \u0012)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Link Index: ",
				\u0012,
				" with ID [",
				\u001D,
				"] and Text \"",
				\u000E,
				"\" has been selected."
			}));
		}
	}
}
