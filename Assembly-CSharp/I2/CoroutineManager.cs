using System;
using System.Collections;
using UnityEngine;

namespace I2
{
	public class CoroutineManager : MonoBehaviour
	{
		private static CoroutineManager symbol_001D;

		public static Coroutine symbol_000E(IEnumerator symbol_001D)
		{
			if (CoroutineManager.symbol_001D == null)
			{
				GameObject gameObject = new GameObject("_Coroutiner");
				gameObject.hideFlags |= HideFlags.HideAndDontSave;
				CoroutineManager.symbol_001D = gameObject.AddComponent<CoroutineManager>();
				if (Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
			}
			return CoroutineManager.symbol_001D.StartCoroutine(symbol_001D);
		}
	}
}
