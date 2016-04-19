# IdentityTemplate
Proyecto Web Template que utiliza como base el framework Identity 2.0 de MVC para autorizacion/autenticación de usuarios
de una aplicación. Este proyecto se basa en un modelo de Usuarios/Roles/Claims tal como se define en ASP.NET Identity, este modelo puede extenderse/redefinirse dependiendo requerimientos a cumplir.

En este proyecto se encuentran tanto el modelo, componentes y vistas que implementan los CRUD's de las entidades del dominio.


Componente ApplicationUserManager
---------------------------------
  
Componente que implementa la clase UserManager de Microsoft.AspNet.Identity y extiende funcionalidad no implementada en el framework.

Ver [ApplicationUserManager.cs](https://github.com/entaconsulting/IdentityTemplate/blob/master/IdentityManagerWebApp/App_Start/ApplicacionUserManager.cs) donde estan las extensiones como AddClaimsAsync o GetPagedAsync (obtiene una paginación de usuarios) entre otras.
  
Controller/View 
------------------

Se implementó el siguiente controller [AccountManagerController.cs](https://github.com/entaconsulting/IdentityTemplate/blob/master/IdentityManagerWebApp/Controllers/AccountManagerController.cs) donde se encuentra la funcionalidad estandar CRUD de Usuario, Rol y Claim.
Las [vistas](https://github.com/entaconsulting/IdentityTemplate/tree/master/IdentityManagerWebApp/Views/AccountManager) contienen dependencias a controles externos de Telerik-Kendo para lograr un mejor look and feel y funcionalidad.

Demo Ejecutable
---------------
Descargar el template y ejecutar. Se creará la base de datos Sql Server con el esquema de Identity dependiendo donde tenga configurado el archivo [Web.config](https://github.com/entaconsulting/IdentityTemplate/blob/master/IdentityManagerWebApp/Web.config)

    <connectionStrings>
      <add name="DefaultConnection" connectionString="Data Source=(LocalDb)\v11.0;Initial Catalog=IdentityManagerApp;Integrated Security=SSPI;MultipleActiveResultSets=True" providerName="System.Data.SqlClient" />
    </connectionStrings>
Una vez generada la base se crearán usuarios de prueba para probar la app y navegar en ella.
