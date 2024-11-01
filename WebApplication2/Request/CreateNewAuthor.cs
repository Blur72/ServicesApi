using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication2.Model;

namespace WebApplication2.Request
{
    public class CreateNewAuthor
    {
        public string LName { get; set; }
        public string FName { get; set; }
    }
}
