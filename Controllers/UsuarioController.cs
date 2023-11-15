using AquaSense.DAO;
using AquaSense.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace AquaSense.Controllers
{
    public class UsuarioController : PadraoController<UsuarioViewModel>
    {
        public UsuarioController()
        {
            DAO = new UsuarioDAO();
        }


        public override IActionResult Save(UsuarioViewModel model, string Operacao)
        {
            try
            {
                ValidaDados(model, Operacao);

                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    PreencheDadosParaView(Operacao, model);
                    return View(NomeViewForm, model);
                }
                else
                {
                    ConjuntoHabitacionalViewModel conj = new ConjuntoHabitacionalViewModel()
                    {
                        Nome = model.NomeConjuntoHabitacional
                    };
                    if (Operacao == "I")
                    {
                        conj.IdUsuarioAdm = DAO.Insert(model);
                    }
                    else
                    {
                        DAO.Update(model);
                    }

                    return RedirectToAction(NomeViewIndex);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public byte[] ConvertImageToByte(IFormFile file)
        {
            if (file != null)
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            else
                return null;
        }

        protected override void ValidaDados(UsuarioViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);
            if (string.IsNullOrEmpty(model.NomePessoa))
                ModelState.AddModelError("NomePessoa", "Preencha o nome.");
            if (string.IsNullOrEmpty(model.NomeConjuntoHabitacional))
                ModelState.AddModelError("NomeConjuntoHabitacional", "Preencha o portifolio.");
            if (string.IsNullOrEmpty(model.Senha))
                ModelState.AddModelError("senha", "Preencha a senha.");
            if (model != null && !string.IsNullOrEmpty(model.Senha) && model.Senha.Length < 4)
                ModelState.AddModelError("senha", "A senha tem que conter pelo menos 4 caracteres");
            if (model.Imagem == null && operacao == "I")
                ModelState.AddModelError("Imagem", "Escolha uma imagem.");
            if (model.Imagem != null && model.Imagem.Length / 1024 / 1024 >= 2)
                ModelState.AddModelError("Imagem", "Imagem limitada a 2 mb.");
            if (ModelState.IsValid)
            {
                //na alteração, se não foi informada a imagem, iremos manter a que já estava salva.
                if (operacao == "A" && model.Imagem == null)
                {
                    UsuarioViewModel cid = DAO.Consulta(model.Id);
                    model.ImagemEmByte = cid.ImagemEmByte;
                }
                else
                {
                    model.ImagemEmByte = ConvertImageToByte(model.Imagem);
                }

            }

        }
        public override IActionResult Edit(int id)
        {
            try
            { 
                ViewBag.Operacao = "A";
                var model = DAO.Consulta(id);
                var portifolioDAO = new ConjuntoHabitacionalDAO();
                model.NomeConjuntoHabitacional = portifolioDAO.ConsultaConjuntoHabitacionalPorUsuario(id).Nome;
                if (model == null)
                    return RedirectToAction(NomeViewIndex);
                else
                {
                    PreencheDadosParaView("A", model);
                    return View(NomeViewForm, model);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
    }
  
}
