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

    public MainWindow()
    {
        Title = "Snake";
        Width = Game.BoardWidth * CellSize + 20;
        Height = Game.BoardHeight * CellSize + 80;
        CanResize = false;
        game = new Game();
        game.Initialize();

        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(16);
        timer.Tick += OnTick;
        timer.Start();

        canvas = new Canvas
        {
            Width = Game.BoardWidth * CellSize,
            Height = Game.BoardHeight * CellSize,
            Background = Brushes.Black,
        };

        scoreLebel = new TextBlock{
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
    private void Draw()
    {
        canvas.Children.Clear();

        DrawCell(game.food.X, game.food.Y, Brushes.Red);

        for (int i = 1; i < game.snake.Count; i++)
        {
            DrawCell(game.snake[i].X, game.snake[i].Y, Brushes.LimeGreen);
        }

        DrawCell(game.snake[0].X, game.snake[0].Y, Brushes.Green);

        if(game.currentState == Game.GameState.GameOver)
            DrawGameOver();
    }
    private void OnTick(object? sender, EventArgs e)
    {
        game.Update();
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
                    break;

                case Key.Escape:
                    Close();
                    break;
            }
        }
        base.OnKeyDown(e);
    }

    private void DrawGameOver(){
        var overlay = new TextBlock{
            Text = $"GAME OVER\nScore: {game.score}     Best: {game.highScore} \n\n R = Restart     ESC = Quit",
            Foreground = Brushes.White,
            FontSize = 20,
            FontWeight = FontWeight.Bold,
            TextAlignment = TextAlignment.Center
        };

        Canvas.SetLeft(overlay, Game.BoardWidth * CellSize /2 - 120);
        Canvas.SetTop(overlay, Game.BoardHeight * CellSize /2 - 50);
        canvas.Children.Add(overlay);
    }
}