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
				if (Application.isPlaying)
				{
					this.Target.SendMessage(this.MethodName, Sender, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
