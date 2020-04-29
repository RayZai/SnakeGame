using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.IO;
namespace Snake
{
	//Structure Entity called Position
	struct Position
	{
		//The attributes of Position
		public int row;
		public int col;
		//Constructor for Position
		public Position(int row, int col)
		{
			this.row = row;
			this.col = col;
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			//adds background music to game
			var myPlayer = new System.Media.SoundPlayer();
			myPlayer.SoundLocation = @"bgmusic.wav";
			myPlayer.Play();
			myPlayer.PlayLooping();

			//Initializing variables
			byte right = 0;
			byte left = 1;
			byte down = 2;
			byte up = 3;
			int lastFoodTime = 0;
			int foodDissapearTime = 15000;
			int negativePoints = 0;

			//A array of Position entities called directions
			//defining the direction that the snake can move
			Position[] directions = new Position[]
			{
				new Position(0, 1), // right
				new Position(0, -1), // left
				new Position(1, 0), // down
				new Position(-1, 0), // up
			 };

			double sleepTime = 100;
			int direction = right;
			Random randomNumbersGenerator = new Random();
			Console.BufferHeight = Console.WindowHeight;
			lastFoodTime = Environment.TickCount;

			//A list of Positions entity that contain the positions of the obstacle
			List<Position> obstacles = new List<Position>()
			{
				new Position(randomNumbersGenerator.Next(1, Console.WindowHeight),
							randomNumbersGenerator.Next(0, Console.WindowWidth)),
				new Position(randomNumbersGenerator.Next(1, Console.WindowHeight),
							randomNumbersGenerator.Next(0, Console.WindowWidth)),
				new Position(randomNumbersGenerator.Next(1, Console.WindowHeight),
							randomNumbersGenerator.Next(0, Console.WindowWidth)),
				new Position(randomNumbersGenerator.Next(1, Console.WindowHeight),
							randomNumbersGenerator.Next(0, Console.WindowWidth)),
				new Position(randomNumbersGenerator.Next(1, Console.WindowHeight),
							randomNumbersGenerator.Next(0, Console.WindowWidth)),

			};
			//Displaying the obstacles
			foreach (Position obstacle in obstacles)
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.SetCursorPosition(obstacle.col, obstacle.row);
				Console.Write("=");
			}

			//creating the snake and putting the coordinates into queue
			Queue<Position> snakeElements = new Queue<Position>();
			for (int i = 0; i <= 3; i++)
			{
				snakeElements.Enqueue(new Position(0, i));
			}

			//Creating position for the food and displaying it
			//The loop continues until the food element is not in snakeElements or obstacles
			Position food;
			do
			{
				food = new Position(randomNumbersGenerator.Next(1, Console.WindowHeight),
					randomNumbersGenerator.Next(0, Console.WindowWidth));
			}
			while (snakeElements.Contains(food) || obstacles.Contains(food));
			Console.SetCursorPosition(food.col, food.row);
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("@");

			//Displaying the snake
			foreach (Position position in snakeElements)
			{
				Console.SetCursorPosition(position.col, position.row);
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write("*");
			}

			
			
		
			
			
			
			while (true)
			{
				negativePoints++;

				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo userInput = Console.ReadKey();
					if (userInput.Key == ConsoleKey.LeftArrow) //if direction isnt equal to right it will move left
					{
						if (direction != right) direction = left;
					}
					if (userInput.Key == ConsoleKey.RightArrow) //if right arrow click it will move to the right
					{
						if (direction != left) direction = right;
					}
					if (userInput.Key == ConsoleKey.UpArrow)
					{
						if (direction != down) direction = up;
					}
					if (userInput.Key == ConsoleKey.DownArrow)
					{
						if (direction != up) direction = down;
					}
				}

				Position snakeHead = snakeElements.Last();
				Position nextDirection = directions[direction];

				Position snakeNewHead = new Position(snakeHead.row + nextDirection.row,
					snakeHead.col + nextDirection.col);

				if (snakeNewHead.col < 0) snakeNewHead.col = Console.WindowWidth - 1;
				if (snakeNewHead.row < 1) snakeNewHead.row = Console.WindowHeight - 1;
				if (snakeNewHead.row >= Console.WindowHeight) snakeNewHead.row = 1;
				if (snakeNewHead.col >= Console.WindowWidth) snakeNewHead.col = 0;

