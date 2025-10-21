namespace Game.World
{
    public class WaypointToggler : WaypointHandler
    {
        protected override void ReachTarget()
        {
            objectMover.StopMovement();
            if (currentIndex == waypoints.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            SetWaypoint(currentIndex);
        }

        public void ToggleWaypoint()
        {
            MoveToNextWaypoint();
        }
    }
}