using System.Linq;
using johnmarket.Data;
using johnmarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace johnmarket.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly ApplicationDbContext _database;

        public EstoqueController(ApplicationDbContext database) {
            _database = database;
        }

        [HttpPost]
        public IActionResult Salvar(Estoque tempEstoque) {
            _database.Estoques.Add(tempEstoque);
            _database.SaveChanges();
            return RedirectToAction("Estoque", "Gestao");
        }

        [HttpPost]
        public IActionResult Atualizar(Estoque tempEstoque) {
            var estoque = _database.Estoques.First(e => e.Id == tempEstoque.Id);
            estoque.ProdutoId = tempEstoque.ProdutoId;
            estoque.Quantidade = tempEstoque.Quantidade;
            _database.SaveChanges();
            return RedirectToAction("Estoque", "Gestao");
        }
    }
}