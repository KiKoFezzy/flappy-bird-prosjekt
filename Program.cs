using Raylib_cs;
using System.Collections.Generic;
using System;

namespace flappy_bird_project
{
    public struct Pipe
    {
        public float X;
        public float GapY;
        public bool Scored;
    }

    internal static class Program
    {
        [System.STAThread]
        public static void Main()
        {
            Raylib.InitWindow(800, 800, "Flappy Bird");
            Raylib.SetTargetFPS(60);

            // Bird variables
            float birdX = 200;
            float birdY = 240;
            float birdRadius = 15;
            float birdVelocity = 0;
            float gravity = 0.5f;
            float jumpStrength = -8f;

            // Obstacle variables
            float pipeWidth = 60f;
            float gapHeight = 150f;
            float pipeSpeed = 3f;
            float pipeSpawnTimer = 0.0f;
            float spawnInterval = 2.0f;
            List<Pipe> pipes = new List<Pipe>();

            while (!Raylib.WindowShouldClose())
            {
                // --- Game Logic --- //

                // 1. Check for jump input
                if (Raylib.IsKeyPressed(KeyboardKey.Space))
                {
                    birdVelocity = jumpStrength;
                }

                // 2. Apply physics
                birdVelocity += gravity;
                birdY += birdVelocity;

                // Spawn New Pipes
                pipeSpawnTimer += Raylib.GetFrameTime();
                if (pipeSpawnTimer >= spawnInterval)
                {
                    pipeSpawnTimer = 0.0f;
                    Random rand = new Random();
                    float randomGapY = rand.Next(200, 600);
                    pipes.Add(new Pipe { X = 800, GapY = randomGapY, Scored = false });
                }

                // Update Pipes
                for (int i = pipes.Count - 1; i >= 0; i--)
                {
                    Pipe pipe = pipes[i];
                    pipe.X -= pipeSpeed;

                    // Remove pipes that go off-screen
                    if (pipe.X < -pipeWidth)
                    {
                        pipes.RemoveAt(i);
                    }
                    else
                    {
                        pipes[i] = pipe; // Update struct in list
                    }
                }

                // Collision Logic
                Rectangle birdRect = new Rectangle(birdX - birdRadius, birdY - birdRadius, birdRadius * 2, birdRadius * 2);

                foreach (Pipe pipe in pipes)
                {
                    float topPipeHeight = pipe.GapY - (gapHeight / 2);
                    Rectangle topRect = new Rectangle(pipe.X, 0, pipeWidth, topPipeHeight);

                    float bottomPipeY = pipe.GapY + (gapHeight / 2);
                    float bottomPipeHeight = 800 - bottomPipeY;
                    Rectangle bottomRect = new Rectangle(pipe.X, bottomPipeY, pipeWidth, bottomPipeHeight);

                    if (Raylib.CheckCollisionRecs(birdRect, topRect) || Raylib.CheckCollisionRecs(birdRect, bottomRect))
                    {
                        // Game Over / Reset
                        birdY = 240;
                        birdVelocity = 0;
                        pipes.Clear();
                        break;
                    }
                }

                // --- Drawing --- //
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.RayWhite); // Clear background to prevent trailing visuals

                // Draw Pipes
                foreach (Pipe pipe in pipes)
                {
                    float topPipeHeight = pipe.GapY - (gapHeight / 2);
                    Rectangle topRect = new Rectangle(pipe.X, 0, pipeWidth, topPipeHeight);

                    float bottomPipeY = pipe.GapY + (gapHeight / 2);
                    float bottomPipeHeight = 800 - bottomPipeY;
                    Rectangle bottomRect = new Rectangle(pipe.X, bottomPipeY, pipeWidth, bottomPipeHeight);

                    Raylib.DrawRectangleRec(topRect, Color.Green);
                    Raylib.DrawRectangleRec(bottomRect, Color.Green);
                }

                // Draw Bird
                Raylib.DrawCircle((int)birdX, (int)birdY, birdRadius, Color.Yellow);

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}