namespace GerenciarPedidos.Models
{
    public class ResponseModel<T>
    {
        // Tipo de modelo generico para Response
        public T? Dados { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
    }
}
