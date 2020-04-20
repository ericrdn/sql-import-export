using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using Newtonsoft.Json;

namespace sql_import
{
    class Program
    {

        static void Log(string Msg)
        {
            Console.WriteLine($"{DateTime.Now:dd/MM/yyyy HH:mm:ss.fff} - {Msg}");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Conectando...");

            var conn = new SqlConnection($@"Server=localhost;Database=master;User Id=sa;Password=<YourStrong!Passw0rd>;");

            Log("Levantando bases de dados...");

            var BancosdeDados = conn.Query("SELECT * FROM SYS.sysdatabases WHERE DBID > 4");

            foreach (var item in BancosdeDados)
            {
                Log($"{item.name}...");

                using (var conn_db = new SqlConnection($@"Server=localhost;Database={item.name};User Id=sa;Password=<YourStrong!Passw0rd>;"))
                {
                    var Tabelas = conn_db.Query("SELECT * FROM SYSOBJECTS WHERE XTYPE = 'U' ");

                    foreach (var tabela in Tabelas)
                    {
                        Log($"{item.name}.{tabela.name} - Consutando...");

                        var Dados = conn_db.Query($"SELECT * FROM {tabela.name} ");

                        Log($"{item.name}.{tabela.name} - Gravando...");



                        using (StreamWriter file = File.CreateText($@"data\{item.name}.{tabela.name}.json"))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Serialize(file, Dados);
                        }
                    }
                }
            }





            Console.WriteLine("Convertendo para JSON...");



        }
    }
}
