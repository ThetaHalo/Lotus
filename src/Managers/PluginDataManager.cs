using System.IO;
using VentLib.Utilities.Extensions;

namespace TownOfHost.Managers;

public class PluginDataManager
{
    public const string DataDirectory = "./TOHTOR_DATA";
    public const string TemplateFile = "Template.txt";
    public const string WordListFile = "BannedWords.txt";

    private DirectoryInfo dataDirectory;
    public TemplateManager2 TemplateManager;
    public ChatManager ChatManager;

    public PluginDataManager()
    {
        dataDirectory = new DirectoryInfo(DataDirectory);
        if (!dataDirectory.Exists) dataDirectory.Create();
        TemplateManager = new TemplateManager2(dataDirectory.GetFile(TemplateFile));
        ChatManager = new ChatManager(dataDirectory.GetFile(WordListFile));
    }
}