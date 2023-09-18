
using FlaxEngine;

namespace Game
{
    public class CameraManager : Script
    {
        public Actor    pivot;
        public Player   player;
        public float    speed = 10;

        public override void OnStart()
        {
            player = Level.FindScript<Player>();
        }

        public override void OnUpdate()
        {
            if (player == null)
                return;

            pivot.Position = Vector3.Lerp(pivot.Position, player.Actor.Position, speed * Time.DeltaTime);
        }
    }
}
