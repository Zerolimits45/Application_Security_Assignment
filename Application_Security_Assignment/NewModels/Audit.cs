namespace Application_Security_Assignment.NewModels
{
	public class Audit
	{
		public int ID { get; set; }
		public string UserID { get; set; }
		public string Action { get; set; } // login, logout
		public DateTime Time { get; set; }

	}
}
