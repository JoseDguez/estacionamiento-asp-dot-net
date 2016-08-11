using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EstacionamientoUDCI.App_Code
{
    public class GeneralClass
    {
    }

    //clase para los datos del usuario
    public class UserData
    {
        public string EnrollmentNumber { get; set; } //matricula
        public string Name { get; set; }             //nombre
        public string Lastname { get; set; }         //apellido
        public string Email { get; set; }            //email
        public string Turn { get; set; }             //turno
        public string Rol { get; set; }              //rol
    }
}