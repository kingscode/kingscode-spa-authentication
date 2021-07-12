namespace Nl.KingsCode.SpaAuthentication.Interfaces
{
    public interface IUser
    {
        public long Id { get; }
        string Email { get; set; }
        string Password { get; set; }
        string Name { get; set; }
    }
}