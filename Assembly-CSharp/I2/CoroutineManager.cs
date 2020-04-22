using System.Collections;
using UnityEngine;

namespace I2
{
	public class CoroutineManager : MonoBehaviour
	{
		private static CoroutineManager _001D;

		public static Coroutine _000E(IEnumerator _001D)
		{
			if (CoroutineManager._001D == null)
			{
				while (true)
				{
					switch (5)
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
				GameObject gameObject = new GameObject("_Coroutiner");
				gameObject.hideFlags |= HideFlags.HideAndDontSave;
				CoroutineManager._001D = gameObject.AddComponent<CoroutineManager>();
				if (Application.isPlaying)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					Object.DontDestroyOnLoad(gameObject);
				}
			}
			return CoroutineManager._001D.StartCoroutine(_001D);
		}
	}
}
