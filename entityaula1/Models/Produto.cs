namespace entityaula1.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        // virtual por causa do Lazy Loading
        public virtual Categoria Categoria { get; set; }

        public override string ToString() {
            return $"Id: {this.Id}; Nome: {this.Nome}; Categoria: [{this.Categoria.ToString()}]";
        }
    }
}