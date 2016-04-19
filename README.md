# IdentityTemplate
Proyecto Web Template que utiliza como base al framework Identity 2.0 de MVC para autorizacion/autenticación de usuarios
de una aplicación. Este proyecto se basa en un modelo de Usuarios/Roles/Claims tal como se define en ASP.NET Identity, este modelo puede extenderse/redefinirse dependiendo requerimientos a cumplir.

En este proyecto se encuentran tanto el modelo, componentes de servicios y vistas que implementan los CRUD's
básicos de las entidades del dominio.


Componente ApplicationUserManager
---------------------------------
  
Componente que implementa la clase UserManager de Microsoft.AspNet.Identity y extiende funcionalidad no implementada en el framework.

Ver [ApplicationUserManager.cs](https://github.com/entaconsulting/IdentityTemplate/blob/master/IdentityManagerWebApp/App_Start/ApplicacionUserManager.cs) donde estan las extensiones como AddClaimsAsync o GetPagedAsync (obtiene una paginacion de usuarios) entre otras.
  
