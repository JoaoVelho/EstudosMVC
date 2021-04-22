using System.Linq;
using johnmarket.Data;
using johnmarket.DTO;
using johnmarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace johnmarket.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _database;

        public CategoriasController(ApplicationDbContext database) {
            _database = database;
        }

        [HttpPost]
        public IActionResult Salvar(CategoriaDTO tempCategoria) {
            if (ModelState.IsValid) {
                Categoria categoria = new Categoria();
                categoria.Nome = tempCategoria.Nome;
                categoria.Status = true;
                _database.Categorias.Add(categoria);
                _database.SaveChanges();
                return RedirectToAction("Categorias", "Gestao");
            } else {
                return View("../Gestao/NovaCategoria");
            }
        }

        [HttpPost]
        public IActionResult Atualizar(CategoriaDTO tempCategoria) {
            if (ModelState.IsValid) {
                var categoria = _database.Categorias.First(cat => cat.Id == tempCategoria.Id);
                categoria.Nome = tempCategoria.Nome;
                _database.SaveChanges();
                return RedirectToAction("Categorias", "Gestao");
            } else {
                return View("../Gestao/EditarCategoria");
            }
        }

        [HttpPost]
        public IActionResult Deletar(int id) {
            if (id > 0) {
                var categoria = _database.Categorias.First(cat => cat.Id == id);
                categoria.Status = false;
                _database.SaveChanges();
            }
            return RedirectToAction("Categorias", "Gestao");
        }
    }
}