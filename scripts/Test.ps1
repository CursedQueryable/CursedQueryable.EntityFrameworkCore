param([switch]$InMemoryOnly)

$root = $PSScriptRoot -replace '\\', '/' -replace '/$', '' -replace '/scripts$', ''
$artifacts = "$root/artifacts"
$solution = "$root/CursedQueryable.EntityFrameworkCore.sln"

Import-Module "$root/scripts/Exec.psm1"

if(Test-Path "$artifacts/coverage") { Remove-Item "$artifacts/coverage" -Force -Recurse }

exec {
	dotnet restore $solution
}

exec {
	dotnet build $solution `
	-c Debug `
	--no-restore
}

$filter = ""

if($InMemoryOnly) {
	$filter = "FullyQualifiedName!~CursedQueryable.EntityFrameworkCore.IntegrationTests.MariaDb
		&FullyQualifiedName!~CursedQueryable.EntityFrameworkCore.IntegrationTests.MySql
		&FullyQualifiedName!~CursedQueryable.EntityFrameworkCore.IntegrationTests.Oracle
		&FullyQualifiedName!~CursedQueryable.EntityFrameworkCore.IntegrationTests.Postgres
		&FullyQualifiedName!~CursedQueryable.EntityFrameworkCore.IntegrationTests.SqlServer"
}

exec {
	dotnet test $solution `
	/p:CollectCoverage=true `
	/p:CoverletOutput="$artifacts/coverage/" `
	/p:MergeWith="$artifacts/coverage/coverage.json" `
	/p:CoverletOutputFormat="opencover%2cjson" `
	--filter $filter `
	--no-build `
	--verbosity normal
}

exec {
	reportgenerator `
	-sourcedirs:"$root/src/" `
	-targetdir:"$artifacts/coverage_report/" `
	-historydir:"$artifacts/coverage_history/" `
	-reports:"$artifacts/coverage/*.xml" `
	-reporttypes:"Html_Dark"
}