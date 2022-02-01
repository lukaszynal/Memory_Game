﻿using System;
using System.Diagnostics;
using System.Text;

namespace Memory_Game
{
    internal class GameUIManager
    {
        private readonly Menu r_Menu;
        private GameLogicManager m_GameLogicManager;

        public static int GetNumberInRange(int i_RangeStart, int i_RangeEnd)
        {
            int userInput = i_RangeStart;
            bool isNumber = false;
            bool isWithinRange = false;

            while (!isNumber || !isWithinRange)
            {
                Console.WriteLine("Please enter a value (must be between {0} and {1}):", i_RangeStart, i_RangeEnd);
                isNumber = int.TryParse(Console.ReadLine(), out userInput);

                if (isNumber)
                {
                    isWithinRange = userInput >= i_RangeStart && userInput <= i_RangeEnd;
                }

                if (!isNumber || !isWithinRange)
                {
                    Console.WriteLine("Incorrect value");
                }
            }

            return userInput;
        }

        public GameUIManager()
        {
            r_Menu = new Menu();
        }

        public void StartGame()
        {
            if (GameLogicManager.CurrentGameState == eGameStates.Menu)
            {
                runMenu();
            }
            runGame();
            gameOver();
        }

        private void runGame()
        {
            while (GameLogicManager.CurrentGameState == eGameStates.Running)
            {
                DrawData();
                string playerInput = getPlayerInput();
                sendInputAndUpdateUI(playerInput);
            }
        }

        private string getPlayerInput()
        {
            string playerInput;

            playerInput = GetInput();

            return playerInput;
        }

        private void sendInputAndUpdateUI(string i_PlayerInput)
        {
            if (i_PlayerInput == "Q")
            {
                stopGame();
            }
            else
            {
                m_GameLogicManager.UpdateData(Cell.Parse(i_PlayerInput));

                if (m_GameLogicManager.SelectionNotMatching)
                {
                    DrawData();
                    DrawText("Mismatch, remember this!");

                    System.Threading.Thread.Sleep(2000);

                    m_GameLogicManager.PlayerSelections();
                }
            }
        }

        public void stopGame()
        {
            DrawText("Goodbye!");
            System.Threading.Thread.Sleep(2000);
            Environment.Exit(0);
        }

        private void gameOver()
        {
            DrawData();

            DrawText(m_GameLogicManager.GetGameOverStatus());

            bool restartNeeded = CheckRestart();

            if (restartNeeded)
            {
                ClearWindow();
                restartGame();
            }
            else
            {
                stopGame();
            }
        }

        private void restartGame()
        {
            r_Menu.GetBoardSize(out int height, out int width, out int o_Chances);
            m_GameLogicManager.ResetRound(height, width);
            StartGame();
        }

        private void runMenu()
        {
            string playerName1;
            int width, height;
            int chances;

            eGameMode desiredGameMode = r_Menu.Run(out playerName1, out width, out height, out chances);

            Player playerOne = new Player(playerName1, chances);

            m_GameLogicManager = new GameLogicManager(playerOne, width, height);
        }

        public void DrawData()
        {
            int amountOfEqualSigns = (m_GameLogicManager.BoardWidth * 17) + 1;
            string equalLine = string.Format("  {0}", new string('=', amountOfEqualSigns));

            ClearWindow();
            Console.WriteLine(m_GameLogicManager.CurrentPlayer.PlayerName + ", your chances left: " + m_GameLogicManager.CurrentPlayer.ChancesLeft);
            Console.WriteLine();

            drawTopLetterRow(m_GameLogicManager.BoardWidth);

            Console.WriteLine(equalLine);

            for (int i = 0; i < m_GameLogicManager.BoardHeight; i++)
            {
                drawRowAtIndex(i);
                Console.WriteLine(equalLine);
            }

            Console.WriteLine();
        }

        private bool validateUserCellSelection(string i_UserInput)
        {
            bool isValidInput = true;

            if (i_UserInput != null)
            {
                i_UserInput = i_UserInput.ToUpper();

                if (i_UserInput != "Q")
                {
                    isValidInput = validateCellInputSelection(i_UserInput);

                    if (isValidInput)
                    {
                        isValidInput = checkCellNotAlreadyRevealed(i_UserInput);
                    }
                }
            }
            else
            {
                Console.WriteLine("Input must not be empty");
                isValidInput = false;
            }

            return isValidInput;
        }

