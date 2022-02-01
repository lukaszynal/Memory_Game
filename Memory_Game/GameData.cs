using System.Collections.Generic;

namespace Memory_Game
{
    internal class GameData
    {
        private readonly Player r_PlayerOne;
        private Player m_CurrentPlayer;
        private BoardWord[,] m_Words;
        private int m_BoardHeight;
        private int m_BoardWidth;

        public GameData(Player i_PlayerOne, int i_BoardWidth, int i_BoardHeight)
        {
            r_PlayerOne = i_PlayerOne;
            m_BoardWidth = i_BoardWidth;
            m_BoardHeight = i_BoardHeight;

            m_CurrentPlayer = r_PlayerOne;
            m_Words = new BoardWord[BoardHeight, BoardWidth];
        }

        public BoardWord[,] Words
        {
            get
            {
                return m_Words;
            }

            set
            {
                m_Words = value;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }

            set
            {
                m_CurrentPlayer = value;
            }
        }

        public int BoardHeight
        {
            get
            {
                return m_BoardHeight;
            }

            set
            {
                m_BoardHeight = value;
            }
        }

        public int BoardWidth
        {
            get
            {
                return m_BoardWidth;
            }

            set
            {
                m_BoardWidth = value;
            }
        }

        public Player PlayerOne
        {
            get
            {
                return r_PlayerOne;
            }
        }
        public void InitializeBoardMatrix()
        {
            string fileName = "Words.txt";
            var workingDirectory = System.Environment.CurrentDirectory;
            var path = $"{workingDirectory}\\{fileName}";

            if (FileExists(path))
            {
                string[] boardWords = initializeBoardWords(path);
                List<Cell> randomCells = getRandomCellsList();

                foreach (string letter in boardWords)
                {
                    int randomSelection = GameLogicManager.GetRandomNumber(0, randomCells.Count);
                    Cell firstCell = randomCells[randomSelection];

                    randomCells.Remove(firstCell);
                    randomSelection = GameLogicManager.GetRandomNumber(0, randomCells.Count);

                    Cell secondCell = randomCells[randomSelection];

                    randomCells.Remove(secondCell);

                    Words[firstCell.Row, firstCell.Column] = new BoardWord(letter, true);
                    Words[secondCell.Row, secondCell.Column] = new BoardWord(letter, true);
                }
            }
            else
            {
                System.Console.WriteLine("!! File \"Words.txt\" not found !!");
                System.Console.WriteLine("This file must be in the same directory as the application");
                System.Console.WriteLine("Goodbye!");
                System.Threading.Thread.Sleep(4000);
                System.Environment.Exit(0);
            }
        }

        private bool FileExists(string path)
        {
            return System.IO.File.Exists(path);
        }
        private List<Cell> getRandomCellsList()
        {
            List<Cell> randomCells = new List<Cell>(m_BoardHeight * m_BoardWidth);

            for (int i = 0; i < m_BoardHeight; i++)
            {
                for (int j = 0; j < m_BoardWidth; j++)
                {
                    randomCells.Add(new Cell(i, j));
                }
            }

            return randomCells;
        }

        private string[] initializeBoardWords(string path)
        {
            string[] lines = new string[m_BoardHeight * m_BoardWidth / 2];
            string[] alllines = System.IO.File.ReadAllLines(path);
            List<int> randomList = new List<int>();

            System.Random rnd = new System.Random();

            for (int i = 0; i < lines.Length; i++)
            {
                int randomLine = rnd.Next(0, alllines.Length);
                if (!randomList.Contains(randomLine))
                {
                    randomList.Add(randomLine);
                    lines[i] = alllines[randomLine];
                }
                else
                {
                    --i;
                    continue;
                }
                    
                int charsInLine = lines[i].Length;
                int requiredLength = 14;
                if (charsInLine < requiredLength)
                {
                    for (int j = 0; j < (requiredLength - charsInLine); j++)
                    {
                        lines[i] = lines[i].Insert(lines[i].Length, " ");
                    }
                }
            }
            return lines;
        }
    }
}