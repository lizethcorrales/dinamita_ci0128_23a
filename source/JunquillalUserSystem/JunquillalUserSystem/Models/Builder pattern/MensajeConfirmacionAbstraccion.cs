using JunquillalUserSystem.Models.Patron_Bridge;

namespace JunquillalUserSystem.Models.Builder_pattern
{
      // Clase abstracta que representa la abstracción en el patrón Bridge
      public abstract class MensajeConfirmacionAbstraccion
      {
        

          // Constructor sin parámetros
          protected MensajeConfirmacionAbstraccion()
          {
              // Declaración vacía para permitir la llamada en la clase derivada
          }


         // Método abstracto que debe ser implementado por las subclases concretas
         public abstract string CrearConfirmacionMensaje(ReservacionModelo reservacion, HospederoModelo hospedero, List<PrecioReservacionDesglose> desglose);
      }
 }

