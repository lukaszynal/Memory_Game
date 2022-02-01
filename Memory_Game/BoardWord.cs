namespace Memory_Game
{
    internal class BoardWord
    {
        private string m_Word;
        private bool m_IsHidden;

        public BoardWord(string i_Word, bool i_IsHidden)
        {
            m_Word = i_Word;
            m_IsHidden = i_IsHidden;
        }

        public string Word
        {
            get
            {
                return m_Word;
            }

            set
            {
                m_Word = value;
            }
        }

        public bool IsHidden
        {
            get
            {
                return m_IsHidden;
            }

            set
            {
                m_IsHidden = value;
            }
        }
    }
}