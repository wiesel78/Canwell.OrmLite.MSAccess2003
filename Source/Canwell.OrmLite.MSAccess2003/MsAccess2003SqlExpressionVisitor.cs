using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Canwell.OrmLite.MSAccess2003.Extensions;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace Canwell.OrmLite.MSAccess2003
{
    public class MsAccess2003SqlExpressionVisitor<T> : SqlExpressionVisitor<T>
    {
        public override string LimitExpression
        {
            get
            {
                var rows = Rows.HasValue ? string.Format(" TOP {0} ", Rows.Value) : string.Empty;
                return string.Format("{0}", rows);
            }
        }

        protected override string ApplyPaging(string sql)
        {
            var s = "SELECT";

            sql = sql.Insert(sql.IndexOf(s, StringComparison.CurrentCultureIgnoreCase) + s.Length, LimitExpression);
            return sql;
        }
    }
}
