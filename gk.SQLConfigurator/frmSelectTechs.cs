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
    public partial class frmSelectTech : Form
    {
        public String mask;
        public frmSelectTech(object o)
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = o;
        }
    }
}
