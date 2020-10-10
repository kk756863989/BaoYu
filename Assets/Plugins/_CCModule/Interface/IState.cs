namespace CC
{
    public enum CharState
    {
        IDLE,
        MOVE,
        ATTACK,
        HURT,
        DIE,
    }

    public interface IState
    {
        IState OnStateEnter(params object[] args);
        IState OnStateStay(params object[] args);
        IState OnStateExit(params object[] args);
        IState SwitchState(params object[] args);
    }
}