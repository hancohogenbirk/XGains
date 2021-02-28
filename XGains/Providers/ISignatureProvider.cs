using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XGains.Providers
{
    public interface ISignatureProvider
    {
        string SignRequest(string endpoint, ref Dictionary<string, string> urlParameters, string base64EncodedSecret);
    }
}
