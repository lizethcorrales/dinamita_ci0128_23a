using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunquillalUserSystemTest
{
    public class CalculadorFechas
    {
        private static int[] Meses = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        public static bool anioBisiesto(int anio)
        {
            return anio % 4 == 0;
        }

        public static bool esFebrero(int mes)
        {
            return mes == 2;
        }

        public static int obtenerDiasDeMes(int mes, int anio)
        {
            return esFebrero(mes) && anioBisiesto(anio) ? Meses[mes - 1] + 1 : Meses[mes - 1];
        }
    }
}
