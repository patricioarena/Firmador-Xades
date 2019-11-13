using Demo.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Signature : Form
    {
        private readonly List<IDataNode> listDataNode = new List<IDataNode>();
        private static string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public Signature()
        {
            InitializeComponent();
            panel1.Visible = true;
            panel2.Visible = true;
            panel3.Visible = false;
        }

        private void Configuración_Load(object sender, EventArgs e)
        {
            label1.Text = String.Format("     Version: {0}", assemblyVersion);
        }

        private void ListView1MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listView = (ListView)sender;
            ListViewItem item = listView.FocusedItem;
            ViewCert(item);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = !panel2.Visible;
            panel3.Visible = !panel3.Visible;
            
            if (panel2.Visible == true)
            {
                panel3.SendToBack();
                panel2.BringToFront();
                //LoadViewList();
            }
            if (panel2.Visible == false)
            {
                panel2.SendToBack();
                panel3.BringToFront();
                LoadViewList();
            }
            panel1.Refresh();
            Console.WriteLine("cambie la prop visible del panel");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();

            //notifyIcon.Visible = true;
            //this.MessagesOnChange();
            //this.Hide();
        }

        private void Form_Hide(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                MessagesOnChange();
                Hide();
            }
        }

        private void Form_Show(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                ListView listView = (ListView)sender;
                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (listView.SelectedIndices[0] == i)
                    {
                        ListViewItem item = listView.SelectedItems[0];
                        ViewCert(item);
                    }
                }

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

        private void LoadViewList()
        {
            listDataNode.Clear();
            ObtainModel();
            listView1.Items.Clear();
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

        private void ViewCert(ListViewItem item)
        {
            String Thumbprint = item.SubItems[2].Text;
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2 certificate = collection.OfType<X509Certificate2>().Where(cert => cert.Thumbprint == Thumbprint).FirstOrDefault();
            X509Certificate2UI.DisplayCertificate(certificate);
        }

        private void MessagesOnChange()
        {
            //if (Network.StatusPrincipalInterface().Equals(true))
            //{
            //    this.notifyIcon.BalloonTipText = "« Adaptador de Red: Habilitado »";
            //    this.notifyIcon.Text = "« Adaptador de Red: Habilitado »";
            //    notifyIcon.ShowBalloonTip(500);

            //}
            //else
            //{
            //    this.notifyIcon.BalloonTipText = "« Adaptador de Red: Deshabilitado »";
            //    this.notifyIcon.Text = "« Adaptador de Red: Deshabilitado »";
            //    notifyIcon.ShowBalloonTip(500);
            //}
        }

        private void button1_Leave(object sender, EventArgs e)
        {
            Console.WriteLine("sali del boton");
        }

        private void button1_Enter(object sender, EventArgs e)
        {
            Console.WriteLine("entré al boton");
        }
    }

}
