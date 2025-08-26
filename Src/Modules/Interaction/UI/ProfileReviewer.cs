using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.Interaction.Application.Services;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.User.Application.Services;
using Spectre.Console;

namespace campuslovejorge_edwin.Src.Modules.Interaction.UI
{
    public class ProfileReviewer
    {
        private readonly InteractionService _interactionService;
        private readonly UserService _userService;
        private readonly int _currentUserId;

        public ProfileReviewer(InteractionService interactionService, UserService userService, int currentUserId)
        {
            _interactionService = interactionService;
            _userService = userService;
            _currentUserId = currentUserId;
        }

        public async Task StartProfileReviewAsync()
        {
            var exit = false;
            
            while (!exit)
            {
                AnsiConsole.Clear();
                
                // Título principal
                AnsiConsole.Write(
                    new FigletText("REVISAR PERFILES")
                        .Centered()
                        .Color(Color.Blue));
                
                AnsiConsole.Write(
                    new Rule("[yellow]Descubre nuevas personas[/]")
                        .RuleStyle("blue")
                        .Centered());

                // Mostrar estadísticas del usuario
                await ShowUserStatsAsync();

                // Obtener perfiles para revisar
                var profilesToReview = await _interactionService.GetProfilesToReviewAsync(_currentUserId, 10);
                var totalProfiles = await _interactionService.GetProfilesToReviewCountAsync(_currentUserId);

                if (!profilesToReview.Any())
                {
                    AnsiConsole.MarkupLine("[yellow]¡No hay más perfiles para revisar![/]");
                    AnsiConsole.MarkupLine("[blue]Vuelve más tarde para ver nuevos perfiles.[/]");
                    
                    var updateChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[green]¿Qué deseas hacer?[/]")
                            .AddChoices("🔄 Actualizar", "❌ Salir"));

                    if (updateChoice == "❌ Salir")
                        exit = true;
                    
                    continue;
                }

                AnsiConsole.MarkupLine($"[green]Perfiles disponibles para revisar: {totalProfiles}[/]");
                AnsiConsole.WriteLine();

                // Menú principal
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Selecciona una opción:[/]")
                        .AddChoices(new[] {
                            "👀 Revisar Perfiles",
                            "❤️  Mis Likes",
                            "💕 Mis Matches",
                            "📊 Estadísticas",
                            "❌ Salir"
                        }));

