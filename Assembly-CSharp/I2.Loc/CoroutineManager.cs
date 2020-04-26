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
				if (mInstance == null)
				{
					GameObject gameObject = new GameObject("GoogleTranslation");
					gameObject.hideFlags |= HideFlags.HideAndDontSave;
					mInstance = gameObject.AddComponent<CoroutineManager>();
				}
				return mInstance;
			}
		}
	}
}
