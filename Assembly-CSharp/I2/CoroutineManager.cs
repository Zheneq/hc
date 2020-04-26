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
				GameObject gameObject = new GameObject("_Coroutiner");
				gameObject.hideFlags |= HideFlags.HideAndDontSave;
				CoroutineManager._001D = gameObject.AddComponent<CoroutineManager>();
				if (Application.isPlaying)
				{
					Object.DontDestroyOnLoad(gameObject);
				}
			}
			return CoroutineManager._001D.StartCoroutine(_001D);
		}
	}
}
