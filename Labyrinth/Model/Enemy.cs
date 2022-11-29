﻿namespace Labyrinth.Model;

internal class Enemy
{
    public int PositionX { get; private set; }
    public int PositionY { get; private set; }
    public int Slowdown { get; private set; }
    private int TicksForMove { get; set; }

    public Enemy(int x, int y)
    {
        PositionX = x;
        PositionY = y;
        Slowdown = 0;
        TicksForMove = 0;
    }

    public void SetPosition(int x, int y)
    {
        PositionX = x;
        PositionY = y;
    }

    public void SetSlowdown(int slowdown)
    {
        if (slowdown < 0)
        {
            throw new ArgumentException(
                $"{nameof(slowdown)} is less than 0 or more than labyrinth width");
        }

        Slowdown = slowdown;
    }

    public void Update(Field field)
    {
        if (TicksForMove == 0)
        {
            TicksForMove = Slowdown;
        }
        else
        {
            TicksForMove--;
            return;
        }

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
