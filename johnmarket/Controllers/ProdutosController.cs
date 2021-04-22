using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using johnmarket.Data;
using johnmarket.DTO;
using johnmarket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace johnmarket.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly ApplicationDbContext _database;

        public ProdutosController(ApplicationDbContext database) {
            _database = database;
        }

        [HttpPost]
        public IActionResult Salvar(ProdutoDTO tempProduto) {
            if (ModelState.IsValid) {
                Produto produto = new Produto();
                produto.Nome = tempProduto.Nome;
                produto.Categoria = _database.Categorias.First(categoria => categoria.Id == tempProduto.CategoriaId);
                produto.Fornecedor = _database.Fornecedores.First(fornecedor => fornecedor.Id == tempProduto.FornecedorId);
                produto.PrecoDeCusto = float.Parse(tempProduto.PrecoDeCustoString, CultureInfo.InvariantCulture.NumberFormat);
                produto.PrecoDeVenda = float.Parse(tempProduto.PrecoDeVendaString, CultureInfo.InvariantCulture.NumberFormat);
                produto.Medicao = tempProduto.Medicao;
                produto.Status = true;
                _database.Produtos.Add(produto);
                _database.SaveChanges();
                return RedirectToAction("Produtos", "Gestao");
            } else {
                ViewBag.Categorias = _database.Categorias.ToList();
                ViewBag.Fornecedores = _database.Fornecedores.ToList();
                return View("../Gestao/NovoProduto");
            }
        }

        [HttpPost]
        public IActionResult Atualizar(ProdutoDTO tempProduto) {
            if (ModelState.IsValid) {
                var produto = _database.Produtos.First(p => p.Id == tempProduto.Id);
                produto.Nome = tempProduto.Nome;
                produto.Categoria = _database.Categorias.First(categoria => categoria.Id == tempProduto.CategoriaId);
                produto.Fornecedor = _database.Fornecedores.First(fornecedor => fornecedor.Id == tempProduto.FornecedorId);
                produto.PrecoDeCusto = float.Parse(tempProduto.PrecoDeCustoString, CultureInfo.InvariantCulture.NumberFormat);;
                produto.PrecoDeVenda = float.Parse(tempProduto.PrecoDeVendaString, CultureInfo.InvariantCulture.NumberFormat);
                produto.Medicao = tempProduto.Medicao;
                _database.SaveChanges();
                return RedirectToAction("Produtos", "Gestao");
            } else {
                ViewBag.Categorias = _database.Categorias.ToList();
                ViewBag.Fornecedores = _database.Fornecedores.ToList();
                return View("../Gestao/EditarProduto");
            }
        }

        [HttpPost]
        public IActionResult Deletar(int id) {
            if (id > 0) {
                var produto = _database.Produtos.First(p => p.Id == id);
                produto.Status = false;
                _database.SaveChanges();
            }
            return RedirectToAction("Produtos", "Gestao");
        }

        [HttpPost]
        public IActionResult Produto(int id) {
            if (id > 0) {
                var produto = _database.Produtos
                    .Include(p => p.Categoria)
                    .Include(p => p.Fornecedor)
                    .Where(p => p.Status == true)
                    .First(p => p.Id == id);

                if (produto != null) {
                    var estoque = _database.Estoques.First(e => e.Produto.Id == produto.Id);
                    if (estoque == null) {
                        produto = null;
                    }
                }

                if (produto != null) {
                    Promocao promocao;
                    try {
                        promocao = _database.Promocoes
                            .First(prom => prom.Produto.Id == produto.Id && prom.Status == true);
                    } catch (Exception) {
                        promocao = null;
                    }
                    
                    if (promocao != null) {
                        produto.PrecoDeVenda -= produto.PrecoDeVenda * (promocao.Porcentagem/100);
                    }

                    Response.StatusCode = 200;
                    return Json(produto);
                } else {
                    Response.StatusCode = 404;
                    return Json(null);
                }
            } else {
                Response.StatusCode = 404;
                return Json(null);
            }
        }

        [HttpPost]
        public IActionResult GerarVenda([FromBody] VendaDTO dados) {
            Venda venda = new Venda();
            venda.Total = dados.total;
            venda.Troco = dados.troco;
            venda.ValorPago = dados.troco <= 0.01f ? dados.total : dados.total + dados.troco;
            venda.Data = DateTime.Now;
            _database.Vendas.Add(venda);
            _database.SaveChanges();

            List<Saida> saidas = new List<Saida>();
            foreach (var saida in dados.produtos) {
                Saida s = new Saida();
                s.Quantidade = saida.quantidade;
                s.ValorDaVenda = saida.subtotal;
                s.Venda = venda;
                s.Produto = _database.Produtos.First(p => p.Id == saida.produto);
                s.Data = DateTime.Now;
                saidas.Add(s);
            }
            _database.Saidas.AddRange(saidas);
            _database.SaveChanges();
            
            return Ok(new { msg = "Venda processada com sucesso!" });
        }

        public class SaidaDTO {
            public int produto { get; set; }
            public int quantidade { get; set; }
            public float subtotal { get; set; }
        }

        public class VendaDTO {
            public float total { get; set; }
            public float troco { get; set; }
            public SaidaDTO[] produtos { get; set; }
        }
    }
}