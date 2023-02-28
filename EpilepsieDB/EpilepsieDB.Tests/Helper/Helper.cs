using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EpilepsieDB.Tests.Helper
{
    public static class Helper
    {
        public static bool PropertyHasAttribute<T>(string property, Type clazz)
        {
            PropertyInfo info = clazz.GetProperty(property);
            return Attribute.IsDefined(info, typeof(T));
        }

        public static bool MethodHasAttribute<T>(Expression<System.Action> expression)
        {
            MemberInfo member = MethodOf(expression);
            return MemberHasAttribute<T>(member);
        }

        public static bool TypeHasAttribute<T>(Type t)
        {
            return MemberHasAttribute<T>(t);
        }

        public static TValue TypeHasAttributeValue<TClass, TAttribute, TValue>(
        Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute
        {
            TAttribute attr = typeof(TClass).GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            return attr != null ? valueSelector(attr) : default(TValue);
        }

        private static bool MemberHasAttribute<T>(MemberInfo member)
        {
            const bool includeInherited = false;
            return member.GetCustomAttributes(typeof(T), includeInherited).Any();
        }

        private static MethodInfo MethodOf(Expression<System.Action> expression)
        {
            MethodCallExpression body = (MethodCallExpression)expression.Body;
            return body.Method;
        }
    }
}
