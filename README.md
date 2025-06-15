
# TFG Projects App - Frontend

Aplicación cliente desarrollada con .NET MAUI como parte del Trabajo de Fin de Grado en Desarrollo de Aplicaciones Multiplataforma. Esta aplicación permite la gestión colaborativa de proyectos, integrando funcionalidades inspiradas en herramientas como Trello y Milanote.

Para información sobre el servidor, visitar el siguiente enlace:

[TFG_Projects_APP_Backend](https://github.com/AlvaroSoldevilla/TFG_Projects_APP_Backend)

## Características

- Gestión de proyectos personales o colaborativos
- Mapas conceptuales interactivos
- Tableros de tareas con subtareas jerárquicas
- Controles personalizados: notas, tablas, contenedores...
- Sistema de permisos por proyecto
- Internacionalización de la interfaz (multiidioma)


## Tecnologías usadas

- **.NET MAUI** — Framework de desarrollo multiplataforma
- **CommunityToolkit.MVVM** — Arquitectura MVVM simplificada
- **XAML / C#** — Para la interfaz y lógica de negocio


## Instalación

Puedes encontrar una versión compilada de la aplicación en la carpeta Compiled.

Para clonar el repositorio:

```bash
    git clone https://github.com/AlvaroSoldevilla/TFG_Projects_APP_Frontend
    cd TFG_Projects_APP_Frontend
```

Para ejecutar la aplicación:

```bash
    dotnet build
    dotnet run
```

Para generar un ejecutable para Windows:

```bash
    dotnet publish -c Release -f net9.0-windows10.0.19041.0 -r win-x64 --self-contained true
```

El ejecutable se generará en la carpeta:

```bash
    bin\Release\net9.0-windows10.0.19041.0\win-x64\publish
```
