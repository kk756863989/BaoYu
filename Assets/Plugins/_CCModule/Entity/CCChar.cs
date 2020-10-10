namespace CC
{
    class CCChar : CCStaticObject, IMove, IState
    {
        public override void OnFirstUse()
        {
            MoveSystem.Add(this);
        }

        public override void OnReuse()
        {
            MoveSystem.Add(this);
        }

        public override void OnRecycle()
        {
            MoveSystem.Remove(this);
        }

        public override void OnRelease()
        {
            MoveSystem.Remove(this);
        }

        public IMove OnStartMove(params object[] args)
        {
            return this;
        }

        public IMove BeforeMove(params object[] args)
        {
            return this;
        }

        public IMove OnMoving(params object[] args)
        {
            return this;
        }

        public IMove AfterMove(params object[] args)
        {
            return this;
        }

        public IMove OnStopMove(params object[] args)
        {
            return this;
        }

        public IState OnStateEnter(params object[] args)
        {
            return this;
        }

        public IState OnStateStay(params object[] args)
        {
            return this;
        }

        public IState OnStateExit(params object[] args)
        {
            return this;
        }

        public IState SwitchState(params object[] args)
        {
            return this;
        }
    }
}