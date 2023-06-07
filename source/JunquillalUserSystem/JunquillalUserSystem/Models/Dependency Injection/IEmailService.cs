namespace JunquillalUserSystem.Models.Dependency_Injection
{
    // Interfaz que abstrae la funcionalidad de envío de correo electrónico
    public interface IEmailService
    {
        void EnviarEmail(string mensaje, string correo);
    }
}
