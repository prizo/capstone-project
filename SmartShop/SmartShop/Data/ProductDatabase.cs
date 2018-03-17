using SmartShop.Model;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartShop.Data
{
    public class ProductDatabase
    {
        readonly SQLiteAsyncConnection database;

        public ProductDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Product>().Wait();
        }

        public Task<List<Product>> GetProductsAsync()
        {
            return database.Table<Product>().ToListAsync();
        }

        public Task<Product> GetProductAsync(int id)
        {
            return database.Table<Product>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveProductAsync(Product product)
        {
            if (product.ID != 0)
            {
                return database.UpdateAsync(product);
            }
            else
            {
                return database.InsertAsync(product);
            }
        }

        public Task<int> DeleteProductAsync(Product product)
        {
            return database.DeleteAsync(product);
        }
    }
}
