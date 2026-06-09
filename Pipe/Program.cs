using Raylib_cs;
using System.Collections.Generic;
using System;

public struct Pipe 
{ 
    public float X; 
    public float GapY; 
    public bool Scored; 
}

// Variables
float pipeWidth = 60f; // Scaled up slightly from 3 for standard window sizes
float gapHeight = 150f;
float pipeSpeed = 2f;
float pipeSpawnTimer = 0.0f;
float spawnInterval = 2.0f; // Time in seconds between spawns

List<Pipe> pipes = new List<Pipe>();
// Update Pipes
for (int i = pipes.Count - 1; i >= 0; i--)
{
    Pipe pipe = pipes[i];
    pipe.X -= pipeSpeed; // Move to the left

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

// Spawn New Pipes
pipeSpawnTimer += Raylib.GetFrameTime();
if (pipeSpawnTimer >= spawnInterval)
{
    pipeSpawnTimer = 0.0f;
    Random rand = new Random();
    
    // Assuming a window height of 800. 
    // GapY is the Y coordinate of the *center* of the gap.
    float randomGapY = rand.Next(200, 600); 

    pipes.Add(new Pipe { X = 800, GapY = randomGapY, Scored = false });
}
foreach (Pipe pipe in pipes)
{
    // Top Pipe
    // Height of top pipe extends from 0 to the top of the gap
    float topPipeHeight = pipe.GapY - (gapHeight / 2);
    Rectangle topRect = new Rectangle(pipe.X, 0, pipeWidth, topPipeHeight);
    .DrawRectangleRec(topRect, Color.Green);

    // Bottom Pipe
    // Height of bottom pipe extends from the bottom of the gap to the bottom of the screen (e.g., 800)
    float bottomPipeY = pipe.GapY + (gapHeight / 2);
    float bottomPipeHeight = 800 - bottomPipeY;
    Rectangle bottomRect = new Rectangle(pipe.X, bottomPipeY, pipeWidth, bottomPipeHeight);
    Raylib.DrawRectangleRec(bottomRect, Color.Green);
}
Rectangle birdRect = new Rectangle(birdX, birdY, birdWidth, birdHeight);

foreach (Pipe pipe in pipes)
{
    float topPipeHeight = pipe.GapY - (gapHeight / 2);
    Rectangle topRect = new Rectangle(pipe.X, 0, pipeWidth, topPipeHeight);
    
    float bottomPipeY = pipe.GapY + (gapHeight / 2);
    float bottomPipeHeight = 800 - bottomPipeY;
    Rectangle bottomRect = new Rectangle(pipe.X, bottomPipeY, pipeWidth, bottomPipeHeight);

    if (Raylib.CheckCollisionRecs(birdRect, topRect) || Raylib.CheckCollisionRecs(birdRect, bottomRect))
    {
        // Game Over Logic
    }
}