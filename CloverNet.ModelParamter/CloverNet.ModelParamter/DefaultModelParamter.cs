using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloverNet.ModelParamter
{
    public class DefaultModelParamter : IModelParamter
    {
        public virtual MemberInfo GetProperty(Expression<Func<dynamic>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            return memberExpression?.Member;
        }

        public MemberInfo GetProperty<T>(Expression<Func<T, dynamic>> expression) where T : class
        {
            var memberExpression = expression.Body as MemberExpression;
            return memberExpression?.Member;
        }

        public virtual string GetPropertyName(Expression<Func<dynamic>> expression)
        {
             return GetProperty(expression)?.Name;
        }

        public string GetPropertyName<T>(Expression<Func<T, dynamic>> expression) where T : class
        {
            return GetProperty(expression)?.Name;
        }

        public virtual KeyValuePair<string, object> GetPropertyValue(Expression<Func<dynamic>> expression)
        {
            var key = GetPropertyName(expression);
            var value = expression.Compile().Invoke();
            return new KeyValuePair<string, object>(key, value);
        }
    }
}
