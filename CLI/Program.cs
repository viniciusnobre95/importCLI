using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Inicio();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: ", ex.Message);
            }
            
        }

        static void selectionCase(string selecao){
            switch (selecao)
            {
                case "1":
                    Console.WriteLine("Opção 1 selecionada.");
                    LerArquivo();
                    break;
                case "2":
                    Console.WriteLine("Opção 2 selecionada.");
                    ListarArquivosImportados();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    SelecaoInvalida();
                    break;
            }
        }   

        static void Inicio()
        {
            Console.WriteLine("");
            Console.WriteLine("Bem-vindo ao programa de importação de dados IMDB. Por favor, digite o número referente a uma das opções a seguir:");
            Console.WriteLine("");
            Console.WriteLine("1 - Importar registros de um arquivo.");
            Console.WriteLine("2 - Listar arquivos já importados.");
            Console.WriteLine("3 - Sair.");
            Console.WriteLine("");
            Console.WriteLine("Digite o valor desejado: ");
            var selecao = Console.ReadLine();

            selectionCase(selecao);
        }

        static void SelecaoInvalida()
        {
            Console.WriteLine("");
            Console.WriteLine("Valor informado incorretamente, por favor digite o número da opção desejada:");
            Console.WriteLine("");
            Console.WriteLine("1 - Importar registros de um arquivo.");
            Console.WriteLine("2 - Listar arquivos já importados.");
            Console.WriteLine("3 - Sair.");
            Console.WriteLine("");
            Console.WriteLine("Digite o valor desejado: ");
            var selecao = Console.ReadLine();
            selectionCase(selecao);
        }

        static void LerArquivo()
        {
            Console.WriteLine("Digite o caminho do arquivo: ");
            var caminhoArquivo = Console.ReadLine();

            if(string.IsNullOrEmpty(caminhoArquivo))
            {
                ExtensaoOuArquivoInvalido();
            }

            EnviarArquivo(caminhoArquivo);
        }

        static void ExtensaoOuArquivoInvalido()
        {
            Console.WriteLine("");
            Console.WriteLine("Extensão ou arquivo informado incorretamente, por favor tente novamente:");
            Console.WriteLine("");
            Console.WriteLine("1 - Importar registros de um arquivo.");
            Console.WriteLine("2 - Listar arquivos já importados.");
            Console.WriteLine("3 - Sair.");
            Console.WriteLine("");
            Console.WriteLine("Digite o valor desejado: ");
            var selecao = Console.ReadLine();
            selectionCase(selecao);
        }

        public static void EnviarArquivo(string caminhoArquivo)
        {
            try
            {
                var fileName = Path.GetFileName(caminhoArquivo);

                if (!fileName.EndsWith(".tsv"))
                {
                    ExtensaoOuArquivoInvalido();
                }

                var url = "http://localhost:44397/api/import/Importar";
                using (var httpCliente = new HttpClient())
                {
                    using var requestContent = new MultipartFormDataContent();
                    using var fileStream = File.OpenRead(caminhoArquivo);
                    requestContent.Add(new StreamContent(fileStream), "file", fileName);
                    var response = httpCliente.PostAsync(url, requestContent).Result;

                    var resultado = response.Content.ReadAsStringAsync().Result;
                    var message = JsonConvert.DeserializeObject<BaseResponse>(resultado);

                    Console.WriteLine("");
                    Console.WriteLine($"Resultado: { message.Mensagem } ");

                    RealizarNovaImportacao();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void ListarArquivosImportados()
        {
            try
            {

                var url = "http://localhost:44397/api/import/ListarArquivosImportados";
                using (var httpCliente = new HttpClient())
                {
                    var response = httpCliente.GetStringAsync(url).Result;

                    var resultado = response;

                    var message = JsonConvert.DeserializeObject<BaseResponse>(resultado);

                    var listagem = JsonConvert.DeserializeObject<List<TblArquivo>>(message.Resultado);

                    if (message.Status)
                    {
                        Console.WriteLine("");
                        Console.WriteLine($"Resultado: { message.Mensagem } ");
                        foreach(var arquivo in listagem)
                        {
                            Console.WriteLine($"-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine($"Id: {arquivo.ArquivoId} | Data de Importação: {arquivo.DthImport} | Quantidade de registros: { arquivo.QuantidadeRegistros } | Quantidade de registros importados: { arquivo.QuantidadeRegistrosImportados}");
                        }
                    } else
                    {
                        Console.WriteLine("");
                        Console.WriteLine($"Resultado: { message.Mensagem } ");
                    }

                    RealizarNovaImportacao();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine($"Erro: {ex.Message}");
                RealizarNovaImportacao();
            }
        }

        static void RealizarNovaImportacao()
        {
            Console.WriteLine("");
            Console.WriteLine("Deseja realizar nova importação ou listar arquivos?");
            Console.WriteLine("");
            Console.WriteLine("1 - Importar registros de um arquivo.");
            Console.WriteLine("2 - Listar arquivos já importados.");
            Console.WriteLine("3 - Sair.");
            Console.WriteLine("");
            Console.WriteLine("Digite o valor desejado: ");
            var selecao = Console.ReadLine();
            selectionCase(selecao);
        }
    }
}
