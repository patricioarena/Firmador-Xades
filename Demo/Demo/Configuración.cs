using Demo.Properties;
using Helper.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Windows.Forms;

namespace Demo
{
    public partial class Signature : Form
    {
        private static List<IDataNode> _listDataNode = new List<IDataNode>();

        private static Color _colorLinks = Color.Blue;

        private static string _applicationName;

        private static string _keyName = @"Software\Microsoft\Windows\CurrentVersion\Run";

        private static string _path = AppDomain.CurrentDomain.BaseDirectory;

        private static string appExe = Path.Combine(_path, "Authentica.exe");

        private const string RegistryPath = @"Software\AuthenticaApp";

        private const string FirstRunKey = "FirstRun";

        private const string StartupKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        private const string AppName = "Authentica"; // Nombre de la aplicación en el Registro

        private static string _version;

        private static string _mapsUrl = Settings.Default.FiscaliaEnGoogleMAps;

        private static string _fiscaliaWeb = Settings.Default.FiscaliaWeb;

        private static string _reason = Settings.Default.Reason;

        private static string _country = Settings.Default.Country;

        private static Signature _instance;

        private PictureBox _pictureBox;

        public Signature()
        {
            this.InitializeComponent();

            if (IsFirstRun())
            {
                RegisterStartup();
            }

            var position = rotatedLabel1.Parent.PointToScreen(rotatedLabel1.Location);
            position = pictureBox12.PointToClient(position);
            rotatedLabel1.Parent = pictureBox12;
            rotatedLabel1.Location = position;
            rotatedLabel1.BackColor = Color.Transparent;
            this.PublishVersion();
            LoadCheckeds();
            this.listView1.ColumnWidthChanging += new ColumnWidthChangingEventHandler(listView1_ColumnWidthChanging);
            this.panel7.Visible = true;
            this.panel2.Visible = true;
            this.panel3.Visible = false;
        }

