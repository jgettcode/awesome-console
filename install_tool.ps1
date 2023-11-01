using namespace System.Diagnostics
using namespace System.Xml.Linq

Set-Location AwesomeConsole.Tools.Table

$root = [XElement]::Parse('<root/>')

function Get-LastResultMessage {
    if ($LastExitCode -eq 0) {
        return 'success'
    } else {
        return "error: $LastExitCode"
    }
}

function Add-XmlElement {
    param (
        [string]$command,
        [Stopwatch]$stopwatch
    )

    $stopwatch.Stop()

    $root.Add([XElement]::new("item",
        [XElement]::new("Command", $command),
        [XElement]::new("Time", $stopwatch.Elapsed.TotalSeconds.ToString("0.00's'")),
        [XElement]::new("Result", $(Get-LastResultMessage))))
}

$sw = [Stopwatch]::StartNew()
dotnet build
Add-XmlElement "dotnet build" $sw

$sw = [Stopwatch]::StartNew()
dotnet pack
Add-XmlElement "dotnet pack" $sw

if (dotnet tool list --global | Select-String 'table') {
    $sw = [Stopwatch]::StartNew()
    dotnet tool update --global --configfile .\disable_nuget.config --add-source .\nupkg table
    Add-XmlElement "dotnet tool update" $sw
} else {
    $sw = [Stopwatch]::StartNew()
    dotnet tool install --global --configfile .\disable_nuget.config --add-source .\nupkg table
    Add-XmlElement "dotnet tool install" $sw
}

Set-Location ..

Write-Output $root.ToString([SaveOptions]::DisableFormatting) | table -f simple
Write-Output ''