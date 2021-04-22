using System.Linq;
using johnmarket.Data;
using johnmarket.DTO;
using johnmarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace johnmarket.Controllers
{
    public class PromocoesController : Controller
    {
        private readonly ApplicationDbContext _database;

        public PromocoesController(ApplicationDbContext database) {
            _database = database;
        }

        [HttpPost]
        public IActionResult Salvar(PromocaoDTO tempPromocao) {
            if (ModelState.IsValid) {
                Promocao promocao = new Promocao();
                promocao.Nome = tempPromocao.Nome;
                promocao.Produto = _database.Produtos.First(p => p.Id == tempPromocao.ProdutoId);
                promocao.Porcentagem = tempPromocao.Porcentagem;
                promocao.Status = true;
                _database.Promocoes.Add(promocao);
                _database.SaveChanges();
                return RedirectToAction("Promocoes", "Gestao");
            } else {
                ViewBag.Produtos = _database.Produtos.ToList();
                return View("../Gestao/NovaPromocao");
            }
        }

        [HttpPost]
        public IActionResult Atualizar(PromocaoDTO tempPromocao) {
            if (ModelState.IsValid) {
                var promocao = _database.Promocoes.First(prom => prom.Id == tempPromocao.Id);
                promocao.Nome = tempPromocao.Nome;
                promocao.Produto = _database.Produtos.First(p => p.Id == tempPromocao.ProdutoId);
                promocao.Porcentagem = tempPromocao.Porcentagem;
                _database.SaveChanges();
                return RedirectToAction("Promocoes", "Gestao");
            } else {
                ViewBag.Produtos = _database.Produtos.ToList();
                return View("../Gestao/EditarPromocao");
            }
        }

        [HttpPost]
        public IActionResult Deletar(int id) {
            if (id > 0) {
                var promocao = _database.Promocoes.First(prom => prom.Id == id);
                promocao.Status = false;
                _database.SaveChanges();
            }
            return RedirectToAction("Promocoes", "Gestao");
        }
    }
}