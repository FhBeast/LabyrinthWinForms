using Labyrinth.Model;
using Labyrinth.Util;

namespace Labyrinth;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        Player = new(0, 0);
        Enemy = new(10, 10);
        Field = new();
        Coins = new();
        Coins.Add(new Coin(5, 5));
        Coins.Add(new Coin(50, 5));
        Coins.Add(new Coin(5, 25));
    }

    private bool ReadyToDraw { get; set; }
    private int WidthBitmap { get; set; }
    private int HeightBitmap { get; set; }
    private Field Field { get; set; }
    private Point MousePictureBoxPosition { get; set; }
    private Player Player { get; set; }
    private Enemy Enemy { get; set; }

    private List<Coin> Coins { get; set; }

    private void Form1_Load(object sender, EventArgs e)
    {
        HeightBitmap = pictureBox1.Height;
        WidthBitmap = pictureBox1.Width;

        Field = new(30, 60);
        FieldGenerator.GenerateDepthSearch(Field);
        FieldGenerator.AddBraidng(Field);
        Enemy.SetSlowdown(1);
        StartWave();
    }

    private void Draw()
    {
        Bitmap bitmap = new(WidthBitmap, HeightBitmap);
        Drawing.DrawWaves(bitmap, Field);
        Drawing.DrawLabytinth(bitmap, Field);
        Drawing.DrawPlayer(bitmap, Player);
        Drawing.DrawEnemy(bitmap, Enemy);
        foreach (var coin in Coins)
        {
            Drawing.DrawCoin(bitmap, coin);
        }
        pictureBox1.Image = bitmap;
    }

    private void Timer1_Tick(object sender, EventArgs e)
    {
        if(ReadyToDraw)
            Draw();
    }

    private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
    {
        MousePictureBoxPosition = new(e.X, e.Y);
    }
    
    private void StartWave()
    {
        ReadyToDraw = false;
        Task.Run(() =>
        {
            Field.WaveTracingFast();
            ReadyToDraw = true;
        });
    }

    private void PictureBox1_Click(object sender, EventArgs e)
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

    private void Timer2_Tick(object sender, EventArgs e)
    {
        Enemy.Update(Field);
        Player.Update(Field);
        if (!new Point(Player.PositionX, Player.PositionY).Equals(Field.EndPosition))
        {
            Field.SetEnd(Player.PositionX, Player.PositionY);
            StartWave();
        }
        Player.MoveLeft = false;
        Player.MoveRight = false;
        Player.MoveUp = false;
        Player.MoveDown = false;
    }

    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.W)
        {
            Player.MoveUp = true;
        }
        else if (e.KeyCode == Keys.S)
        {
            Player.MoveDown = true;
        }
        else if (e.KeyCode == Keys.A)
        {
            Player.MoveLeft = true;
        }
        else if (e.KeyCode == Keys.D)
        {
            Player.MoveRight = true;
        }
    }
}
