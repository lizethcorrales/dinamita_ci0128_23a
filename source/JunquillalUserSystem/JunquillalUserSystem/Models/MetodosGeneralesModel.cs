using System;
using System.Net.Mail;
using System.Net;
using System.Text;
using JunquillalUserSystem.Handlers;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace JunquillalUserSystem.Models
{
    public class MetodosGeneralesModel
    {
         private static readonly Random _random = new Random();

        public MetodosGeneralesModel()
        {

        }

        /*
         * Crea un ID de tamaño "length"
         */
        public string crearIdentificador(int length)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = allowedChars[_random.Next(0, allowedChars.Length)];
            }

            return new string(result);
        }


       
    }
}
