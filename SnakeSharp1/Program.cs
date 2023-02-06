using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;
using System.Threading;

namespace Snake
{
    /*
     *declare struct position with public row and
     * column
    */
    struct Position
    {
        public int row;
        public int column;

        public Position(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }

    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    class Program
    {
        static void Main(string[] args)
        {
            /*
            * declare 4 bytes for 4 directions values 0-3
            * declare 3 ints for last time snake eat, last
            * time food dissapear
            * and negative points
            */

            byte right = 0;
            byte left = 1;
            byte down = 2;
            byte up = 3;
            int lastTimeSnakeEat = 0;
            int lastTimeFoodDissapear = 8000;
            int negativePoints = 0;

            /*
            * array of Position:right, left, down, up
            */

            Position[] myPositions = new Position[]
            {
                new Position(0, 1),
                new Position(0, -1),
                new Position(1, 0),
                new Position(-1, 0)
            };

            //declare sleep time and direction
            double sleepTime = 100;
            int direction = right;

            //declare new random int generator
            //make console height to be = window height

            Random randomIntGenerator = new Random();
            Console.BufferHeight = Console.WindowHeight;

            //last food time to be = environment ticking
            lastTimeSnakeEat = Environment.TickCount;

            /*
            * list of positions is 
            */

            List<Position> obstacles = new List<Position>()
            {
                new Position(12, 12),
                new Position(14, 20),
                new Position(7, 7),
                new Position(19, 19),
                new Position(6, 9)
            };

            /*
            * for every position in obstacles
            * console foreground is cyan
            * cursor position is at obstacle column and row
            * write = 
            */

            foreach (Position obstacle in obstacles)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(obstacle.column, obstacle.row);
                Console.Write("=");
            }

            /*
            * queue of Position traverse 5 times and enqueue at Position 0 an i
            */

            Queue<Position> snakeElements = new Queue<Position>();
            for (int i = 0; i <= 5; i++)
            {
                snakeElements.Enqueue(new Position(i, 0));
            }

            //declare food of type Position
            Position food;

            /*
            *using do while loop generate new food at random length, from 0 till window height
            * while snake elements and obstacles contain food
            */
            do
            {
                food = new Position(randomIntGenerator.Next(0, Console.WindowHeight),
                            randomIntGenerator.Next(0, Console.WindowWidth));
            } while (snakeElements.Contains(food) || obstacles.Contains(food));

            /*
            * cursor position at food row and column
            * console foreground color yellow
            * write "@"
            */

            Console.SetCursorPosition(food.column, food.row);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("@");

            /*
            * for every position in snake elements
            * cursor position is at column and row
            * console foreground color is darkgray
            * write "*"
            */

            foreach (Position position in snakeElements)
            {
                Console.SetCursorPosition(position.column, position.row);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("*");
            }

            /*
            * increment negative points while is true
            * and if console key is available take user input with console info key = left arrow
            * and if direction is not equal to right than direction is equal to left
            * if user input key is right arrow and direction is not equal to left than direction is right
            * if user input is up arrow and direction is not down than direction is up
            * if user input is down arrow and direction is not up than direction is down 
            */

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (direction != right) direction = left;
                    }
                    if (userInput.Key == ConsoleKey.RightArrow)
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

                /*
                * last position of snake elements is snake head of type Position
                * next direction of type Position is in array of type directions
                */

                Position snakeHead = snakeElements.Last();
                Position nextDirection = myPositions[direction];

                /*
                * next position of snake head of type Position is new Position in
                * snake head and next direction ro and column
                */

                Position snakeNewHead = new Position(snakeHead.row + nextDirection.row,
                                            snakeHead.column + nextDirection.column);

                /*
                * keep snake inside Console window
                * if snake new head in rows and columns is less than 0 than its rows and columns
                * will be Console window -1
                * and if snake new head in rows and columns is grater or equal to window width and height set them to 0
                */

                if (snakeNewHead.column < 0) snakeNewHead.column = Console.WindowWidth - 1;
                if (snakeNewHead.row < 0) snakeNewHead.row = Console.WindowHeight - 1;
                if (snakeNewHead.row >= Console.WindowHeight) snakeNewHead.row = 0;
                if (snakeNewHead.column >= Console.WindowWidth) snakeNewHead.column = 0;

