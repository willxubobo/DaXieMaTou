using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CMICT.CSP.Model.BusinessSearchModels
{
    [Serializable]
    public class TemplateModel
    {
        public string TemplateName { get; set; }

        public string TemplateDesc { get; set; }

        public bool IsTruePaged { get; set; }

        public bool IsReleased { get; set; }
        //查询控件生成
        public List<QueryControls> QueryControls { get; set; }
        //Grid生成
        public GridBuilder GridBuilder { get; set; }
        //SQL生成
        public SQLBuilder SQLBuilder { get; set; }
        //行汇总
        public List<GroupBy> GroupBy { get; set; }
        //通信
        public List<Communication> Communication { get; set; }
    }
    [Serializable]
    public class QueryControls
    {
        public string ColumnName;
        public string DisplayName;
        public string ControlType;
        public string Compare;
        public bool IsLike;
        public string SourceSql;
        public string Reminder;
        public string DefautValue;
    }
    [Serializable]
    public class GridBuilder
    {
        //展现方式 田字形、普通列表等
        public string DisplayType;
        //每行字段数
        public decimal ColumnSize;
    }
    [Serializable]
    public class SQLBuilder
    {
        //是否存储过程
        public bool IsProcudure;
        //连接字符串
        public string ConnectionStrings;
        //表名
        public string TableName;
        //SQL Select部分
        public string SelectSQL;
        //列名 “,”隔开
        public string ColumnNames;
        //展现名 “,”隔开
        public string DisplayNames;
        //隐藏列 “,”隔开
        public string HiddenNames;
        //合并表头名 “,”隔开
        public string MergeColumnNames;
        //默认筛选条件 “where 1=1”开头
        public string DefauleQuery;
        //默认排序
        public string Orderby;
        //默认每页条数
        public string PageSize;

        public Dictionary<string, string> Parameters;

        public Dictionary<string, string> ProcCalColumns;
    }
    [Serializable]
    public class GroupBy
    {
        //汇总列名 “,”隔开
        public string Columns;
        //是否在表尾
        public bool IsAtLast;
        public Dictionary<string, string> GroupByColumns;
    }

    [Serializable]
    public class Communication
    {
        public string CoumunicationID { get; set; }

        public string SourceTemplateID { get; set; }

        public Dictionary<string,string> Fields { get; set; }

    }
}
