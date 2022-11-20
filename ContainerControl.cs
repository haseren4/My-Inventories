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
    public partial class ContainerControl : UserControl
    {
        public ContainerClass container;
        public ContainerControl(ContainerClass containerClass)
        {
            InitializeComponent();
            container = containerClass;
            reloadData();
        }
        public ContainerControl()
        {
            container = new ContainerClass();
            InitializeComponent();
        }
        public void reloadData() 
        {
            nameLbl.Text = container.description;
            StringBuilder tipText = new StringBuilder("Key Number: " + container.keyNum + "\n");
            tipText.Append("Serial Number: " + container.serialNum + "\n");
            tipText.Append("Notes:\n" + container.notes);
            toolTip.SetToolTip(this.nameLbl, tipText.ToString());
        }
        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tableLayoutPanel1.Controls.Add(new ItemControl());

            
            
        }

        private void editContainerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContainerDetailForm form = new ContainerDetailForm(container);
            form.Text = nameLbl.Text;
            if (form.ShowDialog() == DialogResult.OK)
            {
               
            }
            container.description = form.descriptionTbx.Text;
            container.keyNum = form.keyTbx.Text;
            container.serialNum = form.serialTbx.Text;
            container.notes = form.notesTbx.Text;

            reloadData();
        }

        private void toolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void deleteContainerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
