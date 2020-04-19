using System;
using System.Collections;
using UnityEngine;

namespace I2
{
	public class CoroutineManager : MonoBehaviour
	{
		private static CoroutineManager \u001D;

		public static Coroutine \u000E(IEnumerator \u001D)
		{
			if (CoroutineManager.\u001D == null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(CoroutineManager.\u000E(IEnumerator)).MethodHandle;
				}
				GameObject gameObject = new GameObject("_Coroutiner");
				gameObject.hideFlags |= HideFlags.HideAndDontSave;
				CoroutineManager.\u001D = gameObject.AddComponent<CoroutineManager>();
				if (Application.isPlaying)
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
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
			}
			return CoroutineManager.\u001D.StartCoroutine(\u001D);
		}
	}
}
