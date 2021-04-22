using System.ComponentModel.DataAnnotations;

namespace johnmarket.DTO
{
    public class PromocaoDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage="Nome da promoção é obrigatório!")]
        [StringLength(100, ErrorMessage="Nome da promoção muito grande!")]
        [MinLength(2, ErrorMessage="Nome da promoção muito pequeno!")]
        public string Nome { get; set; }

        [Required(ErrorMessage="Produto da promoção é obrigatório!")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage="Porcentagem da promoção é obrigatória!")]
        [Range(0, 100, ErrorMessage="Porcentagem inválida!")]
        public float Porcentagem { get; set; }
    }
}