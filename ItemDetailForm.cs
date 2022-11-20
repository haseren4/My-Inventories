using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Item_Finder
{
    public partial class ItemDetailForm : Form
    {
        public ItemDetailForm()
        {
            InitializeComponent();
        }
        public ItemDetailForm(ItemClass item)
        {
            InitializeComponent();
            this.descTbx.Text = item.description;
            this.snTbx.Text = item.serialNum;
            this.notesTbx.Text = item.note;
            this.nsnTbx.Text = item.nsn;
            this.qtyTbx.Text = item.qty.ToString();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
