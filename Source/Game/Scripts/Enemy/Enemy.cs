
using FlaxEngine;

namespace Game
{
    public class Enemy : Agent
    {
        // idle, moving, attackings
        [Header("Enemy")]
        public EnemyState       enemyState;
        public Player           player;
        public NavmeshAgent     navAgent;
        public float attackDistance = 7; // in centimeters

        float AttackDistance => attackDistance * 100;

        public override void OnStart()
        {
            base.OnStart();

            player = Actor.Scene.FindScript<Player>();

            navAgent.SetActorToMove(Actor);
            navAgent.speed      = speed;
            navAgent.lookSpeed  = lookSpeed;

            ChangeEnemyState(EnemyState.Movement);
        }

        public override void OnUpdate()
        {
            HandleStates();
        }

        public void HandleStates()
        {
            switch (enemyState)
            {
                case EnemyState.Idle:
                    EnemyState_Idle();
                    break;

                case EnemyState.Movement:
                    EnemyState_Movement();
                    break;

                case EnemyState.Attacking:
                    EnemyState_Attacking();
                    break;
            }
        }

        void EnemyState_Idle()
        {

        }

        void EnemyState_Movement()
        {
            // get close to the player
            // if close enough, attack

            if (player == null)
                player = Actor.Scene.FindScript<Player>();

            navAgent.SetDestination(player.Actor.Position);
            navAgent.MoveAlongPath();

            if (navAgent.RemainingDistance < AttackDistance)
                ChangeEnemyState(EnemyState.Attacking);
        }

        void EnemyState_Attacking()
        {
            // look at the player
            // spawn projectiles

            Vector3 direction = player.Actor.Position - Actor.Position;
            Actor.Orientation = Quaternion.Slerp(Actor.Orientation, Quaternion.LookRotation(direction), lookSpeed * Time.DeltaTime);

            shootComponent.HandleShooting(true);

            float distance = Vector3.Distance(player.Actor.Position, Actor.Position);
            if (distance > AttackDistance)
                ChangeEnemyState(EnemyState.Movement);
        }

        void ChangeEnemyState(EnemyState targetState)
        {
            if (enemyState == targetState)
                return;

            enemyState = targetState;
        }

        protected override void Death()
        {
            Debug.Log(Actor.Name + ", has died!");
            Destroy(Actor);
        }

        public override void OnDebugDraw()
        {
            if (enemyState != EnemyState.Idle)
            {
                if (enemyState == EnemyState.Movement)
                    Gizmos.DrawWireSphere(Actor.Position, AttackDistance, Color.Red);

                if (enemyState == EnemyState.Attacking)
                    Gizmos.DrawWireSphere(Actor.Position, AttackDistance, Color.Green);
            }
        }
    }
}
