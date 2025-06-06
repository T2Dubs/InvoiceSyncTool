using Intacct.SDK;
using Microsoft.Extensions.Configuration;

namespace InvoiceSyncTool.Utilities
{
    public class CreateAccountingClient
    {
        public static OnlineClient Execute(IConfiguration appConfig, string locationCode)
        {
            string entityCode = locationCode.Substring(0,10);
            ClientConfig config = new()
            {
                SenderId = appConfig["AccountingCredentials:SenderId"],
                SenderPassword = appConfig["AccountingCredentials:SenderPassword"],
                CompanyId = appConfig["AccountingCredentials:CompanyId"],
                UserId = appConfig["AccountingCredentials:UserId"],
                UserPassword = appConfig["AccountingCredentials:UserPassword"],
                EntityId = entityCode,
            };

            return new OnlineClient(config);
        }
    }
}
