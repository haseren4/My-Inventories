using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace Item_Finder
{
    public partial class MainForm : Form
    {
        private static String DEF_FILENAME = "inventory.xml";

        private static String XML_INVENTORY = "INVENTORY";
        private static String XML_CONTAINER = "CONTAINER";
        private static String XML_CONTAINER_DESCRIPTION = "description";
        private static String XML_CONTAINER_SERIAL = "serial";
        private static String XML_CONTAINER_KEY = "key";
        private static String XML_CONTAINER_NOTES = "notes";

        private static String XML_ITEM = "ITEM";
        private static String XML_ITEM_DESCRIPTION = "description";
        private static String XML_ITEM_SERIAL = "serial";
        private static String XML_ITEM_NSN = "nsn";
        private static String XML_ITEM_NOTES = "notes";
        private static String XML_ITEM_QTY = "qty";

        private static String BASE_DA1750 = "\\DA1750.xls";
        private static int DA1750_NUMBER = 1;
        private static int DA1750_ITEM = 2;
        private static int DA1750_TYPE = 8;
        private static int DA1750_QTY = 9;
        public MainForm()
        {
            InitializeComponent();
            loadInventory(DEF_FILENAME);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void addContainerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tableLayoutPanel1.Controls.Add(new ContainerControl());

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveInventory(DEF_FILENAME);
            Close();
        }
        private void SaveInventory(String filename) 
        {
            XmlWriter xmlDoc = XmlWriter.Create(filename);
            xmlDoc.WriteStartDocument();
            xmlDoc.WriteStartElement(XML_INVENTORY);
            

            foreach(ContainerControl container in tableLayoutPanel1.Controls)
            {
                xmlDoc.WriteStartElement(XML_CONTAINER);
                xmlDoc.WriteAttributeString(XML_CONTAINER_DESCRIPTION, container.container.description);
                xmlDoc.WriteAttributeString(XML_CONTAINER_KEY, container.container.keyNum);
                xmlDoc.WriteAttributeString(XML_CONTAINER_NOTES, container.container.notes);
                xmlDoc.WriteAttributeString(XML_CONTAINER_SERIAL, container.container.serialNum);

                foreach (ItemControl item in container.tableLayoutPanel1.Controls)
                {
                    xmlDoc.WriteStartElement(XML_ITEM);
                    xmlDoc.WriteAttributeString(XML_ITEM_DESCRIPTION, item.item.description);
                    xmlDoc.WriteAttributeString(XML_ITEM_NOTES, item.item.note);
                    xmlDoc.WriteAttributeString(XML_ITEM_NSN, item.item.nsn);
                    xmlDoc.WriteAttributeString(XML_ITEM_QTY, item.item.qty.ToString());
                    xmlDoc.WriteAttributeString(XML_ITEM_SERIAL, item.item.serialNum);
                    xmlDoc.WriteEndElement();
                    
                }

                
                xmlDoc.WriteEndElement();
            }
            
            xmlDoc.Close();

        }
        private void loadInventory(String filename) 
        {
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(filename);
                XmlNode inventory = xmlDocument.SelectSingleNode(XML_INVENTORY);
                foreach (XmlNode node in inventory.SelectNodes(XML_CONTAINER))
                {
                    ContainerControl newContainer = new ContainerControl();
                    newContainer.container.description = node.Attributes.GetNamedItem(XML_CONTAINER_DESCRIPTION).Value;
                    newContainer.container.keyNum = node.Attributes.GetNamedItem(XML_CONTAINER_KEY).Value;
                    newContainer.container.serialNum = node.Attributes.GetNamedItem(XML_CONTAINER_SERIAL).Value;
                    newContainer.container.notes = node.Attributes.GetNamedItem(XML_CONTAINER_NOTES).Value;

                    foreach (XmlNode item in node.SelectNodes(XML_ITEM))
                    {
                        ItemControl iControl = new ItemControl();
                        iControl.item.description = item.Attributes.GetNamedItem(XML_ITEM_DESCRIPTION).Value;
                        iControl.item.note = item.Attributes.GetNamedItem(XML_ITEM_NOTES).Value;
                        iControl.item.nsn = item.Attributes.GetNamedItem(XML_ITEM_NSN).Value;
                        iControl.item.qty = int.Parse(item.Attributes.GetNamedItem(XML_ITEM_QTY).Value);
                        iControl.item.serialNum = item.Attributes.GetNamedItem(XML_ITEM_SERIAL).Value;

                        iControl.reloadData();
                        newContainer.tableLayoutPanel1.Controls.Add(iControl);
                    }
                    newContainer.reloadData();
                    tableLayoutPanel1.Controls.Add(newContainer);
                }
            }
            catch
            {
                MessageBox.Show("No default XML file", "This is normal for first time users.");
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.Controls.Clear();
        }

        private void loadInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = "c:\\";
                dialog.Filter = "XML files (*.xml)|*.xml|All Files (*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    loadInventory(dialog.FileName);
                }
            }
        }

        private void saveInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.InitialDirectory = "c:\\";
                dialog.Filter = "XML files (*.xml)|*.xml|All Files (*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SaveInventory(dialog.FileName);
                }
            }    
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.InitialDirectory = "c:\\";
                dialog.Filter = "HTML files (*html)|*html|All Files (*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    toHTML(dialog.FileName);
                }
            }

        }
        private void toHTML(String filename)
        {
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(filename);

                sw.WriteLine("<HTML>");
                sw.WriteLine("<HEAD><TITLE>" + "My Inventories powered by Walks-Models" + "</TITLE></HEAD>");
                sw.WriteLine("<BODY>");
                sw.WriteLine("<H1 align=\"center\">My Inventories</H1>\n<HR>");
                foreach(ContainerControl container in tableLayoutPanel1.Controls)
                {
                    sw.WriteLine("<H2>" + container.container.description + "</H2>");
                    sw.WriteLine("<H3>Summary Info</H3>");
                    sw.WriteLine("<P>Serial Number: " + container.container.serialNum + "</P>");
                    sw.WriteLine("<P>Key Number: " + container.container.keyNum + "</P>");
                    sw.WriteLine("<H4>Notes</H4>\n<P>" + container.container.notes + "</P>");
                    sw.WriteLine("<H3>Items</h3>");
                    sw.WriteLine("<TABLE><TR><TH>NSN</TH><TH>DESCRIPTION</TH><TH>SERIAL NUMBER</TH><TH>QTY</TH></TR>");
                    foreach(ItemControl item in container.tableLayoutPanel1.Controls)
                    {
                        sw.WriteLine(String.Format("<TR><TD>{0}</TD><TD>{1}</TD><TD>{2}</TD><TD>{3}</TD></TR><TR><TD>{4}</TD></TR>",item.item.nsn,item.item.description,item.item.serialNum,item.item.qty,item.item.note));
                    }
                    sw.WriteLine("</TABLE>");
                    sw.WriteLine("<HR>");
                }
                sw.WriteLine("</BODY>");
                sw.WriteLine("</HTML>");
                //Close the file
                sw.Close();
                System.Diagnostics.Process.Start("iexplorer.exe " + filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }
        private void dA1750PackingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String chosenDir ="c:\\";

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = true;
                dialog.Description = "Choose Directory to save DA1750's";
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    chosenDir = dialog.SelectedPath;
                }
            }
            foreach(ContainerControl container in this.tableLayoutPanel1.Controls)
            {
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                xlWorkBook = xlApp.Workbooks.Open(Application.StartupPath+BASE_DA1750);
                xlWorkSheet = xlWorkBook.Worksheets["Sheet1"];
                xlWorkSheet.Cells[8, 1].Value = container.container.description;
                
                xlWorkSheet.Cells[9, 1].Value = "SN: " + container.container.serialNum + "     Key: " + container.container.keyNum;
                xlWorkSheet.Cells[8, 9].Value = System.DateTime.Now.ToShortDateString();
                int iRow = 16, itemNum = 1;
                foreach (ItemControl item in container.tableLayoutPanel1.Controls)
                {
                    
                    xlWorkSheet.Cells[iRow, DA1750_NUMBER].Value = itemNum;
                    itemNum++;
                    if(item.item.serialNum !="")
                        xlWorkSheet.Cells[iRow, DA1750_ITEM].Value = item.item.description + "     (SN: " + item.item.serialNum + ")";
                    else
                        xlWorkSheet.Cells[iRow, DA1750_ITEM].Value = item.item.description.Trim();
                    xlWorkSheet.Cells[iRow, DA1750_TYPE].Value = "EA";
                    xlWorkSheet.Cells[iRow, DA1750_QTY].Value = item.item.qty;
                    iRow++;
                }

                xlWorkBook.SaveAs(chosenDir +"\\"+ container.container.description + ".xls");
                xlWorkBook.Close();
                xlApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
