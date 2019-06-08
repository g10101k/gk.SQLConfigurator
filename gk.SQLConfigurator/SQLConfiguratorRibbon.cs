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
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace gk.SQLConfigurator
{
    public partial class SQLConfiguratorRibbon
    {
        public event Action ButtonClicked;
        public event Action getFromDB;
        public event Action btnExecuteToDBClicked;
        public event Action AttachConsole;
        public event Action btSqlEditClicked;
        //public event Action Log;
        public event Action btnSQLSaveCliked;
        public event Action btnSettingCliked;
        public ThisAddIn addin;

        public int SelectedObjectIndex { get; set; }
        private void gLDSRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            UpdateICConteiner();
        }

        public void UpdateICConteiner()
        {
            this.cmbItemChanger.Items.Clear();
            foreach (ItemChanger ic in ThisAddIn.ICList.Items)
            {
                try
                {
                    if (ic._Icon == null)
                        ic._Icon = global::gk.SQLConfigurator.Properties.Resources.brick;
                    Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ddi = this.Factory.CreateRibbonDropDownItem();
                    ddi.Image = ic._Icon;
                    ddi.Label = ic.Name;
                    ddi.Tag = ic;
                    this.cmbItemChanger.Items.Add(ddi);
                }
                catch { }
            }

            cmbItemChanger.SelectedItemIndex = ThisAddIn.ICList.SelectedObjectIndex;
            gallery1_Click(null, null);

            editorTypeSelect.SelectedItemIndex = ThisAddIn.ICList.EditorType;
            editorTypeSelect_Click(null, null);
        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            if (ButtonClicked != null)
                ButtonClicked();
        }

        private void gallery1_Click(object sender, RibbonControlEventArgs e)
        {
            SelectedObjectIndex = cmbItemChanger.SelectedItemIndex;
            ThisAddIn.ICList.SelectedObjectIndex = cmbItemChanger.SelectedItemIndex;
            addin.SaveICL();
            cmbItemChanger.Image = cmbItemChanger.Items[cmbItemChanger.SelectedItemIndex].Image;
            cmbItemChanger.Label = cmbItemChanger.Items[cmbItemChanger.SelectedItemIndex].Label;
        }

        private void button3_Click(object sender, RibbonControlEventArgs e)
        {
            if (getFromDB != null)
                getFromDB();
        }

        private void button2_Click_1(object sender, RibbonControlEventArgs e)
        {
            if (btnExecuteToDBClicked != null)
                btnExecuteToDBClicked();
        }

        private void button4_Click(object sender, RibbonControlEventArgs e)
        {
            if (AttachConsole != null)
                AttachConsole();
        }

        private void editorTypeSelect_Click(object sender, RibbonControlEventArgs e)
        {
            ThisAddIn.ICList.EditorType = editorTypeSelect.SelectedItemIndex;
            addin.SaveICL();
            editorTypeSelect.Label = editorTypeSelect.Items[editorTypeSelect.SelectedItemIndex].Label;
            btnAction.Image = editorTypeSelect.Items[editorTypeSelect.SelectedItemIndex].Image;
            editorTypeSelect.Image = editorTypeSelect.Items[editorTypeSelect.SelectedItemIndex].Image;

            
        }

        private void btSqlEdit_Click(object sender, RibbonControlEventArgs e)
        {
            if (btSqlEditClicked != null)
                btSqlEditClicked();
        }

        private void btnSQLSave_Click(object sender, RibbonControlEventArgs e)
        {
            if (btnSQLSaveCliked != null)
                btnSQLSaveCliked();
        }

        private void btnSetting_Click(object sender, RibbonControlEventArgs e)
        {
            if (btnSettingCliked != null)
                btnSettingCliked();
        }
    }
}
