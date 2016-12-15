using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace SP.Framework.Common
{
    public static class GridViewExtensions
    {
        /// 
        /// GridView行合并
        /// 
        /// 
        /// 合并参数(匿名类型)
        /// ColumnIndex:要合并行的索引 (以0开始,必须指定)
        /// ID(可选):如果该行为模板行则必须指定
        /// PropertyName:根据ID属性 默认值为Text
        /// Colums:(string类型)表示额外的行合并方式和ColumnIndex一样(多个使用逗号隔开,如Colums="5,6,7,8")
        /// 例:
        /// 合并第一行(第一行为模板行),绑定的一个Label名称为lblName 根据Text属性值合并 第6行方式和第一行相同
        /// new {ColumnIndex=0,ID="lblName",PropertyName="Text",Columns="5"}
        /// 
        public static GridView RowSpan(this GridView gridView, object field)
        {
            IDictionary rowDictionary = ObjectLoadDictionary(field);
            int columnIndex = int.Parse(rowDictionary["ColumnIndex"].ToString());
            //string columnName = rowDictionary["ColumnName"].ToString();
            //string propertyName = rowDictionary["PropertyName"].ToString();
            string columnName = null;
            string propertyName = null;
            string columns = rowDictionary["Columns"].ToString();
            for (var i = 0; i < gridView.Rows.Count; i++)
            {
                int rowSpanCount = 1;
                for (int j = i + 1; j < gridView.Rows.Count; j++)
                {
                    //绑定行合并处理
                    if (string.IsNullOrEmpty(columnName))
                    {
                        //比较2行的值是否相同
                        if (gridView.Rows[i].Cells[columnIndex].Text == gridView.Rows[j].Cells[columnIndex].Text)
                        {
                            //合并行的数量+1
                            rowSpanCount++;
                            //隐藏相同的行
                            gridView.Rows[j].Cells[columnIndex].Visible = false;
                            if (!string.IsNullOrEmpty(columns))
                            {
                                columns.Split(',').ToList().ForEach(c => gridView.Rows[j].Cells[int.Parse(c)].Visible = false);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        //模板行的合并处理
                        if (GetPropertyValue(gridView.Rows[i].Cells[columnIndex].FindControl(columnName), propertyName) == GetPropertyValue(gridView.Rows[j].Cells[columnIndex].FindControl(columnName), propertyName))
                        {
                            rowSpanCount++;
                            //隐藏相同的行
                            gridView.Rows[j].Cells[columnIndex].Visible = false;
                            if (!string.IsNullOrEmpty(columns))
                            {
                                columns.Split(',').ToList().ForEach(c => gridView.Rows[j].Cells[int.Parse(c)].Visible = false);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (rowSpanCount > 1)
                {
                    //行合并
                    gridView.Rows[i].Cells[columnIndex].RowSpan = rowSpanCount;
                    //判断是否有额外的行需要合并
                    if (!string.IsNullOrEmpty(columns))
                    {
                        //额外的行合并
                        columns.Split(',').ToList().ForEach(c => gridView.Rows[i].Cells[int.Parse(c)].RowSpan = rowSpanCount);
                    }
                    i = i + rowSpanCount - 1;
                }
            }
            return gridView;
        }
        private static IDictionary ObjectLoadDictionary(object fields)
        {
            IDictionary resultDictionary = new Dictionary<string,string>();
            PropertyInfo[] property = fields.GetType().GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.GetProperty);
            foreach (PropertyInfo tempProperty in property)
            {
                resultDictionary.Add(tempProperty.Name, tempProperty.GetValue(fields, null).ToString());
            }
            //指定默认值
            if (!resultDictionary.Contains("ColumnIndex"))
            {
                throw new Exception("未指定要合并行的索引 ColumnIndex 属性!");
            }
            if (!resultDictionary.Contains("ColumnName"))
            {
                resultDictionary.Add("ColumnName", null);
            }
            if (!resultDictionary.Contains("PropertyName"))
            {
                resultDictionary.Add("PropertyName", "Text");
            }
            if (!resultDictionary.Contains("Columns"))
            {
                resultDictionary.Add("Columns", null);
            }
            return resultDictionary;
        }
        /// 
        /// 获取一个对象的一个属性..
        /// 
        /// 
        /// 属性名称
        /// 属性的值, 如果无法获取则返回null
        private static object GetPropertyValue(object obj, string PropertyName)
        {
            PropertyInfo property = obj.GetType().GetProperty(PropertyName);
            return property.GetValue(obj, null);
        }
    }
}