                switch (choice)
                {
                    case "👀 Revisar Perfiles":
                        await ReviewProfilesAsync(profilesToReview);
                        break;
                    case "❤️  Mis Likes":
                        await ShowMyLikesAsync();
                        break;
                    case "💕 Mis Matches":
                        await ShowMyMatchesAsync();
                        break;
                    case "📊 Estadísticas":
                        await ShowDetailedStatsAsync();
                        break;
                    case "❌ Salir":
                        exit = true;
                        break;
                }
            }
        }

        private async Task ReviewProfilesAsync(List<UserEntity> profiles)
        {
            var currentIndex = 0;
            
            while (currentIndex < profiles.Count)
            {
                var profile = profiles[currentIndex];
                
                AnsiConsole.Clear();
                AnsiConsole.Write(
                    new Rule($"[blue]Perfil {currentIndex + 1} de {profiles.Count}[/]")
                        .RuleStyle("blue")
                        .Centered());

                // Mostrar información del perfil
                await DisplayProfileAsync(profile);

                // Opciones de acción
                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]¿Qué te parece este perfil?[/]")
                        .AddChoices(new[] {
                            "❤️  Me gusta",
                            "👎 No me gusta",
                            "⏭️  Siguiente",
                            "⏮️  Anterior",
                            "🔙 Volver al menú"
                        }));

                switch (action)
                {
                    case "❤️  Me gusta":
                        await LikeProfileAsync(profile);
                        profiles.RemoveAt(currentIndex);
                        break;
                        
                    case "👎 No me gusta":
                        await DislikeProfileAsync(profile);
                        profiles.RemoveAt(currentIndex);
                        break;
                        
                    case "⏭️  Siguiente":
                        currentIndex = Math.Min(currentIndex + 1, profiles.Count - 1);
                        break;
                        
                    case "⏮️  Anterior":
                        currentIndex = Math.Max(currentIndex - 1, 0);
                        break;
                        
                    case "🔙 Volver al menú":
                        return;
                }

                // Si no quedan perfiles, salir
                if (!profiles.Any())
                {
                    AnsiConsole.MarkupLine("[yellow]¡Has revisado todos los perfiles disponibles![/]");
                    AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
                    Console.ReadKey();
                    break;
                }
            }
        }

        private async Task DisplayProfileAsync(UserEntity profile)
        {
            // Crear una tabla atractiva para mostrar el perfil
            var profileTable = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Campo[/]")
                .AddColumn("[green]Información[/]");

            profileTable.AddRow("👤 Nombre", profile.FullName);
            profileTable.AddRow("📧 Email", profile.Email);
            profileTable.AddRow("🎂 Fecha de nacimiento", profile.Birthdate.ToString("dd/MM/yyyy"));
            profileTable.AddRow("📅 Edad", profile.Age.ToString() + " años");
            profileTable.AddRow("👥 Género", await GetGenderNameAsync(profile.GenderId));
            profileTable.AddRow("💕 Orientación", await GetOrientationNameAsync(profile.OrientationId));
            profileTable.AddRow("🎓 Carrera", await GetCareerNameAsync(profile.CareerId));
            profileTable.AddRow("💬 Frase de perfil", !string.IsNullOrEmpty(profile.ProfilePhrase) ? profile.ProfilePhrase : "Sin frase de perfil");
            profileTable.AddRow("📅 Miembro desde", profile.CreatedAt.ToString("dd/MM/yyyy"));

            AnsiConsole.Write(profileTable);
            AnsiConsole.WriteLine();

            // Mostrar intereses si están disponibles
            // TODO: Implementar cuando se agregue la funcionalidad de intereses
        }

        private async Task LikeProfileAsync(UserEntity profile)
        {
            try
            {
                // Verificar si el usuario puede dar like (límite diario)
                if (!await _interactionService.CanUserLikeAsync(_currentUserId))
                {
                    AnsiConsole.MarkupLine("[red]❌ Has alcanzado el límite de likes diarios[/]");
                    AnsiConsole.MarkupLine("[yellow]Vuelve mañana para dar más likes[/]");
                    AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
                    Console.ReadKey();
                    return;
                }

                var likeResult = await _interactionService.LikeUserAsync(_currentUserId, profile.UserId);
                
                if (likeResult)
                {
                    // Verificar si hay match
                    var hasMatch = await _interactionService.HasLikedAsync(profile.UserId, _currentUserId);
                    
                    if (hasMatch)
                    {
                        AnsiConsole.MarkupLine("[green]🎉 ¡MATCH! ¡A esta persona también le gustaste![/]");
                        AnsiConsole.MarkupLine($"[blue]Has hecho match con {profile.FullName}[/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[green]❤️  Te gustó {profile.FullName}[/]");
                        AnsiConsole.MarkupLine("[blue]¡Espera a ver si también le gustas![/]");
                    }

                    // Mostrar likes restantes
                    var remainingLikes = await _interactionService.GetRemainingLikesAsync(_currentUserId);
                    AnsiConsole.MarkupLine($"[yellow]🎯 Likes restantes hoy: {remainingLikes}[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]❌ No se pudo procesar el like[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            }

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task DislikeProfileAsync(UserEntity profile)
        {
            try
            {
                var dislikeResult = await _interactionService.DislikeUserAsync(_currentUserId, profile.UserId);
                
                if (dislikeResult)
                {
                    AnsiConsole.MarkupLine($"[yellow]👎 No te gustó {profile.FullName}[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]👎 Perfil marcado como no interesante[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            }

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }

        private async Task ShowMyLikesAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Mis Likes[/]")
                    .RuleStyle("blue")
                    .Centered());

            var myLikes = await _interactionService.GetUsersLikedByAsync(_currentUserId);
            var whoLikedMe = await _interactionService.GetUsersWhoLikedAsync(_currentUserId);

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
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Mis Matches[/]")
                    .RuleStyle("blue")
                    .Centered());

            var matches = await _interactionService.GetMatchesAsync(_currentUserId);

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
                    var otherUserId = match.User1Id == _currentUserId ? match.User2Id : match.User1Id;
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

        private async Task ShowUserStatsAsync()
        {
            var likesSent = await _interactionService.GetLikesSentCountAsync(_currentUserId);
            var likesReceived = await _interactionService.GetLikesReceivedCountAsync(_currentUserId);
            var matchesCount = await _interactionService.GetMatchesCountAsync(_currentUserId);
            var remainingLikes = await _interactionService.GetRemainingLikesAsync(_currentUserId);

            var statsTable = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Estadística[/]")
                .AddColumn("[green]Valor[/]");

            statsTable.AddRow("❤️  Likes enviados", likesSent.ToString());
            statsTable.AddRow("💕 Likes recibidos", likesReceived.ToString());
            statsTable.AddRow("🎉 Matches", matchesCount.ToString());
            statsTable.AddRow("🎯 Likes restantes hoy", remainingLikes.ToString());

            AnsiConsole.Write(statsTable);
            AnsiConsole.WriteLine();
        }

        private async Task ShowDetailedStatsAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule("[blue]Estadísticas Detalladas[/]")
                    .RuleStyle("blue")
                    .Centered());

            await ShowUserStatsAsync();

            // Mostrar estadísticas adicionales
            var profilesToReview = await _interactionService.GetProfilesToReviewCountAsync(_currentUserId);
            
            var additionalStatsTable = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Estadística[/]")
                .AddColumn("[green]Valor[/]");

            additionalStatsTable.AddRow("👀 Perfiles por revisar", profilesToReview.ToString());
            additionalStatsTable.AddRow("📊 Total de usuarios", (await _userService.GetAllUsersAsync()).Count.ToString());

            AnsiConsole.Write(additionalStatsTable);

            AnsiConsole.WriteLine("[blue]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
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
    }
}
