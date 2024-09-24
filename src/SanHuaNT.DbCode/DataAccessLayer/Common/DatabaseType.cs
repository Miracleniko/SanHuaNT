using System.ComponentModel;

namespace SanHuaNT.DbCode.DataAccessLayer;

/// <summary>数据库类型</summary>
public enum DatabaseType
{
    /// <summary>无效值</summary>
    [Description("无效值")]
    None = 0,
   
    /// <summary>MS的SqlServer数据库</summary>
    [Description("SqlServer数据库")]
    SqlServer = 1,
  
    /// <summary>SQLite数据库</summary>
    [Description("SQLite数据库")]
    SQLite = 2,

    /// <summary>SqlCe数据库</summary>
    [Description("PostgreSQL数据库")]
    PostgreSQL = 3,
}