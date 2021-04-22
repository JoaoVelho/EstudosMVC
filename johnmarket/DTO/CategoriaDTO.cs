using System.ComponentModel.DataAnnotations;

namespace johnmarket.DTO
{
    public class CategoriaDTO
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage="Nome da categoria é obrigatório!")]
        [StringLength(100, ErrorMessage="Nome de categoria muito grande!")]
        [MinLength(2, ErrorMessage="Nome de categoria muito pequeno!")]
        public string Nome { get; set; }
    }
}