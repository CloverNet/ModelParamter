using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CloverNet.ModelParamter
{
    public abstract class ModelParamterBuilder
    {
        protected IModelParamter ModelParamter { get; set; } = new DefaultModelParamter();

        protected IDictionary<string, object> Paramter { get; set; }


        public virtual ModelParamterBuilder AddParamter(Expression<Func<dynamic>> expression)
        {
            this.Paramter.Add(ModelParamter.GetPropertyValue(expression));
            return this;
        }
        public virtual ModelParamterBuilder AddParamter(string key ,object value)
        {
            this.Paramter.Add(key,value);
            return this;
        }
    }
}
