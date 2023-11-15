using Microsoft.AspNetCore.Http;
using System;

namespace AquaSense.Models
{
    public class UsuarioViewModel : PadraoViewModel
    {
        public string LoginUsuario { get; set; }
        public string NomePessoa { get; set; }
        public string Senha { get; set; }
        public bool Adm { get; set; }
        public IFormFile Imagem { get; set; }
        /// <summary>
        /// Imagem em bytes pronta para ser salva
        /// </summary>
        public byte[] ImagemEmByte { get; set; }
        /// <summary>
        /// Imagem usada para ser enviada ao form no formato para ser exibida
        /// </summary>
        public string ImagemEmBase64
        {
            get
            {
                if (ImagemEmByte != null)
                    return Convert.ToBase64String(ImagemEmByte);
                else
                    return string.Empty;
            }
        }

        public string NomeConjuntoHabitacional { get; set; }
    }
}
