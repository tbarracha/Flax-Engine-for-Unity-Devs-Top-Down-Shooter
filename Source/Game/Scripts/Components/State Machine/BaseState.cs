

using FlaxEngine;

namespace Game
{
    public abstract class BaseState : Script
    {
        public StateMachine stateMachine;
        public Script targetScript;
        [ShowInEditor] protected float timeInState;


        public void EnterState()
        {
            timeInState = 0;
        }

        public void ExitState()
        {

        }

        public void UpdateState()
        {
            timeInState += Time.DeltaTime;
        }
    }
}
