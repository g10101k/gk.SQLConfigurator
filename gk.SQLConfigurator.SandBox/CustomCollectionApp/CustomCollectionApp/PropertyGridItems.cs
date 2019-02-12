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
using System.ComponentModel;
using System.Text;

namespace CustomCollectionApp
{
    internal class PropertyGridItems
    {
        [Editor(typeof(Organization.EmployeeCollectionEditor),typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Organization")]
        [DisplayName("Employees")]
        [Description("A collection of the employees within the organization")]
        public Organization.EmployeeCollection Employees
        {
            get { return Properties.Settings.Default.Employees; }
            set { Properties.Settings.Default.Employees = value; }
        }
    }
}
