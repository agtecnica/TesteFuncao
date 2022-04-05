using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using WebAtividadeEntrevista.Util;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        private List<Beneficiario> _BeneficiarioList
        {
            get
            {
                if (HttpContext.Session["_beneficiarioList"] == null)
                    return new List<Beneficiario>();

                return (List<Beneficiario>)HttpContext.Session["_beneficiarioList"];
            }
            set
            {
                HttpContext.Session["_beneficiarioList"] = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {

            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                var clientes = boCliente.Listar();
                if (clientes.Where(b => Formatacao.RemocePontucaoCpfCnpj(b.CPF).Equals(Formatacao.RemocePontucaoCpfCnpj(model.CPF))).Any())
                {
                    Response.StatusCode = 400;
                    return Json("Cliente já cadastrado!");
                }

                var listaBeneficiarios = new List<Beneficiario>();

                foreach (var beneficiarioModel in _BeneficiarioList)
                {
                    var beneficiario = new Beneficiario()
                    {
                        Id = beneficiarioModel.Id,
                        CPF = Formatacao.RemocePontucaoCpfCnpj(beneficiarioModel.CPF),
                        Nome = beneficiarioModel.Nome,
                        IdCliente = beneficiarioModel.IdCliente
                    };
                    listaBeneficiarios.Add(beneficiario);
                }

                model.Id = boCliente.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    CPF = Formatacao.RemocePontucaoCpfCnpj(model.CPF),
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    ListaBeneficiario = listaBeneficiarios
                });

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                var listaBeneficiarios = new List<Beneficiario>();

                foreach (var beneficiarioModel in _BeneficiarioList)
                {
                    var beneficiario = new Beneficiario()
                    {
                        Id = beneficiarioModel.Id,
                        CPF = Formatacao.RemocePontucaoCpfCnpj(beneficiarioModel.CPF),
                        Nome = beneficiarioModel.Nome,
                        IdCliente = beneficiarioModel.IdCliente == 0 ? model.Id : beneficiarioModel.IdCliente
                    };
                    listaBeneficiarios.Add(beneficiario);
                }

                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    CPF = Formatacao.RemocePontucaoCpfCnpj(model.CPF),
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    ListaBeneficiario = listaBeneficiarios
                });

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    CPF = cliente.CPF,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone
                };

                model.ListBeneficiario = cliente.ListaBeneficiario.Select(b => new BeneficiarioModel()
                {
                    Id = b.Id,
                    Nome = b.Nome,
                    CPF = b.CPF,
                    IdCliente = b.IdCliente,
                }).ToList();
            }

            _BeneficiarioList = cliente.ListaBeneficiario;

            return View(model);
        }

        #region Beneficiário

        public JsonResult BeneficiarioList(int id)
        {
            //Return result to jTable
            return Json(new { Result = "OK", Records = _BeneficiarioList.Where(b => b.IdCliente == id).ToList() });
        }

        [HttpPost]
        public JsonResult IncluirBeneficiario(BeneficiarioModel model)
        {
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                if (!(erros.Contains("O campo Id é obrigatório.") && erros.Count == 1))
                {
                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, erros));
                }
            }

            if (_BeneficiarioList == null)
                _BeneficiarioList = new List<Beneficiario>();

            var listaBeneficiarios = _BeneficiarioList;

            if (listaBeneficiarios.Where(b => Formatacao.RemocePontucaoCpfCnpj(b.CPF).Equals(Formatacao.RemocePontucaoCpfCnpj(model.CPF))).Any())
            {
                Response.StatusCode = 400;
                return Json("Beneficiário já cadastrado!");
            }

            var beneficiario = new Beneficiario()
            {
                Id = model.Id,
                CPF = Formatacao.RemocePontucaoCpfCnpj(model.CPF),
                Nome = model.Nome,
                IdCliente = model.IdCliente
            };

            listaBeneficiarios.Add(beneficiario);
            _BeneficiarioList = listaBeneficiarios;

            return Json(new { Result = "Beneficiário alterado com sucesso", Records = listaBeneficiarios });

        }

        [HttpPost]
        public JsonResult AlterarBeneficiario(BeneficiarioModel model)
        {
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                var listaBeneficiarios = _BeneficiarioList;
                var beneficiario = listaBeneficiarios.Where(b => Formatacao.RemocePontucaoCpfCnpj(b.CPF).Equals(Formatacao.RemocePontucaoCpfCnpj(model.CPF))).FirstOrDefault();

                if (beneficiario != null)
                {
                    beneficiario.CPF = model.CPF;
                    beneficiario.Nome = model.Nome;
                }
                _BeneficiarioList = listaBeneficiarios;

                return Json(new { Result = "Beneficiário alterado com sucesso", Records = listaBeneficiarios });
            }
        }

        [HttpDelete]
        public JsonResult ExcluirBeneficiario(BeneficiarioModel model)
        {
            BoBeneficiario boBeneficiario = new BoBeneficiario();
            var listaBeneficiario = _BeneficiarioList;
            var beneficiario = listaBeneficiario
                                        .Where(b =>
                                                Formatacao.RemocePontucaoCpfCnpj(b.CPF).Equals(Formatacao.RemocePontucaoCpfCnpj(model.CPF))
                                                && b.Id == model.Id
                                                && b.IdCliente == model.IdCliente)
                                        .FirstOrDefault();

            if (beneficiario != null)
            {
                listaBeneficiario.Remove(beneficiario);
                boBeneficiario.Excluir(beneficiario.Id);
                _BeneficiarioList = listaBeneficiario;
                return Json(new { Result = "Beneficiário removido com sucesso", Records = _BeneficiarioList });
            }
            return Json(new { Result = "Falha ao remover o Beneficiário!", Records = _BeneficiarioList });
        }

        #endregion

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}