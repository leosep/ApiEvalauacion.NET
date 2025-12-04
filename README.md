# ApiEvaluacion.NET

Esta es una API RESTful en .NET 8 para autenticación de usuarios y acceso a API externa de posts.

## Arquitectura

El proyecto está estructurado en capas:

- **ApiEvaluacion.Domain**: Contiene entidades de dominio (Usuario).
- **ApiEvaluacion.Application**: Contiene servicios de aplicación, DTOs y lógica de negocio.
- **ApiEvaluacion.Infrastructure**: Contiene capa de acceso a datos con EF Core.
- **ApiEvaluacion.Presentation**: Contiene controladores de ASP.NET Core Web API y configuración.

## Características

- Registro de usuario con validaciones (nombre, regex de correo, fortaleza de contraseña, correo único).
- Inicio de sesión de usuario con generación de token JWT.
- Endpoints protegidos para GET y POST a https://jsonplaceholder.typicode.com/posts.
- Base de datos en memoria usando EF Core.
- Hashing de contraseñas con BCrypt.
- Autenticación JWT.
- Documentación Swagger.
- FluentValidation para validaciones de entrada.
- Pruebas unitarias básicas con xUnit.

## Prerrequisitos

- SDK de .NET 8

## Instalación

1. Clona el repositorio.
2. Navega al directorio raíz.
3. Restaura paquetes: `dotnet restore`
4. Construye la solución: `dotnet build`

## Ejecutando la Aplicación

1. Establece ApiEvaluacion como proyecto de inicio.
2. Ejecuta: `dotnet run --project ApiEvaluacion.Presentation`
3. La API estará disponible en https://localhost:5001 o http://localhost:5000
4. Interfaz de usuario de Swagger: https://localhost:5001/swagger

## Endpoints de la API

### Autenticación

- **POST /api/auth/register**
  - Cuerpo: { "name": "string", "email": "string", "password": "string" }
  - Retorna: { "name": "string", "email": "string", "id": "guid", "token": "string" }

- **POST /api/auth/login**
  - Cuerpo: { "email": "string", "password": "string" }
  - Retorna: { "token": "string" }

### Publicaciones (Protegidas)

- **GET /api/posts**
  - Cabeceras: Authorization: Bearer {token}
  - Retorna: Array JSON de publicaciones de jsonplaceholder.typicode.com

- **POST /api/posts**
  - Cabeceras: Authorization: Bearer {token}
  - Cuerpo: Objeto JSON para datos de publicación
  - Retorna: Respuesta de jsonplaceholder.typicode.com

## Pruebas

- Para pruebas unitarias: `dotnet test`

## Configuración

- El secreto JWT y patrones regex están en appsettings.json.
- La base de datos es en memoria, los datos persisten solo durante el tiempo de ejecución de la aplicación.

## Video

<video width="320" height="240" controls>
  <source src="https://leandrosepulveda.com/recursos/api.mp4" type="video/mp4">
  No lo soporta.
</video>