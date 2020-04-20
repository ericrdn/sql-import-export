using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using Newtonsoft.Json;

namespace sql_import
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Conectando...");

            var conn = new SqlConnection(@"Server=localhost;Database=DBTESTE;User Id=sa;Password=<YourStrong!Passw0rd>;");

            Console.WriteLine("Efetuando Consulta...");

            var Dados = conn.Query("SELECT * FROM DADOS");

            Console.WriteLine("Convertendo para JSON...");

            using (StreamWriter file = File.CreateText(@"dados.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Dados);
            }

        }
    }
}
