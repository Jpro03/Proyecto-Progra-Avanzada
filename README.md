# Fidepolis – Proyecto Final SC-701 Programación Avanzada

Aplicación web del cine Fidepolis para consultar la cartelera y realizar compras simuladas de entradas. Desarrollada en ASP.NET Core MVC (.NET 8).

## 1. Integrantes del grupo

- 
- 
- 
- 

## 2. Repositorio

https://github.com/Jpro03/Proyecto-Progra-Avanzada

## 3. Especificación del proyecto

### a. Arquitectura

Solución `Fidepolis.sln` con patrón MVC y arquitectura en capas, compuesta por 4 proyectos:

| Proyecto | Tipo | Capa | Contenido |
|---|---|---|---|
| `Fidepolis.Web` | ASP.NET Core MVC | Presentación | Controladores, vistas, ViewModels y archivos estáticos |
| `Fidepolis.Business` | Biblioteca de clases | Lógica de negocio | Servicios con las reglas del cine |
| `Fidepolis.Data` | Biblioteca de clases | Datos | Repositorios de acceso a datos |
| `Fidepolis.Entities` | Biblioteca de clases | Entidades | Clases del dominio compartidas por todas las capas |

Referencias: `Web → Business → Data → Entities`. Los controladores solo se comunican con la capa de negocio.

### b. Librerías y paquetes NuGet

- `Microsoft.NET.Sdk.Web` (ASP.NET Core MVC, incluido en .NET 8)

### c. Principios SOLID y patrones de diseño

**Principios SOLID**
- **Responsabilidad única (SRP):** cada capa y cada clase tiene una sola responsabilidad (presentar, aplicar reglas o acceder a datos).
- **Inversión de dependencias (DIP):** los controladores dependen de `ICinemaService` y el servicio depende de `ICinemaRepository`, no de implementaciones concretas.

**Patrones de diseño**
- **MVC:** separación entre modelos, vistas y controladores en la capa de presentación.
- **Repository:** el acceso a datos se encapsula detrás de `ICinemaRepository`.
- **Inyección de dependencias:** las implementaciones se registran en `Program.cs` y se inyectan por constructor.
- **ViewModel:** cada vista recibe un modelo con solo los datos que necesita.

## Ejecución

```bash
dotnet run --project Fidepolis.Web
```
