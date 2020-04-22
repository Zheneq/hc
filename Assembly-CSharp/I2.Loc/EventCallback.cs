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
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (Application.isPlaying)
				{
					Target.SendMessage(MethodName, Sender, SendMessageOptions.DontRequireReceiver);
				}
				return;
			}
		}
	}
}
