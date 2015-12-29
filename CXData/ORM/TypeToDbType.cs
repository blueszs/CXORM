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
        public static DbType GetDbType<T>(this T entity) where T : Type
        {
            DbType dbtype;
            TypeCode typecode = Type.GetTypeCode(entity);
            switch (typecode)
            {
                case TypeCode.Boolean:
                    dbtype = DbType.Boolean;
                    break;
                case TypeCode.Byte:
                    dbtype = DbType.Byte;
                    break;
                case TypeCode.Char:
                    dbtype = DbType.String;
                    break;
                case TypeCode.DateTime:
                    dbtype = DbType.DateTime;
                    break;
                case TypeCode.Decimal:
                    dbtype = DbType.Decimal;
                    break;
                case TypeCode.Double:
                    dbtype = DbType.Double;
                    break;
                case TypeCode.Int32:
                    dbtype = DbType.Int32;
                    break;
                case TypeCode.Int16:
                    dbtype = DbType.Int16;
                    break;
                case TypeCode.Int64:
                    dbtype = DbType.Int64;
                    break;
                case TypeCode.Object:
                    dbtype = Equals(entity, typeof(Guid))? DbType.Guid : DbType.Object;
                    break;
                case TypeCode.SByte:
                    dbtype = DbType.SByte;
                    break;
                case TypeCode.Single:
                    dbtype = DbType.Single;
                    break;
                case TypeCode.String:
                    dbtype = DbType.String;
                    break;
                case TypeCode.UInt16:
                    dbtype = DbType.UInt16;
                    break;
                case TypeCode.UInt32:
                    dbtype = DbType.UInt32;
                    break;
                case TypeCode.UInt64:
                    dbtype = DbType.UInt64;
                    break;
                default:
                    dbtype = DbType.String;
                    break;
            }
            return dbtype;
        }

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
