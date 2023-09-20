

using FlaxEngine;

namespace Game
{
    public abstract class BaseState : Script
    {
        public StateMachine stateMachine;
        public Script targetScript;
        [ShowInEditor] protected float timeInState;


        public virtual void EnterState()
        {
            timeInState = 0;
        }

        public virtual void ExitState()
        {

        }

        public virtual void UpdateState()
        {
            timeInState += Time.DeltaTime;
        }

        public void ChangeState(int stateIndex)
            => stateMachine.ChangeState(stateIndex);
    }
}
