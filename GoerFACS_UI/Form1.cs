using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GoerFACS_UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.V)
            {
                if (sender != null && sender.GetType() == typeof(DataGridView))
                    // 调用上面的粘贴代码
                    DataGirdViewCellPaste((DataGridView)sender);
            }
        }
        public void DataGirdViewCellPaste(DataGridView p_Data)
        {
            try
            {
                // 获取剪切板的内容，并按行分割
                string pasteText = Clipboard.GetText();
                if (string.IsNullOrEmpty(pasteText))
                    return;
                string[] lines = pasteText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line.Trim()))
                        continue;
                    // 按 Tab 分割数据
                    string[] vals = line.Split('\t');
                    p_Data.Rows.Add(vals);
                }
            }
            catch
            {
                // 不处理
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.path = textBox1.Text;
            Properties.Settings.Default.InspectionName = textBox2.Text;
            Properties.Settings.Default.PCMPath = textBox3.Text;
            Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.path;
            textBox2.Text = Properties.Settings.Default.InspectionName;
            textBox3.Text = Properties.Settings.Default.PCMPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(textBox1.Text);
            sw.WriteLine(string.Format("InspectionName = \"{0}\"", textBox2.Text));
            sw.Close();
            sw = new StreamWriter(textBox3.Text);
            sw.WriteLine(string.Format("Np={0}", dataGridView1.Rows.Count-1));
            for(int i=1;i<dataGridView1.Rows.Count;i++)
            {
                DataGridViewRow r = dataGridView1.Rows[i-1];
                string x = r.Cells[0].Value.ToString();
                string y = r.Cells[1].Value.ToString();
                string z = r.Cells[2].Value.ToString();
                string u = r.Cells[3].Value.ToString();
                string v = r.Cells[4].Value.ToString();
                string w = r.Cells[5].Value.ToString();
                sw.WriteLine(string.Format(
                    "Pt[{0}]=point({1},{2},{3},{4},{5},{6})",
                    new string[] { i.ToString(), x, y, z, u, v, w }));
            }

            sw.Close();
        }
    }
}