                /*
                * if snake new elements or obstacles contain snake new head
                * set cursor position to 0 and foreground ro red, write game over
                * user points are equal when there are no more elements in snake body * 100 - negative points
                * user points are going to be max by 0
                * write user points
                * return 
                */

                if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead))
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Game over");
                    int userPoints = (snakeElements.Count - 6) * 100 - negativePoints;
                    userPoints = Math.Max(userPoints, 0);
                    Console.Write("User points:{0}", userPoints);
                    return;
                }

                //set cursor position at snake head
                //console foreground is darkgray
                //write "*"
                Console.SetCursorPosition(snakeHead.column, snakeHead.row);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("*");

                //snake elements are put in snake new head
                //cursor is at snake new head row and column
                //foreground is gray
                //if direction is right, left, up, down write arrows

                snakeElements.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.column, snakeNewHead.row);
                Console.ForegroundColor = ConsoleColor.Gray;
                if (direction == right) Console.Write(">");
                if (direction == left) Console.Write("<");
                if (direction == up) Console.Write("^");
                if (direction == down) Console.Write("v");

                /*
                * if snake new head column and row is equal to food column and row
                * create new food using random generator next in window height and width
                * while snake elements or obstacles contain food
                * last time food was seen is when tick
                * cursor position is at food column and row
                * foreground is yellow
                * write "@"
                * decrement sleep time
                */

                if (snakeNewHead.column == food.column && snakeNewHead.row == food.row)
                {
                    do
                    {
                        food = new Position(randomIntGenerator.Next(0, Console.WindowHeight),
                                            randomIntGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food) || obstacles.Contains(food));
                    lastTimeSnakeEat = Environment.TickCount;
                    Console.SetCursorPosition(food.column, food.row);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("@");
                    sleepTime--;

                    /*
                    * new obstacle position is generated using random from current position inside window height and width
                    * if snake elements or obstacles contain obstacle
                    * than food row and column will not match obstacle row and column
                    * add obstacle to obstacles
                    * cursor position is at obstacle position
                    * foreground is cyan
                    * write "="
                    * else keep moving:last position of snake elements is in deque
                    * cursor position is at last position column and row
                    * write " "
                    */
                    Position obstacle = new Position();
                    do
                    {
                        obstacle = new Position(randomIntGenerator.Next(0, Console.WindowHeight),
                                                randomIntGenerator.Next(0, Console.WindowWidth));
                    } while (snakeElements.Contains(obstacle) || obstacles.Contains(obstacle));
                    obstacles.Add(obstacle);
                    Console.SetCursorPosition(obstacle.column, obstacle.row);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("=");
                }
                else
                {
                    Position last = snakeElements.Dequeue();
                    Console.SetCursorPosition(last.column, last.row);
                    Console.Write(" ");
                }

                /*
                * if ticking time minus last time snake eaten greater or equal time food dissapearing
                * than increase negative points by 50
                * cursor is at food column and row
                * write ""
                * than do new food inside window height and width 
                * while snake elements and obstacles contain food
                * than last time food appeared will be equal to ticking time
                * set cursor position at food column and row
                * foreground is yellow
                *write "@"
                * sleep time minus 0.01
                * thread will sleep value of sleep time
                */
                if (Environment.TickCount - lastTimeSnakeEat >= lastTimeFoodDissapear)
                {
                    negativePoints = negativePoints + 50;
                    Console.SetCursorPosition(food.column, food.row);
                    Console.Write(" ");
                    do
                    {
                        food = new Position(randomIntGenerator.Next(0, Console.WindowHeight),
                                            randomIntGenerator.Next(0, Console.WindowWidth));
                    } while (snakeElements.Contains(food) || obstacles.Contains(food));
                    lastTimeFoodDissapear = Environment.TickCount;
                }

                Console.SetCursorPosition(food.column, food.row);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("@");

                sleepTime -= 0.01;

                Thread.Sleep((int)sleepTime);

            }
            //This is app was done as learning project using source code of
            //https://github.com/NikolayIT/CSharpConsoleGames/blob/master/Snake/Program.cs



        }
    }
}