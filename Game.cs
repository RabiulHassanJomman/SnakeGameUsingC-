class Game
{
  private bool isRunning = true;
  private const int BoardWidth = 40;
  private const int BoardHeight = 20;

  private int moveCounter = 0;
  private int moveDelay = 3;

  private enum Direction { LEFT, RIGHT, UP, DOWN };
  private Direction currentDirection = Direction.LEFT;

  private List<Position> snake = new List<Position>();

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
    DrawConsole();

    snake.Add(new Position(BoardWidth / 2, BoardHeight / 2));
    snake.Add(new Position(BoardWidth / 2 - 1, BoardHeight / 2));
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
  }

  private void Update()
  {
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
      snake.Insert(0, new Position(currentHead.x + dx, currentHead.y + dy));

      Position tail = snake[^1];
      DrawAt(tail.x, tail.y, " ");
      snake.RemoveAt(snake.Count - 1);
    }
  }

  private void Render()
  {
    foreach (var pos in snake)
    {
      DrawAt(pos.x, pos.y, "○");
    }

    DrawAt(snake[0].x, snake[0].y, "●");

    Console.SetCursorPosition(0, BoardHeight + 1);
    Console.Write("Score: 0");
  }

  public void Run()
  {
    Initialize();
    while (isRunning)
    {
      ProcessInput();
      Update();
      Render();
      Thread.Sleep(100);
    }
  }
}