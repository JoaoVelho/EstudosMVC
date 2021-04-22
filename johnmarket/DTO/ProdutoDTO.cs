using System.ComponentModel.DataAnnotations;

namespace johnmarket.DTO
{
    public class ProdutoDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage="Nome do produto é obrigatório!")]
        [StringLength(100, ErrorMessage="Nome de produto muito grande!")]
        [MinLength(2, ErrorMessage="Nome de produto muito pequeno!")]
        public string Nome { get; set; }

        [Required(ErrorMessage="Categoria do produto é obrigatória!")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage="Fornecedor do produto é obrigatório!")]
        public int FornecedorId { get; set; }

        [Required(ErrorMessage="Preço de custo do produto é obrigatório!")]
        public float PrecoDeCusto { get; set; }

        [Required(ErrorMessage="Preço de custo do produto é obrigatório!")]
        public string PrecoDeCustoString { get; set; }

        [Required(ErrorMessage="Preço de venda do produto é obrigatório!")]
        public float PrecoDeVenda { get; set; }

        [Required(ErrorMessage="Preço de venda do produto é obrigatório!")]
        public string PrecoDeVendaString { get; set; }

        [Required(ErrorMessage="Medição do produto é obrigatória!")]
        [Range(0, 2, ErrorMessage="Medição inválida!")]
        public int Medicao { get; set; }
    }
}