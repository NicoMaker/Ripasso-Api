using Dapper;
using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace API_Ripasso2;

public class Acesso_Dati
{
    private readonly string _stringaDiConnessione;
    public Acesso_Dati(IConfiguration configuration, ILogger<Acesso_Dati> Logger)
    {
        _stringaDiConnessione = configuration.GetConnectionString("testDB");
    }

    public List<Prodotti> GetProducts()
    {
        List<Prodotti> prodotti = new List<Prodotti>();
        using var Connessione = new NpgsqlConnection(_stringaDiConnessione);

        Connessione.Open();

        string query = " SELECT * FROM prodotti;";

        using var comando = new NpgsqlCommand(query, Connessione);

        using var reader = comando.ExecuteReader();

        while (reader.Read())
        {
            var p = new Prodotti();

            //p.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            //p.Nome = reader.GetString(reader.GetOrdinal("Nome"));
            //p.Prezzo = reader.GetDecimal(reader.GetOrdinal("Prezzo"));

            p.Id = (int)reader["Id"];
            p.Prezzo = (decimal)reader["Prezzo"];
            p.Descrizione = reader["Descrizione"] as string;

            if (reader["Data"] != null)
                p.Data = (DateTime)reader["Data"];

            p.Id_Categoria = (int)reader["Id_Categoria"];
            p.Conto_Visite = (int)reader["Conto_Visite"];
            p.Visibile = (bool)reader["Visibile"];

            prodotti.Add(p);
        }

        return prodotti;
    }


    public IEnumerable<Prodotti> GetProductsDapper()
    {
        using var Connessione = new NpgsqlConnection(_stringaDiConnessione);

        string query = " SELECT * FROM prodotti;";

        return Connessione.Query<Prodotti>(query);
    }

    public Prodotti GetProdotto(int Id)
    {
        using var Connessione = new NpgsqlConnection(_stringaDiConnessione);

        Connessione.Open();

        string query = " SELECT * FROM prodotti WHERE Id = @Id;";

        using var comando = Connessione.CreateCommand();

        comando.CommandText = query;

        comando.Parameters.AddWithValue("@Id", Id);
        using var reader = comando.ExecuteReader();

        if (reader.Read())
        {
            var p = new Prodotti();

            p.Id = (int)reader["Id"];
            p.Prezzo = (decimal)reader["Prezzo"];
            p.Descrizione = reader["Descrizione"] as string;

            if (reader["Data"] != null)
                p.Data = (DateTime)reader["Data"];

            p.Id_Categoria = (int)reader["Id_Categoria"];
            p.Conto_Visite = (int)reader["Conto_Visite"];
            p.Visibile = (bool)reader["Visibile"];

            return p;
        }

        return null;
    }

    public Prodotti GetProdottoDapper(int Id)
    {
        using var Connessione = new NpgsqlConnection(_stringaDiConnessione);

        string query = " SELECT * FROM prodotti WHERE Id = @Id;";

        return Connessione.QueryFirstOrDefault<Prodotti>(query, new
        {
            Id = Id
        });
    }

    public void AggiornaProdotto(Prodotti prodotto)
    {
        using var Connessione = new NpgsqlConnection(_stringaDiConnessione);
        Connessione.Open();

        var query = """
            UPDDATE prodotti SET 
                Nome = @Nome
                Prezzo = @Prezzo
                Descrizione = @Descrizione
                Data = @Data
                Id_Categoria = @Id_Categooria
                Conto_Visite = @Conto_Visite
                Visibile = @Visibile

            WHERE 

                Id = @Id
            """;

        using var Comando = new NpgsqlCommand(query, Connessione);

        Comando.Parameters.AddWithValue("@Nome", prodotto.Nome);
        Comando.Parameters.AddWithValue("@Prezzo", prodotto.Prezzo);
        Comando.Parameters.AddWithValue("@Descrizione", prodotto.Descrizione);
        Comando.Parameters.AddWithValue("@Data", prodotto.Data);
        Comando.Parameters.AddWithValue("@Id_Categoria", prodotto.Id_Categoria);
        Comando.Parameters.AddWithValue("@Conta_Visite", prodotto.Conto_Visite);
        Comando.Parameters.AddWithValue("@Visibile", prodotto.Visibile);

        Comando.ExecuteNonQuery();
    }

    public void AggiornaProdottodapper(Prodotti prodotto)
    {
        using var Connessione = new NpgsqlConnection(_stringaDiConnessione);

        var query = """
            UPDDATE prodotti SET 
                Nome = @Nome
                Prezzo = @Prezzo
                Descrizione = @Descrizione
                Data = @Data
                Id_Categoria = @Id_Categooria
                Conto_Visite = @Conto_Visite
                Visibile = @Visibile

            WHERE 

                Id = @Id
            """;

        Connessione.Execute(query, prodotto);
    }

    public void InserisciProdotto(Prodotti prodotto)
    {
        using var Connessione = new NpgsqlConnection(_stringaDiConnessione);
        Connessione.Open();

        var query = """
            INSERT INTO prodotti
                (Nome,
                Prezzo,
                Descrizione,
                Data,
                Id_Categoria,
                Conto_Visite,
                Visibile)

                VALUES 

                (@Nome,
                @Prezzo,
                @Descrizione,
                @Data,
                @Id_Categooria,
                @Conto_Visite,
                @Visibile)
            """;

        using var comando = new NpgsqlCommand(query, Connessione);

        comando.Parameters.AddWithValue("@Nome", prodotto.Nome);
        comando.Parameters.AddWithValue("@Prezzo", prodotto.Prezzo);
        comando.Parameters.AddWithValue("@Descrizione", prodotto.Descrizione ?? (object)DBNull.Value);
        comando.Parameters.AddWithValue("@Data", prodotto.Data);
        comando.Parameters.AddWithValue("@Id_Categooria", prodotto.Id_Categoria);
        comando.Parameters.AddWithValue("@Conto_Visite", prodotto.Conto_Visite);
        comando.Parameters.AddWithValue("@Visibile", prodotto.Visibile);

        comando.ExecuteNonQuery();
    }

    public void InserisciProdottoDapper(Prodotti prodotto)
    {
        using var Connessione = new NpgsqlConnection(_stringaDiConnessione);

        var query = """
            INSERT INTO prodotti
                (Nome,
                Prezzo,
                Descrizione,
                Data,
                Id_Categoria,
                Conto_Visite,
                Visibile)

                VALUES 

                (@Nome,
                @Prezzo,
                @Descrizione,
                @Data,
                @Id_Categooria,
                @Conto_Visite,
                @Visibile)
            """;

        Connessione.Execute(query, prodotto);
    }

    public void CancellaProdotto(int prodotto_id)
    {
        try
        {
            using var Connessione = new NpgsqlConnection(_stringaDiConnessione);
            Connessione.Open();

            var query = """
                DELETE FROM prodotti
                WHERE Id = @Id
                """;

            using var comando = new NpgsqlCommand(query, Connessione);

            comando.Parameters.AddWithValue("@Id", prodotto_id);
            comando.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            throw;
        }
    }

    public void CancellaProdottodapper(int prodotto_id)
    {
        try
        {
            using var Connessione = new NpgsqlConnection(_stringaDiConnessione);

            var query = """
                DELETE FROM prodotti
                WHERE Id = @Id
                """;

            Connessione.Execute(query, new
            {
                prodotto_id
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            throw;
        }
    }
}