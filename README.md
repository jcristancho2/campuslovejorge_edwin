# 🏫 Campus Love - Sistema de Emparejamiento Universitario

[![SOLID](https://img.shields.io/badge/SOLID-black.svg?style=for-the-badge)](https://en.wikipedia.org/wiki/SOLID)
[![Arquitectura Hexagonal](https://img.shields.io/badge/Hexagonal_Architecture-blue.svg?style=for-the-badge)](https://en.wikipedia.org/wiki/Hexagonal_architecture_(software))
[![Vertical Slicing](https://img.shields.io/badge/Vertical_Slicing-purple.svg?style=for-the-badge)](https://en.wikipedia.org/wiki/Vertical_slice)
[![Git Flow](https://img.shields.io/badge/Git_Flow-F05032.svg?style=for-the-badge&logo=git&logoColor=white)](https://nvie.com/posts/a-successful-git-branching-model/)
[![Conventional Commits](https://img.shields.io/badge/Conventional%20Commits-FE5196.svg?style=for-the-badge&logo=conventional-commits&logoColor=white)](https://www.conventionalcommits.org/)
[![.NET 9](https://img.shields.io/badge/.NET_9-512BD4.svg?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-0078D7.svg?style=for-the-badge&logo=nuget&logoColor=white)](https://docs.microsoft.com/en-us/ef/core/)
[![MySQL](https://img.shields.io/badge/MySQL-4479A1.svg?style=for-the-badge&logo=mysql&logoColor=white)](https://www.mysql.com/)
[![Spectre.Console](https://img.shields.io/badge/Spectre.Console-212121.svg?style=for-the-badge&logo=nuget&logoColor=white)](https://spectreconsole.net/)
[![BCrypt.Net](https://img.shields.io/badge/BCrypt.Net-orange.svg?style=for-the-badge&logo=nuget&logoColor=white)](https://github.com/BcryptNet/bcrypt.net)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)

## 🎯 Descripción del Proyecto

**Campus Love** es una aplicación de consola en C# que simula un sistema de emparejamiento universitario completo. El proyecto implementa un flujo integral donde los usuarios pueden registrarse, ver perfiles de otras personas, hacer "like" o "dislike", y revisar sus coincidencias (matches).

El sistema está diseñado para demostrar la implementación de:
- **Arquitectura Limpia** con separación clara de responsabilidades
- **Principios SOLID** en toda la estructura del código
- **Patrones de Diseño** como Factory, Repository, y Strategy
- **Arquitectura Hexagonal** con Vertical Slicing
- **Validaciones robustas** y manejo de errores
- **Sistema de créditos** para limitar interacciones diarias

## 🚀 Características

### ✨ Funcionalidades Principales
- 🔐 **Sistema de Autenticación** con encriptación BCrypt
- 👤 **Registro de Usuarios** con validaciones completas
- 👀 **Visualización de Perfiles** uno por uno
- ❤️ **Sistema de Likes/Dislikes** con límites diarios
- 💕 **Matching Automático** cuando hay likes mutuos
- 📊 **Estadísticas del Sistema** usando LINQ
- 👑 **Panel de Administrador** para gestión de usuarios
- 🎨 **Interfaz de Consola** moderna con Spectre.Console

### 🔒 Seguridad y Validaciones
- **Encriptación de contraseñas** con BCrypt
- **Validación de edad mínima** (18+ años)
- **Verificación de emails únicos**
- **Control de acceso por roles**
- **Validación de datos de entrada**

## 🏗️ Arquitectura

### 📐 Patrón Arquitectónico
El proyecto implementa **Arquitectura Hexagonal** con **Vertical Slicing**, organizando el código en módulos independientes que encapsulan funcionalidades específicas del dominio.

```
📁 Src/
├── 📁 Modules/
│   ├── 📁 User/           # Gestión de usuarios
│   ├── 📁 Interaction/    # Sistema de likes y matches
│   └── 📁 MainMenu/       # Interfaz principal
├── 📁 Shared/             # Componentes compartidos
│   ├── 📁 Context/        # Entity Framework
│   ├── 📁 Configuration/  # Mapeos de entidades
│   └── 📁 Helpers/        # Utilidades comunes
└── 📁 Program.cs          # Punto de entrada
```

### 🎯 Principios SOLID Implementados

- **S** - **Single Responsibility**: Cada clase tiene una responsabilidad única
- **O** - **Open/Closed**: Extensible sin modificar código existente
- **L** - **Liskov Substitution**: Interfaces bien definidas
- **I** - **Interface Segregation**: Interfaces específicas y cohesivas
- **D** - **Dependency Inversion**: Dependencias a través de abstracciones

### 🔄 Patrones de Diseño

- **Factory Pattern**: Creación de entidades y servicios
- **Repository Pattern**: Acceso a datos abstraído
- **Strategy Pattern**: Diferentes algoritmos de matching
- **Observer Pattern**: Notificaciones de eventos
- **Command Pattern**: Operaciones de usuario

## 🛠️ Tecnologías Utilizadas

### 🎯 Plataforma Principal
- **.NET 9** - Framework de desarrollo
- **C# 12** - Lenguaje de programación
- **Entity Framework Core 9** - ORM para acceso a datos

### 🗄️ Base de Datos
- **MySQL 8.0+** - Sistema de gestión de base de datos
- **Pomelo.EntityFrameworkCore.MySql** - Proveedor MySQL para EF Core

### 🎨 Interfaz de Usuario
- **Spectre.Console** - Biblioteca para consolas modernas
- **FigletText** - Texto ASCII art
- **Tables y Rules** - Componentes visuales

### 🔐 Seguridad
- **BCrypt.Net-Next** - Encriptación de contraseñas
- **Validación de entrada** - Sanitización de datos

### 📊 Reportes
- **QuestPDF** - Generación de reportes PDF
- **LINQ** - Consultas y estadísticas

## 📊 Diagramas

### 🗃️ Diagrama de Base de Datos (ER)

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│    user     │    │   gender    │    │ orientation │
├─────────────┤    ├─────────────┤    ├─────────────┤
│ user_id (PK)│    │gender_id(PK)│    │orientation_id│
│ fullname    │    │ name        │    │ name        │
│ email       │    │ created_at  │    │ created_at  │
│ password_hash│   └─────────────┘    └─────────────┘
│ birthdate   │           │                   │
│ gender_id   │◄──────────┘                   │
│ orientation_id◄─────────────────────────────┘
│ career_id   │
│ profile_phrase│
│ created_at  │
│ updated_at  │
└─────────────┘
       │
       │
       ▼
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│   career    │    │   interest  │    │ user_interest│
├─────────────┤    ├─────────────┤    ├─────────────┤
│career_id(PK)│    │interest_id(PK)│  │ user_id    │
│ name        │    │ name        │    │ interest_id │
│ category    │    │ category    │    │ created_at  │
│ created_at  │    │ created_at  │    └─────────────┘
└─────────────┘    └─────────────┘
       │                   │
       │                   │
       ▼                   ▼
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│ user_like   │    │daily_likes  │    │match_table  │
├─────────────┤    ├─────────────┤    ├─────────────┤
│ like_id (PK)│    │daily_like_id│    │ match_id(PK)│
│ liker_id    │    │ user_id     │    │ user1_id    │
│ liked_id    │    │ likes_used  │    │ user2_id    │
│ created_at  │    │ max_likes   │    │ created_at  │
└─────────────┘    │ date        │    └─────────────┘
                   │ created_at  │
                   │ updated_at  │
                   └─────────────┘
```

### 🏗️ Diagrama de Clases

```
┌─────────────────────────────────────────────────────────────┐
│                    DOMAIN LAYER                            │
├─────────────────────────────────────────────────────────────┤
│  UserEntity  │  Career  │  Interest  │  UserLike  │  Match  │
│  -UserId     │  -CareerId│  -InterestId│  -LikeId  │  -MatchId│
│  -FullName   │  -Name   │  -Name     │  -LikerId  │  -User1Id│
│  -Email      │  -Category│  -Category │  -LikedId  │  -User2Id│
│  -PasswordHash│  -CreatedAt│  -CreatedAt│  -CreatedAt│  -CreatedAt│
│  -Birthdate  │           │           │           │           │
│  -GenderId   │           │           │           │           │
│  -OrientationId│         │           │           │           │
│  -CareerId   │           │           │           │           │
│  -ProfilePhrase│         │           │           │           │
│  -CreatedAt  │           │           │           │           │
│  -UpdatedAt  │           │           │           │           │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                  APPLICATION LAYER                         │
├─────────────────────────────────────────────────────────────┤
│  IUserService  │  IAuthService  │  IInteractionService     │
│  +AddUserAsync │  +LoginAsync   │  +LikeUserAsync          │
│  +GetUserAsync │  +IsAdminAsync │  +GetMatchesAsync        │
│  +UpdateUserAsync│  +HashPassword│  +GetStatsAsync          │
│  +DeleteUserAsync│  +VerifyPassword│  +GetDailyLimitAsync   │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                INFRASTRUCTURE LAYER                        │
├─────────────────────────────────────────────────────────────┤
│  UserRepository  │  CareerRepository  │  InteractionRepository│
│  +AddAsync      │  +GetAllAsync      │  +AddLikeAsync        │
│  +GetByIdAsync  │  +GetByCategoryAsync│  +GetMatchesAsync    │
│  +GetByEmailAsync│  +GetByNameAsync  │  +GetStatsAsync       │
│  +UpdateAsync   │  +GetByIdAsync     │  +GetDailyLimitAsync  │
│  +DeleteAsync   │                    │                      │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                     UI LAYER                               │
├─────────────────────────────────────────────────────────────┤
│  MainMenu  │  MenuUser  │  ProfileReviewer  │  AuthService  │
│  +StartAsync│  +ShowMenuAsync│  +ShowProfilesAsync│  +LoginAsync│
│  +LoginAsync│  +AddUserAsync│  +LikeProfileAsync│  +IsAdminAsync│
│  +RegisterAsync│  +ListUsersAsync│  +DislikeProfileAsync│  +HashPassword│
│  +ShowStatsAsync│  +EditUserAsync│  +ShowMatchesAsync│  +VerifyPassword│
└─────────────────────────────────────────────────────────────┘
```

## 🗄️ Base de Datos

### 📋 Tablas del Sistema

| Componente | Tablas Utilizadas | Descripción |
|------------|-------------------|-------------|
| **User Module** | `user`, `gender`, `orientation`, `career` | Gestión de usuarios y perfiles |
| **Interaction Module** | `user_like`, `daily_likes`, `match_table` | Sistema de likes y matches |
| **Interest Module** | `interest`, `user_interest` | Gestión de intereses de usuario |
| **Statistics Module** | Todas las tablas | Reportes y estadísticas del sistema |

### 🔗 Relaciones Principales

- **User** → **Gender** (1:N) - Un género puede tener muchos usuarios
- **User** → **Orientation** (1:N) - Una orientación puede tener muchos usuarios  
- **User** → **Career** (1:N) - Una carrera puede tener muchos usuarios
- **User** ↔ **User** (N:N) - Usuarios pueden dar likes a otros usuarios
- **User** ↔ **Interest** (N:N) - Usuarios pueden tener múltiples intereses

### 📊 Índices de Optimización

- **Claves primarias** en todas las entidades
- **Claves únicas** para emails y combinaciones de likes
- **Índices compuestos** para búsquedas frecuentes
- **Índices de fecha** para consultas temporales

## 🔧 Instalación y Configuración

### 📋 Prerrequisitos

- **.NET 9 SDK** o superior
- **MySQL 8.0** o superior
- **Visual Studio Code**
- **Git** para control de versiones

### 🚀 Pasos de Instalación

1. **Clonar el repositorio**
```bash
git clone https://github.com/Edwincamilocorzosanchez/campuslovejorge_edwin.git
cd campuslovejorge_edwin
```

2. **Restaurar dependencias**
```bash
dotnet restore
```

3. **Configurar la base de datos**
```bash
# Crear usuario MySQL
mysql -u root -p -e "CREATE USER 'campus2023'@'localhost' IDENTIFIED BY 'campus2023';"
mysql -u root -p -e "GRANT ALL PRIVILEGES ON *.* TO 'campus2023'@'localhost';"
mysql -u root -p -e "FLUSH PRIVILEGES;"

# Ejecutar script de base de datos
mysql -u campus2023 -p'campus2023' < database_schema_complete.sql
```

4. **Configurar appsettings.json**
```json
{
    "ConnectionStrings": {
        "MySqlDb": "server=localhost;database=examendb;user=campus2023;password=campus2023;"
    }
}
```

5. **Compilar y ejecutar**
```bash
dotnet build
dotnet run
```

### 🔐 Credenciales de Prueba

| Email | Contraseña | Rol |
|-------|------------|-----|
| `admin@campuslove.com` | `admin123` | Administrador |
| `edwin@gmail.com` | `000000` | Usuario |

## 📱 Uso de la Aplicación

### 🎯 Flujo Principal

1. **Inicio de Sesión/Registro**
   - Opción de login para usuarios existentes
   - Registro de nuevos usuarios con validaciones

2. **Menú Principal**
   - Revisar perfiles disponibles
   - Ver matches y estadísticas
   - Gestión de perfil personal

3. **Sistema de Likes**
   - Visualización de perfiles uno por uno
   - Like/Dislike con límites diarios
   - Notificaciones de matches

4. **Panel de Administrador**
   - Gestión completa de usuarios
   - Estadísticas del sistema
   - Reportes y análisis

### ⌨️ Comandos Principales

- **Navegación**: Usar flechas del teclado
- **Selección**: Enter para confirmar
- **Salida**: Ctrl+C o opción "Salir"
- **Ayuda**: F1 (en desarrollo)

## 👥 Participantes del Proyecto

### 👨‍💻 **Edwin Camilo Corzo Sánchez**
- **GitHub**: [@Edwincamilocorzosanchez](https://github.com/Edwincamilocorzosanchez)
- **Rol**: Desarrollador Backend & Arquitectura
- **Contribuciones**: 
  - Arquitectura del sistema
  - Implementación de Entity Framework
  - Sistema de autenticación y seguridad
  - Configuración de base de datos

### 👨‍💻 **Jorge Andrés Cristancho Olarte**
- **GitHub**: [@jcristancho2](https://github.com/jcristancho2)
- **Rol**: Desarrollador Full Stack & UI/UX
- **Contribuciones**:
  - Interfaz de usuario con Spectre.Console
  - Sistema de matching y estadísticas
  - Implementación de principios SOLID
  - Patrones de diseño y arquitectura

## 📈 Funcionalidades Implementadas

### ✅ **Requerimientos Funcionales Completados**

- [x] **Registro de usuarios** con validaciones completas
- [x] **Sistema de autenticación** con encriptación BCrypt
- [x] **Visualización de perfiles** uno por uno
- [x] **Sistema de likes/dislikes** con límites diarios
- [x] **Matching automático** cuando hay likes mutuos
- [x] **Estadísticas del sistema** usando LINQ
- [x] **Panel de administrador** para gestión completa
- [x] **Interfaz de consola moderna** con Spectre.Console

### ✅ **Requerimientos No Funcionales Completados**

- [x] **Arquitectura limpia** con separación de responsabilidades
- [x] **Principios SOLID** implementados completamente
- [x] **Patrones de diseño** (Factory, Repository, Strategy)
- [x] **Validaciones robustas** de entrada de datos
- [x] **Manejo de errores** y excepciones
- [x] **Código organizado** y bien estructurado
- [x] **Documentación completa** del sistema

### 🎯 **Características Adicionales**

- **Sistema de créditos diarios** para likes
- **Validación de edad mínima** (18+ años)
- **Control de acceso por roles** (Admin/Usuario)
- **Encriptación de contraseñas** con BCrypt
- **Interfaz de consola moderna** y atractiva
- **Sistema de estadísticas avanzadas** con LINQ
- **Manejo de transacciones** en base de datos


### 🔄 **Git Flow**

- **main**: Código de producción estable
- **develop**: Código en desarrollo
- **feature/**: Nuevas funcionalidades
- **hotfix/**: Correcciones urgentes
- **release/**: Preparación de releases

## 🤝 Contribución

### 📋 **Cómo Contribuir**

1. **Fork** el repositorio
2. **Clone** tu fork localmente
3. **Crea** una rama para tu feature
4. **Desarrolla** tu funcionalidad
5. **Ejecuta** las pruebas
6. **Commit** siguiendo Conventional Commits
7. **Push** a tu fork
8. **Crea** un Pull Request

### 🎯 **Áreas de Contribución**

- **Nuevas funcionalidades** del sistema
- **Mejoras en la interfaz** de usuario
- **Optimizaciones** de base de datos
- **Documentación** y ejemplos
- **Pruebas** unitarias e integración
- **Corrección de bugs** y issues

### 📚 **Recursos para Desarrolladores**

- [Documentación .NET 9](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Spectre.Console](https://spectreconsole.net/)
- [Principios SOLID](https://en.wikipedia.org/wiki/SOLID)
- [Arquitectura Hexagonal](https://en.wikipedia.org/wiki/Hexagonal_architecture_(software))
- [BreakLineEducate](https://www.youtube.com/@BreakLineEducate)

## 📄 Licencia

Este proyecto está bajo la **Licencia MIT**. Ver el archivo [LICENSE](LICENSE) para más detalles.
