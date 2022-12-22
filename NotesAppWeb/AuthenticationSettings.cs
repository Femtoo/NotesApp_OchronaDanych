namespace NotesAppWeb
{
    public static class AuthenticationSettings
    {
        public static string JwtKey { get; set; } = "sauidhufihaiuhfuidshfuihdfuisdhiugdsuighsuidhfuisdhuisdhufigsdhuighsduihgui";
        public static string Issuer { get; set; } = "https://localhost:7236;http://localhost:5074";
        public static string Audience { get; set; } = "https://localhost:7236;http://localhost:5074";
        public static double JwtExpiresDays { get; set; } = 1;
    }
}
