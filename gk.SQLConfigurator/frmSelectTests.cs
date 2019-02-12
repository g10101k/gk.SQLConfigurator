/*
 *  "gk.SQLConfigurator", Excel add-in that allows you to fill / edit / delete SQL table data.
 *
 *  Copyright (C) 2015-2019  Igor Tyulyakov aka g10101k, g101k. Contacts: <g101k@mail.ru>
 *  
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 */
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
