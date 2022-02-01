using System;
using System.Collections.Generic;

namespace Memory_Game
{
    internal class GameLogicManager
    {
        private static readonly Random sr_Random = new Random();
        private static eGameStates s_CurrentGameState = eGameStates.Menu;
        private readonly GameData r_GameData;
        private Cell m_CurrentUserSelection;
        private Cell m_PreviousUserSelection;
        private bool m_IsFirstSelection;
        private bool m_SelectionNotMatching;

        public static int GetRandomNumber(int i_RangeStart, int i_RangeEnd)
        {
            return sr_Random.Next(i_RangeStart, i_RangeEnd);
        }

        public GameLogicManager(Player i_Player1, int i_Width, int i_Height)
        {
            r_GameData = new GameData(i_Player1, i_Width, i_Height);
            r_GameData.InitializeBoardMatrix();
            s_CurrentGameState = eGameStates.Running;

            initializeLogicData();
        }

        public static eGameStates CurrentGameState
        {
            get
            {
                return s_CurrentGameState;
            }

            set
            {
                s_CurrentGameState = value;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return r_GameData.CurrentPlayer;
            }

            set
            {
                r_GameData.CurrentPlayer = value;
            }
        }

        public BoardWord[,] Words
        {
            get
            {
                return r_GameData.Words;
            }
        }

        public bool SelectionNotMatching
        {
            get
            {
                return m_SelectionNotMatching;
            }

            set
            {
                m_SelectionNotMatching = value;
            }
        }

        public int BoardWidth
        {
            get
            {
                return r_GameData.BoardWidth;
            }
        }

        public int BoardHeight
        {
            get
            {
                return r_GameData.BoardHeight;
            }
        }

        private void initializeLogicData()
        {
            m_SelectionNotMatching = false;
            m_IsFirstSelection = true;
            CurrentPlayer.Counter = 0;
            if(BoardHeight == 2)
            {
                CurrentPlayer.ChancesLeft = 10;
            }
            else
            {
                CurrentPlayer.ChancesLeft = 15;
            }           
        }

        public void UpdateData(Cell i_UserSelection)
        {
            if (!m_SelectionNotMatching)
            {
                updateNextTurn(i_UserSelection);
            }

            if (((r_GameData.PlayerOne.Counter) == (BoardWidth * BoardHeight) / 2) || r_GameData.PlayerOne.ChancesLeft == 0 )
            {
                s_CurrentGameState = eGameStates.GameOver;
            }
        }

        private void updateNextTurn(Cell i_UserSelection)
        {
            m_CurrentUserSelection = i_UserSelection;

            if (m_IsFirstSelection)
            {
                m_PreviousUserSelection = m_CurrentUserSelection;
                getBoardLetterAt(m_CurrentUserSelection).IsHidden = false;
                m_IsFirstSelection = false;
            }
            else
            {
                BoardWord firstSelectionLetter = getBoardLetterAt(m_PreviousUserSelection);
                BoardWord secondSelectionLetter = getBoardLetterAt(m_CurrentUserSelection);

                secondSelectionLetter.IsHidden = false;

                m_SelectionNotMatching = firstSelectionLetter.Word != secondSelectionLetter.Word;

                if (!m_SelectionNotMatching)
                {
                    CurrentPlayer.Counter++;
                }
                else 
                {
                    CurrentPlayer.ChancesLeft--;
                }

                m_IsFirstSelection = true;
            }
        }

        public string GetScoreboard()
        {
            if (!((r_GameData.PlayerOne.Counter) == (BoardWidth * BoardHeight) / 2) && r_GameData.PlayerOne.ChancesLeft == 0)
            {
                return string.Format(
                "You didn't solve the memory game. Try again!");
            }
            else
            {
                return string.Format(
                "You solved the memory game and you still got {0} chances/points.", 
                 r_GameData.PlayerOne.ChancesLeft);
            }        
        }

        public void PlayerSelections()
        {
            CurrentPlayer = r_GameData.PlayerOne;

            getBoardLetterAt(m_CurrentUserSelection).IsHidden = true;
            getBoardLetterAt(m_PreviousUserSelection).IsHidden = true;
            m_SelectionNotMatching = false;
        }

        private BoardWord getBoardLetterAt(Cell i_CellLocation)
        {
            return Words[i_CellLocation.Row, i_CellLocation.Column];
        }

        public string GetGameOverStatus()
        {
            Player playerOne = r_GameData.PlayerOne;
            string gameResult;

            gameResult = getGameResultText(playerOne.PlayerName);

            return gameResult;
        }

        private string getGameResultText(string i_PlayerName)
        {
            string gameResultText;

            gameResultText = string.Format(
            "{0}, It's over!{1}{2}",
            i_PlayerName,
            Environment.NewLine,
            GetScoreboard());

            return gameResultText;
        }
        public void ResetRound(int i_Height, int i_Width)
        {
            r_GameData.BoardHeight = i_Height;
            r_GameData.BoardWidth = i_Width;

            r_GameData.Words = new BoardWord[i_Height, i_Width];
            r_GameData.InitializeBoardMatrix();

            initializeLogicData();

            s_CurrentGameState = eGameStates.Running;
        }
    }
}