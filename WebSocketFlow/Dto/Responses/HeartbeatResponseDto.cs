using WebSocketFlow.DtoInterfaces;
using WebSocketFlow.Extra;

namespace WebSocketFlow.Dto.Responses
{
    public class HeartbeatResponseDto : IResponseDto
    {
        public static string ClassMethod => Methods.HeartbeatResponse;
        public long Id { get; set; }
        public string Method { get; set; } = ClassMethod;
    }
}
