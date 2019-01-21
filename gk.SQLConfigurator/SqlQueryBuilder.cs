using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gk.SQLConfigurator
{
    class SqlQueryBuilder
    {
        public SqlQueryType QueryType { get; set; }
        public string Table { get; set; }
        private List<string> UField = new List<string>();
        private List<string> Where = new List<string>();

        public SqlQueryBuilder(SqlQueryType type, string table)
        {
            QueryType = type;
            Table = table;
        }

        public SqlQueryBuilder(SqlQueryType type, string table, string test)
        {
            QueryType = type;
            Table = table;
        }

        public void UAddField(string Field, string Value)
        {
            UField.Add(string.Format(@"   {0} = '{1}'", Field, Value));
        }

        public void UAddField(string Field, int Value)
        {
            UField.Add(string.Format(@"   {0} = {1}", Field, Value));
        }

        public void UAddField(string Field, double Value)
        {
            UField.Add(string.Format(@"   {0} = {1}", Field, Value.ToString().Replace(',', '.')));
        }

        public void WhereAdd(string Field, SqlRelationType relate, string Value)
        {
           
            switch (relate)
            {
                case SqlRelationType.Equal:
                    Where.Add(string.Format(@" AND  {0} = '{1}'", Field, Value));
                    break;
                case SqlRelationType.More:
                    Where.Add(string.Format(@" AND  {0} > '{1}'", Field, Value));
                    break;
                case SqlRelationType.Less:
                    Where.Add(string.Format(@" AND  {0} < '{1}'", Field, Value));
                    break;
                case SqlRelationType.MoreOrEqual:
                    Where.Add(string.Format(@" AND  {0} >= '{1}'", Field, Value));
                    break;
                case SqlRelationType.LessOrEqual:
                    Where.Add(string.Format(@" AND  {0} <= '{1}'", Field, Value));
                    break;
                case SqlRelationType.Like:
                    Where.Add(string.Format(@" AND  {0} like '{1}'", Field, Value));
                    break;
                default:
                    break;
            }
        }
        public void WhereAdd(string Field, SqlRelationType relate, double Value)
        {
            switch (relate)
            {
                case SqlRelationType.Equal:
                    Where.Add(string.Format(@" AND  {0} = {1}", Field, Value.ToString().Replace(',', '.')));
                    break;
                case SqlRelationType.More:
                    Where.Add(string.Format(@" AND  {0} > {1}", Field, Value.ToString().Replace(',', '.')));
                    break;
                case SqlRelationType.Less:
                    Where.Add(string.Format(@" AND  {0} < {1}", Field, Value.ToString().Replace(',', '.')));
                    break;
                case SqlRelationType.MoreOrEqual:
                    Where.Add(string.Format(@" AND  {0} >= {1}", Field, Value.ToString().Replace(',', '.')));
                    break;
                case SqlRelationType.LessOrEqual:
                    Where.Add(string.Format(@" AND  {0} <= {1}", Field, Value.ToString().Replace(',', '.')));
                    break;
                case SqlRelationType.Like:
                    //UField.Add(string.Format(@"   {0} like '{1}'", Field, Value));
                    break;
                default:
                    break;
            }
            UField.Add(string.Format(@"   {0} = {1}", Field, Value.ToString().Replace(',','.')));
        }
        public void WhereAdd(string Field, SqlRelationType relate, int Value)
        {
            switch (relate)
            {
                case SqlRelationType.Equal:
                    Where.Add(string.Format(@" AND  {0} = {1} ", Field, Value));
                    break;
                case SqlRelationType.More:
                    Where.Add(string.Format(@" AND  {0} > {1} ", Field, Value));
                    break;
                case SqlRelationType.Less:
                    Where.Add(string.Format(@" AND  {0} < {1} ", Field, Value));
                    break;
                case SqlRelationType.MoreOrEqual:
                    Where.Add(string.Format(@" AND  {0} >= {1} ", Field, Value));
                    break;
                case SqlRelationType.LessOrEqual:
                    Where.Add(string.Format(@" AND  {0} <= {1} ", Field, Value));
                    break;
                case SqlRelationType.Like:
                    //UField.Add(string.Format(@"   {0} like '{1}'", Field, Value));
                    break;
                default:
                    break;
            }
        }


        public string GetQuery()
        {
            try
            {
                switch (this.QueryType)
                {
                    case SqlQueryType.Select:
                        return getSelectQuery();
                    case SqlQueryType.Update:
                        return getUpdateQuery();
                    case SqlQueryType.Delete:
                        return getDeleteQuery();
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        private string getSelectQuery()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        private string getUpdateQuery()
        {
            try
            {
                string sqlUpdate = string.Format( "UPDATE [{0}] SET \n", this.Table);
                for (int i = 0, c = UField.Count; i < c; i++)
                {
                    sqlUpdate += string.Format("\t  {0} {1} \n", (i == 0 ? " " : ","),  UField[i]); 
                }
                sqlUpdate += "WHERE 1=1 \n";
                for (int i = 0, c = Where.Count; i < c; i++)
                {
                    sqlUpdate += string.Format("\t {0} \n", Where[i]);
                }
                return sqlUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        private string getDeleteQuery()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
    }
    public enum SqlQueryType
    {
        Select,
        Update,
        Delete
    }

    public enum SqlRelationType
    {
        Equal,
        More,
        Less,
        MoreOrEqual,
        LessOrEqual,
        Like
    }

}
