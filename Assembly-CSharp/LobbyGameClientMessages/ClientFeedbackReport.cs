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

		public bool symbol_001D()
		{
			if (this.Reason != ClientFeedbackReport.FeedbackReason.symbol_0015)
			{
				if (this.Reason != ClientFeedbackReport.FeedbackReason.symbol_0016 && this.Reason != ClientFeedbackReport.FeedbackReason.symbol_0018)
				{
					if (this.Reason != ClientFeedbackReport.FeedbackReason.symbol_0019)
					{
						if (this.Reason != ClientFeedbackReport.FeedbackReason.symbol_0011)
						{
							return this.Reason == ClientFeedbackReport.FeedbackReason.symbol_001A;
						}
					}
				}
			}
			return true;
		}

		public enum FeedbackReason
		{
			symbol_001D,
			symbol_000E,
			symbol_0012,
			symbol_0015,
			symbol_0016,
			symbol_0013,
			symbol_0018,
			symbol_0009,
			symbol_0019,
			symbol_0011,
			symbol_001A,
			symbol_0004
		}
	}
}
