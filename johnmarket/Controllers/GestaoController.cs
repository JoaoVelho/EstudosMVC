using System.Linq;
using johnmarket.Data;
using johnmarket.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace johnmarket.Controllers
{
    [Authorize]
    public class GestaoController : Controller
    {
        private readonly ApplicationDbContext _database;

        public GestaoController(ApplicationDbContext database) {
            _database = database;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Categorias() {
            var categorias = _database.Categorias.Where(cat => cat.Status == true).ToList();
            return View(categorias);
        }

        public IActionResult NovaCategoria() {
            return View();
        }

        public IActionResult EditarCategoria(int id) {
            var categoria = _database.Categorias.First(cat => cat.Id == id);
            CategoriaDTO categoriaView = new CategoriaDTO();
            categoriaView.Id = categoria.Id;
            categoriaView.Nome = categoria.Nome;
            return View(categoriaView);
        }

        public IActionResult Fornecedores() {
            var fornecedores = _database.Fornecedores.Where(forn => forn.Status == true).ToList();
            return View(fornecedores);
        }

        public IActionResult NovoFornecedor() {
            return View();
        }

        public IActionResult EditarFornecedor(int id) {
            var fornecedor = _database.Fornecedores.First(forn => forn.Id == id);
            FornecedorDTO fornecedorView = new FornecedorDTO();
            fornecedorView.Id = fornecedor.Id;
            fornecedorView.Nome = fornecedor.Nome;
            fornecedorView.Email = fornecedor.Email;
            fornecedorView.Telefone = fornecedor.Telefone;
            return View(fornecedorView);
        }

        public IActionResult Produtos() {
            var produtos = _database.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Fornecedor)
                .Where(p => p.Status == true)
                .ToList();
            return View(produtos);
        }

        public IActionResult NovoProduto() {
            ViewBag.Categorias = _database.Categorias.ToList();
            ViewBag.Fornecedores = _database.Fornecedores.ToList();
            return View();
        }

        public IActionResult EditarProduto(int id) {
            var produto = _database.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Fornecedor)
                .First(p => p.Id == id);
            ProdutoDTO produtoView = new ProdutoDTO();
            produtoView.Id = produto.Id;
            produtoView.Nome = produto.Nome;
            produtoView.CategoriaId = produto.Categoria.Id;
            produtoView.FornecedorId = produto.Fornecedor.Id;
            produtoView.PrecoDeCusto = produto.PrecoDeCusto;
            produtoView.PrecoDeVenda = produto.PrecoDeVenda;
            produtoView.Medicao = produto.Medicao;

            ViewBag.Categorias = _database.Categorias.ToList();
            ViewBag.Fornecedores = _database.Fornecedores.ToList();
            return View(produtoView);
        }

        public IActionResult Promocoes() {
            var promocoes = _database.Promocoes
                .Include(prom => prom.Produto)
                .Where(prom => prom.Status == true)
                .ToList();
            return View(promocoes);
        }

        public IActionResult NovaPromocao() {
            ViewBag.Produtos = _database.Produtos.ToList();
            return View();
        }

        public IActionResult EditarPromocao(int id) {
            var promocao = _database.Promocoes
                .Include(prom => prom.Produto)
                .First(prom => prom.Id == id);
            PromocaoDTO promocaoView = new PromocaoDTO();
            promocaoView.Id = promocao.Id;
            promocaoView.Nome = promocao.Nome;
            promocaoView.ProdutoId = promocao.Produto.Id;
            promocaoView.Porcentagem = promocao.Porcentagem;

            ViewBag.Produtos = _database.Produtos.ToList();
            return View(promocaoView);
        }

        public IActionResult Estoque() {
            var listaDeEstoque = _database.Estoques.Include(e => e.Produto).ToList();
            return View(listaDeEstoque);
        }

        public IActionResult NovoEstoque() {
            ViewBag.Produtos = _database.Produtos.ToList();
            return View();
        }

        public IActionResult EditarEstoque(int id) {
            var estoque = _database.Estoques
                .Include(e => e.Produto)
                .First(e => e.Id == id);

            ViewBag.Produtos = _database.Produtos.ToList();
            return View(estoque);
        }

        public IActionResult Vendas() {
            var listaDeVendas = _database.Vendas.ToList();
            return View(listaDeVendas);
        }

        [HttpPost]
        public IActionResult RelatorioDeVendas() {
            return Ok(_database.Vendas.ToList());
        }
    }
}