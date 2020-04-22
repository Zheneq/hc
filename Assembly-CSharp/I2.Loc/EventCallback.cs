using System;
using UnityEngine;

namespace I2.Loc
{
	[Serializable]
	public class EventCallback
	{
		public MonoBehaviour Target;

		public string MethodName = string.Empty;

		public void Execute(UnityEngine.Object Sender = null)
		{
			if (!Target)
			{
				return;
			}
			while (true)
			{
				if (Application.isPlaying)
				{
					Target.SendMessage(MethodName, Sender, SendMessageOptions.DontRequireReceiver);
				}
				return;
			}
		}
	}
}
