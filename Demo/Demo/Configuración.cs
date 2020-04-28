using Helper.Model;
using Helper.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Signature : Form
    {
        private static readonly List<IDataNode> listDataNode = new List<IDataNode>();
        private static Color colorLinks = Color.Blue;
        private static string assemblyName;
        private static string keyName = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private static string path = Assembly.GetExecutingAssembly().Location;
        private static string version;

        public Signature()
        {
            this.InitializeComponent();
            this.PublishVersion();
            this.LoadCheckeds();
            this.listView1.ColumnWidthChanging += new ColumnWidthChangingEventHandler(listView1_ColumnWidthChanging);
            this.panel1.Visible = true;
            this.panel2.Visible = true;
            this.panel3.Visible = false;
            this.CheckSSLCertificateInStores();
        }

        private void Configuración_Load(object sender, EventArgs e)
        {
            this.label1.Text = String.Format("      Version: {0}", version);
            this.notifyIcon.Text = assemblyName;
            this.Text = assemblyName;
        }

        public void PublishVersion()
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                assemblyName = Assembly.GetExecutingAssembly().GetName().Name.ToString();
                Version ver = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                version = string.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
            }
            else
            {
                assemblyName = "Not Published";
                version = "Not Published";
            }
        }

        private void CheckSSLCertificateInStores()
        {
            DualCheck dual = CertificateControl.CheckStores();
            if (dual.My.Equals(false) || dual.Root.Equals(false))
            {
                this.label8.Text = "False";
            }
            else
            {
                this.label8.Text = "True";
            }
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
                listView1.Refresh();
                LoadViewList();
            }
            if (panel2.Visible == false)
            {
                panel2.SendToBack();
                panel3.BringToFront();
                listView1.Refresh();
                LoadViewList();
            }
            panel1.Refresh();
            //Console.WriteLine("cambie la prop visible del panel");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Form_Hide(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
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
                string simpleName = cert.GetNameInfo(X509NameType.SimpleName, true);
                List<KeyValuePair<string, string>> kvs = cert.Subject.Split(',').Select(x => new KeyValuePair<string, string>(x.Split('=')[0], x.Split('=')[1])).ToList();
                string subject = kvs[0].Value;
                string friendlyName = cert.FriendlyName;
                string thumbprint = cert.Thumbprint;
                bool isValid = cert.Verify();
                IDataNode m = new DataNode(subject, friendlyName, thumbprint, isValid);
                listDataNode.Add(m);
            }

        }
        private void LoadViewList()
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

                listViewItem.Text = item.Subject;
                subItem_Thumbprint.Text = item.Thumbprint;
                subItem_flag.Font = new Font(listViewItem.Font, FontStyle.Bold);
                listViewItem.UseItemStyleForSubItems = false;

                if (item.isValid)
                {
                    subItem_flag.Text = "Valido";
                    subItem_flag.ForeColor = Color.Green;
                }
                else
                {
                    subItem_flag.Text = "No valido";
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
        private void LoadCheckeds()
        {
            using (RegistryKey registry = Registry.CurrentUser.OpenSubKey(keyName))
            {
                List<string> names = registry.GetValueNames().ToList();
                if (names.Contains(assemblyName).Equals(true))
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
        }

        private void button1_Leave(object sender, EventArgs e)
        {
            //Console.WriteLine("sali del boton");
        }
        private void button1_Enter(object sender, EventArgs e)
        {
            //Console.WriteLine("entré al boton");
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                //Console.WriteLine($"checkBox1 => True {checkBox1}");
                Registry.CurrentUser.CreateSubKey(keyName).SetValue(assemblyName, path, RegistryValueKind.String);
            }
            else
            {
                //Console.WriteLine($"checkBox1 => False {checkBox1}");
                Registry.CurrentUser.OpenSubKey(keyName, true).DeleteValue(assemblyName);
            }
        }
        void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = this.listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.google.com/maps/place/Fiscal%C3%ADa+de+Estado/" +
                "@-34.9149269,-57.9377341,17z/data=!3m1!4b1!4m5!3m4!1s0x95a2e619d798ea5f:0xaa78c0a6a7e5167!" +
                "8m2!3d-34.9149269!4d-57.9355454");
        }

        private void label6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www2.fepba.gov.ar/");
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Black;
        }

        private void label5_MouseMove(object sender, MouseEventArgs e)
        {
            label5.ForeColor = colorLinks;
        }

        ///
        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Black;
        }

        private void label6_MouseMove(object sender, MouseEventArgs e)
        {
            label6.ForeColor = colorLinks;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.CheckSSLCertificateInStores();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Program.ExecuteCertUtilCustom();
        }

    }

}
