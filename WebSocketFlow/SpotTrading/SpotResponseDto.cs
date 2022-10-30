namespace WebSocketFlow.SpotTrading
{
    public class SpotResponseDto<T> : IResultResponse<T> where T : IResponseDto
    {
        public static bool CanJson(string json) => T.CanJson(json);

        public T? Result { get; set; }
    }
}
