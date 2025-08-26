using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.User.Application.Services;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
using Spectre.Console;
using Microsoft.Extensions.Configuration;

namespace campuslovejorge_edwin.Src.Modules.User.UI
{
    public class MenuUser
    {
        private readonly UserService _service;

        public MenuUser(UserService service)
        {
            _service = service;
        }

        public async Task ShowMenuAsync()
        {
            var exit = false;
            
            while (!exit)
            {
                AnsiConsole.Clear();
                
                // Título principal con estilo
                AnsiConsole.Write(
                    new FigletText("CAMPUS LOVE")
                        .Centered()
                        .Color(Color.Green));
                
                AnsiConsole.Write(
                    new Rule("[yellow]Sistema de Gestión de Usuarios[/]")
                        .RuleStyle("blue")
                        .Centered());

                // Menú principal con opciones numeradas
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Selecciona una opción:[/]")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "👤 Agregar Usuario",
                            "📋 Listar Usuarios", 
                            "✏️  Editar Usuario",
                            "🗑️  Eliminar Usuario",
                            "❌ Salir"
                        }));

                switch (choice)
                {
                    case "👤 Agregar Usuario":
                        await AddUserAsync();
                        break;
                    case "📋 Listar Usuarios":
                        await ListUsersAsync();
                        break;
                    case "✏️  Editar Usuario":
                        await EditUserAsync();
                        break;
                    case "🗑️  Eliminar Usuario":
                        await DeleteUserAsync();
                        break;
                    case "❌ Salir":
                        exit = true;
                        break;
                }
            }
            
            AnsiConsole.Write(
                new Rule("[green]¡Gracias por usar Campus Love![/]")
                    .RuleStyle("green")
                    .Centered());
        }

        private async Task AddUserAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Agregar Nuevo Usuario[/]")
                    .RuleStyle("blue")
                    .Centered());

            try
            {

                var name = AnsiConsole.Ask<string>("[green]Nombre completo:[/]");
                var email = AnsiConsole.Ask<string>("[green]Email:[/]");
                var password = AnsiConsole.Prompt(
                    new TextPrompt<string>("[green]Contraseña:[/]")
                        .Secret());
                
                var birthdate = AnsiConsole.Prompt(
                    new TextPrompt<DateTime>("[green]Fecha de nacimiento (yyyy-MM-dd):[/]")
                        .ValidationErrorMessage("[red]Formato de fecha inválido. Use yyyy-MM-dd[/]")
                        .DefaultValue(DateTime.Now.AddYears(-18)));

                // Mostrar opciones de género y orientación
                var genderId = await SelectGenderAsync();
                var orientationId = await SelectOrientationAsync();

                var user = new UserEntity
                {
                    FullName = name,
                    Email = email,
                    PasswordHash = password, // En producción deberías hashear la contraseña
                    Birthdate = birthdate,
                    GenderId = genderId,
                    OrientationId = orientationId
                };

                // Mostrar resumen antes de guardar
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn("[blue]Campo[/]")
                    .AddColumn("[green]Valor[/]");

                table.AddRow("Nombre", user.FullName);
                table.AddRow("Email", user.Email);
                table.AddRow("Fecha de nacimiento", user.Birthdate.ToString("yyyy-MM-dd"));
                table.AddRow("Género", await GetGenderNameAsync(user.GenderId));
                table.AddRow("Orientación", await GetOrientationNameAsync(user.OrientationId));

                AnsiConsole.Write(table);

                if (AnsiConsole.Confirm("[yellow]¿Confirmar la creación del usuario?[/]"))
                {
                    await _service.AddUserAsync(user);
                    AnsiConsole.MarkupLine("[green]✅ Usuario agregado exitosamente![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]Operación cancelada.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            }

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task ListUsersAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Lista de Usuarios[/]")
                    .RuleStyle("blue")
                    .Centered());

            try
            {
                var users = await _service.GetAllUsersAsync();
                
                if (!users.Any())
                {
                    AnsiConsole.MarkupLine("[yellow]No hay usuarios registrados.[/]");
                }
                else
                {
                                    var table = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn("[blue]ID[/]")
                    .AddColumn("[green]Nombre[/]")
                    .AddColumn("[blue]Email[/]")
                    .AddColumn("[blue]Fecha Nac.[/]")
                    .AddColumn("[yellow]Género[/]")
                    .AddColumn("[cyan]Orientación[/]")
                    .AddColumn("[blue]Creado[/]");

                    foreach (var user in users)
                    {
                        table.AddRow(
                            user.UserId.ToString(),
                            user.FullName,
                            user.Email,
                            user.Birthdate.ToString("yyyy-MM-dd"),
                            await GetGenderNameAsync(user.GenderId),
                            await GetOrientationNameAsync(user.OrientationId),
                            user.CreatedAt.ToString("yyyy-MM-dd")
                        );
                    }

                    AnsiConsole.Write(table);
                    AnsiConsole.MarkupLine($"[green]Total de usuarios: {users.Count}[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            }

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task EditUserAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Editar Usuario[/]")
                    .RuleStyle("blue")
                    .Centered());

            try
            {
                var id = AnsiConsole.Ask<int>("[green]ID del usuario a editar:[/]");
                var user = await _service.GetUserByIdAsync(id);
                
                if (user == null)
                {
                    AnsiConsole.MarkupLine("[red]❌ Usuario no encontrado.[/]");
                }
                else
                {
                    // Mostrar información actual
                    AnsiConsole.MarkupLine($"[blue]Editando usuario: {user.FullName}[/]");
                    
                    var currentTable = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[blue]Campo[/]")
                        .AddColumn("[green]Valor Actual[/]");

                    currentTable.AddRow("Nombre", user.FullName);
                    currentTable.AddRow("Email", user.Email);
                    currentTable.AddRow("Fecha de nacimiento", user.Birthdate.ToString("yyyy-MM-dd"));
                    currentTable.AddRow("Género", await GetGenderNameAsync(user.GenderId));
                    currentTable.AddRow("Orientación", await GetOrientationNameAsync(user.OrientationId));

                    AnsiConsole.Write(currentTable);
                    AnsiConsole.WriteLine();

                    // Solicitar cambios
                    var newName = AnsiConsole.Ask<string>($"[green]Nuevo nombre (actual: {user.FullName}):[/]", user.FullName);
                    var newEmail = AnsiConsole.Ask<string>($"[green]Nuevo email (actual: {user.Email}):[/]", user.Email);
                    
                    if (AnsiConsole.Confirm("[yellow]¿Deseas cambiar la fecha de nacimiento?[/]"))
                    {
                        user.Birthdate = AnsiConsole.Prompt(
                            new TextPrompt<DateTime>("[green]Nueva fecha de nacimiento (yyyy-MM-dd):[/]")
                                .ValidationErrorMessage("[red]Formato de fecha inválido. Use yyyy-MM-dd[/]")
                                .DefaultValue(user.Birthdate));
                    }

                    if (AnsiConsole.Confirm("[yellow]¿Deseas cambiar el género?[/]"))
                    {
                        user.GenderId = await SelectGenderAsync();
                    }

                    if (AnsiConsole.Confirm("[yellow]¿Deseas cambiar la orientación?[/]"))
                    {
                        user.OrientationId = await SelectOrientationAsync();
                    }

                    user.FullName = newName;
                    user.Email = newEmail;

                    if (AnsiConsole.Confirm("[yellow]¿Confirmar los cambios?[/]"))
                    {
                        await _service.UpdateUserAsync(user);
                        AnsiConsole.MarkupLine("[green]✅ Usuario actualizado exitosamente![/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[yellow]Operación cancelada.[/]");
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            }

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task DeleteUserAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[red]Eliminar Usuario[/]")
                    .RuleStyle("red")
                    .Centered());

            try
            {
                var id = AnsiConsole.Ask<int>("[red]ID del usuario a eliminar:[/]");
                var user = await _service.GetUserByIdAsync(id);
                
                if (user == null)
                {
                    AnsiConsole.MarkupLine("[red]❌ Usuario no encontrado.[/]");
                }
                else
                {
                    // Mostrar información del usuario a eliminar
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[red]Campo[/]")
                        .AddColumn("[yellow]Valor[/]");

                    table.AddRow("ID", user.UserId.ToString());
                    table.AddRow("Nombre", user.FullName);
                    table.AddRow("Email", user.Email);
                    table.AddRow("Fecha de nacimiento", user.Birthdate.ToString("yyyy-MM-dd"));

                    AnsiConsole.Write(table);
                    AnsiConsole.WriteLine();

                    if (AnsiConsole.Confirm($"[red]⚠️  ¿Estás seguro de que deseas eliminar al usuario '{user.FullName}'?[/]"))
                    {
                        if (AnsiConsole.Confirm("[red]Esta acción no se puede deshacer. ¿Confirmar eliminación?[/]"))
                        {
                            await _service.DeleteUserAsync(id);
                            AnsiConsole.MarkupLine("[green]✅ Usuario eliminado exitosamente![/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[yellow]Eliminación cancelada.[/]");
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[yellow]Operación cancelada.[/]");
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            }

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task<int> SelectGenderAsync()
        {
            // Obtener opciones de género desde la base de datos
            var genderOptions = await GetGenderOptionsFromDatabaseAsync();
            
            if (!genderOptions.Any())
            {
                // Si no hay datos en la base de datos, usar opciones por defecto
                genderOptions = new Dictionary<string, int>
                {
                    { "Masculino", 1 },
                    { "Femenino", 2 },
                    { "No binario", 3 }
                };
            }

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Selecciona el género:[/]")
                    .PageSize(10)
                    .AddChoices(genderOptions.Keys));

            return genderOptions[choice];
        }

        private async Task<int> SelectOrientationAsync()
        {
            // Obtener opciones de orientación desde la base de datos
            var orientationOptions = await GetOrientationOptionsFromDatabaseAsync();
            
            if (!orientationOptions.Any())
            {
                // Si no hay datos en la base de datos, usar opciones por defecto
                orientationOptions = new Dictionary<string, int>
                {
                    { "Heterosexual", 1 },
                    { "Homosexual", 2 },
                    { "Bisexual", 3 },
                    { "Pansexual", 4 }
                };
            }

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Selecciona la orientación:[/]")
                    .PageSize(10)
                    .AddChoices(orientationOptions.Keys));

            return orientationOptions[choice];
        }

        private async Task<Dictionary<string, int>> GetGenderOptionsFromDatabaseAsync()
        {
            try
            {
                // Aquí deberías hacer una llamada al repositorio para obtener los géneros
                // Por ahora, voy a simular la obtención desde la base de datos
                // En un sistema real, esto vendría de un servicio o repositorio
                var genderOptions = new Dictionary<string, int>();
                
                // Simular consulta a la base de datos
                using var connection = new MySqlConnector.MySqlConnection(GetConnectionString());
                await connection.OpenAsync();
                
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT gender_id, name FROM gender ORDER BY name";
                
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var id = reader.GetInt32("gender_id");
                    var name = reader.GetString("name");
                    genderOptions[name] = id;
                }
                
                return genderOptions;
            }
            catch
            {
                // Si hay error, retornar opciones por defecto
                return new Dictionary<string, int>();
            }
        }

        private async Task<Dictionary<string, int>> GetOrientationOptionsFromDatabaseAsync()
        {
            try
            {
                // Aquí deberías hacer una llamada al repositorio para obtener las orientaciones
                // Por ahora, voy a simular la obtención desde la base de datos
                var orientationOptions = new Dictionary<string, int>();
                
                // Simular consulta a la base de datos
                using var connection = new MySqlConnector.MySqlConnection(GetConnectionString());
                await connection.OpenAsync();
                
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT orientation_id, name FROM orientation ORDER BY name";
                
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var id = reader.GetInt32("orientation_id");
                    var name = reader.GetString("name");
                    orientationOptions[name] = id;
                }
                
                return orientationOptions;
            }
            catch
            {
                // Si hay error, retornar opciones por defecto
                return new Dictionary<string, int>();
            }
        }

        private string GetConnectionString()
        {
            // Obtener la cadena de conexión desde la configuración
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
                
            return Environment.GetEnvironmentVariable("MYSQL_CONNECTION") 
                ?? config.GetConnectionString("MySqlDb") 
                ?? "server=localhost;database=examendb;user=campus2023;password=campus2023;";
        }

        private async Task<string> GetGenderNameAsync(int genderId)
        {
            var genderNames = new Dictionary<int, string>
            {
                { 1, "Masculino" },
                { 2, "Femenino" },
                { 3, "No binario" }
            };

            return genderNames.TryGetValue(genderId, out var name) ? name : "Desconocido";
        }

        private async Task<string> GetOrientationNameAsync(int orientationId)
        {
            var orientationNames = new Dictionary<int, string>
            {
                { 1, "Heterosexual" },
                { 2, "Homosexual" },
                { 3, "Bisexual" },
                { 4, "Pansexual" }
            };

            return orientationNames.TryGetValue(orientationId, out var name) ? name : "Desconocido";
        }
    }
}

