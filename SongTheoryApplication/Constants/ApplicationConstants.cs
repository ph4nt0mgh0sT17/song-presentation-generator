namespace SongTheoryApplication.Constants;

public static class ApplicationConstants
{
    public const string SONGS_JSON_FILENAME = "songs.json";
    public const string ERROR_NOTIFICATION_DIALOG_TITLE = "Chyba";

    public static class PresentationCannotBeGeneratedDialog
    {
        public const string TITLE = "Chyba";

        public const string DESCRIPTION =
            "Prezentace písničky nemohla být úspěšně vygenerována. Kontaktujte administrátora.";
    }

    public static class PresentationCannotBeStartedDialog
    {
        public const string TITLE = "Chyba";

        public const string DESCRIPTION =
            "Prezentace nemůže být z neznámých důvodu spuštěna. Prosím spusťte ji manuálně.";
    }

    public static class Logs
    {
        public const string CANNOT_GENERATE_PRESENTATION = "Cannot generate the presentation";
    }
}