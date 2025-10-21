namespace Game.World
{
    public class WaypointCycler : WaypointHandler
    {
        protected override void SetWaypoint(int index)
        {
            base.SetWaypoint(index);
            MoveToNextWaypoint();
        }

        protected override void ReachTarget()
        {
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
    }
}