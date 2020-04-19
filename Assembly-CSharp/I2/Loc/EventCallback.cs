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
			if (this.Target)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(EventCallback.Execute(UnityEngine.Object)).MethodHandle;
				}
				if (Application.isPlaying)
				{
					this.Target.SendMessage(this.MethodName, Sender, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
