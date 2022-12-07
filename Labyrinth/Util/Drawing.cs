using Labyrinth.Model;

namespace Labyrinth.Util;

internal static class Drawing
{
    public static int CellSize { get; } = 30;
    public static int StartPointX { get; } = 10;
    public static int StartPointY { get; } = 10;
    public static int WallThickness { get; } = 1;
    public static Color PlayerColor { get; } = Color.FromArgb(100, 100, 255);
    public static Color EnemyColor { get; } = Color.FromArgb(255, 100, 100);
    public static Color CoinColor { get; } = Color.FromArgb(200, 0, 255);

    private static Brush playerBrush = new SolidBrush(PlayerColor);
    private static Brush enemyBrush = new SolidBrush(EnemyColor);
    private static Brush coinBrush = new SolidBrush(CoinColor);
    public static int CoinDiameter { get; } = 20;

    public static void DrawLabytinth(Bitmap bitmap, Field field)
    {
        var formGraphics = Graphics.FromImage(bitmap);
        Pen pen = new(Color.Black, WallThickness);

        var halfCellSize = CellSize / 2;

        var currentPositionX = StartPointX + halfCellSize;
        int currentPositionY;

        int up, down, left, right;

        for (int x = 0; x < field.Width; x++)
        {
            currentPositionY = StartPointY + halfCellSize;
            left = currentPositionX - halfCellSize;
            right = currentPositionX + halfCellSize;

            for (int y = 0; y < field.Height; y++)
            {
                up = currentPositionY - halfCellSize;
                down = currentPositionY + halfCellSize;

                if (!field.GetCell(x, y).UpAvailable)
                {
                    formGraphics.DrawLine(pen, left, up, right, up);
                }
                if (!field.GetCell(x, y).DownAvailable)
                {
                    formGraphics.DrawLine(pen, left, down, right, down);
                }
                if (!field.GetCell(x, y).LeftAvailable)
                {
                    formGraphics.DrawLine(pen, left, up, left, down);
                }
                if (!field.GetCell(x, y).RightAvailable)
                {
                    formGraphics.DrawLine(pen, right, up, right, down);
                }

                currentPositionY += CellSize;
            }
            currentPositionX += CellSize;
        }

        formGraphics.Dispose();
        pen.Dispose();
    }

    public static void DrawWaves(Bitmap bitmap, Field field)
    {
        var formGraphics = Graphics.FromImage(bitmap);

        var currentPositionX = StartPointX;
        int currentPositionY;
        double smoothness = 255.0 / field.MaxStepsForEnd;

        for (int x = 0; x < field.Width; x++)
        {
            currentPositionY = StartPointY;

            for (int y = 0; y < field.Height; y++)
            {
                Cell cell = field.GetCell(x, y);
                int steps = cell.StepsForEnd;

                if (cell.IsFound())
                {
                    int greenColor = Convert.ToInt32(smoothness * steps);
                    greenColor = greenColor > 255 ? 255 : greenColor;
                    int redBlueColor = 255 - greenColor;

                    formGraphics.FillRectangle(new SolidBrush(Color.FromArgb(
                        redBlueColor, 255, redBlueColor)),
                        new Rectangle(currentPositionX, currentPositionY, CellSize, CellSize));
                }
                else
                {
                    formGraphics.FillRectangle(new SolidBrush(Color.FromArgb(150, 150, 255)),
                        new Rectangle(currentPositionX, currentPositionY, CellSize, CellSize));
                }

                currentPositionY += CellSize;
            }
            currentPositionX += CellSize;
        }

        formGraphics.Dispose();
    }

    public static void DrawPlayer(Bitmap bitmap, Player player)
    {
        var formGraphics = Graphics.FromImage(bitmap);

        var drawPositionX = StartPointX + player.PositionX * CellSize;
        var drawPositionY = StartPointY + player.PositionY * CellSize;

        formGraphics.FillRectangle(playerBrush,
            new Rectangle(drawPositionX, drawPositionY,
            CellSize, CellSize));
    }

    public static void DrawEnemy(Bitmap bitmap, Enemy enemy)
    {
        var formGraphics = Graphics.FromImage(bitmap);

        var drawPositionX = StartPointX + enemy.PositionX * CellSize;
        var drawPositionY = StartPointY + enemy.PositionY * CellSize;

        formGraphics.FillRectangle(enemyBrush,
            new Rectangle(drawPositionX, drawPositionY,
            CellSize, CellSize));
    }

    public static void DrawCoin(Bitmap bitmap, Coin coin)
    {
        var formGraphics = Graphics.FromImage(bitmap);

        var drawPositionX = StartPointX + coin.PositionX * CellSize;
        var drawPositionY = StartPointY + coin.PositionY * CellSize;

        var delta = (CellSize - CoinDiameter) / 2;
        var firstX = drawPositionX + delta;
        var firstY = drawPositionY + delta;

        formGraphics.FillEllipse(coinBrush,
            new Rectangle(firstX, firstY,
            CoinDiameter, CoinDiameter));
    }
}
