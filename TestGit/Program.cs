using System;
using System.Collections.Generic;
using System.Threading;

namespace TestGit
{
	class Program
	{
		static void Main(string[] args)
		{
			gd();
		}

		static void gd()
		{
			const int width = 60;
			const int groundY = 10;
			const int playerX = 5;

			int playerY = groundY;
			double velocityY = 0;
			bool onGround = true;
			bool alive = true;
			int score = 0;
			int tick = 0;

			var obstacles = new List<int>(); // x positions of spikes

			Console.CursorVisible = false;
			Console.Clear();

			while (alive)
			{
				// Input (non-blocking)
				if (Console.KeyAvailable)
				{
					var key = Console.ReadKey(true).Key;
					if ((key == ConsoleKey.Spacebar || key == ConsoleKey.UpArrow) && onGround)
					{
						velocityY = -2.5;
						onGround = false;
					}
				}

				// Physics
				velocityY += 0.5; // gravity
				playerY += (int)Math.Round(velocityY);

				if (playerY >= groundY)
				{
					playerY = groundY;
					velocityY = 0;
					onGround = true;
				}

				// Spawn obstacles
				if (tick % 20 == 0)
					obstacles.Add(width - 1);

				// Move obstacles
				for (int i = 0; i < obstacles.Count; i++)
					obstacles[i]--;
				obstacles.RemoveAll(x => x < 0);

				// Collision detection
				foreach (int ox in obstacles)
				{
					if (ox == playerX && playerY == groundY)
					{
						alive = false;
						break;
					}
				}

				// Render
				Console.SetCursorPosition(0, 0);

				for (int row = 0; row <= groundY + 1; row++)
				{
					for (int col = 0; col < width; col++)
					{
						if (row == playerY && col == playerX)
							Console.Write('[');
						else if (row == playerY && col == playerX + 1)
							Console.Write(']');
						else if (row == groundY + 1)
							Console.Write('=');
						else if (row == groundY && obstacles.Contains(col))
							Console.Write('^');
						else
							Console.Write(' ');
					}
					Console.WriteLine();
				}

				Console.WriteLine($"Score: {score}   ");
				Console.WriteLine("SPACE / UP to jump");

				score++;
				tick++;
				Thread.Sleep(80);
			}

			Console.SetCursorPosition(0, groundY + 4);
			Console.WriteLine($"Game Over! Score: {score}");
			Console.CursorVisible = true;
		}
	}
}
