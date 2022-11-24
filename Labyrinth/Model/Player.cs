namespace Labyrinth.Model;

internal class Player
{
    public int PositionX { get; private set; }
    public int PositionY { get; private set; }

    public int PixelsToPoint { get; set; }

    public bool MoveUp { get; set; }
    public bool MoveDown { get; set; }
    public bool MoveLeft { get; set; }
    public bool MoveRight { get; set; }

    public Player(int x, int y)
    {
        PositionX = x;
        PositionY = y;
    }

    public void Update(Field field)
    {
        PixelsToPoint = 30;

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
        else 
        {
            PixelsToPoint = 0;
        }
    }

    public void UpdatePixelsToPoint()
    {
        if (PixelsToPoint > 0)
        {
            PixelsToPoint -= 1;
        }
    }
}
