namespace SL
{
	class Message
	{
        public bool success { get; }
        public string description { get; }

        string Message (bool success, string description)
        {
            this.success = success;
            this.description = description;
        }   
	}
}
