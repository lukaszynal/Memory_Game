namespace Memory_Game
{
    internal class Player
    {
        private string m_PlayerName;
        private int m_Counter;
        private int m_ChancesLeft;

        public Player(string i_PlayerName, int i_ChancesLeft)
        {
            m_PlayerName = i_PlayerName;
            m_Counter = 0;
            m_ChancesLeft = i_ChancesLeft;
        }

        public string PlayerName
        {
            get
            {
                return m_PlayerName;
            }

            set
            {
                m_PlayerName = value;
            }
        }

        public int Counter
        {
            get
            {
                return m_Counter;
            }

            set
            {
                m_Counter = value;
            }
        }

        public int ChancesLeft
        {
            get
            {
                return m_ChancesLeft;
            }

            set
            {
                m_ChancesLeft = value;
            }
        }
    }
}