        private bool validateCellInputSelection(string i_UserCellInput)
        {
            bool isValidCell;

            if (i_UserCellInput.Length != 2)
            {
                Console.WriteLine("Input must have exactly 2 characters");
                isValidCell = false;
            }
            else
            {
                char letter = i_UserCellInput[0];
                char digit = i_UserCellInput[1];

                isValidCell = checkIfLetterInRange(letter) && checkIfDigitInRange(digit);
            }

            return isValidCell;
        }

        private bool checkIfLetterInRange(char i_Letter)
        {
            char maxAllowedLetter = (char)('A' + m_GameLogicManager.BoardWidth - 1);
            bool isValidLetter = true;

            if (i_Letter < 'A' || i_Letter > maxAllowedLetter)
            {
                Console.WriteLine(
                    "Invalid input, first character must be a character between A-{0}",
                    maxAllowedLetter);
                isValidLetter = false;
            }

            return isValidLetter;
        }

        private bool checkIfDigitInRange(char i_Digit)
        {
            char maxAllowedDigit = (char)('0' + m_GameLogicManager.BoardHeight);
            bool isValidDigit = true;

            if (i_Digit < '1' || i_Digit > maxAllowedDigit)
            {
                Console.WriteLine(
                    "Invalid input, second character must be a digit between 1-{0}",
                    maxAllowedDigit);
                isValidDigit = false;
            }

            return isValidDigit;
        }

        private bool checkCellNotAlreadyRevealed(string i_UserInput)
        {
            int column = i_UserInput[0] - 'A';
            int row = i_UserInput[1] - '1';

            bool isValidInput = m_GameLogicManager.Words[row, column].IsHidden;

            if (!isValidInput)
            {
                Console.WriteLine("Cell already revealed");
            }

            return isValidInput;
        }

        private void drawTopLetterRow(int i_LengthOfRow)
        {
            StringBuilder topRowToPrint = new StringBuilder("  ");

            for (int i = 0; i < i_LengthOfRow; i++)
            {
                topRowToPrint.Append(string.Format("        {0}        ", (char)(i + 'A')));
            }

            Console.WriteLine(topRowToPrint.ToString());
        }

        private void drawRowAtIndex(int i_Index)
        {
            string beginningOfRow = string.Format("{0} |", i_Index + 1);

            Console.Write(beginningOfRow);

            for (int j = 0; j < m_GameLogicManager.BoardWidth; j++)
            {
                BoardWord currentBoardLetter = m_GameLogicManager.Words[i_Index, j];
                string CellToProint = string.Format(" {0} |", currentBoardLetter.IsHidden ? "              " : currentBoardLetter.Word);

                Console.Write(CellToProint);
            }

            Console.WriteLine();
        }

        public string GetInput()
        {
            string userInput = string.Empty;
            bool isValidInput = false;

            while (!isValidInput)
            {
                Console.WriteLine("{0}, please choose a cell to reveal:", m_GameLogicManager.CurrentPlayer.PlayerName);

                userInput = Console.ReadLine();
                isValidInput = validateUserCellSelection(userInput);
            }

            return userInput.ToUpper();
        }

        public void DrawText(string i_TextToDraw)
        {
            Console.WriteLine(i_TextToDraw);
        }

        public bool CheckRestart()
        {
            Console.WriteLine();

            char userInput;

            Console.WriteLine("Another round? (Y/N)");

            bool isValid = char.TryParse(Console.ReadLine(), out userInput);

            if (isValid)
            {
                userInput = char.ToUpper(userInput);
                isValid = userInput == 'Y' || userInput == 'N';
            }

            while (!isValid)
            {
                Console.WriteLine("Invalid input, please enter Y/N");
                Console.WriteLine("Another round? (Y/N)");

                isValid = char.TryParse(Console.ReadLine(), out userInput);

                if (isValid)
                {
                    userInput = char.ToUpper(userInput);
                    isValid = userInput == 'Y' || userInput == 'N';
                }
            }

            return userInput == 'Y';
        }

        public void ClearWindow()
        {
            Console.Clear();
        }
    }
}