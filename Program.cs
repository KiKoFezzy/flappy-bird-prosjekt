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

    public enum GameState { Countdown, Playing, Paused, GameOver } // Added Paused state

    internal static class Program
    {
        [System.STAThread]
        public static void Main()
        {
            Raylib.InitWindow(800, 800, "Flappy Bird");
            Raylib.SetTargetFPS(60);

            // Game State
            GameState currentState = GameState.Countdown;
            float stateTimer = 0.0f;
            int score = 0; 

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
                if (currentState == GameState.Countdown)
                {
                    stateTimer += Raylib.GetFrameTime();
                    if (stateTimer >= 3.0f)
                    {
                        currentState = GameState.Playing;
                    }
                }
                else if (currentState == GameState.Playing)
                {
                    // Toggle Pause (Press P to pause)
                    if (Raylib.IsKeyPressed(KeyboardKey.P))
                    {
                        currentState = GameState.Paused;
                    }

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

                    // Update Pipes & Score
                    for (int i = pipes.Count - 1; i >= 0; i--)
                    {
                        Pipe pipe = pipes[i];
                        pipe.X -= pipeSpeed;

                        // NEW: Scoring Logic
                        if (!pipe.Scored && pipe.X + pipeWidth < birdX)
                        {
                            score++;
                            pipe.Scored = true;
                        }

                        // Remove pipes that go off-screen
                        if (pipe.X < -pipeWidth)
                        {
                            pipes.RemoveAt(i);
                        }
                        else
                        {
                            pipes[i] = pipe;
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
                            score = 0;
                            currentState = GameState.Countdown;
                            stateTimer = 0.0f;
                            break;
                        }
                    }
                }
                else if (currentState == GameState.Paused)
                {
                    // Unpause (Press P to resume)
                    if (Raylib.IsKeyPressed(KeyboardKey.P))
                    {
                        currentState = GameState.Playing;
                    }
                }
                else if (currentState == GameState.GameOver)
                {
                    currentState = GameState.Countdown;
                    stateTimer = 0.0f;
                }

                // --- Drawing --- //
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.SkyBlue);

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

                // Draw Score Display
                string scoreText = score.ToString();
                Raylib.DrawText(scoreText, 20, 20, 40, Color.White);

                // Draw Countdown Overlay
                if (currentState == GameState.Countdown)
                {
                    int countdownNumber = 3 - (int)stateTimer;
                    if (countdownNumber > 0)
                    {
                        string text = countdownNumber.ToString();
                        int fontSize = 80;
                        int textWidth = Raylib.MeasureText(text, fontSize);
                        Raylib.DrawText(text, (800 / 2) - (textWidth / 2), (800 / 2) - 40, fontSize, Color.White);
                    }
                }

                // Draw Pause Overlay
                if (currentState == GameState.Paused)
                {
                    // Draw a dark semi-transparent overlay
                    Raylib.DrawRectangle(0, 0, 800, 800, new Color(0, 0, 0, 150));

                    // Pause Text
                    string text = "PAUSED";
                    int fontSize = 60;
                    int textWidth = Raylib.MeasureText(text, fontSize);
                    Raylib.DrawText(text, (800 / 2) - (textWidth / 2), (800 / 2) - 60, fontSize, Color.White);

                    // Subtext
                    string subtext = "Press P to Resume";
                    int subSize = 30;
                    int subWidth = Raylib.MeasureText(subtext, subSize);
                    Raylib.DrawText(subtext, (800 / 2) - (subWidth / 2), (800 / 2) + 20, subSize, Color.LightGray);
                }

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
