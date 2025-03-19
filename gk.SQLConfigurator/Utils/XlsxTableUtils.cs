using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;

namespace gk.SQLConfigurator.Utils
{
    public static class XlsxTableExtensions
    {
        public static void SetColumns(this Worksheet worksheet, DataColumnCollection columns)
        {
            worksheet.Cells.ClearContents();


            Range currentRange = worksheet.Rows[1];

            for (int i = 0; i < columns.Count; i++)
            {
                currentRange.Cells[i + 1].Value = columns[i].ColumnName;
            }

            worksheet.Columns.WrapText = false;
        }

        public static void Fill(this Worksheet worksheet, System.Data.DataTable table)
        {
            if (table == null) return;

            int columnCount = table.Columns.Count;

            worksheet.SetColumns(table.Columns);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                Range currentRange = worksheet.Rows[i + 2];
                for (int j = 0; j < columnCount; j++)
                {
                    currentRange.Cells[j + 1].Value = table.Rows[i][j].ToString();
                }
            }
        }
    }
}
