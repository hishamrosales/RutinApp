RutinApp 🏋️‍♀️

Aplicación móvil desarrollada en .NET MAUI para la gestión de rutinas y ejercicios físicos, permitiendo a los usuarios crear, visualizar, editar y eliminar rutinas de entrenamiento mediante persistencia local con SQLite y arquitectura MVVM.

📱 Descripción del Proyecto

RutinApp es una aplicación móvil enfocada en la organización de entrenamientos personalizados. El sistema permite administrar ejercicios y rutinas de manera dinámica, almacenando toda la información localmente en el dispositivo.

El proyecto fue desarrollado como integración final de los conocimientos adquiridos durante el curso, implementando buenas prácticas de arquitectura de software, persistencia de datos y separación de responsabilidades.

🚀 Tecnologías Utilizadas
.NET MAUI
C#
SQLite
Entity Framework Core
CommunityToolkit.MVVM
MVVM (Model-View-ViewModel)
XAML
Git & GitHub
🧩 Arquitectura Implementada

El proyecto sigue el patrón de arquitectura MVVM, separando claramente:

Models → Representación de entidades y datos.
Views → Interfaces gráficas en XAML.
ViewModels → Lógica de presentación y comunicación entre vistas y modelos.

Se utilizó el paquete:

CommunityToolkit.MVVM

para implementar:

Propiedades observables
Comandos
Notificación automática de cambios
💾 Persistencia de Datos

La aplicación utiliza:

SQLite como base de datos local.
Entity Framework Core para el acceso y manejo de datos.
Migraciones para la creación y actualización de la base de datos.

Las operaciones CRUD implementadas incluyen:

Crear registros
Consultar información
Actualizar datos
Eliminar registros
✨ Funcionalidades Principales
📌 Gestión de ejercicios
Agregar ejercicios
Editar ejercicios
Eliminar ejercicios
Visualizar lista de ejercicios
📌 Gestión de rutinas
Crear rutinas
Asociar ejercicios
Editar rutinas
Eliminar rutinas
📌 Persistencia local
Almacenamiento permanente en SQLite
📂 Estructura del Proyecto
RutinApp/
│
├── Models/
├── ViewModels/
├── Views/
├── Services/
├── Data/
├── Migrations/
├── Resources/
└── Platforms/
⚙️ Instalación y Ejecución
1. Clonar repositorio
git clone https://github.com/TU-USUARIO/RutinApp.git
2. Abrir solución

Abrir el archivo .sln en:

Visual Studio 2022 o superior
3. Restaurar paquetes NuGet
dotnet restore
4. Ejecutar migraciones (si aplica)
dotnet ef database update
5. Ejecutar proyecto

Seleccionar:

Android Emulator
Windows Machine
Dispositivo físico

y ejecutar la aplicación.

📸 Capturas del Sistema
Pantalla principal
Gestión de rutinas
<img width="1912" height="907" alt="image" src="https://github.com/user-attachments/assets/5eae42f2-2c9d-423e-9630-2fb0bc918c32" />
Gestión de ejercicios
<img width="1906" height="777" alt="image" src="https://github.com/user-attachments/assets/c0d195e9-4a86-4a60-866b-7c96f65b9436" />
Historial de Ejercicios
<img width="1918" height="336" alt="image" src="https://github.com/user-attachments/assets/b0f2eb61-925a-4eb6-bb66-fedbec059576" />


👥 Integrantes
Hisham Yael Rosales Morales
Elías 
Debanhi Librado Almada

📚 Lecciones Aprendidas

Durante el desarrollo del proyecto se reforzaron conocimientos relacionados con:

Arquitectura MVVM
Persistencia de datos local
Uso de Entity Framework Core
Manejo de SQLite en aplicaciones móviles
Organización y estructura de proyectos en .NET MAUI
Trabajo colaborativo mediante Git y GitHub

Uno de los principales retos fue implementar correctamente la comunicación entre vistas y ViewModels, así como mantener la sincronización adecuada entre la interfaz y la base de datos.

🔮 Mejoras Futuras

Posibles mejoras para versiones futuras:

Sistema de autenticación
Estadísticas de progreso
Notificaciones y recordatorios
Modo claro

📌 Estado del Proyecto

✅ Proyecto finalizado
✅ Funcionalidades principales operativas
✅ Persistencia de datos implementada
✅ Arquitectura MVVM aplicada

📄 Licencia

Proyecto desarrollado con fines académicos.
