using System.Drawing;
using System.Windows.Forms;

namespace Demo
{
    internal class MyItem : ListViewItem.ListViewSubItem
    {
        public int Key { get; set; }
        public Image value;
        public MyItem(int flag)
        {
            this.Key = flag;
        }

        private void SetImage()
        {
            if (this.Key == 1)
                value = Image.FromFile(@"C:\Image\inValid.jpg");
            value = Image.FromFile(@"C:\Image\inValid.jpg");
        }

    }
}