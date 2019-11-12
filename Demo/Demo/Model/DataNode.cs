using System.Collections.Generic;

namespace Demo.Model
{

    public interface IDataNode
    {
        string Name { get; set; }
        bool isValid { get; set; }
        string SimpleName { get; set; }
        string Thumbprint { get; set; }
    }


    public class DataNode : IDataNode
    {
        private string m_Name;
        private string m_SimpleName;
        private string m_Thumbprint;
        private bool m_isValid;



        public DataNode(string nam, string cmt, string thumbprint, bool stat)
        {
            m_Name = nam;
            m_SimpleName = cmt;
            m_isValid = stat;
            m_Thumbprint = thumbprint;
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
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

        public string SimpleName
        {
            get { return m_SimpleName; }
            set { m_SimpleName = value; }
        }
    }
}