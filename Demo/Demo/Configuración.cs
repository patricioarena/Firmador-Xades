using Demo.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Signature : Form
    {
        private readonly List<IDataNode> listDataNode = new List<IDataNode>();

        public Signature()
        {
            InitializeComponent();
        }

        private void Configuración_Load(object sender, EventArgs e)
        {

        }


        private void ListView1MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void ListView1MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView lv = (ListView)sender;
            ListViewItem lvi = lv.GetItemAt(e.X, e.Y);
            String Thumbprint = lv.SelectedItems[0].SubItems[2].Text;

            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2 certificate = collection.OfType<X509Certificate2>().Where(cert => cert.Thumbprint == Thumbprint).FirstOrDefault();
            X509Certificate2UI.DisplayCertificate(certificate);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listDataNode.Clear();
            ObtainModel();
            listView1.Items.Clear();
            listView1.Refresh();
            for (int i = 0; i < listDataNode.Count; i++)
            {
                IDataNode item = listDataNode[i];
                ListViewItem listViewItem = new ListViewItem();
                ListViewItem.ListViewSubItem subItem_flag = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subItem_Thumbprint = new ListViewItem.ListViewSubItem();
                
                listViewItem.Text = item.Name;
                subItem_Thumbprint.Text = item.Thumbprint;
                subItem_flag.Font = new Font(listViewItem.Font, FontStyle.Bold);
                listViewItem.UseItemStyleForSubItems = false;

                if (item.isValid)
                {
                    subItem_flag.Text = "Valid";
                    subItem_flag.ForeColor = Color.Green;
                }
                else
                {
                    subItem_flag.Text = "Invalid";
                    subItem_flag.ForeColor = Color.Red;
                }

                listView1.Items.Add(listViewItem);
                listViewItem.SubItems.Add(subItem_flag);
                listViewItem.SubItems.Add(subItem_Thumbprint);
            }
        }

        private void ObtainModel()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            collection.OfType<X509Certificate2>().ToList();

            foreach (X509Certificate2 cert in collection)
            {
                string friendlyName = cert.FriendlyName;
                string simpleName = cert.GetNameInfo(X509NameType.SimpleName, true);
                string thumbprint = cert.Thumbprint;
                bool isValid = cert.Verify();
                IDataNode m = new DataNode(friendlyName, simpleName, thumbprint, isValid);
                listDataNode.Add(m);
            }

        }


    }

}
