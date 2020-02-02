dotnet restore
dotnet test /p:CollectCoverage=true /p:Exclude="[xunit.*]" /p:CoverletOutput="..\TestReports\coverage.cobertura.xml" /p:MergeWith="..\TestReports\coverage.cobertura.xml" /p:CoverletOutputFormat=cobertura
tools\reportgenerator.exe "-reports:TestReports\coverage.cobertura.xml" "-targetdir:TestReports" -reporttypes:HTML;HTMLSummary "-assemblyfilters:-Excluded ExpenseTracker.Language" "-classfilters:-Excluded ExpenseTracker.Business.Bootstrap.StartupExtension"
start TestReports\index.htm