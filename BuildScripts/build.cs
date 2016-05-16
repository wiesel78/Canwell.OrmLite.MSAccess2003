
var target = Argument("target", "Compress");
var configuration = Argument("configuration", "Release");
var ProjectName = "Canwell.OrmLite";

var Tools = "../Tools";

var MainDir = "../Source";
var MainSln = string.Format("{0}/Canwell.OrmLite.sln", MainDir);

var BuildDir = "../Build";
var SourceToolsDirectory = "../Tools";

var AssemblyInfoPath = string.Format("{0}/GlobalAssemblyInfo.cs", MainDir);

var NugetToolPath = string.Format("{0}/Nuget/nuget.exe", Tools);
var NugetRepository = "D:/NuGetRepository";

string Version = "0.0.0.0";

// Bereinigen
Task("Clean").Does(() =>
	{
		if(!DirectoryExists(BuildDir))
			CreateDirectory(BuildDir);
	
		CleanDirectories(string.Format("{0}/Canwell.*/**/{1}", MainDir, configuration));
		CleanDirectories(string.Format("{0}/", BuildDir));
		DeleteFiles(string.Format("{0}/*/*.nupkg", MainDir));
	});

// Versionsnummer inkrementieren
Task("Set-Version")
	.IsDependentOn("Clean")
	.Does(() => 
	{
		Information("Versionsnummer wird aktualisiert");
		
		// Version aus AssemblyInfo auslesen
		var assemblyInfo = ParseAssemblyInfo( AssemblyInfoPath );
		
		var versionNumbers = assemblyInfo.AssemblyVersion.Split('.');
		
        int mainNumber     = Int32.Parse(versionNumbers[0]);
		int featureNumber  = Int32.Parse(versionNumbers[1]);
		int bugfixNumber   = Int32.Parse(versionNumbers[2]);
		int buildNumber    = Int32.Parse(versionNumbers[3]);
		
		
		// Versionsanpassung nach Argumentwerten
		if(HasArgument("IncMain"))
			mainNumber += Argument<int>("IncMain");
		
		if(HasArgument("IncFeature"))
			featureNumber += Argument<int>("IncFeature");
		
		if(HasArgument("IncBugfix"))
			bugfixNumber += Argument<int>("IncBugfix");
		
		if(HasArgument("IncBuild"))
			buildNumber += Argument<int>("IncBuild");
		else
			buildNumber++;
		

		// versionsstring zusammenbauen mit den neuen Werten
		Version = string.Format("{0}.{1}.{2}.{3}",
			mainNumber, featureNumber, bugfixNumber, buildNumber);
		
		Version = string.Format("{0}.{1}.{2}.{3}",
			versionNumbers[0], versionNumbers[1], versionNumbers[2], buildNumber);


		// beschreiben der AssemblyInfo mit den neuen Versionswerten
		CreateAssemblyInfo( AssemblyInfoPath, new AssemblyInfoSettings {
			Version = Version,
			InformationalVersion = Version,
			FileVersion = Version,
			Company = assemblyInfo.Company,
			Copyright = assemblyInfo.Copyright
		});
		
		Information("Versionsnummer von {0} auf {1} angepasst.", assemblyInfo.AssemblyVersion, Version);
	});

// Nuget Packete herunterladen
Task("Restore-NuGet-Packages")
	.IsDependentOn("Set-Version")
	.Does( context => {
		
		var settings = new NuGetRestoreSettings  {
			ToolPath = new FilePath(NugetToolPath)
		};
		
		NuGetRestore( MainSln, settings );
	})
	.OnError(Exception => {
		Console.WriteLine("");
		Console.WriteLine("NuGet.exe konnte nicht gefunden werden");
		Console.WriteLine("");
	});

// Kompilieren
Task("Build")
	.IsDependentOn("Restore-NuGet-Packages")
	.Does(() => 
	{
		MSBuild(MainSln, s => {
			s.SetConfiguration(configuration);
			s.WithTarget("Clean;Rebuild");
		});

		//StartProcess("clients.bat");
	});

// Commitet die letzten aenderungen und legt einen Tag mit der Version an
Task("Version-Control")
	.IsDependentOn("Build")
	.Does(() =>
	{
		// Git add, commit und tag
		Information("Committen und Taggen dieser Version");

		// fuege alle aenderungen hinzu
		var gitAddAll = StartProcess("git", new ProcessSettings
		{
			Arguments = "add ../ --all"
		});

		// committe alle aenderungen
		var gitCommit = StartProcess("git", new ProcessSettings
		{
			Arguments = string.Format("commit -m \"build version: {0}\" ../ ", Version)
		});

		// Tagge die Version
		var gitTag = StartProcess("git", new ProcessSettings
		{
			Arguments = string.Format("tag -a {0} -m \"{0}\"", Version)
		});

	});
	
Task("Publish-Nuget-Packages")
.IsDependentOn("Version-Control")
.Does(() =>
{
	Information("Nuget Pakete werden veröffentlicht");
	
	var buildDirectoryPath = MakeAbsolute(new DirectoryPath(BuildDir)).FullPath;
	
	var settings = new NuGetPushSettings()
	{
		ToolPath = NugetToolPath
	};
	
	var files = GetFiles(string.Format("{0}/Canwell.*/*.nupkg", MainDir));
	
	foreach(var file in files)
	{
		settings.Source = NugetRepository;
		NuGetPush(file, settings);
		
		settings.Source = buildDirectoryPath;
		NuGetPush(file, settings);
	}
});

RunTarget(target);
