using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.OrderServices.Exceptions
{
    public class BasketNotFoundException: Exception
    {
        public BasketNotFoundException() : base()
        {

        }
        public BasketNotFoundException(string message) : base(message)
        {

        }
        public BasketNotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
