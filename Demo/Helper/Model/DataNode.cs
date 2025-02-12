namespace Helper.Model
{

    public interface IDataNode
    {
        string Subject { get; set; }
        string FriendlyName { get; set; }
        string Thumbprint { get; set; }
        bool isValid { get; set; }

    }


    public class DataNode : IDataNode
    {
        private string m_subject;
        private string m_friendlyName;
        private string m_Thumbprint;
        private bool m_isValid;
        
        public DataNode(string subjec, string friendlyName, string thumbprint, bool isValid)
        {
            m_subject = subjec;
            m_friendlyName = friendlyName;
            m_Thumbprint = thumbprint;
            m_isValid = isValid;
        }

        public string Subject
        {
            get { return m_subject; }
            set { m_subject = value; }
        }

        public string Thumbprint
        {
            get { return m_Thumbprint; }
            set { m_Thumbprint = value; }
        }

        public bool isValid
        {
            get { return m_isValid; }
            set { m_isValid = value; }
        }

        public string FriendlyName
        {
            get { return m_friendlyName; }
            set { m_friendlyName = value; }
        }
    }
}