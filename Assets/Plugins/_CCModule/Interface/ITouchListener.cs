namespace CC
{
    public interface ITouchListener
    {
        void RegisterTouchEvents();
        void UnregisterTouchEvents();
        void OnTouchStart(params object[] args);
        void OnTouchMove(params object[] args);
        void OnTouchCancel(params object[] args);
        void OnTouchEnd(params object[] args);
    }
}