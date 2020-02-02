dotnet restore
dotnet test ExpenseTracker.Business.Tests\ExpenseTracker.Business.Tests.csproj /p:CollectCoverage=true /p:Exclude="[xunit.*]" /p:CoverletOutput="..\TestReports\Coverage\coverage.business.cobertura.xml" /p:CoverletOutputFormat=cobertura
dotnet test ExpenseTracker.UOW.Tests\ExpenseTracker.UOW.Tests.csproj /p:CollectCoverage=true /p:Exclude="[xunit.*]" /p:CoverletOutput="..\TestReports\Coverage\coverage.uow.cobertura.xml" /p:CoverletOutputFormat=cobertura
tools\reportgenerator.exe "-reports:TestReports\Coverage\coverage.business.cobertura.xml;TestReports\Coverage\coverage.uow.cobertura.xml" "-targetdir:TestReports" -reporttypes:HTML;HTMLSummary "-assemblyfilters:-Excluded ExpenseTracker.Language" "-classfilters:-Excluded ExpenseTracker.Business.Bootstrap.StartupExtension"
del TestReports\Coverage\coverage.business.cobertura.xml
del TestReports\Coverage\coverage.uow.cobertura.xml
start TestReports\index.htm