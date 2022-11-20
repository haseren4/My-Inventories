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
    public partial class ContainerDetailForm : Form
    {
        public ContainerDetailForm()
        {
            InitializeComponent();
        }
        public ContainerDetailForm(ContainerClass container)
        {
            InitializeComponent();
            descriptionTbx.Text = container.description;
            serialTbx.Text = container.serialNum;
            keyTbx.Text = container.keyNum;
            notesTbx.Text = container.notes;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
