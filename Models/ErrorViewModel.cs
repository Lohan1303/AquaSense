using System;

namespace AquaSense.Models
{
    public class ErrorViewModel
    {
        public ErrorViewModel(string msgErro)
        {
            Erro = msgErro;
        }

        public ErrorViewModel()
        { }

        public string Erro { get; set; }

        #region Inútil 

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        #endregion
    }
}
