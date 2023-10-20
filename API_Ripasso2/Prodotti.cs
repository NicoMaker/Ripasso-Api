namespace API_Ripasso2;

public class Prodotti
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Prezzo { get; set; }
    public string? Descrizione { get; set; }
    public DateTime Data { get; set; }
    public int Id_Categoria { get; set; }
    public int Conto_Visite { get; set; }
    public bool Visibile { get; set; }
}
