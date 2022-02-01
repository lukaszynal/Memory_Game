using System;

namespace Memory_Game
{
    internal class Menu
    {
        public eGameMode Run(out string o_PlayerName1, out int o_Width, out int o_Height, out int o_Chances)
        {
            Console.WriteLine("Welcome to Memory Game!");
            Console.WriteLine("Please enter your name:");
            o_PlayerName1 = Console.ReadLine();

            Console.WriteLine("Hello {0}! Please choose a game mode:", o_PlayerName1);
            eGameMode desiredGameMode = GameMode();

            GetBoardSize(out o_Height, out o_Width, out o_Chances);

            return desiredGameMode;
        }

        public void GetBoardSize(out int o_Height, out int o_Width, out int o_Chances)
        {
            o_Height = 2;
            o_Width = 4;
            o_Chances = 10;


            Console.WriteLine("1) Easy");
            Console.WriteLine("2) Hard");

            string playerSelection = validateGameModeInput();

            if (playerSelection == "2")
            {
                o_Height = 4;
                o_Width = 4;
                o_Chances = 15;
            }
        }

        private eGameMode GameMode()
        {
            eGameMode GameMode = eGameMode.Solo;

            return GameMode;
        }

        private string validateGameModeInput()
        {
            string playerSelection = Console.ReadLine();

            while (playerSelection != "1" && playerSelection != "2")
            {
                Console.WriteLine("Wrong option entered. Please select option 1 or 2.");
                playerSelection = Console.ReadLine();
            }

            return playerSelection;
        }
    }
}