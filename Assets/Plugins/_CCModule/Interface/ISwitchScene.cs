namespace CC
{
    public interface ISwitchScene
    {
        void MarkAsPersistent(params object[] args);
        void OnSceneExit();
        void OnSceneEnter();
    }
}