using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using entityaula1.Models;
using entityaula1.Database;
using Microsoft.EntityFrameworkCore;

namespace entityaula1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext database;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext database)
        {
            _logger = logger;
            this.database = database;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Teste() {
            // Categoria c1 = new Categoria();
            // c1.Nome = "Victor";
            // Categoria c2 = new Categoria();
            // c2.Nome = "Victor";
            // Categoria c3 = new Categoria();
            // c3.Nome = "Erik";
            // Categoria c4 = new Categoria();
            // c4.Nome = "Wesley";

            // List<Categoria> catList = new List<Categoria>() {
            //     c1,
            //     c2,
            //     c3,
            //     c4
            // };

            // database.AddRange(catList);

            // database.SaveChanges();

            List<Categoria> listaDeCategorias = database.Categorias.Where(cat => cat.Nome.Equals("Victor")).ToList();

            listaDeCategorias.ForEach(categoria => {
                Console.WriteLine(categoria.ToString());
            });

            return Content("Dados salvos");
        }

        public IActionResult Relacionamento() {
            // Produto p1 = new Produto();
            // p1.Nome = "Doritos";
            // p1.Categoria = database.Categorias.First(c => c.Id == 5);
            // Produto p2 = new Produto();
            // p2.Nome = "Frango";
            // p2.Categoria = database.Categorias.First(c => c.Id == 5);
            // Produto p3 = new Produto();
            // p3.Nome = "Bolo";
            // p3.Categoria = database.Categorias.First(c => c.Id == 6);

            // database.Add(p1);
            // database.Add(p2);
            // database.Add(p3);

            // database.SaveChanges();

            // List<Produto> listaDeProdutos = database.Produtos.Include(p => p.Categoria).ToList();

            // listaDeProdutos.ForEach(produto => {
            //     Console.WriteLine(produto.ToString());
            // });

            // Sem Include por causa do Lazy Loading
            List<Produto> listaDeProdutosDeUmaCategoria = database.Produtos.Where(p => p.Categoria.Id == 5).ToList();

            listaDeProdutosDeUmaCategoria.ForEach(produto => {
                Console.WriteLine(produto.ToString());
            });

            return Content("Relacionamento");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
