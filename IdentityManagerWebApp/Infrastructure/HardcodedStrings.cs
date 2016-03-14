using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityManagerWebApp.Infrastructure
{
    public static class HardcodedStrings
    {
        public const string ChangePasswordSuccess = "Su contraseña ha sido cambiada.";
        public const string DuplicateUserName = "El usuario ya existe.";
        public const string Error = "Ha ocurrido un error.";
        public const string InvalidLoginAttempt = "Inicio de sesión inválido.";
        public const string PasswordMismatch = "Contraseña incorrecta";
        public const string PasswordRequiresDigit = "La contraseña debe tener al menos un dígito ('0'-'9').";
        public const string PasswordRequiresNonLetterAndDigit = "La contraseña debe tener al menos un caracter que no sea ni letra ni número.";
        public const string SetPasswordSuccess = "Su contraseña ha sido establecida.";
        public const string UserAlreadyHasPassword = "El usuario ya tiene una contraseña establecida.";
    }
}