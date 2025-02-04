using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MyFunctionApp.CosmosDBFunction
{
    public static class CosmosDBFunction
    {
        [FunctionName("CosmosDBFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "cosmos/{id?}")] HttpRequest req,
            // Binding de entrada: busca um documento pelo {id} (se fornecido)
            [CosmosDB(
                databaseName: "MyDatabase",
                collectionName: "MyCollection",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{id}",
                PartitionKey = "{id}")] dynamic documentIn,
            // Binding de saída: para inserir um novo documento
            [CosmosDB(
                databaseName: "MyDatabase",
                collectionName: "MyCollection",
                ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<dynamic> documentsOut,
            ILogger log)
        {
            if (req.Method == HttpMethods.Post)
            {
                // Adiciona um novo documento ao Cosmos DB
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                await documentsOut.AddAsync(data);
                return new OkObjectResult("Documento inserido com sucesso.");
            }
            else
            {
                // Se for GET, retorna o documento (se o id foi fornecido) ou poderá ser implementado para listar/filtrar
                return new OkObjectResult(documentIn ?? "Nenhum documento encontrado.");
            }
        }
    }
}