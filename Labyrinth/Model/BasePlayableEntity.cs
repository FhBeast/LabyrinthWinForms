namespace Labyrinth.Model;

internal abstract class BasePlayableEntity
{
    public int PositionX { get; protected set; }
    public int PositionY { get; protected set; }

    protected BasePlayableEntity(int positionX, int positionY)    {
        PositionX = positionX;
        PositionY = positionY;
    }
    public void SetPosition(int x, int y)
    {
        if (x < 0)
        {
            throw new ArgumentException($"{nameof(x)} is less than 0");
        }
        if (y < 0)
        {
            throw new ArgumentException($"{nameof(y)} is less than 0");
        }

        PositionX = x;
        PositionY = y;
    }
}
