namespace CC
{
    public interface IContact
    {
        IMove BeforeContact(params object[] args);
        IMove OnContactEnter(params object[] args);
        IMove OnContactStay(params object[] args);
        IMove OnContactExit(params object[] args);
        IMove AfterContact(params object[] args);
    }
}