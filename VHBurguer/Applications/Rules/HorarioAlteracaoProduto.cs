using VHBurguer.Exceptions;

namespace VHBurguer.Applications.Rules
{
    public class HorarioAlteracaoProduto
    {
        public static void ValidarHorario()
        {
            var agora = DateTime.Now.TimeOfDay;
            var abertura = new TimeSpan(10); // alterar para 16h
            var fechamento = new TimeSpan(23);

            var estaAberto = agora >= abertura && agora <= fechamento;

            if (estaAberto)
            {
                throw new DomainException("Produto só pode ser alterado fora do horário de funcionamento");
            }
        }
    }
}
