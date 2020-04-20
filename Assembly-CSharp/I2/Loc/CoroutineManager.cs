using System;
using UnityEngine;

namespace I2.Loc
{
	public class CoroutineManager : MonoBehaviour
	{
		private static CoroutineManager mInstance;

		public static CoroutineManager pInstance
		{
			get
			{
				if (CoroutineManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("GoogleTranslation");
					gameObject.hideFlags |= HideFlags.HideAndDontSave;
					CoroutineManager.mInstance = gameObject.AddComponent<CoroutineManager>();
				}
				return CoroutineManager.mInstance;
			}
		}
	}
}
