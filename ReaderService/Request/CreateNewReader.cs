using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Request
{

    public class CreateNewReader
    {
        [Required(ErrorMessage = "Имя обязательно")] 
        public string FName { get; set; }
        [Required(ErrorMessage = "Фамилия обязательна")]
        public string LName { get; set; }
        [Required(ErrorMessage = "Дата рождения обязательна")]
        public DateTime DateOfBirth { get; set; }
    }
}
