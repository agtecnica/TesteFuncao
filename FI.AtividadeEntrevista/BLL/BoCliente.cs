using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            cliente.Id = cli.Incluir(cliente);

            foreach (var beneficiario in cliente.ListaBeneficiario)
            {
                beneficiario.IdCliente = cliente.Id;
                if (beneficiario.Id > 0)
                    boBeneficiario.Alterar(beneficiario);
                else
                    boBeneficiario.Incluir(beneficiario);
            }

            return cliente.Id;
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            cli.Alterar(cliente);

            var listaBeneficiariosDB = boBeneficiario.Listar(cliente.Id);

            var idsBeneficiariosLocal = cliente.ListaBeneficiario.Select(b => b.Id).ToList();
            var idsBeneficiariosDB = listaBeneficiariosDB.Select(b => b.Id).ToList();
            foreach (var beneficiarioDB in cliente.ListaBeneficiario)
            {
                beneficiarioDB.IdCliente = cliente.Id;

                beneficiarioDB.IdCliente = cliente.Id;
                if (beneficiarioDB.Id > 0)
                    boBeneficiario.Alterar(beneficiarioDB);
                else
                    boBeneficiario.Incluir(beneficiarioDB);

            }
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            var clienteDB = cli.Consultar(id);
            clienteDB.ListaBeneficiario = boBeneficiario.Listar(clienteDB.Id);

            return clienteDB;
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF);
        }
    }
}
