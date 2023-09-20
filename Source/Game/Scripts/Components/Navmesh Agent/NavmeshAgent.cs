
using FlaxEngine;

namespace Game
{
    /// <summary>
    /// NavmeshAgent is a script/component that allows to set the destination and calculation of a path of an AI agent inside a Navmesh
    /// </summary>
    public class NavmeshAgent : Script
    {
        [Header("Movement")]
        [Serialize, ShowInEditor] Actor actorToMove;
        public NavmeshMoveType          moveType;
        public float speed              = 5;
        public float lookSpeed          = 5;
        public float stoppingDistance   = 5;
        public bool updateRotation      = true;
        public Vector3 offset           = new Vector3(0, 0, 0);

        private Vector3[] path;
        private float pathDistance;     // total path distance
        private float pathProgress;     // progressed distance along total path distance
        private float pathPercent;      // percentage along the path

        [Header("View Only")]
        [ShowInEditor]
        private Vector3 velocity;
        private Vector3 previousPosition;

        [ShowInEditor]
        private bool isStopped;

        RigidBody rigidbody;
        CharacterController controller;

#if FLAX_EDITOR
        [Header("Debug")]
        public bool showPath;
        public Color pathColor          = Color.BlueViolet;
        public float pathRadius         = 50;
#endif

        public float Speed              => speed * 100;
        public float StoppingDistance   => stoppingDistance * 100;

        public Vector3 PathEndPosition
        {
            get
            {
                if (path != null)
                    return path[path.Length - 1];
                else
                    return actorToMove.Position;
            }
        }

        public Vector3[] Path           => path;
        public float PathDistance       => pathDistance;
        public float RemainingDistance  => pathDistance - pathProgress;
        public float PathPercent        => pathPercent;
        public Vector3 Velocity         => velocity;

        public override void OnStart()
        {
            if (actorToMove == null)
                SetActorToMove(Actor);

            previousPosition = actorToMove.Position;
        }

        public void SetActorToMove(Actor targetActor)
        {
            actorToMove = targetActor;
            rigidbody   = targetActor.FindActor<RigidBody>() != null            ? targetActor.FindActor<RigidBody>()            : null;
            controller  = targetActor.FindActor<CharacterController>() != null  ? targetActor.FindActor<CharacterController>()  : null;
        }

        /// <summary>
        /// Attempts to calculate a path, returning true if one is found and false if none was found
        /// </summary>
        public bool SetDestination(Vector3 position, bool resumeAgent = true)
        {
            Vector3 currentPosition = Actor.Position;
            previousPosition = currentPosition;
            pathPercent = 0;
            pathDistance = 0;

            if (Navigation.FindPath(currentPosition, position, out path) == false)
            {
                Debug.LogWarning("Failed to find path to the target.");
                return false;
            }

            // Move the start/end points to navmesh floor
            if (path.Length != 0)
                Navigation.ProjectPoint(path[0], out path[0]);
            if (path.Length > 1)
                Navigation.ProjectPoint(PathEndPosition, out path[path.Length - 1]);

            // Compute path length
            for (int i = 1; i < path.Length; i++)
                pathDistance += Vector3.Distance(ref path[i - 1], ref path[i]);

            if (resumeAgent)
                ResumeAgent();

            return true;
        }

        /// <summary>
        /// Moves Actor along path.
        /// <para> Returns true if agent is moving, and false if there is no path or if the agent has reached its destination </para> 
        /// </summary>
        public bool MoveAlongPath()
        {
            // Skip if has no path
            if (isStopped == true || path == null)
                return false;

            Vector3 currentPos  = Actor.Position;
            Vector3 targetPos   = PathEndPosition;

            // Check if reached target location
            if (Vector3.Distance(currentPos, targetPos) < StoppingDistance)
                return false;

            // Move
            pathProgress    = Mathf.Min(pathDistance * pathPercent + Time.DeltaTime * Speed, pathDistance);
            pathPercent     = Mathf.Min(pathProgress / pathDistance, 1);

            // Calculate position on path
            float segmentsSum = 0;
            for (int i = 0; i < path.Length - 1; i++)
            {
                var segmentLength = Vector3.Distance(ref path[i], ref path[i + 1]);
                if (segmentsSum <= pathProgress && segmentsSum + segmentLength >= pathProgress)
                {
                    float t = (pathProgress - segmentsSum) / segmentLength;
                    targetPos = Vector3.Lerp(path[i], path[i + 1], t) + offset;
                    HandleMovementType(targetPos - Actor.Position);                    
                    break;
                }

                segmentsSum += segmentLength;
            }

            CalculateVelocity();

            if (updateRotation)
            {
                Vector3 lookDirection = velocity;
                lookDirection.Y = 0;

                actorToMove.Orientation = Quaternion.Slerp(actorToMove.Orientation, Quaternion.LookRotation(lookDirection), lookSpeed * Time.DeltaTime);
            }
            
            return true;
        }

        public void StopAgent()
        {
            if (isStopped == true)
                return;

            if (moveType == NavmeshMoveType.RigidbodyVelocity)
                

            isStopped = true;
        }

        public void ResumeAgent()
        {
            if (isStopped == false)
                return;

            isStopped = false;
        }

        public void ClearPath()
        {
            StopAgent();
            path = null;
        }

        void HandleMovementType(Vector3 direction)
        {
            if (moveType == NavmeshMoveType.AddMovement)
                actorToMove.AddMovement(direction);

            else if (moveType == NavmeshMoveType.CharacterController)
                controller.Move(direction);

            else if (moveType == NavmeshMoveType.RigidbodyVelocity)
            {
                actorToMove.AddMovement(direction);
                rigidbody.LinearVelocity = direction;
            }
        }

        private Vector3 CalculateVelocity()
        {
            if (isStopped)
            {
                velocity = Vector3.Zero;
                return velocity;
            }

            Vector3 currentPosition = Actor.Position;
            Vector3 positionChange = currentPosition - previousPosition;
            velocity = positionChange / Time.DeltaTime;

            previousPosition = currentPosition;

            return velocity;
        }

#if FLAX_EDITOR
        public override void OnDebugDraw()
        {
            if (showPath && path != null && path.Length > 0)
            {
                for (int i = 0; i < path.Length; i++)
                {
                    var sphere = new BoundingSphere(path[i], pathRadius);
                    DebugDraw.DrawSphere(sphere, pathColor);
                }
            }
        }
#endif
    }
}
