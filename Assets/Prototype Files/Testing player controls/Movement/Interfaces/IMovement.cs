namespace Game
{
    public interface IMovement
    {
        public void MoveHorizontal(float speed, bool @base);

        public void MoveVertical(float speed, bool @base);

        public void Move(float horizontalSpeed, float verticalSpeed, bool @base);
    }
}