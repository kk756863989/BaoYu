namespace CC
{
    public interface IReuseObject
    {
        bool HasUsed { get; set; }
        string EntityId { get; set; }
        void Init();
        void OnFirstUse();
        void OnReuse();
        void OnRecycle();
        void OnRelease();
    }
}