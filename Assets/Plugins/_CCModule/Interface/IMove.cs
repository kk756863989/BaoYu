namespace CC
{
    public interface IMove
    {
        IMove OnStartMove(params object[] args);
        IMove BeforeMove(params object[] args);
        IMove OnMoving(params object[] args);
        IMove AfterMove(params object[] args);
        IMove OnStopMove(params object[] args);
    }
}