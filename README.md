# TestMicroServiciosDevsu

API de microservicios desarrollada con **ASP.NET Core / .NET**, orientada a la gestión de clientes, cuentas bancarias, movimientos y reportes.

El proyecto está preparado para ejecutarse de forma contenerizada mediante **Docker** y utilizar **PostgreSQL** como motor de base de datos.

## Tecnologías

- .NET / ASP.NET Core
- C#
- Entity Framework Core
- PostgreSQL
- Docker
- Docker Compose
- REST API
- Swagger / OpenAPI
- Postman

## Arquitectura

El proyecto está organizado como una solución de microservicios:

```text
TestMicroServiciosDevsu/
│
├── UserService/
│   ├── Controllers/
│   ├── Services/
│   ├── Repositories/
│   ├── DTOs/
│   ├── Entities/
│   └── Program.cs
│
├── AccountService/
│   ├── Controllers/
│   ├── Services/
│   ├── Repositories/
│   ├── DTOs/
│   ├── Entities/
│   └── Program.cs
│
├── docker-compose.yml
├── Dockerfile
├── .dockerignore
├── .gitignore
└── README.md
```

> La estructura puede variar ligeramente dependiendo de la organización final de los proyectos dentro de la solución.

## Microservicios

### UserService

Responsable de la administración de clientes.

Base URL local:

```text
http://localhost:5062
```

Endpoints principales:

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/clientes` | Listar clientes |
| GET | `/api/clientes/{id}` | Obtener cliente |
| POST | `/api/clientes` | Crear cliente |
| PUT | `/api/clientes/{id}` | Actualizar cliente |
| DELETE | `/api/clientes/{id}` | Eliminar cliente |

### AccountService

Responsable de cuentas y movimientos.

Base URL local:

```text
http://localhost:5014
```

Endpoints principales:

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/cuentas` | Listar cuentas |
| GET | `/api/cuentas/{id}` | Obtener cuenta |
| POST | `/api/cuentas` | Crear cuenta |
| PUT | `/api/cuentas/{id}` | Actualizar cuenta |
| DELETE | `/api/cuentas/{id}` | Eliminar cuenta |
| POST | `/api/cuentas/{id}/movimientos` | Registrar movimiento |
| GET | `/api/movimientos` | Listar movimientos |
| GET | `/api/movimientos/{accountId}` | Movimientos de una cuenta |
| GET | `/api/reportes/movimientos` | Reporte de movimientos |



### Requisitos

Instalar:

- Docker Desktop
- Docker Compose
- Git

Verificar la instalación:

```bash
docker --version
docker compose version
```

##  Ejecución con Docker Compose

Desde la raíz del proyecto:

```bash
docker compose up --build
```

Para ejecutar los servicios en segundo plano:

```bash
docker compose up -d --build
```

Ver los contenedores:

```bash
docker compose ps
```

Ver logs:

```bash
docker compose logs -f
```

Ver logs de un servicio específico:

```bash
docker compose logs -f userservice
docker compose logs -f accountservice
docker compose logs -f postgres
```

Detener los servicios:

```bash
docker compose down
```

Detener y eliminar también los volúmenes:

```bash
docker compose down -v
```

> `docker compose down -v` elimina los datos persistidos de PostgreSQL. Utilizarlo únicamente cuando se quiera reiniciar la base de datos desde cero.





## Pruebas con Postman

La colección de Postman permite probar el flujo completo:

```text
1. Crear cliente
       ↓
2. Crear cuenta
       ↓
3. Registrar depósito
       ↓
4. Registrar retiro
       ↓
5. Consultar movimientos
       ↓
6. Generar reporte
```

Las variables principales son:

```text
userServiceUrl
accountServiceUrl
clientId
accountId
```

Ejemplo:

```text
userServiceUrl = http://localhost:5062
accountServiceUrl = http://localhost:5014
```

Los IDs generados por las operaciones de creación pueden almacenarse automáticamente como variables de colección.

## Movimientos

El endpoint de movimientos utiliza el signo del monto para determinar el tipo:

### Depósito

```json
{
  "amount": 100.00
}
```

Aumenta el saldo de la cuenta.

### Retiro

```json
{
  "amount": -100.00
}
```

Disminuye el saldo de la cuenta.

## Reportes

Ejemplo:

```http
GET /api/reportes/movimientos?clientId={clientId}&desde=2026-01-01&hasta=2026-12-31
```

El reporte debe ejecutarse sobre un cliente que tenga movimientos registrados.



## Swagger

Durante desarrollo, Swagger permite explorar y ejecutar los endpoints.

Ejemplo:

```text
http://localhost:5062/swagger
http://localhost:5014/swagger
```

Los puertos pueden cambiar dependiendo de la configuración de Docker Compose.



## Buenas prácticas

El proyecto busca aplicar:

- SOLID
- Clean Code
- Separation of Concerns
- Dependency Injection
- Repository Pattern
- DTOs para contratos de API
- Entity Framework Core
- Validación de datos
- Manejo centralizado de errores
- Configuración mediante variables de entorno
- Contenerización con Docker
- Persistencia mediante PostgreSQL
- Documentación mediante Swagger
- Pruebas de API mediante Postman



## Estructura Docker recomendada

```text
.
├── UserService/
│   ├── Dockerfile
│   └── ...
│
├── AccountService/
│   ├── Dockerfile
│   └── ...
│
├── docker-compose.yml
├── .dockerignore
├── .gitignore
└── README.md
```

##  Flujo completo

```bash
# 1. Clonar repositorio
git clone <repository-url>

# 2. Entrar al proyecto
cd TestMicroServiciosDevsu

# 3. Levantar infraestructura y APIs
docker compose up -d --build

# 4. Verificar contenedores
docker compose ps

# 5. Revisar logs
docker compose logs -f

# 6. Probar APIs mediante Swagger/Postman
```

## Solución de problemas

### PostgreSQL no inicia

Revisar:

```bash
docker compose logs postgres
```

### La API no puede conectarse a PostgreSQL

Verificar que el connection string utilice:

```text
Host=postgres
```

y que el servicio esté definido como:

```yaml
depends_on:
  - postgres
```

### El puerto ya está ocupado

Verificar los puertos utilizados:

```bash
docker ps
```

Cambiar el puerto externo en `docker-compose.yml`.

Por ejemplo:

```yaml
ports:
  - "5063:8080"
```

### Reiniciar completamente el entorno

```bash
docker compose down -v
docker compose up -d --build
```

## Licencia

Este proyecto es utilizado con fines de evaluación técnica y demostración de desarrollo de APIs con .NET, PostgreSQL y Docker.
