using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloverNet.PaymentParamter
{
    public class NotifyParamterConfig
    {
        public KeyValuePair<string, object> SecretKey { get; set; }

        public IDictionary<string, object> SignParamter { get; set; } = new Dictionary<string, object>();
        /// <summary>
        /// 签名结果属性名
        /// </summary>
        public string SignatureParamter { get; set; }

        /// <summary>
        /// 签名算法
        /// </summary>
        public string SignatureAlgorithm { get; set; }

        /// <summary>
        /// 签名密钥
        /// </summary>
        public string SignatureSecretKey { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public string SortFlag { get; set; }

        public bool Result { get; set; }
    }
}
