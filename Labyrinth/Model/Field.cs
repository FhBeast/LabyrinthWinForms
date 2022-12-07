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

    public static List<Point> GetUnvisitedNeighbors(bool[,] array, int x, int y)
    {
        if (array is null)
        {
            throw new ArgumentNullException(
                $"{nameof(array)}");
        }

        int height = array.GetLength(0);
        int width = array.GetLength(1);

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

    public List<Point> GetAvailableNeighbors(int x, int y)
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
            throw new ArgumentNullException($"{nameof(waves)}");
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

    private static bool CompareToUpdate(Cell original, Cell updated)
    {
        if (original is null)
        {
            throw new ArgumentNullException($"{nameof(original)}");
        }

        if (updated is null)
        {
            throw new ArgumentNullException( $"{nameof(original)}");
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