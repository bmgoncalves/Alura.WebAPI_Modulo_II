using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.Api.Formatters
{
    /// <summary>
    /// Classe para enviar para a API o objeto livro em formato CSV
    /// </summary>
    public class LivroCSVFormatter : TextOutputFormatter
    {
        public LivroCSVFormatter()
        {
            MediaTypeHeaderValue textCsvMediaType = MediaTypeHeaderValue.Parse("text/csv");
            MediaTypeHeaderValue appCsvMediaType = MediaTypeHeaderValue.Parse("application/csv");
            SupportedMediaTypes.Add(textCsvMediaType);
            SupportedMediaTypes.Add(appCsvMediaType);
            SupportedEncodings.Add(Encoding.UTF8);
        }

        /// <summary>
        /// Metodo para tratar o retorno, caso o tipo não seja da classe destinada para conversão (Livroapi), enviar no formato padrão (JSON)
        /// </summary>
        /// <param name="type">Tipo</param>
        /// <returns>Retorn se o tipo esta correto</returns>
        protected override bool CanWriteType(Type type)
        {
            return type == typeof(LivroApi);
        }


        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var livroEmCsv = "";

            if (context.Object is LivroApi)
            {
                var livro = context.Object as LivroApi;
                livroEmCsv = $"{ livro.Titulo}; {livro.Subtitulo };{livro.Autor };{livro.Lista }";
            }

            using (var escritor = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
            {
                return escritor.WriteAsync(livroEmCsv);
            } //escritor.Close()
        }
    }
}
