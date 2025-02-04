using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MyFunctionApp.FileUploadFunction
{
    public static class FileUploadFunction
    {
        [FunctionName("FileUploadFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            // Binding de saída para o Blob Storage; o nome do container é "uploads"
            [Blob("uploads/{rand-guid}.txt", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream outputBlob,
            ILogger log)
        {
            log.LogInformation("Recebida requisição de upload de arquivo.");
            
            // Lê o conteúdo do corpo da requisição
            string fileContent = await new StreamReader(req.Body).ReadToEndAsync();
            byte[] byteArray = Encoding.UTF8.GetBytes(fileContent);
            await outputBlob.WriteAsync(byteArray, 0, byteArray.Length);
            
            return new OkObjectResult("Arquivo salvo com sucesso no Storage.");
        }
    }
}