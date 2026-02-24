using VHBurguer.Exceptions;

namespace VHBurguer.Applications.Rules
{
    public class ValidarDataExpiracaoPromocao
    {
        public static void ValidarDataExpiracao(DateTime dataExpiracao)
        {
            if(dataExpiracao == DateTime.Now)
            {
                throw new DomainException("Data de expiração deve ser futura");
            }
        }
    }
}
