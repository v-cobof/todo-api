using System.ComponentModel.DataAnnotations;

namespace MyTodoApp.ViewModels
{
    public class CreateTodoViewModel
    {
        [Required]
        public string Title {get; set;}
    }
}