using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Conexao_MongoDB
{
    public class connection_API
    {
        public JsonNode RequestRealTime(string device)
        {
            using(var client = new RestClient("http://46.17.108.113:1026"))
            {
                try
                {
                    var request = new RestRequest("v2/entities/urn:ngsi-ld:Flux:010/attrs/flux", Method.Get);
                    request.AddHeader("fiware-service", "smart");
                    request.AddHeader("fiware-servicepath", "/");
                    request.AddHeader("accept", "application/json");

                    return ExecuteRequest(request, client, true);
                }
                catch (Exception e)
                {
                    ErrorObject erro = new ErrorObject
                    {
                        Message = e.Message
                    };

                    var error = JsonSerializer.Serialize(erro);
                    return error;
                }
            }
        }

        public JsonNode RequestHistory(string device, int lastN)
        {
            using (var client = new RestClient("http://46.17.108.113:8666"))
            {
                try
                {
                    RestRequest request = new RestRequest($"STH/v1/contextEntities/type/Flux/id/urn:ngsi-ld:{device}/attributes/flux?lastN={lastN}", Method.Get);
                    request.AddHeader("fiware-service", "smart");
                    request.AddHeader("fiware-servicepath", "/");

                    return ExecuteRequest(request, client, false);

                }
                catch(Exception e)
                {
                    ErrorObject erro = new ErrorObject
                    {
                        Message = e.Message
                    };

                    var error = JsonSerializer.Serialize(erro);
                    return error;
                }
            }
        }

        public JsonNode RequestHistory(string device, DateTime dateFrom, DateTime dateTo, int hLimit, int hOffset)
        {
            using (var client = new RestClient("http://46.17.108.113:8666"))
            {
                try
                {
                    string dateFrom_String = dateFrom.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                    string dateTo_String = dateTo.ToString("yyyy-MM-ddTHH:mm:ss.fff");

                    RestRequest request = new RestRequest($"STH/v1/contextEntities/type/Flux/id/urn:ngsi-ld:Flux:010/attributes/flux?dateFrom={dateFrom_String}&dateTo={dateTo_String}&hLimit={hLimit}&hOffset={hOffset}", Method.Get);
                    request.AddHeader("fiware-service", "smart");
                    request.AddHeader("fiware-servicepath", "/");

                    return ExecuteRequest(request, client, false);
                }
                catch(Exception e)
                {
                    ErrorObject erro = new ErrorObject
                    {
                        Message = e.Message
                    };

                    var error = JsonSerializer.Serialize(erro);
                    return error;
                }
            }
        }

        private JsonNode ExecuteRequest(RestRequest request, RestClient client, bool realtime)
        {
            RestResponse response = client.Execute(request);

            if (Convert.ToInt32(response.StatusCode) == 200)
            {
                JsonNode data = JsonSerializer.Deserialize<JsonNode>(response.Content);
                JsonNode listValues;
                if(realtime)
                    listValues = data["value"];
                else
                    listValues = data["contextResponses"][0]["contextElement"]["attributes"][0]["values"];
                
                return listValues;
            }
            else
            {
                throw new Exception($"Erro ao acessar a API: {Convert.ToInt32(response.StatusCode)}||| {response.ErrorException}||| {response.Content}");
            }
        }
    }
}
