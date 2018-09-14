using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloverNet.ModelParamter
{
    public interface IModelParamter
    {
        string GetPropertyName(Expression<Func<dynamic>> expression );

        MemberInfo GetProperty(Expression<Func<dynamic>> expression);

        KeyValuePair<string,object> GetPropertyValue(Expression<Func<dynamic>> expression);
    }
}
