dotnet-ef migrations add Initialize_$(Get-Date -Format "ddMMyyyHHmmss") --project src/Hackathon.Infrastructure --startup-project src/Hackathon.API --output-dir Persistence/Migrations