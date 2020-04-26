using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ClientFeedbackReport : WebSocketMessage
	{
		public enum FeedbackReason
		{
			_001D,
			_000E,
			_0012,
			_0015,
			_0016,
			_0013,
			_0018,
			_0009,
			_0019,
			_0011,
			_001A,
			_0004
		}

		public FeedbackReason Reason;

		public string ReportedPlayerHandle;

		public long ReportedPlayerAccountId;

		public string Message;

		public bool _001D()
		{
			int result;
			if (Reason != FeedbackReason._0015)
			{
				if (Reason != FeedbackReason._0016 && Reason != FeedbackReason._0018)
				{
					if (Reason != FeedbackReason._0019)
					{
						if (Reason != FeedbackReason._0011)
						{
							result = ((Reason == FeedbackReason._001A) ? 1 : 0);
							goto IL_006c;
						}
					}
				}
			}
			result = 1;
			goto IL_006c;
			IL_006c:
			return (byte)result != 0;
		}
	}
}
