# TaskNotes API

TaskNotes API es una API REST educativa para administrar categorías y notas
privadas por usuario. Incluye registro e inicio de sesión con ASP.NET Core
Identity, autenticación JWT, filtros y paginación.

## Tecnologías

- ASP.NET Core Web API sobre .NET 10
- Entity Framework Core 10
- SQL Server LocalDB
- ASP.NET Core Identity
- Autenticación JWT Bearer
- Swagger/OpenAPI

## Funcionalidades

- Registro, inicio de sesión y consulta del perfil autenticado.
- CRUD de categorías protegido por JWT.
- CRUD de notas protegido por JWT.
- Marcado de notas como importantes.
- Filtrado de notas por texto, prioridad, categoría e importancia.
- Paginación con un máximo de 100 registros por página.
- Middleware global para respuestas de error.
- Swagger configurado para enviar tokens Bearer.

Cada usuario solo puede consultar o modificar sus propias categorías y notas.
Las respuestas de autenticación exponen únicamente el identificador, nombre,
correo, token y fecha de expiración; no exponen propiedades internas de
Identity.

## Estructura del proyecto

```text
Controllers/   Endpoints HTTP
Data/          DbContext de Entity Framework Core
DTOs/          Modelos de entrada y salida de la API
Entities/      Entidades de Identity, categorías y notas
Helpers/       Respuesta paginada
Interfaces/    Contratos de los servicios
Middleware/    Manejo global de excepciones
Migrations/    Migraciones de Entity Framework Core
Services/      Autenticación, JWT, categorías y notas
Settings/      Configuración tipada de JWT
```

## Requisitos

- .NET SDK 10
- SQL Server LocalDB (incluido habitualmente con Visual Studio en Windows)
- La herramienta `dotnet-ef` compatible con EF Core 10 para aplicar migraciones

## Configuración

La cadena incluida en `appsettings.json` usa la instancia local
`(localdb)\MSSQLLocalDB` y la base de datos `TaskNotesDB`. Puedes cambiarla en
ese archivo para tu entorno local:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TaskNotesDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Si la cadena contiene credenciales, guárdala en User Secrets en lugar de
confirmarla en Git:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "TU_CADENA_DE_CONEXION"
```

`appsettings.json` contiene solamente un valor descriptivo para `Jwt:Key`.
Configura una clave larga y aleatoria en User Secrets:

```powershell
dotnet user-secrets set "Jwt:Key" "TU_CLAVE_LARGA_Y_ALEATORIA_DE_DESARROLLO"
```

El proyecto ya tiene configurado un `UserSecretsId`; no es necesario ejecutar
`dotnet user-secrets init`.

## Restaurar, preparar y ejecutar

Desde la raíz del proyecto:

```powershell
dotnet restore
dotnet ef database update
dotnet run
```

Con el perfil HTTPS, la API usa normalmente
`https://localhost:7239`; con el perfil HTTP usa
`http://localhost:5043`. Los puertos están definidos en
`Properties/launchSettings.json`.

## Swagger y autenticación

En el entorno `Development`, abre `/swagger`, por ejemplo:

```text
https://localhost:7239/swagger
```

1. Ejecuta `POST /api/auth/registro` o `POST /api/auth/login`.
2. Copia el valor `token` de la respuesta.
3. Pulsa **Authorize** en Swagger e introduce el token JWT.
4. Ejecuta los endpoints protegidos de perfil, categorías o notas.

## Endpoints

| Método | Ruta | Autenticación | Descripción |
| --- | --- | --- | --- |
| POST | `/api/auth/registro` | No | Registrar un usuario y devolver un JWT |
| POST | `/api/auth/login` | No | Iniciar sesión y devolver un JWT |
| GET | `/api/auth/perfil` | Bearer | Consultar el perfil autenticado |
| POST | `/api/categorias` | Bearer | Crear una categoría |
| GET | `/api/categorias` | Bearer | Listar categorías |
| GET | `/api/categorias/{categoriaId}` | Bearer | Consultar una categoría |
| PUT | `/api/categorias/{categoriaId}` | Bearer | Actualizar una categoría |
| DELETE | `/api/categorias/{categoriaId}` | Bearer | Eliminar una categoría |
| POST | `/api/notas` | Bearer | Crear una nota |
| GET | `/api/notas` | Bearer | Listar, filtrar y paginar notas |
| GET | `/api/notas/{notaId}` | Bearer | Consultar una nota |
| PUT | `/api/notas/{notaId}` | Bearer | Actualizar una nota |
| DELETE | `/api/notas/{notaId}` | Bearer | Eliminar una nota |
| PATCH | `/api/notas/{notaId}/importante` | Bearer | Alternar el estado importante |

## Ejemplos JSON

Registro:

```json
{
  "nombre": "Ada Lovelace",
  "email": "ada@example.com",
  "password": "clave123"
}
```

Login:

```json
{
  "email": "ada@example.com",
  "password": "clave123"
}
```

Crear o actualizar una categoría:

```json
{
  "nombre": "Trabajo"
}
```

Crear o actualizar una nota:

```json
{
  "titulo": "Preparar presentación",
  "contenido": "Terminar las diapositivas antes del viernes.",
  "prioridad": 3,
  "categoriaId": 1
}
```

Los valores de `prioridad` son `1` (Baja), `2` (Media) y `3` (Alta).
`esImportante` comienza en `false` al crear una nota y se alterna mediante el
endpoint `PATCH`.

## Filtros y paginación

`GET /api/notas` acepta estos parámetros opcionales en la cadena de consulta:

- `buscar`: texto contenido en el título o el contenido.
- `prioridad`: `1`, `2` o `3`.
- `categoriaId`: identificador de una categoría.
- `esImportante`: `true` o `false`.
- `pagina`: número de página, mínimo `1` y valor predeterminado `1`.
- `cantidadPorPagina`: registros por página, entre `1` y `100`; el valor
  predeterminado es `10`.

Ejemplo:

```text
GET /api/notas?buscar=presentación&prioridad=3&esImportante=true&pagina=1&cantidadPorPagina=10
```

La respuesta conserva el formato `RespuestaPaginada<T>` e incluye la página
actual, cantidad por página, total de registros, total de páginas y datos.

## Propósito

Este es un proyecto educativo creado para practicar ASP.NET Core Web API,
Entity Framework Core, Identity y autenticación JWT. No está presentado como
una solución de producción.
