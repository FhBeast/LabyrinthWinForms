namespace Labyrinth.Model;

internal class Field
{
    public Cell[,] Cells { get; }
    public int Height { get; }
    public int Width { get; }
    public Point EndPosition { get; private set; }

    public int MaxStepsForEnd { get; private set; }

    public Field()
    {
        Cells = new Cell[0, 0];
    }

    public Field(int height, int width)
    {
        if (height < 1)
        {
            throw new ArgumentException($"{nameof(height)} is less than 1");
        }
        if (width < 1)
        {
            throw new ArgumentException($"{nameof(width)} is less than 1");
        }

        Width = width;
        Height = height;

        Cells = new Cell[height, width];

        MaxStepsForEnd = -1;

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Cell cell = new();

                Cells[h, w] = cell;
            }
        }

        SetEnd(0, 0);
    }

    public void SetEnd(int x, int y)
    {
        if (x < 0 || x >= Width)
        {
            throw new ArgumentException(
                $"{nameof(x)} is less than 0 or more than labyrinth width");
        }

        if (y < 0 || y >= Height)
        {
            throw new ArgumentException(
                $"{nameof(y)} is less than 0 or more than labyrinth height");
        }

        ClearWaves();
        EndPosition = new Point(x, y);
        GetCell(EndPosition.X, EndPosition.Y).StepsForEnd = 0;
    }

    public void EditCell(int x, int y,
        bool up = false,
        bool down = false,
        bool left = false,
        bool right = false)
    {
        if (x < 0 || x >= Width)
        {
            throw new ArgumentException(
                $"{nameof(x)} is less than 0 or more than labyrinth width");
        }

        if (y < 0 || y >= Height)
        {
            throw new ArgumentException(
                $"{nameof(y)} is less than 0 or more than labyrinth height");
        }

        if (up && y != 0)
        {
            GetCell(x, y).UpAvailable = !GetCell(x, y).UpAvailable;
            GetCell(x, y - 1).DownAvailable = !GetCell(x, y - 1).DownAvailable;
        }
        if (down && y != Height - 1)
        {
            GetCell(x, y).DownAvailable = !GetCell(x, y).DownAvailable;
            GetCell(x, y + 1).UpAvailable = !GetCell(x, y + 1).UpAvailable;
        }
        if (left && x != 0)
        {
            GetCell(x, y).LeftAvailable = !GetCell(x, y).LeftAvailable;
            GetCell(x - 1, y).RightAvailable = !GetCell(x - 1, y).RightAvailable;
        }
        if (right && x != Width - 1)
        {
            GetCell(x, y).RightAvailable = !GetCell(x, y).RightAvailable;
            GetCell(x + 1, y).LeftAvailable = !GetCell(x + 1, y).LeftAvailable;
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (x < 0 || x >= Width)
        {
            throw new ArgumentException(
                $"{nameof(x)} is less than 0 or more than labyrinth width");
        }

        if (y < 0 || y >= Height)
        {
            throw new ArgumentException(
                $"{nameof(y)} is less than 0 or more than labyrinth height");
        }

        return Cells[y, x];
    }

    public void GenerateBinaryTree()
    {
        Random boolRandom = new();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (y != 0 && x != Width - 1)
                {
                    if (boolRandom.Next(2) == 0)
                    {
                        EditCell(x, y, right: true);
                    }
                    else
                    {
                        EditCell(x, y, up: true);
                    }
                }
                else if (y == 0)
                {
                    EditCell(x, y, right: true);
                }
                else if (x == Width - 1)
                {
                    EditCell(x, y, up: true);
                }
            }
        }
    }

    public void GenerateSidewinder()
    {
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
                        EditCell(x, y, right: true);
                    }
                    else
                    {
                        int pathUp = setRandom.Next(endSet - startSet + 1);
                        EditCell(x - pathUp, y, up: true);
                        startSet = x + 1;
                    }

                    endSet++;
                }
                else if (y == 0)
                {
                    EditCell(x, y, right: true);
                }
            }
        }
    }

    public void GenerateOldosBorder()
    {
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
                    EditCell(PositionX, PositionY, down: true);
                    cellCount--;
                }
            }
            else if (direction == 1 && PositionY != Height - 1)
            {
                PositionY++;

                if (!cellsInTree[PositionY, PositionX])
                {
                    cellsInTree[PositionY, PositionX] = true;
                    EditCell(PositionX, PositionY, up: true);
                    cellCount--;
                }
            }
            else if (direction == 2 && PositionX != 0)
            {
                PositionX--;

                if (!cellsInTree[PositionY, PositionX])
                {
                    cellsInTree[PositionY, PositionX] = true;
                    EditCell(PositionX, PositionY, right: true);
                    cellCount--;
                }
            }
            else if (direction == 3 && PositionX != Width - 1)
            {
                PositionX++;

                if (!cellsInTree[PositionY, PositionX])
                {
                    cellsInTree[PositionY, PositionX] = true;
                    EditCell(PositionX, PositionY, left: true);
                    cellCount--;
                }
            }
        }
    }

    public void GenerateDepthSearch()
    {
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

            List<Point> neighbors = GetUnvisitedNeighbors(cellsInTree, PositionX, PositionY);

            if (neighbors.Count > 0)
            {
                Point neighbor = neighbors[randomCell.Next(neighbors.Count)];
                newPositionX = neighbor.X;
                newPositionY = neighbor.Y;

                if (newPositionY < PositionY)
                {
                    EditCell(PositionX, PositionY, up: true);
                }
                else if (newPositionY > PositionY)
                {
                    EditCell(PositionX, PositionY, down: true);
                }
                else if (newPositionX < PositionX)
                {
                    EditCell(PositionX, PositionY, left: true);
                }
                else if (newPositionX > PositionX)
                {
                    EditCell(PositionX, PositionY, right: true);
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

    private static List<Point> GetUnvisitedNeighbors(bool[,] array, int x, int y)
    {
        int height = array.GetLength(0);
        int width = array.GetLength(1);

        if (array is null)
        {
            throw new ArgumentNullException(
                $"{nameof(array)}");
        }

        if (x < 0 || x >= width)
        {
            throw new ArgumentException(
                $"{nameof(x)} is less than 0 or more than labyrinth width");
        }

        if (y < 0 || y >= height)
        {
            throw new ArgumentException(
                $"{nameof(y)} is less than 0 or more than labyrinth height");
        }

        List<Point> neighbors = new();

        if (y != 0 && !array[y - 1, x])
        {
            neighbors.Add(new Point(x, y - 1));
        }
        if (y != height - 1 && !array[y + 1, x])
        {
            neighbors.Add(new Point(x, y + 1));
        }
        if (x != 0 && !array[y, x - 1])
        {
            neighbors.Add(new Point(x - 1, y));
        }
        if (x != width - 1 && !array[y, x + 1])
        {
            neighbors.Add(new Point(x + 1, y));
        }

        return neighbors;
    }

    private List<Point> GetAvailableNeighbors(int x, int y)
    {
        if (x < 0 || x >= Width)
        {
            throw new ArgumentException(
                $"{nameof(x)} is less than 0 or more than labyrinth width");
        }

        if (y < 0 || y >= Height)
        {
            throw new ArgumentException(
                $"{nameof(y)} is less than 0 or more than labyrinth height");
        }

        List<Point> neighbors = new();

        if (GetCell(x, y).UpAvailable && y != 0)
        {
            neighbors.Add(new Point(x, y - 1));
        }
        if (GetCell(x, y).DownAvailable && y != Height - 1)
        {
            neighbors.Add(new Point(x, y + 1));
        }
        if (GetCell(x, y).LeftAvailable && x != 0)
        {
            neighbors.Add(new Point(x - 1, y));
        }
        if (GetCell(x, y).RightAvailable && x != Width - 1)
        {
            neighbors.Add(new Point(x + 1, y));
        }

        return neighbors;
    }

    public void AddBraidng()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (GetCell(x, y).GetWaysNumber() < 2)
                {
                    if (GetCell(x, y).UpAvailable)
                    {
                        EditCell(x, y, down: true);
                    }
                    else if (GetCell(x, y).DownAvailable)
                    {
                        EditCell(x, y, up: true);
                    }
                    else if (GetCell(x, y).LeftAvailable)
                    {
                        EditCell(x, y, right: true);
                    }
                    else if (GetCell(x, y).RightAvailable)
                    {
                        EditCell(x, y, left: true);
                    }
                }
            }
        }
    }

    public void WaveTracing()
    {
        bool isSolved = false;

        while (!isSolved)
        {
            isSolved = RunOneStepWaveTracing();
        }
    }

    public void WaveTracingFast()
    {
        bool isSolved = false;
        List<Point> waves = new()
        {
            EndPosition
        };

        while (!isSolved)
        {
            isSolved = RunOneStepWaveTracingFast(waves);
        }
    }

    public bool RunOneStepWaveTracingFast(List<Point> waves)
    {
        if (waves is null)
        {
            throw new ArgumentNullException(
                $"{nameof(waves)}");
        }

        bool isSolved = true;

        List<Point> wavesNew = new();

        foreach (var wave in waves)
        {
            List<Point> neighbors = GetAvailableNeighbors(wave.X, wave.Y);
            Cell currentCell = GetCell(wave.X, wave.Y);

            foreach (var neighbor in neighbors)
            {
                Cell neighborCell = GetCell(neighbor.X, neighbor.Y);

                if (CompareToUpdate(currentCell, neighborCell))
                {
                    isSolved = false;
                    wavesNew.Add(neighbor);
                    neighborCell.StepsForEnd = currentCell.StepsForEnd + 1;
                    if (neighborCell.StepsForEnd > MaxStepsForEnd)
                    {
                        MaxStepsForEnd = neighborCell.StepsForEnd;
                    }
                }
            }
        }

        waves.Clear();
        waves.AddRange(wavesNew);
        return isSolved;
    }

    public bool RunOneStepWaveTracing()
    {
        bool isSolved = true;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                List<Point> neighbors = GetAvailableNeighbors(x, y);

                Cell currentCell = GetCell(x, y);

                foreach (var neighbor in neighbors)
                {
                    Cell neighborCell = GetCell(neighbor.X, neighbor.Y);

                    if (CompareToUpdate(currentCell, neighborCell))
                    {
                        isSolved = false;
                        neighborCell.StepsForEnd = currentCell.StepsForEnd + 1;
                        if (neighborCell.StepsForEnd > MaxStepsForEnd)
                        {
                            MaxStepsForEnd = neighborCell.StepsForEnd;
                        }
                    }
                }
            }
        }

        return isSolved;
    }

    private bool CompareToUpdate(Cell original, Cell updated)
    {
        if (original is null)
        {
            throw new ArgumentNullException(
                $"{nameof(original)}");
        }

        if (updated is null)
        {
            throw new ArgumentNullException(
                $"{nameof(original)}");
        }

        int originalSteps = original.StepsForEnd;
        int updatedSteps = updated.StepsForEnd;

        return original.IsFound() &&
            updatedSteps - originalSteps > 1;
    }

    private void ClearWaves()
    {
        MaxStepsForEnd = -1;
        foreach (var cell in Cells)
        {
            cell.StepsForEnd = int.MaxValue;
        }
    }
}