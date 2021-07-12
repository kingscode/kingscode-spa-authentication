namespace NL.Kingscode.Flok.Storage.Api.Responses
{
    public sealed class DataResponse<TData>
        where TData : class
    {
        public DataResponse(TData data)
        {
            Data = data;
        }

        public TData Data { get; }
    }
}