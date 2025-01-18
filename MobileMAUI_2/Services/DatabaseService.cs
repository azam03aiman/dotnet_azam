using SQLite;
using System.IO;
using MobileMAUI_2.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileMAUI_2.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "friends_places_food.db3");
            _database = new SQLiteAsyncConnection(databasePath);

            // Create tables if they don't exist
            _database.CreateTableAsync<Friend>().Wait();
            _database.CreateTableAsync<Places>().Wait();
            _database.CreateTableAsync<Food>().Wait();
        }

        // ========== FRIEND OPERATIONS ==========
        // Add a Friend
        public Task<int> AddFriendAsync(Friend friend)
        {
            return _database.InsertAsync(friend);
        }

        // Get All Friends
        public Task<List<Friend>> GetAllFriendsAsync()
        {
            return _database.Table<Friend>().ToListAsync();
        }

        // Update a Friend
        public Task<int> UpdateFriendAsync(Friend friend)
        {
            return _database.UpdateAsync(friend);
        }

        // Delete a Friend
        public Task<int> DeleteFriendAsync(Friend friend)
        {
            return _database.DeleteAsync(friend);
        }

        // ========== PLACES OPERATIONS ==========
        // Add a Place
        public Task<int> AddPlaceAsync(Places place)
        {
            return _database.InsertAsync(place);
        }

        // Get All Places
        public Task<List<Places>> GetAllPlacesAsync()
        {
            return _database.Table<Places>().ToListAsync();
        }

        // Update a Place
        public Task<int> UpdatePlaceAsync(Places place)
        {
            return _database.UpdateAsync(place);
        }

        // Delete a Place
        public Task<int> DeletePlaceAsync(Places place)
        {
            return _database.DeleteAsync(place);
        }

        // ========== FOOD OPERATIONS ==========
        // Add a Food
        public Task<int> AddFoodAsync(Food food)
        {
            return _database.InsertAsync(food);
        }

        // Get All Food
        public Task<List<Food>> GetAllFoodAsync()
        {
            return _database.Table<Food>().ToListAsync();
        }

        // Update a Food
        public Task<int> UpdateFoodAsync(Food food)
        {
            return _database.UpdateAsync(food);
        }

        // Delete a Food
        public Task<int> DeleteFoodAsync(Food food)
        {
            return _database.DeleteAsync(food);
        }
    }
}
