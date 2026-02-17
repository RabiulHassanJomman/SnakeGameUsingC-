class Game
{
  private bool isRunning = true;

  private void Initialize()
  {
    Console.CursorVisible = false;
    Console.Clear();
    Console.Title = "Snake Game";
  }

  private void ProcessInput()
  {
    if (Console.KeyAvailable)
    {
      var key = Console.ReadKey(true);
      if(key.Key == ConsoleKey.Escape)
        isRunning = false;
    }
  }

  private void Update()
  {
    Console.WriteLine("UPDATED");
  }

  private void Render()
  {
    Console.WriteLine("RENDERED");
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