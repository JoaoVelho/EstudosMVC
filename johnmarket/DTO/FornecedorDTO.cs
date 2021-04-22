using System.ComponentModel.DataAnnotations;

namespace johnmarket.DTO
{
    public class FornecedorDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage="Nome de fornecedor é obrigatório!")]
        [StringLength(100, ErrorMessage="Nome de fornecedor muito grande!")]
        [MinLength(2, ErrorMessage="Nome de fornecedor muito pequeno!")]
        public string Nome { get; set; }

        [Required(ErrorMessage="E-mail de fornecedor é obrigatório!")]
        [EmailAddress(ErrorMessage="E-mail inválido!")]
        public string Email { get; set; }

        [Required(ErrorMessage="Número de telefone de fornecedor é obrigatório!")]
        [Phone(ErrorMessage="Número de telefone inválido!")]
        public string Telefone { get; set; }
    }
}