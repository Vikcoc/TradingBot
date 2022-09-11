using WebSocketFlow.DtoInterfaces;
using WebSocketFlow.Extra;

namespace WebSocketFlow.Dto
{
    public class HeartbeatResponseDto : IResponseDto
    {
        public static string ClassMethod => Methods.HeartbeatResponse;
        public long Id { get; set; }
        public string Method { get; set; } = "";
    }
}
