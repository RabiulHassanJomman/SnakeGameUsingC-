using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.Input;
using Avalonia;

public class MainWindow : Window
{
    private const int CellSize = 20;
    private Canvas canvas;
    private Game game;
    private TextBlock scoreLebel;

    private double moveProgress = 0;
    private List<(double X, double Y)> prevPositions = new();

    public MainWindow()
    {
        Title = "Snake";
        Width = Game.BoardWidth * CellSize + 20;
        Height = Game.BoardHeight * CellSize + 80;
        CanResize = false;
        game = new Game();
        game.Initialize();

        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(5);
        timer.Tick += OnTick;
        timer.Start();

        canvas = new Canvas
        {
            Width = Game.BoardWidth * CellSize,
            Height = Game.BoardHeight * CellSize,
            Background = Brushes.Black,
        };

        scoreLebel = new TextBlock
        {
            Text = "Score: 0    Best: 0",
            Foreground = Brushes.White,
            FontSize = 18,
            Margin = new Thickness(10)
        };

        var layout = new StackPanel();
        layout.Children.Add(canvas);
        layout.Children.Add(scoreLebel);

        Content = layout;
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
    private void DrawCellAt(double x, double y, IBrush color)
    {
        var rect = new Rectangle
        {
            Width = CellSize,
            Height = CellSize,
            Fill = color
        };
        Canvas.SetLeft(rect, x);
        Canvas.SetTop(rect, y);
        canvas.Children.Add(rect);
    }
    private void Draw()
    {
        canvas.Children.Clear();
        DrawBorder();

        DrawCell(game.food.X, game.food.Y, Brushes.Red);

        for (int i = 0; i < game.snake.Count; i++)
        {
            double targetX = game.snake[i].X * CellSize;
            double targetY = game.snake[i].Y * CellSize;

            double prevX = i < prevPositions.Count ? prevPositions[i].X : targetX;
            double prevY = i < prevPositions.Count ? prevPositions[i].Y : targetY;

            double vx = prevX + (targetX - prevX) * moveProgress;
            double vy = prevY + (targetY - prevY) * moveProgress;

            IBrush color = (i == 0) ? Brushes.Green : Brushes.LimeGreen;
            DrawCellAt(vx, vy, color);
        }

        if (game.currentState == Game.GameState.GameOver)
            DrawGameOver();
    }
    private void OnTick(object? sender, EventArgs e)
    {
        moveProgress += 1.0 / game.moveDelay;

        if (moveProgress >= 1.0)
        {
            prevPositions = game.snake
                .Select(p => ((double)(p.X * CellSize), (double)(p.Y * CellSize)))
                .ToList();
            game.Update();
            moveProgress -= 1.0;
        }

        Draw();
        scoreLebel.Text = $"Score: {game.score}     Best: {game.highScore}";
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (game.currentState == Game.GameState.Playing)
        {
            switch (e.Key)
            {
                case Key.Left:
                    if (game.currentDirection != Game.Direction.RIGHT)
                        game.currentDirection = Game.Direction.LEFT;
                    break;

                case Key.Right:
                    if (game.currentDirection != Game.Direction.LEFT)
                        game.currentDirection = Game.Direction.RIGHT;
                    break;

                case Key.Up:
                    if (game.currentDirection != Game.Direction.DOWN)
                        game.currentDirection = Game.Direction.UP;
                    break;


                case Key.Down:
                    if (game.currentDirection != Game.Direction.UP)
                        game.currentDirection = Game.Direction.DOWN;
                    break;
            }
        }
        else if (game.currentState == Game.GameState.GameOver)
        {
            switch (e.Key)
            {
                case Key.R:
                    game.Restart();
                    moveProgress = 0;
                    prevPositions = game.snake
                        .Select(p => ((double)(p.X * CellSize), (double)(p.Y * CellSize)))
                        .ToList();
                    break;

                case Key.Escape:
                    Close();
                    break;
            }
        }
        base.OnKeyDown(e);
    }

    private void DrawGameOver()
    {
        var overlay = new TextBlock
        {
            Text = $"GAME OVER\nScore: {game.score}     Best: {game.highScore} \n\n R = Restart     ESC = Quit",
            Foreground = Brushes.White,
            FontSize = 20,
            FontWeight = FontWeight.Bold,
            TextAlignment = TextAlignment.Center
        };

        Canvas.SetLeft(overlay, Game.BoardWidth * CellSize / 2 - 120);
        Canvas.SetTop(overlay, Game.BoardHeight * CellSize / 2 - 50);
        canvas.Children.Add(overlay);
    }
    private void DrawBorder()
    {
        for (int x = 0; x < Game.BoardWidth; x++)
        {
            DrawCell(x, 0, Brushes.SlateGray);
            DrawCell(x, Game.BoardHeight - 1, Brushes.SlateGray);
        }

        for (int y = 1; y < Game.BoardHeight - 1; y++)
        {
            DrawCell(0, y, Brushes.SlateGray);
            DrawCell(Game.BoardWidth - 1, y, Brushes.SlateGray);
        }
    }
}