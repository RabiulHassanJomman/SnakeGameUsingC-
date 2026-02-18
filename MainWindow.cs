using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;

public class MainWindow : Window
{
    private const int CellSize = 20;
    private Canvas canvas;
    private Game game;

    public MainWindow()
    {
        Title = "Snake";
        Width = 820;
        Height = 480;
        CanResize = false;
        game = new Game();
        game.Initialize();

        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(16);
        timer.Tick += OnTick;
        timer.Start();

        canvas = new Canvas
        {
            Width = 800,
            Height = 400,
            Background = Brushes.Black,
        };

        Content = canvas;
    }

    private void DrawCell(int x, int y, IBrush color)
    {
        var rect = new Rectangle
        {
            Width = CellSize,
            Height = CellSize,
            Fill = color
        };

        Canvas.SetLeft(rect, x * CellSize);
        Canvas.SetTop(rect, y * CellSize);
        canvas.Children.Add(rect);
    }
    private void Draw(){
        canvas.Children.Clear();

        DrawCell(game.food.X, game.food.Y, Brushes.Red);

        for(int i = 1; i < game.snake.Count; i++){
            DrawCell(game.snake[i].X, game.snake[i].Y, Brushes.LimeGreen);
        }

        DrawCell(game.snake[0].X, game.snake[0].Y, Brushes.Green);
    }
    private void OnTick(object? sender, EventArgs e){
        game.Update();
        Draw();
    }


}