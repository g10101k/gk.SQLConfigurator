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
        
        public event Action BtnConnectSetings;
        public event Action BtnExecuteToDBClicked;
        public event Action BtSqlEditClicked;
        public event Action BtnSQLSaveCliked;
        public event Action BtnSettingCliked;

        public ThisAddIn Addin { get; set; }
        public int SelectedObjectIndex { get; set; }
        private void gLDSRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            UpdateICConteiner();
        }

        public void UpdateICConteiner()
        {
            try
            {
                this.cmbItemChanger.Items.Clear();
                foreach (ItemChanger ic in ThisAddIn.ICList.Items)
                {
                    try
                    {
                        if (ic._Icon == null)
                            ic._Icon = Properties.Resources.brick;
                        RibbonDropDownItem ddi = this.Factory.CreateRibbonDropDownItem();
                        ddi.Image = ic._Icon;
                        ddi.Label = ic.Name;
                        ddi.Tag = ic;
                        this.cmbItemChanger.Items.Add(ddi);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("(UpdateICConteiner.foreach)", ex);
                    }
                }

                cmbItemChanger.SelectedItemIndex = ThisAddIn.ICList.SelectedObjectIndex;
                gallery1_Click(null, null);

                editorTypeSelect.SelectedItemIndex = ThisAddIn.ICList.EditorType;
                editorTypeSelect_Click(null, null);
            }
            catch (Exception ex)
            {
                Logger.Error("(UpdateICConteiner)", ex);
            }
        }

        private void btnConnectSetings_Click(object sender, RibbonControlEventArgs e)
        {
            BtnConnectSetings?.Invoke();
        }

        private void gallery1_Click(object sender, RibbonControlEventArgs e)
        {
            SelectedObjectIndex = cmbItemChanger.SelectedItemIndex;
            ThisAddIn.ICList.SelectedObjectIndex = cmbItemChanger.SelectedItemIndex;
            Addin.SaveICL();
            cmbItemChanger.Image = cmbItemChanger.Items[cmbItemChanger.SelectedItemIndex].Image;
            cmbItemChanger.Label = cmbItemChanger.Items[cmbItemChanger.SelectedItemIndex].Label;
        }

        private void btnExecuteToDB_Click(object sender, RibbonControlEventArgs e)
        {
            BtnExecuteToDBClicked?.Invoke();
        }

        private void editorTypeSelect_Click(object sender, RibbonControlEventArgs e)
        {
            ThisAddIn.ICList.EditorType = editorTypeSelect.SelectedItemIndex;
            Addin.SaveICL();
            editorTypeSelect.Label = editorTypeSelect.Items[editorTypeSelect.SelectedItemIndex].Label;
            btnAction.Image = editorTypeSelect.Items[editorTypeSelect.SelectedItemIndex].Image;
            editorTypeSelect.Image = editorTypeSelect.Items[editorTypeSelect.SelectedItemIndex].Image;
        }

        private void btSqlEdit_Click(object sender, RibbonControlEventArgs e)
        {
            BtSqlEditClicked?.Invoke();
        }

        private void btnSQLSave_Click(object sender, RibbonControlEventArgs e)
        {
            BtnSQLSaveCliked?.Invoke();
        }

        private void btnSetting_Click(object sender, RibbonControlEventArgs e)
        {
            BtnSettingCliked?.Invoke();
        }
    }
}
