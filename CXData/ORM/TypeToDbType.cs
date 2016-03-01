using System;
using System.Data;

namespace CXData.ORM
{
    /// <summary>
    /// 将类型转化为数据库类型
    /// 20150625-周盛-添加
    /// </summary>
    public static class TypeToDbType
    {
        public static string ConvertDbType<T>(this T entity) where T : Type
        {
            string dbtypestr = "VARCHAR";
            TypeCode typecode = Type.GetTypeCode(entity);
            switch (typecode)
            {
                case TypeCode.Boolean:
                    dbtypestr = "BIT";
                    break;
                case TypeCode.Double:
                    dbtypestr = "FLOAT";
                    break;
                case TypeCode.Char:
                    dbtypestr = "CHAR";
                    break;
                case TypeCode.DateTime:
                    dbtypestr = "DATETIME";
                    break;
                case TypeCode.Decimal:
                    dbtypestr = "DECIMAL";
                    break;
                case TypeCode.Int32:
                case TypeCode.Int16:
                    dbtypestr = "INT";
                    break;
                case TypeCode.Int64:
                    dbtypestr = "BIGING";
                    break;
                case TypeCode.Object:
                    dbtypestr = Equals(entity, typeof(Guid)) ? "UNIQUEIDENTIFIER" : "OBJECT";
                    break;
                case TypeCode.String:
                    dbtypestr = "VARCHAR";
                    break;
            }
            return dbtypestr;
        }
    }
}
