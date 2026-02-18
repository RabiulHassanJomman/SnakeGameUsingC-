class Game
{
  public const int BoardWidth = 40;
  public const int BoardHeight = 20;

  private int moveCounter = 0;
  private int moveDelay = 10;

  public enum Direction { LEFT, RIGHT, UP, DOWN };
  public Direction currentDirection;

  public List<Position> snake = new List<Position>();
  public Position food;
  public Random random = new Random();

  public enum GameState { Playing, GameOver };
  public GameState currentState = GameState.Playing;

  public int score = 0;
  public int highScore = 0;

  public void Initialize()
  {
    currentState = GameState.Playing;
    snake = new List<Position>();
    score = 0;
    currentDirection = Direction.RIGHT;

    snake.Add(new Position(BoardWidth / 2, BoardHeight / 2));
    snake.Add(new Position(BoardWidth / 2 - 1, BoardHeight / 2));
    SpawnFood();

    moveDelay = Math.Max(3, 15 - score);
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

  public void Update()
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
        snake.RemoveAt(snake.Count - 1);
      }
      else
      {
        score++;
        moveDelay = Math.Max(3, 15 - score);

        SpawnFood();
      }
    }
  }

  public void Restart() { Initialize(); }

  
}
