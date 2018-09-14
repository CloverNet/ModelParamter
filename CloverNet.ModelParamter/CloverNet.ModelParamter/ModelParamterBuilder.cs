using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CloverNet.ModelParamter
{
    public class ModelParamterBuilder
    {
        public IModelParamter ModelParamter { get; set; } = new DefaultModelParamter();

        public IDictionary<string, object> Paramter { get; set; }

        public virtual ModelParamterBuilder AddParamter(Expression<Func<dynamic>> expression)
        {
            this.Paramter.Add(ModelParamter.GetPropertyValue(expression));
            return this;
        }

    }
}
