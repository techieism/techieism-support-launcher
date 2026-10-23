$ErrorActionPreference = 'Stop'
$compiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (!(Test-Path -LiteralPath $compiler)) { throw 'Windows .NET Framework compiler required.' }
Push-Location $PSScriptRoot
try {
 New-Item -ItemType Directory -Force out | Out-Null
 & $compiler /nologo /target:winexe /platform:anycpu /optimize+ /reference:System.Windows.Forms.dll '/win32manifest:src\app.manifest' '/out:out\TechieismSupportLauncher.exe' 'src\Program.cs' 'src\SessionClient.cs' 'src\TrustedAssistant.cs'
 if ($LASTEXITCODE -ne 0) { throw 'Launcher build failed.' }
 & $compiler /nologo /target:exe /platform:anycpu '/out:out\Tests.exe' 'tests\SessionClientTests.cs' 'src\SessionClient.cs' 'src\TrustedAssistant.cs'
 if ($LASTEXITCODE -ne 0) { throw 'Test build failed.' }
 & .\out\Tests.exe
 if ($LASTEXITCODE -ne 0) { throw 'Tests failed.' }
 Get-FileHash -LiteralPath out\TechieismSupportLauncher.exe -Algorithm SHA256 | Format-List
} finally { Pop-Location }
