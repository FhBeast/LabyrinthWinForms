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

        field = new(30, 60);
        field.GenerateDepthSearch();
        Draw();
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
        field.RunOneStepWaveTracing();
        Draw();
    }
}
