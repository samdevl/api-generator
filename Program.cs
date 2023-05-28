﻿using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
 using System.Diagnostics;


class Program
{
    static void Main()
    {
        // Solicitar al cliente los valores para la especificación
        Console.WriteLine("Ingrese el título de la API:");
        string title = Console.ReadLine();

        Console.WriteLine("Ingrese la versión de la API:");
        string version = Console.ReadLine();

        Console.WriteLine("Ingrese la descripción de la API:");
        string description = Console.ReadLine();

        // Crear la especificación OpenAPI
        var document = new OpenApiDocument
        {
            Info = new OpenApiInfo
            {
                Title = title,
                Version = version,
                Description = description
            },

            Servers = new List<OpenApiServer>
            {
                new OpenApiServer { Url = "http://petstore.swagger.io/api" } //url localhost donde genero el api
            },
            Paths = new OpenApiPaths
            {
                // Agregar endpoints adicionales según las necesidades del cliente
            }
        };

        // Mostrar mensaje de generación en progreso
        Console.WriteLine("Generando la especificación OpenAPI...");

        // works!
        string projectFolder = "console-api";
        
        string outputFolder = "/home/gonzalez_l/Escritorio/console/console-api/console-api";
        
        string openApiFile = Path.Combine(outputFolder, "console-api.json");
        
        // Guardar el archivo OpenAPI JSON en la carpeta especificada
        using (var streamWriter = new StreamWriter(openApiFile))
        {
            var jsonWriter = new OpenApiJsonWriter(streamWriter);
            document.SerializeAsV3(jsonWriter);
            jsonWriter.Flush();
        }

        // Mostrar mensaje de generación de API
        Console.WriteLine("Generación de la especificación OpenAPI completa. Generando la API...");

        string outputApi = "/home/gonzalez_l/Escritorio/console-api/console-api/Api";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "openapi-generator-cli",
            Arguments = $"generate -i {openApiFile} -g aspnetcore -o {outputApi} --additional-properties targetFramework=net6.0",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = new Process())
        {
            process.StartInfo = startInfo;
            process.Start();

            // Leer y mostrar la salida del proceso de generación de API
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }

            process.WaitForExit();
        }

        Console.WriteLine("Generación de la API completa.");
        Console.ReadLine();
        Environment.Exit(0);
    }
}