        public static Signature GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Signature();
            }
            return _instance;
        }

        private void pictureBox11_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Image image = pictureBox11.Image;
            float angle = -45; // Ángulo de rotación deseado

            // Calcula el centro de rotación
            // Centro horizontal del PictureBox
            float startX = -60f;
            // Posición Y en el margen inferior horizontal
            float startY = pictureBox11.Height + 3f + image.Height;

            // Realiza la transformación de rotación
            g.TranslateTransform(startX, startY);
            g.RotateTransform(angle);
            g.TranslateTransform(-startX, -startY);

            // Dibuja la imagen rotada dentro del PictureBox
            g.DrawImage(image, 0, startY);

            // Restaura la transformación
            g.ResetTransform();
        }

        private void Configuración_Load(object sender, EventArgs e)
        {
            this.labelVersion.Text = String.Format("{0}", _version);
            this.rotatedLabel1.Text = String.Format("{0}", _version);
            this.notifyIcon.Text = _applicationName;
            this.Text = _applicationName;
        }

        public void PublishVersion()
        {
#if DEBUG
            _applicationName = Settings.Default.ApplicationName;
            _version = "Not Published";
#else
            _applicationName = Properties.Settings.Default.ApplicationName;
            _version = Properties.Settings.Default.Version;
#endif
        }

        private void ListView1MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listView = (ListView)sender;
            ListViewItem item = listView.FocusedItem;
            ViewCert(item);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(panel2.Visible);
            Console.WriteLine(panel7.Visible);

            panel3.Visible = !panel3.Visible;
            panel7.Visible = !panel7.Visible;

            if (panel3.Visible == true)
            {
                panel7.SendToBack();
                panel2.SendToBack();
                panel3.BringToFront();
                listView1.Refresh();
                LoadViewList();
            }
            if (panel3.Visible == false)
            {
                panel3.SendToBack();
                panel2.SendToBack();
                panel7.BringToFront();
                listView1.Refresh();
                LoadViewList();
            }
            panel7.Refresh();
            Console.WriteLine("cambie la prop visible del panel");
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
                _listDataNode.Add(m);
            }
        }
        private void LoadViewList()
        {
            _listDataNode.Clear();
            ObtainModel();
            listView1.Items.Clear();
            listView1.Refresh();
            for (int i = 0; i < _listDataNode.Count; i++)
            {
                IDataNode item = _listDataNode[i];
                ListViewItem listViewItem = new ListViewItem();
                ListViewItem.ListViewSubItem subItemFlag = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subItemThumbprint = new ListViewItem.ListViewSubItem();

                listViewItem.Text = item.Subject;
                subItemThumbprint.Text = item.Thumbprint;
                subItemFlag.Font = new Font(listViewItem.Font, FontStyle.Bold);
                listViewItem.UseItemStyleForSubItems = false;

                if (item.isValid)
                {
                    subItemFlag.Text = "Valido";
                    subItemFlag.ForeColor = Color.Green;
                }
                else
                {
                    subItemFlag.Text = "No valido";
                    subItemFlag.ForeColor = Color.Red;
                }

                listView1.Items.Add(listViewItem);
                listViewItem.SubItems.Add(subItemFlag);
                listViewItem.SubItems.Add(subItemThumbprint);
            }
        }
        private void ViewCert(ListViewItem item)
        {
            String thumbprint = item.SubItems[2].Text;
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2 certificate = collection.OfType<X509Certificate2>().Where(cert => cert.Thumbprint == thumbprint).FirstOrDefault();
            X509Certificate2UI.DisplayCertificate(certificate);
        }
        private void LoadCheckeds()
        {
            using (RegistryKey registry = Registry.CurrentUser.OpenSubKey(_keyName))
            {
                List<string> names = registry.GetValueNames().ToList();
                if (names.Contains(_applicationName).Equals(true))
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
                Registry.CurrentUser.CreateSubKey(_keyName).SetValue(_applicationName, $"\"{appExe}\"", RegistryValueKind.String);
            }
            else
            {
                Registry.CurrentUser.OpenSubKey(_keyName, true).DeleteValue(_applicationName);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Program._ontiChecked = !Program._ontiChecked;
        }

        void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = this.listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Process.Start(_mapsUrl);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            //Process.Start(_fiscaliaWeb);
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _fiscaliaWeb,
                    UseShellExecute = true // Required in .NET 5+ to open a URL
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening URL: {ex.Message}");
            }
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Black;
        }

        private void label6_MouseMove(object sender, MouseEventArgs e)
        {
            label6.ForeColor = _colorLinks;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void customButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
        }

        private void customButton5_Click(object sender, EventArgs e)
        {
            panel2.Visible = !panel2.Visible;

            if (panel2.Visible == true)
            {
                panel3.SendToBack();
                panel2.BringToFront();
                panel7.BringToFront();
                listView1.Refresh();
                LoadViewList();
            }
            if (panel2.Visible == false)
            {
                panel2.SendToBack();
                panel7.SendToBack();
                panel3.SendToBack();
                listView1.Refresh();
                LoadViewList();
            }
            //Console.WriteLine("cambie la prop visible del panel");
        }

        private void customButton6_Click(object sender, EventArgs e)
        {
            panel2.Visible = !panel2.Visible;
            panel2.Refresh();
            //Console.WriteLine("cambie la prop visible del panel");
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void customButton7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel7_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel7_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void rotatedLabel1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackgroundImage =
                (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("boton_certificados_hover");
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackgroundImage =
                (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject("boton_certificados");
        }

        private void CreateSubKeyFirstRun()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_keyName, true))
            {
                if (key != null && key.GetValue(_applicationName) != null)
                {
                    key.DeleteValue(_applicationName, false);
                }
            }

            if (checkBox1.Checked)
            {
                Registry.CurrentUser.CreateSubKey(_keyName).SetValue(_applicationName, $"\"{appExe}\"", RegistryValueKind.String);
            }
        }

        private bool IsFirstRun()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath, true))
            {
                if (key == null || key.GetValue(FirstRunKey) == null)
                {
                    using (RegistryKey newKey = Registry.CurrentUser.CreateSubKey(RegistryPath))
                    {
                        newKey.SetValue(FirstRunKey, "1", RegistryValueKind.String);
                    }
                    return true;
                }
            }
            return false;
        }

        private void RegisterStartup()
        {
            string appPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Authentica.exe");

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKeyPath, true))
            {
                if (key.GetValue(AppName) == null)
                {
                    key.SetValue(AppName, $"\"{appPath}\"", RegistryValueKind.String);
                    this.CreateSubKeyFirstRun();
                    MessageBox.Show("Se ha registrado la aplicación para iniciarse con Windows automáticamente.");
                }
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }
    }
}
