using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABSDrive.Repositories
{
    public interface IPaymentValidator
    {
       decimal paymentGateway(String[] codeValues);
    }
}
