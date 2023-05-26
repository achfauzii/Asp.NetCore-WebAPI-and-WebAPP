namespace Client.Models
{
   
    public class Employees
    {
        public List<DataItem> data { get; set; }
    }

    public class DataItem
    {
        public string nik { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phone { get; set; }
        public DateTime birthDate { get; set; }
        public int salary { get; set; }
        public string email { get; set; }
        public int gender { get; set; }
        public Department department { get; set; }
    }

    public class Department
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}
