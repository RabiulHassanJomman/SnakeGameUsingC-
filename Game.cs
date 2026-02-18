class Game
{
  private bool isRunning = true;
  private const int BoardWidth = 40;
  private const int BoardHeight = 20;

  private int moveCounter = 0;
  private int moveDelay = 10;

  private enum Direction { LEFT, RIGHT, UP, DOWN };
  private Direction currentDirection;

  private List<Position> snake = new List<Position>();
  private Position food;
  private Random random = new Random();

  private enum GameState { Playing, GameOver };
  private GameState currentState = GameState.Playing;

  private int score = 0;
  private int highScore = 0;

  private void DrawConsole()
  {
    Console.CursorVisible = false;
    Console.Clear();
    Console.Title = "Snake Game";

    for (int i = 0; i < BoardHeight; i++)
    {
      DrawAt(0, i, "║");
      DrawAt(BoardWidth - 1, i, "║");
    }

    for (int i = 0; i < BoardWidth; i++)
    {
      DrawAt(i, 0, "═");
      DrawAt(i, BoardHeight - 1, "═");
    }

    DrawAt(0, 0, "╔");
    DrawAt(BoardWidth - 1, 0, "╗");
    DrawAt(0, BoardHeight - 1, "╚");
    DrawAt(BoardWidth - 1, BoardHeight - 1, "╝");
  }

  private void Initialize()
  {
    currentState = GameState.Playing;
    snake = new List<Position>();
    score = 0;
    currentDirection = Direction.RIGHT;
    DrawConsole();

    snake.Add(new Position(BoardWidth / 2, BoardHeight / 2));
    snake.Add(new Position(BoardWidth / 2 - 1, BoardHeight / 2));
    SpawnFood();

    moveDelay = Math.Max(3, 15 - score);
  }

  private void DrawAt(int x, int y, string symbol)
  {
    Console.SetCursorPosition(x, y);
    Console.Write(symbol);
  }

  private void ProcessInput()
  {
    if (Console.KeyAvailable)
    {
      var key = Console.ReadKey(true);
      if (currentState == GameState.Playing)
      {
        switch (key.Key)
        {
          case ConsoleKey.LeftArrow:
            if (currentDirection != Direction.RIGHT)
              currentDirection = Direction.LEFT;
            break;
          case ConsoleKey.RightArrow:
            if (currentDirection != Direction.LEFT)
              currentDirection = Direction.RIGHT;
            break;
          case ConsoleKey.UpArrow:
            if (currentDirection != Direction.DOWN)
              currentDirection = Direction.UP;
            break;
          case ConsoleKey.DownArrow:
            if (currentDirection != Direction.UP)
              currentDirection = Direction.DOWN;
            break;
          case ConsoleKey.Escape:
            isRunning = false;
            break;
        }
      }
      else if (currentState == GameState.GameOver)
      {
        switch (key.Key)
        {
          case ConsoleKey.R:
            Restart();
            break;

          case ConsoleKey.Escape:
            isRunning = false;
            break;
        }
      }
    }
  }

  private bool IsFoodOnTheSnake()
  {
    foreach (var segment in snake)
    {
      if (segment.X == food.X && segment.Y == food.Y)
        return true;
    }
    return false;
  }

  private void SpawnFood()
  {
    do
    {
      food = new Position(random.Next(1, BoardWidth - 1), random.Next(1, BoardHeight - 1));
    } while (IsFoodOnTheSnake());
  }

  private bool IsCollidingWithWall(Position head)
  {
    return head.X <= 0 || head.X >= BoardWidth - 1 ||
        head.Y <= 0 || head.Y >= BoardHeight - 1;
  }

  private bool IsCollidingWithBody(Position head)
  {
    for (int i = 0; i < snake.Count - 1; i++)
    {
      Position segment = snake[i];
      if (segment.X == head.X && segment.Y == head.Y) return true;
    }
    return false;
  }

  private bool DidFoodEaten()
  {
    return food.X == snake[0].X && food.Y == snake[0].Y;
  }

  private void DrawGameOver()
  {
    int cx = BoardWidth / 2;
    int cy = BoardHeight / 2;

    DrawAt(cx - 5, cy - 1, "GAME OVER");
    DrawAt(cx - 7, cy, "Score:      " + score);
    DrawAt(cx - 7, cy + 1, "Best Score: " + highScore);
    DrawAt(cx - 9, cy + 2, "R = Restart | ESC = Quit");
  }

  private void Update()
  {
    if (currentState != GameState.Playing) return;
    moveCounter++;
    if (moveCounter >= moveDelay)
    {
      moveCounter = 0;

      int dx = 0, dy = 0;
      switch (currentDirection)
      {
        case Direction.LEFT:
          dx = -1;
          break;
        case Direction.RIGHT:
          dx = 1;
          break;

        case Direction.UP:
          dy = -1;
          break;

        case Direction.DOWN:
          dy = 1;
          break;
      }

      Position currentHead = snake[0];
      Position newHead = new Position(currentHead.X + dx, currentHead.Y + dy);

      if (IsCollidingWithBody(newHead) || IsCollidingWithWall(newHead))
      {
        if (score > highScore) highScore = score;
        currentState = GameState.GameOver;
        return;
      }
      snake.Insert(0, newHead);

      if (!DidFoodEaten())
      {
        Position tail = snake[^1];
        DrawAt(tail.X, tail.Y, " ");
        snake.RemoveAt(snake.Count - 1);
      }
      else
      {
        score++;
        moveDelay = Math.Max(3, 15 - score);

        DrawAt(food.X, food.Y, " ");
        SpawnFood();
      }
    }
  }

  private void Restart() { Initialize(); }

  private void Render()
  {
    if (currentState == GameState.Playing)
    {
      foreach (var pos in snake)
      {
        DrawAt(pos.X, pos.Y, "○");
      }

      DrawAt(snake[0].X, snake[0].Y, "●");
      DrawAt(food.X, food.Y, "◙");
    }
    else if (currentState == GameState.GameOver) { DrawGameOver(); }
    Console.SetCursorPosition(0, BoardHeight + 1);
    Console.Write("Score: " + score);
  }

  public void Run()
  {
    Initialize();
    while (isRunning)
    {
      ProcessInput();
      Update();
      Render();
      Thread.Sleep(10);
    }
  }
}
