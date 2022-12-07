namespace Labyrinth.Model;

internal class Player : BasePlayableEntity
{
    public bool MoveUp { get; set; }
    public bool MoveDown { get; set; }
    public bool MoveLeft { get; set; }
    public bool MoveRight { get; set; }

    public Player(int x, int y) : base(x, y) { }

    public void Update(Field field)
    {
        if (MoveUp &&
            field.GetCell(PositionX, PositionY).UpAvailable)
        {
            PositionY--;
        }
        else if (MoveDown &&
            field.GetCell(PositionX, PositionY).DownAvailable)
        {
            PositionY++;
        }
        else if (MoveLeft &&
            field.GetCell(PositionX, PositionY).LeftAvailable)
        {
            PositionX--;
        }
        else if (MoveRight &&
            field.GetCell(PositionX, PositionY).RightAvailable)
        {
            PositionX++;
        }
    }
}
