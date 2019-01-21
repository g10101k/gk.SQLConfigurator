using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace gk.SQLConfigurator
{
    public partial class frmSelectTest : Form
    {
        [Browsable(true)]
        [Description("Property: Text param 2")]
        [Category("Text params")]
        [DisplayName("Text param 2")]
        public String mask { get; set; }

        [Browsable(true)]
        [Description("Property: Text param 2")]
        [Category("Text params")]
        [DisplayName("Text param 2")]
        public object obj { get; set; }

        public frmSelectTest(object o)
        {
            obj = o;
            InitializeComponent();
            propertyGrid1.SelectedObject = obj;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            DialogResult = DialogResult.OK;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button1_Click(sender, null);
            }
        }
    }
}
