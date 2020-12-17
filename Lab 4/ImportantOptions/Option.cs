namespace ImportantOptions
{
    public class Option
    {
        public DirectoryOptions DirectoryOptions { get; set; }
        public ArchivationOptions ArchivationOptions { get; set; }
        public ServiceOptions ServiceOptions { get; set; }

    }

    public class DirectoryOptions
    {
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
    }

    public class ArchivationOptions
    {
        public string ZipName { get; set; }
    }

    public class ServiceOptions
    {

        public bool CanStop { get; set; }// = true;

        public bool CanPauseAndContinue { get; set; }// = true;

        public bool AutoLog { get; set; }// = true;
    }
}