using Intacct.SDK.Functions.Common.NewQuery.QueryFilter;
using Intacct.SDK.Functions.Common.NewQuery.QuerySelect;
using Intacct.SDK.Functions.Common.NewQuery;
using Intacct.SDK.Xml.Response;
using Intacct.SDK.Xml;
using Intacct.SDK;
using Newtonsoft.Json;

namespace InvoiceSyncTool.Utilities
{
    public class ValidateBillNumberAvailability
    {
        public static bool Execute(OnlineClient client, string invoiceNumber, string vendorId)
        {
            List<IFilter> filterList =
            [
                (new Filter("RECORDID")).SetLike(invoiceNumber),
                (new Filter("VENDORID")).SetLike(vendorId)
            ];
            AndOperator filter = new(filterList);

            SelectBuilder selectBuilder = new();
            ISelect[] fields = selectBuilder.
                Fields(["RECORDID"]).
                GetFields();

            QueryFunction query = new()
            {
                SelectFields = fields,
                FromObject = "APBILL",
                Filter = filter
            };

            OnlineResponse response = client.Execute(query).Result;
            Result result = response.Results[0];

            dynamic json = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(result.Data))!;
            return json?.First != null;
        }
    }
}
