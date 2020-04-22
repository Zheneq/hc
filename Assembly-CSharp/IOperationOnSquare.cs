public interface IOperationOnSquare
{
	void OperateOnSquare(BoardSquare square, ActorData actor, bool squareHasLos);

	bool ShouldEarlyOut();
}
