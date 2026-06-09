using Raylib_cs;

namespace flappy_bird_prosject;
internal static class Program
{
    [System.STAThread] // STAThread is required if you deploy using NativeAOT on Windows - See https://github.com/raylib-cs/raylib-cs/issues/301
    public static void Main()
    {
        Raylib.InitWindow(800, 480, "Flappy Bird");
        Raylib.SetTargetFPS(60);
        Raylib.GetFrameTime();

            // Bird variables
            float birdX = 200;
            float birdY = 240;
            float birdRadius = 15;
            float birdVelocity = -3;
            //float jumpStrength = +1;
            //float gravity = +2;

            while (!Raylib.WindowShouldClose())
            
    {
        // --- Game Logic ---
        // 1. Check for jump input
        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            birdVelocity = -8; // Set negative velocity to move the bird UP
        }

        // 2. Apply physics
            birdVelocity += 0.5f; // Gravity pulls the bird down over time
            birdY += birdVelocity; // Move the bird

        // --- Drawing ---
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Blue);

        Raylib.DrawCircle((int)birdX, (int)birdY, birdRadius, Color.Yellow);

        Raylib.EndDrawing();
        Raylib.CloseWindow();
    }
    
}
}




            /*{
                // --- Game Logic ---
                // Example decrement: you can adjust birdY here for gravity
                birdY += 2; 
                jumpStrength += 1;
                birdVelocity += 3;
                gravity += 2;

                // --- Drawing ---
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Blue);

                // 2. Fixed DrawCircle syntax (integers for coordinates, valid Color call)
                Raylib.DrawCircle((int)birdX, (int)birdY, birdRadius, Color.Yellow);

                Raylib.EndDrawing();
            } */

    
        
    