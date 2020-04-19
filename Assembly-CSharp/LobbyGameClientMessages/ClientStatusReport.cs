using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ClientStatusReport : WebSocketMessage
	{
		public ClientStatusReport.ClientStatusReportType Status;

		public string StatusDetails;

		public string DeviceIdentifier;

		public string UserMessage;

		public string FileDateTime;

		public enum ClientStatusReportType
		{
			\u001D,
			\u000E,
			\u0012,
			\u0015,
			\u0016
		}
	}
}
