using AquaSense.DAO;
using AquaSense.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AquaSense.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }
        public IActionResult Login(LoginViewModel model)
        {
            ValidaDados(model);
            if (ModelState.IsValid == false)
            {
                return View("Index", model);
            }
            else
            {
                UsuarioDAO usuarioDAO = new UsuarioDAO();
                UsuarioViewModel user = usuarioDAO.ConsultaAcesso(model.Login, model.Senha);

                if (user != null)
                {
                    var json = JsonConvert.SerializeObject(user);
                    HttpContext.Session.SetString("Usuario", json);
                    HttpContext.Session.SetString("UsuarioName", json);
                    HttpContext.Session.SetString("UsuarioFoto", json);
                    HttpContext.Session.SetString("Logado", "true");
                    return RedirectToAction("index", "Home");
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Senha", "Usuário ou senha inválidos!");
                    return View("Index", model);
                }
            }
        }
        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        protected void ValidaDados(LoginViewModel model)
        {
            ModelState.Clear();
            if (model.Login == null)
                ModelState.AddModelError("Login", "Preencha esse campo");
            if (model.Senha == null)
                ModelState.AddModelError("Senha", "Preencha esse campo");

        }

        public virtual IActionResult Create()
        {
            try
            {
                ViewBag.Operacao = "I";
                UsuarioViewModel model = Activator.CreateInstance<UsuarioViewModel>();
                return View("Form", model);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public virtual IActionResult Save(UsuarioViewModel model, string Operacao)
        {
            try
            {
                UsuarioDAO usuarioDao = new UsuarioDAO();
                ValidaDados(model, Operacao);
                if (ModelState.IsValid == false)
                {
                    return View("Form", model);
                }
                else
                {
                    usuarioDao.Insert(model);

                    return RedirectToAction("Index", "Login");
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

        protected void ValidaDados(UsuarioViewModel model, string operacao)
        {
            if (string.IsNullOrEmpty(model.NomePessoa))
                ModelState.AddModelError("NomePessoa", "Preencha o nome.");
            if (string.IsNullOrEmpty(model.NomeConjuntoHabitacional))
                ModelState.AddModelError("NomeConjuntoHabitacional", "Preencha o portifolio.");
            if (string.IsNullOrEmpty(model.Senha))
                ModelState.AddModelError("senha", "Preencha a senha.");
            if (model != null && !string.IsNullOrEmpty(model.Senha) && model.Senha.Length < 4)
                ModelState.AddModelError("senha", "A senha tem que conter pelo menos 4 caracteres");
            //Imagem será obrigatio apenas na inclusão.
            //Na alteração iremos considerar a que já estava salva.
            if (model.Imagem == null && operacao == "I")
                ModelState.AddModelError("Imagem", "Escolha uma imagem.");
            if (model.Imagem != null && model.Imagem.Length / 1024 / 1024 >= 2)
                ModelState.AddModelError("Imagem", "Imagem limitada a 2 mb.");

            model.ImagemEmByte = ConvertImageToByte(model.Imagem);
        }
    }
}
