namespace NotesAppWeb
{
    public static class AuthenticationSettings
    {
        public static string JwtKey { get; set; } = "sauidhufihaiuhfuidshfuihdfuisdhiugdsuighsuidhfuisdhuisdhufigsdhuighsduihgui";
        public static string Issuer { get; set; } = "https://localhost/";
        public static string Audience { get; set; } = "https://localhost/";
        public static double JwtExpiresDays { get; set; } = 1;
    }
}
