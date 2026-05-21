param(
    [Parameter(Mandatory)][int]$Day,
    [Parameter(Mandatory)][int]$Part
)

# create solution subfolder and solution file
$Name = "Day${Day}part${Part}"
New-Item -Type Directory -Path $Name
Set-Location $Name
& dotnet new sln -n $Name

# scaffold: library
New-Item -Type Directory -Path lib
$LibName = "${Name}.Lib"
& dotnet new classlib -lang "F#" -n $LibName -o lib/$LibName

# scaffold: production console app
New-Item -Type Directory -Path app
$AppName = "${Name}.App"
& dotnet new console -lang "F#" -n $AppName -o app/$AppName

# scaffold: unit testers using Expecto
New-Item -Type Directory -Path tests
$TestsName = "${Name}.Tests"
& dotnet new console -lang "F#" -n $TestsName -o tests/$TestsName

# scaffold: solution
& dotnet sln add "lib/$LibName/${LibName}.fsproj"
& dotnet sln add "app/$AppName/${AppName}.fsproj"
& dotnet sln add "tests/$TestsName/${TestsName}.fsproj"
& dotnet add "app/$AppName" reference "lib/$LibName"
& dotnet add "tests/$TestsName" reference "lib/$LibName"
& dotnet add "tests/$TestsName" package Expecto

# boilerplates
Set-Content -Path "app/$AppName/Program.fs" -Value @"
open System
open $LibName  // your library namespace

[<EntryPoint>]
let main argv =
    printfn "Running $Name production app..."
    // Call into library code here
    0
"@

Set-Content -Path "tests/$TestsName/Program.fs" @"
open Expecto

[<EntryPoint>]
let main argv =
    runTestsInAssemblyWithCLIArgs [] argv
"@

Set-Content -Path "tests/$TestsName/Tests.fs" @"
module $TestsName

open Expecto
open $LibName

[<Tests>]
let sample =
    testList "Sample tests" [
        testCase "example" <| fun _ ->
            Expect.equal (1 + 2) 3 "Example Arithmetic"
    ]
"@

# patch test project XML to prepend Tests.fs before Program.fs compile
$testProjPath = "tests/$TestsName/${TestsName}.fsproj"
$resolvedTestProjPath = (Resolve-Path $testProjPath).Path
$projXml = [xml](Get-Content $resolvedTestProjPath -Raw)

$existingTestsNode =
    $projXml.SelectSingleNode(
        "/Project/ItemGroup/Compile[@Include='Tests.fs']"
    )

if (-not $existingTestsNode) {
    $programNode =
        $projXml.SelectSingleNode(
            "/Project/ItemGroup/Compile[@Include='Program.fs']"
        )

    $testsNode = $projXml.CreateElement("Compile")
    [void]$testsNode.SetAttribute("Include", "Tests.fs")

    if ($programNode) {
        [void]$programNode.ParentNode.InsertBefore($testsNode, $programNode)
    }
    else {
        $itemGroup = $projXml.CreateElement("ItemGroup")
        [void]$itemGroup.AppendChild($testsNode)
        [void]$projXml.Project.AppendChild($itemGroup)
    }

    $projXml.Save($resolvedTestProjPath)
}

# Try it; make sure it works
& dotnet build
& dotnet run --project app/$AppName
& dotnet run --project tests/$TestsName -- --sequenced --debug
