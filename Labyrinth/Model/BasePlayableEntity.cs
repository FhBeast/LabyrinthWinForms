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
        PositionX = x;
        PositionY = y;
    }

    public abstract void Update(Field field);
}
