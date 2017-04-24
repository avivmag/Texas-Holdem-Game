namespace Backend
{
	public class ReturnMessage
	{
        public bool success { get; set; }
        public string description { get; set; }

        public ReturnMessage (bool success, string description)
        {
            this.success = success;
            this.description = description;
        }
        
        public ReturnMessage()
        {
            this.success = true;
            this.description = "";
        }
	}
}
