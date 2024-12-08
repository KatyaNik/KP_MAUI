using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Nikitina_KP.Shared.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
    }

    public class Employer : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SecondName { get; set; }
        public string Post { get; set; }
        public string Education { get; set; }
        public string History { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Age { get; set; }
    }
    public class Message : BaseEntity
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string FromWho { get; set; }
        public string ToWho { get; set; }
    }

}
