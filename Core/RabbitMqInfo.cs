namespace Core
{
    /// <summary>
    /// Models configuration data about a RabbitMQ cluster
    /// </summary>
    public class RabbitMqInfo
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
