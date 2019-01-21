using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Xml.Serialization;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace gk.SQLConfigurator
{
    [Serializable]
    public class ItemChangerList
    {

        [Editor(typeof(ItemChangerCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("ItemChangers")]
        [DisplayName("ItemChangers")]
        [Description("A collection of the ItemChanger")]
        public ItemChangerCollection Items { get; set; }
        public int SelectedObjectIndex { get; set; }
        public int EditorType { get; set; }
        public ItemChangerList() { }
    }

    public class ItemChangerCollection : CollectionBase
    {
        public ItemChanger this[int index]
        {
            get { return (ItemChanger)List[index]; }
        }

        public void Add(ItemChanger ic)
        {
            List.Add(ic);
        }

        public void Remove(ItemChanger ic)
        {
            List.Remove(ic);
        }
    }

    public class ItemChangerCollectionEditor : CollectionEditor
    {
        public ItemChangerCollectionEditor(Type type) : base(type)
        {
        }

        protected override string GetDisplayText(object value)
        {
            ItemChanger item = new ItemChanger();
            item = (ItemChanger)value; // wtf?

            return base.GetDisplayText(item.Name);
        }
    }

    [Serializable]
    public class ItemChanger
    {
        [Category("ItemChanger")]
        [DisplayName("Name")]
        [Description("Name of new IC.")]
        public string Name { get; set; }
        [Category("ItemChanger")]
        [DisplayName("GetSQL")]
        [Description("GetSQL")]
        public string GetSQL { get; set; }
        [Category("ItemChanger")]
        [DisplayName("GetSQLWhereString")]
        [Description("GetSQLWhereString")]
        public string GetSQLWhereString { get; set; }
        [Category("ItemChanger")]
        [DisplayName("CreateSql")]
        [Description("CreateSql")]
        public string CreateSql { get; set; }
        [Category("ItemChanger")]
        [DisplayName("EditSql")]
        [Description("EditSql")]
        public string EditSql { get; set; }
        [Category("ItemChanger")]
        [DisplayName("CreateoreditSql")]
        [Description("CreateoreditSql")]
        public string CreateoreditSql { get; set; }
        [Category("ItemChanger")]
        [DisplayName("DeleteSql")]
        [Description("DeleteSql")]
        public string DeleteSql { get; set; }


        [XmlIgnore]
        [Category("ItemChanger")]
        [DisplayName("Icon")]
        [Description("Icon")]
        public Bitmap Icon { get { return _Icon; } set { _Icon = value; } }

        [NonSerialized]
        [XmlIgnore]
        public Bitmap _Icon;
        //[XmlElement("Image")]
        public string ImageBuffer
        {
            get
            {
                string imageBuffer = null;

                if (_Icon != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        _Icon.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        imageBuffer = Convert.ToBase64String(ms.ToArray());
                        return imageBuffer;
                    }
                }

                return imageBuffer;
            }
            set
            {
                if (value == null)
                {
                    _Icon = null;
                }
                else
                {                    
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(value)))
                    {
                        _Icon = new Bitmap(ms);
                    }
                }
            }
        }
        public ItemChanger(string _name, string _getSQL, string _createSql, string _editSql, string _createoreditSql, string _deleteSql, System.Drawing.Bitmap _icon)
        {
            Name = _name;
            GetSQL = _getSQL;
            CreateSql = _createSql;
            EditSql = _editSql;
            CreateoreditSql = _createoreditSql;
            DeleteSql = _deleteSql;
            _Icon = _icon;
            //WhereParam = new List<string>();
        }

        public ItemChanger()
        {
            //string s= "adsfasdf";
            
            //string[] buf = s.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            //if (buf.Length > 1)
            //    s.Replace(";", ",");

            //WhereParam = new List<string>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
