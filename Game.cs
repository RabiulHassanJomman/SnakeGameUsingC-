class Game
{
  private bool isRunning = true;
  private const int BoardWidth = 40;
  private const int BoardHeight = 20;



  private void Initialize()
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
      if (key.Key == ConsoleKey.Escape)
        isRunning = false;
    }
  }

  private void Update()
  {

  }

  private void Render()
  {
    DrawAt(10, 10, "*");

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