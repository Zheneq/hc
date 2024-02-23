using System.Text;
using UnityEngine;

namespace TMPro.Examples
{
	public class TMP_TextEventCheck : MonoBehaviour
	{
		public TMP_TextEventHandler _001D;

		private void _000E()
		{
			if (!(_001D != null))
			{
				return;
			}
			while (true)
			{
				_001D.onCharacterSelection.AddListener(_000E);
				_001D.onWordSelection.AddListener(_000E);
				_001D.onLineSelection.AddListener(_0012);
				_001D.onLinkSelection.AddListener(_000E);
				return;
			}
		}

		private void _0012()
		{
			if (!(_001D != null))
			{
				return;
			}
			while (true)
			{
				_001D.onCharacterSelection.RemoveListener(_000E);
				_001D.onWordSelection.RemoveListener(_000E);
				_001D.onLineSelection.RemoveListener(_0012);
				_001D.onLinkSelection.RemoveListener(_000E);
				return;
			}
		}

		private void _000E(char _001D, int _000E)
		{
			Debug.Log(new StringBuilder().Append("Character [").Append(_001D).Append("] at Index: ").Append(_000E).Append(" has been selected.").ToString());
		}

		private void _000E(string _001D, int _000E, int _0012)
		{
			Debug.Log(new StringBuilder().Append("Word [").Append(_001D).Append("] with first character index of ").Append(_000E).Append(" and length of ").Append(_0012).Append(" has been selected.").ToString());
		}

		private void _0012(string _001D, int _000E, int _0012)
		{
			Debug.Log(new StringBuilder().Append("Line [").Append(_001D).Append("] with first character index of ").Append(_000E).Append(" and length of ").Append(_0012).Append(" has been selected.").ToString());
		}

		private void _000E(string _001D, string _000E, int _0012)
		{
			Debug.Log(new StringBuilder().Append("Link Index: ").Append(_0012).Append(" with ID [").Append(_001D).Append("] and Text \"").Append(_000E).Append("\" has been selected.").ToString());
		}
	}
}
