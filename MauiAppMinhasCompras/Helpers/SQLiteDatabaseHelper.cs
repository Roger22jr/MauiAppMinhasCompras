using MauiAppMinhasCompras.Models; // define a utilização da model Produto
using SQLite; // define a utilização do SQLite para manipulação de banco de dados

namespace MauiAppMinhasCompras.Helpers // define o namespace Helpers para organizar as classes auxiliares do projeto
{
    public class SQLiteDatabaseHelper // classe auxiliar para manipulação do banco de dados SQLite
    {
        readonly SQLiteAsyncConnection _conn; // conexão assíncrona com o banco de dados SQLite como campo somente de leitura

        public SQLiteDatabaseHelper(string path) // construtor que recebe o caminho do banco de dados como parâmetro por ser instanciado na classe
        { 
            _conn = new SQLiteAsyncConnection(path); // inicializa a conexão assíncrona com o banco de dados usando o caminho fornecido por ser instanciado na classe
            _conn.CreateTableAsync<Produto>().Wait(); // cria a tabela Produto no banco de dados, se ela não existir com uma instrução de espera para garantir que a tabela seja criada antes de continuar
        }

        public Task<int> Insert(Produto p) // método para inserir um novo produto no banco de dados como uma tarefa que retorna um inteiro representando o número de linhas afetadas
        {
            return _conn.InsertAsync(p); // executa a inserção assíncrona do produto e retorna o resultado da conexão não impedindo a utilização do app durante a execução da tarefa
        }
        
        public Task<List<Produto>> Update(Produto p) // método para atualizar um produto existente no banco de dados como uma tarefa que retorna uma lista de produtos
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?"; // string de consulta SQL para atualizar um produto com base no ID com os marcadores de parâmetros para evitar injeção de SQL

            return _conn.QueryAsync<Produto>(
            sql, p.Descricao, p.Quantidade, p.Preco, p.Id); 
     // executa a consulta SQL de atualização e retorna uma lista de produtos atualizados com os marcadores de parâmetros preenchidos com os valores do produto fornecido com a ordem dos paramentros respeitando a string SQL
        }

        public Task<int> Delete(int id) // método para excluir um produto do banco de dados pelo seu ID como uma tarefa que retorna um inteiro representando o número de linhas afetadas
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id); 
            // executa a exclusão assíncrona do produto com o ID fornecido e retorna o número de linhas afetadas utilizado de uma linha de código lambda para filtrar o produto pelo ID
        }

        public Task<List<Produto>> GetAll() // método para obter todos os produtos do banco de dados como uma tarefa que retorna uma lista de produtos
        {
            return _conn.Table<Produto>().ToListAsync(); // executa a consulta para obter todos os produtos da tabela Produto e retorna uma lista de produtos
        }

        public Task<List<Produto>> Search(string q) // array de objetos como uma tarefa apresentada como lista da model produto
        {
            string sql = "SELECT * Produto WHERE descricao LIKE '%" + q + "%'"; // string de consulta SQL definindo a busca por descrição sobre o parametro q

            return _conn.QueryAsync<Produto>(sql); // executa a consulta SQL e retorna uma lista de produtos da tabela Produto
        }
    }
}
