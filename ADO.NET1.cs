using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data.SqlClient;
namespace ProjectAdoNet1
{
    public class Visitor
    {
        public int id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public double Balance { get; set; }
        public bool Status { get; set; } = false;
    }
    class adonet1
    {
        static SqlConnection connection;
        static void Main(string[] args)
        {           
            string str = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FitnesClub;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
            connection = new SqlConnection(str);
            connection.Open();
            Visitor me = new Visitor
            {
                FirstName = "Matvey",
                LastName = "Seroshtan",
                Birthday = DateTime.Parse("19.11.2006"),
                Phone = "+380000000000",
                Balance = 100.0
            };
            Visitor change = SelectVisitors()[8];
            change.FirstName = "Андрей";
            change.LastName = "Черный";
            //Insert();
            //Delete(10);
            //Update(change);
            foreach (var v in SelectVisitors())
            {
                Console.WriteLine($"{v.id} {v.FirstName} {v.LastName} {v.Birthday.ToString("dd.MM.yyyy")} {v.Phone} {v.Balance} {v.Status}");
            }
        }
        static List<Visitor> SelectVisitors()
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"SELECT v.Id,v.FirstName,v.LastName,v.BirthDay,v.Phone,v.Balance,v.[Status] FROM dbo.Visitors v", connection);
                List<Visitor> visitors = new List<Visitor>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Visitor v = new Visitor();
                    v.id = reader.GetInt32(0);
                    v.FirstName = reader.GetString(1);
                    v.LastName = reader.GetString(2);
                    v.Birthday = reader.GetDateTime(3);
                    v.Phone = reader.GetString(4);
                    v.Balance = decimal.ToDouble(reader.GetDecimal(5));
                    v.Status = reader.GetBoolean(6);
                    visitors.Add(v);
                }
                reader.Close();
            return visitors;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            return new List<Visitor>();
        }
        static void Insert(Visitor visitor)
        {
            string str = @"INSERT INTO dbo.Visitors(FirstName, LastName,  BirthDay, Phone,Balance,[Status]) VALUES (@FirstName, @LastName,  @BirthDay, @Phone, @Balance, @Status)";
            SqlCommand cmd = new SqlCommand(str, connection);
            cmd.Parameters.Add(new SqlParameter("@FirstName", visitor.FirstName));
            cmd.Parameters.Add(new SqlParameter("@LastName", visitor.LastName));
            cmd.Parameters.Add(new SqlParameter("@BirthDay", visitor.Birthday));
            cmd.Parameters.Add(new SqlParameter("@Phone", visitor.Phone));
            cmd.Parameters.Add(new SqlParameter("@Balance", visitor.Balance));
            cmd.Parameters.Add(new SqlParameter("@Status", visitor.Status));
            cmd.ExecuteNonQuery();
        }
        static void Update(Visitor visitor)
        {
            string str = @"UPDATE dbo.Visitors SET FirstName=@FirstName,LastName=@LastName,BirthDay=@BirthDay,Phone=@Phone,Balance = @Balance,[Status]=@Status WHERE dbo.Visitors.Id=" + visitor.id.ToString();
            SqlCommand cmd = new SqlCommand(str, connection);
            cmd.Parameters.Add(new SqlParameter("@FirstName", visitor.FirstName));
            cmd.Parameters.Add(new SqlParameter("@LastName", visitor.LastName));
            cmd.Parameters.Add(new SqlParameter("@BirthDay", visitor.Birthday));
            cmd.Parameters.Add(new SqlParameter("@Phone", visitor.Phone));
            cmd.Parameters.Add(new SqlParameter("@Balance", visitor.Balance));
            cmd.Parameters.Add(new SqlParameter("@Status", visitor.Status));
            cmd.ExecuteNonQuery();
        }
        static void Delete(int Id)
        {
            string str = @"DELETE from dbo.npuxog WHERE dbo.npuxog.VisitorId = @Id;DELETE from  dbo.SectionVisitors where dbo.SectionVisitors.VisitorId=@Id;DELETE from dbo.Visitors WHERE dbo.Visitors.Id = @Id";
            SqlCommand cmd = new SqlCommand(str, connection);
            cmd.Parameters.Add(new SqlParameter("@Id", Id));
            cmd.ExecuteNonQuery();
        }
        static void Delete(Visitor visitor)
        {
            string str = @"DELETE from dbo.npuxog WHERE dbo.npuxog.VisitorId = @Id;DELETE from  dbo.SectionVisitors where dbo.SectionVisitors.VisitorId=@Id;DELETE from dbo.Visitors WHERE dbo.Visitors.Id = @Id";
            SqlCommand cmd = new SqlCommand(str, connection);
            cmd.Parameters.Add(new SqlParameter("@Id", visitor.id));
            cmd.ExecuteNonQuery();
        }
    }
}
