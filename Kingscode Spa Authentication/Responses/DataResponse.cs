namespace Nl.KingsCode.SpaAuthentication.Responses
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