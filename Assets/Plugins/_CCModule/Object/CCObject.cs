namespace CC
{
    public abstract class CCObject : IReuseObject
    {
        public bool HasUsed { get; set; }
        public string EntityId { get; set; }

        public void Init()
        {
            if (!HasUsed)
            {
                OnFirstUse();
                HasUsed = true;
            }
            else
            {
                OnReuse();
            }
        }

        public abstract void OnFirstUse();
        public abstract void OnReuse();
        public abstract void OnRecycle();
        public abstract void OnRelease();
    }
}