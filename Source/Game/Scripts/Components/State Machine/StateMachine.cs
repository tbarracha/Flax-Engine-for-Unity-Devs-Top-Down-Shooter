
using FlaxEngine;
using System.Collections.Generic;

namespace Game
{
    public class StateMachine : Script
    {
        public Script targetScript;
        public BaseState startState;
        public BaseState currentState;
        public List<BaseState> states;

        public void Initialize()
        {
            if (GetStates() == false)
                return;

            foreach (var state in states)
            {
                state.stateMachine = this;
                state.targetScript = targetScript;
            }

            if (startState != null)
                ChangeState(startState);
            else
                ChangeState(0);
        }

        public void UpdateState()
        {
            if (currentState == null)
                return;

            currentState.UpdateState();
        }

        public void ChangeState(BaseState targetState)
        {
            if (currentState != null || currentState == targetState)
                return;

            else
                currentState.ExitState();

            currentState = targetState;
            currentState.EnterState();
        }

        public void ChangeState(int stateIndex)
        {
            if (states == null || states.Count == 0)
                return;

            ChangeState(states[stateIndex]);
        }

        public bool GetStates()
        {
            if (Actor.ChildrenCount == 0)
                return false;

            states = new List<BaseState>();

            foreach (var child in Actor.Children)
            {
                var state = child.GetScript<BaseState>();
                if (state != null)
                    states.Add(state);
            }

            return states.Count > 0;
        }
    }
}
