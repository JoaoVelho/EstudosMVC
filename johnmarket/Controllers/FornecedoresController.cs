using System.Linq;
using johnmarket.Data;
using johnmarket.DTO;
using johnmarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace johnmarket.Controllers
{
    public class FornecedoresController : Controller
    {
        private readonly ApplicationDbContext _database;

        public FornecedoresController(ApplicationDbContext database) {
            _database = database;
        }

        [HttpPost]
        public IActionResult Salvar(FornecedorDTO tempFornecedor) {
            if (ModelState.IsValid) {
                Fornecedor fornecedor = new Fornecedor();
                fornecedor.Nome = tempFornecedor.Nome;
                fornecedor.Email = tempFornecedor.Email;
                fornecedor.Telefone = tempFornecedor.Telefone;
                fornecedor.Status = true;
                _database.Fornecedores.Add(fornecedor);
                _database.SaveChanges();
                return RedirectToAction("Fornecedores", "Gestao");
            } else {
                return View("../Gestao/NovoFornecedor");
            }
        }

        [HttpPost]
        public IActionResult Atualizar(FornecedorDTO tempFornecedor) {
            if (ModelState.IsValid) {
                var fornecedor = _database.Fornecedores.First(forn => forn.Id == tempFornecedor.Id);
                fornecedor.Nome = tempFornecedor.Nome;
                fornecedor.Email = tempFornecedor.Email;
                fornecedor.Telefone = tempFornecedor.Telefone;
                _database.SaveChanges();
                return RedirectToAction("Fornecedores", "Gestao");
            } else {
                return View("../Gestao/EditarFornecedor");
            }
        }

        [HttpPost]
        public IActionResult Deletar(int id) {
            if (id > 0) {
                var fornecedor = _database.Fornecedores.First(forn => forn.Id == id);
                fornecedor.Status = false;
                _database.SaveChanges();
            }
            return RedirectToAction("Fornecedores", "Gestao");
        }
    }
}