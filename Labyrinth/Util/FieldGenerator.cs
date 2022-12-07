using Labyrinth.Model;

namespace Labyrinth.Util;
internal static class FieldGenerator
{
    public static void GenerateBinaryTree(Field field)
    {
        if (field is null)
        {
            throw new ArgumentNullException($"{nameof(field)}");
        }

        int Height = field.Height;
        int Width = field.Width;

        Random boolRandom = new();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (y != 0 && x != Width - 1)
                {
                    if (boolRandom.Next(2) == 0)
                    {
                        field.EditCell(x, y, right: true);
                    }
                    else
                    {
                        field.EditCell(x, y, up: true);
                    }
                }
                else if (y == 0)
                {
                    field.EditCell(x, y, right: true);
                }
                else if (x == Width - 1)
                {
                    field.EditCell(x, y, up: true);
                }
            }
        }
    }

    public static void GenerateSidewinder(Field field)
    {
        if (field is null)
        {
            throw new ArgumentNullException($"{nameof(field)}");
        }

        int Height = field.Height;
        int Width = field.Width;

        Random boolRandom = new();
        Random setRandom = new();

        for (int y = 0; y < Height; y++)
        {
            var startSet = 0;
            var endSet = 0;
            for (int x = 0; x < Width; x++)
            {
                if (y != 0)
                {
                    if (boolRandom.Next(2) == 0 && x != Width - 1)
                    {
                        field.EditCell(x, y, right: true);
                    }
                    else
                    {
                        int pathUp = setRandom.Next(endSet - startSet + 1);
                        field.EditCell(x - pathUp, y, up: true);
                        startSet = x + 1;
                    }

                    endSet++;
                }
                else if (y == 0)
                {
                    field.EditCell(x, y, right: true);
                }
            }
        }
    }

    public static void GenerateOldosBorder(Field field)
    {
        if (field is null)
        {
            throw new ArgumentNullException($"{nameof(field)}");
        }

        int Height = field.Height;
        int Width = field.Width;

        Random randomPosition = new();

        bool[,] cellsInTree = new bool[Height, Width];
        int cellCount = Height * Width;
        int PositionX = randomPosition.Next(Width);
        int PositionY = randomPosition.Next(Height);

        while (cellCount > 0)
        {
            int direction = randomPosition.Next(4);

            if (direction == 0 && PositionY != 0)
            {
                PositionY--;

                if (!cellsInTree[PositionY, PositionX])
                {
                    cellsInTree[PositionY, PositionX] = true;
                    field.EditCell(PositionX, PositionY, down: true);
                    cellCount--;
                }
            }
            else if (direction == 1 && PositionY != Height - 1)
            {
                PositionY++;

                if (!cellsInTree[PositionY, PositionX])
                {
                    cellsInTree[PositionY, PositionX] = true;
                    field.EditCell(PositionX, PositionY, up: true);
                    cellCount--;
                }
            }
            else if (direction == 2 && PositionX != 0)
            {
                PositionX--;

                if (!cellsInTree[PositionY, PositionX])
                {
                    cellsInTree[PositionY, PositionX] = true;
                    field.EditCell(PositionX, PositionY, right: true);
                    cellCount--;
                }
            }
            else if (direction == 3 && PositionX != Width - 1)
            {
                PositionX++;

                if (!cellsInTree[PositionY, PositionX])
                {
                    cellsInTree[PositionY, PositionX] = true;
                    field.EditCell(PositionX, PositionY, left: true);
                    cellCount--;
                }
            }
        }
    }

    public static void GenerateDepthSearch(Field field)
    {
        if (field is null)
        {
            throw new ArgumentNullException($"{nameof(field)}");
        }

        int Height = field.Height;
        int Width = field.Width;

        Random randomCell = new();
        Random randomPosition = new();

        bool[,] cellsInTree = new bool[Height, Width];
        int cellCount = Height * Width;
        int PositionX = randomPosition.Next(Width);
        int PositionY = randomPosition.Next(Height);
        int newPositionX = 0;
        int newPositionY = 0;

        Stack<Point> cellsStack = new();

        while (cellCount > 0)
        {
            if (!cellsInTree[PositionY, PositionX])
            {
                cellsInTree[PositionY, PositionX] = true;
                cellCount--;
            }

            List<Point> neighbors = Field.GetUnvisitedNeighbors(cellsInTree, PositionX, PositionY);

            if (neighbors.Count > 0)
            {
                Point neighbor = neighbors[randomCell.Next(neighbors.Count)];
                newPositionX = neighbor.X;
                newPositionY = neighbor.Y;

                if (newPositionY < PositionY)
                {
                    field.EditCell(PositionX, PositionY, up: true);
                }
                else if (newPositionY > PositionY)
                {
                    field.EditCell(PositionX, PositionY, down: true);
                }
                else if (newPositionX < PositionX)
                {
                    field.EditCell(PositionX, PositionY, left: true);
                }
                else if (newPositionX > PositionX)
                {
                    field.EditCell(PositionX, PositionY, right: true);
                }

                if (neighbors.Count != 1)
                {
                    cellsStack.Push(new Point(PositionX, PositionY));
                }
            }
            else if (cellsStack.Count > 0)
            {
                Point cell = cellsStack.Pop();
                newPositionX = cell.X;
                newPositionY = cell.Y;
            }

            PositionX = newPositionX;
            PositionY = newPositionY;
        }
    }

    public static void AddBraidng(Field field)
    {
        if (field is null)
        {
            throw new ArgumentNullException($"{nameof(field)}");
        }

        int Height = field.Height;
        int Width = field.Width;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (field.GetCell(x, y).GetWaysNumber() < 2)
                {
                    if (field.GetCell(x, y).UpAvailable)
                    {
                        field.EditCell(x, y, down: true);
                    }
                    else if (field.GetCell(x, y).DownAvailable)
                    {
                        field.EditCell(x, y, up: true);
                    }
                    else if (field.GetCell(x, y).LeftAvailable)
                    {
                        field.EditCell(x, y, right: true);
                    }
                    else if (field.GetCell(x, y).RightAvailable)
                    {
                        field.EditCell(x, y, left: true);
                    }
                }
            }
        }
    }
}
