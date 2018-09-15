using CloverNet.ModelParamter;
using CloverNet.WebDataConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CloverNet.PaymentQueryParamter
{
    public class PaymentQueryParamterBuilder : ModelParamterBuilder
    {
        protected QueryParamterConfig Config { get; set; } = new QueryParamterConfig();
        protected IDictionary<string, object> SignParamter { get; set; } = new Dictionary<string, object>();
        
        public static PaymentQueryParamterBuilder Create()
        {
            return new PaymentQueryParamterBuilder();
        }

        public new PaymentQueryParamterBuilder AddParamter(Expression<Func<dynamic>> expression)
        {
            base.AddParamter(expression);
            return this;
        }

        public new PaymentQueryParamterBuilder AddParamter(string key, object value)
        {
            base.AddParamter(key, value);
            return this;
        }

        public PaymentQueryParamterBuilder AddSignParamter(Expression<Func<dynamic>> expression)
        {
            this.SignParamter.Add(ModelParamter.GetPropertyValue(expression));
            return this;
        }

        public PaymentQueryParamterBuilder AddSignParamter(string key, object value)
        {
            this.SignParamter.Add(key, value);
            return this;
        }

        public PaymentQueryParamterBuilder AddSecretKey(string key, object value)
        {
            this.Config.SecretKey = new KeyValuePair<string, object>(key, value);
            return this;
        }

        public PaymentQueryParamterBuilder AddSignatureParamter(string key)
        {
            this.Config.SignatureParamter = key;
            return this;
        }


        public PaymentQueryParamterBuilder WithSortedSortFlag()
        {
            this.Config.SortFlag = "Sortable";
            return this;
        }

        public PaymentQueryParamterBuilder WithNormalSortFlag()
        {
            this.Config.SortFlag = "Normal";
            return this;
        }


        public PaymentQueryParamterBuilder UseFormConverter()
        {
            this.Config.Converter = "Form";
            return this;
        }

        public PaymentQueryParamterBuilder UseUrlConverter()
        {
            this.Config.Converter = "Url";
            return this;
        }

        public PaymentQueryParamterBuilder UseJsonConverter()
        {
            this.Config.Converter = "Json";
            return this;
        }


        public PaymentQueryParamterBuilder UseSHA256()
        {
            this.Config.SignatureAlgorithm = "SHA256";
            return this;
        }

        public PaymentQueryParamterBuilder UseMD5()
        {
            this.Config.SignatureAlgorithm = "MD5";
            return this;
        }

        public PaymentQueryParamterBuilder UseDES(string signatureSecretKey)
        {
            this.Config.SignatureAlgorithm = "DES";
            this.Config.SignatureSecretKey = signatureSecretKey;
            return this;
        }

        public PaymentQueryParamterBuilder UseAES(string signatureSecretKey)
        {
            this.Config.SignatureAlgorithm = "AES";
            this.Config.SignatureSecretKey = signatureSecretKey;
            return this;
        }

        protected virtual void SetSecretKey()
        {
            this.SignParamter.Add(this.Config.SecretKey);
        }

        protected virtual void SetSortFlag()
        {
            switch (this.Config.SortFlag)
            {
                case "Sortable":
                    this.SignParamter = new SortedDictionary<string, object>(this.SignParamter);
                    break;
                case "Normal":
                default:
                    break;
            }
        }

        protected virtual void SetSignatureAlgorithm()
        {
            string _orignText = this.SignParamter.Values.Select(m => m?.ToString()).Aggregate((m, n) => m + n);
            string _signResult = string.Empty;

            switch (this.Config.SignatureAlgorithm)
            {
                case "SHA256":
                    _signResult = SecurityTool.SecurityTool.SHA256.Encrypt(_orignText);
                    break;
                case "MD5":
                    _signResult = SecurityTool.SecurityTool.MD5.Encrypt(_orignText);
                    break;
                case "DES":
                    _signResult = SecurityTool.SecurityTool.DES.Encrypt(_orignText, this.Config.SignatureSecretKey);
                    break;
                case "AES":
                    _signResult = SecurityTool.SecurityTool.AES.Encrypt(_orignText, this.Config.SignatureSecretKey);
                    break;
            }

            this.SignParamter.Add(this.Config.SignatureParamter, _signResult);
        }

        protected virtual void SetConverter()
        {
            Type _type = null;

            switch (this.Config.Converter)
            {
                case "Form":
                    _type = typeof(FormConverter);
                    break;
                case "Url":
                    _type = typeof(UrlConverter);
                    break;
                case "Json":
                    _type = typeof(UrlConverter);
                    break;
            }

            if (_type == null) return;

            this.SignParamter = this.SignParamter.Concat(this.Paramter).ToDictionary(m => m.Key, m => m.Value);

            this.Config.Result = ConverterFactory.GetService(_type).Convert(this.SignParamter);
        }

        public virtual string Build()
        {
            SetSecretKey();
            SetSortFlag();
            SetSignatureAlgorithm();
            SetConverter();

            return this.Config.Result;
        }

    }
}
