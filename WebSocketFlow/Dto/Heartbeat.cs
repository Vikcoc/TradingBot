using WebSocketFlow.DtoInterfaces;

namespace WebSocketFlow.Dto
{
    public class Heartbeat : IHeartbeat
    {
        public bool IsValid() => Method == "public/heartbeat";

        public long Id { get; set; }
        public string Method { get; set; } = "";
    }
}
