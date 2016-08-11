# estacionamiento-asp-dot-net
Proyecto escolar para controlar la asignación de cajones de estacionamiento de una escuela.

## Funcionalidades
1. **Registro:** Un usuario puede registrarse en el sistema.
2. **Login:** Un usuario puede iniciar sesión en el sistema.
3. **Admin panel:** Un usuario de tipo 'administrador' tiene acceso a un panel de administración, en el cual puede
administrar los usuarios registrados, los cajones del estacionamiento y las solicitudes pendientes.
4. **Registro de auto:** Un usuario puede registrar su automovil.
5. **Solicitud de cajón:** Un usuario puede solicitar un cajón del estacionamiento.
6. **Reportes:** El sistema muestra un reporte de pago para que el usuario realice el pago por el servicio.
7. **Registro ciclo escolar:** Un usuario de tipo 'administrador' tiene acceso al registro de ciclos escolares.

## Descripción
El funcionamiento básico del sistema consiste en: el alumno registra su automovil, una vez tenga un auto asignado puede solicitar
un cajón de estacionamiento. El usuario de tipo 'administrador' procesa estas solicitudes, aprobando o rechazandolas.

El alumno puede imprimir su reporte de pago si un administrador aprobó su solicitud, una vez realizado el pago el alumno regresa con
el administrador para finalizar la asignación del cajón.

## Desarrollo y Plugins
El sistema fue desarrollado en C# ASP.NET, haciendo uso también de ReportViewer y RDLC.
La base de datos está manejada en SQL Server (Express) y se utilizaron los siguientes plugins/frameworks:

1. jQuery (https://jquery.com/)
2. Bootstrap (http://getbootstrap.com/)
