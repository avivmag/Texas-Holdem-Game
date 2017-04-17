namespace Backend
{
	public class Message
	{
        public bool success { get; set; }
        public string description { get; set; }

        public Message (bool success, string description)
        {
            this.success = success;
            this.description = description;
        }
        
        public Message()
        {
            this.success = true;
            this.description = "";
        }
	}
}
