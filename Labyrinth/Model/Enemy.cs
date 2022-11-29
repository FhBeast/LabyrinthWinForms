namespace Labyrinth.Model;

internal class Enemy
{
    public int PositionX { get; private set; }
    public int PositionY { get; private set; }

    public Enemy(int x, int y)
    {
        PositionX = x;
        PositionY = y;
    }

    public void SetPosition(int x, int y)
    {
        PositionX = x;
        PositionY = y;
    }

    public void Update(Field field)
    {
        List<Point> cells = field.GetAvailableNeighbors(PositionX, PositionY);

        foreach (var cell in cells)
        {
            int playerSteps = field.GetCell(PositionX, PositionY).StepsForEnd;
            int cellSteps = field.GetCell(cell.X, cell.Y).StepsForEnd;

            if (playerSteps > cellSteps)
            {
                SetPosition(cell.X, cell.Y);
                break;
            }
        }
    }
}
