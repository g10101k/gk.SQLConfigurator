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
namespace gk.SQLConfigurator
{
    partial class SQLConfiguratorRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public SQLConfiguratorRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl1 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl2 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl3 = this.Factory.CreateRibbonDropDownItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLConfiguratorRibbon));
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl4 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl5 = this.Factory.CreateRibbonDropDownItem();
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.lServer = this.Factory.CreateRibbonLabel();
            this.lDb = this.Factory.CreateRibbonLabel();
            this.lConnectState = this.Factory.CreateRibbonLabel();
            this.separator1 = this.Factory.CreateRibbonSeparator();
            this.btnConnect = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.btnAction = this.Factory.CreateRibbonButton();
            this.btnSQLSave = this.Factory.CreateRibbonButton();
            this.editorTypeSelect = this.Factory.CreateRibbonGallery();
            this.cmbItemChanger = this.Factory.CreateRibbonGallery();
            this.btSqlEdit = this.Factory.CreateRibbonButton();
            this.btnSetting = this.Factory.CreateRibbonButton();
            this.group3 = this.Factory.CreateRibbonGroup();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.group3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Groups.Add(this.group3);
            this.tab1.Label = global::gk.SQLConfigurator.Properties.Settings.Default.PanelName;
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.lServer);
            this.group1.Items.Add(this.lDb);
            this.group1.Items.Add(this.lConnectState);
            this.group1.Items.Add(this.separator1);
            this.group1.Items.Add(this.btnConnect);
            this.group1.Label = "Подключение";
            this.group1.Name = "group1";
            // 
            // lServer
            // 
            this.lServer.Label = "Сервер:";
            this.lServer.Name = "lServer";
            // 
            // lDb
            // 
            this.lDb.Label = "БД:";
            this.lDb.Name = "lDb";
            // 
            // lConnectState
            // 
            this.lConnectState.Label = "Состояние: ";
            this.lConnectState.Name = "lConnectState";
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // btnConnect
            // 
            this.btnConnect.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnConnect.Image = global::gk.SQLConfigurator.Properties.Resources.database;
            this.btnConnect.Label = "Выбрать БД";
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.ShowImage = true;
            this.btnConnect.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnConnectSetings_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.btnAction);
            this.group2.Items.Add(this.btnSQLSave);
            this.group2.Label = "Действия";
            this.group2.Name = "group2";
            // 
            // btnAction
            // 
            this.btnAction.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnAction.Enabled = false;
            this.btnAction.Image = global::gk.SQLConfigurator.Properties.Resources.database_save;
            this.btnAction.Label = "Выполнить";
            this.btnAction.Name = "btnAction";
            this.btnAction.ShowImage = true;
            this.btnAction.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnExecuteToDB_Click);
            // 
            // btnSQLSave
            // 
            this.btnSQLSave.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnSQLSave.Image = global::gk.SQLConfigurator.Properties.Resources.script_save;
            this.btnSQLSave.Label = "Сохранить в SQL файл";
            this.btnSQLSave.Name = "btnSQLSave";
            this.btnSQLSave.ShowImage = true;
            this.btnSQLSave.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSQLSave_Click);
            // 
            // editorTypeSelect
            // 
            this.editorTypeSelect.ColumnCount = 1;
            this.editorTypeSelect.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            ribbonDropDownItemImpl1.Image = global::gk.SQLConfigurator.Properties.Resources.database_go;
            ribbonDropDownItemImpl1.Label = "Получить";
            ribbonDropDownItemImpl2.Image = global::gk.SQLConfigurator.Properties.Resources.database_edit;
            ribbonDropDownItemImpl2.Label = "Изменить";
            ribbonDropDownItemImpl3.Image = ((System.Drawing.Image)(resources.GetObject("ribbonDropDownItemImpl3.Image")));
            ribbonDropDownItemImpl3.Label = "Создать";
            ribbonDropDownItemImpl4.Image = global::gk.SQLConfigurator.Properties.Resources.database_refresh;
            ribbonDropDownItemImpl4.Label = "Создать/Изменить";
            ribbonDropDownItemImpl5.Image = global::gk.SQLConfigurator.Properties.Resources.database_delete;
            ribbonDropDownItemImpl5.Label = "Удалить";
            this.editorTypeSelect.Items.Add(ribbonDropDownItemImpl1);
            this.editorTypeSelect.Items.Add(ribbonDropDownItemImpl2);
            this.editorTypeSelect.Items.Add(ribbonDropDownItemImpl3);
            this.editorTypeSelect.Items.Add(ribbonDropDownItemImpl4);
            this.editorTypeSelect.Items.Add(ribbonDropDownItemImpl5);
            this.editorTypeSelect.Label = "Режим";
            this.editorTypeSelect.Name = "editorTypeSelect";
            this.editorTypeSelect.RowCount = 5;
            this.editorTypeSelect.ShowImage = true;
            this.editorTypeSelect.ShowItemSelection = true;
            this.editorTypeSelect.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.editorTypeSelect_Click);
            // 
            // cmbItemChanger
            // 
            this.cmbItemChanger.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.cmbItemChanger.Label = "gallery1";
            this.cmbItemChanger.Name = "cmbItemChanger";
            this.cmbItemChanger.RowCount = 4;
            this.cmbItemChanger.ShowImage = true;
            this.cmbItemChanger.ShowItemSelection = true;
            this.cmbItemChanger.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.gallery1_Click);
            // 
            // btSqlEdit
            // 
            this.btSqlEdit.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btSqlEdit.Image = global::gk.SQLConfigurator.Properties.Resources.script_edit;
            this.btSqlEdit.Label = "Редактировать SQL";
            this.btSqlEdit.Name = "btSqlEdit";
            this.btSqlEdit.ShowImage = true;
            this.btSqlEdit.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btSqlEdit_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnSetting.Image = global::gk.SQLConfigurator.Properties.Resources.setting_tools;
            this.btnSetting.Label = "Настройка";
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.ShowImage = true;
            this.btnSetting.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSetting_Click);
            // 
            // group3
            // 
            this.group3.Items.Add(this.editorTypeSelect);
            this.group3.Items.Add(this.cmbItemChanger);
            this.group3.Items.Add(this.btSqlEdit);
            this.group3.Items.Add(this.btnSetting);
            this.group3.Label = "Настройки";
            this.group3.Name = "group3";
            // 
            // SQLConfiguratorRibbon
            // 
            this.Name = "SQLConfiguratorRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.gLDSRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public Microsoft.Office.Tools.Ribbon.RibbonGallery cmbItemChanger;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAction;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        public Microsoft.Office.Tools.Ribbon.RibbonLabel lServer;
        public Microsoft.Office.Tools.Ribbon.RibbonLabel lDb;
        public Microsoft.Office.Tools.Ribbon.RibbonLabel lConnectState;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnConnect;
        public Microsoft.Office.Tools.Ribbon.RibbonGallery editorTypeSelect;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btSqlEdit;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSQLSave;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSetting;
        public Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
    }

    partial class ThisRibbonCollection
    {
        internal SQLConfiguratorRibbon SQLConfiguratorRibbon
        {
            get { return this.GetRibbon<SQLConfiguratorRibbon>(); }
        }
    }
}
