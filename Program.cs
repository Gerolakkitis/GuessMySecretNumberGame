using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace GuessMe
{


    class GetGameInfo
    {
        public int secretNum { get; set; }
        public int minNum { get; set; }
        public int maxNum { get; set; }
        public string difficultyChosen { get; set; }

        public GetGameInfo(int secNum, int min, int max, string playerDifficulty)
        {
            secretNum = secNum;
            minNum = min;
            maxNum = max;
            difficultyChosen = playerDifficulty;
        }

    }

    //TODO -- ADD TIME
    class Program
    {
        static void Main(string[] args)
        {
            Random randomNum = new Random();
            string userName;
            int difficulty;
            int userGuess;
            bool gameOver = false;
            int gameStartingSec = 3;

            


            userName = GetUserName();
            difficulty=GetDifficulty();

            GetGameInfo myGame = GetSecretNumber(difficulty);
            Console.WriteLine("\n\n\nGame Starting in:\n");
            for (int i=gameStartingSec; i>0; i--)
            {
                Console.WriteLine("{0}", i);
                Thread.Sleep(1000);
            }

            Console.Clear();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("I've chosen a number between {0} and {1}\nBetcha can't guess it!\n", myGame.minNum, myGame.maxNum);
            while (!gameOver)
            {
                userGuess = GetUserGuess(myGame, userName);
                if (userGuess == myGame.secretNum)
                {
                    Console.Clear();
                    stopwatch.Stop();
                    TimeSpan timeElapsed = stopwatch.Elapsed;
                    PrintGameOver(timeElapsed, userName);

                    gameOver = true;
                }
                else
                {
                    
                    PrintInfoAboutSecretNum(userGuess, myGame);
                }
            }
        }

        //get user name and introduce the game
        static string GetUserName()
        {
            string playerName;
            Console.WriteLine("GUESS MY NUMBER GAME\n");
            Console.WriteLine("Hello, stranger!");
            Console.WriteLine("In this game, you have to guess my secret number");
            Console.Write("What is your name?  ");
            playerName = Console.ReadLine();
            Console.Clear();
            Console.WriteLine($"Good to meet you {playerName}");
            return playerName;
        }

        //Print info about the user's guess that will help them guess the correct number
        static void PrintInfoAboutSecretNum(int userGuess, GetGameInfo gameInfo)
        {
            if (userGuess < gameInfo.secretNum)
            {
                Console.WriteLine("{0}? Ha, nice try - too low! \n", userGuess);
            }
            else
            {
                Console.WriteLine("{0}? Too bad, way too high! \n", userGuess);
            }
            Thread.Sleep(750);
        }

        static List<int> keepTrackOfInput = new List<int>();

        //Get user guess, verify it was not entered more than once. Add guess to the keep track of input list
        static int GetUserGuess(GetGameInfo gameInfo, string playerName)
        {
            int guess = GetUserInputGuess(gameInfo, playerName);

            //check if input was already entered
            while (keepTrackOfInput.Contains(guess))
            {
                Console.Clear();
                Console.WriteLine("You have already entered {0}. Please try again\n", guess);
                guess = GetUserInputGuess(gameInfo, playerName);
            }

            keepTrackOfInput.Add(guess);
            Console.Clear();
            return guess;
        }

        //Print the list of all previous guesses of the user
        static void PrintPreviousGuesses()
        {
            if (keepTrackOfInput.Count > 0)
            {
                Console.Write("Your previous guesses are:  ");
                foreach (int i in keepTrackOfInput)
                {
                    Console.Write(i + ", ");
                }
                Console.WriteLine("\n");
            }
        }

        //Get user input for their guess. Validate user input
        static int GetUserInputGuess(GetGameInfo gameInfo, string playerName)
        {
            string userInput;
            int userGuess;
            Console.WriteLine("Make a guess {0}!!!!! ", playerName);
            PrintPreviousGuesses();
            userInput = Console.ReadLine();
            userGuess = ConvToDigit(userInput);
            userGuess = CheckIfDigitWithinRange(userGuess, gameInfo.minNum, gameInfo.maxNum);
            return userGuess;
        }

        //Put information in GetGameInfo (secret num, min-max acceptable range, get difficulty string)
        static GetGameInfo GetSecretNumber(int difLevel)
        {
            int secretNum;
            int min = 0;
            int max;
            string difficulty;
            switch (difLevel)
            {

                case 1:
                    secretNum=GetRandomNum(11);
                    max = 10;
                    difficulty = "Beginner";
                    break;
                case 2:
                    secretNum=GetRandomNum(101);
                    max = 750;
                    difficulty = "Intermediate";
                    break;
                case 3:
                    secretNum=GetRandomNum(1001);
                    max = 5000;
                    difficulty = "Advanced";
                    break;
                default:
                    secretNum = 0;
                    max = 10;
                    difficulty = "Error in GetSecretNumber";
                    break;         
            }
            GetGameInfo game = new GetGameInfo(secretNum, min, max, difficulty);
            Console.WriteLine("\nYou have chosen {0}", difficulty);
            return game;
        }

        //Get random number based on difficulty
        static int GetRandomNum(int num)
        {
            Random range = new Random();
            int randomNum = range.Next(num);

            return randomNum;
        }

        //print difficulty selected and validate user input
        static int GetDifficulty()
        {
            string userInput;
            int userDifficulty;
            
            Console.WriteLine("\nChoose difficulty:");
            Console.WriteLine("1.Beginner (0-10)\n2.Intermediate (0-750)\n3.Advanced (0-5000)");
            Console.Write("\nEnter 1, 2, or 3:  ");
            userInput = Console.ReadLine();
            userDifficulty = ConvToDigit(userInput);
            userDifficulty = CheckIfDigitWithinRange(userDifficulty, 1, 3);
            
            return userDifficulty;
        }

        //check if user input is valid
        static int ConvToDigit(string input)
        {
            int num;
            while(!Int32.TryParse(input, out num)){
                Console.WriteLine($"\nInvalid input : {input}");
                Console.Write("Enter a valid number:  ");
                input = Console.ReadLine();
            }
            return num;
        }

        //Check if digit is between the number range determined by difficulty
        static int CheckIfDigitWithinRange(int userNum, int minNum, int maxNum)
        {
           string userInput;
           while (!(userNum>= minNum && userNum <= maxNum))
            {
                Console.WriteLine("\nInput out of range");
                Console.WriteLine("Acceptable range is {0} to {1}", minNum, maxNum);
                Console.Write("\nEnter a number: ", minNum, maxNum);
                userInput = Console.ReadLine();
                userNum = ConvToDigit(userInput);
            }
            return userNum;
        }

        //Print Game over and time elapsed since game began
        static void PrintGameOver(TimeSpan timeElapsed, string playerName)
        {
            Console.WriteLine("====================================");
            Console.WriteLine("====================================");
            Console.WriteLine("CONGRATULATIONS {0} YOU WON THE GAME", playerName);
            Console.WriteLine("  Total time elapsed: {0} seconds  ", timeElapsed.Seconds);
            Console.WriteLine("    Total amount of guesses: {0}    ", keepTrackOfInput.Count);
            Console.WriteLine("====================================");
            Console.WriteLine("====================================");
        }

    }
}
