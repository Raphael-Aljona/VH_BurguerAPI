
using DotNetEnv;
using Google.GenAI;
using VHBurguer.Exceptions;

namespace VHBurguer.Applications.ContentSafety
{
    public class ContentSafetyService : IContentSafetyRepository
    {
        private readonly string _apiKey;

        public ContentSafetyService(IConfiguration configuration)
        {
            _apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? throw new Exception("Api key não configurada.");
        }

        public async Task<(bool aprovado, string msg)> ValidarConteudo(string texto)
        {
            if (string.IsNullOrEmpty(_apiKey)) return (true, texto);

            try
            {
                Client client = new Client(apiKey: _apiKey);

                string prompt = $@"Você é um moderador de conteúdo extremamente rigoroso para uma plataforma pública.

                    Analise o TEXTO abaixo considerando as regras:

                    - NÃO é permitido:
                      - palavrões, xingamentos ou linguagem vulgar (ex: ""caralho"", ""porra"", ""merda"", etc.)
                      - conteúdo ofensivo, agressivo ou desrespeitoso
                      - conteúdo com duplo sentido ou conotação sexual
                      - qualquer linguagem inadequada para ambiente profissional ou educacional
                      - conteúdo ilegal (drogas, armas, etc.)

                    - Mesmo que esteja em tom informal ou ""brincadeira"", ainda deve ser considerado INSEGURO.

                    - Seja extremamente conservador: na dúvida, classifique como INSEGURO.

                    Responda APENAS com:

                    SEGURO ou INSEGURO: [breve motivo em português]

                    TEXTO:{texto}";

                var response = await client.Models.GenerateContentAsync(
                    model: "gemini-2.5-flash-lite",
                    contents: prompt
                    );

                string result = response.Text?.Trim().ToUpper() ?? string.Empty;

                if(result.StartsWith("SEGURO"))
                {
                    return (true, result);
                }

                return(false, result);
            }
            catch (DomainException ex)
            {
                return (false, "Erro na IA" + ex.Message);
            }
        }
    }
}
