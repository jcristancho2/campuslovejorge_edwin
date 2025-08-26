using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.User.Application.Services;
using campuslovejorge_edwin.Src.Modules.Interaction.Application.Services;
using campuslovejorge_edwin.Src.Modules.Interaction.UI;
using campuslovejorge_edwin.Src.Modules.Interaction.Infrastructure.Repositories;
using campuslovejorge_edwin.Src.Shared.Context;

namespace campuslovejorge_edwin.Src.Modules.MainMenu
{
    public class MainMenu
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;
        private readonly InteractionService _interactionService;
        private UserEntity? _currentUser;

        public MainMenu(AuthService authService, UserService userService, InteractionService interactionService)
        {
            _authService = authService;
            _userService = userService;
            _interactionService = interactionService;
        }

        public async Task StartAsync()
        {
            Console.WriteLine("DEBUG: StartAsync iniciado");
            var exit = false;
            
            while (!exit)
            {
                Console.WriteLine("DEBUG: Iniciando bucle del menú");
                AnsiConsole.Clear();
                
                // Título principal con estilo
                AnsiConsole.Write(
                    new FigletText("CAMPUS LOVE")
                        .Centered()
                        .Color(Color.Purple));
                
                AnsiConsole.Write(
                    new Rule("[yellow]Sistema de Citas Universitarias[/]")
                        .RuleStyle("blue")
                        .Centered());

                if (_currentUser == null)
                {
                    Console.WriteLine("DEBUG: Usuario no autenticado, mostrando menú de login");
                    // Usuario no autenticado
                    var choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[green]Bienvenido a Campus Love[/]")
                            .AddChoices(new[] {
                                "🔐 Iniciar Sesión",
                                "👤 Registrarse",
                                "❌ Salir"
                            }));

                    switch (choice)
                    {
                        case "🔐 Iniciar Sesión":
                            await LoginAsync();
                            break;
                        case "👤 Registrarse":
                            await RegisterAsync();
                            break;
                        case "❌ Salir":
                            exit = true;
                            break;
                    }
                    
                    // Solo continuar el bucle si el usuario no está autenticado
                    // Si se autenticó, el bucle continuará y mostrará el menú autenticado
                }
                else
                {
                    Console.WriteLine("DEBUG: Usuario autenticado, mostrando menú principal");
                    // Usuario autenticado
                    await ShowAuthenticatedMenuAsync();
                }
            }
            
