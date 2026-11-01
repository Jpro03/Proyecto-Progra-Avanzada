# Fidepolis – Proyecto final Programación Avanzada SC-701

Aplicación web de cine con compra **simulada** de entradas, desarrollada en **ASP.NET Core MVC (.NET 8)**.

## Instalación / Visual Studio Code

1. Instalar .NET 8 SDK y Visual Studio Code con la extensión **C# Dev Kit**.
2. Abrir la carpeta `Fidepolis` en VS Code (la carpeta que contiene `Fidepolis.csproj`).
3. Abrir la terminal integrada (`Ctrl+ñ` o Terminal > New Terminal).
4. Ejecutar:

```bash
dotnet restore
dotnet run --urls http://localhost:5081
```

5. Abrir `http://localhost:5081`. Si el puerto está ocupado, usar otro.

## Funcionalidades

- Cartelera con búsqueda por título, filtro por género y resultado vacío.
- Detalle de película y horarios futuros con disponibilidad y precio.
- Mapa de 40 asientos: A1-E8, máximo 6 por compra, indicador de ocupados y actualización periódica.
- Revalidación en servidor antes de registrar: ninguna compra parcial.
- Formulario con validación, compra simulada sin datos bancarios y código aleatorio único.
- Comprobante, consulta por código y cancelación previa al inicio de la función.
- Persistencia local de compras en `AppData/purchases.json`, generado automáticamente.

## Arquitectura (MVC y capas)

- **Presentación**: `Controllers/`, `Views/`, `ViewModels/`, `wwwroot/`.
- **Negocio**: `Services/ICinemaService.cs`, `Services/CinemaService.cs`.
- **Datos**: `Data/ICinemaRepository.cs`, `Data/JsonCinemaRepository.cs`.
- **Entidades**: `Models/`.

Controladores delegan las reglas de negocio a `ICinemaService`; el servicio usa `ICinemaRepository`, y así se pueden cambiar las implementaciones sin reescribir los controladores (inversión de dependencias). Cada clase tiene una responsabilidad principal (responsabilidad única). No se requieren paquetes NuGet externos; se utiliza el framework ASP.NET Core MVC incluido en `Microsoft.NET.Sdk.Web`.

## Validaciones y seguridad

- Formulario validado con DataAnnotations y comprobación en servidor.
- Tokens antifalsificación (`ValidateAntiForgeryToken`) en operaciones POST.
- Vistas Razor codifican las cadenas mostradas y evitan insertar HTML de usuario.
- Código de compra generado con números aleatorios criptográficamente seguros.
- Escrituras JSON con archivo temporal y bloqueo de aplicación para compras/cancelaciones concurrentes en la misma instancia.
- Sin información de pago real.

**Limitaciones académicas:** datos locales JSON, sin autenticación ni envío de correos; una sola instancia del servidor. Para producción: base de datos transaccional, control de concurrencia entre procesos, autorización para consultar/cancelar compras y políticas de protección de datos.

## Entrega / datos a completar

- **Integrantes:** [Completar con los nombres reales del grupo]
- **Repositorio:** https://github.com/Jpro03/Proyecto-Progra-Avanzada
- **Curso:** SC-701 Programación Avanzada
- **Patrones:** MVC, Repository, Dependency Injection.
- **SOLID:** SRP, DIP, ISP mediante interfaces acotadas.

## Subir al repositorio

Desde la carpeta raíz del proyecto, si el repositorio remoto está vacío:

```bash
git init
git branch -M main
git remote add origin https://github.com/Jpro03/Proyecto-Progra-Avanzada.git
git add .
git commit -m "Proyecto final Fidepolis MVC"
git push -u origin main
```

Si el repositorio ya tiene archivos, clonarlo primero y copiar esta carpeta o su contenido dentro del clon; hacer `git pull` antes de `git push`. No sobrescribir cambios existentes.
