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
						RuntimeMethodHandle runtimeMethodHandle = methodof(CoroutineManager.get_pInstance()).MethodHandle;
					}
					GameObject gameObject = new GameObject("GoogleTranslation");
					gameObject.hideFlags |= HideFlags.HideAndDontSave;
					CoroutineManager.mInstance = gameObject.AddComponent<CoroutineManager>();
				}
				return CoroutineManager.mInstance;
			}
		}
	}
}
