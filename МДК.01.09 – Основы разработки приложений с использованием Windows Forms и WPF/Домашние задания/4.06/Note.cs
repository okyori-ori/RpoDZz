using System;
namespace Homework
{
    public class Note
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public override string ToString() { return Title; }
    }
}
