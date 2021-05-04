# VirtualMind.CodeChallenge.API
### Como correr el proyecto?

1. Ejecutar el archivo "CREATE DATABASE.SQL" en la base de datos local.
2. Abrir la solucion **VirtualMind.CodeChallenge.API.sln** en Visual Studio.
3. Selecionar **VirtualMind.CodeChallenge.API** como startup project.
4. Presionar F5

# Preguntas
## ¿Qué opina de pasar el id de usuario como input del endpoint?
Es una practica que compromete data muy sencible de la aplicacion y se pueden explotar bulnerabilidades a causa de esto.

## ¿Cómo mejoraría la transacción para asegurarnos de que el usuario que pidió la compra es quien dice ser?
Integrando seguridad atravez de Json Web Tokkens (JWT) y enviar los datos del usuarios encriptados para que el servidor 
pueda verificar su autenticacion y autorizacion por medio de los Claims.


# Proyecto Angular
https://github.com/luisferomero/virtualmind-codeChallenge-angular
