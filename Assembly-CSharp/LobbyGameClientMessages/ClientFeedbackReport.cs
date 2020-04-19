using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ClientFeedbackReport : WebSocketMessage
	{
		public ClientFeedbackReport.FeedbackReason Reason;

		public string ReportedPlayerHandle;

		public long ReportedPlayerAccountId;

		public string Message;

		public bool \u001D()
		{
			if (this.Reason != ClientFeedbackReport.FeedbackReason.\u0015)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientFeedbackReport.\u001D()).MethodHandle;
				}
				if (this.Reason != ClientFeedbackReport.FeedbackReason.\u0016 && this.Reason != ClientFeedbackReport.FeedbackReason.\u0018)
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
					if (this.Reason != ClientFeedbackReport.FeedbackReason.\u0019)
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
						if (this.Reason != ClientFeedbackReport.FeedbackReason.\u0011)
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
							return this.Reason == ClientFeedbackReport.FeedbackReason.\u001A;
						}
					}
				}
			}
			return true;
		}

		public enum FeedbackReason
		{
			\u001D,
			\u000E,
			\u0012,
			\u0015,
			\u0016,
			\u0013,
			\u0018,
			\u0009,
			\u0019,
			\u0011,
			\u001A,
			\u0004
		}
	}
}
