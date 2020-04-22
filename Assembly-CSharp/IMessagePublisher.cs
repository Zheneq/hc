public interface IMessagePublisher
{
	void Subscribe<TMessageType>(WebSocket subscriber);

	void Unsubscribe<TMessageType>(WebSocket subscriber);

	void Unsubscribe(WebSocket subscriber);

	void Publish<TMessageType>(TMessageType data);
}