				//Displaying the score
				int userPoints = (snakeElements.Count - 6) * 100 - negativePoints;
						//if (userPoints < 0) userPoints = 0;
				Console.SetCursorPosition(0, 0);
				Console.ForegroundColor = ConsoleColor.White;
				userPoints = Math.Max(userPoints, 0);
				if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead))
				{
					string gameover = "Game over!";
					Console.SetCursorPosition((Console.WindowWidth - gameover.Length) / 2, (Console.WindowHeight / 2) - 4);
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(gameover);



					string statuspoint = "Your points are {0}";	
					Console.SetCursorPosition((Console.WindowWidth - statuspoint.Length) / 2, (Console.WindowHeight / 2)- 3);
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(statuspoint,userPoints);
					//Asks the user for name then the name and score will be stored in a text file
					string enternamemsg = "Enter your name: ";
					Console.SetCursorPosition((Console.WindowWidth - statuspoint.Length) / 2, (Console.WindowHeight / 2) - 2);
					Console.Write(enternamemsg);
					Console.ForegroundColor = ConsoleColor.Red;


					string name = Console.ReadLine();
					string LMsg = name + " " + userPoints + "\n";
					File.AppendAllText("score.txt", LMsg);

					string exit = "Press enter to exit";
					Console.SetCursorPosition((Console.WindowWidth -exit.Length) / 2, (Console.WindowHeight / 2) - 1);
					Console.WriteLine(exit);
					Console.ReadLine();
					return;


				//If the userPoints is more than 500, then the user wins	
				}else if(userPoints > 500)
				{

					string gameover = "You win!";
					Console.SetCursorPosition((Console.WindowWidth - gameover.Length) / 2, (Console.WindowHeight / 2) - 4);
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(gameover);



					string statuspoint = "Your points are {0}";
					Console.SetCursorPosition((Console.WindowWidth - statuspoint.Length) / 2, (Console.WindowHeight / 2) - 3);
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(statuspoint, userPoints);
					//Asks the user for name then the name and score will be stored in a text file
					string enternamemsg = "Enter your name: ";
					Console.SetCursorPosition((Console.WindowWidth - statuspoint.Length) / 2, (Console.WindowHeight / 2) - 2);
					Console.Write(enternamemsg);
					Console.ForegroundColor = ConsoleColor.Red;


					string name = Console.ReadLine();
					string LMsg = name + " " + userPoints + "\n";
					File.AppendAllText("score.txt", LMsg);

					string exit = "Press enter to exit";
					Console.SetCursorPosition((Console.WindowWidth - exit.Length) / 2, (Console.WindowHeight / 2) - 1);
					Console.WriteLine(exit);
					Console.ReadLine();
					return;
				}

				Console.SetCursorPosition(snakeHead.col, snakeHead.row);
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write("*");

				snakeElements.Enqueue(snakeNewHead);
				Console.SetCursorPosition(snakeNewHead.col, snakeNewHead.row);
				Console.ForegroundColor = ConsoleColor.Gray;
				if (direction == right) Console.Write(">"); //direction for snake moving right 
				if (direction == left) Console.Write("<");//direction for snake moving left and so forth
				if (direction == up) Console.Write("^");
				if (direction == down) Console.Write("v");
				//check snakehead overlapping food position
				if (snakeNewHead.col == food.col && snakeNewHead.row == food.row)
				{
					
					// feeding the snake
					//create new food position object until position is not overlapping snake or obstacle
					do
					{
						food = new Position(randomNumbersGenerator.Next(1, Console.WindowHeight),
							randomNumbersGenerator.Next(0, Console.WindowWidth));
					}
					while (snakeElements.Contains(food) || obstacles.Contains(food));
					//refresh last food eat time timer counter 
					lastFoodTime = Environment.TickCount;
					Console.SetCursorPosition(food.col, food.row);
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write("@");
					sleepTime--;
					//create new obstacle position object until no overlapping with snake and other obstacle
					Position obstacle = new Position();
					do
					{
						obstacle = new Position(randomNumbersGenerator.Next(1, Console.WindowHeight),
							randomNumbersGenerator.Next(0, Console.WindowWidth));
					}
					while (snakeElements.Contains(obstacle) ||
						obstacles.Contains(obstacle) ||
						(food.row != obstacle.row && food.col != obstacle.row));
					obstacles.Add(obstacle);
					Console.SetCursorPosition(obstacle.col, obstacle.row);
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write("=");
				}
				else
				{
					// moving...
					//move the snake to new postion and delete the last snake element
					Position last = snakeElements.Dequeue();
					Console.SetCursorPosition(last.col, last.row);
					Console.Write(" ");
				}
				//when timer counter between eating last food is higher than default food dissapear time
				if (Environment.TickCount - lastFoodTime >= foodDissapearTime)
				{
					//decrease user score by 50 if user take too long to eat the food
					//delete the food
					superFood = false;
					negativePoints = negativePoints + 50;
					Console.SetCursorPosition(food.col, food.row);
					Console.Write(" ");
					//create new food position until no overlapping with obstacle and snake
					do
					{
						food = new Position(randomNumbersGenerator.Next(1, Console.WindowHeight),
							randomNumbersGenerator.Next(0, Console.WindowWidth));
					}
					while (snakeElements.Contains(food) || obstacles.Contains(food));
					//refresh last food eat time timer counter 
					lastFoodTime = Environment.TickCount;
				}
				
				//This will clear the previous score shown then display the new score
				for (int i = 0; i < 13;i++)
				{
					Console.SetCursorPosition(i, 0);
					Console.Write(" ");
				}
				Console.SetCursorPosition(0, 0);
				string msg = "Score: " + userPoints;
				Console.Write(msg);

				Console.SetCursorPosition(food.col, food.row);
				Console.ForegroundColor = ConsoleColor.Yellow;
				sleepTime -= 0.01;

				Thread.Sleep((int)sleepTime);
			}
		}
	}
}
