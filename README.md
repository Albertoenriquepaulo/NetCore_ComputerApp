# NetCore_ComputerApp
Final Project 3rd Module BBKBootCamp. Build your own PC. .Newt Core EF w Identity
**This is bold text**

## Pasos para crear el proyecto .NetCore
1. Crear el modelo
2. Si hay manejo de usuarios con Identity, modificar *Startup.cs*, en este caso nuestra extension de la clase usuario se llama **AppUser**
```
 services.AddDefaultIdentity<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
```
3. Irse al context, ubicado en *Data* folder, en este caso *ApplicationDbContext.cs*
```
Antes:      public class ApplicationDbContext 
Debe verse: public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole, string>
```
*Todo esto hecho antes del Migration*

4. Añadir *New Scaffolded Item* para tarernos todas las Razor del Identity
5. Editar *_LoginPartial.cshtml*
```
Antes

@using Microsoft.AspNetCore.Identity
@inject SignInManager<IdentityUser> SignInManager
@inject UserManager<IdentityUser> UserManager


Debe verse

@using Microsoft.AspNetCore.Identity
@inject SignInManager<AppUser> SignInManager
@inject UserManager<AppUser> UserManager
```
6. Modificar *RegisterConfirmation.cshtml.cs*, por alguna extraña razón el VS no lo hace. Añadir la siguiente libreria
```
using Microsoft.AspNetCore.Identity;
```
_De no hacerlo el migration dará failed, y será muy fácil darse cuenta del error, corregirlo para poder hacer el migration_

7. Hacer el migration *add-migration* y *update-database*
