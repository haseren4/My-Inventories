using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Item_Finder
{
    public partial class ItemControl : UserControl
    {
        public ItemClass item = new ItemClass();
        public ItemControl()
        {
            InitializeComponent();
        }
        public ItemControl(ItemClass itemClass)
        {
            InitializeComponent();
            item = itemClass;
            reloadData();
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ItemDetailForm form = new ItemDetailForm(item);
            form.Text = itemLbl.Text;
            if(form.ShowDialog() == DialogResult.OK)
            {
                
            }
            item.description = form.descTbx.Text;
            if (form.snTbx.Text != "SN: ")
                item.serialNum = form.snTbx.Text;
            item.serialNum.Replace("SN:", "");
            item.note = form.notesTbx.Text;
            item.nsn = form.nsnTbx.Text;
            try
            {
                item.qty = int.Parse(form.qtyTbx.Text);
            }
            catch (System.FormatException) {
                item.qty = 1;
            }
            reloadData();
        }
        public void reloadData()
        {
            itemLbl.Text = item.description;
            qtyLbl.Text = item.qty.ToString();
            snLbl.Text = "SN: " + item.serialNum;
            StringBuilder tipText = new StringBuilder("NSN: " + item.nsn + "\n");
            tipText.Append("Notes:\n" + item.note);
       
            toolTip1.SetToolTip(this.itemLbl, tipText.ToString());
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
