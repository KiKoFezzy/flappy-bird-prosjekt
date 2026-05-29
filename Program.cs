using Raylib_cs;

namespace flappy_bird_prosject;
internal static class Program
{
    [System.STAThread] // STAThread is required if you deploy using NativeAOT on Windows - See https://github.com/raylib-cs/raylib-cs/issues/301
    public static void Main()
    {
        Raylib.InitWindow(800, 480, "Flappy Bird");

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.Blue);
            //Raylib.DrawText("Hello, player ONE!", 12, 12, 20, Color.Black);
            Raylib.DrawCircle((float birdX, 10); {float birdY; decrement}; {float birdRadius; 5} Color.Yellow);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}