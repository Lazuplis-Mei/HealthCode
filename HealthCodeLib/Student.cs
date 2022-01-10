namespace HealthCodeLib
{
    public class Student
    {
        public int Id { get; }
        public string Name { get; }
        public Student(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override string ToString()
        {
            return $"{Id}.{Name}";
        }
    }
}