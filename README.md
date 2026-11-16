# DicSign — Aplicación .NET MAUI

DicSign es una aplicación educativa de ejemplo para explorar vocabulario y el abecedario dactilológico, practicar con tarjetas, guardar favoritos y consultar el avance del usuario.

## Requisitos

- Visual Studio 2026 (o una versión compatible con .NET 10).
- Carga de trabajo **Desarrollo de la interfaz de usuario de aplicaciones multiplataforma de .NET (MAUI)**.
- .NET 10 SDK y los componentes de Android Emulator/SDK para ejecutar en Android.
- Para Windows, el componente de desarrollo de aplicaciones de Windows correspondiente.

## Abrir y ejecutar

1. Descomprime la carpeta `DicSign`.
2. Abre `DicSign.sln` con Visual Studio.
3. Espera a que Visual Studio restaure los componentes del proyecto.
4. Selecciona el destino `Windows Machine` o un emulador/dispositivo Android.
5. Ejecuta con **F5**.

El proyecto está configurado para `net10.0-android` y `net10.0-windows10.0.19041.0`.

## Funciones incluidas

- Pantalla de bienvenida, registro e inicio de sesión.
- Cuentas locales con contraseñas almacenadas como hash PBKDF2.
- Diccionario con búsqueda y filtros de Saludos y Abecedario.
- Detalle de palabras/letras y acceso al recurso visual educativo de LSM.
- Abecedario de práctica con respuestas de “No lo sé” y “Aprendido”.
- Favoritos guardados por usuario.
- Estadísticas de práctica, configuración para restablecer estadísticas y cierre de sesión.
- Persistencia local en un archivo JSON dentro del almacenamiento privado de la aplicación.

## Nota importante sobre el contenido de LSM

La LSM es una lengua natural con gramática propia. El abecedario dactilológico se usa para deletrear palabras del español; no sustituye las señas léxicas ni la gramática de la LSM.

Para no enseñar gestos inventados, las tarjetas de ejemplo no representan sus iconos como señas correctas. El botón **Ver recurso de LSM** abre/integra el material de la Biblioteca Universitaria UABCS, cuyos materiales de alfabeto se presentan como recurso educativo abierto CC BY-SA 4.0:

https://biblio.uabcs.mx/recursosedu/lsm/

Las palabras de la categoría “Saludos” son fichas de vocabulario de demostración. Antes de publicar DicSign, completa esas entradas con videos o imágenes de señas léxicas revisadas por personas sordas y especialistas en LSM, incluyendo atribución y permisos del material.

## Alcance de las cuentas

El registro y el inicio de sesión son locales al dispositivo. No hay servidor, sincronización entre dispositivos, recuperación de contraseña ni autenticación en la nube. Para producción se necesitaría un backend seguro y pruebas de accesibilidad con usuarios de la comunidad sorda.
