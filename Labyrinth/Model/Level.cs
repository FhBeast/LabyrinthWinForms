﻿using Labyrinth.Util;

namespace Labyrinth.Model;

internal class Level
{
    public int Height { get; }
    public int Width { get; }
    public int CoinsNumber { get; }
    public int EnemySlowdown { get; }

    public Level(int height, int width, int coinsNumber, int enemySlowdown)
    {
        if (height < 1)
        {
            throw new ArgumentException($"{nameof(height)} is less than 1");
        }

        if (width < 1)
        {
            throw new ArgumentException($"{nameof(height)} is less than 1");
        }

        if (CoinsNumber < 1)
        {
            throw new ArgumentException($"{nameof(CoinsNumber)} is less than 1");
        }

        if (EnemySlowdown < 0)
        {
            throw new ArgumentException($"{nameof(EnemySlowdown)} is less than 0");
        }

        Height = height;
        Width = width;
        CoinsNumber = coinsNumber;
        EnemySlowdown = enemySlowdown;
    }

    public Field GenerateField()
    {
        Field field = new(Height, Width);

        FieldGenerator.GenerateDepthSearch(field);
        FieldGenerator.AddBraidng(field);

        return field;
    }

    public Player GeneratePlayer()
    {
        return GeneratePlayer(0, 0);
    }

    public Player GeneratePlayer(int x, int y)
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

        return new Player(x, y);
    }

    public Enemy GenerateEnemy()
    {
        return GenerateEnemy(0, 0);
    }

    public Enemy GenerateEnemy(int x, int y)
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

        return new Enemy(x, y);
    }
}
