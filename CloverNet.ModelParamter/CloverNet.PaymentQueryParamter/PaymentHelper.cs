using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloverNet.PaymentParamter
{
    public class PaymentHelper
    {
        public static string AutoSubmitForm(string url, string formParamter)
        {
            return $@"<form action='{url}' method='post'>{formParamter}<input type='submit'></form>
                        <script>
                            document.querySelector('[action = \'{url}\']').submit();
                        </script>";
        }

    }
}
