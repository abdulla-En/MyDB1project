using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDBproject.Models
{
    public class ProgramModel
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int InstructorId { get; set; }
        public string Difficulty { get; set; }
        public decimal Fee { get; set; }

    }
}
