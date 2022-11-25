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
    private Field field = new();

    private void Form1_Load(object sender, EventArgs e)
    {
        HeightBitmap = pictureBox1.Height;
        WidthBitmap = pictureBox1.Width;

        field = new(90, 180);
        field.GenerateDepthSearch();
        field.AddBraidng();
        StartWave();
    }

    private void Draw()
    {
        Bitmap bitmap = new(WidthBitmap, HeightBitmap);
        Drawing.DrawWaves(bitmap, field);
        Drawing.DrawLabytinth(bitmap, field);
        pictureBox1.Image = bitmap;
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        Draw();
    }

    private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
    {
        int startPointX = Drawing.StartPointX;
        int startPointY = Drawing.StartPointY;
        int cellSize = Drawing.CellSize;
        int width = field.Width;
        int height = field.Height;

        int PositionX = (e.X - startPointX) / cellSize;
        int PositionY = (e.Y - startPointY) / cellSize;

        if (PositionX >= 0 &&
            PositionY >= 0 &&
            PositionX < width &&
            PositionY < height && 
            !new Point(PositionX, PositionY).Equals(field.EndPosition))
        {
            field.SetEnd(PositionX, PositionY);
            StartWave();
        }
    }

    private void StartWave()
    {
        Task.Run(
            () => {
                field.WaveTracing();
            });
    }
}