            Console.WriteLine("DEBUG: Saliendo del bucle del menú");
            AnsiConsole.Write(
                new Rule("[green]¡Gracias por usar Campus Love![/]")
                    .RuleStyle("green")
                    .Centered());
        }

        private async Task LoginAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Iniciar Sesión[/]")
                    .RuleStyle("blue")
                    .Centered());

            try
            {
                var email = AnsiConsole.Ask<string>("[green]Email:[/]");
                var password = AnsiConsole.Prompt(
                    new TextPrompt<string>("[green]Contraseña:[/]")
                        .Secret());

                var user = await _authService.LoginAsync(email, password);
                
                if (user != null)
                {
                    _currentUser = user;
                    AnsiConsole.MarkupLine($"[green]✅ ¡Bienvenido, {user.FullName}![/]");
                    AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
                    Console.ReadKey();
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]❌ Credenciales incorrectas[/]");
                    AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
                AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
                Console.ReadKey();
            }
        }

        private async Task RegisterAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Registrarse[/]")
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

                // Validar edad mínima
                var age = DateTime.Today.Year - birthdate.Year;
                if (birthdate.Date > DateTime.Today.AddYears(-age)) age--;
                
                if (age < 18)
                {
                    AnsiConsole.MarkupLine("[red]❌ Debes ser mayor de 18 años para registrarte[/]");
                    AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
                    Console.ReadKey();
                    return;
                }

                var genderId = await SelectGenderAsync();
                var orientationId = await SelectOrientationAsync();
                
                var careerId = await SelectCareerAsync();
                var profilePhrase = AnsiConsole.Ask<string>("[green]Frase de perfil (opcional):[/]");

                var user = new UserEntity
                {
                    FullName = name.Trim().ToUpper(),
                    Email = email.Trim().ToLower(),
                    PasswordHash = AuthService.HashPassword(password),
                    Birthdate = birthdate,
                    GenderId = genderId,
                    OrientationId = orientationId,
                    CareerId = careerId,
                    ProfilePhrase = profilePhrase.Trim()
                };

                await _userService.AddUserAsync(user);
                AnsiConsole.MarkupLine("[green]✅ Usuario registrado exitosamente![/]");
                AnsiConsole.MarkupLine("[blue]Ahora puedes iniciar sesión.[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            }

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task ShowAuthenticatedMenuAsync()
        {
            if (_currentUser == null) return;

            // Mostrar información del usuario actual
            var userInfoTable = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Usuario Actual[/]")
                .AddColumn("[green]Valor[/]");

            userInfoTable.AddRow("👤 Nombre", _currentUser.FullName);
            userInfoTable.AddRow("📧 Email", _currentUser.Email);
            userInfoTable.AddRow("🎂 Fecha de nacimiento", _currentUser.Birthdate.ToString("dd/MM/yyyy"));

            AnsiConsole.Write(userInfoTable);
            AnsiConsole.WriteLine();

            // Verificar si es administrador
            var isAdmin = await _authService.IsAdminAsync(_currentUser.UserId);
            if (isAdmin)
            {
                AnsiConsole.MarkupLine("[red]👑 Eres Administrador[/]");
                AnsiConsole.WriteLine();
            }

            // Menú principal
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]¿Qué deseas hacer?[/]")
                    .AddChoices(new[] {
                        "👀 Revisar Perfiles",
                        "❤️  Mis Likes",
                        "💕 Mis Matches",
                        "📊 Estadísticas",
                        "🏆 Estadísticas del Sistema",
                        isAdmin ? "👑 Panel de Administrador" : "",
                        "👤 Gestionar Mi Perfil",
                        "🔓 Cerrar Sesión"
                    }.Where(x => !string.IsNullOrEmpty(x)).ToArray()));

            switch (choice)
            {
                case "👀 Revisar Perfiles":
                    await ShowProfileReviewerAsync();
                    break;
                case "❤️  Mis Likes":
                    await ShowMyLikesAsync();
                    break;
                case "💕 Mis Matches":
                    await ShowMyMatchesAsync();
                    break;
                case "📊 Estadísticas":
                    await ShowStatsAsync();
                    break;
                case "🏆 Estadísticas del Sistema":
                    await ShowSystemAdvancedStatsAsync();
                    break;
                case "👑 Panel de Administrador":
                    if (isAdmin)
                        await ShowAdminPanelAsync();
                    break;
                case "👤 Gestionar Mi Perfil":
                    await ShowUserManagementAsync();
                    break;
                case "🔓 Cerrar Sesión":
                    _currentUser = null;
                    AnsiConsole.MarkupLine("[yellow]Sesión cerrada exitosamente[/]");
                    AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
                    Console.ReadKey();
                    break;
            }
        }

        private async Task ShowProfileReviewerAsync()
        {
            if (_currentUser == null) return;

            var profileReviewer = new ProfileReviewer(_interactionService, _userService, _currentUser.UserId);
            await profileReviewer.StartProfileReviewAsync();
        }

        private async Task ShowMyLikesAsync()
        {
            if (_currentUser == null) return;

            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Mis Likes[/]")
                    .RuleStyle("blue")
                    .Centered());

            var myLikes = await _interactionService.GetUsersLikedByAsync(_currentUser.UserId);
            var whoLikedMe = await _interactionService.GetUsersWhoLikedAsync(_currentUser.UserId);

            if (!myLikes.Any() && !whoLikedMe.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Aún no tienes actividad de likes.[/]");
            }
            else
            {
                if (myLikes.Any())
                {
                    AnsiConsole.MarkupLine($"[green]❤️  Personas que te gustaron ({myLikes.Count}):[/]");
                    var likesTable = CreateUsersTable(myLikes);
                    AnsiConsole.Write(likesTable);
                    AnsiConsole.WriteLine();
                }

                if (whoLikedMe.Any())
                {
                    AnsiConsole.MarkupLine($"[blue]💕 Personas a las que les gustaste ({whoLikedMe.Count}):[/]");
                    var likedMeTable = CreateUsersTable(whoLikedMe);
                    AnsiConsole.Write(likedMeTable);
                }
            }

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task ShowMyMatchesAsync()
        {
            if (_currentUser == null) return;

            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Mis Matches[/]")
                    .RuleStyle("blue")
                    .Centered());

            var matches = await _interactionService.GetMatchesAsync(_currentUser.UserId);

            if (!matches.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Aún no tienes matches.[/]");
                AnsiConsole.MarkupLine("[blue]¡Sigue revisando perfiles para encontrar tu match![/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]💕 Tienes {matches.Count} match(es):[/]");
                
                var matchesTable = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn("[blue]Match #[/]")
                    .AddColumn("[green]Persona[/]")
                                            .AddColumn("[blue]Fecha del Match[/]");

                for (int i = 0; i < matches.Count; i++)
                {
                    var match = matches[i];
                    var otherUserId = match.User1Id == _currentUser.UserId ? match.User2Id : match.User1Id;
                    var otherUser = await _userService.GetUserByIdAsync(otherUserId);
                    
                    if (otherUser != null)
                    {
                        matchesTable.AddRow(
                            (i + 1).ToString(),
                            otherUser.FullName,
                            match.CreatedAt.ToString("dd/MM/yyyy HH:mm")
                        );
                    }
                }

                AnsiConsole.Write(matchesTable);
            }

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task ShowStatsAsync()
        {
            if (_currentUser == null) return;

            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Estadísticas[/]")
                    .RuleStyle("blue")
                    .Centered());

            var likesSent = await _interactionService.GetLikesSentCountAsync(_currentUser.UserId);
            var likesReceived = await _interactionService.GetLikesReceivedCountAsync(_currentUser.UserId);
            var matchesCount = await _interactionService.GetMatchesCountAsync(_currentUser.UserId);

            var statsTable = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Estadística[/]")
                .AddColumn("[green]Valor[/]");

            statsTable.AddRow("❤️  Likes enviados", likesSent.ToString());
            statsTable.AddRow("💕 Likes recibidos", likesReceived.ToString());
            statsTable.AddRow("🎉 Matches", matchesCount.ToString());

            AnsiConsole.Write(statsTable);

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task ShowSystemAdvancedStatsAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Estadisticas del Sistema[/]")
                    .RuleStyle("blue")
                    .Centered());

            try
            {
                // Estadísticas básicas
                var totalUsers = (await _userService.GetAllUsersAsync()).Count;
                var averageLikes = await _interactionService.GetAverageLikesPerUserAsync();
                var usersWithoutLikes = await _interactionService.GetUsersWithoutLikesCountAsync();

                var basicStatsTable = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn("Estadistica")
                    .AddColumn("Valor");

                basicStatsTable.AddRow("Total de Usuarios", totalUsers.ToString());
                basicStatsTable.AddRow("Promedio de Likes por Usuario", averageLikes.ToString("F2"));
                basicStatsTable.AddRow("Usuarios sin Likes", usersWithoutLikes.ToString());

                AnsiConsole.Write(basicStatsTable);
                AnsiConsole.WriteLine();

                // Top usuarios por likes
                var topUsersByLikes = await _interactionService.GetTopUsersByLikesAsync(5);
                if (topUsersByLikes.Any())
                {
                    AnsiConsole.WriteLine("Top 5 Usuarios con Mas Likes:");
                    var topLikesTable = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("Posicion")
                        .AddColumn("Usuario")
                        .AddColumn("Email");

                    for (int i = 0; i < topUsersByLikes.Count; i++)
                    {
                        var user = topUsersByLikes[i];
                        topLikesTable.AddRow(
                            (i + 1).ToString(),
                            user.FullName,
                            user.Email
                        );
                    }

                    AnsiConsole.Write(topLikesTable);
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"Error al obtener estadisticas: {ex.Message}");
            }

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Presiona cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task ShowAdminPanelAsync()
        {
            if (_currentUser == null || !await _authService.IsAdminAsync(_currentUser.UserId)) return;

            var exit = false;
            
            while (!exit)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(
                    new Rule("[red]👑 Panel de Administrador[/]")
                        .RuleStyle("red")
                        .Centered());

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[red]Selecciona una acción administrativa:[/]")
                        .AddChoices(new[] {
                            "📋 Listar Usuarios",
                            "✏️  Editar Usuario",
                            "🗑️  Eliminar Usuario",
                            "📊 Estadísticas del Sistema",
                            "🔙 Volver al Menú Principal"
                        }));

                switch (choice)
                {
                    case "📋 Listar Usuarios":
                        await ListAllUsersAsync();
                        break;
                    case "✏️  Editar Usuario":
                        await EditUserAsync();
                        break;
                    case "🗑️  Eliminar Usuario":
                        await DeleteUserAsync();
                        break;
                    case "📊 Estadísticas del Sistema":
                        await ShowSystemStatsAsync();
                        break;
                    case "🔙 Volver al Menú Principal":
                        exit = true;
                        break;
                }
            }
        }

        private async Task ListAllUsersAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[red]📋 Lista de Usuarios[/]")
                    .RuleStyle("red")
                    .Centered());

            var users = await _userService.GetAllUsersAsync();
            
            if (!users.Any())
            {
                AnsiConsole.MarkupLine("[yellow]No hay usuarios registrados.[/]");
            }
            else
            {
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn("[red]ID[/]")
                    .AddColumn("[green]Nombre[/]")
                    .AddColumn("[blue]Email[/]")
                    .AddColumn("[blue]Fecha Nac.[/]")
                    .AddColumn("[yellow]Género[/]")
                    .AddColumn("[blue]Orientación[/]")
                    .AddColumn("[blue]Carrera[/]")
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
                        await GetCareerNameAsync(user.CareerId),
                        user.CreatedAt.ToString("yyyy-MM-dd")
                    );
                }

                AnsiConsole.Write(table);
                AnsiConsole.MarkupLine($"[green]Total de usuarios: {users.Count}[/]");
            }

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task EditUserAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[red]✏️  Editar Usuario[/]")
                    .RuleStyle("red")
                    .Centered());

            try
            {
                var id = AnsiConsole.Ask<int>("[red]ID del usuario a editar:[/]");
                var user = await _userService.GetUserByIdAsync(id);
                
                if (user == null)
                {
                    AnsiConsole.MarkupLine("[red]❌ Usuario no encontrado.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[blue]Editando usuario: {user.FullName}[/]");
                    
                    var newName = AnsiConsole.Ask<string>($"[green]Nuevo nombre (actual: {user.FullName}):[/]", user.FullName);
                    var newEmail = AnsiConsole.Ask<string>($"[green]Nuevo email (actual: {user.Email}):[/]", user.Email);
                    
                    user.FullName = newName;
                    user.Email = newEmail;

                    if (AnsiConsole.Confirm("[yellow]¿Confirmar los cambios?[/]"))
                    {
                        await _userService.UpdateUserAsync(user);
                        AnsiConsole.MarkupLine("[green]✅ Usuario actualizado exitosamente![/]");
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            }

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task DeleteUserAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[red]🗑️  Eliminar Usuario[/]")
                    .RuleStyle("red")
                    .Centered());

            try
            {
                var id = AnsiConsole.Ask<int>("[red]ID del usuario a eliminar:[/]");
                
                if (id == _currentUser?.UserId)
                {
                    AnsiConsole.MarkupLine("[red]❌ No puedes eliminar tu propio usuario.[/]");
                }
                else
                {
                    var user = await _userService.GetUserByIdAsync(id);
                    
                    if (user == null)
                    {
                        AnsiConsole.MarkupLine("[red]❌ Usuario no encontrado.[/]");
                    }
                    else
                    {
                        if (AnsiConsole.Confirm($"[red]⚠️  ¿Estás seguro de que deseas eliminar al usuario '{user.FullName}'?[/]"))
                        {
                            if (AnsiConsole.Confirm("[red]Esta acción no se puede deshacer. ¿Confirmar eliminación?[/]"))
                            {
                                await _userService.DeleteUserAsync(id);
                                AnsiConsole.MarkupLine("[green]✅ Usuario eliminado exitosamente![/]");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            }

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task ShowSystemStatsAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[red]📊 Estadísticas del Sistema[/]")
                    .RuleStyle("red")
                    .Centered());

            var totalUsers = (await _userService.GetAllUsersAsync()).Count;
            var totalLikes = await _interactionService.GetLikesSentCountAsync(1) + await _interactionService.GetLikesReceivedCountAsync(1); // Aproximado
            var totalMatches = await _interactionService.GetMatchesCountAsync(1); // Aproximado

            var statsTable = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[red]Estadística[/]")
                .AddColumn("[green]Valor[/]");

            statsTable.AddRow("👥 Total de Usuarios", totalUsers.ToString());
            statsTable.AddRow("❤️  Total de Likes", totalLikes.ToString());
            statsTable.AddRow("💕 Total de Matches", totalMatches.ToString());

            AnsiConsole.Write(statsTable);

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task ShowUserManagementAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Gestionar Mi Perfil[/]")
                    .RuleStyle("blue")
                    .Centered());

            try
            {
                var currentUser = await _userService.GetUserByIdAsync(_currentUser!.UserId);
                if (currentUser == null)
                {
                    AnsiConsole.MarkupLine("[red]❌ Error: Usuario no encontrado[/]");
                    return;
                }

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]¿Qué deseas hacer?[/]")
                        .AddChoices(new[] {
                            "👁️  Ver mi perfil",
                            "✏️  Editar mi perfil",
                            "🔒 Cambiar contraseña",
                            "🔙 Volver al menú principal"
                        }));

                switch (choice)
                {
                    case "👁️  Ver mi perfil":
                        await ShowMyProfileAsync(currentUser);
                        break;
                    case "✏️  Editar mi perfil":
                        await EditMyProfileAsync(currentUser);
                        break;
                    case "🔒 Cambiar contraseña":
                        await ChangeMyPasswordAsync(currentUser);
                        break;
                    case "🔙 Volver al menú principal":
                        return;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            }

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task ShowMyProfileAsync(UserEntity currentUser)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Mi Perfil[/]")
                    .RuleStyle("blue")
                    .Centered());

            var profileTable = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Campo[/]")
                .AddColumn("[green]Valor[/]");

            profileTable.AddRow("👤 Nombre", currentUser.FullName);
            profileTable.AddRow("📧 Email", currentUser.Email);
            profileTable.AddRow("🎂 Fecha de nacimiento", currentUser.Birthdate.ToString("dd/MM/yyyy"));
            profileTable.AddRow("👥 Género", await GetGenderNameAsync(currentUser.GenderId));
            profileTable.AddRow("👥 Orientación", await GetOrientationNameAsync(currentUser.OrientationId));
            profileTable.AddRow("🎓 Carrera/Profesión", await GetCareerNameAsync(currentUser.CareerId));
            profileTable.AddRow("💬 Frase de perfil", currentUser.ProfilePhrase);

            AnsiConsole.Write(profileTable);
        }

        private async Task EditMyProfileAsync(UserEntity currentUser)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]✏️  Editar Mi Perfil[/]")
                    .RuleStyle("blue")
                    .Centered());

            try
            {
                var newName = AnsiConsole.Ask<string>($"[green]Nuevo nombre (actual: {currentUser.FullName}):[/]", currentUser.FullName);
                var newEmail = AnsiConsole.Ask<string>($"[green]Nuevo email (actual: {currentUser.Email}):[/]", currentUser.Email);
                
                currentUser.FullName = newName;
                currentUser.Email = newEmail;

                if (AnsiConsole.Confirm("[yellow]¿Confirmar los cambios?[/]"))
                {
                    await _userService.UpdateUserAsync(currentUser);
                    AnsiConsole.MarkupLine("[green]✅ Perfil actualizado exitosamente![/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            }

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task ChangeMyPasswordAsync(UserEntity currentUser)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Cambiar Contraseña[/]")
                    .RuleStyle("blue")
                    .Centered());

            try
            {
                var currentPassword = AnsiConsole.Prompt(
                    new TextPrompt<string>("[green]Contraseña actual:[/]")
                        .Secret());

                var newPassword = AnsiConsole.Prompt(
                    new TextPrompt<string>("[green]Nueva contraseña:[/]")
                        .Secret()
                        .ValidationErrorMessage("[red]La contraseña debe tener al menos 6 caracteres[/]")
                        .Validate(password => password.Length >= 6 ? ValidationResult.Success() : ValidationResult.Error()));

                var confirmPassword = AnsiConsole.Prompt(
                    new TextPrompt<string>("[green]Confirmar nueva contraseña:[/]")
                        .Secret());

                if (newPassword != confirmPassword)
                {
                    AnsiConsole.MarkupLine("[red]❌ Las contraseñas no coinciden[/]");
                    return;
                }

                var success = await _userService.ChangePasswordAsync(currentUser.UserId, currentPassword, newPassword);
                
                if (success)
                {
                    AnsiConsole.MarkupLine("[green]✅ Contraseña cambiada exitosamente[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]❌ La contraseña actual es incorrecta[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            }

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        // Métodos auxiliares
        private async Task<int> SelectGenderAsync()
        {
            var genderOptions = new Dictionary<string, int>
            {
                { "Masculino", 1 },
                { "Femenino", 2 },
                { "No binario", 3 }
            };

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Selecciona el género:[/]")
                    .AddChoices(genderOptions.Keys));

            return genderOptions[choice];
        }

        private async Task<int> SelectOrientationAsync()
        {
            var orientationOptions = new Dictionary<string, int>
            {
                { "Heterosexual", 1 },
                { "Homosexual", 2 },
                { "Bisexual", 3 },
                { "Pansexual", 4 }
            };

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Selecciona la orientación:[/]")
                    .AddChoices(orientationOptions.Keys));

            return orientationOptions[choice];
        }

        private async Task<int> SelectCareerAsync()
        {
            var careers = new List<(int id, string name, string category)>
            {
                (1, "Ingeniería de Sistemas", "Ingeniería"),
                (2, "Ingeniería Informática", "Ingeniería"),
                (3, "Ingeniería Civil", "Ingeniería"),
                (4, "Medicina", "Ciencias de la Salud"),
                (5, "Enfermería", "Ciencias de la Salud"),
                (6, "Odontología", "Ciencias de la Salud"),
                (7, "Derecho", "Ciencias Sociales"),
                (8, "Psicología", "Ciencias Sociales"),
                (9, "Administración de Empresas", "Ciencias Sociales"),
                (10, "Contabilidad", "Ciencias Sociales"),
                (11, "Matemáticas", "Ciencias"),
                (12, "Física", "Ciencias"),
                (13, "Química", "Ciencias"),
                (14, "Biología", "Ciencias"),
                (15, "Arquitectura", "Arte y Diseño"),
                (16, "Diseño Gráfico", "Arte y Diseño"),
                (17, "Música", "Arte y Diseño")
            };

            var groupedCareers = careers.GroupBy(c => c.category).ToList();

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[green]Selecciona tu carrera/profesión:[/]");
            AnsiConsole.WriteLine();

            var careerChoices = new List<string>();
            var careerIds = new List<int>();

            foreach (var group in groupedCareers)
            {
                AnsiConsole.MarkupLine($"[blue]{group.Key}:[/]");
                foreach (var career in group)
                {
                    var choice = $"{career.name}";
                    careerChoices.Add(choice);
                    careerIds.Add(career.id);
                    AnsiConsole.MarkupLine($"  {career.id}. {career.name}");
                }
                AnsiConsole.WriteLine();
            }

            var selectedIndex = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Selecciona tu carrera:[/]")
                    .AddChoices(careerChoices));

            var selectedCareerIndex = careerChoices.IndexOf(selectedIndex);
            return careerIds[selectedCareerIndex];
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

        private async Task<string> GetCareerNameAsync(int careerId)
        {
            var careerNames = new Dictionary<int, string>
            {
                { 1, "Ingeniería de Sistemas" },
                { 2, "Ingeniería Informática" },
                { 3, "Ingeniería Civil" },
                { 4, "Medicina" },
                { 5, "Enfermería" },
                { 6, "Odontología" },
                { 7, "Derecho" },
                { 8, "Psicología" },
                { 9, "Administración de Empresas" },
                { 10, "Contabilidad" },
                { 11, "Matemáticas" },
                { 12, "Física" },
                { 13, "Química" },
                { 14, "Biología" },
                { 15, "Arquitectura" },
                { 16, "Diseño Gráfico" },
                { 17, "Música" }
            };

            return careerNames.TryGetValue(careerId, out var name) ? name : "Desconocida";
        }

        private Table CreateUsersTable(List<UserEntity> users)
        {
            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Nombre[/]")
                .AddColumn("[green]Email[/]")
                                    .AddColumn("[blue]Edad[/]")
                .AddColumn("[blue]Género[/]");

            foreach (var user in users)
            {
                table.AddRow(
                    user.FullName,
                    user.Email,
                    CalculateAge(user.Birthdate).ToString() + " años",
                    GetGenderNameAsync(user.GenderId).Result
                );
            }

            return table;
        }

        private int CalculateAge(DateTime birthdate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthdate.Year;
            if (birthdate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}

