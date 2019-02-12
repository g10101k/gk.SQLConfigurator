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
using System.Text;
using System.ComponentModel;

namespace Organization
{
    public class Employee
    {
        #region Private Variables
        private string firstName;
        private string lastName;
        private DateTime dateOfHire;
        #endregion

        #region Public Properties
        [Category("Employee")]
        [DisplayName("First Name")]
        [Description("The first name of the employee.")]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [Category("Employee")]
        [DisplayName("Last Name")]
        [Description("The last name of the employee.")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        [Category("Employee")]
        [DisplayName("Date of Hire")]
        [Description("The hire date of the employee.")]
        public DateTime DateOfHire
        {
            get { return dateOfHire; }
            set { dateOfHire = value; }
        }
        #endregion
    }
}
