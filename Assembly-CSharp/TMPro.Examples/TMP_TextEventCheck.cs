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
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
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
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				_001D.onCharacterSelection.RemoveListener(_000E);
				_001D.onWordSelection.RemoveListener(_000E);
				_001D.onLineSelection.RemoveListener(_0012);
				_001D.onLinkSelection.RemoveListener(_000E);
				return;
			}
		}

		private void _000E(char _001D, int _000E)
		{
			Debug.Log("Character [" + _001D + "] at Index: " + _000E + " has been selected.");
		}

		private void _000E(string _001D, int _000E, int _0012)
		{
			Debug.Log("Word [" + _001D + "] with first character index of " + _000E + " and length of " + _0012 + " has been selected.");
		}

		private void _0012(string _001D, int _000E, int _0012)
		{
			Debug.Log("Line [" + _001D + "] with first character index of " + _000E + " and length of " + _0012 + " has been selected.");
		}

		private void _000E(string _001D, string _000E, int _0012)
		{
			Debug.Log("Link Index: " + _0012 + " with ID [" + _001D + "] and Text \"" + _000E + "\" has been selected.");
		}
	}
}
