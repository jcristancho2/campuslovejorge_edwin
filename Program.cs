﻿using examencsharp.src.Shared.Helpers;

namespace examencsharp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            var context = DbContextFactory.Create();

            Console.WriteLine("Conexion creada");
        }
        catch
        {
            throw new Exception("Error al conectar con la base de datos");
        }
        
        
    }
}