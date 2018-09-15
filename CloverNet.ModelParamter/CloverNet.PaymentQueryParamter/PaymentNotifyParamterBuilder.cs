using CloverNet.ModelParamter;
using CloverNet.WebDataConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CloverNet.PaymentParamter
{
    public class PaymentNotifyParamterBuilder
    {
        protected IModelParamter ModelParamter = new DefaultModelParamter();
        protected NotifyParamterConfig Config { get; set; } = new NotifyParamterConfig();
        protected IList<string> SignParamter { get; set; } = new List<string>();
        
        public static PaymentNotifyParamterBuilder Create()
        {
            return new PaymentNotifyParamterBuilder();
        }

        public PaymentNotifyParamterBuilder AddSignParamter<T>(Expression<Func<T,dynamic>> expression) where T:class
        {
            this.SignParamter.Add(ModelParamter.GetPropertyName<T>(expression));
            return this;
        }

        public PaymentNotifyParamterBuilder AddSignParamter(string key)
        {
            this.SignParamter.Add(key);
            return this;
        }

        public PaymentNotifyParamterBuilder AddSecretKey(string key, object value)
        {
            this.Config.SecretKey = new KeyValuePair<string, object>(key, value);
            return this;
        }

        public PaymentNotifyParamterBuilder AddSignatureParamter(string key)
        {
            this.Config.SignatureParamter = key;
            return this;
        }


        public PaymentNotifyParamterBuilder WithSortedSortFlag()
        {
            this.Config.SortFlag = "Sortable";
            return this;
        }

        public PaymentNotifyParamterBuilder WithNormalSortFlag()
        {
            this.Config.SortFlag = "Normal";
            return this;
        }

        public PaymentNotifyParamterBuilder UseSHA256()
        {
            this.Config.SignatureAlgorithm = "SHA256";
            return this;
        }

        public PaymentNotifyParamterBuilder UseMD5()
        {
            this.Config.SignatureAlgorithm = "MD5";
            return this;
        }

        public PaymentNotifyParamterBuilder UseDES(string signatureSecretKey)
        {
            this.Config.SignatureAlgorithm = "DES";
            this.Config.SignatureSecretKey = signatureSecretKey;
            return this;
        }

        public PaymentNotifyParamterBuilder UseAES(string signatureSecretKey)
        {
            this.Config.SignatureAlgorithm = "AES";
            this.Config.SignatureSecretKey = signatureSecretKey;
            return this;
        }

        protected virtual void SetSignParam()
        {
            foreach (var signParamter in this.SignParamter)
            {
                this.Config.SignParamter.Add(signParamter,HttpContext.Current.Request[signParamter]);
            }
        }

        protected virtual void SetSecretKey()
        {
            this.Config.SignParamter.Add(this.Config.SecretKey);
        }

        protected virtual void SetSortFlag()
        {
            switch (this.Config.SortFlag)
            {
                case "Sortable":
                    this.Config.SignParamter = new SortedDictionary<string, object>(this.Config.SignParamter);
                    break;
                case "Normal":
                default:
                    break;
            }
        }

        protected virtual void SetSignatureAlgorithm()
        {
            string _orignText = this.Config.SignParamter.Values.Select(m => m?.ToString()).Aggregate((m, n) => m + n);
            string __signResult = HttpContext.Current.Request[this.Config.SignatureParamter];

            string _signResult = string.Empty;
            string __orignText = string.Empty;

            switch (this.Config.SignatureAlgorithm)
            {
                case "SHA256":
                    _signResult = SecurityTool.SecurityTool.SHA256.Encrypt(_orignText);
                    break;
                case "MD5":
                    _signResult = SecurityTool.SecurityTool.MD5.Encrypt(_orignText);
                    break;
                case "DES":
                    __orignText = SecurityTool.SecurityTool.DES.Decrypt(__signResult, this.Config.SignatureSecretKey);
                    break;
                case "AES":
                    __orignText = SecurityTool.SecurityTool.AES.Decrypt(__signResult, this.Config.SignatureSecretKey);
                    break;
            }

            this.Config.Result = _signResult.Equals(__signResult, StringComparison.OrdinalIgnoreCase)|| _orignText.Equals(__orignText, StringComparison.OrdinalIgnoreCase);
        }

        public virtual bool Build()
        {
            SetSignParam();
            SetSecretKey();
            SetSortFlag();
            SetSignatureAlgorithm();

            return this.Config.Result;
        }

    }
}
