namespace be_artwork_sharing_platform.Core.Constancs
{
    public static class RegexConst
    {
        public const string EMAIL = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public const string PHONE_NUMBER = @"^0[0-9]{2,14}$";
        public const string PASSWORD = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z])[a-zA-Z\d\W_]{8,20}$";
        public const string FULL_NAME = @"^[a-zA-Z ]+$";
    }
}
