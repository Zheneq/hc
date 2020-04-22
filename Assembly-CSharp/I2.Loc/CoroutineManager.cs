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
					while (true)
					{
						switch (4)
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
					GameObject gameObject = new GameObject("GoogleTranslation");
					gameObject.hideFlags |= HideFlags.HideAndDontSave;
					mInstance = gameObject.AddComponent<CoroutineManager>();
				}
				return mInstance;
			}
		}
	}
}
