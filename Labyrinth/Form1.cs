using Labyrinth.Model;
using Labyrinth.Util;

namespace Labyrinth;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private int WidthBitmap { get; set; }
    private int HeightBitmap { get; set; }
    private Field Field = new();
    private Point MousePictureBoxPosition { get; set; }
    private Player Player { get; set; }
    private Enemy Enemy { get; set; }

    private void Form1_Load(object sender, EventArgs e)
    {
        HeightBitmap = pictureBox1.Height;
        WidthBitmap = pictureBox1.Width;

        Field = new(30, 60);
        Field.GenerateDepthSearch();
        Field.AddBraidng();
        Player = new(0, 0);
        Enemy = new(3, 3);
        StartWave();
    }

    private void Draw()
    {
        Bitmap bitmap = new(WidthBitmap, HeightBitmap);
        Drawing.DrawWaves(bitmap, Field);
        Drawing.DrawLabytinth(bitmap, Field);
        Drawing.DrawPlayer(bitmap, Player);
        Drawing.DrawEnemy(bitmap, Enemy);
        pictureBox1.Image = bitmap;
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        Draw();
    }

    private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
    {
        MousePictureBoxPosition = new(e.X, e.Y);
    }

    private void StartWave()
    {
        Task.Run(() =>
        {
            Field.WaveTracingFast();
        });
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
        int startPointX = Drawing.StartPointX;
        int startPointY = Drawing.StartPointY;
        int cellSize = Drawing.CellSize;
        int width = Field.Width;
        int height = Field.Height;

        int PositionX = (MousePictureBoxPosition.X - startPointX) / cellSize;
        int PositionY = (MousePictureBoxPosition.Y - startPointY) / cellSize;

        if (PositionX >= 0 &&
            PositionY >= 0 &&
            PositionX < width &&
            PositionY < height &&
            !new Point(PositionX, PositionY).Equals(Field.EndPosition))
        {
            Field.SetEnd(PositionX, PositionY);
            StartWave();
        }
    }

    private void timer2_Tick(object sender, EventArgs e)
    {
        Enemy.Update(Field);
    }
}
