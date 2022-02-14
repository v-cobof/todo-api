using MyTodoApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MyTodoApp.Data
{
    public class AppDbContext : DbContext
    {   
        public DbSet<Todo> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql(connectionString:"Host=localhost; Port=5432; Database=TodoApp;Username=postgres;Password=1234567");
        }
    }
}