using WebSocketFlow.DtoInterfaces;
using WebSocketFlow.Extra;

namespace WebSocketFlow.Dto.Responses
{
    public class HeartbeatResponseDto : IResponseDto, IStandardResponse
    {
        public static bool CanJson(string json) => json.Contains(Methods.HeartbeatResponse);

        public long Id { get; set; }
        public string Method { get; set; } = Methods.HeartbeatResponse;
    }
}
